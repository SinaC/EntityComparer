using DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Arc4u;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.ActivationRemuneration
{
    [DebuggerDisplay("{DebuggerDisplay, nq}")]
    public class ActivationRemunerationBid : PersistEntity
    {
        // composite PK (PK from ActivationRemunerationDirectionDetail + BidGroupId)
        // PK from ActivationRemunerationDirectionDetail
        public int ActivationRemunerationId { get; set; }
        public DateTime StartsOn { get; set; }
        public Direction Direction { get; set; }
        // PK
        public string BidGroupId { get; set; } = null!;

        public string DeliveryPointEan { get; set; } = null!;
        public string DeliveryPointName { get; set; } = null!;

        public decimal EnergyRequested { get; set; }
        public decimal EnergyRequestedForRedispatching { get; set; }
        public decimal EnergyPrice { get; set; }
        public decimal BidPrice { get; set; }
        public decimal Remuneration { get; set; }

        // one-to-many
        public List<ActivationRemunerationBidDetail> ActivationRemunerationBidDetails { get; set; } = null!;

        // FK to ActivationRemunerationDirectionDetail
        public ActivationRemunerationDirectionDetail ActivationRemunerationDirectionDetail { get; set; } = null!;

        //
        public decimal TotalEnergyRequested => EnergyRequested + EnergyRequestedForRedispatching;

        private string DebuggerDisplay => $"{BidGroupId} {Direction} DP:{DeliveryPointEan} ER:{EnergyRequested} ERR:{EnergyRequestedForRedispatching} EP:{EnergyPrice} BP:{BidPrice} R:{Remuneration} {ActivationRemunerationId}-{StartsOn}-{Direction}";
    }
}
