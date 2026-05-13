using NexLibrary.Domain.Common;
using NexLibrary.Domain.Enums;

namespace NexLibrary.Domain.Entities;

public sealed class KitapKopya : BaseEntity
{
    public int KitapId { get; set; }

    public Kitap Kitap { get; set; } = null!;

    public string Barkod { get; set; } = string.Empty;

    public string? DemirbasNo { get; set; }

    public KitapKopyaDurumu Durum { get; set; } = KitapKopyaDurumu.Musait;

    public string? Aciklama { get; set; }
}