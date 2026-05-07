using NexLibrary.Domain.Common;

namespace NexLibrary.Domain.Entities
{
    public sealed class Kitap : BaseEntity
    {
        public string KitapAdi { get; set; } = string.Empty;

        public ICollection<DinamikAlanDegeri> DinamikAlanDegerleri { get; set; } = new List<DinamikAlanDegeri>();
    }
}
