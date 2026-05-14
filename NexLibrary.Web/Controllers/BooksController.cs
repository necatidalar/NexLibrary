using Microsoft.AspNetCore.Mvc;
using NexLibrary.Contracts.Books;
using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.DynamicForms;
using NexLibrary.Web.Services;
using NexLibrary.Web.ViewModels.Books;

namespace NexLibrary.Web.Controllers;

public sealed class BooksController : Controller
{
    private readonly BookApiService _bookApiService;
    private readonly FormFieldApiService _formFieldApiService;

    public BooksController(
        BookApiService bookApiService,
        FormFieldApiService formFieldApiService)
    {
        _bookApiService = bookApiService;
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

        var books = await _bookApiService.GetPagedAsync(
            pageNumber,
            pageSize,
            search,
            cancellationToken);

        if (books is null)
        {
            ViewBag.ErrorMessage = "API bağlantısı kurulamadı veya kitap listesi alınamadı.";

            books = new PagedResponse<BookListResponse>
            {
                Items = new List<BookListResponse>(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = 0
            };
        }

        var model = new BooksIndexViewModel
        {
            Search = search,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Books = books
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken = default)
    {
        var fields = await _formFieldApiService.GetByModuleAsync(
            "Kitaplar",
            cancellationToken);

        var model = new BookCreateViewModel
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
        BookCreateViewModel model,
        CancellationToken cancellationToken = default)
    {
        var fields = await _formFieldApiService.GetByModuleAsync(
            "Kitaplar",
            cancellationToken);

        var visibleFields = fields
            .Where(x => x.AktifMi && x.FormdaGorunsunMu)
            .OrderBy(x => x.SiraNo)
            .ThenBy(x => x.Id)
            .ToList();

        model.Fields = visibleFields;

        if (string.IsNullOrWhiteSpace(model.KitapAdi))
        {
            TempData["ErrorMessage"] = "Kitap adı zorunludur.";
            return View(model);
        }

        var request = new BookCreateRequest
        {
            KitapAdi = model.KitapAdi.Trim(),
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

        var result = await _bookApiService.CreateAsync(
            request,
            cancellationToken);

        if (result is null)
        {
            TempData["ErrorMessage"] = "Kitap kaydedilemedi. Zorunlu alanları veya benzersiz alanları kontrol edin.";
            return View(model);
        }

        TempData["SuccessMessage"] = "Kitap başarıyla eklendi.";

        return RedirectToAction(nameof(Index));
    }
}