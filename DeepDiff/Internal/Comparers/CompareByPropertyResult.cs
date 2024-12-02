using System.Collections.Generic;
using System.Linq;

namespace DeepDiff.Internal.Comparers
{
    internal class CompareByPropertyResult
    {
        public CompareByPropertyResult(bool isEqual)
        {
            IsEqual = isEqual;
        }

        public CompareByPropertyResult(IReadOnlyCollection<CompareByPropertyResultDetail> details)
        {
            IsEqual = details?.Any() == false;
            Details = details;
        }

        public bool IsEqual { get; }

        public IReadOnlyCollection<CompareByPropertyResultDetail> Details { get; } // empty if IsEqual is true or if no properties specified in ComparerByProperty or if compared property was not of the expected type
    }
}
