using DeepDiff.Configuration;
using DeepDiff.Internal.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DeepDiff.Internal.Configuration
{
    internal sealed class ForceUpdateIfConfiguration<TEntity> : IForceUpdateIfConfiguration<TEntity>
        where TEntity : class
    {
        public ForceUpdateIfConfiguration Configuration { get; }

        public ForceUpdateIfConfiguration(ForceUpdateIfConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IForceUpdateIfConfiguration<TEntity> NestedEntitiesModified()
        {
            Configuration.EnableNestedEntitiesModified();
            return this;
        }

        public IForceUpdateIfConfiguration<TEntity> Equals<TMember>(Expression<Func<TEntity, TMember>> compareToMember, TMember compareToValue)
        {
            var compareToProperty = compareToMember.GetSimplePropertyAccess().Single();
            Configuration.AddEqualsConfiguration(compareToProperty, compareToValue);
            return this;
        }
    }
}
