using DeepDiff.Internal.Comparers;
using DeepDiff.Internal.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DeepDiff.UnitTest.Comparer
{
    internal class ComparerFactory<TEntity>
            where TEntity : class
    {
        public NaiveEqualityComparerByProperty<TEntity> CreateNaiveComparer<TKey>(Expression<Func<TEntity, TKey>> expression)
            => CreateNaiveComparer(expression, null!, null!);

        public NaiveEqualityComparerByProperty<TEntity> CreateNaiveComparer<TKey>(Expression<Func<TEntity, TKey>> expression, IReadOnlyDictionary<Type, object> typeSpecificComparers, IReadOnlyDictionary<PropertyInfo, object> propertySpecificComparers)
        {
            var properties = expression.GetSimplePropertyAccessList().Select(p => p.Single());
            return new NaiveEqualityComparerByProperty<TEntity>(properties, typeSpecificComparers, propertySpecificComparers);
        }

        public PrecompiledEqualityComparerByProperty<TEntity> CreatePrecompiledComparer<TKey>(Expression<Func<TEntity, TKey>> expression)
            => CreatePrecompiledComparer(expression, null!, null!);

        public PrecompiledEqualityComparerByProperty<TEntity> CreatePrecompiledComparer<TKey>(Expression<Func<TEntity, TKey>> expression, IReadOnlyDictionary<Type, object> typeSpecificComparers, IReadOnlyDictionary<PropertyInfo, object> propertySpecificComparers)
        {
            var properties = expression.GetSimplePropertyAccessList().Select(p => p.Single());
            return new PrecompiledEqualityComparerByProperty<TEntity>(properties, typeSpecificComparers, propertySpecificComparers);
        }

        public PropertyInfo GetPropertyInfo<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression)
            => propertyExpression.GetSimplePropertyAccess().Single();
    }
}
