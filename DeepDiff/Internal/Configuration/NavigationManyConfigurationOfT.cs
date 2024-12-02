using DeepDiff.Configuration;

namespace DeepDiff.Internal.Configuration
{
    internal sealed class NavigationManyConfiguration<TEntity, TChildEntity> : INavigationManyConfiguration<TEntity, TChildEntity>
        where TEntity : class
        where TChildEntity : class
    {
        public NavigationManyConfiguration Configuration { get; private set; }

        public NavigationManyConfiguration(NavigationManyConfiguration navigationManyConfiguration)
        {
            Configuration = navigationManyConfiguration;
        }
    }
}