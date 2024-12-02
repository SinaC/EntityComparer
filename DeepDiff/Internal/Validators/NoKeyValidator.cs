using DeepDiff.Exceptions;
using DeepDiff.Internal.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeepDiff.Internal.Validators
{
    internal sealed class NoKeyValidator : ValidatorBase
    {
        // if NoKey = true, cannot be found in NavigationManyConfiguration
        public override IEnumerable<Exception> Validate(Type entityType, EntityConfiguration entityConfiguration, IReadOnlyDictionary<Type, EntityConfiguration> entityConfigurationByTypes)
        {
            if (entityConfiguration.NoKey)
            {
                // NoKey cannot be true if KeyConfiguration is set
                if (entityConfiguration.KeyConfiguration != null)
                    yield return new NoKeyAndKeyConfigurationException(entityType);
                // if NoKey is true, entity cannot be found in NavigationMany
                var referencingEntityNavigationManyConfigurations = entityConfigurationByTypes.Values.Where(x => x.EntityType != entityType && x.NavigationManyConfigurations.Any() && x.NavigationManyConfigurations.Select(x => x.NavigationChildType).Contains(entityType)).ToArray();
                if (referencingEntityNavigationManyConfigurations.Length > 0)
                    yield return new NoKeyButFoundInNavigationManyConfigurationException(entityType, referencingEntityNavigationManyConfigurations.Select(x => x.EntityType));
            }
        }
    }
}
