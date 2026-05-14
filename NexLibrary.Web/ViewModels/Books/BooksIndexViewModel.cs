using NexLibrary.Contracts.Books;
using NexLibrary.Contracts.Common;

namespace NexLibrary.Web.ViewModels.Books;

public sealed class BooksIndexViewModel
{
    public string? Search { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;

    public PagedResponse<BookListResponse> Books { get; set; } = new();

    public List<int> PageSizeOptions { get; set; } = new()
    {
        10,
        20,
        50,
        100
    };
}