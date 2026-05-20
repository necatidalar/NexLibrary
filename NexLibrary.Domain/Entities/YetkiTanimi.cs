using NexLibrary.Domain.Common;

namespace NexLibrary.Domain.Entities;

public sealed class YetkiTanimi : BaseEntity
{
    public string ModulKodu { get; set; } = string.Empty;

    public string YetkiKodu { get; set; } = string.Empty;

    public string YetkiAdi { get; set; } = string.Empty;

    public string? Aciklama { get; set; }

    public bool MenuYetkisiMi { get; set; }

    public int SiraNo { get; set; }

    public ICollection<RolYetki> RolYetkileri { get; set; } = new List<RolYetki>();

    public ICollection<ApiClientYetki> ApiClientYetkileri { get; set; } = new List<ApiClientYetki>();
}