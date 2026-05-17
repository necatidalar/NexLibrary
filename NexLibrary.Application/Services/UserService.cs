using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using NexLibrary.Application.Interfaces.Repositories;
using NexLibrary.Application.Interfaces.Services;
using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Users;
using NexLibrary.Domain.Entities;

namespace NexLibrary.Application.Services;

public sealed class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<PagedResponse<UserListResponse>>> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 20,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 20 : pageSize;

        var query = _unitOfWork.Kullanicilar
            .Query()
            .AsNoTracking()
            .Include(x => x.KullaniciRolleri)
            .ThenInclude(x => x.Rol)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchText = search.Trim();

            query = query.Where(x =>
                x.KullaniciAdi.Contains(searchText) ||
                x.AdSoyad.Contains(searchText) ||
                (x.Eposta != null && x.Eposta.Contains(searchText)) ||
                (x.Telefon != null && x.Telefon.Contains(searchText)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var users = await query
            .OrderByDescending(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var response = new PagedResponse<UserListResponse>
        {
            Items = users.Select(MapToListResponse).ToList(),
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };

        return ApiResponse<PagedResponse<UserListResponse>>.Success(response);
    }

    public async Task<ApiResponse<UserDetailResponse>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Kullanicilar
            .Query()
            .AsNoTracking()
            .Include(x => x.KullaniciRolleri)
            .ThenInclude(x => x.Rol)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (user is null)
        {
            return ApiResponse<UserDetailResponse>.Fail("Kullanıcı bulunamadı.");
        }

        return ApiResponse<UserDetailResponse>.Success(MapToDetailResponse(user));
    }

    public async Task<ApiResponse<UserDetailResponse>> CreateAsync(
        UserCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var errors = await ValidateCreateAsync(request, cancellationToken);

        if (errors.Count > 0)
        {
            return ApiResponse<UserDetailResponse>.ValidationFail(errors);
        }

        UserDetailResponse? createdUser = null;

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var password = HashPassword(request.Sifre);

            var user = new Kullanici
            {
                KullaniciAdi = request.KullaniciAdi.Trim(),
                AdSoyad = request.AdSoyad.Trim(),
                Eposta = string.IsNullOrWhiteSpace(request.Eposta) ? null : request.Eposta.Trim(),
                Telefon = string.IsNullOrWhiteSpace(request.Telefon) ? null : request.Telefon.Trim(),
                SifreHash = password.Hash,
                SifreSalt = password.Salt,
                AktifMi = request.AktifMi,
                OlusturmaTarihi = DateTime.UtcNow
            };

            await _unitOfWork.Kullanicilar.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var userRole = new KullaniciRol
            {
                KullaniciId = user.Id,
                RolId = request.RolId,
                AktifMi = true,
                OlusturmaTarihi = DateTime.UtcNow
            };

            await _unitOfWork.KullaniciRolleri.AddAsync(userRole, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var detailResult = await GetByIdAsync(user.Id, cancellationToken);
            createdUser = detailResult.Veri;
        }, cancellationToken);

        return ApiResponse<UserDetailResponse>.Success(
            createdUser!,
            "Kullanıcı başarıyla oluşturuldu.");
    }

    public async Task<ApiResponse<UserDetailResponse>> UpdateAsync(
        int id,
        UserUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (id != request.Id)
        {
            return ApiResponse<UserDetailResponse>.Fail("Geçersiz kullanıcı bilgisi.");
        }

        var user = await _unitOfWork.Kullanicilar
            .Query()
            .Include(x => x.KullaniciRolleri)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (user is null)
        {
            return ApiResponse<UserDetailResponse>.Fail("Kullanıcı bulunamadı.");
        }

        var roleExists = await _unitOfWork.Roller
            .Query()
            .AnyAsync(x => x.Id == request.RolId && x.AktifMi, cancellationToken);

        if (!roleExists)
        {
            return ApiResponse<UserDetailResponse>.Fail("Geçerli bir rol seçilmelidir.");
        }

        user.AdSoyad = request.AdSoyad.Trim();
        user.Eposta = string.IsNullOrWhiteSpace(request.Eposta) ? null : request.Eposta.Trim();
        user.Telefon = string.IsNullOrWhiteSpace(request.Telefon) ? null : request.Telefon.Trim();
        user.AktifMi = request.AktifMi;
        user.GuncellemeTarihi = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(request.YeniSifre))
        {
            if (request.YeniSifre.Length < 6)
            {
                return ApiResponse<UserDetailResponse>.Fail("Yeni şifre en az 6 karakter olmalıdır.");
            }

            var password = HashPassword(request.YeniSifre);
            user.SifreHash = password.Hash;
            user.SifreSalt = password.Salt;
        }

        var currentUserRole = user.KullaniciRolleri.FirstOrDefault();

        if (currentUserRole is null)
        {
            user.KullaniciRolleri.Add(new KullaniciRol
            {
                KullaniciId = user.Id,
                RolId = request.RolId,
                AktifMi = true,
                OlusturmaTarihi = DateTime.UtcNow
            });
        }
        else
        {
            currentUserRole.RolId = request.RolId;
            currentUserRole.AktifMi = true;
            currentUserRole.GuncellemeTarihi = DateTime.UtcNow;
        }

        _unitOfWork.Kullanicilar.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var detail = await GetByIdAsync(user.Id, cancellationToken);

        return ApiResponse<UserDetailResponse>.Success(
            detail.Veri!,
            "Kullanıcı başarıyla güncellendi.");
    }

    public async Task<ApiResponse<List<RoleResponse>>> GetRolesAsync(
        CancellationToken cancellationToken = default)
    {
        var roles = await _unitOfWork.Roller
            .Query()
            .AsNoTracking()
            .Where(x => x.AktifMi)
            .OrderBy(x => x.Id)
            .Select(x => new RoleResponse
            {
                Id = x.Id,
                RolKodu = x.RolKodu,
                RolAdi = x.RolAdi,
                Aciklama = x.Aciklama,
                AktifMi = x.AktifMi
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<RoleResponse>>.Success(roles);
    }

    private async Task<List<ValidationError>> ValidateCreateAsync(
    UserCreateRequest request,
    CancellationToken cancellationToken)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(request.KullaniciAdi))
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.KullaniciAdi),
                Message = "Kullanıcı adı zorunludur."
            });
        }

        if (string.IsNullOrWhiteSpace(request.AdSoyad))
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.AdSoyad),
                Message = "Ad soyad zorunludur."
            });
        }

        if (string.IsNullOrWhiteSpace(request.Sifre) || request.Sifre.Length < 6)
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.Sifre),
                Message = "Şifre en az 6 karakter olmalıdır."
            });
        }

        if (!string.IsNullOrWhiteSpace(request.KullaniciAdi))
        {
            var username = request.KullaniciAdi.Trim();

            var usernameExists = await _unitOfWork.Kullanicilar
                .Query()
                .AnyAsync(x => x.KullaniciAdi == username, cancellationToken);

            if (usernameExists)
            {
                errors.Add(new ValidationError
                {
                    Field = nameof(request.KullaniciAdi),
                    Message = "Bu kullanıcı adı zaten kullanılıyor."
                });
            }
        }

        var roleExists = await _unitOfWork.Roller
            .Query()
            .AnyAsync(x => x.Id == request.RolId && x.AktifMi, cancellationToken);

        if (!roleExists)
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.RolId),
                Message = "Geçerli bir rol seçilmelidir."
            });
        }

        return errors;
    }

    private static UserListResponse MapToListResponse(Kullanici user)
    {
        return new UserListResponse
        {
            Id = user.Id,
            KullaniciAdi = user.KullaniciAdi,
            AdSoyad = user.AdSoyad,
            Eposta = user.Eposta,
            Telefon = user.Telefon,
            AktifMi = user.AktifMi,
            OlusturmaTarihi = user.OlusturmaTarihi,
            Roller = user.KullaniciRolleri
                .Where(x => x.AktifMi)
                .Select(x => x.Rol.RolAdi)
                .ToList()
        };
    }

    private static UserDetailResponse MapToDetailResponse(Kullanici user)
    {
        return new UserDetailResponse
        {
            Id = user.Id,
            KullaniciAdi = user.KullaniciAdi,
            AdSoyad = user.AdSoyad,
            Eposta = user.Eposta,
            Telefon = user.Telefon,
            AktifMi = user.AktifMi,
            OlusturmaTarihi = user.OlusturmaTarihi,
            GuncellemeTarihi = user.GuncellemeTarihi,
            SonGirisTarihi = user.SonGirisTarihi,
            Roller = user.KullaniciRolleri
                .Where(x => x.AktifMi)
                .Select(x => new RoleResponse
                {
                    Id = x.Rol.Id,
                    RolKodu = x.Rol.RolKodu,
                    RolAdi = x.Rol.RolAdi,
                    Aciklama = x.Rol.Aciklama,
                    AktifMi = x.Rol.AktifMi
                })
                .ToList()
        };
    }

    private static (string Hash, string Salt) HashPassword(string password)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(16);

        var hashBytes = Rfc2898DeriveBytes.Pbkdf2(
            password,
            saltBytes,
            100_000,
            HashAlgorithmName.SHA256,
            32);

        return (
            Convert.ToBase64String(hashBytes),
            Convert.ToBase64String(saltBytes));
    }
}