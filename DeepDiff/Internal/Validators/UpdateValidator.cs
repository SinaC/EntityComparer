using DeepDiff.Exceptions;
using DeepDiff.Internal.Configuration;
using DeepDiff.Internal.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeepDiff.Internal.Validators
{
    internal sealed class UpdateValidator : OperationValidatorBase
    {
        protected override string OperationConfigurationName { get; } = "OnUpdate";

        // cannot be null
        // must contain SetValue
        // set value cannot be found in key nor values configuration (not in copy values will be validated in copy values validation)
        // if copy values, cannot contain duplicate, cannot be found in key nor values not set value configuration
        public override IEnumerable<Exception> Validate(Type entityType, EntityConfiguration entityConfiguration, IReadOnlyDictionary<Type, EntityConfiguration> entityConfigurationByTypes)
        {
            var updateConfiguration = entityConfiguration.UpdateConfiguration;
            if (updateConfiguration != null)
            {
                // set value
                var setValueExceptions = ValidateSetValueConfigurations(entityType, entityConfiguration, updateConfiguration.SetValueConfigurations);
                foreach (var setValueEception in setValueExceptions)
                    yield return setValueEception;

                // copy values
                var copyValuesConfigurations = updateConfiguration.CopyValuesConfiguration;
                if (copyValuesConfigurations != null)
                {
                    // cannot be empty
                    if (copyValuesConfigurations.CopyValuesProperties == null || copyValuesConfigurations.CopyValuesProperties.Count == 0)
                        yield return new EmptyConfigurationException(entityType, NameOf<CopyValuesConfiguration>());
                    else
                    {
                        // cannot contain duplicates
                        var duplicates = copyValuesConfigurations.CopyValuesProperties.FindDuplicate().ToArray();
                        if (duplicates.Length > 0)
                            yield return new DuplicatePropertyConfigurationException(entityType, NameOf<CopyValuesConfiguration>(), duplicates.Select(x => x.Name));
                        // cannot be defined in keys
                        if (entityConfiguration.KeyConfiguration?.KeyProperties != null)
                        {
                            var alreadyDefinedInKey = copyValuesConfigurations.CopyValuesProperties.Intersect(entityConfiguration.KeyConfiguration.KeyProperties).ToArray();
                            if (alreadyDefinedInKey.Length > 0)
                                yield return new AlreadyDefinedPropertyException(entityType, NameOf<CopyValuesConfiguration>(OperationConfigurationName), NameOf<KeyConfiguration>(), alreadyDefinedInKey.Select(x => x.Name));
                        }
                        // cannot be found in values
                        if (entityConfiguration.ValuesConfiguration?.ValuesProperties != null)
                        {
                            var alreadyDefinedInValues = copyValuesConfigurations.CopyValuesProperties.Intersect(entityConfiguration.ValuesConfiguration.ValuesProperties).ToArray();
                            if (alreadyDefinedInValues.Length > 0)
                                yield return new AlreadyDefinedPropertyException(entityType, NameOf<CopyValuesConfiguration>(OperationConfigurationName), NameOf<ValuesConfiguration>(), alreadyDefinedInValues.Select(x => x.Name));
                        }
                        // cannot be found in set value
                        if (updateConfiguration.SetValueConfigurations != null && updateConfiguration.SetValueConfigurations.Count > 0)
                        {
                            foreach (var setValueConfiguration in updateConfiguration.SetValueConfigurations)
                            {
                                if (setValueConfiguration?.DestinationProperty != null)
                                {
                                    var alreadyDefinedInSetValue = copyValuesConfigurations.CopyValuesProperties.Contains(setValueConfiguration.DestinationProperty);
                                    if (alreadyDefinedInSetValue)
                                        yield return new AlreadyDefinedPropertyException(entityType, NameOf<CopyValuesConfiguration>(OperationConfigurationName), NameOf<SetValueConfiguration>(), new[] { setValueConfiguration.DestinationProperty.Name });
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
