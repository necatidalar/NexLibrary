using NexLibrary.Contracts.Books;
using NexLibrary.Contracts.Members;

namespace NexLibrary.Web.ViewModels.Loans;

public sealed class LoanCreateViewModel
{
    public int KitapId { get; set; }

    public int UyeId { get; set; }

    public DateTime PlanlananIadeTarihi { get; set; } = DateTime.Today.AddDays(14);

    public string? Aciklama { get; set; }

    public List<BookListResponse> Books { get; set; } = new();

    public List<MemberListResponse> Members { get; set; } = new();
}