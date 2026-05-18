using Microsoft.AspNetCore.Mvc;
using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.DynamicForms;
using NexLibrary.Contracts.Loans;
using NexLibrary.Contracts.Members;
using NexLibrary.Contracts.Permissions;
using NexLibrary.Web.Security;
using NexLibrary.Web.Services;
using NexLibrary.Web.ViewModels.Members;

namespace NexLibrary.Web.Controllers;

public sealed class MembersController : Controller
{
    private const string MembersModuleName = "Uyeler";
    private const string BooleanFieldType = "EvetHayir";

    private readonly MemberApiService _memberApiService;
    private readonly FormFieldApiService _formFieldApiService;
    private readonly LoanApiService _loanApiService;
    private readonly ILogger<MembersController> _logger;

    public MembersController(
        MemberApiService memberApiService,
        FormFieldApiService formFieldApiService,
        LoanApiService loanApiService,
        ILogger<MembersController> logger)
    {
        _memberApiService = memberApiService;
        _formFieldApiService = formFieldApiService;
        _loanApiService = loanApiService;
        _logger = logger;
    }

    [PermissionAuthorize(PermissionCodes.MembersView)]
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
    [PermissionAuthorize(PermissionCodes.MembersCreate)]
    public async Task<IActionResult> Create(CancellationToken cancellationToken = default)
    {
        var fields = await GetVisibleMemberFieldsAsync(cancellationToken);

        var model = new MemberCreateViewModel
        {
            Fields = fields
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [PermissionAuthorize(PermissionCodes.MembersCreate)]
    public async Task<IActionResult> Create(
        MemberCreateViewModel model,
        CancellationToken cancellationToken = default)
    {
        var visibleFields = await GetVisibleMemberFieldsAsync(cancellationToken);
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
            request.DinamikAlanlar.Add(new DynamicFieldValueRequest
            {
                AlanKodu = field.AlanKodu,
                Deger = GetDynamicFieldValue(field)
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

    [HttpGet]
    [PermissionAuthorize(PermissionCodes.MembersEdit)]
    public async Task<IActionResult> Edit(
        int id,
        CancellationToken cancellationToken = default)
    {
        var model = await CreateEditModelAsync(id, cancellationToken);

        if (model is null)
        {
            TempData["ErrorMessage"] = "Düzenlenecek üye bulunamadı.";
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [PermissionAuthorize(PermissionCodes.MembersEdit)]
    public async Task<IActionResult> Edit(
        int id,
        MemberEditViewModel model,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0 || id != model.Id)
        {
            TempData["ErrorMessage"] = "Geçersiz üye bilgisi.";
            return RedirectToAction(nameof(Index));
        }

        var allActiveFields = await GetAllActiveMemberFieldsAsync(cancellationToken);

        var visibleFields = allActiveFields
            .Where(x => x.FormdaGorunsunMu)
            .OrderBy(x => x.SiraNo)
            .ThenBy(x => x.Id)
            .ToList();

        model.Fields = visibleFields;

        if (string.IsNullOrWhiteSpace(model.UyeAdiSoyadi))
        {
            TempData["ErrorMessage"] = "Üye adı soyadı zorunludur.";
            return View(model);
        }

        var currentMember = await _memberApiService.GetByIdAsync(
            id,
            cancellationToken);

        if (currentMember is null)
        {
            TempData["ErrorMessage"] = "Güncellenecek üye bulunamadı.";
            return RedirectToAction(nameof(Index));
        }

        var currentDynamicValues = currentMember.DinamikAlanlar is null
            ? new Dictionary<string, string?>()
            : currentMember.DinamikAlanlar.ToDictionary(
                x => x.AlanKodu,
                x => x.Deger);

        var request = new MemberUpdateRequest
        {
            Id = id,
            UyeAdiSoyadi = model.UyeAdiSoyadi.Trim(),
            DinamikAlanlar = new List<DynamicFieldValueRequest>()
        };

        foreach (var field in allActiveFields.Where(x => !x.SistemAlaniMi))
        {
            string? value;

            if (field.FormdaGorunsunMu)
            {
                value = GetDynamicFieldValue(field);
            }
            else
            {
                currentDynamicValues.TryGetValue(field.AlanKodu, out value);
                value = NormalizeDynamicValue(value);
            }

            request.DinamikAlanlar.Add(new DynamicFieldValueRequest
            {
                AlanKodu = field.AlanKodu,
                Deger = value
            });
        }

        var result = await _memberApiService.UpdateAsync(
            id,
            request,
            cancellationToken);

        if (result is null)
        {
            model.DynamicValues = BuildPostedDynamicValues(visibleFields);
            TempData["ErrorMessage"] = "Üye güncellenemedi. Zorunlu alanları veya benzersiz alanları kontrol edin.";
            return View(model);
        }

        TempData["SuccessMessage"] = "Üye başarıyla güncellendi.";

        return RedirectToAction(nameof(Index));
    }

    [PermissionAuthorize(PermissionCodes.MembersView)]
    public async Task<IActionResult> Details(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Geçersiz üye bilgisi.";
            return RedirectToAction(nameof(Index));
        }

        var member = await _memberApiService.GetByIdAsync(id, cancellationToken);

        if (member is null)
        {
            TempData["ErrorMessage"] = "Üye bulunamadı.";
            return RedirectToAction(nameof(Index));
        }

        var loans = new List<LoanListResponse>();

        try
        {
            var loanResult = await _loanApiService.GetPagedAsync(
                1,
                1000,
                member.UyeAdiSoyadi,
                cancellationToken);

            loans = loanResult?.Items
                .Where(x => x.UyeId == member.Id)
                .OrderByDescending(x => x.Id)
                .ToList() ?? new List<LoanListResponse>();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Üye ödünç geçmişi alınamadı. UyeId: {UyeId}",
                member.Id);

            loans = new List<LoanListResponse>();
        }

        var model = new MemberDetailViewModel
        {
            Member = member,
            Loans = loans
        };

        return View(model);
    }

    private async Task<MemberEditViewModel?> CreateEditModelAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var member = await _memberApiService.GetByIdAsync(
            id,
            cancellationToken);

        if (member is null)
        {
            return null;
        }

        var fields = await GetVisibleMemberFieldsAsync(cancellationToken);

        var dynamicValues = member.DinamikAlanlar is null
            ? new Dictionary<string, string?>()
            : member.DinamikAlanlar.ToDictionary(
                x => x.AlanKodu,
                x => x.Deger);

        return new MemberEditViewModel
        {
            Id = member.Id,
            UyeAdiSoyadi = member.UyeAdiSoyadi,
            Fields = fields,
            DynamicValues = dynamicValues
        };
    }

    private async Task<List<FormFieldResponse>> GetVisibleMemberFieldsAsync(
        CancellationToken cancellationToken)
    {
        var fields = await _formFieldApiService.GetByModuleAsync(
            MembersModuleName,
            cancellationToken);

        return fields
            .Where(x => x.AktifMi && x.FormdaGorunsunMu)
            .OrderBy(x => x.SiraNo)
            .ThenBy(x => x.Id)
            .ToList();
    }

    private async Task<List<FormFieldResponse>> GetAllActiveMemberFieldsAsync(
        CancellationToken cancellationToken)
    {
        var fields = await _formFieldApiService.GetByModuleAsync(
            MembersModuleName,
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
            values[field.AlanKodu] = GetDynamicFieldValue(field);
        }

        return values;
    }

    private string? GetDynamicFieldValue(FormFieldResponse field)
    {
        var key = $"DynamicFields[{field.AlanKodu}]";

        if (field.AlanTipi == BooleanFieldType)
        {
            return Request.Form.ContainsKey(key) ? "true" : "false";
        }

        var value = Request.Form[key].ToString();

        return NormalizeDynamicValue(value);
    }

    private static string? NormalizeDynamicValue(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}