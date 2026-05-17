using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexLibrary.Contracts.DynamicForms;
using NexLibrary.Web.Services;
using NexLibrary.Web.ViewModels.FormFields;

namespace NexLibrary.Web.Controllers;

[Authorize(Roles = "ADMIN")]
public sealed class FormFieldsController : Controller
{
    private readonly FormFieldApiService _formFieldApiService;

    public FormFieldsController(FormFieldApiService formFieldApiService)
    {
        _formFieldApiService = formFieldApiService;
    }

    public async Task<IActionResult> Index(
        string modulKodu = "Kitaplar",
        CancellationToken cancellationToken = default)
    {
        var model = await CreateIndexModelAsync(modulKodu, cancellationToken);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        FormFieldsIndexViewModel model,
        CancellationToken cancellationToken = default)
    {
        var request = model.NewField;
        request.ModulKodu = string.IsNullOrWhiteSpace(request.ModulKodu)
            ? model.ModulKodu
            : request.ModulKodu;

        request.AlanAdi = request.AlanAdi?.Trim() ?? string.Empty;
        request.AlanKodu = request.AlanKodu?.Trim().ToUpperInvariant() ?? string.Empty;
        request.AlanTipi = string.IsNullOrWhiteSpace(request.AlanTipi)
            ? "Metin"
            : request.AlanTipi;

        if (string.IsNullOrWhiteSpace(request.AlanAdi))
        {
            TempData["ErrorMessage"] = "Alan adı zorunludur.";
            return RedirectToAction(nameof(Index), new { modulKodu = request.ModulKodu });
        }

        if (string.IsNullOrWhiteSpace(request.AlanKodu))
        {
            TempData["ErrorMessage"] = "Alan kodu zorunludur.";
            return RedirectToAction(nameof(Index), new { modulKodu = request.ModulKodu });
        }

        var result = await _formFieldApiService.CreateAsync(request, cancellationToken);

        if (result is null)
        {
            TempData["ErrorMessage"] = "Form alanı oluşturulamadı. Alan kodu daha önce kullanılmış olabilir.";
        }
        else
        {
            TempData["SuccessMessage"] = "Form alanı başarıyla oluşturuldu.";
        }

        return RedirectToAction(nameof(Index), new { modulKodu = request.ModulKodu });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveSettings(
        FormFieldsSaveSettingsViewModel model,
        CancellationToken cancellationToken = default)
    {
        var existingFields = await _formFieldApiService.GetByModuleAsync(
            model.ModulKodu,
            cancellationToken);

        var successCount = 0;
        var failCount = 0;

        foreach (var input in model.Fields)
        {
            var field = existingFields.FirstOrDefault(x => x.Id == input.Id);

            if (field is null)
            {
                failCount++;
                continue;
            }

            var request = new FormFieldUpdateRequest
            {
                Id = field.Id,
                AlanAdi = field.AlanAdi,
                MinimumKarakter = field.MinimumKarakter,
                MaksimumKarakter = field.MaksimumKarakter,
                ZorunluMu = field.SistemAlaniMi ? field.ZorunluMu : input.ZorunluMu,
                BenzersizMi = input.BenzersizMi,
                VarsayilanDeger = field.VarsayilanDeger,
                Aciklama = field.Aciklama,
                Placeholder = field.Placeholder,
                SiraNo = input.SiraNo <= 0 ? field.SiraNo : input.SiraNo,
                FormdaGorunsunMu = input.FormdaGorunsunMu,
                ListedeGorunsunMu = field.SistemAlaniMi ? field.ListedeGorunsunMu : input.ListedeGorunsunMu,
                AramadaGorunsunMu = input.AramadaGorunsunMu,
                DetaydaGorunsunMu = input.DetaydaGorunsunMu,
                HizliKayittaGorunsunMu = input.HizliKayittaGorunsunMu,
                AktifMi = field.SistemAlaniMi ? field.AktifMi : input.AktifMi
            };

            var result = await _formFieldApiService.UpdateAsync(
                field.Id,
                request,
                cancellationToken);

            if (result is null)
            {
                failCount++;
            }
            else
            {
                successCount++;
            }
        }

        if (failCount > 0)
        {
            TempData["ErrorMessage"] = $"{failCount} alan güncellenemedi. {successCount} alan güncellendi.";
        }
        else
        {
            TempData["SuccessMessage"] = "Form alanı ayarları başarıyla güncellendi.";
        }

        return RedirectToAction(nameof(Index), new { modulKodu = model.ModulKodu });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetActive(
        int id,
        string modulKodu,
        bool aktifMi,
        CancellationToken cancellationToken = default)
    {
        var success = await _formFieldApiService.SetActiveAsync(
            id,
            aktifMi,
            cancellationToken);

        TempData[success ? "SuccessMessage" : "ErrorMessage"] = success
            ? "Alan durumu güncellendi."
            : "Alan durumu güncellenemedi.";

        return RedirectToAction(nameof(Index), new { modulKodu });
    }

    private async Task<FormFieldsIndexViewModel> CreateIndexModelAsync(
        string modulKodu,
        CancellationToken cancellationToken)
    {
        modulKodu = string.IsNullOrWhiteSpace(modulKodu)
            ? "Kitaplar"
            : modulKodu;

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

        model.NewField.ModulKodu = modulKodu;

        return model;
    }
}