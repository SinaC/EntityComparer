using DeepDiff.Configuration;

namespace DeepDiff.Internal.Configuration
{
    internal sealed class MergeManyConfiguration : IMergeManyConfiguration
    {
        public DiffEngineConfiguration Configuration { get; }

        public MergeManyConfiguration()
        {
            Configuration = new DiffEngineConfiguration();
        }

        public IMergeManyConfiguration UseHashtable(bool use = true)
        {
            Configuration.SetUseHashtable(use);
            return this;
        }

        public IMergeManyConfiguration HashtableThreshold(int threshold = 15)
        {
            Configuration.SetHashtableThreshold(threshold);
            return this;
        }

        public IMergeManyConfiguration ForceOnUpdateEvenIfModificationsDetectedOnlyInNestedLevel(bool force = false)
        {
            Configuration.SetForceOnUpdateEvenIfModificationsDetectedOnlyInNestedLevel(force);
            return this;
        }

        public IMergeManyConfiguration GenerateOperations(bool generate = true)
        {
            Configuration.SetGenerateOperations(generate);
            return this;
        }

        public IMergeManyConfiguration UsePrecompiledEqualityComparer(bool use = true)
        {
            Configuration.SetUsePrecompiledEqualityComparer(use);
            return this;
        }
    }
}
