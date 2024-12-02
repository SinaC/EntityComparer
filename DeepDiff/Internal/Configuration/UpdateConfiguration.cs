using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeepDiff.Internal.Configuration
{
    internal sealed class UpdateConfiguration
    {
        public IList<SetValueConfiguration> SetValueConfigurations { get; private set; } = new List<SetValueConfiguration>();
        public CopyValuesConfiguration CopyValuesConfiguration { get; private set; } = null!;
        public bool GenerateOperations { get; private set; } = true;

        public SetValueConfiguration AddSetValueConfiguration(PropertyInfo destinationProperty, object value)
        {
            var config = new SetValueConfiguration(destinationProperty, value);
            SetValueConfigurations.Add(config);
            return config;
        }

        public CopyValuesConfiguration SetCopyValuesConfiguration(IEnumerable<PropertyInfo> copyValuesProperties)
        {
            var config = new CopyValuesConfiguration(copyValuesProperties.ToArray());
            CopyValuesConfiguration = config;
            return config;
        }

        public void SetGenerationOperations(bool generateOperations)
        {
            GenerateOperations = generateOperations;
        }
    }
}
