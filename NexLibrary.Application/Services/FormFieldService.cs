using Microsoft.EntityFrameworkCore;
using NexLibrary.Application.Interfaces.Repositories;
using NexLibrary.Application.Interfaces.Services;
using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.DynamicForms;
using NexLibrary.Domain.Entities;
using NexLibrary.Domain.Enums;

namespace NexLibrary.Application.Services;

public sealed class FormFieldService : IFormFieldService
{
    private readonly IUnitOfWork _unitOfWork;

    public FormFieldService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<FormDesignResponse>> GetFormDesignAsync(
        string modulKodu,
        CancellationToken cancellationToken = default)
    {
        if (!TryParseModulKodu(modulKodu, out var parsedModule))
        {
            return ApiResponse<FormDesignResponse>.Fail("Geçersiz modül kodu.");
        }

        var fields = await _unitOfWork.FormAlanlari
            .Query()
            .Where(x => x.ModulKodu == parsedModule && x.AktifMi && x.FormdaGorunsunMu)
            .OrderBy(x => x.SiraNo)
            .ThenBy(x => x.Id)
            .ToListAsync(cancellationToken);

        var response = new FormDesignResponse
        {
            ModulKodu = parsedModule.ToString(),
            ModulAdi = GetModuleDisplayName(parsedModule),
            Alanlar = fields.Select(MapToResponse).ToList()
        };

        return ApiResponse<FormDesignResponse>.Success(response);
    }

    public async Task<ApiResponse<List<FormFieldResponse>>> GetByModuleAsync(
        string modulKodu,
        CancellationToken cancellationToken = default)
    {
        if (!TryParseModulKodu(modulKodu, out var parsedModule))
        {
            return ApiResponse<List<FormFieldResponse>>.Fail("Geçersiz modül kodu.");
        }

        var fields = await _unitOfWork.FormAlanlari
            .Query()
            .Where(x => x.ModulKodu == parsedModule)
            .OrderBy(x => x.SiraNo)
            .ThenBy(x => x.Id)
            .ToListAsync(cancellationToken);

        return ApiResponse<List<FormFieldResponse>>.Success(
            fields.Select(MapToResponse).ToList());
    }

    public async Task<ApiResponse<FormFieldResponse>> CreateAsync(
        FormFieldCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var errors = ValidateCreateRequest(request);

        if (!TryParseModulKodu(request.ModulKodu, out var parsedModule))
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.ModulKodu),
                Message = "Geçersiz modül kodu."
            });
        }

        if (!TryParseAlanTipi(request.AlanTipi, out var parsedFieldType))
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.AlanTipi),
                Message = "Geçersiz alan tipi."
            });
        }

        if (errors.Count > 0)
        {
            return ApiResponse<FormFieldResponse>.ValidationFail(errors);
        }

        var normalizedFieldCode = NormalizeFieldCode(request.AlanKodu);

        var exists = await _unitOfWork.FormAlanlari.AnyAsync(
            x => x.ModulKodu == parsedModule && x.AlanKodu == normalizedFieldCode,
            cancellationToken);

        if (exists)
        {
            return ApiResponse<FormFieldResponse>.Fail("Bu modülde aynı alan kodu zaten var.");
        }

        var entity = new FormAlani
        {
            ModulKodu = parsedModule,
            AlanKodu = normalizedFieldCode,
            AlanAdi = request.AlanAdi.Trim(),
            AlanTipi = parsedFieldType,
            MinimumKarakter = request.MinimumKarakter,
            MaksimumKarakter = request.MaksimumKarakter,
            ZorunluMu = request.ZorunluMu,
            BenzersizMi = request.BenzersizMi,
            VarsayilanDeger = request.VarsayilanDeger,
            Aciklama = request.Aciklama,
            Placeholder = request.Placeholder,
            SiraNo = request.SiraNo,
            FormdaGorunsunMu = request.FormdaGorunsunMu,
            ListedeGorunsunMu = request.ListedeGorunsunMu,
            AramadaGorunsunMu = request.AramadaGorunsunMu,
            DetaydaGorunsunMu = request.DetaydaGorunsunMu,
            HizliKayittaGorunsunMu = request.HizliKayittaGorunsunMu,
            SistemAlaniMi = false,
            SilinebilirMi = true,
            TipDegistirilebilirMi = true,
            AktifMi = true,
            OlusturmaTarihi = DateTime.UtcNow
        };

        await _unitOfWork.FormAlanlari.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<FormFieldResponse>.Success(
            MapToResponse(entity),
            "Form alanı başarıyla oluşturuldu.");
    }

    public async Task<ApiResponse<FormFieldResponse>> UpdateAsync(
        int id,
        FormFieldUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.FormAlanlari.GetByIdAsync(id, cancellationToken);

        if (entity is null)
        {
            return ApiResponse<FormFieldResponse>.Fail("Form alanı bulunamadı.");
        }

        var errors = ValidateUpdateRequest(request);

        if (errors.Count > 0)
        {
            return ApiResponse<FormFieldResponse>.ValidationFail(errors);
        }

        if (entity.SistemAlaniMi && !request.AktifMi)
        {
            return ApiResponse<FormFieldResponse>.Fail("Sistem alanı pasif yapılamaz.");
        }

        entity.AlanAdi = request.AlanAdi.Trim();
        entity.MinimumKarakter = request.MinimumKarakter;
        entity.MaksimumKarakter = request.MaksimumKarakter;
        entity.ZorunluMu = entity.SistemAlaniMi ? entity.ZorunluMu : request.ZorunluMu;
        entity.BenzersizMi = request.BenzersizMi;
        entity.VarsayilanDeger = request.VarsayilanDeger;
        entity.Aciklama = request.Aciklama;
        entity.Placeholder = request.Placeholder;
        entity.SiraNo = request.SiraNo;
        entity.FormdaGorunsunMu = request.FormdaGorunsunMu;
        entity.ListedeGorunsunMu = request.ListedeGorunsunMu;
        entity.AramadaGorunsunMu = request.AramadaGorunsunMu;
        entity.DetaydaGorunsunMu = request.DetaydaGorunsunMu;
        entity.HizliKayittaGorunsunMu = request.HizliKayittaGorunsunMu;
        entity.AktifMi = request.AktifMi;
        entity.GuncellemeTarihi = DateTime.UtcNow;

        _unitOfWork.FormAlanlari.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<FormFieldResponse>.Success(
            MapToResponse(entity),
            "Form alanı başarıyla güncellendi.");
    }

    public async Task<ApiResponse<bool>> SetActiveAsync(
        int id,
        bool aktifMi,
        CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.FormAlanlari.GetByIdAsync(id, cancellationToken);

        if (entity is null)
        {
            return ApiResponse<bool>.Fail("Form alanı bulunamadı.");
        }

        if (entity.SistemAlaniMi && !aktifMi)
        {
            return ApiResponse<bool>.Fail("Sistem alanı pasif yapılamaz.");
        }

        entity.AktifMi = aktifMi;
        entity.GuncellemeTarihi = DateTime.UtcNow;

        _unitOfWork.FormAlanlari.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Success(true, "Form alanı durumu güncellendi.");
    }

    private static List<ValidationError> ValidateCreateRequest(FormFieldCreateRequest request)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(request.ModulKodu))
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.ModulKodu),
                Message = "Modül kodu zorunludur."
            });
        }

        if (string.IsNullOrWhiteSpace(request.AlanAdi))
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.AlanAdi),
                Message = "Alan adı zorunludur."
            });
        }

        if (string.IsNullOrWhiteSpace(request.AlanKodu))
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.AlanKodu),
                Message = "Alan kodu zorunludur."
            });
        }

        if (string.IsNullOrWhiteSpace(request.AlanTipi))
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.AlanTipi),
                Message = "Alan tipi zorunludur."
            });
        }

        if (request.MinimumKarakter.HasValue &&
            request.MaksimumKarakter.HasValue &&
            request.MinimumKarakter.Value > request.MaksimumKarakter.Value)
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.MaksimumKarakter),
                Message = "Maksimum karakter, minimum karakterden küçük olamaz."
            });
        }

        return errors;
    }

    private static List<ValidationError> ValidateUpdateRequest(FormFieldUpdateRequest request)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(request.AlanAdi))
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.AlanAdi),
                Message = "Alan adı zorunludur."
            });
        }

        if (request.MinimumKarakter.HasValue &&
            request.MaksimumKarakter.HasValue &&
            request.MinimumKarakter.Value > request.MaksimumKarakter.Value)
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.MaksimumKarakter),
                Message = "Maksimum karakter, minimum karakterden küçük olamaz."
            });
        }

        return errors;
    }

    private static FormFieldResponse MapToResponse(FormAlani entity)
    {
        return new FormFieldResponse
        {
            Id = entity.Id,
            ModulKodu = entity.ModulKodu.ToString(),
            AlanKodu = entity.AlanKodu,
            AlanAdi = entity.AlanAdi,
            AlanTipi = entity.AlanTipi.ToString(),
            MinimumKarakter = entity.MinimumKarakter,
            MaksimumKarakter = entity.MaksimumKarakter,
            ZorunluMu = entity.ZorunluMu,
            BenzersizMi = entity.BenzersizMi,
            VarsayilanDeger = entity.VarsayilanDeger,
            Aciklama = entity.Aciklama,
            Placeholder = entity.Placeholder,
            SiraNo = entity.SiraNo,
            FormdaGorunsunMu = entity.FormdaGorunsunMu,
            ListedeGorunsunMu = entity.ListedeGorunsunMu,
            AramadaGorunsunMu = entity.AramadaGorunsunMu,
            DetaydaGorunsunMu = entity.DetaydaGorunsunMu,
            HizliKayittaGorunsunMu = entity.HizliKayittaGorunsunMu,
            SistemAlaniMi = entity.SistemAlaniMi,
            SilinebilirMi = entity.SilinebilirMi,
            TipDegistirilebilirMi = entity.TipDegistirilebilirMi,
            AktifMi = entity.AktifMi
        };
    }

    private static bool TryParseModulKodu(string value, out ModulKodu modulKodu)
    {
        if (Enum.TryParse(value, true, out modulKodu))
        {
            return true;
        }

        var normalizedValue = value.Trim().Replace("_", "").Replace(" ", "").ToUpperInvariant();

        foreach (var item in Enum.GetValues<ModulKodu>())
        {
            var normalizedName = item.ToString().Replace("_", "").Replace(" ", "").ToUpperInvariant();

            if (normalizedName == normalizedValue)
            {
                modulKodu = item;
                return true;
            }
        }

        modulKodu = default;
        return false;
    }

    private static bool TryParseAlanTipi(string value, out AlanTipi alanTipi)
    {
        if (Enum.TryParse(value, true, out alanTipi))
        {
            return true;
        }

        var normalizedValue = value.Trim().Replace("_", "").Replace(" ", "").ToUpperInvariant();

        foreach (var item in Enum.GetValues<AlanTipi>())
        {
            var normalizedName = item.ToString().Replace("_", "").Replace(" ", "").ToUpperInvariant();

            if (normalizedName == normalizedValue)
            {
                alanTipi = item;
                return true;
            }
        }

        alanTipi = default;
        return false;
    }

    private static string NormalizeFieldCode(string value)
    {
        return value
            .Trim()
            .Replace(" ", "_")
            .Replace("-", "_")
            .ToUpperInvariant();
    }

    private static string GetModuleDisplayName(ModulKodu modulKodu)
    {
        return modulKodu switch
        {
            ModulKodu.Kitaplar => "Kitaplar",
            ModulKodu.Uyeler => "Üyeler",
            ModulKodu.Personeller => "Personeller",
            ModulKodu.Oduncler => "Ödünçler",
            ModulKodu.Iadeler => "İadeler",
            ModulKodu.Raporlar => "Raporlar",
            ModulKodu.SistemAyarlari => "Sistem Ayarları",
            _ => modulKodu.ToString()
        };
    }
}