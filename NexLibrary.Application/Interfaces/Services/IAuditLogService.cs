namespace NexLibrary.Application.Interfaces.Services;

public interface IAuditLogService
{
    Task LogAsync(
        string islemTuru,
        string tabloAdi,
        int? kayitId = null,
        object? eskiDeger = null,
        object? yeniDeger = null,
        string? aciklama = null,
        int? kullaniciId = null,
        string? ipAdresi = null,
        CancellationToken cancellationToken = default);
}