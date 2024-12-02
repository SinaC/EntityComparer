using DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Arc4u;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.ActivationControl
{
    [DebuggerDisplay("{DebuggerDisplay, nq}")]
    public class ActivationControlDetail : PersistEntity
    {
        // composite PK (PK from ActivationControl + StartsOn)
        // PK from ActivationControl
        public int ActivationControlId { get; set; }
        // PK
        public DateTime StartsOn { get; set; }

        public decimal OfferedVolumeUp { get; set; }
        public decimal OfferedVolumeDown { get; set; }
        public decimal OfferedVolumeForRedispatchingUp { get; set; }
        public decimal OfferedVolumeForRedispatchingDown { get; set; }
        public decimal PermittedDeviationUp { get; set; }
        public decimal PermittedDeviationDown { get; set; }
        public decimal RampingRate { get; set; }
        public bool HasJump { get; set; }
        public int DeliveryPointExcludedCount { get; set; }

        // one-to-many
        public List<ActivationControlTimestampDetail> TimestampDetails { get; set; } = null!;
        public List<ActivationControlDpDetail> DpDetails { get; set; } = null!;

        //
        public ActivationControl ActivationControl { get; set; } = null!;

        private string DebuggerDisplay => $"{StartsOn} OVU:{OfferedVolumeUp} OVD:{OfferedVolumeDown} OVRdU:{OfferedVolumeForRedispatchingUp} OVRdD:{OfferedVolumeForRedispatchingDown} PDeU:{PermittedDeviationUp} PDeD:{PermittedDeviationDown} RR:{RampingRate} J?:{HasJump} {ActivationControlId}";
    }
}
