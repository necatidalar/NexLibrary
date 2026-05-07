using NexLibrary.Domain.Common;

namespace NexLibrary.Domain.Entities
{
    public sealed class AuditLog : BaseEntity
    {
        public string IslemTuru { get; set; } = string.Empty;

        public string TabloAdi { get; set; } = string.Empty;

        public int? KayitId { get; set; }

        public string? EskiDegerJson { get; set; }

        public string? YeniDegerJson { get; set; }

        public string? Aciklama { get; set; }

        public int? KullaniciId { get; set; }

        public string? IpAdresi { get; set; }
    }
}
