using System.Collections;

namespace DeepDiff.Internal.Comparers
{
    internal interface IComparerByProperty : IEqualityComparer
    {
        CompareByPropertyResult Compare(object left, object right);
    }
}
