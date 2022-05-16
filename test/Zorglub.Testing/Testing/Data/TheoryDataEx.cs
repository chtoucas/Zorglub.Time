// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

using System.Linq;

/// <summary>
/// Provides factory methods for <see cref="TheoryDataEx{T}"/>.
/// </summary>
public static class TheoryDataEx
{
    [Pure]
    public static TheoryDataEx<T> Create<T>(IEnumerable<T> source)
    {
        return new TheoryDataEx<T>(source);
    }

    [Pure]
    public static TheoryDataEx<TResult> Create<TSource, TResult>(
        IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        var q = source.Select(selector);
        return new TheoryDataEx<TResult>(q);
    }
}

/// <summary>
/// Provides factory methods for <see cref="TheoryDataEx{T}"/> where <c>T</c> is
/// <see cref="DaysSinceEpochInfo"/>.
/// </summary>
public static class TheoryDataOfDaysSinceEpochInfo
{
    /// <summary>
    /// Converts a sequence of <see cref="DaysSinceOriginInfo"/> to a set of data of type
    /// <see cref="DaysSinceEpochInfo"/>.
    /// </summary>
    [Pure]
    public static TheoryDataEx<DaysSinceEpochInfo> FromDaysSinceOriginInfos(
        IEnumerable<DaysSinceOriginInfo> source, DayNumber origin, DayNumber epoch)
    {
        Requires.NotNull(source);

        int shift = origin - epoch;
        var q = from x in source select Map(x, shift);
        return new TheoryDataEx<DaysSinceEpochInfo>(q);

        static DaysSinceEpochInfo Map(DaysSinceOriginInfo x, int shift)
        {
            var (daysSinceOrigin, y, m, d) = x;
            return new DaysSinceEpochInfo(daysSinceOrigin + shift, y, m, d);
        }
    }

    /// <summary>
    /// Converts a sequence of <see cref="DaysSinceRataDieInfo"/> to a set of data of type
    /// <see cref="DaysSinceEpochInfo"/>.
    /// </summary>
    [Pure]
    public static TheoryDataEx<DaysSinceEpochInfo> FromDaysSinceRataDieInfos(
        IEnumerable<DaysSinceRataDieInfo> source, DayNumber epoch)
    {
        Requires.NotNull(source);

        int shift = DayZero.RataDie - epoch;
        var q = from x in source select Map(x, shift);
        return new TheoryDataEx<DaysSinceEpochInfo>(q);

        static DaysSinceEpochInfo Map(DaysSinceRataDieInfo x, int shift)
        {
            var (daysSinceRataDie, y, m, d) = x;
            return new DaysSinceEpochInfo(daysSinceRataDie + shift, y, m, d);
        }
    }
}

/// <summary>
/// Provides factory methods for <see cref="TheoryDataEx{T}"/> where <c>T</c> is
/// <see cref="DayNumberInfo"/>.
/// </summary>
public static class TheoryDataOfDayNumberInfo
{
    /// <summary>
    /// Converts a sequence of <see cref="DaysSinceEpochInfo"/> to a set of data of type
    /// <see cref="DayNumberInfo"/>.
    /// </summary>
    [Pure]
    public static TheoryDataEx<DayNumberInfo> FromDaysSinceEpochInfos(
        IEnumerable<DaysSinceEpochInfo> source, DayNumber epoch)
    {
        var q = from x in source select x.ToDayNumberInfo(epoch);
        return new TheoryDataEx<DayNumberInfo>(q);
    }

    /// <summary>
    /// Converts a sequence of <see cref="DaysSinceOriginInfo"/> to a set of data of type
    /// <see cref="DayNumberInfo"/>.
    /// </summary>
    [Pure]
    public static TheoryDataEx<DayNumberInfo> FromDaysSinceZeroInfos(
        IEnumerable<DaysSinceOriginInfo> source)
    {
        var q = from x in source select x.ToDayNumberInfo(DayNumber.Zero);
        return new TheoryDataEx<DayNumberInfo>(q);
    }

    /// <summary>
    /// Converts a sequence of <see cref="DaysSinceRataDieInfo"/> to a set of data of type
    /// <see cref="DayNumberInfo"/>.
    /// </summary>
    [Pure]
    public static TheoryDataEx<DayNumberInfo> FromDaysSinceRataDieInfos(
        IEnumerable<DaysSinceRataDieInfo> source)
    {
        var q = from x in source select x.ToDayNumberInfo();
        return new TheoryDataEx<DayNumberInfo>(q);
    }

    /// <summary>
    /// Converts a sequence of <see cref="DaysSinceRataDieInfo"/> to a set of data of type
    /// <see cref="DayNumberInfo"/>.
    /// </summary>
    [Pure]
    public static TheoryDataEx<DayNumberInfo> FromDaysSinceRataDieInfos(
        IEnumerable<DaysSinceRataDieInfo> source, DayNumber epoch, DayNumber newEpoch)
    {
        DayNumber zero = DayZero.RataDie + (newEpoch - epoch);
        var q = from x in source select x.ToDayNumberInfo(zero);
        return new TheoryDataEx<DayNumberInfo>(q);
    }
}

public sealed class TheoryDataEx<T> : TheoryData<T>, IEnumerable<T>
{
    private readonly IEnumerable<T> _seq;

    public TheoryDataEx(IEnumerable<T> seq)
    {
        _seq = seq ?? throw new ArgumentNullException(nameof(seq));

        foreach (T item in seq) { AddRow(item); }
    }

    [Pure]
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => _seq.GetEnumerator();
}
