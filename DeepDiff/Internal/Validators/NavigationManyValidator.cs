using DeepDiff.Exceptions;
using DeepDiff.Internal.Configuration;
using System;
using System.Collections.Generic;

namespace DeepDiff.Internal.Validators
{
    internal sealed class NavigationManyValidator : ValidatorBase
    {
        // every NavigationManyChildType must exist in configuration
        public override IEnumerable<Exception> Validate(Type entityType, EntityConfiguration entityConfiguration, IReadOnlyDictionary<Type, EntityConfiguration> entityConfigurationByTypes)
        {
            if (entityConfiguration.NavigationManyConfigurations != null)
            {
                foreach (var configuration in entityConfiguration.NavigationManyConfigurations)
                {
                    // check if navigation child type is found in configuration
                    if (!entityConfigurationByTypes.ContainsKey(configuration.NavigationChildType))
                        yield return new MissingNavigationManyChildConfigurationException(entityType, configuration.NavigationChildType);
                }
            }
        }
    }
}
