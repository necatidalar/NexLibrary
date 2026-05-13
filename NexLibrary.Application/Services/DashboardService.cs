using Microsoft.EntityFrameworkCore;
using NexLibrary.Application.Interfaces.Repositories;
using NexLibrary.Application.Interfaces.Services;
using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Dashboard;
using NexLibrary.Domain.Enums;

namespace NexLibrary.Application.Services;

public sealed class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;

    public DashboardService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<DashboardSummaryResponse>> GetSummaryAsync(
        CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);
        var last7Days = today.AddDays(-7);

        var totalBooks = await _unitOfWork.Kitaplar
            .Query()
            .AsNoTracking()
            .CountAsync(x => x.AktifMi, cancellationToken);

        var totalMembers = await _unitOfWork.Uyeler
            .Query()
            .AsNoTracking()
            .CountAsync(x => x.AktifMi, cancellationToken);

        var activeCopiesQuery = _unitOfWork.KitapKopyalari
            .Query()
            .AsNoTracking()
            .Where(x => x.AktifMi);

        var totalCopies = await activeCopiesQuery.CountAsync(cancellationToken);

        var availableCopies = await activeCopiesQuery
            .CountAsync(x => x.Durum == KitapKopyaDurumu.Musait, cancellationToken);

        var loanedCopies = await activeCopiesQuery
            .CountAsync(x => x.Durum == KitapKopyaDurumu.Oduncte, cancellationToken);

        var overdueCopies = await activeCopiesQuery
            .CountAsync(x => x.Durum == KitapKopyaDurumu.Gecikti, cancellationToken);

        var lostCopies = await activeCopiesQuery
            .CountAsync(x => x.Durum == KitapKopyaDurumu.Kayip, cancellationToken);

        var damagedCopies = await activeCopiesQuery
            .CountAsync(x => x.Durum == KitapKopyaDurumu.Hasarli, cancellationToken);

        var activeLoans = await _unitOfWork.Oduncler
            .Query()
            .AsNoTracking()
            .CountAsync(x =>
                x.AktifMi &&
                (x.Durum == OduncDurumu.Oduncte || x.Durum == OduncDurumu.Gecikti),
                cancellationToken);

        var overdueLoans = await _unitOfWork.Oduncler
            .Query()
            .AsNoTracking()
            .CountAsync(x =>
                x.AktifMi &&
                (
                    x.Durum == OduncDurumu.Gecikti ||
                    (x.Durum == OduncDurumu.Oduncte && x.PlanlananIadeTarihi < today)
                ),
                cancellationToken);

        var returnedToday = await _unitOfWork.Oduncler
            .Query()
            .AsNoTracking()
            .CountAsync(x =>
                x.Durum == OduncDurumu.IadeEdildi &&
                x.IadeTarihi.HasValue &&
                x.IadeTarihi.Value >= today &&
                x.IadeTarihi.Value < tomorrow,
                cancellationToken);

        var last7DaysLoans = await _unitOfWork.Oduncler
            .Query()
            .AsNoTracking()
            .CountAsync(x => x.VerilisTarihi >= last7Days, cancellationToken);

        var last7DaysReturns = await _unitOfWork.Oduncler
            .Query()
            .AsNoTracking()
            .CountAsync(x =>
                x.IadeTarihi.HasValue &&
                x.IadeTarihi.Value >= last7Days,
                cancellationToken);

        var recentLoans = await _unitOfWork.Oduncler
            .Query()
            .AsNoTracking()
            .Include(x => x.Kitap)
            .Include(x => x.KitapKopya)
            .Include(x => x.Uye)
            .OrderByDescending(x => x.Id)
            .Take(10)
            .Select(x => new RecentLoanSummaryResponse
            {
                Id = x.Id,
                KitapAdi = x.Kitap.KitapAdi,
                Barkod = x.KitapKopya != null ? x.KitapKopya.Barkod : null,
                UyeAdiSoyadi = x.Uye.UyeAdiSoyadi,
                VerilisTarihi = x.VerilisTarihi,
                PlanlananIadeTarihi = x.PlanlananIadeTarihi,
                IadeTarihi = x.IadeTarihi,
                Durum = x.Durum.ToString()
            })
            .ToListAsync(cancellationToken);

        var response = new DashboardSummaryResponse
        {
            ToplamKitap = totalBooks,
            ToplamUye = totalMembers,
            ToplamKopya = totalCopies,
            MusaitKopya = availableCopies,
            OdunctekiKopya = loanedCopies,
            GecikenKopya = overdueCopies,
            KayipKopya = lostCopies,
            HasarliKopya = damagedCopies,
            AktifOdunc = activeLoans,
            GecikenOdunc = overdueLoans,
            BugunIadeEdilen = returnedToday,
            Son7GunOdunc = last7DaysLoans,
            Son7GunIade = last7DaysReturns,
            SonOduncler = recentLoans
        };

        return ApiResponse<DashboardSummaryResponse>.Success(response);
    }
}