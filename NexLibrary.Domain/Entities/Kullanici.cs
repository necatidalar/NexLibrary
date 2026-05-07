using NexLibrary.Domain.Common;

namespace NexLibrary.Domain.Entities
{
    public sealed class Kullanici : BaseEntity
    {
        public string KullaniciAdi { get; set; } = string.Empty;

        public string AdSoyad { get; set; } = string.Empty;

        public string? Eposta { get; set; }

        public string? Telefon { get; set; }

        public string SifreHash { get; set; } = string.Empty;

        public string? SifreSalt { get; set; }

        public DateTime? SonGirisTarihi { get; set; }

        public ICollection<KullaniciRol> KullaniciRolleri { get; set; } = new List<KullaniciRol>();
    }
}
