using DeepDiff.Configuration;

namespace DeepDiff.Internal.Configuration
{
    internal sealed class MergeSingleConfiguration : IMergeSingleConfiguration
    {
        public DiffEngineConfiguration Configuration { get; }

        public MergeSingleConfiguration()
        {
            Configuration = new DiffEngineConfiguration();
        }

        public IMergeSingleConfiguration UseHashtable(bool use = true)
        {
            Configuration.SetUseHashtable(use);
            return this;
        }

        public IMergeSingleConfiguration HashtableThreshold(int threshold = 15)
        {
            Configuration.SetHashtableThreshold(threshold);
            return this;
        }

        public IMergeSingleConfiguration ForceOnUpdateEvenIfModificationsDetectedOnlyInNestedLevel(bool force = false)
        {
            Configuration.SetForceOnUpdateEvenIfModificationsDetectedOnlyInNestedLevel(force);
            return this;
        }

        public IMergeSingleConfiguration GenerateOperations(bool generate = true)
        {
            Configuration.SetGenerateOperations(generate);
            return this;
        }

        public IMergeSingleConfiguration UsePrecompiledEqualityComparer(bool use = true)
        {
            Configuration.SetUsePrecompiledEqualityComparer(use);
            return this;
        }
    }
}
