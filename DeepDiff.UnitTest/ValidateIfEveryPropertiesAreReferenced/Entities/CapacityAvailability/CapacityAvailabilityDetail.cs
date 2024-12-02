using DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Seas;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.CapacityAvailability
{
    [DebuggerDisplay("{DebuggerDisplay, nq}")]
    public class CapacityAvailabilityDetail : AuditEntity
    {
        public Direction Direction { get; set; }

        public int CctuNonCompliant { get; set; } // calculated based on current day cctus penalty missing volume and cctu detail missing volume
        public int CctuNonCompliantInPreviousDays { get; set; } // calculated based on current and previous days cctus penalty missing volume and cctu details missing volume
        public decimal WeightedCapacityPrice { get; set; } // calculated based on current and previous days cctus penalty missing volume and cctu details missing volume

        public bool ToBeRecomputed { get; set; }

        // one-to-many
        public List<CapacityAvailabilityCctu> CapacityAvailabilityCctus { get; set; } = null!;

        // FK to CapacityAvailability
        public Guid CapacityAvailabilityId { get; set; }
        public CapacityAvailability CapacityAvailability { get; set; } = null!;

        private string DebuggerDisplay => $"{Direction} #CCTU:{CctuNonCompliant} #CCTU-:{CctuNonCompliantInPreviousDays} WCP:{WeightedCapacityPrice} TBR?:{ToBeRecomputed} {Id}";
    }
}