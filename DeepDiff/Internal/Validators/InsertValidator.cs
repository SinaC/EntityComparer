using DeepDiff.Internal.Configuration;
using System;
using System.Collections.Generic;

namespace DeepDiff.Internal.Validators
{
    internal sealed class InsertValidator : OperationValidatorBase
    {
        protected override string OperationConfigurationName { get; } = "OnInsert";

        // cannot be null
        // must contain SetValue
        // set value cannot be found in key nor values configuration
        public override IEnumerable<Exception> Validate(Type entityType, EntityConfiguration entityConfiguration, IReadOnlyDictionary<Type, EntityConfiguration> entityConfigurationByTypes)
        {
            var insertConfiguration = entityConfiguration.InsertConfiguration;
            if (insertConfiguration != null)
            {
                // set value
                var setValueExceptions = ValidateSetValueConfigurations(entityType, entityConfiguration, insertConfiguration.SetValueConfigurations);
                foreach (var setValueEception in setValueExceptions)
                    yield return setValueEception;
            }
        }
    }
}
