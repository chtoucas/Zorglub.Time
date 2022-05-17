// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

using System.Linq;

using Zorglub.Testing.Data.Bounded;

// REVIEW(data): I'm pretty sure we can improve the perf using DataGroup as a
// return value in various places. We might even use DataGroup everywhere?
// See e.g. GregorianDataSet.ConsecutiveDaysData.
// Can we initialize lazily a DataGroup?

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

    //
    // CreateDaysSinceEpochInfoData()
    //

    [Pure]
    public static DataGroup<DaysSinceEpochInfo> CreateDaysSinceEpochInfoData<T>(
        IEnumerable<T> source, DayNumber epoch)
        where T : IConvertibleToDayNumberInfo
    {
        var q = from x in source select x.ToDayNumberInfo() - epoch;
        return new DataGroup<DaysSinceEpochInfo>(q);
    }

    //
    // CreateDayNumberInfoData()
    //

    [Pure]
    public static DataGroup<DayNumberInfo> CreateDayNumberInfoData(
        IEnumerable<DaysSinceEpochInfo> source, DayNumber epoch)
    {
        var q = from x in source select x.ToDayNumberInfo(epoch);
        return new DataGroup<DayNumberInfo>(q);
    }

    [Pure]
    public static DataGroup<DayNumberInfo> CreateDayNumberInfoData<T>(
        IEnumerable<T> source)
        where T : IConvertibleToDayNumberInfo
    {
        var q = from x in source select x.ToDayNumberInfo();
        return new DataGroup<DayNumberInfo>(q);
    }

    [Pure]
    public static DataGroup<DayNumberInfo> CreateDayNumberInfoData<T>(
        IEnumerable<T> source, DayNumber sourceEpoch, DayNumber resultEpoch)
        where T : IConvertibleToDayNumberInfo
    {
        int shift = resultEpoch - sourceEpoch;
        var q = from x in source select x.ToDayNumberInfo() + shift;
        return new DataGroup<DayNumberInfo>(q);
    }
}

/// <summary>
/// Represents a group of data for a theory.
/// <para>The advantage over <see cref="TheoryData{T}"/> is that we can enumerate a group of data
/// without any boxing. It was created to improve the performance of <i>bounded</i> calendar datasets;
/// see
/// <see cref="BoundedCalendarDataSet{TDataSet}.FilterData{T}(DataGroup{T}, Func{T, bool})"/>.</para>
/// </summary>
public sealed class DataGroup<T> : TheoryData<T>
{
    private readonly IEnumerable<T> _seq;

    public DataGroup(IEnumerable<T> seq)
    {
        _seq = seq ?? throw new ArgumentNullException(nameof(seq));

        foreach (T item in seq) { AddRow(item); }
    }

    public IEnumerable<T> AsEnumerable() => _seq;
}
