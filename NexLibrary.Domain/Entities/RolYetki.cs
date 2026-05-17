using NexLibrary.Domain.Common;

namespace NexLibrary.Domain.Entities;

public sealed class RolYetki : BaseEntity
{
    public int RolId { get; set; }

    public Rol Rol { get; set; } = null!;

    public int YetkiTanimiId { get; set; }

    public YetkiTanimi YetkiTanimi { get; set; } = null!;
}