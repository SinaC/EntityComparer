using DeepDiff.Configuration;
using DeepDiff.Internal.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DeepDiff.Internal.Configuration
{
    internal sealed class DeleteConfiguration<TEntity> : IDeleteConfiguration<TEntity>
        where TEntity : class
    {
        public DeleteConfiguration Configuration { get; }

        public DeleteConfiguration(DeleteConfiguration DeleteConfiguration)
        {
            Configuration = DeleteConfiguration;
        }

        public IDeleteConfiguration<TEntity> SetValue<TMember>(Expression<Func<TEntity, TMember>> destinationMember, TMember value)
        {
            var destinationProperty = destinationMember.GetSimplePropertyAccess().Single();
            Configuration.AddSetValueConfiguration(destinationProperty, value);
            return this;
        }

        public IDeleteConfiguration<TEntity> GenerateOperations(bool generate = true)
        {
            Configuration.SetGenerationOperations(generate);
            return this;
        }
    }
}
