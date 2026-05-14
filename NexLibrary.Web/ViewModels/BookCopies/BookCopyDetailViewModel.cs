using NexLibrary.Contracts.BookCopies;

namespace NexLibrary.Web.ViewModels.BookCopies;

public sealed class BookCopyDetailViewModel
{
    public BookCopyListResponse Copy { get; set; } = new();

    public List<BookCopyListResponse> OtherCopies { get; set; } = new();
}