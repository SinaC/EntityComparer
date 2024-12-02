using DeepDiff.Exceptions;
using DeepDiff.Internal.Configuration;
using System;
using System.Collections.Generic;

namespace DeepDiff.Configuration
{
    public abstract class DiffProfile
    {
        internal Dictionary<Type, EntityConfiguration> EntityConfigurations { get; private set; } = new Dictionary<Type, EntityConfiguration>();

        protected IEntityConfiguration<TEntity> CreateConfiguration<TEntity>()
            where TEntity : class
        {
            var entityType = typeof(TEntity);
            if (EntityConfigurations.ContainsKey(entityType))
                throw new DuplicateEntityConfigurationException(entityType);

            var entityConfiguration = new EntityConfiguration(entityType);
            EntityConfigurations.Add(entityType, entityConfiguration);

            return new EntityConfiguration<TEntity>(entityConfiguration);
        }
    }
}
