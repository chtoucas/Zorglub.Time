// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

using System.Linq;

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
/// Provides factory methods for TheoryDataEx&lt;DaysSinceEpochInfo&gt;
/// </summary>
public static class TheoryDataDseInfo
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
    public static TheoryData<DaysSinceEpochInfo> FromDaysSinceRataDieInfos(
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
/// Provides factory methods for TheoryDataEx&lt;DayNumberInfo&gt;
/// </summary>
public static class TheoryDataDNInfo
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

public sealed class TheoryDataEx<T> : TheoryData<T>
{
    public TheoryDataEx(IEnumerable<T> seq)
    {
        Requires.NotNull(seq);

        foreach (T item in seq) { AddRow(item); }
    }
}
