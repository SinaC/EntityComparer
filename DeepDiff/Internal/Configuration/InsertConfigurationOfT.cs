using DeepDiff.Configuration;
using DeepDiff.Internal.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DeepDiff.Internal.Configuration
{
    internal sealed class InsertConfiguration<TEntity> : IInsertConfiguration<TEntity>
        where TEntity : class
    {
        public InsertConfiguration Configuration { get; }

        public InsertConfiguration(InsertConfiguration InsertConfiguration)
        {
            Configuration = InsertConfiguration;
        }

        public IInsertConfiguration<TEntity> SetValue<TMember>(Expression<Func<TEntity, TMember>> destinationMember, TMember value)
        {
            var destinationProperty = destinationMember.GetSimplePropertyAccess().Single();
            Configuration.AddSetValueConfiguration(destinationProperty, value);
            return this;
        }

        public IInsertConfiguration<TEntity> GenerateOperations(bool generate = true)
        {
            Configuration.SetGenerationOperations(generate);
            return this;
        }
    }
}
