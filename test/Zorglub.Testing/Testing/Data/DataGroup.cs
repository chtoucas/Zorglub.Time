// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

using System.Collections;
using System.Linq;

// TODO(data): use DataGroup. Filter matching data in Bounded.
// See which optional/supplementary data should be lazy and explain why.
// Explain when to use ToXunitData(): never... except within the "DataConsumer"
// base classes.

#region Developer Notes

// The advantage of a DataGroup<T> over a TheoryData<T> is that we can enumerate
// and manipulate a group of data using the actual underlying data type directly.
// This is particularly useful when dealing with <i>bounded</i> calendar datasets.
// Notice that if one does not need to transform the data, there is no reason to
// use a DataGroup<T>; one should stick to TheoryData<T>.
// Another advantage is that a DataGroup<T> initialized with a sequence does not
// enumerate the sequence until explicitely requested.
//
// The same with TheoryData<T> would require an array access and boxing:
// > public TheoryData<T> WhereT(Func<T, bool> predicate)
// > {
// >     var q = from item in this
// >             let value = (T)item[0]
// >             where predicate(value)
// >             select value;
// >     Debug.Assert(q.Any());
// >     return DataGroup.Create(q);
// > }
//
// See https://github.com/xunit/xunit/blob/main/src/xunit.v3.core/TheoryData.cs

#endregion

/// <summary>
/// Provides factory methods for <see cref="DataGroup{T}"/>.
/// </summary>
public static class DataGroup
{
    /// <summary>
    /// Creates a <i>read-only</i> instance of the <see cref="DataGroup{T}"/> class.
    /// </summary>
    [Pure]
    public static DataGroup<T> Create<T>(IEnumerable<T> source) => new(source);

    //
    // CreateDaysSinceEpochInfoData()
    //

    [Pure]
    public static DataGroup<DaysSinceEpochInfo> CreateDaysSinceEpochInfoData<T>(
        IEnumerable<T> source, DayNumber epoch)
        where T : IConvertibleToDayNumberInfo
    {
        var q = from x in source select x.ToDayNumberInfo() - epoch;
        return Create(q);
    }

    //
    // CreateDayNumberInfoData()
    //

    [Pure]
    public static DataGroup<DayNumberInfo> CreateDayNumberInfoData(
        IEnumerable<DaysSinceEpochInfo> source, DayNumber epoch)
    {
        var q = from x in source select x.ToDayNumberInfo(epoch);
        return Create(q);
    }

    [Pure]
    public static DataGroup<DayNumberInfo> CreateDayNumberInfoData<T>(
        IEnumerable<T> source)
        where T : IConvertibleToDayNumberInfo
    {
        var q = from x in source select x.ToDayNumberInfo();
        return Create(q);
    }

    [Pure]
    public static DataGroup<DayNumberInfo> CreateDayNumberInfoData<T>(
        IEnumerable<T> source, DayNumber sourceEpoch, DayNumber resultEpoch)
        where T : IConvertibleToDayNumberInfo
    {
        int shift = resultEpoch - sourceEpoch;
        var q = from x in source select x.ToDayNumberInfo() + shift;
        return Create(q);
    }

    //
    // CreateDayNumberYewedaInfo()
    //

    [Pure]
    public static DataGroup<DayNumberYewedaInfo> CreateDayNumberYewedaInfo(
        IEnumerable<DaysSinceEpochYewedaInfo> source, DayNumber epoch)
    {
        var q = from x in source select x.ToDayNumberYewedaInfo(epoch);
        return Create(q);
    }
}

/// <summary>
/// Represents a group of data for a theory.
/// </summary>
[SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "<Pending>")]
public sealed class DataGroup<T> : IReadOnlyCollection<object?[]>
{
    /// <summary>
    /// Represents the container.
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly IContainer _container;

    /// <summary>
    /// Initializes a new <i>read-write</i> instance of the <see cref="DataGroup{T}"/> class.
    /// </summary>
    public DataGroup()
    {
        _container = new Container();
    }

    /// <summary>
    /// Initializes a new <i>read-only</i> instance of the <see cref="DataGroup{T}"/> class.
    /// </summary>
    public DataGroup(IEnumerable<T> values)
    {
        _container = new ReadOnlyContainer(values);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataGroup{T}"/> class.
    /// </summary>
    private DataGroup(IContainer container)
    {
        _container = container;
    }

    public bool IsReadOnly => _container.IsReadOnly;

    // Collection initializer.
    public void Add(T v) => _container.Add(v);

    public DataGroup<TResult> SelectT<TResult>(Func<T, TResult> selector)
    {
        var q = _container.Values.Select(selector);
        return DataGroup.Create(q);
    }

    public DataGroup<T> WhereT(Func<T, bool> predicate)
    {
        var q = _container.Values.Where(predicate);
        Debug.Assert(q.Any());

        return DataGroup.Create(q);
    }

    public DataGroup<T> ConcatT(IEnumerable<T> values)
    {
        var q = _container.Values.Concat(values);

        return DataGroup.Create(q);
    }

    public DataGroup<T> ConcatT(DataGroup<T> data)
    {
        Requires.NotNull(data);

        var q = _container.Values.Concat(data._container.Values);

        return DataGroup.Create(q);
    }

    public DataGroup<T> AsReadOnly() => IsReadOnly ? this : new(new ReadOnlyContainer(_container.Values));

    public DataGroup<T> AsReadWrite() => IsReadOnly ? new(new Container(_container.Values)) : this;

    public XunitData<T> ToXunitData()
    {
        var q = _container.ToEnumerable();
        return new XunitData<T>(q);
    }

    //
    // IReadOnlyCollection
    //

    public int Count => _container.Count();

    public IEnumerator<object?[]> GetEnumerator()
    {
        var q = _container.ToEnumerable();
        return q.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #region Container

    private interface IContainer
    {
        bool IsReadOnly { get; }

        IEnumerable<T> Values { get; }

        void Add(T v);

        int Count();

        public IEnumerable<object?[]> ToEnumerable() => Values.Select(v => new object?[] { v });
    }

    /// <summary>
    /// Represents a <i>read-write</i> container.
    /// </summary>
    private sealed class Container : IContainer
    {
        private readonly List<T> _values;

        public Container()
        {
            _values = new List<T>();
        }

        public Container(IEnumerable<T> values)
        {
            _values = values is List<T> l ? l : new List<T>(values);
        }

        public bool IsReadOnly => false;

        public IEnumerable<T> Values => _values;

        public int Count() => _values.Count;

        public void Add(T v) => _values.Add(v);
    }

    /// <summary>
    /// Represents a <i>read-only</i> container.
    /// </summary>
    private sealed class ReadOnlyContainer : IContainer
    {
        public ReadOnlyContainer(IEnumerable<T> values)
        {
            Values = values ?? throw new ArgumentNullException(nameof(values));
        }

        public bool IsReadOnly => true;

        public IEnumerable<T> Values { get; }

        public int Count() => Values.Count();

        public void Add(T v) => throw new NotSupportedException();
    }

    #endregion
}
