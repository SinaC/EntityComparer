using DeepDiff.Exceptions;
using DeepDiff.Internal.Configuration;
using DeepDiff.Internal.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeepDiff.Configuration
{
    public sealed class DeepDiffConfiguration : IDeepDiffConfiguration
    {
        internal Dictionary<Type, EntityConfiguration> EntityConfigurationByTypes { get; } = new Dictionary<Type, EntityConfiguration>();

        public IEntityConfiguration<TEntity> Entity<TEntity>()
            where TEntity : class
        {
            var entityType = typeof(TEntity);
            if (EntityConfigurationByTypes.ContainsKey(entityType))
                throw new DuplicateEntityConfigurationException(entityType);

            var entityConfiguration = new EntityConfiguration(entityType);
            EntityConfigurationByTypes.Add(entityType, entityConfiguration);

            return new EntityConfiguration<TEntity>(entityConfiguration);
        }

        public IDeepDiffConfiguration AddProfile<TProfile>()
            where TProfile : DiffProfile
        {
            var diffProfileInstance = CreateProfileInstance(typeof(TProfile));
            AddProfileFromInstance(diffProfileInstance);
            return this;
        }

        public IDeepDiffConfiguration AddProfile(DiffProfile diffProfile)
        {
            AddProfileFromInstance(diffProfile);
            return this;
        }

        public IDeepDiffConfiguration AddProfiles(params Assembly[] assembliesToScan)
        {
            if (assembliesToScan != null && assembliesToScan.Length > 0)
            {
                foreach (var assembly in assembliesToScan)
                {
                    var diffProfileType = typeof(DiffProfile);
                    foreach (var derivedDiffProfileType in assembly.GetTypes().Where(x => x != diffProfileType && diffProfileType.IsAssignableFrom(x)))
                    {
                        var diffProfileInstance = CreateProfileInstance(derivedDiffProfileType);
                        AddProfileFromInstance(diffProfileInstance);
                    }
                }
            }
            return this;
        }

        public IDeepDiff CreateDeepDiff()
        {
            ValidateConfiguration();
            CreateComparers();

            return new Internal.DeepDiff(this);
        }

        public void ValidateConfiguration()
        {
            var exceptions = new List<Exception>();

            var validators = new ValidatorBase[]
            {
                new NoKeyValidator(),
                new KeyValidator(),
                new ValuesValidator(),
                new NavigationManyValidator(),
                new NavigationOneValidator(),
                new UpdateValidator(),
                new InsertValidator(),
                new DeleteValidator(),
                new ComparerValidator(),
            };

            foreach (var (type, entityConfiguration) in EntityConfigurationByTypes)
            {
                foreach (var validator in validators)
                {
                    var validationExceptions = validator.Validate(type, entityConfiguration, EntityConfigurationByTypes);
                    exceptions.AddRange(validationExceptions);
                }
            }
            if (exceptions.Count == 1)
                throw exceptions.Single();
            if (exceptions.Count > 0)
                throw new AggregateException(exceptions);
        }

        public void ValidateIfEveryPropertiesAreReferenced()
        {
            var validator = new CheckEveryPropertiesAreReferencedValidator();

            var exceptions = new List<Exception>();
            foreach (var (type, entityConfiguration) in EntityConfigurationByTypes)
            {
                var validationExceptions = validator.Validate(type, entityConfiguration);
                exceptions.AddRange(validationExceptions);
            }
            if (exceptions.Count == 1)
                throw exceptions.Single();
            if (exceptions.Count > 0)
                throw new AggregateException(exceptions);
        }

        private void CreateComparers()
        {
            foreach (var (_, entityConfiguration) in EntityConfigurationByTypes)
                entityConfiguration.CreateComparers();
        }

        private static DiffProfile CreateProfileInstance(Type diffProfileType)
        {
            DiffProfile diffProfileInstance;
            try
            {
                diffProfileInstance = (DiffProfile)Activator.CreateInstance(diffProfileType)!;
            }
            catch (TargetInvocationException ex) when (ex.InnerException is DuplicateEntityConfigurationException)
            {
                throw ex.InnerException;
            }
            return diffProfileInstance;
        }

        private void AddProfileFromInstance(DiffProfile diffProfile)
        {
            foreach (var typeAndEntityConfiguration in diffProfile.EntityConfigurations)
            {
                if (EntityConfigurationByTypes.ContainsKey(typeAndEntityConfiguration.Key))
                    throw new DuplicateEntityConfigurationException(typeAndEntityConfiguration.Key);
                EntityConfigurationByTypes.Add(typeAndEntityConfiguration.Key, typeAndEntityConfiguration.Value);
            }
        }
    }
}