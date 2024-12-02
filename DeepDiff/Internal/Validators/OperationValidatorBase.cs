using DeepDiff.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using DeepDiff.Internal.Configuration;
using DeepDiff.Internal.Extensions;

namespace DeepDiff.Internal.Validators
{
    internal abstract class OperationValidatorBase : ValidatorBase
    {
        protected abstract string OperationConfigurationName { get; }

        protected IEnumerable<Exception> ValidateSetValueConfigurations(Type entityType, EntityConfiguration entityConfiguration, IEnumerable<SetValueConfiguration> setValueConfigurations)
        {
            if (setValueConfigurations != null && setValueConfigurations.Any())
            {
                // cannot contain duplicate
                var duplicates = setValueConfigurations.Select(x => x.DestinationProperty).FindDuplicate().ToArray();
                if (duplicates.Length > 0)
                    yield return new DuplicatePropertyConfigurationException(entityType, NameOf<SetValueConfiguration>(OperationConfigurationName), duplicates.Select(x => x.Name));

                foreach (var setValueConfiguration in setValueConfigurations)
                {
                    // cannot be defined in keys
                    if (entityConfiguration.KeyConfiguration?.KeyProperties != null)
                    {
                        var alreadyDefinedInKey = entityConfiguration.KeyConfiguration.KeyProperties.Contains(setValueConfiguration.DestinationProperty);
                        if (alreadyDefinedInKey)
                            yield return new AlreadyDefinedPropertyException(entityType, NameOf<SetValueConfiguration>(OperationConfigurationName), NameOf<KeyConfiguration>(), new[] { setValueConfiguration.DestinationProperty.Name });
                    }
                    // cannot be found in values
                    if (entityConfiguration.ValuesConfiguration?.ValuesProperties != null)
                    {
                        var alreadyDefinedInValues = entityConfiguration.ValuesConfiguration.ValuesProperties.Contains(setValueConfiguration.DestinationProperty);
                        if (alreadyDefinedInValues)
                            yield return new AlreadyDefinedPropertyException(entityType, NameOf<SetValueConfiguration>(OperationConfigurationName), NameOf<ValuesConfiguration>(), new[] { setValueConfiguration.DestinationProperty.Name });
                    }
                }
            }
        }
    }
}
