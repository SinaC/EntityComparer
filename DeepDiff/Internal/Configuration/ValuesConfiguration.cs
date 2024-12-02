using DeepDiff.Internal.Comparers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeepDiff.Internal.Configuration
{
    internal sealed class ValuesConfiguration
    {
        public IReadOnlyCollection<PropertyInfo> ValuesProperties { get; } = null!;

        public IComparerByProperty PrecompiledEqualityComparer { get; private set; } = null!;
        public IComparerByProperty NaiveEqualityComparer { get; private set; } = null!;
        public bool UsePrecompiledEqualityComparer { get; private set; } = true;

        public ValuesConfiguration(IEnumerable<PropertyInfo> valuesProperties)
        {
            ValuesProperties = valuesProperties.ToArray();
        }

        public void SetUsePrecompiledEqualityComparer(bool usePrecompiledEqualityComparer)
        {
            UsePrecompiledEqualityComparer = usePrecompiledEqualityComparer;
        }

        public void CreateComparers(Type typeOfT, ComparerConfiguration comparerConfiguration)
        {
            var naiveEqualityComparerByPropertyTypeOfT = typeof(NaiveEqualityComparerByProperty<>).MakeGenericType(typeOfT);
            NaiveEqualityComparer = (IComparerByProperty)Activator.CreateInstance(naiveEqualityComparerByPropertyTypeOfT, ValuesProperties, comparerConfiguration?.TypeSpecificComparers, comparerConfiguration?.PropertySpecificComparers);

            var precompiledEqualityComparerByPropertyTypeOfT = typeof(PrecompiledEqualityComparerByProperty<>).MakeGenericType(typeOfT);
            PrecompiledEqualityComparer = (IComparerByProperty)Activator.CreateInstance(precompiledEqualityComparerByPropertyTypeOfT, ValuesProperties, comparerConfiguration?.TypeSpecificComparers, comparerConfiguration?.PropertySpecificComparers);
        }

    }
}