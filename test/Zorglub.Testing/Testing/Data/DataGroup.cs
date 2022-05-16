// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

using System.Linq;

/// <summary>
/// Provides factory methods for <see cref="DataGroup{T}"/>.
/// </summary>
public static class DataGroup
{
    [Pure]
    public static DataGroup<T> Create<T>(IEnumerable<T> source)
    {
        return new DataGroup<T>(source);
    }

    [Pure]
    public static DataGroup<TResult> Create<TSource, TResult>(
        IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        var q = source.Select(selector);
        return new DataGroup<TResult>(q);
    }

#pragma warning disable CA1034 // Nested types should not be visible (Design)

    public static class OfDaysSinceEpochInfo
    {
        [Pure]
        public static DataGroup<DaysSinceEpochInfo> Create(
            IEnumerable<DaysSinceOriginInfo> source, DayNumber origin, DayNumber epoch)
        {
            int shift = origin - epoch;
            var q = from x in source select Map(x, shift);
            return new DataGroup<DaysSinceEpochInfo>(q);

            static DaysSinceEpochInfo Map(DaysSinceOriginInfo x, int shift)
            {
                var (daysSinceOrigin, y, m, d) = x;
                return new DaysSinceEpochInfo(daysSinceOrigin + shift, y, m, d);
            }
        }

        [Pure]
        public static DataGroup<DaysSinceEpochInfo> Create(
            IEnumerable<DaysSinceRataDieInfo> source, DayNumber epoch)
        {
            int shift = DayZero.RataDie - epoch;
            var q = from x in source select Map(x, shift);
            return new DataGroup<DaysSinceEpochInfo>(q);

            static DaysSinceEpochInfo Map(DaysSinceRataDieInfo x, int shift)
            {
                var (daysSinceRataDie, y, m, d) = x;
                return new DaysSinceEpochInfo(daysSinceRataDie + shift, y, m, d);
            }
        }

        [Pure]
        public static DataGroup<DaysSinceEpochInfo> FromDaysSinceZeroInfos(
            IEnumerable<DaysSinceOriginInfo> source, DayNumber epoch)
        {
            int shift = DayNumber.Zero - epoch;
            var q = from x in source select Map(x, shift);
            return new DataGroup<DaysSinceEpochInfo>(q);

            static DaysSinceEpochInfo Map(DaysSinceOriginInfo x, int shift)
            {
                var (daysSinceOrigin, y, m, d) = x;
                return new DaysSinceEpochInfo(daysSinceOrigin + shift, y, m, d);
            }
        }
    }

    public static class OfDayNumberInfo
    {
        [Pure]
        public static DataGroup<DayNumberInfo> Create(
            IEnumerable<DaysSinceEpochInfo> source, DayNumber epoch)
        {
            var q = from x in source select x.ToDayNumberInfo(epoch);
            return new DataGroup<DayNumberInfo>(q);
        }

        [Pure]
        public static DataGroup<DayNumberInfo> Create(
            IEnumerable<DaysSinceRataDieInfo> source)
        {
            var q = from x in source select x.ToDayNumberInfo();
            return new DataGroup<DayNumberInfo>(q);
        }

        [Pure]
        public static DataGroup<DayNumberInfo> Create(
            IEnumerable<DaysSinceRataDieInfo> source, DayNumber epoch, DayNumber newEpoch)
        {
            DayNumber zero = DayZero.RataDie + (newEpoch - epoch);
            var q = from x in source select x.ToDayNumberInfo(zero);
            return new DataGroup<DayNumberInfo>(q);
        }

        [Pure]
        public static DataGroup<DayNumberInfo> FromDaysSinceZeroInfos(
            IEnumerable<DaysSinceOriginInfo> source)
        {
            var q = from x in source select x.ToDayNumberInfo(DayNumber.Zero);
            return new DataGroup<DayNumberInfo>(q);
        }
    }

#pragma warning restore CA1034
}

public sealed class DataGroup<T> : TheoryData<T>, IEnumerable<T>
{
    private readonly IEnumerable<T> _seq;

    public DataGroup(IEnumerable<T> seq)
    {
        _seq = seq ?? throw new ArgumentNullException(nameof(seq));

        foreach (T item in seq) { AddRow(item); }
    }

    [Pure]
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => _seq.GetEnumerator();
}
