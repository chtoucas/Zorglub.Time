// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing;

using System.Collections;

using Zorglub.Time.Hemerology;

[Obsolete("IDateRange is no longer internal.")]
public static class DateRangeProxy<TDate> where TDate : struct, IComparable<TDate>
{
    internal static DateRangeProxy<TRange, TDate> Create<TRange>(TRange range)
        where TRange : IDateRange<TRange, TDate>
    {
        return DateRangeProxy<TRange, TDate>.Create(range);
    }
}

/// <summary>
/// Proxy for the internal interface <see cref="IDateRange{TRange, TDate}"/>.
/// </summary>
[Obsolete("IDateRange is no longer internal.")]
[DebuggerTypeProxy(typeof(DateRangeProxy<,>.DebugView))]
public sealed partial class DateRangeProxy<TRange, TDate> : IDateRange<TRange, TDate>
    where TDate : struct, IComparable<TDate>
    where TRange : IEquatable<TRange>, IEnumerable<TDate>
{
    private readonly IDateRange<TRange, TDate> _subject;

    private DateRangeProxy(TRange range, IDateRange<TRange, TDate> subject)
    {
        Range = range ?? throw new ArgumentNullException(nameof(range));
        _subject = subject;
    }

    public TRange Range { get; }

    public override string? ToString() => _subject.ToString();

    internal static DateRangeProxy<SRange, TDate> Create<SRange>(SRange range)
        where SRange : IDateRange<SRange, TDate>
    {
        return new DateRangeProxy<SRange, TDate>(range, range);
    }

    private sealed class DebugView
    {
        private readonly DateRangeProxy<TRange, TDate> _obj;
        public DebugView(DateRangeProxy<TRange, TDate> obj) { _obj = obj; }
        public TRange Range => _obj.Range;
    }
}

public partial class DateRangeProxy<TRange, TDate> // IDateRange
{
    public TDate Start => _subject.Start;
    public TDate End => _subject.End;
    public int Length => _subject.Length;

    [Pure] public bool Contains(TDate date) => _subject.Contains(date);
    [Pure] public bool IsSupersetOf(TRange range) => _subject.IsSupersetOf(range);
    [Pure] public TRange? Intersect(TRange range) => _subject.Intersect(range);
    [Pure] public TRange? Union(TRange range) => _subject.Union(range);
}

public partial class DateRangeProxy<TRange, TDate> // IEquatable
{
    [Pure] public bool Equals(TRange? other) => _subject.Equals(other);

    [Pure]
    public override bool Equals(object? obj) =>
        (obj is DateRangeProxy<TRange, TDate> x && Equals(x.Range))
        || (obj is TRange y && Equals(y));

    [Pure] public override int GetHashCode() => _subject.GetHashCode();
}

public partial class DateRangeProxy<TRange, TDate> // IEnumerable
{
    [Pure] public IEnumerator<TDate> GetEnumerator() => _subject.GetEnumerator();
    [Pure] IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_subject).GetEnumerator();
}
