using DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Seas;
using System;
using System.Collections.Generic;

namespace DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.MonthlyAggregation
{
    public class MonthlyAggregationDetail<TMonthlyAggregationValues> : AuditEntity
        where TMonthlyAggregationValues : MonthlyAggregationValues
    {
        public string ModuleKey { get; set; } = null!;

        public AggregatedStatus Status { get; set; }

        // one-to-many
        public List<TMonthlyAggregationValues> MonthlyAggregationValues { get; set; } = null!;

        // FK
        public Guid MonthlyAggregationId { get; set; }
        public MonthlyAggregation<TMonthlyAggregationValues> MonthlyAggregation { get; set; } = null!;
    }
}
