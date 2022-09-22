// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Utilities
{
    /// <summary>
    /// Provides extension methods for value tuples.
    /// </summary>
    internal static class ValueTupleExtensions
    {
        /// <summary>
        /// Maps both enclosed elements with the specified selector.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> is null.</exception>
        [Pure]
        public static ValueTuple<TResult, TResult> Select<T, TResult>(
            this ValueTuple<T, T> tuple,
            Func<T, TResult> selector)
        {
            Requires.NotNull(selector);

            return (selector(tuple.Item1), selector(tuple.Item2));
        }

        /// <summary>
        /// Maps the enclosed elements with the specified selectors.
        /// </summary>
        /// <remarks>
        /// <para>Despite its name, this method cannot appear within a Query
        /// Expression Pattern.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">One of the parameters is
        /// null.</exception>
        [Pure]
        public static ValueTuple<TResult1, TResult2> Select<T1, T2, TResult1, TResult2>(
            this ValueTuple<T1, T2> tuple,
            Func<T1, TResult1> firstSelector,
            Func<T2, TResult2> secondSelector)
        {
            Requires.NotNull(firstSelector);
            Requires.NotNull(secondSelector);

            return (firstSelector(tuple.Item1), secondSelector(tuple.Item2));
        }
    }
}
