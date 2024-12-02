using DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Arc4u;
using System;
using System.Diagnostics;

namespace DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.CapacityAvailability
{
    [DebuggerDisplay("{DebuggerDisplay, nq}")]
    public class ForcedOutagePeriod : IdEntity
    {
        public DateTime FirstPeriodStart { get; set; }
        public DateTime FirstPeriodEnd { get; set; }
        public DateTime SecondPeriodStart { get; set; }
        public DateTime SecondPeriodEnd { get; set; }

        // FK to CapacityAvailability
        public Guid CapacityAvailabilityId { get; set; }
        public CapacityAvailability CapacityAvailability { get; set; } = null!;

        private string DebuggerDisplay => $"P1:{FirstPeriodStart}->{FirstPeriodEnd} PS:{SecondPeriodStart}->{SecondPeriodEnd} {Id}";
    }
}
