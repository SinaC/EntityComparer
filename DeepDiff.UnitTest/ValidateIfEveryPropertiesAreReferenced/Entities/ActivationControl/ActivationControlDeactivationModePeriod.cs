using DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Arc4u;
using System;

namespace DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.ActivationControl
{
    public class ActivationControlDeactivationModePeriod : PersistEntity
    {
        public int ActivationControlId { get; set; }

        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        public ActivationControl ActivationControl { get; set; } = null!;
    }
}
