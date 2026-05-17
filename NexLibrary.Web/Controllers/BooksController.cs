using Microsoft.AspNetCore.Mvc;
using NexLibrary.Contracts.BookCopies;
using NexLibrary.Contracts.Books;
using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.DynamicForms;
using NexLibrary.Contracts.Permissions;
using NexLibrary.Web.Security;
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

    [PermissionAuthorize(PermissionCodes.BooksView)]
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
    [PermissionAuthorize(PermissionCodes.BooksCreate)]
    public async Task<IActionResult> Create(CancellationToken cancellationToken = default)
    {
        var fields = await GetVisibleBookFieldsAsync(cancellationToken);

        var model = new BookCreateViewModel
        {
            Fields = fields
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [PermissionAuthorize(PermissionCodes.BooksCreate)]
    public async Task<IActionResult> Create(
        BookCreateViewModel model,
        CancellationToken cancellationToken = default)
    {
        var visibleFields = await GetVisibleBookFieldsAsync(cancellationToken);
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

    [HttpGet]
    [PermissionAuthorize(PermissionCodes.BooksEdit)]
    public async Task<IActionResult> Edit(
        int id,
        CancellationToken cancellationToken = default)
    {
        var model = await CreateEditModelAsync(id, cancellationToken);

        if (model is null)
        {
            TempData["ErrorMessage"] = "Düzenlenecek kitap bulunamadı.";
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [PermissionAuthorize(PermissionCodes.BooksEdit)]
    public async Task<IActionResult> Edit(
        int id,
        BookEditViewModel model,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0 || id != model.Id)
        {
            TempData["ErrorMessage"] = "Geçersiz kitap bilgisi.";
            return RedirectToAction(nameof(Index));
        }

        var allActiveFields = await GetAllActiveBookFieldsAsync(cancellationToken);

        var visibleFields = allActiveFields
            .Where(x => x.FormdaGorunsunMu)
            .OrderBy(x => x.SiraNo)
            .ThenBy(x => x.Id)
            .ToList();

        model.Fields = visibleFields;

        if (string.IsNullOrWhiteSpace(model.KitapAdi))
        {
            TempData["ErrorMessage"] = "Kitap adı zorunludur.";
            return View(model);
        }

        var currentBook = await _bookApiService.GetByIdAsync(id, cancellationToken);

        if (currentBook is null)
        {
            TempData["ErrorMessage"] = "Güncellenecek kitap bulunamadı.";
            return RedirectToAction(nameof(Index));
        }

        var currentDynamicValues = currentBook.DinamikAlanlar is null
            ? new Dictionary<string, string?>()
            : currentBook.DinamikAlanlar.ToDictionary(
                x => x.AlanKodu,
                x => x.Deger);

        var request = new BookUpdateRequest
        {
            Id = id,
            KitapAdi = model.KitapAdi.Trim(),
            DinamikAlanlar = new List<DynamicFieldValueRequest>()
        };

        foreach (var field in allActiveFields.Where(x => !x.SistemAlaniMi))
        {
            var key = $"DynamicFields[{field.AlanKodu}]";
            string? value;

            if (field.FormdaGorunsunMu)
            {
                value = Request.Form[key].ToString();

                if (field.AlanTipi == "EvetHayir")
                {
                    value = Request.Form.ContainsKey(key) ? "true" : "false";
                }
            }
            else
            {
                currentDynamicValues.TryGetValue(field.AlanKodu, out value);
            }

            request.DinamikAlanlar.Add(new DynamicFieldValueRequest
            {
                AlanKodu = field.AlanKodu,
                Deger = string.IsNullOrWhiteSpace(value) ? null : value.Trim()
            });
        }

        var result = await _bookApiService.UpdateAsync(
            id,
            request,
            cancellationToken);

        if (result is null)
        {
            model.DynamicValues = BuildPostedDynamicValues(visibleFields);
            TempData["ErrorMessage"] = "Kitap güncellenemedi. Zorunlu alanları veya benzersiz alanları kontrol edin.";
            return View(model);
        }

        TempData["SuccessMessage"] = "Kitap başarıyla güncellendi.";

        return RedirectToAction(nameof(Index));
    }

    [PermissionAuthorize(PermissionCodes.BooksView)]
    public async Task<IActionResult> Details(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Geçersiz kitap bilgisi.";
            return RedirectToAction(nameof(Index));
        }

        var book = await _bookApiService.GetByIdAsync(id, cancellationToken);

        if (book is null)
        {
            TempData["ErrorMessage"] = "Kitap bulunamadı.";
            return RedirectToAction(nameof(Index));
        }

        var copies = new List<BookCopyListResponse>();

        try
        {
            var bookCopyApiService = HttpContext.RequestServices
                .GetRequiredService<BookCopyApiService>();

            copies = await bookCopyApiService.GetByBookIdAsync(
                id,
                cancellationToken);
        }
        catch
        {
            copies = new List<BookCopyListResponse>();
        }

        var model = new BookDetailViewModel
        {
            Book = book,
            Copies = copies
        };

        return View(model);
    }

    private async Task<BookEditViewModel?> CreateEditModelAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var book = await _bookApiService.GetByIdAsync(id, cancellationToken);

        if (book is null)
        {
            return null;
        }

        var fields = await GetVisibleBookFieldsAsync(cancellationToken);

        var dynamicValues = book.DinamikAlanlar is null
            ? new Dictionary<string, string?>()
            : book.DinamikAlanlar.ToDictionary(
                x => x.AlanKodu,
                x => x.Deger);

        return new BookEditViewModel
        {
            Id = book.Id,
            KitapAdi = book.KitapAdi,
            Fields = fields,
            DynamicValues = dynamicValues
        };
    }

    private async Task<List<FormFieldResponse>> GetVisibleBookFieldsAsync(
        CancellationToken cancellationToken)
    {
        var fields = await _formFieldApiService.GetByModuleAsync(
            "Kitaplar",
            cancellationToken);

        return fields
            .Where(x => x.AktifMi && x.FormdaGorunsunMu)
            .OrderBy(x => x.SiraNo)
            .ThenBy(x => x.Id)
            .ToList();
    }

    private async Task<List<FormFieldResponse>> GetAllActiveBookFieldsAsync(
        CancellationToken cancellationToken)
    {
        var fields = await _formFieldApiService.GetByModuleAsync(
            "Kitaplar",
            cancellationToken);

        return fields
            .Where(x => x.AktifMi)
            .OrderBy(x => x.SiraNo)
            .ThenBy(x => x.Id)
            .ToList();
    }

    private Dictionary<string, string?> BuildPostedDynamicValues(
        List<FormFieldResponse> fields)
    {
        var values = new Dictionary<string, string?>();

        foreach (var field in fields.Where(x => !x.SistemAlaniMi))
        {
            var key = $"DynamicFields[{field.AlanKodu}]";
            var value = Request.Form[key].ToString();

            if (field.AlanTipi == "EvetHayir")
            {
                value = Request.Form.ContainsKey(key) ? "true" : "false";
            }

            values[field.AlanKodu] = string.IsNullOrWhiteSpace(value)
                ? null
                : value.Trim();
        }

        return values;
    }
}