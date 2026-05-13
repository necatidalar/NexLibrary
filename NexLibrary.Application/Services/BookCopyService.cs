using Microsoft.EntityFrameworkCore;
using NexLibrary.Application.Interfaces.Repositories;
using NexLibrary.Application.Interfaces.Services;
using NexLibrary.Contracts.BookCopies;
using NexLibrary.Contracts.Common;
using NexLibrary.Domain.Entities;
using NexLibrary.Domain.Enums;

namespace NexLibrary.Application.Services;

public sealed class BookCopyService : IBookCopyService
{
    private readonly IUnitOfWork _unitOfWork;

    public BookCopyService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<List<BookCopyListResponse>>> GetByBookIdAsync(
        int kitapId,
        CancellationToken cancellationToken = default)
    {
        var copies = await _unitOfWork.KitapKopyalari
            .Query()
            .AsNoTracking()
            .Include(x => x.Kitap)
            .Where(x => x.KitapId == kitapId)
            .OrderBy(x => x.Barkod)
            .ToListAsync(cancellationToken);

        return ApiResponse<List<BookCopyListResponse>>.Success(
            copies.Select(MapToResponse).ToList());
    }

    public async Task<ApiResponse<List<BookCopyListResponse>>> GetAvailableByBookIdAsync(
        int kitapId,
        CancellationToken cancellationToken = default)
    {
        var copies = await _unitOfWork.KitapKopyalari
            .Query()
            .AsNoTracking()
            .Include(x => x.Kitap)
            .Where(x =>
                x.KitapId == kitapId &&
                x.AktifMi &&
                x.Durum == KitapKopyaDurumu.Musait)
            .OrderBy(x => x.Barkod)
            .ToListAsync(cancellationToken);

        return ApiResponse<List<BookCopyListResponse>>.Success(
            copies.Select(MapToResponse).ToList());
    }

    public async Task<ApiResponse<List<BookCopyStockSummaryResponse>>> GetStockSummaryAsync(
        CancellationToken cancellationToken = default)
    {
        var books = await _unitOfWork.Kitaplar
            .Query()
            .AsNoTracking()
            .Where(x => x.AktifMi)
            .OrderBy(x => x.KitapAdi)
            .ToListAsync(cancellationToken);

        var copies = await _unitOfWork.KitapKopyalari
            .Query()
            .AsNoTracking()
            .Where(x => x.AktifMi)
            .ToListAsync(cancellationToken);

        var result = books.Select(book =>
        {
            var bookCopies = copies.Where(x => x.KitapId == book.Id).ToList();

            return new BookCopyStockSummaryResponse
            {
                KitapId = book.Id,
                KitapAdi = book.KitapAdi,
                ToplamKopya = bookCopies.Count,
                Musait = bookCopies.Count(x => x.Durum == KitapKopyaDurumu.Musait),
                Oduncte = bookCopies.Count(x => x.Durum == KitapKopyaDurumu.Oduncte),
                Gecikti = bookCopies.Count(x => x.Durum == KitapKopyaDurumu.Gecikti),
                Kayip = bookCopies.Count(x => x.Durum == KitapKopyaDurumu.Kayip),
                Hasarli = bookCopies.Count(x => x.Durum == KitapKopyaDurumu.Hasarli),
                Pasif = copies.Count(x => x.KitapId == book.Id && !x.AktifMi)
            };
        }).ToList();

        return ApiResponse<List<BookCopyStockSummaryResponse>>.Success(result);
    }

    public async Task<ApiResponse<BookCopyListResponse>> CreateAsync(
        BookCopyCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.KitapId <= 0)
        {
            return ApiResponse<BookCopyListResponse>.Fail("Kitap seçilmelidir.");
        }

        if (string.IsNullOrWhiteSpace(request.Barkod))
        {
            return ApiResponse<BookCopyListResponse>.Fail("Barkod zorunludur.");
        }

        var book = await _unitOfWork.Kitaplar.GetByIdAsync(request.KitapId, cancellationToken);

        if (book is null || !book.AktifMi)
        {
            return ApiResponse<BookCopyListResponse>.Fail("Aktif kitap bulunamadı.");
        }

        var normalizedBarcode = request.Barkod.Trim();

        var exists = await _unitOfWork.KitapKopyalari.AnyAsync(
            x => x.Barkod == normalizedBarcode,
            cancellationToken);

        if (exists)
        {
            return ApiResponse<BookCopyListResponse>.Fail("Bu barkod daha önce kullanılmış.");
        }

        var copy = new KitapKopya
        {
            KitapId = request.KitapId,
            Barkod = normalizedBarcode,
            DemirbasNo = string.IsNullOrWhiteSpace(request.DemirbasNo)
                ? null
                : request.DemirbasNo.Trim(),
            Aciklama = string.IsNullOrWhiteSpace(request.Aciklama)
                ? null
                : request.Aciklama.Trim(),
            Durum = KitapKopyaDurumu.Musait,
            AktifMi = true,
            OlusturmaTarihi = DateTime.UtcNow
        };

        await _unitOfWork.KitapKopyalari.AddAsync(copy, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        copy.Kitap = book;

        return ApiResponse<BookCopyListResponse>.Success(
            MapToResponse(copy),
            "Kitap kopyası başarıyla oluşturuldu.");
    }

    private static BookCopyListResponse MapToResponse(KitapKopya copy)
    {
        return new BookCopyListResponse
        {
            Id = copy.Id,
            KitapId = copy.KitapId,
            KitapAdi = copy.Kitap?.KitapAdi ?? string.Empty,
            Barkod = copy.Barkod,
            DemirbasNo = copy.DemirbasNo,
            Durum = copy.Durum.ToString(),
            Aciklama = copy.Aciklama,
            AktifMi = copy.AktifMi
        };
    }
}