using Microsoft.AspNetCore.Mvc;
using NexLibrary.Contracts.DynamicForms;
using NexLibrary.Contracts.Permissions;
using NexLibrary.Web.Security;
using NexLibrary.Web.Services;
using NexLibrary.Web.ViewModels.FormFields;

namespace NexLibrary.Web.Controllers;

public sealed class FormFieldsController : Controller
{
    private readonly FormFieldApiService _formFieldApiService;

    public FormFieldsController(FormFieldApiService formFieldApiService)
    {
        _formFieldApiService = formFieldApiService;
    }

    [PermissionAuthorize(PermissionCodes.FormFieldsView)]
    public async Task<IActionResult> Index(
        string modulKodu = "Kitaplar",
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(modulKodu))
        {
            modulKodu = "Kitaplar";
        }

        var fields = await _formFieldApiService.GetByModuleAsync(
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
    [PermissionAuthorize(PermissionCodes.FormFieldsEdit)]
    public async Task<IActionResult> Create(
        string modulKodu = "Kitaplar",
        CancellationToken cancellationToken = default)
    {
        var existingFields = await _formFieldApiService.GetByModuleAsync(
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
    [PermissionAuthorize(PermissionCodes.FormFieldsEdit)]
    public async Task<IActionResult> Create(
        FormFieldCreateViewModel model,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(model.ModulKodu))
        {
            TempData["ErrorMessage"] = "Modül kodu zorunludur.";
            return View(model);
        }

        if (string.IsNullOrWhiteSpace(model.AlanAdi))
        {
            TempData["ErrorMessage"] = "Alan adı zorunludur.";
            return View(model);
        }

        if (string.IsNullOrWhiteSpace(model.AlanKodu))
        {
            TempData["ErrorMessage"] = "Alan kodu zorunludur.";
            return View(model);
        }

        if (string.IsNullOrWhiteSpace(model.AlanTipi))
        {
            TempData["ErrorMessage"] = "Alan tipi zorunludur.";
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
            VarsayilanDeger = string.IsNullOrWhiteSpace(model.VarsayilanDeger)
                ? null
                : model.VarsayilanDeger.Trim(),
            Aciklama = string.IsNullOrWhiteSpace(model.Aciklama)
                ? null
                : model.Aciklama.Trim(),
            Placeholder = string.IsNullOrWhiteSpace(model.Placeholder)
                ? null
                : model.Placeholder.Trim(),
            SiraNo = model.SiraNo,
            FormdaGorunsunMu = model.FormdaGorunsunMu,
            ListedeGorunsunMu = model.ListedeGorunsunMu,
            AramadaGorunsunMu = model.AramadaGorunsunMu,
            DetaydaGorunsunMu = model.DetaydaGorunsunMu,
            HizliKayittaGorunsunMu = model.HizliKayittaGorunsunMu
        };

        var result = await _formFieldApiService.CreateAsync(
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
        string modulKodu = "Kitaplar",
        CancellationToken cancellationToken = default)
    {
        var fields = await _formFieldApiService.GetByModuleAsync(
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
            VarsayilanDeger = string.IsNullOrWhiteSpace(model.VarsayilanDeger)
                ? null
                : model.VarsayilanDeger.Trim(),
            Aciklama = string.IsNullOrWhiteSpace(model.Aciklama)
                ? null
                : model.Aciklama.Trim(),
            Placeholder = string.IsNullOrWhiteSpace(model.Placeholder)
                ? null
                : model.Placeholder.Trim(),
            SiraNo = model.SiraNo,
            FormdaGorunsunMu = model.FormdaGorunsunMu,
            ListedeGorunsunMu = model.ListedeGorunsunMu,
            AramadaGorunsunMu = model.AramadaGorunsunMu,
            DetaydaGorunsunMu = model.DetaydaGorunsunMu,
            HizliKayittaGorunsunMu = model.HizliKayittaGorunsunMu,
            AktifMi = model.AktifMi
        };

        var result = await _formFieldApiService.UpdateAsync(
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
    [PermissionAuthorize(PermissionCodes.FormFieldsEdit)]
    public async Task<IActionResult> SetActive(
        int id,
        string modulKodu,
        bool aktifMi,
        CancellationToken cancellationToken = default)
    {
        var result = await _formFieldApiService.SetActiveAsync(
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
}