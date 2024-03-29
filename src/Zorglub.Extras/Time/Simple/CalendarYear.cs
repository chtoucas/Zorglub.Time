﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

// Explicit layout or not? Beware, both options are not binary compatible.
// Answer: no.
// Pros: less error-prone (no bit manip), the binary format is very easy to
// understand (we don't even need to explain it).
// Cons: different from the other types in this project.
//
// WARNING: If we decide to enable CALENDARYEAR_EXPLICIT_LAYOUT
// then we MUST update the code so that default value for year is 1,
// I'm also not sure about the pack size.
// https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.structlayoutattribute.pack?view=net-6.0
//#define CALENDARYEAR_EXPLICIT_LAYOUT

// To represent individual days within a year, the most natural type is
// OrdinalDate.

namespace Zorglub.Time.Simple;

using System.Numerics;
#if CALENDARYEAR_EXPLICIT_LAYOUT
using System.Runtime.InteropServices;
#endif

using Zorglub.Time.Core;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

/// <summary>
/// Represents a calendar year.
/// <para><see cref="CalendarYear"/> is an immutable struct.</para>
/// </summary>
#if CALENDARYEAR_EXPLICIT_LAYOUT
[StructLayout(LayoutKind.Explicit, Pack = 1)]
#endif
public readonly partial struct CalendarYear :
    ISerializable<CalendarYear, int>,
    // Comparison
    IComparisonOperators<CalendarYear, CalendarYear>,
    // Arithmetic
    IAdditionOperators<CalendarYear, int, CalendarYear>,
    ISubtractionOperators<CalendarYear, int, CalendarYear>,
    IDifferenceOperators<CalendarYear, int>,
    IIncrementOperators<CalendarYear>,
    IDecrementOperators<CalendarYear>
{
#if CALENDARYEAR_EXPLICIT_LAYOUT

    /// <summary>
    /// Represents the year.
    /// </summary>
    [FieldOffset(0)] private readonly short _year; // 2 bytes.

    /// <summary>
    /// Represents the calendar ID.
    /// </summary>
    [FieldOffset(2)] private readonly Cuid _cuid; // 1 byte.

    /// <summary>
    /// Always set to zero.
    /// </summary>
    [FieldOffset(3)] private readonly byte _zero; // 1 byte.

    /// <summary>
    /// Represents the internal binary representation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The data is organised as follows:
    /// <code><![CDATA[
    ///   Year       bbbb bbbb bbbb bbbb
    ///   Cuid                           0bbb bbbb
    ///   (unused)                                 0000 0000
    /// ]]></code>
    /// </para>
    /// </remarks>
    [FieldOffset(0)] private readonly int _bin;

#else

    /// <summary>
    /// <see cref="Cuid"/> is a 7-bit unsigned integer.
    /// <para>This field is a constant equal to 7.</para>
    /// </summary>
    private const int CuidBits = Yemodax.ExtraBits;

    /// <summary>This field is a constant equal to 7.</summary>
    private const int YearShift = CuidBits;

    /// <summary>This field is a constant equal to 127.</summary>
    private const int CuidMask = (1 << CuidBits) - 1;

    /// <summary>
    /// Represents the internal binary representation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The data is organised as follows:
    /// <code><![CDATA[
    ///   Year       bbbb bbbb bbbb bbbb bbbb bbbb b
    ///   Cuid                                      bbb bbbb
    /// ]]></code>
    /// </para>
    /// </remarks>
    private readonly int _bin; // 4 bytes

#endif

    /// <summary>
    /// Initializes a new instance of the <see cref="CalendarYear"/> struct to the specified
    /// year in the Gregorian calendar.
    /// <para>To create an instance for another calendar, see
    /// <see cref="SimpleCalendar.GetCalendarYear(int)"/>.</para>
    /// </summary>
    /// <exception cref="AoorException"><paramref name="year"/> is outside the range of years
    /// supported by the Gregorian calendar.</exception>
    public CalendarYear(int year)
    {
        GregorianProlepticScope.YearsValidator.Validate(year);

#if CALENDARYEAR_EXPLICIT_LAYOUT
        _bin = 0;
        _year = (short)year;
        _cuid = Cuid.Gregorian;
        _zero = 0;
#else
        _bin = Pack(year, (byte)Cuid.Gregorian);
#endif
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CalendarYear"/> struct.
    /// <para>This constructor does NOT validate its parameters.</para>
    /// </summary>
    internal CalendarYear(int y, Cuid cuid)
    {
#if CALENDARYEAR_EXPLICIT_LAYOUT
        _bin = 0;
        _year = (short)y;
        _cuid = cuid;
        _zero = 0;
#else
        _bin = Pack(y, (byte)cuid);
#endif
    }

#if CALENDARYEAR_EXPLICIT_LAYOUT

    /// <summary>
    /// Initializes a new instance of the <see cref="CalendarYear"/> struct.
    /// <para>This constructor does NOT validate its parameters.</para>
    /// <para>This constructor should only be used by <see cref="FromBinary(int)"/>.</para>
    /// </summary>
    private CalendarYear(int bin, bool _)
    {
        _year = 0;
        _cuid = 0;
        _zero = 0;
        _bin = bin;
    }

#endif

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
#if CALENDARYEAR_EXPLICIT_LAYOUT
    public int Year => _year;
#else
    public int Year => unchecked(1 + (_bin >> YearShift));
#endif

    /// <summary>
    /// Returns true if the current instance is a leap year; otherwise returns false.
    /// </summary>
    public bool IsLeap
    {
        get
        {
            ref readonly var chr = ref CalendarRef;
            return chr.Schema.IsLeapYear(Year);
        }
    }

    /// <summary>
    /// Gets the calendar to which belongs the current instance.
    /// </summary>
    /// <remarks>
    /// <para>Performance tip: cache this property locally if used repeatedly within a code
    /// block.</para>
    /// </remarks>
#if CALENDARYEAR_EXPLICIT_LAYOUT
    public Calendar Calendar => SimpleCatalog.GetCalendarUnchecked((int)_cuid);
#else
    public SimpleCalendar Calendar => SimpleCatalog.GetCalendarUnchecked(unchecked(_bin & CuidMask));
#endif

    /// <summary>
    /// Gets the first month of this year instance.
    /// </summary>
    public CalendarMonth FirstMonth => new(Year, 1, Cuid);

    /// <summary>
    /// Gets the last month of this year instance.
    /// </summary>
    public CalendarMonth LastMonth => new(Year, CountMonthsInYear(), Cuid);

    /// <summary>
    /// Gets the first day of this year instance.
    /// </summary>
    public OrdinalDate FirstDay
    {
        get
        {
            var ydoy = Yedoy.AtStartOfYear(Year);
            return new OrdinalDate(ydoy, Cuid);
        }
    }

    /// <summary>
    /// Gets the last day of this year instance.
    /// </summary>
    public OrdinalDate LastDay
    {
        get
        {
            ref readonly var chr = ref CalendarRef;
            var ydoy = chr.Schema.GetOrdinalPartsAtEndOfYear(Year);
            return new OrdinalDate(ydoy, Cuid);
        }
    }

    /// <summary>
    /// Gets the ID of the calendar to which belongs the current instance.
    /// </summary>
#if CALENDARYEAR_EXPLICIT_LAYOUT
    internal Cuid Cuid => _cuid;
#else
    internal Cuid Cuid => unchecked((Cuid)(_bin & CuidMask));
#endif

    /// <summary>
    /// Gets a read-only reference to the calendar to which belongs the current instance.
    /// </summary>
    internal ref readonly SimpleCalendar CalendarRef
    {
        // CIL code size = 12 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref SimpleCatalog.GetCalendarUnsafe((int)Cuid);
    }

    /// <summary>
    /// Returns a culture-independent string representation of the current instance.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        ref readonly var chr = ref CalendarRef;
        return FormattableString.Invariant($"{Year:D4} ({chr})");
    }

#if !CALENDARYEAR_EXPLICIT_LAYOUT

    /// <summary>
    /// Packs the specified month parts into a single 32-bit word.
    /// </summary>
    [Pure]
    // CIL code size = XXX bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int Pack(int y, byte cuid)
    {
        Debug.Assert(Yemodax.MinYear <= y);
        Debug.Assert(y <= Yemodax.MaxYear);

        unchecked
        {
            return ((y - 1) << YearShift) | cuid;
        }
    }

#endif
}

public partial struct CalendarYear // Factories, infos, adjustments
{
    #region Binary serialization

    /// <summary>
    /// Deserializes a 32-bit binary value and recreates an original serialized
    /// <see cref="CalendarYear"/> object.
    /// </summary>
    /// <exception cref="AoorException">Invalid input.</exception>
    [Pure]
    public static CalendarYear FromBinary(int data)
    {
        unchecked
        {
#if CALENDARYEAR_EXPLICIT_LAYOUT
            // Intermediate variable just to get our hands on Cuid and Year.
            // We still have to validate the input.
            var year = new CalendarYear(data, true);
            int y = year.Year;
            var ident = (CalendarId)year.Cuid;
#else
            int y = 1 + (data >> YearShift);
            var ident = (CalendarId)(data & CuidMask);
#endif

            return SimpleCatalog.GetSystemCalendar(ident).GetCalendarYear(y);
        }
    }

    /// <summary>
    /// Serializes the current <see cref="CalendarYear"/> object to a 32-bit binary value that
    /// subsequently can be used to recreate the <see cref="CalendarYear"/> object.
    /// </summary>
    /// <exception cref="NotSupportedException">Binary serialization is only supported for dates
    /// belonging to a system calendar.</exception>
    [Pure]
    public int ToBinary() => Cuid.IsFixed() ? _bin : Throw.NotSupported<int>();

    #endregion
    #region Conversions

    /// <summary>
    /// Converts the current instance to a range of days.
    /// </summary>
    [Pure]
    public Range<OrdinalDate> ToRange() => Range.Create(FirstDay, LastDay);

    /// <summary>
    /// Converts the current instance to a range of months.
    /// </summary>
    [Pure]
    public Range<CalendarMonth> ToMonthRange() => Range.Create(FirstMonth, LastMonth);

    #endregion
    #region Counting

    /// <summary>
    /// Obtains the number of months in this year instance.
    /// </summary>
    [Pure]
    public int CountMonthsInYear()
    {
        ref readonly var chr = ref CalendarRef;
        return chr.Schema.CountMonthsInYear(Year);
    }

    /// <summary>
    /// Obtains the number of days in this year instance.
    /// </summary>
    [Pure]
    public int CountDaysInYear()
    {
        ref readonly var chr = ref CalendarRef;
        return chr.Schema.CountDaysInYear(Year);
    }

    #endregion
    #region Months and days within the year

    /// <summary>
    /// Obtains the month corresponding to the specified month of this year instance.
    /// </summary>
    [Pure]
    public CalendarMonth GetMonthOfYear(int month)
    {
        // We already know that "y" is valid, we only need to check "month".
        ref readonly var chr = ref CalendarRef;
        int y = Year;
        chr.PreValidator.ValidateMonth(y, month);
        return new CalendarMonth(y, month, Cuid);
    }

    /// <summary>
    /// Obtains the sequence of all months in this year instance.
    /// </summary>
    [Pure]
    public IEnumerable<CalendarMonth> GetAllMonths()
    {
        var cuid = Cuid;
        int y = Year;
        int monthsInYear = Calendar.Schema.CountMonthsInYear(y);
        for (int m = 1; m <= monthsInYear; m++)
        {
            yield return new CalendarMonth(y, m, cuid);
        }
    }

    /// <summary>
    /// Obtains the ordinal date corresponding to the specified day of this year instance.
    /// </summary>
    /// <exception cref="AoorException"><paramref name="dayOfYear"/> is outside the range of
    /// valid values.</exception>
    [Pure]
    public OrdinalDate GetDayOfYear(int dayOfYear)
    {
        ref readonly var chr = ref CalendarRef;
        int y = Year;
        chr.PreValidator.ValidateDayOfYear(y, dayOfYear);
        return new OrdinalDate(y, dayOfYear, Cuid);
    }

    /// <summary>
    /// Obtains the sequence of all days in this year instance.
    /// </summary>
    [Pure]
    public IEnumerable<OrdinalDate> GetAllDays()
    {
        var cuid = Cuid;
        int y = Year;
        // NB: we cannot use CalendarRef (CS8176).
        int daysInYear = Calendar.Schema.CountDaysInYear(y);

        for (int doy = 1; doy <= daysInYear; doy++)
        {
            yield return new OrdinalDate(y, doy, cuid);
        }
    }

    //
    // REVIEW(api): "Membership" (disabled)
    //
#if false

    /// <summary>
    /// Determines whether the current instance contains the specified month or not.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="month"/> does not belong to the
    /// calendar of the specified range.</exception>
    [Pure]
    internal bool Contains(CalendarMonth month)
    {
        if (month.Cuid != Cuid) ThrowHelpers.BadCuid(nameof(month), Cuid, month.Cuid);

        return month.Year == Year;
    }

    /// <summary>
    /// Determines whether the current instance contains the specified date or not.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="date"/> does not belong to the
    /// calendar of the specified range.</exception>
    [Pure]
    internal bool Contains(CalendarDate date)
    {
        if (date.Cuid != Cuid) ThrowHelpers.BadCuid(nameof(date), Cuid, date.Cuid);

        return date.Year == Year;
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

        return date.Year == Year;
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

        return date.Year == Year;
    }

#endif
    #endregion
    #region Adjustments

    /// <summary>
    /// Adjusts the year field to the specified value, yielding a new year.
    /// </summary>
    /// <exception cref="AoorException">The resulting year would be invalid.</exception>
    [Pure]
    public CalendarYear WithYear(int newYear)
    {
        // Usefulness of this method is not obvious. Nevertheless, I keep it
        // in case I wanted to create a new calendar year within the -same-
        // calendar.

        ref readonly var chr = ref CalendarRef;
        chr.YearsValidator.Validate(newYear, nameof(newYear));
        return new CalendarYear(newYear, Cuid);
    }

    #endregion
}

public partial struct CalendarYear // IEquatable
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="CalendarYear"/> are equal.
    /// </summary>
    public static bool operator ==(CalendarYear left, CalendarYear right) =>
        left._bin == right._bin;

    /// <summary>
    /// Determines whether two specified instances of <see cref="CalendarYear"/> are not equal.
    /// </summary>
    public static bool operator !=(CalendarYear left, CalendarYear right) =>
        left._bin != right._bin;

    /// <inheritdoc />
    [Pure]
    public bool Equals(CalendarYear other) => _bin == other._bin;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is CalendarYear year && Equals(year);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _bin.GetHashCode();
}

public partial struct CalendarYear // IComparable
{
    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly earlier than the
    /// right one.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="right"/> belongs to a different
    /// calendar from that of <paramref name="left"/>.</exception>
    public static bool operator <(CalendarYear left, CalendarYear right) =>
        left.CompareTo(right) < 0;

    /// <summary>
    /// Compares the two specified instances to see if the left one is earlier than or equal to
    /// the right one.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="right"/> belongs to a different
    /// calendar from that of <paramref name="left"/>.</exception>
    public static bool operator <=(CalendarYear left, CalendarYear right) =>
        left.CompareTo(right) <= 0;

    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly later than the
    /// right one.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="right"/> belongs to a different
    /// calendar from that of <paramref name="left"/>.</exception>
    public static bool operator >(CalendarYear left, CalendarYear right) =>
        left.CompareTo(right) > 0;

    /// <summary>
    /// Compares the two specified instances to see if the left one is later than or equal to
    /// the right one.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="right"/> belongs to a different
    /// calendar from that of <paramref name="left"/>.</exception>
    public static bool operator >=(CalendarYear left, CalendarYear right) =>
        left.CompareTo(right) >= 0;

    /// <summary>
    /// Obtains the earlier year of two specified years.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="y"/> belongs to a different
    /// calendar from that of <paramref name="y"/>.</exception>
    [Pure]
    public static CalendarYear Min(CalendarYear x, CalendarYear y) =>
        x.CompareTo(y) < 0 ? x : y;

    /// <summary>
    /// Obtains the later year of two specified years.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="y"/> belongs to a different
    /// calendar from that of <paramref name="y"/>.</exception>
    [Pure]
    public static CalendarYear Max(CalendarYear x, CalendarYear y) =>
        x.CompareTo(y) > 0 ? x : y;

    /// <summary>
    /// Indicates whether this instance is earlier, later or the same as the specified one.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="other"/> does not belong to the
    /// calendar of the current instance.</exception>
    [Pure]
    public int CompareTo(CalendarYear other)
    {
        if (other.Cuid != Cuid) ThrowHelpers.BadCuid(nameof(other), Cuid, other.Cuid);

        return Year.CompareTo(other.Year);
    }

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is CalendarYear year ? CompareTo(year)
        : Throw.NonComparable(typeof(CalendarYear), obj);
}

public partial struct CalendarYear // Math ops
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) ✓
    // Friendly alternates do exist but use domain-specific names.

    /// <summary>
    /// Subtracts the two specified years and returns the number of years between them.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="right"/> belongs to a different
    /// calendar from that of <paramref name="left"/>.</exception>
    public static int operator -(CalendarYear left, CalendarYear right) =>
        left.CountYearsSince(right);

    /// <summary>
    /// Adds a number of years to the specified year, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the range of supported
    /// years.</exception>
    public static CalendarYear operator +(CalendarYear value, int years) =>
        value.PlusYears(years);

    /// <summary>
    /// Subtracts a number of years to the specified year, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the range of supported
    /// years.</exception>
    public static CalendarYear operator -(CalendarYear value, int years) =>
        value.PlusYears(-years);

    /// <summary>
    /// Adds one year to the specified year, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the range of supported
    /// years.</exception>
    public static CalendarYear operator ++(CalendarYear value) => value.PlusYears(1);

    /// <summary>
    /// Subtracts one year to the specified year, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the range of supported
    /// years.</exception>
    public static CalendarYear operator --(CalendarYear value) => value.PlusYears(-1);

#pragma warning restore CA2225

    /// <summary>
    /// Counts the number of years elapsed since the specified year.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="other"/> does not belong to the
    /// calendar of the current instance.</exception>
    [Pure]
    public int CountYearsSince(CalendarYear other)
    {
        if (other.Cuid != Cuid) ThrowHelpers.BadCuid(nameof(other), Cuid, other.Cuid);

        return Year - other.Year;
    }

    /// <summary>
    /// Adds a number of years to this year instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the range of supported
    /// years.</exception>
    [Pure]
    public CalendarYear PlusYears(int years)
    {
        int y = checked(Year + years);
        ref readonly var chr = ref CalendarRef;
        chr.YearsValidator.CheckOverflow(y);
        return new CalendarYear(y, Cuid);
    }

    /// <summary>
    /// Obtains the year after this year instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the range of supported
    /// years.</exception>
    [Pure]
    public CalendarYear NextYear() => PlusYears(1);

    /// <summary>
    /// Obtains the year before this year instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the range of supported
    /// years.</exception>
    [Pure]
    public CalendarYear PreviousYear() => PlusYears(-1);
}
