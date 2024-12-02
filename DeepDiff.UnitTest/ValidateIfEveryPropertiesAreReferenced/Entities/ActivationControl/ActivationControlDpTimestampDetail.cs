using DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Arc4u;
using System;
using System.Diagnostics;

namespace DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.ActivationControl
{
    [DebuggerDisplay("{DebuggerDisplay, nq}")]
    public class ActivationControlDpTimestampDetail : PersistEntity
    {
        // composite PK (PK from ActivationControlDpDetail + Timestamp)
        // PK from ActivationControlDpDetail
        public int ActivationControlId { get; set; }
        public DateTime StartsOn { get; set; }
        public string DeliveryPointEan { get; set; } = null!;
        // PK
        public DateTime Timestamp { get; set; }

        public int AvailableSec { get; set; }
        public decimal PowerMeasured { get; set; }
        public decimal PowerBaseline { get; set; }
        public decimal FcrCorrection { get; set; }
        public decimal EnergySupplied { get; set; }

        public QualityFactorSources QualityFactorMissing { get; set; }
        public QualityFactorSources QualityFactorInvalid { get; set; }

        // FK to ActivationControlDpDetail
        public ActivationControlDpDetail ActivationControlDpDetail { get; set; } = null!;

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

        private string DebuggerDisplay => $"{Timestamp} PM:{PowerMeasured} PB:{PowerBaseline} FC:{FcrCorrection} ES:{EnergySupplied} QFM:{QualityFactorMissing} QFI:{QualityFactorInvalid} {ActivationControlId}-{StartsOn}-{DeliveryPointEan}";
    }
}
