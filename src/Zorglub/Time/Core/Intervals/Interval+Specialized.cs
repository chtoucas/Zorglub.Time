// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Core.Intervals;

// Omitted:
// - Coalesce(LowerRay<T> x, LowerRay<T> y) => Union(x, y).
// - Coalesce(UpperRay<T> x, UpperRay<T> y) => Union(x, y).
// - Coalesce(LowerRay<T> x, UpperRay<T> y) => null or the whole "interval".
public partial class Interval // Coalesce
{
    #region Int32

    /// <summary>
    /// Attempts to obtain the set union of the two specified intervals.
    /// <para>See also <seealso cref="Connected(Range{int}, Range{int})"/>.</para>
    /// </summary>
    /// <returns><see langword="null"/> if the set union is not an interval.</returns>
    [Pure]
    public static Range<int>? Coalesce(Range<int> x, Range<int> y)
    {
        var hull = Span(x, y);
        return hull.LongCount() <= x.LongCount() + y.LongCount() ? hull : null;
    }

    /// <summary>
    /// Attempts to obtain the set union of the two specified intervals.
    /// <para>See also <seealso cref="Connected(Range{int}, LowerRay{int})"/>.</para>
    /// </summary>
    /// <returns><see langword="null"/> if the set union is not an interval.</returns>
    [Pure]
    public static LowerRay<int>? Coalesce(Range<int> x, LowerRay<int> y) =>
        Connected(x, y) ? Span(x, y) : null;

    /// <summary>
    /// Attempts to obtain the set union of the two specified intervals.
    /// <para>See also <seealso cref="Connected(Range{int}, UpperRay{int})"/>.</para>
    /// </summary>
    /// <returns><see langword="null"/> if the set union is not an interval.</returns>
    [Pure]
    public static UpperRay<int>? Coalesce(Range<int> x, UpperRay<int> y) =>
        Connected(x, y) ? Span(x, y) : null;

    #endregion
    #region DayNumber

    /// <summary>
    /// Attempts to obtain the set union of the two specified intervals.
    /// <para>See also <seealso cref="Connected(Range{DayNumber}, Range{DayNumber})"/>.</para>
    /// </summary>
    /// <returns><see langword="null"/> if the set union is not an interval.</returns>
    [Pure]
    public static Range<DayNumber>? Coalesce(Range<DayNumber> x, Range<DayNumber> y)
    {
        var hull = Span(x, y);
        return hull.LongCount() <= x.LongCount() + y.LongCount() ? hull : null;
    }

    /// <summary>
    /// Attempts to obtain the set union of the two specified intervals.
    /// <para>See also <seealso cref="Connected(Range{DayNumber}, LowerRay{DayNumber})"/>.</para>
    /// </summary>
    /// <returns><see langword="null"/> if the set union is not an interval.</returns>
    [Pure]
    public static LowerRay<DayNumber>? Coalesce(Range<DayNumber> x, LowerRay<DayNumber> y) =>
        Connected(x, y) ? Span(x, y) : null;

    /// <summary>
    /// Attempts to obtain the set union of the two specified intervals.
    /// <para>See also <seealso cref="Connected(Range{DayNumber}, UpperRay{DayNumber})"/>.</para>
    /// </summary>
    /// <returns><see langword="null"/> if the set union is not an interval.</returns>
    [Pure]
    public static UpperRay<DayNumber>? Coalesce(Range<DayNumber> x, UpperRay<DayNumber> y) =>
        Connected(x, y) ? Span(x, y) : null;

    #endregion
}

// Omitted:
// - Gap(LowerRay<T> x, LowerRay<T> y) => empty.
// - Gap(UpperRay<T> x, UpperRay<T> y) => empty.
public partial class Interval // Gap
{
    // Alternative impl but may overflow.
    // > int min = Math.Min(x.Max, y.Max) + 1;
    // > int max = Math.Max(x.Min, y.Min) - 1;
    // > return min > max ? RangeSet<int>.Empty : RangeSet.CreateLeniently(min, max);

    #region Int32

    /// <summary>
    /// Obtains the largest interval lying between the two specified intervals.
    /// </summary>
    /// <returns>The empty interval if the two intervals overlap or are adjacent.</returns>
    [Pure]
    public static RangeSet<int> Gap(Range<int> x, Range<int> y) =>
        x.Max < y.Min ? GapCore(x.Max, y.Min)
        : y.Max < x.Min ? GapCore(y.Max, x.Min)
        : RangeSet<int>.Empty;

    /// <summary>
    /// Obtains the largest interval lying between the two specified intervals.
    /// </summary>
    /// <returns>The empty interval if the two intervals overlap or are adjacent.</returns>
    [Pure]
    public static RangeSet<int> Gap(Range<int> x, LowerRay<int> y) =>
        y.Max < x.Min ? GapCore(y.Max, x.Min) : RangeSet<int>.Empty;

    /// <summary>
    /// Obtains the largest interval lying between the two specified intervals.
    /// </summary>
    /// <returns>The empty interval if the two intervals overlap or are adjacent.</returns>
    [Pure]
    public static RangeSet<int> Gap(Range<int> x, UpperRay<int> y) =>
        x.Max < y.Min ? GapCore(x.Max, y.Min) : RangeSet<int>.Empty;

    /// <summary>
    /// Obtains the largest interval lying between the two specified intervals.
    /// </summary>
    /// <returns>The empty interval if the two intervals overlap or are adjacent.</returns>
    [Pure]
    public static RangeSet<int> Gap(LowerRay<int> x, UpperRay<int> y) =>
        x.Max < y.Min ? GapCore(x.Max, y.Min) : RangeSet<int>.Empty;

    private static RangeSet<int> GapCore(int max, int min)
    {
        // max < min => both ops do not overflow.
        int x = max + 1;
        int y = min - 1;

        return x > y ? RangeSet<int>.Empty : RangeSet.CreateLeniently(x, y);
    }

    #endregion
    #region DayNumber

    /// <summary>
    /// Obtains the largest interval lying between the two specified intervals.
    /// </summary>
    /// <returns>The empty interval if the two intervals overlap or are adjacent.</returns>
    [Pure]
    public static RangeSet<DayNumber> Gap(Range<DayNumber> x, Range<DayNumber> y) =>
         x.Max < y.Min ? GapCore(x.Max, y.Min)
        : y.Max < x.Min ? GapCore(y.Max, x.Min)
        : RangeSet<DayNumber>.Empty;

    /// <summary>
    /// Obtains the largest interval lying between the two specified intervals.
    /// </summary>
    /// <returns>The empty interval if the two intervals overlap or are adjacent.</returns>
    [Pure]
    public static RangeSet<DayNumber> Gap(Range<DayNumber> x, LowerRay<DayNumber> y) =>
        y.Max < x.Min ? GapCore(y.Max, x.Min) : RangeSet<DayNumber>.Empty;

    /// <summary>
    /// Obtains the largest interval lying between the two specified intervals.
    /// </summary>
    /// <returns>The empty interval if the two intervals overlap or are adjacent.</returns>
    [Pure]
    public static RangeSet<DayNumber> Gap(Range<DayNumber> x, UpperRay<DayNumber> y) =>
         x.Max < y.Min ? GapCore(x.Max, y.Min) : RangeSet<DayNumber>.Empty;

    /// <summary>
    /// Obtains the largest interval lying between the two specified intervals.
    /// </summary>
    /// <returns>The empty interval if the two intervals overlap or are adjacent.</returns>
    [Pure]
    public static RangeSet<DayNumber> Gap(LowerRay<DayNumber> x, UpperRay<DayNumber> y) =>
         x.Max < y.Min ? GapCore(x.Max, y.Min) : RangeSet<DayNumber>.Empty;

    private static RangeSet<DayNumber> GapCore(DayNumber max, DayNumber min)
    {
        // max < min => both ops do not overflow.
        var x = max + 1;
        var y = min - 1;

        return x > y ? RangeSet<DayNumber>.Empty : RangeSet.CreateLeniently(x, y);
    }

    #endregion
}

// Omitted:
// - Adjacent(LowerRay<T> x, LowerRay<T> y) => false.
// - Adjacent(UpperRay<T> x, UpperRay<T> y) => false.
public partial class Interval // Adjacency
{
    #region Int32

    /// <summary>
    /// Determines whether the two specified intervals are adjacent or not.
    /// <para>Two intervals are said to be <i>adjacent</i> if they are disjoint and there is an
    /// empty gap between them.</para>
    /// </summary>
    [Pure]
    public static bool Adjacent(Range<int> x, Range<int> y) =>
        (long)x.Max + 1 == y.Min || (long)y.Max + 1 == x.Min;

    /// <summary>
    /// Determines whether the two specified intervals are adjacent or not.
    /// <para>Two intervals are said to be <i>adjacent</i> if they are disjoint and there is an
    /// empty gap between them.</para>
    /// </summary>
    [Pure]
    public static bool Adjacent(Range<int> x, LowerRay<int> y) => (long)y.Max + 1 == x.Min;

    /// <summary>
    /// Determines whether the two specified intervals are adjacent or not.
    /// <para>Two intervals are said to be <i>adjacent</i> if they are disjoint and there is an
    /// empty gap between them.</para>
    /// </summary>
    [Pure]
    public static bool Adjacent(Range<int> x, UpperRay<int> y) => (long)x.Max + 1 == y.Min;

    /// <summary>
    /// Determines whether the two specified intervals are adjacent or not.
    /// <para>Two intervals are said to be <i>adjacent</i> if they are disjoint and there is an
    /// empty gap between them.</para>
    /// </summary>
    [Pure]
    public static bool Adjacent(LowerRay<int> x, UpperRay<int> y) => (long)x.Max + 1 == y.Min;

    #endregion
    #region DayNumber

    /// <summary>
    /// Determines whether the two specified intervals are adjacent or not.
    /// <para>Two intervals are said to be <i>adjacent</i> if they are disjoint and there is an
    /// empty gap between them.</para>
    /// </summary>
    [Pure]
    public static bool Adjacent(Range<DayNumber> x, Range<DayNumber> y) =>
        (long)x.Max.DaysSinceZero + 1 == y.Min.DaysSinceZero
        || (long)y.Max.DaysSinceZero + 1 == x.Min.DaysSinceZero;

    /// <summary>
    /// Determines whether the two specified intervals are adjacent or not.
    /// <para>Two intervals are said to be <i>adjacent</i> if they are disjoint and there is an
    /// empty gap between them.</para>
    /// </summary>
    [Pure]
    public static bool Adjacent(Range<DayNumber> x, LowerRay<DayNumber> y) =>
        (long)y.Max.DaysSinceZero + 1 == x.Min.DaysSinceZero;

    /// <summary>
    /// Determines whether the two specified intervals are adjacent or not.
    /// <para>Two intervals are said to be <i>adjacent</i> if they are disjoint and there is an
    /// empty gap between them.</para>
    /// </summary>
    [Pure]
    public static bool Adjacent(Range<DayNumber> x, UpperRay<DayNumber> y) =>
        (long)x.Max.DaysSinceZero + 1 == y.Min.DaysSinceZero;

    /// <summary>
    /// Determines whether the two specified intervals are adjacent or not.
    /// <para>Two intervals are said to be <i>adjacent</i> if they are disjoint and there is an
    /// empty gap between them.</para>
    /// </summary>
    [Pure]
    public static bool Adjacent(LowerRay<DayNumber> x, UpperRay<DayNumber> y) =>
        (long)x.Max.DaysSinceZero + 1 == y.Min.DaysSinceZero;

    #endregion
}

// Omitted:
// - Connected(LowerRay<T> x, LowerRay<T> y) => true.
// - Connected(UpperRay<T> x, UpperRay<T> y) => true.
public partial class Interval // Connectedness
{
    // Alternative impl:
    // > Disjoint(x, y) == false || Adjacent(x, y)
    // Discrete intervals
    // > Span(x, y).Count() <= x.Count() + y.Count()
    // Continuous intervals
    // > Distance(x, y) == 0
    // Continuous (finite) intervals
    // > Span(x, y).Width <= x.Width + y.Width

    #region Int32

    /// <summary>
    /// Determines whether the two specified intervals are connected or not.
    /// <para>Two intervals are said to be <i>connected</i> if their set union is an interval
    /// too.</para>
    /// </summary>
    [Pure]
    public static bool Connected(Range<int> x, Range<int> y) =>
        Disjoint(x, y) == false || Adjacent(x, y);

    /// <summary>
    /// Determines whether the two specified intervals are connected or not.
    /// <para>Two intervals are said to be <i>connected</i> if their set union is an interval
    /// too.</para>
    /// </summary>
    [Pure]
    public static bool Connected(Range<int> x, LowerRay<int> y) =>
        Disjoint(x, y) == false || Adjacent(x, y);

    /// <summary>
    /// Determines whether the two specified intervals are connected or not.
    /// <para>Two intervals are said to be <i>connected</i> if their set union is an interval
    /// too.</para>
    /// </summary>
    [Pure]
    public static bool Connected(Range<int> x, UpperRay<int> y) =>
        Disjoint(x, y) == false || Adjacent(x, y);

    /// <summary>
    /// Determines whether the two specified intervals are connected or not.
    /// <para>Two intervals are said to be <i>connected</i> if their set union is an interval
    /// too.</para>
    /// </summary>
    [Pure]
    public static bool Connected(LowerRay<int> x, UpperRay<int> y) =>
        Disjoint(x, y) == false || Adjacent(x, y);

    #endregion
    #region DayNumber

    /// <summary>
    /// Determines whether the two specified intervals are connected or not.
    /// <para>Two intervals are said to be <i>connected</i> if their set union is an interval
    /// too.</para>
    /// </summary>
    [Pure]
    public static bool Connected(Range<DayNumber> x, Range<DayNumber> y) =>
        Disjoint(x, y) == false || Adjacent(x, y);

    /// <summary>
    /// Determines whether the two specified intervals are connected or not.
    /// <para>Two intervals are said to be <i>connected</i> if their set union is an interval
    /// too.</para>
    /// </summary>
    [Pure]
    public static bool Connected(Range<DayNumber> x, LowerRay<DayNumber> y) =>
        Disjoint(x, y) == false || Adjacent(x, y);

    /// <summary>
    /// Determines whether the two specified intervals are connected or not.
    /// <para>Two intervals are said to be <i>connected</i> if their set union is an interval
    /// too.</para>
    /// </summary>
    [Pure]
    public static bool Connected(Range<DayNumber> x, UpperRay<DayNumber> y) =>
        Disjoint(x, y) == false || Adjacent(x, y);

    /// <summary>
    /// Determines whether the two specified intervals are connected or not.
    /// <para>Two intervals are said to be <i>connected</i> if their set union is an interval
    /// too.</para>
    /// </summary>
    [Pure]
    public static bool Connected(LowerRay<DayNumber> x, UpperRay<DayNumber> y) =>
        Disjoint(x, y) == false || Adjacent(x, y);

    #endregion
}
