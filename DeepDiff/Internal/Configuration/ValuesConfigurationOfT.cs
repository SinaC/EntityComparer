using DeepDiff.Configuration;

namespace DeepDiff.Internal.Configuration
{
    internal sealed class ValuesConfiguration<TEntity> : IValuesConfiguration<TEntity>
        where TEntity : class
    {
        private ValuesConfiguration Configuration { get; }

        public ValuesConfiguration(ValuesConfiguration valuesConfiguration)
        {
            Configuration = valuesConfiguration;
        }

        public IValuesConfiguration<TEntity> UsePrecompiledEqualityComparer(bool use = true)
        {
            Configuration.SetUsePrecompiledEqualityComparer(use);
            return this;
        }
    }
}