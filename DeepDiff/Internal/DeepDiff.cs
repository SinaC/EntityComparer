using DeepDiff.Configuration;
using DeepDiff.Exceptions;
using DeepDiff.Internal.Configuration;
using DeepDiff.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("DeepDiff.UnitTest")]
[assembly: InternalsVisibleTo("DeepDiff.PerformanceTest")]

namespace DeepDiff.Internal
{
    internal sealed class DeepDiff : IDeepDiff
    {
        private DeepDiffConfiguration Configuration { get; }

        internal DeepDiff(DeepDiffConfiguration configuration)
        {
            Configuration = configuration;
        }

        public MergeSingleResult<TEntity> MergeSingle<TEntity>(TEntity existingEntity, TEntity newEntity)
            where TEntity : class
            => MergeSingle(existingEntity, newEntity, null);

        public MergeSingleResult<TEntity> MergeSingle<TEntity>(TEntity existingEntity, TEntity newEntity, Action<IMergeSingleConfiguration> mergeSingleConfigurationAction)
            where TEntity : class
        {
            if (!Configuration.EntityConfigurationByTypes.TryGetValue(typeof(TEntity), out var entityConfiguration))
                throw new MissingConfigurationException(typeof(TEntity));

            var mergeSingleConfiguration = new MergeSingleConfiguration();
            mergeSingleConfigurationAction?.Invoke(mergeSingleConfiguration);

            var engine = new DeepDiffEngine(Configuration.EntityConfigurationByTypes, mergeSingleConfiguration.Configuration);
            var diffOperations = new List<DiffOperationBase>();
            var diffEntity = engine.InternalMergeSingle(entityConfiguration, existingEntity, newEntity, diffOperations);
            return new MergeSingleResult<TEntity>
            {
                Entity = mergeSingleConfiguration.Configuration.GenerateOperationsOnly
                    ? default
                    : (TEntity)diffEntity,
                Operations = diffOperations
            };
        }

        public MergeManyResult<TEntity> MergeMany<TEntity>(IEnumerable<TEntity> existingEntities, IEnumerable<TEntity> newEntities)
            where TEntity : class
            => MergeMany(existingEntities, newEntities, null);

        public MergeManyResult<TEntity> MergeMany<TEntity>(IEnumerable<TEntity> existingEntities, IEnumerable<TEntity> newEntities, Action<IMergeManyConfiguration> mergeManyConfigurationAction)
            where TEntity : class
        {
            if (!Configuration.EntityConfigurationByTypes.TryGetValue(typeof(TEntity), out var entityConfiguration))
                throw new MissingConfigurationException(typeof(TEntity));

            var mergeManyConfiguration = new MergeManyConfiguration();
            mergeManyConfigurationAction?.Invoke(mergeManyConfiguration);

            var engine = new DeepDiffEngine(Configuration.EntityConfigurationByTypes, mergeManyConfiguration.Configuration);
            var diffOperations = new List<DiffOperationBase>();
            var diffEntities = engine.InternalMergeMany(entityConfiguration, existingEntities, newEntities, diffOperations);
            return new MergeManyResult<TEntity>
            {
                Entities = mergeManyConfiguration.Configuration.GenerateOperationsOnly
                    ? default
                    : diffEntities.Cast<TEntity>(),
                Operations = diffOperations
            };
        }

        public IReadOnlyCollection<DiffOperationBase> DiffSingle<TEntity>(TEntity existingEntity, TEntity newEntity)
           where TEntity : class
            => DiffSingle(existingEntity, newEntity, null);

        public IReadOnlyCollection<DiffOperationBase> DiffSingle<TEntity>(TEntity existingEntity, TEntity newEntity, Action<IDiffSingleConfiguration> diffSingleConfigurationAction)
            where TEntity : class
        {
            if (!Configuration.EntityConfigurationByTypes.TryGetValue(typeof(TEntity), out var entityConfiguration))
                throw new MissingConfigurationException(typeof(TEntity));

            var diffSingleConfiguration = new DiffSingleConfiguration();
            diffSingleConfigurationAction?.Invoke(diffSingleConfiguration);

            var engine = new DeepDiffEngine(Configuration.EntityConfigurationByTypes, diffSingleConfiguration.Configuration);
            var diffOperations = new List<DiffOperationBase>();
            engine.InternalMergeSingle(entityConfiguration, existingEntity, newEntity, diffOperations);
            return diffOperations;
        }

        public IReadOnlyCollection<DiffOperationBase> DiffMany<TEntity>(IEnumerable<TEntity> existingEntities, IEnumerable<TEntity> newEntities)
            where TEntity : class
            => DiffMany(existingEntities, newEntities, null);

        public IReadOnlyCollection<DiffOperationBase> DiffMany<TEntity>(IEnumerable<TEntity> existingEntities, IEnumerable<TEntity> newEntities, Action<IDiffManyConfiguration> diffManyConfigurationAction)
            where TEntity : class
        {
            if (!Configuration.EntityConfigurationByTypes.TryGetValue(typeof(TEntity), out var entityConfiguration))
                throw new MissingConfigurationException(typeof(TEntity));

            var diffManyConfiguration = new DiffManyConfiguration();
            diffManyConfigurationAction?.Invoke(diffManyConfiguration);

            var engine = new DeepDiffEngine(Configuration.EntityConfigurationByTypes, diffManyConfiguration.Configuration);
            var diffOperations = new List<DiffOperationBase>();
            engine.InternalMergeMany(entityConfiguration, existingEntities, newEntities, diffOperations);
            return diffOperations;
        }
    }
}