using DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Seas;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.CapacityAvailability
{
    [DebuggerDisplay("{DebuggerDisplay, nq}")]
    public class CapacityAvailabilityCctu : AuditEntity
    {
        private DateTime _startsOn;
        public DateTime StartsOn
        {
            get => _startsOn;
            set
            {
                _startsOn = value;
                _endsOn = null;
            }
        }
        public string CctuName { get; set; } = null!;

        public decimal AwardedVolume { get; set; }
        public decimal ReservationPrice { get; set; }
        public decimal Remuneration { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsAllDayCctu { get; set; } // true: 24h false: 4h

        public decimal MissingVolume { get; set; }
        public decimal? MissingVolumeOverride { get; set; }
        public decimal Penalty { get; set; }

        public CapacityAvailabilityStatus Status { get; set; }
        public string InternalComment { get; set; } = null!;
        public string TsoComment { get; set; } = null!;
        public SupplierStatus SupplierStatus { get; set; }
        public string SupplierComment { get; set; } = null!;

        // one-to-many
        public List<CapacityAvailabilityCctuDetail> CapacityAvailabilityCctuDetails { get; set; } = null!;

        // FK
        public Guid CapacityAvailabilityDetailId { get; set; }
        public CapacityAvailabilityDetail CapacityAvailabilityDetail { get; set; } = null!;

        public int HourCount() => (int)(EndsOn - StartsOn).TotalHours;

        private DateTime? _endsOn;
        public DateTime EndsOn // calculated property (based on StartsOn and IsAllDayCctu)
        {
            get
            {
                _endsOn ??= IsAllDayCctu
                    ? StartsOn.ToLocalTime().AddDays(1).ToUniversalTime()
                    : StartsOn.ToLocalTime().AddHours(4).ToUniversalTime();
                return _endsOn.Value;
            }
        }

        private string DebuggerDisplay => $"{CctuName} V?:{IsVirtual} AD?:{IsAllDayCctu} A:{AwardedVolume} RP:{ReservationPrice} R:{Remuneration} MV:{MissingVolume} MVO:{MissingVolumeOverride} P:{Penalty} {Id}";
    }
}