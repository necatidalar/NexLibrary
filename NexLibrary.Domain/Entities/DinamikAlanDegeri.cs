using NexLibrary.Domain.Common;
using NexLibrary.Domain.Enums;

namespace NexLibrary.Domain.Entities
{
    public sealed class DinamikAlanDegeri : BaseEntity
    {
        public ModulKodu ModulKodu { get; set; }

        public int KayitId { get; set; }

        public int FormAlaniId { get; set; }

        public FormAlani FormAlani { get; set; } = null!;

        public string? DegerMetin { get; set; }

        public decimal? DegerSayi { get; set; }

        public DateTime? DegerTarih { get; set; }

        public bool? DegerBool { get; set; }

        public string? DegerJson { get; set; }
    }
}
