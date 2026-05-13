namespace NexLibrary.Contracts.Loans;

public sealed class LoanCreateRequest
{
    public int KitapId { get; set; }

    public int UyeId { get; set; }

    public DateTime PlanlananIadeTarihi { get; set; }

    public string? Aciklama { get; set; }
}