using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using NexLibrary.Application.Interfaces.Repositories;
using NexLibrary.Application.Interfaces.Services;
using NexLibrary.Contracts.Auth;
using NexLibrary.Contracts.Common;

namespace NexLibrary.Application.Services;

public sealed class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
            .FirstOrDefaultAsync(x => x.KullaniciAdi == kullaniciAdi, cancellationToken);

        if (user is null)
        {
            return ApiResponse<LoginResponse>.Fail("Kullanıcı adı veya şifre hatalı.");
        }

        if (!user.AktifMi)
        {
            return ApiResponse<LoginResponse>.Fail("Bu kullanıcı pasif durumdadır.");
        }

        if (string.IsNullOrWhiteSpace(user.SifreHash) || string.IsNullOrWhiteSpace(user.SifreSalt))
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
            Yetkiler = permissionCodes
        };

        return ApiResponse<LoginResponse>.Success(
            response,
            "Giriş başarılı.");
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