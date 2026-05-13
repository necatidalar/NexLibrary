using Microsoft.EntityFrameworkCore;
using NexLibrary.Application.Interfaces.Repositories;
using NexLibrary.Application.Interfaces.Services;
using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Loans;
using NexLibrary.Domain.Entities;
using NexLibrary.Domain.Enums;

namespace NexLibrary.Application.Services;

public sealed class LoanService : ILoanService
{
    private readonly IUnitOfWork _unitOfWork;

    public LoanService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<PagedResponse<LoanListResponse>>> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 20,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        await MarkOverdueInternalAsync(cancellationToken);

        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 20 : pageSize;

        var query = _unitOfWork.Oduncler
            .Query()
            .AsNoTracking()
            .Include(x => x.Kitap)
            .Include(x => x.KitapKopya)
            .Include(x => x.Uye)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchText = search.Trim();

            query = query.Where(x =>
                x.Kitap.KitapAdi.Contains(searchText) ||
                x.Uye.UyeAdiSoyadi.Contains(searchText) ||
                (x.KitapKopya != null && x.KitapKopya.Barkod.Contains(searchText)) ||
                (x.KitapKopya != null && x.KitapKopya.DemirbasNo != null && x.KitapKopya.DemirbasNo.Contains(searchText)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var loans = await query
            .OrderByDescending(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var response = new PagedResponse<LoanListResponse>
        {
            Items = loans.Select(MapToListResponse).ToList(),
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };

        return ApiResponse<PagedResponse<LoanListResponse>>.Success(response);
    }

    public async Task<ApiResponse<PagedResponse<LoanListResponse>>> GetOverdueAsync(
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        await MarkOverdueInternalAsync(cancellationToken);

        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 20 : pageSize;

        var query = _unitOfWork.Oduncler
            .Query()
            .AsNoTracking()
            .Include(x => x.Kitap)
            .Include(x => x.KitapKopya)
            .Include(x => x.Uye)
            .Where(x => x.Durum == OduncDurumu.Gecikti && x.AktifMi);

        var totalCount = await query.CountAsync(cancellationToken);

        var loans = await query
            .OrderBy(x => x.PlanlananIadeTarihi)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var response = new PagedResponse<LoanListResponse>
        {
            Items = loans.Select(MapToListResponse).ToList(),
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };

        return ApiResponse<PagedResponse<LoanListResponse>>.Success(response);
    }

    public async Task<ApiResponse<LoanDetailResponse>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        await MarkOverdueInternalAsync(cancellationToken);

        var loan = await _unitOfWork.Oduncler
            .Query()
            .AsNoTracking()
            .Include(x => x.Kitap)
            .Include(x => x.KitapKopya)
            .Include(x => x.Uye)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (loan is null)
        {
            return ApiResponse<LoanDetailResponse>.Fail("Ödünç kaydı bulunamadı.");
        }

        return ApiResponse<LoanDetailResponse>.Success(MapToDetailResponse(loan));
    }

    public async Task<ApiResponse<LoanDetailResponse>> CreateAsync(
        LoanCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var errors = await ValidateCreateRequestAsync(request, cancellationToken);

        if (errors.Count > 0)
        {
            return ApiResponse<LoanDetailResponse>.ValidationFail(errors);
        }

        var selectedCopy = await GetAvailableBookCopyAsync(request, cancellationToken);

        if (selectedCopy is null)
        {
            return ApiResponse<LoanDetailResponse>.Fail("Bu kitap için müsait kopya bulunamadı.");
        }

        LoanDetailResponse? createdLoan = null;

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            selectedCopy.Durum = KitapKopyaDurumu.Oduncte;
            selectedCopy.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.KitapKopyalari.Update(selectedCopy);

            var loan = new Odunc
            {
                KitapId = request.KitapId,
                KitapKopyaId = selectedCopy.Id,
                UyeId = request.UyeId,
                VerilisTarihi = DateTime.UtcNow,
                PlanlananIadeTarihi = request.PlanlananIadeTarihi.Date,
                Durum = OduncDurumu.Oduncte,
                Aciklama = string.IsNullOrWhiteSpace(request.Aciklama) ? null : request.Aciklama.Trim(),
                AktifMi = true,
                OlusturmaTarihi = DateTime.UtcNow
            };

            await _unitOfWork.Oduncler.AddAsync(loan, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var detailResult = await GetByIdAsync(loan.Id, cancellationToken);
            createdLoan = detailResult.Veri;
        }, cancellationToken);

        return ApiResponse<LoanDetailResponse>.Success(
            createdLoan!,
            "Kitap başarıyla ödünç verildi.");
    }

    public async Task<ApiResponse<LoanDetailResponse>> ReturnAsync(
        int id,
        LoanReturnRequest request,
        CancellationToken cancellationToken = default)
    {
        var loan = await _unitOfWork.Oduncler
            .Query()
            .Include(x => x.Kitap)
            .Include(x => x.KitapKopya)
            .Include(x => x.Uye)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (loan is null)
        {
            return ApiResponse<LoanDetailResponse>.Fail("Ödünç kaydı bulunamadı.");
        }

        if (loan.Durum == OduncDurumu.IadeEdildi)
        {
            return ApiResponse<LoanDetailResponse>.Fail("Bu kayıt zaten iade edilmiş.");
        }

        if (loan.Durum == OduncDurumu.IptalEdildi)
        {
            return ApiResponse<LoanDetailResponse>.Fail("İptal edilmiş ödünç kaydı iade alınamaz.");
        }

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            loan.IadeTarihi = DateTime.UtcNow;
            loan.Durum = OduncDurumu.IadeEdildi;
            loan.GuncellemeTarihi = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(request.Aciklama))
            {
                loan.Aciklama = string.IsNullOrWhiteSpace(loan.Aciklama)
                    ? request.Aciklama.Trim()
                    : $"{loan.Aciklama} | İade Notu: {request.Aciklama.Trim()}";
            }

            if (loan.KitapKopya is not null)
            {
                loan.KitapKopya.Durum = KitapKopyaDurumu.Musait;
                loan.KitapKopya.GuncellemeTarihi = DateTime.UtcNow;

                _unitOfWork.KitapKopyalari.Update(loan.KitapKopya);
            }

            _unitOfWork.Oduncler.Update(loan);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }, cancellationToken);

        return ApiResponse<LoanDetailResponse>.Success(
            MapToDetailResponse(loan),
            "Kitap başarıyla iade alındı.");
    }

    public async Task<ApiResponse<bool>> CancelAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var loan = await _unitOfWork.Oduncler
            .Query()
            .Include(x => x.KitapKopya)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (loan is null)
        {
            return ApiResponse<bool>.Fail("Ödünç kaydı bulunamadı.");
        }

        if (loan.Durum == OduncDurumu.IadeEdildi)
        {
            return ApiResponse<bool>.Fail("İade edilmiş ödünç kaydı iptal edilemez.");
        }

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            loan.Durum = OduncDurumu.IptalEdildi;
            loan.AktifMi = false;
            loan.GuncellemeTarihi = DateTime.UtcNow;

            if (loan.KitapKopya is not null)
            {
                loan.KitapKopya.Durum = KitapKopyaDurumu.Musait;
                loan.KitapKopya.GuncellemeTarihi = DateTime.UtcNow;

                _unitOfWork.KitapKopyalari.Update(loan.KitapKopya);
            }

            _unitOfWork.Oduncler.Update(loan);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }, cancellationToken);

        return ApiResponse<bool>.Success(true, "Ödünç kaydı iptal edildi.");
    }

    public async Task<ApiResponse<int>> MarkOverdueAsync(
        CancellationToken cancellationToken = default)
    {
        var count = await MarkOverdueInternalAsync(cancellationToken);

        return ApiResponse<int>.Success(count, "Geciken ödünç kayıtları güncellendi.");
    }

    private async Task<int> MarkOverdueInternalAsync(CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;

        var overdueLoans = await _unitOfWork.Oduncler
            .Query()
            .Include(x => x.KitapKopya)
            .Where(x =>
                x.AktifMi &&
                x.Durum == OduncDurumu.Oduncte &&
                x.PlanlananIadeTarihi.Date < today)
            .ToListAsync(cancellationToken);

        if (overdueLoans.Count == 0)
        {
            return 0;
        }

        foreach (var loan in overdueLoans)
        {
            loan.Durum = OduncDurumu.Gecikti;
            loan.GuncellemeTarihi = DateTime.UtcNow;

            if (loan.KitapKopya is not null)
            {
                loan.KitapKopya.Durum = KitapKopyaDurumu.Gecikti;
                loan.KitapKopya.GuncellemeTarihi = DateTime.UtcNow;

                _unitOfWork.KitapKopyalari.Update(loan.KitapKopya);
            }

            _unitOfWork.Oduncler.Update(loan);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return overdueLoans.Count;
    }

    private async Task<List<ValidationError>> ValidateCreateRequestAsync(
        LoanCreateRequest request,
        CancellationToken cancellationToken)
    {
        var errors = new List<ValidationError>();

        if (request.KitapId <= 0)
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.KitapId),
                Message = "Kitap seçilmelidir."
            });
        }

        if (request.UyeId <= 0)
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.UyeId),
                Message = "Üye seçilmelidir."
            });
        }

        if (request.PlanlananIadeTarihi.Date < DateTime.UtcNow.Date)
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.PlanlananIadeTarihi),
                Message = "Planlanan iade tarihi bugünden küçük olamaz."
            });
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        var book = await _unitOfWork.Kitaplar.GetByIdAsync(request.KitapId, cancellationToken);

        if (book is null || !book.AktifMi)
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.KitapId),
                Message = "Aktif kitap bulunamadı."
            });
        }

        var member = await _unitOfWork.Uyeler.GetByIdAsync(request.UyeId, cancellationToken);

        if (member is null || !member.AktifMi)
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.UyeId),
                Message = "Aktif üye bulunamadı."
            });
        }

        if (request.KitapKopyaId.HasValue && request.KitapKopyaId.Value > 0)
        {
            var selectedCopy = await _unitOfWork.KitapKopyalari
                .Query()
                .FirstOrDefaultAsync(x =>
                    x.Id == request.KitapKopyaId.Value &&
                    x.KitapId == request.KitapId,
                    cancellationToken);

            if (selectedCopy is null)
            {
                errors.Add(new ValidationError
                {
                    Field = nameof(request.KitapKopyaId),
                    Message = "Seçilen kitap kopyası bulunamadı."
                });
            }
            else if (!selectedCopy.AktifMi)
            {
                errors.Add(new ValidationError
                {
                    Field = nameof(request.KitapKopyaId),
                    Message = "Seçilen kitap kopyası aktif değil."
                });
            }
            else if (selectedCopy.Durum != KitapKopyaDurumu.Musait)
            {
                errors.Add(new ValidationError
                {
                    Field = nameof(request.KitapKopyaId),
                    Message = "Seçilen kitap kopyası müsait değil."
                });
            }
        }
        else
        {
            var hasAvailableCopy = await _unitOfWork.KitapKopyalari.AnyAsync(
                x => x.KitapId == request.KitapId &&
                     x.AktifMi &&
                     x.Durum == KitapKopyaDurumu.Musait,
                cancellationToken);

            if (!hasAvailableCopy)
            {
                errors.Add(new ValidationError
                {
                    Field = nameof(request.KitapId),
                    Message = "Bu kitap için müsait kopya bulunamadı."
                });
            }
        }

        return errors;
    }

    private async Task<KitapKopya?> GetAvailableBookCopyAsync(
        LoanCreateRequest request,
        CancellationToken cancellationToken)
    {
        if (request.KitapKopyaId.HasValue && request.KitapKopyaId.Value > 0)
        {
            return await _unitOfWork.KitapKopyalari
                .Query()
                .FirstOrDefaultAsync(x =>
                    x.Id == request.KitapKopyaId.Value &&
                    x.KitapId == request.KitapId &&
                    x.AktifMi &&
                    x.Durum == KitapKopyaDurumu.Musait,
                    cancellationToken);
        }

        return await _unitOfWork.KitapKopyalari
            .Query()
            .Where(x =>
                x.KitapId == request.KitapId &&
                x.AktifMi &&
                x.Durum == KitapKopyaDurumu.Musait)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    private static LoanListResponse MapToListResponse(Odunc loan)
    {
        return new LoanListResponse
        {
            Id = loan.Id,
            KitapId = loan.KitapId,
            KitapAdi = loan.Kitap.KitapAdi,
            KitapKopyaId = loan.KitapKopyaId,
            KitapKopyaBarkod = loan.KitapKopya?.Barkod,
            UyeId = loan.UyeId,
            UyeAdiSoyadi = loan.Uye.UyeAdiSoyadi,
            VerilisTarihi = loan.VerilisTarihi,
            PlanlananIadeTarihi = loan.PlanlananIadeTarihi,
            IadeTarihi = loan.IadeTarihi,
            Durum = loan.Durum.ToString(),
            AktifMi = loan.AktifMi
        };
    }

    private static LoanDetailResponse MapToDetailResponse(Odunc loan)
    {
        return new LoanDetailResponse
        {
            Id = loan.Id,
            KitapId = loan.KitapId,
            KitapAdi = loan.Kitap.KitapAdi,
            KitapKopyaId = loan.KitapKopyaId,
            KitapKopyaBarkod = loan.KitapKopya?.Barkod,
            KitapKopyaDemirbasNo = loan.KitapKopya?.DemirbasNo,
            UyeId = loan.UyeId,
            UyeAdiSoyadi = loan.Uye.UyeAdiSoyadi,
            VerilisTarihi = loan.VerilisTarihi,
            PlanlananIadeTarihi = loan.PlanlananIadeTarihi,
            IadeTarihi = loan.IadeTarihi,
            Durum = loan.Durum.ToString(),
            Aciklama = loan.Aciklama,
            AktifMi = loan.AktifMi,
            OlusturmaTarihi = loan.OlusturmaTarihi,
            GuncellemeTarihi = loan.GuncellemeTarihi
        };
    }
}