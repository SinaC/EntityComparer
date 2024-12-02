using System.Reflection;

namespace DeepDiff.Internal.Configuration
{
    internal sealed class ForceUpdateIfEqualsConfiguration
    {
        public PropertyInfo CompareToProperty { get; } = null!;
        public object CompareToValue { get; } = null!;

        public ForceUpdateIfEqualsConfiguration(PropertyInfo compareToProperty, object compareToValue)
        {
            CompareToProperty = compareToProperty;
            CompareToValue = compareToValue;
        }
    }
}
