// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

using System.Collections;
using System.Linq;

/// <summary>
/// Provides factory methods for <see cref="DataSeq{T}"/>.
/// </summary>
public static class DataSeq
{
    [Pure]
    public static DataSeq<T> Create<T>(IEnumerable<T> source)
    {
        return new DataSeq<T>(source);
    }

    //
    // CreateDaysSinceEpochInfoData()
    //

    [Pure]
    public static DataSeq<DaysSinceEpochInfo> CreateDaysSinceEpochInfoData<T>(
        IEnumerable<T> source, DayNumber epoch)
        where T : IConvertibleToDayNumberInfo
    {
        var q = from x in source select x.ToDayNumberInfo() - epoch;
        return new DataSeq<DaysSinceEpochInfo>(q);
    }

    //
    // CreateDayNumberInfoData()
    //

    [Pure]
    public static DataSeq<DayNumberInfo> CreateDayNumberInfoData(
        IEnumerable<DaysSinceEpochInfo> source, DayNumber epoch)
    {
        var q = from x in source select x.ToDayNumberInfo(epoch);
        return new DataSeq<DayNumberInfo>(q);
    }

    [Pure]
    public static DataSeq<DayNumberInfo> CreateDayNumberInfoData<T>(
        IEnumerable<T> source)
        where T : IConvertibleToDayNumberInfo
    {
        var q = from x in source select x.ToDayNumberInfo();
        return new DataSeq<DayNumberInfo>(q);
    }

    [Pure]
    public static DataSeq<DayNumberInfo> CreateDayNumberInfoData<T>(
        IEnumerable<T> source, DayNumber sourceEpoch, DayNumber resultEpoch)
        where T : IConvertibleToDayNumberInfo
    {
        int shift = resultEpoch - sourceEpoch;
        var q = from x in source select x.ToDayNumberInfo() + shift;
        return new DataSeq<DayNumberInfo>(q);
    }
}

/// <summary>
/// Represents a sequence of data for a theory.
/// </summary>
public sealed class DataSeq<T> : IReadOnlyCollection<object?[]>
{
    private readonly IEnumerable<T> _values;

    public DataSeq(IEnumerable<T> values)
    {
        _values = values ?? throw new ArgumentNullException(nameof(values));
    }

    public int Count => _values.Count();

    public IEnumerable<T> AsEnumerableT() => _values;

    public DataSeq<TResult> SelectT<TResult>(Func<T, TResult> selector) => new(_values.Select(selector));

    public DataSeq<T> WhereT(Func<T, bool> predicate)
    {
        var q = _values.Where(predicate);

        Debug.Assert(q.Any());

        return new DataSeq<T>(q);
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
