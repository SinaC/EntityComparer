namespace DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.MonthlyAggregation
{
    public class MonthlyAggregationImputation : MonthlyAggregationValues
    {
        public ImputationCategory ImputationCategory { get; set; }

        public decimal? EnergyRequested { get; set; }
        public decimal? EnergySupplied { get; set; }
        public decimal? EnergyDiscrepancy { get; set; }

        // navigation to MonthlyAggregationDetail
        public MonthlyAggregationDetail<MonthlyAggregationImputation> MonthlyAggregationDetail { get; set; } = null!;
    }
}
