using NexLibrary.Domain.Common;
using NexLibrary.Domain.Enums;

namespace NexLibrary.Domain.Entities;

public sealed class Odunc : BaseEntity
{
    public int KitapId { get; set; }

    public Kitap Kitap { get; set; } = null!;

    public int UyeId { get; set; }

    public Uye Uye { get; set; } = null!;

    public DateTime VerilisTarihi { get; set; }

    public DateTime PlanlananIadeTarihi { get; set; }

    public DateTime? IadeTarihi { get; set; }

    public OduncDurumu Durum { get; set; } = OduncDurumu.Oduncte;

    public string? Aciklama { get; set; }
}