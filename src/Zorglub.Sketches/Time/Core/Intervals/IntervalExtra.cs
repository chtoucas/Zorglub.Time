// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable IDE0060 // Remove unused parameter (Style)

namespace Zorglub.Time.Core.Intervals
{
    /// <summary>
    /// Provides the omitted methods in <see cref="Interval"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static partial class IntervalExtra { }

    public partial class IntervalExtra // Convex hull
    {
        /// <summary>
        /// Obtains the smallest range containing the two specified intervals.
        /// </summary>
        /// <returns>The set union of the two lower rays.</returns>
        [Pure]
        public static LowerRay<T> Span<T>(LowerRay<T> x, LowerRay<T> y) where T : struct, IEquatable<T>, IComparable<T> =>
            Interval.Union(x, y);

        /// <summary>
        /// Obtains the smallest range containing the two specified intervals.
        /// </summary>
        /// <returns>The set union of the two upper rays.</returns>
        [Pure]
        public static UpperRay<T> Span<T>(UpperRay<T> x, UpperRay<T> y) where T : struct, IEquatable<T>, IComparable<T> =>
            Interval.Union(x, y);

        /// <summary>
        /// Obtains the smallest range containing the two specified intervals.
        /// </summary>
        /// <returns>The whole "interval".</returns>
        [Pure]
        public static Unbounded<T> Span<T>(LowerRay<T> x, UpperRay<T> y) where T : struct, IEquatable<T>, IComparable<T> =>
            Unbounded<T>.Instance;
    }

    public partial class IntervalExtra // Disjoint
    {
        /// <summary>
        /// Determines whether the two specified intervals are disjoint or not.
        /// </summary>
        /// <returns><see langword="false"/>.</returns>
        [Pure]
        public static bool Disjoint<T>(LowerRay<T> x, LowerRay<T> y) where T : struct, IEquatable<T>, IComparable<T> =>
            false;

        /// <summary>
        /// Determines whether the two specified intervals are disjoint or not.
        /// </summary>
        /// <returns><see langword="false"/>.</returns>
        [Pure]
        public static bool Disjoint<T>(UpperRay<T> x, UpperRay<T> y) where T : struct, IEquatable<T>, IComparable<T> =>
            false;
    }

    public partial class IntervalExtra // Coalesce
    {
        #region Int32

        /// <summary>
        /// Attempts to obtain the set union of the two specified intervals.
        /// </summary>
        [Pure]
        public static LowerRay<int> Coalesce(LowerRay<int> x, LowerRay<int> y) =>
            Interval.Union(x, y);

        /// <summary>
        /// Attempts to obtain the set union of the two specified intervals.
        /// </summary>
        [Pure]
        public static UpperRay<int> Coalesce(UpperRay<int> x, UpperRay<int> y) =>
            Interval.Union(x, y);

        /// <summary>
        /// Attempts to obtain the set union of the two specified intervals.
        /// </summary>
        /// <returns>The whole "interval" if the set union is an interval; otherwise
        /// <see langword="null"/>.</returns>
        [Pure]
        public static Unbounded<int>? Coalesce(LowerRay<int> x, UpperRay<int> y) =>
            Interval.Connected(x, y) ? Unbounded<int>.Instance : null;

        #endregion
        #region DayNumber

        /// <summary>
        /// Attempts to obtain the set union of the two specified intervals.
        /// </summary>
        [Pure]
        public static LowerRay<DayNumber> Coalesce(LowerRay<DayNumber> x, LowerRay<DayNumber> y) =>
            Interval.Union(x, y);

        /// <summary>
        /// Attempts to obtain the set union of the two specified intervals.
        /// </summary>
        [Pure]
        public static UpperRay<DayNumber> Coalesce(UpperRay<DayNumber> x, UpperRay<DayNumber> y) =>
            Interval.Union(x, y);

        /// <summary>
        /// Attempts to obtain the set union of the two specified intervals.
        /// </summary>
        /// <returns>The whole "interval" if the set union is an interval; otherwise
        /// <see langword="null"/>.</returns>
        [Pure]
        public static Unbounded<DayNumber>? Coalesce(LowerRay<DayNumber> x, UpperRay<DayNumber> y) =>
            Interval.Connected(x, y) ? Unbounded<DayNumber>.Instance : null;

        #endregion
    }

    public partial class IntervalExtra // Gap
    {
        #region Int32

        /// <summary>
        /// Obtains the largest interval lying between the two specified intervals.
        /// </summary>
        /// <returns>The empty interval.</returns>
        [Pure]
        public static RangeSet<int> Gap(LowerRay<int> x, LowerRay<int> y) =>
            RangeSet<int>.Empty;

        /// <summary>
        /// Obtains the largest interval lying between the two specified intervals.
        /// </summary>
        /// <returns>The empty interval.</returns>
        [Pure]
        public static RangeSet<int> Gap(UpperRay<int> x, UpperRay<int> y) =>
            RangeSet<int>.Empty;

        #endregion
        #region DayNumber

        /// <summary>
        /// Obtains the largest interval lying between the two specified intervals.
        /// </summary>
        /// <returns>The empty interval.</returns>
        [Pure]
        public static RangeSet<DayNumber> Gap(LowerRay<DayNumber> x, LowerRay<DayNumber> y) =>
            RangeSet<DayNumber>.Empty;

        /// <summary>
        /// Obtains the largest interval lying between the two specified intervals.
        /// </summary>
        /// <returns>The empty interval.</returns>
        [Pure]
        public static RangeSet<DayNumber> Gap(UpperRay<DayNumber> x, UpperRay<DayNumber> y) =>
            RangeSet<DayNumber>.Empty;

        #endregion
    }

    public partial class IntervalExtra // Adjacency
    {
        #region Int32

        /// <summary>
        /// Determines whether the two specified intervals are adjacent or not.
        /// <para>Two intervals are said to be <i>adjacent</i> if they are disjoint and there is an
        /// empty gap between them.</para>
        /// </summary>
        /// <returns><see langword="false"/>.</returns>
        [Pure]
        public static bool Adjacent(LowerRay<int> x, LowerRay<int> y) =>
            false;

        /// <summary>
        /// Determines whether the two specified intervals are adjacent or not.
        /// <para>Two intervals are said to be <i>adjacent</i> if they are disjoint and there is an
        /// empty gap between them.</para>
        /// </summary>
        /// <returns><see langword="false"/>.</returns>
        [Pure]
        public static bool Adjacent(UpperRay<int> x, UpperRay<int> y) =>
            false;

        #endregion
        #region DayNumber

        /// <summary>
        /// Determines whether the two specified intervals are adjacent or not.
        /// <para>Two intervals are said to be <i>adjacent</i> if they are disjoint and there is an
        /// empty gap between them.</para>
        /// </summary>
        /// <returns><see langword="false"/>.</returns>
        [Pure]
        public static bool Adjacent(LowerRay<DayNumber> x, LowerRay<DayNumber> y) =>
            false;

        /// <summary>
        /// Determines whether the two specified intervals are adjacent or not.
        /// <para>Two intervals are said to be <i>adjacent</i> if they are disjoint and there is an
        /// empty gap between them.</para>
        /// </summary>
        /// <returns><see langword="false"/>.</returns>
        [Pure]
        public static bool Adjacent(UpperRay<DayNumber> x, UpperRay<DayNumber> y) =>
            false;

        #endregion
    }

    public partial class IntervalExtra // Connectedness
    {
        #region Int32

        /// <summary>
        /// Determines whether the two specified intervals are connected or not.
        /// <para>Two intervals are said to be <i>connected</i> if their set union is an interval
        /// too.</para>
        /// </summary>
        /// <returns><see langword="true"/>.</returns>
        [Pure]
        public static bool Connected(LowerRay<int> x, LowerRay<int> y) =>
            true;

        /// <summary>
        /// Determines whether the two specified intervals are connected or not.
        /// <para>Two intervals are said to be <i>connected</i> if their set union is an interval
        /// too.</para>
        /// </summary>
        /// <returns><see langword="true"/>.</returns>
        [Pure]
        public static bool Connected(UpperRay<int> x, UpperRay<int> y) =>
            true;

        #endregion
        #region DayNumber

        /// <summary>
        /// Determines whether the two specified intervals are connected or not.
        /// <para>Two intervals are said to be <i>connected</i> if their set union is an interval
        /// too.</para>
        /// </summary>
        /// <returns><see langword="true"/>.</returns>
        [Pure]
        public static bool Connected(LowerRay<DayNumber> x, LowerRay<DayNumber> y) =>
            true;

        /// <summary>
        /// Determines whether the two specified intervals are connected or not.
        /// <para>Two intervals are said to be <i>connected</i> if their set union is an interval
        /// too.</para>
        /// </summary>
        /// <returns><see langword="true"/>.</returns>
        [Pure]
        public static bool Connected(UpperRay<DayNumber> x, UpperRay<DayNumber> y) =>
            true;

        #endregion
    }
}
