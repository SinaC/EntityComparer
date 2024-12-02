using System;
using System.Collections;

namespace DeepDiff.PerformanceTest;

internal class NullableDecimalComparer : IEqualityComparer
{
    private int Decimals { get; }
    private decimal Modulus { get; }

    public NullableDecimalComparer(int decimals)
    {
        Decimals = decimals;
        Modulus = 1m / (decimal)Math.Pow(10, Decimals);
    }

    public new bool Equals(object? left, object? right)
    {
        if (ReferenceEquals(left, right)) // will handle left == right == null
            return true;
        if (left is not decimal)
            return false;
        if (right is not decimal)
            return false;
        var leftAsDecimal = (decimal?)left;
        var rightAsDecimal = (decimal?)right;
        return leftAsDecimal != null && EqualsTruncated(leftAsDecimal.Value, rightAsDecimal.Value);
    }

    private bool EqualsTruncated(decimal left, decimal right)
    {
        var leftTruncated = left - (left % Modulus);
        var rightTruncated = right - (right % Modulus);
        return leftTruncated == rightTruncated;
    }

    public int GetHashCode(object obj)
        => obj.GetHashCode();
}
