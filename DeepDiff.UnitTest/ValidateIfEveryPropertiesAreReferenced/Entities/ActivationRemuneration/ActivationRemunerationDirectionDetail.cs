using DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Arc4u;
using DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Seas;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.ActivationRemuneration
{
    [DebuggerDisplay("{DebuggerDisplay, nq}")]
    public class ActivationRemunerationDirectionDetail : PersistEntity, IAuditEntity<string, DateTime>
    {
        // composite PK (PK from ActivationRemunerationDetail + Direction)
        // PK from ActivationRemunerationDetail
        public int ActivationRemunerationId { get; set; }
        public DateTime StartsOn { get; set; }
        // PK
        public Direction Direction { get; set; }

        public decimal EnergyPrice { get; set; }
        public decimal? EnergyPriceOverride { get; set; }
        public decimal EnergyRequested { get; set; }
        public decimal? EnergyRequestedOverride { get; set; }
        public decimal EnergyRequestedForRedispatching { get; set; }
        public decimal Remuneration { get; set; }
        public int QualityCheck { get; set; }

        // one-to-many
        public List<ActivationRemunerationBid> ActivationRemunerationBids { get; set; } = null!;

        // FK to ActivationRemunerationDetail
        public ActivationRemunerationDetail ActivationRemunerationDetail { get; set; } = null!;

        // technical fields
        public string AuditedBy { get; set; } = null!;
        public DateTime AuditedOn { get; set; }

        //
        public decimal TotalEnergyRequested => (EnergyRequestedOverride ?? EnergyRequested) + EnergyRequestedForRedispatching;

        //
        private string DebuggerDisplay => $"{Direction} EP:{EnergyPrice} EPO:{EnergyPriceOverride} ER:{EnergyRequested} ERO:{EnergyRequestedOverride} ERR:{EnergyRequestedForRedispatching} R:{Remuneration} QC:{QualityCheck} {ActivationRemunerationId}-{StartsOn}";
    }
}
