using System.Collections.Generic;
using System.Reflection;

namespace DeepDiff.Internal.Configuration
{
    internal sealed class ForceUpdateIfConfiguration
    {
        public bool NestedEntitiesModifiedEnabled { get; private set; } = false;
        public IList<ForceUpdateIfEqualsConfiguration> ForceUpdateIfEqualsConfigurations { get; private set; } = new List<ForceUpdateIfEqualsConfiguration>();

        public void EnableNestedEntitiesModified()
        {
            NestedEntitiesModifiedEnabled = true;
        }

        public ForceUpdateIfEqualsConfiguration AddEqualsConfiguration(PropertyInfo compareToProperty, object compareToValue)
        {
            var config = new ForceUpdateIfEqualsConfiguration(compareToProperty, compareToValue);
            ForceUpdateIfEqualsConfigurations.Add(config);
            return config;
        }
    }
}
