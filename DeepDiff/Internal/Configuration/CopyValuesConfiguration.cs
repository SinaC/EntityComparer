using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeepDiff.Internal.Configuration
{
    internal sealed class CopyValuesConfiguration
    {
        public IReadOnlyCollection<PropertyInfo> CopyValuesProperties { get; } = null!;

        public CopyValuesConfiguration(IEnumerable<PropertyInfo> copyValuesProperties)
        {
            CopyValuesProperties = copyValuesProperties.ToArray();
        }
    }
}