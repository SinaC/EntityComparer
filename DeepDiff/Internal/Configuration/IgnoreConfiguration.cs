using DeepDiff.Internal.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeepDiff.Internal.Configuration
{
    internal sealed class IgnoreConfiguration
    {
        public IList<PropertyInfo> IgnoredProperties { get; private set; } = new List<PropertyInfo>();

        public void AddIgnoredProperties(IEnumerable<PropertyInfo> properties)
        {
            foreach (var property in properties)
            {
                if (IgnoredProperties.All(x => !x.IsSameAs(property)))
                    IgnoredProperties.Add(property);
            }
        }
    }
}
