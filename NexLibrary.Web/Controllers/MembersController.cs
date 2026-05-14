using Microsoft.AspNetCore.Mvc;
using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.DynamicForms;
using NexLibrary.Contracts.Members;
using NexLibrary.Web.Services;
using NexLibrary.Web.ViewModels.Members;

namespace NexLibrary.Web.Controllers;

public sealed class MembersController : Controller
{
    private readonly MemberApiService _memberApiService;
    private readonly FormFieldApiService _formFieldApiService;

    public MembersController(
        MemberApiService memberApiService,
        FormFieldApiService formFieldApiService)
    {
        _memberApiService = memberApiService;
        _formFieldApiService = formFieldApiService;
    }

    public async Task<IActionResult> Index(
        string? search = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 20 : pageSize;

        var members = await _memberApiService.GetPagedAsync(
            pageNumber,
            pageSize,
            search,
            cancellationToken);

        if (members is null)
        {
            ViewBag.ErrorMessage = "API bağlantısı kurulamadı veya üye listesi alınamadı.";

            members = new PagedResponse<MemberListResponse>
            {
                Items = new List<MemberListResponse>(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = 0
            };
        }

        var model = new MembersIndexViewModel
        {
            Search = search,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Members = members
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken = default)
    {
        var fields = await _formFieldApiService.GetByModuleAsync(
            "Uyeler",
            cancellationToken);

        var model = new MemberCreateViewModel
        {
            Fields = fields
                .Where(x => x.AktifMi && x.FormdaGorunsunMu)
                .OrderBy(x => x.SiraNo)
                .ThenBy(x => x.Id)
                .ToList()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        MemberCreateViewModel model,
        CancellationToken cancellationToken = default)
    {
        var fields = await _formFieldApiService.GetByModuleAsync(
            "Uyeler",
            cancellationToken);

        var visibleFields = fields
            .Where(x => x.AktifMi && x.FormdaGorunsunMu)
            .OrderBy(x => x.SiraNo)
            .ThenBy(x => x.Id)
            .ToList();

        model.Fields = visibleFields;

        if (string.IsNullOrWhiteSpace(model.UyeAdiSoyadi))
        {
            TempData["ErrorMessage"] = "Üye adı soyadı zorunludur.";
            return View(model);
        }

        var request = new MemberCreateRequest
        {
            UyeAdiSoyadi = model.UyeAdiSoyadi.Trim(),
            DinamikAlanlar = new List<DynamicFieldValueRequest>()
        };

        foreach (var field in visibleFields.Where(x => !x.SistemAlaniMi))
        {
            var key = $"DynamicFields[{field.AlanKodu}]";

            var value = Request.Form[key].ToString();

            if (field.AlanTipi == "EvetHayir")
            {
                value = Request.Form.ContainsKey(key) ? "true" : "false";
            }

            request.DinamikAlanlar.Add(new DynamicFieldValueRequest
            {
                AlanKodu = field.AlanKodu,
                Deger = string.IsNullOrWhiteSpace(value) ? null : value.Trim()
            });
        }

        var result = await _memberApiService.CreateAsync(
            request,
            cancellationToken);

        if (result is null)
        {
            TempData["ErrorMessage"] = "Üye kaydedilemedi. Zorunlu alanları veya benzersiz alanları kontrol edin.";
            return View(model);
        }

        TempData["SuccessMessage"] = "Üye başarıyla eklendi.";

        return RedirectToAction(nameof(Index));
    }
}