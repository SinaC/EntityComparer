using DeepDiff.Exceptions;
using DeepDiff.Internal.Configuration;
using DeepDiff.Internal.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace DeepDiff.Internal.Validators
{
    internal class CheckEveryPropertiesAreReferencedValidator
    {
        public IEnumerable<Exception> Validate(Type type, EntityConfiguration entityConfiguration)
        {
            var propertiesToCheck = new List<PropertyInfo>();
            foreach (var property in type.GetProperties().Where(x => x.GetSetMethod(false)?.IsPublic == true))
            {
                if (entityConfiguration.IgnoreConfiguration == null || entityConfiguration.IgnoreConfiguration.IgnoredProperties.All(y => !property.IsSameAs(y)))
                    propertiesToCheck.Add(property);
            }
            return Validate(type, entityConfiguration, propertiesToCheck);
        }

        private IEnumerable<Exception> Validate(Type type, EntityConfiguration entityConfiguration, IEnumerable<PropertyInfo> propertiesToCheck)
        {
            foreach (var property in propertiesToCheck)
            {
                if (property.Name == "PersistChange")
                    Debugger.Break();
                // check key properties
                var found = CheckIfPropertyFound(property, entityConfiguration);
                if (!found)
                    yield return new PropertyNotReferenceInConfigurationException(type, property.Name);
            }
        }

        private bool CheckIfPropertyFound(PropertyInfo property, EntityConfiguration entityConfiguration)
            => entityConfiguration.KeyConfiguration?.KeyProperties?.Any(x => x.IsSameAs(property)) == true
                    || entityConfiguration.ValuesConfiguration?.ValuesProperties?.Any(x => x.IsSameAs(property)) == true
                    || entityConfiguration.NavigationManyConfigurations?.Select(x => x.NavigationProperty)?.Any(x => x.IsSameAs(property)) == true
                    || entityConfiguration.NavigationOneConfigurations?.Select(x => x.NavigationProperty)?.Any(x => x.IsSameAs(property)) == true
                    || entityConfiguration.UpdateConfiguration?.SetValueConfigurations?.Select(x => x.DestinationProperty)?.Any(x => x.IsSameAs(property)) == true
                    || entityConfiguration.UpdateConfiguration?.CopyValuesConfiguration?.CopyValuesProperties?.Any(x => x.IsSameAs(property)) == true
                    || entityConfiguration.InsertConfiguration?.SetValueConfigurations?.Select(x => x.DestinationProperty)?.Any(x => x.IsSameAs(property)) == true
                    || entityConfiguration.DeleteConfiguration?.SetValueConfigurations?.Select(x => x.DestinationProperty)?.Any(x => x.IsSameAs(property)) == true;


    }
}
