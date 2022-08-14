// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1036 // Override methods on comparable types (Design) 👈 Tests

namespace Zorglub.Testing;

using System.Diagnostics.Contracts;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Hemerology;

// Equality should be structural: the object is a reference type but the
// underlying one is actually a value type.
// Easy solution: use a record, but default implementation will be very slow
// since it will then have to compute a lot of properties.

[Obsolete("IDate is no longer internal.")]
public static class DateProxy
{
    internal static DateProxy<T> Create<T>(T date)
        where T : struct, IEquatable<T>, IDate<T>
    {
        return DateProxy<T>.Create(date);
    }
}

/// <summary>
/// Proxy for the internal interface <see cref="IDate{T}"/>.
/// </summary>
[Obsolete("IDate is no longer internal.")]
[DebuggerTypeProxy(typeof(DateProxy<>.DebugView))]
public sealed partial class DateProxy<T> : IDate<T>
    where T : struct, IEquatable<T>, IDate<T>
{
    private readonly IDate<T> _subject;

    // Not truely obsolete, but we should not use this ctor.
    [Obsolete("Use DateProxy.Create() instead.")]
    internal DateProxy(T date, IDate<T> subject)
    {
        Date = date;
        _subject = subject;
    }

    public T Date { get; }

    public override string? ToString() => _subject.ToString();

    internal static DateProxy<S> Create<S>(S date) where S : struct, IEquatable<S>, IDate<S>
    {
        return new DateProxy<S>(date, date);
    }

    #region Operators
    static T IMinMaxFunctions<T>.Max(T x, T y) => throw new NotImplementedException();
    static T IMinMaxFunctions<T>.Min(T x, T y) => throw new NotImplementedException();
    static T IAdditionOperators<T, int, T>.operator +(T left, int right) => throw new NotImplementedException();
    static T ISubtractionOperators<T, int, T>.operator -(T left, int right) => throw new NotImplementedException();
    static int IDifferenceOperators<T, int>.operator -(T left, T right) => throw new NotImplementedException();
    static T IIncrementOperators<T>.operator ++(T value) => throw new NotImplementedException();
    static T IDecrementOperators<T>.operator --(T value) => throw new NotImplementedException();
    static bool IEqualityOperators<T, T>.operator ==(T left, T right) => throw new NotImplementedException();
    static bool IEqualityOperators<T, T>.operator !=(T left, T right) => throw new NotImplementedException();
    static bool IComparisonOperators<T, T>.operator <(T left, T right) => throw new NotImplementedException();
    static bool IComparisonOperators<T, T>.operator >(T left, T right) => throw new NotImplementedException();
    static bool IComparisonOperators<T, T>.operator <=(T left, T right) => throw new NotImplementedException();
    static bool IComparisonOperators<T, T>.operator >=(T left, T right) => throw new NotImplementedException();
    #endregion

    private sealed class DebugView
    {
        private readonly DateProxy<T> _obj;
        public DebugView(DateProxy<T> obj) { _obj = obj; }
        public T Date => _obj.Date;
    }
}

public partial class DateProxy<T> // IFixedDay
{
    public DayOfWeek DayOfWeek => _subject.DayOfWeek;

    [Pure] public DayNumber DayNumber => _subject.DayNumber;
    [Pure] public int DaysSinceEpoch => _subject.DaysSinceEpoch;

    [Pure] public T Previous(DayOfWeek dayOfWeek) => _subject.Previous(dayOfWeek);
    [Pure] public T PreviousOrSame(DayOfWeek dayOfWeek) => _subject.PreviousOrSame(dayOfWeek);
    [Pure] public T Nearest(DayOfWeek dayOfWeek) => _subject.Nearest(dayOfWeek);
    [Pure] public T NextOrSame(DayOfWeek dayOfWeek) => _subject.NextOrSame(dayOfWeek);
    [Pure] public T Next(DayOfWeek dayOfWeek) => _subject.Next(dayOfWeek);

    [Pure] public int CountDaysSince(T other) => _subject.CountDaysSince(other);
    [Pure] public T PlusDays(int days) => _subject.PlusDays(days);
    [Pure] public T NextDay() => _subject.NextDay();
    [Pure] public T PreviousDay() => _subject.PreviousDay();
}

public partial class DateProxy<T> // IDate
{
    public Ord CenturyOfEra => _subject.CenturyOfEra;
    public int Century => _subject.Century;
    public Ord YearOfEra => _subject.YearOfEra;
    public int YearOfCentury => _subject.YearOfCentury;
    public int Year => _subject.Year;
    public int Month => _subject.Month;
    public int DayOfYear => _subject.DayOfYear;
    public int Day => _subject.Day;

    public bool IsIntercalary => _subject.IsIntercalary;
    public bool IsSupplementary => _subject.IsSupplementary;

    public void Deconstruct(out int year, out int month, out int day) =>
        _subject.Deconstruct(out year, out month, out day);
    public void Deconstruct(out int year, out int dayOfYear) =>
        _subject.Deconstruct(out year, out dayOfYear);

    [Pure] public int CountElapsedDaysInYear() => _subject.CountElapsedDaysInYear();
    [Pure] public int CountRemainingDaysInYear() => _subject.CountRemainingDaysInYear();
    [Pure] public int CountElapsedDaysInMonth() => _subject.CountElapsedDaysInMonth();
    [Pure] public int CountRemainingDaysInMonth() => _subject.CountRemainingDaysInMonth();
}

public partial class DateProxy<T> // IEquatable
{
    [Pure] public bool Equals(T other) => _subject.Equals(other);

    [Pure]
    public override bool Equals(object? obj) =>
        (obj is DateProxy<T> x && Equals(x.Date)) || (obj is T y && Equals(y));

    [Pure] public override int GetHashCode() => _subject.GetHashCode();
}

public partial class DateProxy<T> // IComparable
{
    [Pure] public int CompareTo(T other) => _subject.CompareTo(other);
    [Pure] public int CompareTo(object? obj) => _subject.CompareTo(obj);
}
