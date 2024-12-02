using System.Collections.Generic;
using System;
using DeepDiff.Internal.Configuration;

namespace DeepDiff.Internal.Validators
{
    internal abstract class ValidatorBase
    {
        public abstract IEnumerable<Exception> Validate(Type entityType, EntityConfiguration entityConfiguration, IReadOnlyDictionary<Type, EntityConfiguration> entityConfigurationByTypes);

        protected static string NameOf<T>()
            => typeof(T).Name.Replace("Configuration", string.Empty);

        protected static string NameOf<T>(string prefix)
            => prefix + "." + typeof(T).Name.Replace("Configuration", string.Empty);
    }
}
