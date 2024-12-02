using DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Arc4u;
using System;
using System.Diagnostics;

namespace DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.ActivationRemuneration
{
    [DebuggerDisplay("{DebuggerDisplay, nq}")]
    public class ActivationRemunerationBidDetail : PersistEntity
    {
        // composite PK (PK from ActivationRemunerationBid + Timestamp)
        // PK from ActivationRemunerationDirectionDetail
        public int ActivationRemunerationId { get; set; }
        public DateTime StartsOn { get; set; }
        public Direction Direction { get; set; }
        public string BidGroupId { get; set; } = null!;
        // PK
        public DateTime Timestamp { get; set; }

        public bool IsConnectedToPicasso { get; set; }
        public bool IsProrata { get; set; }
        public decimal VolumeRequested { get; set; }
        public decimal VolumeRequestedForRedispatching { get; set; }
        public decimal MarginalPrice { get; set; }
        public decimal EnergyPrice { get; set; }
        public decimal Remuneration { get; set; }

        // FK to ActivationRemunerationBid
        public ActivationRemunerationBid ActivationRemunerationBid { get; set; } = null!;

        //
        public decimal TotalVolumeRequested => VolumeRequested + VolumeRequestedForRedispatching;

        private string DebuggerDisplay => $"{Timestamp} ICTP?:{IsConnectedToPicasso} IP?:{IsProrata} VR:{VolumeRequested} VRR:{VolumeRequestedForRedispatching} MP:{MarginalPrice} EP:{EnergyPrice} R:{Remuneration} {ActivationRemunerationId}-{StartsOn}-{Direction}-{BidGroupId}";
    }
}
