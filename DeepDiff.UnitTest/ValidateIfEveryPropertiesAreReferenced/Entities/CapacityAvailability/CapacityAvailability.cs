using DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Seas;
using System.Collections.Generic;
using System.Diagnostics;

namespace DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.CapacityAvailability
{
    [DebuggerDisplay("{DebuggerDisplay, nq}")]
    public class CapacityAvailability : CreateAuditEntity
    {
        public string ContractReference { get; set; } = null!;
        public Date Day { get; set; }

        // one-to-many
        public List<CapacityAvailabilityDetail> CapacityAvailabilityDetails { get; set; } = null!;

        // one-to-zero/one
        public ForcedOutagePeriod ForcedOutagePeriod { get; set; } = null!;

        private string DebuggerDisplay => $"{ContractReference} {Day} {Id}";
    }
}