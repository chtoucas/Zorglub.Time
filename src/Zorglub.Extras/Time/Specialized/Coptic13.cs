// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized;

using Zorglub.Time.Core;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Core.Validation;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

/// <summary>
/// Represents the Coptic calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class Coptic13Calendar :
    SpecialCalendar<Coptic13Date>,
    IRegularFeaturette,
    IVirtualMonthFeaturette
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Coptic13Calendar"/> class.
    /// </summary>
    public Coptic13Calendar() : this(new Coptic13Schema()) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Coptic13Calendar"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
    internal Coptic13Calendar(Coptic13Schema schema)
        : base("Coptic", StandardScope.Create(schema, DayZero.Coptic))
    {
        MonthsInYear = schema.MonthsInYear;
        VirtualMonth = schema.VirtualMonth;
    }

    /// <inheritdoc/>
    public int MonthsInYear { get; }

    /// <inheritdoc/>
    public int VirtualMonth { get; }

    /// <inheritdoc/>
    [Pure]
    private protected sealed override Coptic13Date GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>
/// Provides common adjusters for <see cref="Coptic13Date"/>.
/// </summary>
public sealed class Coptic13Adjuster : SpecialAdjuster<Coptic13Date>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Coptic13Adjuster"/> class.
    /// </summary>
    public Coptic13Adjuster() : base(Coptic13Date.Calendar.Scope) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Coptic13Adjuster"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
    internal Coptic13Adjuster(MinMaxYearScope scope) : base(scope) { }

    /// <inheritdoc/>
    [Pure]
    private protected sealed override Coptic13Date GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>
/// Represents the Coptic date.
/// <para><see cref="Coptic13Date"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct Coptic13Date :
    IDate<Coptic13Date, Coptic13Calendar>,
    IAdjustable<Coptic13Date>,
    IEpagomenalDay
{
    // NB: the order in which the static fields are written is important.

    /// <summary>
    /// Represents the schema.
    /// <para>This field is read-only.</para>
    /// </summary>
    private static readonly Coptic13Schema s_Schema = new();

    /// <summary>
    /// Represents the calendar.
    /// <para>This field is read-only.</para>
    /// </summary>
    private static readonly Coptic13Calendar s_Calendar = new(s_Schema);

    /// <summary>
    /// Represents the scope.
    /// <para>This field is read-only.</para>
    /// </summary>
    private static readonly MinMaxYearScope s_Scope = s_Calendar.Scope;

    /// <summary>
    /// Represents the epoch.
    /// <para>This field is read-only.</para>
    /// </summary>
    private static readonly DayNumber s_Epoch = s_Calendar.Epoch;

    /// <summary>
    /// Represents the domain, the interval of supported <see cref="DayNumber"/>.
    /// <para>This field is read-only.</para>
    /// </summary>
    private static readonly Range<DayNumber> s_Domain = s_Calendar.Domain;

    /// <summary>
    /// Represents the date adjuster.
    /// <para>This field is read-only.</para>
    /// </summary>
    private static readonly Coptic13Adjuster s_Adjuster = new(s_Scope);

    /// <summary>
    /// Represents the smallest possible value of a <see cref="Coptic13Date"/>.
    /// <para>This field is read-only.</para>
    /// </summary>
    private static readonly Coptic13Date s_MinValue = new(s_Domain.Min - s_Epoch);

    /// <summary>
    /// Represents the largest possible value of a <see cref="Coptic13Date"/>.
    /// <para>This field is read-only.</para>
    /// </summary>
    private static readonly Coptic13Date s_MaxValue = new(s_Domain.Max - s_Epoch);

    /// <summary>
    /// Represents the count of days since the Coptic epoch.
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly int _daysSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="Coptic13Date"/> struct to the specified
    /// date parts.
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a valid date or
    /// <paramref name="year"/> is outside the range of years supported by
    /// <see cref="Coptic13Calendar"/>.</exception>
    public Coptic13Date(int year, int month, int day)
    {
        s_Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = s_Schema.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Coptic13Date"/> struct to the specified
    /// ordinal date parts.
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a valid ordinal
    /// date or <paramref name="year"/> is outside the range of years supported by
    /// <see cref="Coptic13Calendar"/>.</exception>
    public Coptic13Date(int year, int dayOfYear)
    {
        s_Scope.ValidateOrdinal(year, dayOfYear);

        _daysSinceEpoch = s_Schema.CountDaysSinceEpoch(year, dayOfYear);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Coptic13Date"/> struct.
    /// </summary>
    /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
    /// supported values.</exception>
    public Coptic13Date(DayNumber dayNumber)
    {
        s_Domain.Validate(dayNumber);

        _daysSinceEpoch = dayNumber - s_Epoch;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Coptic13Date"/> struct.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    internal Coptic13Date(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    /// <summary>
    /// Gets the smallest possible value of a <see cref="Coptic13Date"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Coptic13Date MinValue => s_MinValue;

    /// <summary>
    /// Gets the largest possible value of a <see cref="Coptic13Date"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Coptic13Date MaxValue => s_MaxValue;

    /// <summary>
    /// Gets the date adjuster.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Coptic13Adjuster Adjuster => s_Adjuster;

    /// <inheritdoc />
    public static Coptic13Calendar Calendar => s_Calendar;

    /// <inheritdoc />
    public DayNumber DayNumber => s_Epoch + _daysSinceEpoch;

    /// <inheritdoc />
    public int DaysSinceEpoch => _daysSinceEpoch;

    /// <inheritdoc />
    public Ord CenturyOfEra => Ord.FromInt32(Century);

    /// <inheritdoc />
    public int Century => YearNumbering.GetCentury(Year);

    /// <inheritdoc />
    public Ord YearOfEra => Ord.FromInt32(Year);

    /// <inheritdoc />
    public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);

    /// <inheritdoc />
    public int Year => s_Schema.GetYear(_daysSinceEpoch);

    /// <inheritdoc />
    public int Month
    {
        get
        {
            s_Schema.GetDateParts(_daysSinceEpoch, out _, out int m, out _);
            return m;
        }
    }

    /// <inheritdoc />
    public int DayOfYear
    {
        get
        {
            _ = s_Schema.GetYear(_daysSinceEpoch, out int doy);
            return doy;
        }
    }

    /// <inheritdoc />
    public int Day
    {
        get
        {
            s_Schema.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
            return d;
        }
    }

    /// <inheritdoc />
    public DayOfWeek DayOfWeek => DayNumber.DayOfWeek;

    /// <inheritdoc />
    public bool IsIntercalary
    {
        get
        {
            s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return s_Schema.IsIntercalaryDay(y, m, d);
        }
    }

    /// <inheritdoc />
    public bool IsSupplementary
    {
        get
        {
            s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return s_Schema.IsSupplementaryDay(y, m, d);
        }
    }

    /// <summary>
    /// Returns a culture-independent string representation of the current instance.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({s_Calendar})");
    }

    /// <inheritdoc />
    public void Deconstruct(out int year, out int month, out int day) =>
        s_Schema.GetDateParts(_daysSinceEpoch, out year, out month, out day);

    /// <inheritdoc />
    public void Deconstruct(out int year, out int dayOfYear) =>
        year = s_Schema.GetYear(_daysSinceEpoch, out dayOfYear);

    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public partial struct Coptic13Date // Conversions, adjustments...
{
    #region Counting

    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInYear() => s_Schema.CountDaysInYearBefore(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInYear() => s_Schema.CountDaysInYearAfter(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInMonth() => s_Schema.CountDaysInMonthBefore(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInMonth() => s_Schema.CountDaysInMonthAfter(_daysSinceEpoch);

    #endregion
    #region Adjustments

    /// <inheritdoc />
    /// <remarks>See also <seealso cref="Adjuster"/>.</remarks>
    [Pure]
    public Coptic13Date Adjust(Func<Coptic13Date, Coptic13Date> adjuster)
    {
        Requires.NotNull(adjuster);

        return adjuster.Invoke(this);
    }

    //
    // Adjust the day of the week
    //

    /// <inheritdoc />
    [Pure]
    public Coptic13Date Previous(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Previous(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
        return new Coptic13Date(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public Coptic13Date PreviousOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
        return new Coptic13Date(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public Coptic13Date Nearest(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Nearest(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
        return new Coptic13Date(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public Coptic13Date NextOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.NextOrSame(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
        return new Coptic13Date(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public Coptic13Date Next(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Next(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
        return new Coptic13Date(dayNumber - s_Epoch);
    }

    #endregion
}

public partial struct Coptic13Date // IEquatable
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="Coptic13Date"/> are equal.
    /// </summary>
    public static bool operator ==(Coptic13Date left, Coptic13Date right) =>
        left._daysSinceEpoch == right._daysSinceEpoch;

    /// <summary>
    /// Determines whether two specified instances of <see cref="Coptic13Date"/> are not equal.
    /// </summary>
    public static bool operator !=(Coptic13Date left, Coptic13Date right) =>
        left._daysSinceEpoch != right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(Coptic13Date other) => _daysSinceEpoch == other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Coptic13Date date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceEpoch;
}

public partial struct Coptic13Date // IComparable
{
    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly earlier than the
    /// right one.
    /// </summary>
    public static bool operator <(Coptic13Date left, Coptic13Date right) =>
        left._daysSinceEpoch < right._daysSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is earlier than or equal to
    /// the right one.
    /// </summary>
    public static bool operator <=(Coptic13Date left, Coptic13Date right) =>
        left._daysSinceEpoch <= right._daysSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly later than the
    /// right one.
    /// </summary>
    public static bool operator >(Coptic13Date left, Coptic13Date right) =>
        left._daysSinceEpoch > right._daysSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is later than or equal to
    /// the right one.
    /// </summary>
    public static bool operator >=(Coptic13Date left, Coptic13Date right) =>
        left._daysSinceEpoch >= right._daysSinceEpoch;

    /// <summary>
    /// Obtains the earlier date of two specified dates.
    /// </summary>
    [Pure]
    public static Coptic13Date Min(Coptic13Date x, Coptic13Date y) => x < y ? x : y;

    /// <summary>
    /// Obtains the later date of two specified dates.
    /// </summary>
    [Pure]
    public static Coptic13Date Max(Coptic13Date x, Coptic13Date y) => x > y ? x : y;

    /// <summary>
    /// Indicates whether this instance is earlier, later or the same as the specified one.
    /// </summary>
    [Pure]
    public int CompareTo(Coptic13Date other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is Coptic13Date date ? CompareTo(date)
        : Throw.NonComparable(typeof(Coptic13Date), obj);
}

public partial struct Coptic13Date // Math ops
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) ✓
    // Friendly alternates do exist but use domain-specific names.

    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between them.
    /// </summary>
    public static int operator -(Coptic13Date left, Coptic13Date right) =>
        left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either the capacity of
    /// <see cref="Int32"/> or the range of supported dates.</exception>
    public static Coptic13Date operator +(Coptic13Date value, int days) => value.PlusDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either the capacity of
    /// <see cref="Int32"/> or the range of supported dates.</exception>
    public static Coptic13Date operator -(Coptic13Date value, int days) => value.PlusDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the latest supported
    /// date.</exception>
    public static Coptic13Date operator ++(Coptic13Date value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the earliest supported
    /// date.</exception>
    public static Coptic13Date operator --(Coptic13Date value) => value.PreviousDay();

#pragma warning restore CA2225

    /// <inheritdoc />
    [Pure]
    public int CountDaysSince(Coptic13Date other) =>
        // No need to use a checked context here.
        _daysSinceEpoch - other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public Coptic13Date PlusDays(int days)
    {
        int daysSinceEpoch = checked(_daysSinceEpoch + days);
        s_Scope.DaysValidator.CheckOverflow(daysSinceEpoch);
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public Coptic13Date NextDay() =>
        this == s_MaxValue ? Throw.DateOverflow<Coptic13Date>() : new Coptic13Date(_daysSinceEpoch + 1);

    /// <inheritdoc />
    [Pure]
    public Coptic13Date PreviousDay() =>
        this == s_MinValue ? Throw.DateOverflow<Coptic13Date>() : new Coptic13Date(_daysSinceEpoch - 1);
}
