// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

using System.Collections;
using System.Linq;

using Zorglub.Testing.Data.Bounded;

// TODO(data): I'm pretty sure we can improve the perf using DataGroup as a
// return value in various places. We might even use DataGroup everywhere?
// See e.g. GregorianDataSet.ConsecutiveDaysData.
// Can we initialize lazily a DataGroup? DataGroup<T> should not derived from
// TheoryData<T>, and should implement IEnumerable<object[]>.
// See https://github.com/xunit/xunit/blob/main/src/xunit.v3.core/TheoryData.cs
// DataGroup<T>:
// - implement IEnumerable<object[]>
// - do no derive from TheoryData<T>
// - two versions:
//   - one with a collection initializer, very much like TheoryData<T>
//   - one initialized with an enumerable
//   - both must provide AsEnumerableT() for filtering
//
// The advantage over <see cref="TheoryData{T}"/> is that we can enumerate a group of data
// without any boxing.
// It was created to improve the performance of <i>bounded</i> calendar datasets.

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
/// Defines a group of data for a theory.
/// </summary>
public interface IDataGroup<T> : IReadOnlyCollection<object?[]>
{
    IEnumerable<T> AsEnumerableT();
}

public sealed class DataSeq<T> : IDataGroup<T>
{
    private readonly IEnumerable<T> _values;

    public DataSeq(IEnumerable<T> values)
    {
        _values = values ?? throw new ArgumentNullException(nameof(values));
    }

    public int Count => _values.Count();

    public IEnumerable<T> AsEnumerableT() => _values;

    public IEnumerator<object?[]> GetEnumerator() =>
        _values.Select(v => new object?[] { v }).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public sealed class DataColl<T> : IDataGroup<T>
{
    private readonly List<T> _values = new();

    public DataColl() { }

    public DataColl(IEnumerable<T> values)
    {
        Requires.NotNull(values);

        foreach (T v in values) { _values.Add(v); }
    }

    public IEnumerable<T> AsEnumerableT() => _values;

    // Collection initializer.
    public void Add(T v) => _values.Add(v);

    public int Count => _values.Count;

    public IEnumerator<object?[]> GetEnumerator() =>
        _values.Select(v => new object?[] { v }).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public sealed class DataGroup<T> : TheoryData<T>, IDataGroup<T>
{
    private readonly IEnumerable<T> _values;

    public DataGroup(IEnumerable<T> values)
    {
        _values = values ?? throw new ArgumentNullException(nameof(values));

        foreach (T item in values) { AddRow(item); }
    }

    public int Count => _values.Count();

    public new void Add(T value) => throw new NotSupportedException();

    public IEnumerable<T> AsEnumerableT() => _values;
}
