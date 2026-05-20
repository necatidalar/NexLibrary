using NexLibrary.Domain.Common;

namespace NexLibrary.Domain.Entities;

public sealed class ApiClient : BaseEntity
{
    public string ClientId { get; set; } = string.Empty;

    public string ClientName { get; set; } = string.Empty;

    public string ClientSecretHash { get; set; } = string.Empty;

    public string ClientSecretSalt { get; set; } = string.Empty;

    public string? Aciklama { get; set; }

    public DateTime? SonKullanimTarihi { get; set; }

    public ICollection<ApiClientYetki> ApiClientYetkileri { get; set; } = new List<ApiClientYetki>();
}