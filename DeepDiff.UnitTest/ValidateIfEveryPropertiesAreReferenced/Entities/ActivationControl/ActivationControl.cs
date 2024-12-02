using DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Arc4u;
using DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Seas;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.ActivationControl
{
    [DebuggerDisplay("{DebuggerDisplay, nq}")]
    public class ActivationControl : UpdateAuditEntity<int, string, DateTime, string, DateTime?>
    {
        public string ContractReference { get; set; } = null!;
        public Date Day { get; set; }

        public decimal TotalEnergyRequested { get; set; }
        public decimal TotalDiscrepancy { get; set; }
        public decimal TotalEnergySupplied { get; set; }
        public decimal TotalEnergyToBeSupplied { get; set; }
        public decimal FailedPercentage { get; set; }
        public int IsMeasurementExcludedCount { get; set; }
        public int IsJumpExcludedCount { get; set; }
        public int DeactivationModeExcludedCount { get; set; }

        public ActivationControlStatus Status { get; set; }
        public string InternalComment { get; set; } = null!;
        public string TsoComment { get; set; } = null!;
        public SupplierStatus SupplierStatus { get; set; }
        public string SupplierComment { get; set; } = null!;

        public QualityFactorSources QualityFactorMissing { get; set; }
        public QualityFactorSources QualityFactorInvalid { get; set; }

        // one-to-many
        public List<ActivationControlDetail> ActivationControlDetails { get; set; } = null!;
        public List<ActivationControlDeactivationModePeriod> DeactivationModePeriods { get; set; } = null!;

        // calculated property, not saved in db
        public QualityFactor QualityFactor
        {
            get
            {
                QualityFactor qualityFactor = QualityFactor.None;
                if (QualityFactorMissing != QualityFactorSources.None)
                    qualityFactor |= QualityFactor.Missing;
                if (QualityFactorInvalid != QualityFactorSources.None)
                    qualityFactor |= QualityFactor.Invalid;
                return qualityFactor;
            }
        }

        private string DebuggerDisplay => $"{ContractReference} {Day} TER:{TotalEnergyRequested} TD:{TotalDiscrepancy} TES:{TotalEnergySupplied} TETBS:{TotalEnergyToBeSupplied} FP:{FailedPercentage} MEC:{IsMeasurementExcludedCount} JEC:{IsJumpExcludedCount} QFM:{QualityFactorMissing} QFI:{QualityFactorInvalid} {Id}";
    }
}
