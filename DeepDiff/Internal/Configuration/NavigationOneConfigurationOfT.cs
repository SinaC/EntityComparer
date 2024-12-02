using DeepDiff.Configuration;

namespace DeepDiff.Internal.Configuration
{
    internal sealed class NavigationOneConfiguration<TEntity, TChildEntity> : INavigationOneConfiguration<TEntity, TChildEntity>
        where TEntity : class
        where TChildEntity : class
    {
        public NavigationOneConfiguration Configuration { get; private set; }

        public NavigationOneConfiguration(NavigationOneConfiguration navigationOneConfiguration)
        {
            Configuration = navigationOneConfiguration;
        }
    }
}