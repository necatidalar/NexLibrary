using NexLibrary.Domain.Common;

namespace NexLibrary.Domain.Entities;

public sealed class ApiClientYetki : BaseEntity
{
    public int ApiClientId { get; set; }

    public ApiClient ApiClient { get; set; } = null!;

    public int YetkiTanimiId { get; set; }

    public YetkiTanimi YetkiTanimi { get; set; } = null!;
}