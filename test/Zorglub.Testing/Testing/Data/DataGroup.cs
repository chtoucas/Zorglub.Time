// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

using System.Collections;
using System.Linq;

using Zorglub.Testing.Data.Bounded;

// TODO(data): Can we initialize lazily a DataGroup? -> DataSeq
// Does XunitData help to improve perf?
//
// The advantage of a DataGroup<T> over a TheoryData<T> is that we can enumerate
// and manipulate a group of data using the actual underlying data type directly.
// This is particularly useful when dealing with <i>bounded</i> calendar datasets.
// If one does not need to transform the data, there is no reason to use a
// DataGroup<T>; one should stick to TheoryData<T>.
//
// See https://github.com/xunit/xunit/blob/main/src/xunit.v3.core/TheoryData.cs

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
/// </summary>
public sealed class DataGroup<T> : IReadOnlyCollection<object?[]>
{
    private readonly List<T> _values = new();

    public DataGroup() { }

    public DataGroup(IEnumerable<T> values)
    {
        Requires.NotNull(values);

        _values.AddRange(values);
    }

    // Collection initializer.
    public void Add(T v) => _values.Add(v);

    public int Count => _values.Count;

    // The same with TheoryData<T> would require an array access and boxing:
    // > public DataGroup<T> WhereT(Func<T, bool> predicate)
    // > {
    // >     var q = from item in this
    // >             let value = (T)item[0]
    // >             where predicate(value)
    // >             select value;
    // >     Debug.Assert(q.Any());
    // >     return DataGroup.Create(q);
    // > }

    public IEnumerable<T> AsEnumerableT() => _values;

    public DataGroup<TResult> SelectT<TResult>(Func<T, TResult> selector) => new(_values.Select(selector));

    public DataGroup<T> WhereT(Func<T, bool> predicate)
    {
        var q = _values.Where(predicate);

        Debug.Assert(q.Any());

        return new DataGroup<T>(q);
    }

    public XunitData<T> ToXunitData()
    {
        var values = new List<object?[]>(_values.Select(v => new object?[] { v }));

        return new(values);
    }

    public IEnumerator<object?[]> GetEnumerator() =>
        _values.Select(v => new object?[] { v }).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public sealed class XunitData<T> : IReadOnlyCollection<object?[]>
{
    private readonly List<object?[]> _values;

    internal XunitData(List<object?[]> values)
    {
        Requires.NotNull(values);

        _values = values;
    }

    public int Count => _values.Count;

    public IEnumerator<object?[]> GetEnumerator() => _values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
