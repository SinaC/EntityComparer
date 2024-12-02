using DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Arc4u;
using System;
using System.Diagnostics;

namespace DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.CapacityAvailability
{
    [DebuggerDisplay("{DebuggerDisplay, nq}")]
    public class CapacityAvailabilityCctuDetail : IdEntity
    {
        public DateTime StartsOn { get; set; }

        public decimal AwardedVolume { get; set; } // from Star thru Integration Hub
        public decimal ExchangedVolume { get; set; } // from SMART
        public decimal NominatedVolume { get; set; } // from BMAP
        public decimal MissingVolume { get; set; } // Math.Max(AwardedVolume + ExchangedVolume - NominatedVolume, 0)
        public ForcedOutageStatus ForcedOutageStatus { get; set; }
        public ForcedOutagePeriodStatus ForcedOutagePeriodStatus { get; set; }

        public decimal ObligationVolume => AwardedVolume + ExchangedVolume;

        // FK
        public Guid CapacityAvailabilityCctuId { get; set; }
        public CapacityAvailabilityCctu CapacityAvailabilityCctu { get; set; } = null!;

        private string DebuggerDisplay => $"{StartsOn} AV:{AwardedVolume} EV:{ExchangedVolume} OV:{ObligationVolume} NV:{NominatedVolume} MV:{MissingVolume} FOS:{ForcedOutageStatus} FOPS:{ForcedOutagePeriodStatus} {Id}";
    }
}
