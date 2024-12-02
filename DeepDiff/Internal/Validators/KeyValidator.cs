using DeepDiff.Exceptions;
using DeepDiff.Internal.Configuration;
using DeepDiff.Internal.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeepDiff.Internal.Validators
{
    internal sealed class KeyValidator : ValidatorBase
    {
        // cannot be null or empty unless NoKey is true
        // every property must be different
        public override IEnumerable<Exception> Validate(Type entityType, EntityConfiguration entityConfiguration, IReadOnlyDictionary<Type, EntityConfiguration> entityConfigurationByTypes)
        {
            var keyConfiguration = entityConfiguration.KeyConfiguration;
            // cannot be null unless NoKey is true
            if (keyConfiguration == null)
            {
                if (!entityConfiguration.NoKey)
                    yield return new MissingKeyConfigurationException(entityType);
            }
            else
            {
                // NoKey cannot be true if KeyConfiguration is set
                if (entityConfiguration.NoKey)
                    yield return new NoKeyAndKeyConfigurationException(entityType);
                // cannot be empty
                if (keyConfiguration.KeyProperties == null || keyConfiguration.KeyProperties.Count == 0)
                    yield return new EmptyConfigurationException(entityType, NameOf<KeyConfiguration>());
                else
                {
                    // cannot contain duplicates
                    var duplicates = keyConfiguration.KeyProperties.FindDuplicate().ToArray();
                    if (duplicates.Length > 0)
                        yield return new DuplicatePropertyConfigurationException(entityType, NameOf<KeyConfiguration>(), duplicates.Select(x => x.Name));
                }
            }
        }
    }
}
