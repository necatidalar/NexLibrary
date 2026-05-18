using Microsoft.AspNetCore.Mvc;
using NexLibrary.Contracts.DynamicForms;
using NexLibrary.Contracts.Permissions;
using NexLibrary.Web.Security;
using NexLibrary.Web.Services;
using NexLibrary.Web.ViewModels.FormFields;

namespace NexLibrary.Web.Controllers;

public sealed class FormFieldsController : Controller
{
    private const string DefaultModuleCode = "Kitaplar";

    private static readonly HashSet<string> AllowedModules = new(StringComparer.OrdinalIgnoreCase)
    {
        "Kitaplar",
        "Uyeler",
        "Odunc",
        "Kopyalar"
    };

    private readonly FormFieldApiService _formFieldApiService;
    private readonly ILogger<FormFieldsController> _logger;

    public FormFieldsController(
        FormFieldApiService formFieldApiService,
        ILogger<FormFieldsController> logger)
    {
        _formFieldApiService = formFieldApiService;
        _logger = logger;
    }

    [PermissionAuthorize(PermissionCodes.FormFieldsView)]
    public async Task<IActionResult> Index(
        string modulKodu = DefaultModuleCode,
        CancellationToken cancellationToken = default)
    {
        modulKodu = NormalizeModuleCode(modulKodu);

        var fields = await GetFieldsByModuleSafeAsync(
            modulKodu,
            cancellationToken);

        var model = new FormFieldsIndexViewModel
        {
            ModulKodu = modulKodu,
            Fields = fields
                .OrderBy(x => x.SiraNo)
                .ThenBy(x => x.Id)
                .ToList()
        };

        return View(model);
    }

    [HttpGet]
    [PermissionAuthorize(PermissionCodes.FormFieldsCreate)]
    public async Task<IActionResult> Create(
        string modulKodu = DefaultModuleCode,
        CancellationToken cancellationToken = default)
    {
        modulKodu = NormalizeModuleCode(modulKodu);

        var existingFields = await GetFieldsByModuleSafeAsync(
            modulKodu,
            cancellationToken);

        var nextOrder = existingFields.Count == 0
            ? 1
            : existingFields.Max(x => x.SiraNo) + 1;

        var model = new FormFieldCreateViewModel
        {
            ModulKodu = modulKodu,
            SiraNo = nextOrder,
            FormdaGorunsunMu = true,
            ListedeGorunsunMu = true,
            DetaydaGorunsunMu = true,
            AktifMi = true
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [PermissionAuthorize(PermissionCodes.FormFieldsCreate)]
    public async Task<IActionResult> Create(
        FormFieldCreateViewModel model,
        CancellationToken cancellationToken = default)
    {
        model.ModulKodu = NormalizeModuleCode(model.ModulKodu);

        var validationMessage = ValidateCreateModel(model);

        if (validationMessage is not null)
        {
            TempData["ErrorMessage"] = validationMessage;
            return View(model);
        }

        var request = new FormFieldCreateRequest
        {
            ModulKodu = model.ModulKodu.Trim(),
            AlanAdi = model.AlanAdi.Trim(),
            AlanKodu = model.AlanKodu.Trim(),
            AlanTipi = model.AlanTipi.Trim(),
            MinimumKarakter = model.MinimumKarakter,
            MaksimumKarakter = model.MaksimumKarakter,
            ZorunluMu = model.ZorunluMu,
            BenzersizMi = model.BenzersizMi,
            VarsayilanDeger = NormalizeNullableText(model.VarsayilanDeger),
            Aciklama = NormalizeNullableText(model.Aciklama),
            Placeholder = NormalizeNullableText(model.Placeholder),
            SiraNo = model.SiraNo,
            FormdaGorunsunMu = model.FormdaGorunsunMu,
            ListedeGorunsunMu = model.ListedeGorunsunMu,
            AramadaGorunsunMu = model.AramadaGorunsunMu,
            DetaydaGorunsunMu = model.DetaydaGorunsunMu,
            HizliKayittaGorunsunMu = model.HizliKayittaGorunsunMu
        };

        var result = await CreateFieldSafeAsync(
            request,
            cancellationToken);

        if (result is null)
        {
            TempData["ErrorMessage"] = "Form alanı oluşturulamadı.";
            return View(model);
        }

        TempData["SuccessMessage"] = "Form alanı başarıyla oluşturuldu.";

        return RedirectToAction(nameof(Index), new { modulKodu = model.ModulKodu });
    }

    [HttpGet]
    [PermissionAuthorize(PermissionCodes.FormFieldsEdit)]
    public async Task<IActionResult> Edit(
        int id,
        string modulKodu = DefaultModuleCode,
        CancellationToken cancellationToken = default)
    {
        modulKodu = NormalizeModuleCode(modulKodu);

        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Geçersiz form alanı.";
            return RedirectToAction(nameof(Index), new { modulKodu });
        }

        var fields = await GetFieldsByModuleSafeAsync(
            modulKodu,
            cancellationToken);

        var field = fields.FirstOrDefault(x => x.Id == id);

        if (field is null)
        {
            TempData["ErrorMessage"] = "Form alanı bulunamadı.";
            return RedirectToAction(nameof(Index), new { modulKodu });
        }

        var model = new FormFieldEditViewModel
        {
            Id = field.Id,
            ModulKodu = field.ModulKodu,
            AlanKodu = field.AlanKodu,
            AlanAdi = field.AlanAdi,
            AlanTipi = field.AlanTipi,
            MinimumKarakter = field.MinimumKarakter,
            MaksimumKarakter = field.MaksimumKarakter,
            ZorunluMu = field.ZorunluMu,
            BenzersizMi = field.BenzersizMi,
            VarsayilanDeger = field.VarsayilanDeger,
            Aciklama = field.Aciklama,
            Placeholder = field.Placeholder,
            SiraNo = field.SiraNo,
            FormdaGorunsunMu = field.FormdaGorunsunMu,
            ListedeGorunsunMu = field.ListedeGorunsunMu,
            AramadaGorunsunMu = field.AramadaGorunsunMu,
            DetaydaGorunsunMu = field.DetaydaGorunsunMu,
            HizliKayittaGorunsunMu = field.HizliKayittaGorunsunMu,
            SistemAlaniMi = field.SistemAlaniMi,
            SilinebilirMi = field.SilinebilirMi,
            TipDegistirilebilirMi = field.TipDegistirilebilirMi,
            AktifMi = field.AktifMi
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [PermissionAuthorize(PermissionCodes.FormFieldsEdit)]
    public async Task<IActionResult> Edit(
        int id,
        FormFieldEditViewModel model,
        CancellationToken cancellationToken = default)
    {
        model.ModulKodu = NormalizeModuleCode(model.ModulKodu);

        if (id <= 0 || id != model.Id)
        {
            TempData["ErrorMessage"] = "Geçersiz form alanı.";
            return RedirectToAction(nameof(Index), new { modulKodu = model.ModulKodu });
        }

        if (string.IsNullOrWhiteSpace(model.AlanAdi))
        {
            TempData["ErrorMessage"] = "Alan adı zorunludur.";
            return View(model);
        }

        var request = new FormFieldUpdateRequest
        {
            Id = model.Id,
            AlanAdi = model.AlanAdi.Trim(),
            MinimumKarakter = model.MinimumKarakter,
            MaksimumKarakter = model.MaksimumKarakter,
            ZorunluMu = model.ZorunluMu,
            BenzersizMi = model.BenzersizMi,
            VarsayilanDeger = NormalizeNullableText(model.VarsayilanDeger),
            Aciklama = NormalizeNullableText(model.Aciklama),
            Placeholder = NormalizeNullableText(model.Placeholder),
            SiraNo = model.SiraNo,
            FormdaGorunsunMu = model.FormdaGorunsunMu,
            ListedeGorunsunMu = model.ListedeGorunsunMu,
            AramadaGorunsunMu = model.AramadaGorunsunMu,
            DetaydaGorunsunMu = model.DetaydaGorunsunMu,
            HizliKayittaGorunsunMu = model.HizliKayittaGorunsunMu,
            AktifMi = model.AktifMi
        };

        var result = await UpdateFieldSafeAsync(
            id,
            request,
            cancellationToken);

        if (result is null)
        {
            TempData["ErrorMessage"] = "Form alanı güncellenemedi.";
            return View(model);
        }

        TempData["SuccessMessage"] = "Form alanı başarıyla güncellendi.";

        return RedirectToAction(nameof(Index), new { modulKodu = model.ModulKodu });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [PermissionAuthorize(PermissionCodes.FormFieldsManage)]
    public async Task<IActionResult> SetActive(
        int id,
        string modulKodu,
        bool aktifMi,
        CancellationToken cancellationToken = default)
    {
        modulKodu = NormalizeModuleCode(modulKodu);

        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Geçersiz form alanı.";
            return RedirectToAction(nameof(Index), new { modulKodu });
        }

        var result = await SetActiveSafeAsync(
            id,
            aktifMi,
            cancellationToken);

        if (result)
        {
            TempData["SuccessMessage"] = aktifMi
                ? "Form alanı aktif edildi."
                : "Form alanı pasif edildi.";
        }
        else
        {
            TempData["ErrorMessage"] = "Form alanı durumu güncellenemedi.";
        }

        return RedirectToAction(nameof(Index), new { modulKodu });
    }

    private async Task<List<FormFieldResponse>> GetFieldsByModuleSafeAsync(
        string modulKodu,
        CancellationToken cancellationToken)
    {
        try
        {
            return await _formFieldApiService.GetByModuleAsync(
                modulKodu,
                cancellationToken) ?? new List<FormFieldResponse>();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Form alanları alınırken hata oluştu. ModulKodu: {ModulKodu}",
                modulKodu);

            TempData["ErrorMessage"] = "Form alanları alınamadı.";
            return new List<FormFieldResponse>();
        }
    }

    private async Task<object?> CreateFieldSafeAsync(
        FormFieldCreateRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            return await _formFieldApiService.CreateAsync(
                request,
                cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Form alanı oluşturulurken hata oluştu. ModulKodu: {ModulKodu}, AlanKodu: {AlanKodu}",
                request.ModulKodu,
                request.AlanKodu);

            return null;
        }
    }

    private async Task<object?> UpdateFieldSafeAsync(
        int id,
        FormFieldUpdateRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            return await _formFieldApiService.UpdateAsync(
                id,
                request,
                cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Form alanı güncellenirken hata oluştu. Id: {Id}",
                id);

            return null;
        }
    }

    private async Task<bool> SetActiveSafeAsync(
        int id,
        bool aktifMi,
        CancellationToken cancellationToken)
    {
        try
        {
            return await _formFieldApiService.SetActiveAsync(
                id,
                aktifMi,
                cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Form alanı aktif/pasif durumu güncellenirken hata oluştu. Id: {Id}, AktifMi: {AktifMi}",
                id,
                aktifMi);

            return false;
        }
    }

    private static string NormalizeModuleCode(string? modulKodu)
    {
        if (string.IsNullOrWhiteSpace(modulKodu))
        {
            return DefaultModuleCode;
        }

        var normalizedModuleCode = modulKodu.Trim();

        return AllowedModules.Contains(normalizedModuleCode)
            ? normalizedModuleCode
            : DefaultModuleCode;
    }

    private static string? ValidateCreateModel(FormFieldCreateViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.ModulKodu))
        {
            return "Modül kodu zorunludur.";
        }

        if (string.IsNullOrWhiteSpace(model.AlanAdi))
        {
            return "Alan adı zorunludur.";
        }

        if (string.IsNullOrWhiteSpace(model.AlanKodu))
        {
            return "Alan kodu zorunludur.";
        }

        if (string.IsNullOrWhiteSpace(model.AlanTipi))
        {
            return "Alan tipi zorunludur.";
        }

        return null;
    }

    private static string? NormalizeNullableText(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}