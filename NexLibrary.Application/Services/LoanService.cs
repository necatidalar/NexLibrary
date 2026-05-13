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
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 20 : pageSize;

        var query = _unitOfWork.Oduncler
            .Query()
            .AsNoTracking()
            .Include(x => x.Kitap)
            .Include(x => x.Uye)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchText = search.Trim();

            query = query.Where(x =>
                x.Kitap.KitapAdi.Contains(searchText) ||
                x.Uye.UyeAdiSoyadi.Contains(searchText));
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

    public async Task<ApiResponse<LoanDetailResponse>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var loan = await _unitOfWork.Oduncler
            .Query()
            .AsNoTracking()
            .Include(x => x.Kitap)
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

        LoanDetailResponse? createdLoan = null;

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var loan = new Odunc
            {
                KitapId = request.KitapId,
                UyeId = request.UyeId,
                VerilisTarihi = DateTime.UtcNow,
                PlanlananIadeTarihi = request.PlanlananIadeTarihi,
                Durum = OduncDurumu.Oduncte,
                Aciklama = request.Aciklama,
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

    public async Task<ApiResponse<bool>> CancelAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var loan = await _unitOfWork.Oduncler.GetByIdAsync(id, cancellationToken);

        if (loan is null)
        {
            return ApiResponse<bool>.Fail("Ödünç kaydı bulunamadı.");
        }

        if (loan.Durum == OduncDurumu.IadeEdildi)
        {
            return ApiResponse<bool>.Fail("İade edilmiş ödünç kaydı iptal edilemez.");
        }

        loan.Durum = OduncDurumu.IptalEdildi;
        loan.AktifMi = false;
        loan.GuncellemeTarihi = DateTime.UtcNow;

        _unitOfWork.Oduncler.Update(loan);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Success(true, "Ödünç kaydı iptal edildi.");
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

        var hasActiveLoan = await _unitOfWork.Oduncler.AnyAsync(
            x => x.KitapId == request.KitapId &&
                 x.AktifMi &&
                 x.Durum == OduncDurumu.Oduncte,
            cancellationToken);

        if (hasActiveLoan)
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.KitapId),
                Message = "Bu kitap zaten ödünçte görünüyor."
            });
        }

        return errors;
    }

    private static LoanListResponse MapToListResponse(Odunc loan)
    {
        return new LoanListResponse
        {
            Id = loan.Id,
            KitapId = loan.KitapId,
            KitapAdi = loan.Kitap.KitapAdi,
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