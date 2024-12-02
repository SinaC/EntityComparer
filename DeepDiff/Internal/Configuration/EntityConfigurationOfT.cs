using DeepDiff.Configuration;
using DeepDiff.Exceptions;
using DeepDiff.Internal.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DeepDiff.Internal.Configuration
{
    internal sealed class EntityConfiguration<TEntity> : IEntityConfiguration<TEntity>
        where TEntity : class
    {
        public EntityConfiguration Configuration { get; }

        internal EntityConfiguration(EntityConfiguration entityConfiguration)
        {
            Configuration = entityConfiguration;
        }

        public IEntityConfiguration<TEntity> NoKey()
        {
            if (Configuration.KeyConfiguration != null)
                throw new NoKeyAndKeyConfigurationException(typeof(TEntity));
            Configuration.SetNoKey();
            return this;
        }

        public IEntityConfiguration<TEntity> HasKey<TKey>(Expression<Func<TEntity, TKey>> keyExpression)
            => HasKey(keyExpression, null);

        public IEntityConfiguration<TEntity> HasKey<TKey>(Expression<Func<TEntity, TKey>> keyExpression, Action<IKeyConfiguration<TEntity>> keyConfigurationAction)
        {
            if (Configuration.KeyConfiguration != null)
                throw new DuplicateKeyConfigurationException(typeof(TEntity));
            if (Configuration.NoKey)
                throw new NoKeyAndKeyConfigurationException(typeof(TEntity));
            var keyProperties = keyExpression.GetSimplePropertyAccessList().Select(p => p.Single());
            var config = Configuration.SetKey(keyProperties);
            var configOfT = new KeyConfiguration<TEntity>(config);
            keyConfigurationAction?.Invoke(configOfT);
            return this;
        }

        public IEntityConfiguration<TEntity> HasValues<TValue>(Expression<Func<TEntity, TValue>> valuesExpression)
            => HasValues(valuesExpression, null);

        public IEntityConfiguration<TEntity> HasValues<TValue>(Expression<Func<TEntity, TValue>> valuesExpression, Action<IValuesConfiguration<TEntity>> valuesConfigurationAction)
        {
            if (Configuration.ValuesConfiguration != null)
                throw new DuplicateValuesConfigurationException(typeof(TEntity));
            var valueProperties = valuesExpression.GetSimplePropertyAccessList().Select(p => p.Single());
            var config = Configuration.SetValues(valueProperties);
            var configOfT = new ValuesConfiguration<TEntity>(config);
            valuesConfigurationAction?.Invoke(configOfT);
            return this;
        }

        public IEntityConfiguration<TEntity> HasMany<TChildEntity>(Expression<Func<TEntity, List<TChildEntity>>> navigationPropertyExpression)
            where TChildEntity : class
            => HasMany(navigationPropertyExpression, null);

        public IEntityConfiguration<TEntity> HasMany<TChildEntity>(Expression<Func<TEntity, List<TChildEntity>>> navigationPropertyExpression, Action<INavigationManyConfiguration<TEntity, TChildEntity>> navigationManyConfigurationAction)
            where TChildEntity : class
        {
            var navigationManyPropertyInfo = navigationPropertyExpression.GetSimplePropertyAccess().Single();
            var navigationManyDestinationType = typeof(TChildEntity);
            var config = Configuration.AddNavigationMany(navigationManyPropertyInfo, navigationManyDestinationType);
            var configOfT = new NavigationManyConfiguration<TEntity, TChildEntity>(config);
            navigationManyConfigurationAction?.Invoke(configOfT);
            return this;
        }

        public IEntityConfiguration<TEntity> HasOne<TChildEntity>(Expression<Func<TEntity, TChildEntity>> navigationPropertyExpression)
            where TChildEntity : class
            => HasOne(navigationPropertyExpression, null);

        public IEntityConfiguration<TEntity> HasOne<TChildEntity>(Expression<Func<TEntity, TChildEntity>> navigationPropertyExpression, Action<INavigationOneConfiguration<TEntity, TChildEntity>> navigationOneConfigurationAction)
            where TChildEntity : class
        {
            var navigationOnePropertyInfo = navigationPropertyExpression.GetSimplePropertyAccess().Single();
            var navigationOneDestinationType = typeof(TChildEntity);
            var config = Configuration.AddNavigationOne(navigationOnePropertyInfo, navigationOneDestinationType);
            var configOfT = new NavigationOneConfiguration<TEntity, TChildEntity>(config);
            navigationOneConfigurationAction?.Invoke(configOfT);
            return this;
        }

        public IEntityConfiguration<TEntity> OnUpdate(Action<IUpdateConfiguration<TEntity>> updateConfigurationAction)
        {
            var config = Configuration.GetOrSetOnUpdate();
            var configOfT = new UpdateConfiguration<TEntity>(config);
            updateConfigurationAction?.Invoke(configOfT);
            return this;
        }

        public IEntityConfiguration<TEntity> OnInsert(Action<IInsertConfiguration<TEntity>> insertConfigurationAction)
        {
            var config = Configuration.GetOrSetOnInsert();
            var configOfT = new InsertConfiguration<TEntity>(config);
            insertConfigurationAction?.Invoke(configOfT);
            return this;
        }

        public IEntityConfiguration<TEntity> OnDelete(Action<IDeleteConfiguration<TEntity>> deleteConfigurationAction)
        {
            var config = Configuration.GetOrSetOnDelete();
            var configOfT = new DeleteConfiguration<TEntity>(config);
            deleteConfigurationAction?.Invoke(configOfT);
            return this;
        }

        public IEntityConfiguration<TEntity> WithComparer<T>(IEqualityComparer<T> equalityComparer)
        {
            var propertyType = typeof(T);
            var config = Configuration.GetOrSetWithComparer();
            if (config.ContainsTypeSpecificComparer(propertyType))
                throw new DuplicateTypeSpecificComparerConfigurationException(typeof(TEntity), propertyType);
            config.AddTypeSpecificComparer(propertyType, equalityComparer);
            return this;
        }

        public IEntityConfiguration<TEntity> WithComparer<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression, IEqualityComparer<TProperty> propertyEqualityComparer)
        {
            var config = Configuration.GetOrSetWithComparer();
            var propertyInfo = propertyExpression.GetSimplePropertyAccess().Single();
            if (config.ContainsPropertySpecificComparer(propertyInfo))
                throw new DuplicatePropertySpecificComparerConfigurationException(typeof(TEntity), propertyInfo);
            config.AddPropertySpecificComparer(propertyInfo, propertyEqualityComparer);
            return this;
        }

        public IEntityConfiguration<TEntity> Ignore<TIgnore>(Expression<Func<TEntity, TIgnore>> ignoreExpression)
        {
            var ignoredProperties = ignoreExpression.GetSimplePropertyAccessList().Select(p => p.Single());
            var config = Configuration.GetOrSetIgnore();
            config.AddIgnoredProperties(ignoredProperties);
            return this;
        }

        public IEntityConfiguration<TEntity> ForceUpdateIf(Action<IForceUpdateIfConfiguration<TEntity>> forceUpdateIfConfigurationAction)
        {
            var config = Configuration.GetOrSetForceUpdateIf();
            var configOfT = new ForceUpdateIfConfiguration<TEntity>(config);
            forceUpdateIfConfigurationAction?.Invoke(configOfT);
            return this;
        }
    }
}