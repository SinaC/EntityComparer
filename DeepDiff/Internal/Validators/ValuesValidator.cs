using DeepDiff.Exceptions;
using DeepDiff.Internal.Configuration;
using DeepDiff.Internal.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeepDiff.Internal.Validators
{
    internal sealed class ValuesValidator : ValidatorBase
    {
        // if not null, cannot be empty
        // every property must be different and cannot be found in key configuration
        public override IEnumerable<Exception> Validate(Type entityType, EntityConfiguration entityConfiguration, IReadOnlyDictionary<Type, EntityConfiguration> entityConfigurationByTypes)
        {
            var configuration = entityConfiguration.ValuesConfiguration;
            if (configuration != null)
            {
                // cannot be empty
                if (configuration.ValuesProperties == null || configuration.ValuesProperties.Count == 0)
                    yield return new EmptyConfigurationException(entityType, NameOf<ValuesConfiguration>());
                else
                {
                    // cannot contain duplicates
                    var duplicates = configuration.ValuesProperties.FindDuplicate().ToArray();
                    if (duplicates.Length > 0)
                        yield return new DuplicatePropertyConfigurationException(entityType, NameOf<ValuesConfiguration>(), duplicates.Select(x => x.Name));
                    // cannot be defined in keys
                    if (entityConfiguration.KeyConfiguration?.KeyProperties != null)
                    {
                        var alreadyDefinedInKey = configuration.ValuesProperties.Intersect(entityConfiguration.KeyConfiguration.KeyProperties).ToArray();
                        if (alreadyDefinedInKey.Length > 0)
                            yield return new AlreadyDefinedPropertyException(entityType, NameOf<ValuesConfiguration>(), NameOf<KeyConfiguration>(), alreadyDefinedInKey.Select(x => x.Name));
                    }
                }
            }
        }
    }
}
