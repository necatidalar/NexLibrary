using Microsoft.AspNetCore.Mvc;
using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Members;
using NexLibrary.Web.Services;
using NexLibrary.Web.ViewModels.Members;

namespace NexLibrary.Web.Controllers;

public sealed class MembersController : Controller
{
    private readonly MemberApiService _memberApiService;

    public MembersController(MemberApiService memberApiService)
    {
        _memberApiService = memberApiService;
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
}