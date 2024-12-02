using DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Arc4u;
using DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Seas;
using System;
using System.Diagnostics;

namespace DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.ActivationControl
{
    [DebuggerDisplay("{DebuggerDisplay, nq}")]
    public class ActivationControlTimestampDetail : PersistEntity, IAuditEntity<string, DateTime>
    {
        // composite PK (PK from ActivationControlDetail + Timestamp)
        // PK from ActivationControlDetail
        public int ActivationControlId { get; set; }
        public DateTime StartsOn { get; set; }
        // PK
        public DateTime Timestamp { get; set; }

        public decimal PowerMeasured { get; set; }
        public decimal PowerBaseline { get; set; }
        public decimal FcrCorrection { get; set; }
        public decimal EnergyRequested { get; set; }
        public decimal EnergyRequestedForRedispatching { get; set; }
        public decimal EnergySupplied { get; set; }
        public decimal EnergyToBeSupplied { get; set; }
        public decimal Deviation { get; set; }
        public decimal PermittedDeviation { get; set; }
        public decimal MaxDeviation { get; set; }
        public decimal Discrepancy { get; set; }
        public bool IsJumpExcluded { get; set; }
        public bool IsMeasurementExcluded { get; set; }
        public bool IsDeactivationModeExcluded { get; set; }

        // FK to ActivationControlDetail
        public ActivationControlDetail ActivationControlDetail { get; set; } = null!;

        // technical fields
        public string AuditedBy { get; set; } = null!;
        public DateTime AuditedOn { get; set; }

        //
        public decimal TotalEnergyRequested => EnergyRequested + EnergyRequestedForRedispatching;

        private string DebuggerDisplay => $"{Timestamp} PM:{PowerMeasured} PB:{PowerBaseline} FC:{FcrCorrection} ER:{EnergyRequested} ERRd:{EnergyRequestedForRedispatching} ES:{EnergySupplied} ETBS:{EnergyToBeSupplied} De:{Deviation} PDe:{PermittedDeviation} MDe:{MaxDeviation} Di:{Discrepancy} Je?:{IsJumpExcluded} Me?:{IsMeasurementExcluded} {ActivationControlId}-{StartsOn}";
    }
}
