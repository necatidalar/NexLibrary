using System.Text.Json;
using NexLibrary.Application.Interfaces.Repositories;
using NexLibrary.Application.Interfaces.Services;
using NexLibrary.Domain.Entities;

namespace NexLibrary.Application.Services;

public sealed class AuditLogService : IAuditLogService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = false
    };

    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentRequestInfoService _currentRequestInfoService;

    public AuditLogService(
        IUnitOfWork unitOfWork,
        ICurrentRequestInfoService currentRequestInfoService)
    {
        _unitOfWork = unitOfWork;
        _currentRequestInfoService = currentRequestInfoService;
    }

    public async Task LogAsync(
        string islemTuru,
        string tabloAdi,
        int? kayitId = null,
        object? eskiDeger = null,
        object? yeniDeger = null,
        string? aciklama = null,
        int? kullaniciId = null,
        string? ipAdresi = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(islemTuru))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(tabloAdi))
        {
            return;
        }

        var requestInfo = _currentRequestInfoService.GetCurrent();

        var auditLog = new AuditLog
        {
            IslemTuru = islemTuru.Trim(),
            TabloAdi = tabloAdi.Trim(),
            KayitId = kayitId,
            EskiDegerJson = SerializeOrNull(eskiDeger),
            YeniDegerJson = SerializeOrNull(yeniDeger),
            Aciklama = string.IsNullOrWhiteSpace(aciklama)
                ? null
                : aciklama.Trim(),
            KullaniciId = kullaniciId,
            IpAdresi = string.IsNullOrWhiteSpace(ipAdresi)
                ? requestInfo.IpAdresi
                : ipAdresi.Trim(),
            UserAgent = requestInfo.UserAgent,
            MacAdresi = requestInfo.MacAdresi,
            CihazBilgisi = requestInfo.CihazBilgisi,
            TarayiciBilgisi = requestInfo.TarayiciBilgisi,
            IsletimSistemi = requestInfo.IsletimSistemi,
            Dil = requestInfo.Dil,
            Referer = requestInfo.Referer,
            Host = requestInfo.Host,
            HttpMethod = requestInfo.HttpMethod,
            RequestPath = requestInfo.RequestPath,
            HeaderJson = requestInfo.HeaderJson,
            AktifMi = true,
            OlusturmaTarihi = DateTime.UtcNow
        };

        await _unitOfWork.AuditLoglari.AddAsync(
            auditLog,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static string? SerializeOrNull(object? value)
    {
        return value is null
            ? null
            : JsonSerializer.Serialize(value, JsonOptions);
    }
}