using NexLibrary.Domain.Common;

namespace NexLibrary.Domain.Entities
{
    public sealed class Rol : BaseEntity
    {
        public string RolKodu { get; set; } = string.Empty;

        public string RolAdi { get; set; } = string.Empty;

        public string? Aciklama { get; set; }

        public ICollection<KullaniciRol> KullaniciRolleri { get; set; } = new List<KullaniciRol>();
    }
}
