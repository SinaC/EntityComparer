using System;
using System.Reflection;

namespace DeepDiff.Internal.Configuration
{
    internal abstract class NavigationConfigurationBase
    {
        public PropertyInfo NavigationProperty { get; } = null!;
        public Type NavigationChildType { get; } = null!;

        public NavigationConfigurationBase(PropertyInfo navigationProperty, Type navigationChildType)
        {
            NavigationProperty = navigationProperty;
            NavigationChildType = navigationChildType;
        }
    }
}
