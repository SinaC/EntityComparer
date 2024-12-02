using System.Reflection;
using System;

namespace DeepDiff.Internal.Configuration
{
    internal sealed class NavigationManyConfiguration : NavigationConfigurationBase
    {
        public NavigationManyConfiguration(PropertyInfo navigationProperty, Type navigationChildType)
            : base(navigationProperty, navigationChildType)
        {
        }
    }
}