// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Simple;

using System.Numerics;

using Zorglub.Time.Core;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

// To represent individual days within a month, the most natural type is
// CalendarDate.

/// <summary>
/// Represents a calendar month.
/// <para><see cref="CalendarMonth"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct CalendarMonth :
    ISerializable<CalendarMonth, int>,
    // Comparison
    IComparisonOperators<CalendarMonth, CalendarMonth>,
    // Arithmetic
    IAdditionOperators<CalendarMonth, int, CalendarMonth>,
    ISubtractionOperators<CalendarMonth, int, CalendarMonth>,
    IDifferenceOperators<CalendarMonth, int>,
    IIncrementOperators<CalendarMonth>,
    IDecrementOperators<CalendarMonth>
{
    /// <summary>
    /// Represents the internal binary representation.
    /// </summary>
    private readonly Yemox _bin; // 4 bytes

    /// <summary>
    /// Initializes a new instance of the <see cref="CalendarMonth"/> struct to the specified
    /// components in the Gregorian calendar.
    /// <para>To create an instance for another calendar, see
    /// <see cref="SimpleCalendar.GetCalendarMonth(int, int)"/>.</para>
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a valid month or
    /// <paramref name="year"/> is outside the range of years supported by the Gregorian
    /// calendar.</exception>
    public CalendarMonth(int year, int month)
    {
        GregorianProlepticScope.ValidateYearMonth(year, month);

        _bin = new Yemox(year, month, (int)Cuid.Gregorian);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CalendarMonth"/> struct.
    /// <para>This constructor does NOT validate its parameters.</para>
    /// </summary>
    internal CalendarMonth(int y, int m, Cuid cuid)
    {
        _bin = new Yemox(y, m, (int)cuid);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CalendarMonth"/> struct.
    /// <para>This constructor does NOT validate its parameters.</para>
    /// </summary>
    internal CalendarMonth(Yemo ym, Cuid cuid)
    {
        _bin = new Yemox(ym, (int)cuid);
    }

    /// <summary>
    /// Gets the century of the era.
    /// </summary>
    public Ord CenturyOfEra => Ord.FromInt32(Century);

    /// <summary>
    /// Gets the century number.
    /// </summary>
    public int Century => YearNumbering.GetCentury(Year);

    /// <summary>
    /// Gets the year of the era.
    /// </summary>
    public Ord YearOfEra => Ord.FromInt32(Year);

    /// <summary>
    /// Gets the year of the century.
    /// <para>The result is in the range from 1 to 100.</para>
    /// </summary>
    public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);

    /// <summary>
    /// Gets the (algebraic) year number.
    /// </summary>
    public int Year => _bin.Year;

    /// <summary>
    /// Gets the month of the year.
    /// </summary>
    public int Month => _bin.Month;

    /// <summary>
    /// Returns true if the current instance is an intercalary month; otherwise returns false.
    /// </summary>
    public bool IsIntercalary
    {
        get
        {
            _bin.Unpack(out int y, out int m);
            ref readonly var chr = ref CalendarRef;
            return chr.Schema.IsIntercalaryMonth(y, m);
        }
    }

    /// <summary>
    /// Gets the calendar to which belongs the current instance.
    /// </summary>
    /// <remarks>
    /// <para>Performance tip: cache this property locally if used repeatedly within a code
    /// block.</para>
    /// </remarks>
    public SimpleCalendar Calendar => SimpleCatalog.GetCalendarUnchecked(_bin.Extra);

    /// <summary>
    /// Gets the calendar year.
    /// </summary>
    public CalendarYear CalendarYear => new(Year, Cuid);

    /// <summary>
    /// Gets the first day of this month instance.
    /// </summary>
    public CalendarDate FirstDay => new(Parts.StartOfMonth, Cuid);

    /// <summary>
    /// Obtains the last day of the specified month.
    /// </summary>
    public CalendarDate LastDay
    {
        get
        {
            _bin.Unpack(out int y, out int m);
            ref readonly var chr = ref CalendarRef;
            var ymd = chr.Schema.GetDatePartsAtEndOfMonth(y, m);
            return new CalendarDate(ymd, Cuid);
        }
    }

    /// <summary>
    /// Gets the month parts.
    /// </summary>
    internal Yemo Parts => _bin.Yemo;

    /// <summary>
    /// Gets the ID of the calendar to which belongs the current instance.
    /// </summary>
    internal Cuid Cuid => (Cuid)_bin.Extra;

    /// <summary>
    /// Gets a read-only reference to the calendar to which belongs the current instance.
    /// </summary>
    internal ref readonly SimpleCalendar CalendarRef
    {
        // CIL code size = 17 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref SimpleCatalog.GetCalendarUnsafe(_bin.Extra);
    }

    /// <summary>
    /// Returns a culture-independent string representation of the current instance.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        _bin.Unpack(out int y, out int m);
        ref readonly var chr = ref CalendarRef;
        return FormattableString.Invariant($"{m:D2}/{y:D4} ({chr})");
    }

    /// <summary>
    /// Deconstructs the current instance into its components.
    /// </summary>
    public void Deconstruct(out int year, out int month) => _bin.Unpack(out year, out month);
}

public partial struct CalendarMonth // Factories, infos, adjustments
{
    #region Binary serialization

    /// <summary>
    /// Deserializes a 32-bit binary value and recreates an original serialized
    /// <see cref="CalendarMonth"/> object.
    /// </summary>
    /// <exception cref="AoorException">The matching calendar is not a system calendar.
    /// </exception>
    [Pure]
    public static CalendarMonth FromBinary(int data)
    {
        var bin = new Yemox(data);
        bin.Unpack(out int y, out int m);
        var ident = (CalendarId)bin.Extra;
        return SimpleCatalog.GetSystemCalendar(ident).GetCalendarMonth(y, m);
    }

    /// <summary>
    /// Serializes the current <see cref="CalendarMonth"/> object to a 32-bit binary value that
    /// subsequently can be used to recreate the <see cref="CalendarMonth"/> object.
    /// </summary>
    /// <exception cref="NotSupportedException">Binary serialization is only supported for dates
    /// belonging to a system calendar.</exception>
    [Pure]
    public int ToBinary() => Cuid.IsFixed() ? _bin.ToBinary() : Throw.NotSupported<int>();

    #endregion
    #region Conversions

    /// <summary>
    /// Converts the current instance to a range of days.
    /// </summary>
    [Pure]
    public Range<CalendarDate> ToRange() => Range.Create(FirstDay, LastDay);

    #endregion
    #region Counting

    /// <summary>
    /// Obtains the number of whole days in the year elapsed since the start of the year and
    /// before this month instance.
    /// </summary>
    [Pure]
    public int CountElapsedDaysInYear()
    {
        _bin.Unpack(out int y, out int m);
        ref readonly var chr = ref CalendarRef;
        return chr.Schema.CountDaysInYearBeforeMonth(y, m);
    }

    /// <summary>
    /// Obtains the number of whole days remaining after this month instance and until the end
    /// of the year.
    /// </summary>
    [Pure]
    public int CountRemainingDaysInYear()
    {
        _bin.Unpack(out int y, out int m);
        ref readonly var chr = ref CalendarRef;
        return chr.Schema.CountDaysInYearAfterMonth(y, m);
    }

    /// <summary>
    /// Obtains the number of days in this month instance.
    /// </summary>
    [Pure]
    public int CountDaysInMonth()
    {
        _bin.Unpack(out int y, out int m);
        ref readonly var chr = ref CalendarRef;
        return chr.Schema.CountDaysInMonth(y, m);
    }

    #endregion
    #region Days within the month

    /// <summary>
    /// Obtains the date corresponding to the specified day of this month instance.
    /// </summary>
    /// <exception cref="AoorException"><paramref name="dayOfMonth"/> is outside the range of
    /// valid values.</exception>
    [Pure]
    public CalendarDate GetDayOfMonth(int dayOfMonth)
    {
        _bin.Unpack(out int y, out int m);
        ref readonly var chr = ref CalendarRef;
        chr.ValidateDayOfMonth(y, m, dayOfMonth);
        return new CalendarDate(y, m, dayOfMonth, Cuid);
    }

    /// <summary>
    /// Obtains the sequence of all days in this month instance.
    /// </summary>
    [Pure]
    public IEnumerable<CalendarDate> GetAllDays()
    {
        var cuid = Cuid;
        _bin.Unpack(out int y, out int m);
        // NB: we cannot use CalendarRef (CS8176).
        int daysInMonth = Calendar.Schema.CountDaysInMonth(y, m);

        for (int d = 1; d <= daysInMonth; d++)
        {
            yield return new CalendarDate(y, m, d, cuid);
        }
    }

    //
    // REVIEW(api): "Membership" (disabled)
    //
#if false

    /// <summary>
    /// Determines whether the current instance contains the specified date or not.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="date"/> does not belong to the
    /// calendar of the specified range.</exception>
    [Pure]
    internal bool Contains(CalendarDate date)
    {
        if (date.Cuid != Cuid) ThrowHelpers.BadCuid(nameof(date), Cuid, date.Cuid);

        _bin.Unpack(out int y, out int m);
        return date.Year == y && date.Month == m;
    }

    /// <summary>
    /// Determines whether the current instance contains the specified date or not.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="date"/> does not belong to the
    /// calendar of the specified range.</exception>
    [Pure]
    internal bool Contains(CalendarDay date)
    {
        if (date.Cuid != Cuid) ThrowHelpers.BadCuid(nameof(date), Cuid, date.Cuid);

        _bin.Unpack(out int y, out int m);
        return date.Year == y && date.Month == m;
    }

    /// <summary>
    /// Determines whether the current instance contains the specified date or not.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="date"/> does not belong to the
    /// calendar of the specified range.</exception>
    [Pure]
    internal bool Contains(OrdinalDate date)
    {
        if (date.Cuid != Cuid) ThrowHelpers.BadCuid(nameof(date), Cuid, date.Cuid);

        _bin.Unpack(out int y, out int m);
        return date.Year == y && date.Month == m;
    }

#endif
    #endregion
    #region Adjustments

    /// <summary>
    /// Adjusts the year field to the specified value, yielding a new month.
    /// </summary>
    /// <exception cref="AoorException">The specified month cannot be converted into the new
    /// calendar, the resulting year would be outside its range of years.</exception>
    [Pure]
    public CalendarMonth WithYear(int newYear)
    {
        int m = Month;
        ref readonly var chr = ref CalendarRef;
        // Even when "newYear" is valid, we must re-check "m".
        chr.Scope.ValidateYearMonth(newYear, m, nameof(newYear));
        return new CalendarMonth(newYear, m, Cuid);
    }

    /// <summary>
    /// Adjusts the month field to the specified value, yielding a new month.
    /// </summary>
    /// <exception cref="AoorException">The resulting month would be invalid.</exception>
    [Pure]
    public CalendarMonth WithMonth(int newMonth)
    {
        int y = Year;
        ref readonly var chr = ref CalendarRef;
        // We already know that "y" is valid, we only need to check "newMonth".
        chr.PreValidator.ValidateMonth(y, newMonth, nameof(newMonth));
        return new CalendarMonth(y, newMonth, Cuid);
    }

    #endregion
}

public partial struct CalendarMonth // IEquatable
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="CalendarMonth"/> are equal.
    /// </summary>
    public static bool operator ==(CalendarMonth left, CalendarMonth right) =>
        left._bin == right._bin;

    /// <summary>
    /// Determines whether two specified instances of <see cref="CalendarMonth"/> are not equal.
    /// </summary>
    public static bool operator !=(CalendarMonth left, CalendarMonth right) =>
        left._bin != right._bin;

    /// <summary>
    /// Determines whether this instance is equal to the value of the specified
    /// <see cref="CalendarMonth"/>.
    /// </summary>
    [Pure]
    public bool Equals(CalendarMonth other) => _bin == other._bin;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is CalendarMonth month && Equals(month);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _bin.GetHashCode();
}

public partial struct CalendarMonth // IComparable
{
    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly earlier than the
    /// right one.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="right"/> belongs to a different
    /// calendar from that of <paramref name="left"/>.</exception>
    public static bool operator <(CalendarMonth left, CalendarMonth right) =>
        left.CompareTo(right) < 0;

    /// <summary>
    /// Compares the two specified instances to see if the left one is earlier than or equal to
    /// the right one.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="right"/> belongs to a different
    /// calendar from that of <paramref name="left"/>.</exception>
    public static bool operator <=(CalendarMonth left, CalendarMonth right) =>
        left.CompareTo(right) <= 0;

    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly later than the
    /// right one.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="right"/> belongs to a different
    /// calendar from that of <paramref name="left"/>.</exception>
    public static bool operator >(CalendarMonth left, CalendarMonth right) =>
        left.CompareTo(right) > 0;

    /// <summary>
    /// Compares the two specified instances to see if the left one is later than or equal to
    /// the right one.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="right"/> belongs to a different
    /// calendar from that of <paramref name="left"/>.</exception>
    public static bool operator >=(CalendarMonth left, CalendarMonth right) =>
        left.CompareTo(right) >= 0;

    /// <summary>
    /// Obtains the earlier month of two specified months.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="y"/> belongs to a different
    /// calendar from that of <paramref name="x"/>.</exception>
    [Pure]
    public static CalendarMonth Min(CalendarMonth x, CalendarMonth y) =>
        x.CompareTo(y) < 0 ? x : y;

    /// <summary>
    /// Obtains the later month of two specified months.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="y"/> belongs to a different
    /// calendar from that of <paramref name="x"/>.</exception>
    [Pure]
    public static CalendarMonth Max(CalendarMonth x, CalendarMonth y) =>
        x.CompareTo(y) > 0 ? x : y;

    /// <summary>
    /// Indicates whether this instance is earlier, later or the same as the specified one.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="other"/> does not to the calendar of
    /// the current instance.</exception>
    [Pure]
    public int CompareTo(CalendarMonth other)
    {
        if (other.Cuid != Cuid) ThrowHelpers.BadCuid(nameof(other), Cuid, other.Cuid);

        return Parts.CompareTo(other.Parts);
    }

    /// <summary>
    /// Indicates whether this instance is earlier, later or the same as the specified one.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure]
    internal int CompareFast(CalendarMonth other)
    {
        Debug.Assert(other.Cuid == Cuid);

        return Parts.CompareTo(other.Parts);
    }

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is CalendarMonth month ? CompareTo(month)
        : Throw.NonComparable(typeof(CalendarMonth), obj);
}

public partial struct CalendarMonth // Math ops
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) ✓
    // Friendly alternates do exist but use domain-specific names.

    /// <summary>
    /// Subtracts the two specified months and returns the number of months between them.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="right"/> belongs to a different
    /// calendar from that of <paramref name="left"/>.</exception>
    public static int operator -(CalendarMonth left, CalendarMonth right) =>
        left.CountMonthsSince(right);

    /// <summary>
    /// Adds a number of months to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the range of supported
    /// months.</exception>
    public static CalendarMonth operator +(CalendarMonth value, int months) =>
        value.PlusMonths(months);

    /// <summary>
    /// Subtracts a number of months to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the range of supported
    /// months.</exception>
    public static CalendarMonth operator -(CalendarMonth value, int months) =>
        value.PlusMonths(-months);

    /// <summary>
    /// Adds one month to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the range of supported
    /// months.</exception>
    public static CalendarMonth operator ++(CalendarMonth value) => value.NextMonth();

    /// <summary>
    /// Subtracts one month to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the range of supported
    /// months.</exception>
    public static CalendarMonth operator --(CalendarMonth value) => value.PreviousMonth();

#pragma warning restore CA2225

    /// <summary>
    /// Counts the number of months elapsed since the specified month.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="other"/> does not belong to the
    /// calendar of the current instance.</exception>
    [Pure]
    public int CountMonthsSince(CalendarMonth other)
    {
        if (other.Cuid != Cuid) ThrowHelpers.BadCuid(nameof(other), Cuid, other.Cuid);

        ref readonly var chr = ref CalendarRef;
        return chr.Arithmetic.CountMonthsBetween(other.Parts, Parts);
    }

    /// <summary>
    /// Adds a number of months to this month instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the range of supported
    /// months.</exception>
    [Pure]
    public CalendarMonth PlusMonths(int months)
    {
        ref readonly var chr = ref CalendarRef;
        var ym = chr.Arithmetic.AddMonths(Parts, months);
        return new CalendarMonth(ym, Cuid);
    }

    /// <summary>
    /// Obtains the month after this month instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the range of supported
    /// months.</exception>
    [Pure]
    public CalendarMonth NextMonth()
    {
        ref readonly var chr = ref CalendarRef;
        var ym = chr.Arithmetic.NextMonth(Parts);
        return new CalendarMonth(ym, Cuid);
    }

    /// <summary>
    /// Obtains the month before this month instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the range of supported
    /// months.</exception>
    [Pure]
    public CalendarMonth PreviousMonth()
    {
        ref readonly var chr = ref CalendarRef;
        var ym = chr.Arithmetic.PreviousMonth(Parts);
        return new CalendarMonth(ym, Cuid);
    }

    /// <summary>
    /// Counts the number of years elapsed since the specified month.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="other"/> does not belong to the
    /// calendar of the current instance.</exception>
    [Pure]
    public int CountYearsSince(CalendarMonth other)
    {
        if (other.Cuid != Cuid) ThrowHelpers.BadCuid(nameof(other), Cuid, other.Cuid);

        ref readonly var chr = ref CalendarRef;
        return chr.Math.CountYearsBetweenCore(other, this, out _);
    }

    /// <summary>
    /// Adds a number of years to the year field of this month instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the range of supported
    /// months.</exception>
    [Pure]
    public CalendarMonth PlusYears(int years)
    {
        ref readonly var chr = ref CalendarRef;
        return chr.Math.AddYearsCore(this, years);
    }
}
