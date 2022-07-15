// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Intervals
{
    /// <summary>
    /// Defines a bounded interval.
    /// </summary>
    /// <typeparam name="T">The type of the interval's elements.</typeparam>
    public interface ISegment<T> : IInterval<T>
        where T : struct, IEquatable<T>, IComparable<T>
    {
        /// <summary>
        /// Gets the pair of left and right endpoints.
        /// </summary>
        OrderedPair<T> Endpoints { get; }

        /// <summary>
        /// Gets the left endpoint.
        /// <para>The left endpoint is a lower bound.</para>
        /// <para>In general, we can't say that the left endpoint is the infimum (element). For
        /// instance, for the interval ]2, 4] of <i>integers</i>, this property returns 2 which is
        /// not the infimum 3.</para>
        /// </summary>
        T LowerEnd { get; }

        /// <summary>
        /// Gets the right endpoint.
        /// <para>The right endpoint is an upper bound.</para>
        /// <para>In general, we can't say that the right endpoint is the supremum (element). For
        /// instance, for the interval [2, 4[ of <i>integers</i>, this property returns 4 which is
        /// not the supremum 3.</para>
        /// </summary>
        T UpperEnd { get; }
    }
}
