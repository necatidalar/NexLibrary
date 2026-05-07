namespace NexLibrary.Domain.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public bool AktifMi { get; set; } = true;

        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? GuncellemeTarihi { get; set; }

        public int? OlusturanKullaniciId { get; set; }

        public int? GuncelleyenKullaniciId { get; set; }
    }
}