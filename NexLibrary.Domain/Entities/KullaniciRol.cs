using NexLibrary.Domain.Common;

namespace NexLibrary.Domain.Entities
{
    public sealed class KullaniciRol : BaseEntity
    {
        public int KullaniciId { get; set; }

        public Kullanici Kullanici { get; set; } = null!;

        public int RolId { get; set; }

        public Rol Rol { get; set; } = null!;
    }
}
