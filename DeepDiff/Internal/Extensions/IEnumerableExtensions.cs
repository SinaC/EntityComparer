using System;
using System.Collections.Generic;
using System.Linq;
using DeepDiff.Internal.Extensions;

namespace DeepDiff.Internal.Extensions
{
    internal static class IEnumerableExtensions
    {
        public static IEnumerable<T> FindDuplicate<T>(this IEnumerable<T> collection)
            => collection
                .GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .Select(x => x.Key);

        public static bool SequenceEqual<TSource>(
            this IEnumerable<TSource> source, IEnumerable<TSource> other, Func<TSource, TSource, bool> func)
            where TSource : class
        {
            return source.SequenceEqual(other, new LambdaEqualityComparer<TSource>(func));
        }

        private sealed class LambdaEqualityComparer<T> : IEqualityComparer<T>
            where T : class
        {
            private readonly Func<T, T, bool> _func;

            public LambdaEqualityComparer(Func<T, T, bool> func)
            {
                _func = func;
            }

            public bool Equals(T x, T y)
            {
                return _func(x, y);
            }

            public int GetHashCode(T obj)
            {
                return 0; // force Equals
            }
        }
    }
}
