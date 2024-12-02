using System.Collections;

namespace DeepDiff.Comparers
{
    internal interface IComparerByProperty : IEqualityComparer
    {
        CompareByPropertyResult Compare(object left, object right);
    }
}
