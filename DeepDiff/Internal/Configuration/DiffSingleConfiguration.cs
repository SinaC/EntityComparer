using DeepDiff.Configuration;

namespace DeepDiff.Internal.Configuration
{
    internal sealed class DiffSingleConfiguration : IDiffSingleConfiguration
    {
        public DiffEngineConfiguration Configuration { get; }

        public DiffSingleConfiguration()
        {
            Configuration = new DiffEngineConfiguration();
            Configuration.SetForceOnUpdateEvenIfModificationsDetectedOnlyInNestedLevel(false);
            Configuration.SetGenerateOperations(true);
            Configuration.SetGenerateOperationsOnly(true);
        }

        public IDiffSingleConfiguration UseHashtable(bool use = true)
        {
            Configuration.SetUseHashtable(use);
            return this;
        }

        public IDiffSingleConfiguration HashtableThreshold(int threshold = 15)
        {
            Configuration.SetHashtableThreshold(threshold);
            return this;
        }

        public IDiffSingleConfiguration UsePrecompiledEqualityComparer(bool use = true)
        {
            Configuration.SetUsePrecompiledEqualityComparer(use);
            return this;
        }
    }
}
