using DeepDiff.Internal.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeepDiff.Internal.Validators
{
    internal sealed class ComparerValidator : ValidatorBase
    {
        public override IEnumerable<Exception> Validate(Type entityType, EntityConfiguration entityConfiguration, IReadOnlyDictionary<Type, EntityConfiguration> entityConfigurationByTypes)
        {
            // no validation for the moment
            return Enumerable.Empty<Exception>();
        }
    }
}
