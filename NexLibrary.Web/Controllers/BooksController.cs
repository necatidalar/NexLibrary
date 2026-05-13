using Microsoft.AspNetCore.Mvc;
using NexLibrary.Contracts.Common;
using NexLibrary.Web.Services;
using NexLibrary.Web.ViewModels.Books;

namespace NexLibrary.Web.Controllers;

public sealed class BooksController : Controller
{
    private readonly BookApiService _bookApiService;

    public BooksController(BookApiService bookApiService)
    {
        _bookApiService = bookApiService;
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

            books = new PagedResponse<NexLibrary.Contracts.Books.BookListResponse>
            {
                Items = new List<NexLibrary.Contracts.Books.BookListResponse>(),
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
}