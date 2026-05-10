using System.Globalization;
using Microsoft.EntityFrameworkCore;
using NexLibrary.Application.Interfaces.Repositories;
using NexLibrary.Application.Interfaces.Services;
using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.DynamicForms;
using NexLibrary.Contracts.Members;
using NexLibrary.Domain.Entities;
using NexLibrary.Domain.Enums;

namespace NexLibrary.Application.Services;

public sealed class MemberService : IMemberService
{
    private readonly IUnitOfWork _unitOfWork;

    public MemberService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<PagedResponse<MemberListResponse>>> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 20,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 20 : pageSize;

        var query = _unitOfWork.Uyeler
            .Query()
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchText = search.Trim();

            var dynamicMatchedMemberIds = await _unitOfWork.DinamikAlanDegerleri
                .Query()
                .AsNoTracking()
                .Include(x => x.FormAlani)
                .Where(x =>
                    x.ModulKodu == ModulKodu.Uyeler &&
                    x.AktifMi &&
                    x.FormAlani.AktifMi &&
                    x.FormAlani.AramadaGorunsunMu &&
                    (
                        (x.DegerMetin != null && x.DegerMetin.Contains(searchText)) ||
                        (x.DegerJson != null && x.DegerJson.Contains(searchText))
                    ))
                .Select(x => x.KayitId)
                .Distinct()
                .ToListAsync(cancellationToken);

            query = query.Where(x =>
                x.UyeAdiSoyadi.Contains(searchText) ||
                dynamicMatchedMemberIds.Contains(x.Id));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var members = await query
            .OrderByDescending(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var memberIds = members.Select(x => x.Id).ToList();

        var dynamicValues = await _unitOfWork.DinamikAlanDegerleri
            .Query()
            .AsNoTracking()
            .Include(x => x.FormAlani)
            .Where(x =>
                x.ModulKodu == ModulKodu.Uyeler &&
                memberIds.Contains(x.KayitId) &&
                x.AktifMi &&
                x.FormAlani.AktifMi &&
                x.FormAlani.ListedeGorunsunMu)
            .ToListAsync(cancellationToken);

        var items = members.Select(member =>
        {
            var values = dynamicValues
                .Where(x => x.KayitId == member.Id)
                .OrderBy(x => x.FormAlani.SiraNo)
                .ToDictionary(
                    x => x.FormAlani.AlanAdi,
                    x => GetValueAsString(x));

            return new MemberListResponse
            {
                Id = member.Id,
                UyeAdiSoyadi = member.UyeAdiSoyadi,
                AktifMi = member.AktifMi,
                OlusturmaTarihi = member.OlusturmaTarihi,
                DinamikAlanlar = values
            };
        }).ToList();

        var response = new PagedResponse<MemberListResponse>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };

        return ApiResponse<PagedResponse<MemberListResponse>>.Success(response);
    }

    public async Task<ApiResponse<MemberDetailResponse>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var member = await _unitOfWork.Uyeler
            .Query()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (member is null)
        {
            return ApiResponse<MemberDetailResponse>.Fail("Üye bulunamadı.");
        }

        var dynamicValues = await _unitOfWork.DinamikAlanDegerleri
            .Query()
            .AsNoTracking()
            .Include(x => x.FormAlani)
            .Where(x =>
                x.ModulKodu == ModulKodu.Uyeler &&
                x.KayitId == id &&
                x.AktifMi &&
                x.FormAlani.AktifMi)
            .OrderBy(x => x.FormAlani.SiraNo)
            .ToListAsync(cancellationToken);

        var response = new MemberDetailResponse
        {
            Id = member.Id,
            UyeAdiSoyadi = member.UyeAdiSoyadi,
            AktifMi = member.AktifMi,
            OlusturmaTarihi = member.OlusturmaTarihi,
            GuncellemeTarihi = member.GuncellemeTarihi,
            DinamikAlanlar = dynamicValues.Select(x => new DynamicFieldValueResponse
            {
                FormAlaniId = x.FormAlaniId,
                AlanKodu = x.FormAlani.AlanKodu,
                AlanAdi = x.FormAlani.AlanAdi,
                AlanTipi = x.FormAlani.AlanTipi.ToString(),
                Deger = GetValueAsString(x)
            }).ToList()
        };

        return ApiResponse<MemberDetailResponse>.Success(response);
    }

    public async Task<ApiResponse<MemberDetailResponse>> CreateAsync(
        MemberCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var validationErrors = await ValidateMemberRequestAsync(
            request.UyeAdiSoyadi,
            request.DinamikAlanlar,
            cancellationToken);

        if (validationErrors.Count > 0)
        {
            return ApiResponse<MemberDetailResponse>.ValidationFail(validationErrors);
        }

        MemberDetailResponse? createdMember = null;

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var member = new Uye
            {
                UyeAdiSoyadi = request.UyeAdiSoyadi.Trim(),
                AktifMi = true,
                OlusturmaTarihi = DateTime.UtcNow
            };

            await _unitOfWork.Uyeler.AddAsync(member, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var activeFields = await GetActiveMemberDynamicFieldsAsync(cancellationToken);

            foreach (var fieldValue in request.DinamikAlanlar)
            {
                var field = activeFields.FirstOrDefault(x =>
                    x.AlanKodu.Equals(fieldValue.AlanKodu, StringComparison.OrdinalIgnoreCase));

                if (field is null || field.SistemAlaniMi)
                {
                    continue;
                }

                var dynamicValue = CreateDynamicValueEntity(member.Id, field, fieldValue.Deger);

                if (dynamicValue is not null)
                {
                    await _unitOfWork.DinamikAlanDegerleri.AddAsync(dynamicValue, cancellationToken);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var detailResult = await GetByIdAsync(member.Id, cancellationToken);
            createdMember = detailResult.Veri;
        }, cancellationToken);

        return ApiResponse<MemberDetailResponse>.Success(
            createdMember!,
            "Üye başarıyla oluşturuldu.");
    }

    public async Task<ApiResponse<MemberDetailResponse>> UpdateAsync(
        int id,
        MemberUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        var member = await _unitOfWork.Uyeler.GetByIdAsync(id, cancellationToken);

        if (member is null)
        {
            return ApiResponse<MemberDetailResponse>.Fail("Üye bulunamadı.");
        }

        var validationErrors = await ValidateMemberRequestAsync(
            request.UyeAdiSoyadi,
            request.DinamikAlanlar,
            cancellationToken);

        if (validationErrors.Count > 0)
        {
            return ApiResponse<MemberDetailResponse>.ValidationFail(validationErrors);
        }

        MemberDetailResponse? updatedMember = null;

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            member.UyeAdiSoyadi = request.UyeAdiSoyadi.Trim();
            member.AktifMi = request.AktifMi;
            member.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.Uyeler.Update(member);

            var activeFields = await GetActiveMemberDynamicFieldsAsync(cancellationToken);

            var existingValues = await _unitOfWork.DinamikAlanDegerleri
                .Query()
                .Where(x => x.ModulKodu == ModulKodu.Uyeler && x.KayitId == id)
                .ToListAsync(cancellationToken);

            foreach (var fieldValue in request.DinamikAlanlar)
            {
                var field = activeFields.FirstOrDefault(x =>
                    x.AlanKodu.Equals(fieldValue.AlanKodu, StringComparison.OrdinalIgnoreCase));

                if (field is null || field.SistemAlaniMi)
                {
                    continue;
                }

                var existingValue = existingValues.FirstOrDefault(x => x.FormAlaniId == field.Id);

                if (string.IsNullOrWhiteSpace(fieldValue.Deger))
                {
                    if (existingValue is not null)
                    {
                        ClearDynamicValue(existingValue);
                        existingValue.GuncellemeTarihi = DateTime.UtcNow;
                        _unitOfWork.DinamikAlanDegerleri.Update(existingValue);
                    }

                    continue;
                }

                if (existingValue is null)
                {
                    var newValue = CreateDynamicValueEntity(id, field, fieldValue.Deger);

                    if (newValue is not null)
                    {
                        await _unitOfWork.DinamikAlanDegerleri.AddAsync(newValue, cancellationToken);
                    }
                }
                else
                {
                    SetDynamicValue(existingValue, field, fieldValue.Deger);
                    existingValue.AktifMi = true;
                    existingValue.GuncellemeTarihi = DateTime.UtcNow;

                    _unitOfWork.DinamikAlanDegerleri.Update(existingValue);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var detailResult = await GetByIdAsync(id, cancellationToken);
            updatedMember = detailResult.Veri;
        }, cancellationToken);

        return ApiResponse<MemberDetailResponse>.Success(
            updatedMember!,
            "Üye başarıyla güncellendi.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var member = await _unitOfWork.Uyeler.GetByIdAsync(id, cancellationToken);

        if (member is null)
        {
            return ApiResponse<bool>.Fail("Üye bulunamadı.");
        }

        member.AktifMi = false;
        member.GuncellemeTarihi = DateTime.UtcNow;

        _unitOfWork.Uyeler.Update(member);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Success(true, "Üye pasif hale getirildi.");
    }

    private async Task<List<FormAlani>> GetActiveMemberDynamicFieldsAsync(
        CancellationToken cancellationToken)
    {
        return await _unitOfWork.FormAlanlari
            .Query()
            .Where(x => x.ModulKodu == ModulKodu.Uyeler && x.AktifMi)
            .OrderBy(x => x.SiraNo)
            .ToListAsync(cancellationToken);
    }

    private async Task<List<ValidationError>> ValidateMemberRequestAsync(
        string uyeAdiSoyadi,
        List<DynamicFieldValueRequest> dynamicValues,
        CancellationToken cancellationToken)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(uyeAdiSoyadi))
        {
            errors.Add(new ValidationError
            {
                Field = nameof(MemberCreateRequest.UyeAdiSoyadi),
                Message = "Üye adı soyadı zorunludur."
            });
        }
        else if (uyeAdiSoyadi.Length > 200)
        {
            errors.Add(new ValidationError
            {
                Field = nameof(MemberCreateRequest.UyeAdiSoyadi),
                Message = "Üye adı soyadı en fazla 200 karakter olabilir."
            });
        }

        var activeFields = await GetActiveMemberDynamicFieldsAsync(cancellationToken);

        var dynamicFieldDictionary = dynamicValues
            .GroupBy(x => x.AlanKodu, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(x => x.Key, x => x.Last().Deger, StringComparer.OrdinalIgnoreCase);

        foreach (var field in activeFields.Where(x => !x.SistemAlaniMi))
        {
            dynamicFieldDictionary.TryGetValue(field.AlanKodu, out var value);

            if (field.ZorunluMu && string.IsNullOrWhiteSpace(value))
            {
                errors.Add(new ValidationError
                {
                    Field = field.AlanKodu,
                    Message = $"{field.AlanAdi} alanı zorunludur."
                });

                continue;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            if (field.MinimumKarakter.HasValue && value.Length < field.MinimumKarakter.Value)
            {
                errors.Add(new ValidationError
                {
                    Field = field.AlanKodu,
                    Message = $"{field.AlanAdi} en az {field.MinimumKarakter.Value} karakter olmalıdır."
                });
            }

            if (field.MaksimumKarakter.HasValue && value.Length > field.MaksimumKarakter.Value)
            {
                errors.Add(new ValidationError
                {
                    Field = field.AlanKodu,
                    Message = $"{field.AlanAdi} en fazla {field.MaksimumKarakter.Value} karakter olmalıdır."
                });
            }

            if (!IsValueCompatibleWithFieldType(field.AlanTipi, value))
            {
                errors.Add(new ValidationError
                {
                    Field = field.AlanKodu,
                    Message = $"{field.AlanAdi} alanı {field.AlanTipi} tipine uygun değil."
                });
            }
        }

        return errors;
    }

    private static DinamikAlanDegeri? CreateDynamicValueEntity(
        int memberId,
        FormAlani field,
        string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var entity = new DinamikAlanDegeri
        {
            ModulKodu = ModulKodu.Uyeler,
            KayitId = memberId,
            FormAlaniId = field.Id,
            AktifMi = true,
            OlusturmaTarihi = DateTime.UtcNow
        };

        SetDynamicValue(entity, field, value);

        return entity;
    }

    private static void SetDynamicValue(
        DinamikAlanDegeri entity,
        FormAlani field,
        string? value)
    {
        ClearDynamicValue(entity);

        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var trimmedValue = value.Trim();

        switch (field.AlanTipi)
        {
            case AlanTipi.Sayi:
            case AlanTipi.OndalikliSayi:
            case AlanTipi.Para:
                if (TryParseDecimal(trimmedValue, out var decimalValue))
                {
                    entity.DegerSayi = decimalValue;
                }
                break;

            case AlanTipi.Tarih:
            case AlanTipi.TarihSaat:
                if (DateTime.TryParse(trimmedValue, out var dateValue))
                {
                    entity.DegerTarih = dateValue;
                }
                break;

            case AlanTipi.EvetHayir:
                if (TryParseBool(trimmedValue, out var boolValue))
                {
                    entity.DegerBool = boolValue;
                }
                break;

            case AlanTipi.CokluListe:
                entity.DegerJson = trimmedValue;
                break;

            default:
                entity.DegerMetin = trimmedValue;
                break;
        }
    }

    private static void ClearDynamicValue(DinamikAlanDegeri entity)
    {
        entity.DegerMetin = null;
        entity.DegerSayi = null;
        entity.DegerTarih = null;
        entity.DegerBool = null;
        entity.DegerJson = null;
    }

    private static string? GetValueAsString(DinamikAlanDegeri entity)
    {
        if (entity.DegerMetin is not null)
        {
            return entity.DegerMetin;
        }

        if (entity.DegerSayi.HasValue)
        {
            return entity.DegerSayi.Value.ToString(CultureInfo.InvariantCulture);
        }

        if (entity.DegerTarih.HasValue)
        {
            return entity.DegerTarih.Value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        if (entity.DegerBool.HasValue)
        {
            return entity.DegerBool.Value ? "true" : "false";
        }

        if (entity.DegerJson is not null)
        {
            return entity.DegerJson;
        }

        return null;
    }

    private static bool IsValueCompatibleWithFieldType(
        AlanTipi fieldType,
        string value)
    {
        return fieldType switch
        {
            AlanTipi.Sayi or AlanTipi.OndalikliSayi or AlanTipi.Para
                => TryParseDecimal(value, out _),

            AlanTipi.Tarih or AlanTipi.TarihSaat
                => DateTime.TryParse(value, out _),

            AlanTipi.EvetHayir
                => TryParseBool(value, out _),

            _ => true
        };
    }

    private static bool TryParseDecimal(string value, out decimal result)
    {
        return decimal.TryParse(
                   value,
                   NumberStyles.Any,
                   CultureInfo.GetCultureInfo("tr-TR"),
                   out result)
               ||
               decimal.TryParse(
                   value,
                   NumberStyles.Any,
                   CultureInfo.InvariantCulture,
                   out result);
    }

    private static bool TryParseBool(string value, out bool result)
    {
        var normalizedValue = value.Trim().ToLowerInvariant();

        if (normalizedValue is "true" or "1" or "evet" or "e")
        {
            result = true;
            return true;
        }

        if (normalizedValue is "false" or "0" or "hayır" or "hayir" or "h")
        {
            result = false;
            return true;
        }

        result = false;
        return false;
    }
}