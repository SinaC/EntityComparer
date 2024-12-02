using DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Arc4u;
using DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Seas;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.ActivationRemuneration
{
    [DebuggerDisplay("{DebuggerDisplay, nq}")]
    public class ActivationRemuneration : CreateAuditEntity<int, string, DateTime>
    {
        public string ContractReference { get; set; } = null!;
        public Date Day { get; set; }

        // one-to-many
        public List<ActivationRemunerationDetail> ActivationRemunerationDetails { get; set; } = null!;

        private string DebuggerDisplay => $"{ContractReference} {Day} {Id}";
    }
}
