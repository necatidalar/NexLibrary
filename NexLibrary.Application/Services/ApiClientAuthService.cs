using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NexLibrary.Application.Interfaces.Repositories;
using NexLibrary.Application.Interfaces.Services;
using NexLibrary.Contracts.Audit;
using NexLibrary.Contracts.Auth;
using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Permissions;

namespace NexLibrary.Application.Services;

public sealed class ApiClientAuthService : IApiClientAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly IAuditLogService _auditLogService;

    public ApiClientAuthService(
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        IAuditLogService auditLogService)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _auditLogService = auditLogService;
    }

    public async Task<ApiResponse<ApiClientTokenResponse>> CreateTokenAsync(
        ApiClientTokenRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.ClientId))
        {
            await LogAuditSafeAsync(
                AuditActionTypes.ApiClientTokenFailed,
                "ApiClients",
                kayitId: null,
                yeniDeger: new
                {
                    ClientId = request.ClientId,
                    Sebep = "ClientId boş"
                },
                aciklama: "API client token oluşturma başarısız: ClientId boş.",
                cancellationToken);

            return ApiResponse<ApiClientTokenResponse>.Fail("ClientId zorunludur.");
        }

        if (string.IsNullOrWhiteSpace(request.ClientSecret))
        {
            await LogAuditSafeAsync(
                AuditActionTypes.ApiClientTokenFailed,
                "ApiClients",
                kayitId: null,
                yeniDeger: new
                {
                    ClientId = request.ClientId,
                    Sebep = "ClientSecret boş"
                },
                aciklama: "API client token oluşturma başarısız: ClientSecret boş.",
                cancellationToken);

            return ApiResponse<ApiClientTokenResponse>.Fail("ClientSecret zorunludur.");
        }

        var clientId = request.ClientId.Trim();

        var apiClient = await _unitOfWork.ApiClients
            .Query()
            .Include(x => x.ApiClientYetkileri)
                .ThenInclude(x => x.YetkiTanimi)
            .FirstOrDefaultAsync(
                x => x.ClientId == clientId,
                cancellationToken);

        if (apiClient is null)
        {
            await LogAuditSafeAsync(
                AuditActionTypes.ApiClientTokenFailed,
                "ApiClients",
                kayitId: null,
                yeniDeger: new
                {
                    ClientId = clientId,
                    Sebep = "Client bulunamadı"
                },
                aciklama: "API client token oluşturma başarısız: client bulunamadı.",
                cancellationToken);

            return ApiResponse<ApiClientTokenResponse>.Fail("API client bilgileri hatalı.");
        }

        if (!apiClient.AktifMi)
        {
            await LogAuditSafeAsync(
                AuditActionTypes.ApiClientTokenFailed,
                "ApiClients",
                kayitId: apiClient.Id,
                yeniDeger: new
                {
                    apiClient.ClientId,
                    Sebep = "Client pasif"
                },
                aciklama: "API client token oluşturma başarısız: client pasif.",
                cancellationToken);

            return ApiResponse<ApiClientTokenResponse>.Fail("API client pasif durumdadır.");
        }

        if (string.IsNullOrWhiteSpace(apiClient.ClientSecretHash) ||
            string.IsNullOrWhiteSpace(apiClient.ClientSecretSalt))
        {
            await LogAuditSafeAsync(
                AuditActionTypes.ApiClientTokenFailed,
                "ApiClients",
                kayitId: apiClient.Id,
                yeniDeger: new
                {
                    apiClient.ClientId,
                    Sebep = "Client secret bilgisi eksik"
                },
                aciklama: "API client token oluşturma başarısız: secret bilgisi eksik.",
                cancellationToken);

            return ApiResponse<ApiClientTokenResponse>.Fail("API client secret bilgisi eksik.");
        }

        var secretValid = VerifySecret(
            request.ClientSecret,
            apiClient.ClientSecretHash,
            apiClient.ClientSecretSalt);

        if (!secretValid)
        {
            await LogAuditSafeAsync(
                AuditActionTypes.ApiClientTokenFailed,
                "ApiClients",
                kayitId: apiClient.Id,
                yeniDeger: new
                {
                    apiClient.ClientId,
                    Sebep = "Client secret hatalı"
                },
                aciklama: "API client token oluşturma başarısız: secret hatalı.",
                cancellationToken);

            return ApiResponse<ApiClientTokenResponse>.Fail("API client bilgileri hatalı.");
        }

        var permissionCodes = apiClient.ApiClientYetkileri
            .Where(x => x.AktifMi && x.YetkiTanimi.AktifMi)
            .Select(x => x.YetkiTanimi.YetkiKodu)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .ToList();

        if (permissionCodes.Count == 0)
        {
            await LogAuditSafeAsync(
                AuditActionTypes.ApiClientTokenFailed,
                "ApiClients",
                kayitId: apiClient.Id,
                yeniDeger: new
                {
                    apiClient.ClientId,
                    Sebep = "Aktif yetki bulunamadı"
                },
                aciklama: "API client token oluşturma başarısız: aktif yetki bulunamadı.",
                cancellationToken);

            return ApiResponse<ApiClientTokenResponse>.Fail("API client için aktif yetki bulunamadı.");
        }

        var expiresAt = GetTokenExpiration();

        var accessToken = CreateAccessToken(
            apiClient.Id,
            apiClient.ClientId,
            apiClient.ClientName,
            permissionCodes,
            expiresAt);

        apiClient.SonKullanimTarihi = DateTime.UtcNow;
        apiClient.GuncellemeTarihi = DateTime.UtcNow;

        _unitOfWork.ApiClients.Update(apiClient);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await LogAuditSafeAsync(
            AuditActionTypes.ApiClientTokenSuccess,
            "ApiClients",
            kayitId: apiClient.Id,
            yeniDeger: new
            {
                apiClient.ClientId,
                apiClient.ClientName,
                YetkiSayisi = permissionCodes.Count,
                ExpiresAt = expiresAt
            },
            aciklama: "API client token başarıyla oluşturuldu.",
            cancellationToken);

        var response = new ApiClientTokenResponse
        {
            ApiClientId = apiClient.Id,
            ClientId = apiClient.ClientId,
            ClientName = apiClient.ClientName,
            AccessToken = accessToken,
            ExpiresAt = expiresAt,
            Yetkiler = permissionCodes
        };

        return ApiResponse<ApiClientTokenResponse>.Success(
            response,
            "API client token başarıyla oluşturuldu.");
    }

    private string CreateAccessToken(
        int apiClientId,
        string clientId,
        string clientName,
        IReadOnlyCollection<string> permissions,
        DateTimeOffset expiresAt)
    {
        var issuer = GetRequiredJwtSetting("Jwt:Issuer");
        var audience = GetRequiredJwtSetting("Jwt:Audience");
        var key = GetRequiredJwtSetting("Jwt:Key");

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, apiClientId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("token_type", "client"),
            new("api_client_id", apiClientId.ToString()),
            new("client_id", clientId),
            new("client_name", clientName),
            new(ClaimTypes.NameIdentifier, apiClientId.ToString()),
            new(ClaimTypes.Name, clientId)
        };

        foreach (var permission in permissions)
        {
            claims.Add(new Claim(AppClaimTypes.Permission, permission));
        }

        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key));

        var signingCredentials = new SigningCredentials(
            signingKey,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAt.UtcDateTime,
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private DateTimeOffset GetTokenExpiration()
    {
        var expiresInMinutesText = _configuration["Jwt:ExpiresInMinutes"];

        var expiresInMinutes = int.TryParse(
            expiresInMinutesText,
            out var parsedValue)
            ? parsedValue
            : 480;

        return DateTimeOffset.UtcNow.AddMinutes(expiresInMinutes);
    }

    private string GetRequiredJwtSetting(string key)
    {
        var value = _configuration[key];

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"{key} ayarı bulunamadı.");
        }

        return value;
    }

    private static bool VerifySecret(
        string clientSecret,
        string storedHash,
        string storedSalt)
    {
        var saltBytes = Convert.FromBase64String(storedSalt);

        using var pbkdf2 = new Rfc2898DeriveBytes(
            clientSecret,
            saltBytes,
            100_000,
            HashAlgorithmName.SHA256);

        var enteredHashBytes = pbkdf2.GetBytes(32);
        var storedHashBytes = Convert.FromBase64String(storedHash);

        return CryptographicOperations.FixedTimeEquals(
            storedHashBytes,
            enteredHashBytes);
    }

    private async Task LogAuditSafeAsync(
        string islemTuru,
        string tabloAdi,
        int? kayitId,
        object? yeniDeger,
        string? aciklama,
        CancellationToken cancellationToken)
    {
        try
        {
            await _auditLogService.LogAsync(
                islemTuru,
                tabloAdi,
                kayitId,
                eskiDeger: null,
                yeniDeger: yeniDeger,
                aciklama: aciklama,
                kullaniciId: null,
                ipAdresi: null,
                cancellationToken);
        }
        catch
        {
            // Audit log hatası token üretim akışını bozmasın.
        }
    }
}