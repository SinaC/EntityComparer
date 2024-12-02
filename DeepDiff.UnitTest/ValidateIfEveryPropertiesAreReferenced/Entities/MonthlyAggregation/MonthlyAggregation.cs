using DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Seas;
using System.Collections.Generic;

namespace DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.MonthlyAggregation
{
    public class MonthlyAggregation<TMonthlyAggregationValues> : AuditEntity
        where TMonthlyAggregationValues : MonthlyAggregationValues
    {
        public Month Month { get; set; }
        public string SupplierEan { get; set; } = null!;
        public string ContractReference { get; set; } = null!;

        public string SupplierName { get; set; } = null!;

        // one-to-many
        public List<MonthlyAggregationDetail<TMonthlyAggregationValues>> Details { get; set; } = null!;
    }
}
