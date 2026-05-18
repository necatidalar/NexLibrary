using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NexLibrary.Application.Interfaces.Repositories;
using NexLibrary.Application.Interfaces.Services;
using NexLibrary.Contracts.Auth;
using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Permissions;

namespace NexLibrary.Application.Services;

public sealed class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUnitOfWork unitOfWork,
        IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<ApiResponse<LoginResponse>> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.KullaniciAdi))
        {
            return ApiResponse<LoginResponse>.Fail("Kullanıcı adı zorunludur.");
        }

        if (string.IsNullOrWhiteSpace(request.Sifre))
        {
            return ApiResponse<LoginResponse>.Fail("Şifre zorunludur.");
        }

        var kullaniciAdi = request.KullaniciAdi.Trim();

        var user = await _unitOfWork.Kullanicilar
            .Query()
            .Include(x => x.KullaniciRolleri)
                .ThenInclude(x => x.Rol)
                    .ThenInclude(x => x.RolYetkileri)
                        .ThenInclude(x => x.YetkiTanimi)
            .FirstOrDefaultAsync(
                x => x.KullaniciAdi == kullaniciAdi,
                cancellationToken);

        if (user is null)
        {
            return ApiResponse<LoginResponse>.Fail("Kullanıcı adı veya şifre hatalı.");
        }

        if (!user.AktifMi)
        {
            return ApiResponse<LoginResponse>.Fail("Bu kullanıcı pasif durumdadır.");
        }

        if (string.IsNullOrWhiteSpace(user.SifreHash) ||
            string.IsNullOrWhiteSpace(user.SifreSalt))
        {
            return ApiResponse<LoginResponse>.Fail("Kullanıcı şifre bilgisi eksik.");
        }

        var passwordValid = VerifyPassword(
            request.Sifre,
            user.SifreHash,
            user.SifreSalt);

        if (!passwordValid)
        {
            return ApiResponse<LoginResponse>.Fail("Kullanıcı adı veya şifre hatalı.");
        }

        var roleCodes = user.KullaniciRolleri
            .Where(x => x.AktifMi && x.Rol.AktifMi)
            .Select(x => x.Rol.RolKodu)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .ToList();

        if (roleCodes.Count == 0)
        {
            return ApiResponse<LoginResponse>.Fail("Kullanıcıya atanmış aktif rol bulunamadı.");
        }

        var permissionCodes = user.KullaniciRolleri
            .Where(x => x.AktifMi && x.Rol.AktifMi)
            .SelectMany(x => x.Rol.RolYetkileri
                .Where(y => y.AktifMi && y.YetkiTanimi.AktifMi)
                .Select(y => y.YetkiTanimi.YetkiKodu))
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .ToList();

        var expiresAt = GetTokenExpiration();

        var accessToken = CreateAccessToken(
            user.Id,
            user.KullaniciAdi,
            user.AdSoyad,
            user.Eposta,
            roleCodes,
            permissionCodes,
            expiresAt);

        user.SonGirisTarihi = DateTime.UtcNow;
        user.GuncellemeTarihi = DateTime.UtcNow;

        _unitOfWork.Kullanicilar.Update(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new LoginResponse
        {
            KullaniciId = user.Id,
            KullaniciAdi = user.KullaniciAdi,
            AdSoyad = user.AdSoyad,
            Eposta = user.Eposta,
            Telefon = user.Telefon,
            Roller = roleCodes,
            Yetkiler = permissionCodes,
            AccessToken = accessToken,
            ExpiresAt = expiresAt
        };

        return ApiResponse<LoginResponse>.Success(
            response,
            "Giriş başarılı.");
    }

    private string CreateAccessToken(
        int userId,
        string kullaniciAdi,
        string adSoyad,
        string? eposta,
        IReadOnlyCollection<string> roles,
        IReadOnlyCollection<string> permissions,
        DateTimeOffset expiresAt)
    {
        var issuer = GetRequiredJwtSetting("Jwt:Issuer");
        var audience = GetRequiredJwtSetting("Jwt:Audience");
        var key = GetRequiredJwtSetting("Jwt:Key");

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, kullaniciAdi),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Name, kullaniciAdi),
            new("AdSoyad", adSoyad)
        };

        if (!string.IsNullOrWhiteSpace(eposta))
        {
            claims.Add(new Claim(ClaimTypes.Email, eposta));
        }

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

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

    private static bool VerifyPassword(
        string password,
        string storedHash,
        string storedSalt)
    {
        var saltBytes = Convert.FromBase64String(storedSalt);

        var enteredHashBytes = Rfc2898DeriveBytes.Pbkdf2(
            password,
            saltBytes,
            100_000,
            HashAlgorithmName.SHA256,
            32);

        var storedHashBytes = Convert.FromBase64String(storedHash);

        return CryptographicOperations.FixedTimeEquals(
            storedHashBytes,
            enteredHashBytes);
    }
}