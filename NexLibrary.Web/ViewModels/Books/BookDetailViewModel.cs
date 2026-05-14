using NexLibrary.Contracts.Books;
using NexLibrary.Contracts.BookCopies;

namespace NexLibrary.Web.ViewModels.Books;

public sealed class BookDetailViewModel
{
    public BookDetailResponse Book { get; set; } = new();

    public List<BookCopyListResponse> Copies { get; set; } = new();
}