﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Simple;

using Zorglub.Time.Core;
using Zorglub.Time.Core.Validation;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

// Le seul constructeur publique ne permet que la création de dates dans le
// calendrier grégorien. Pour les autres, on suit le procédé plus naturel
// qui consiste à d'abord choisir un calendrier pour ensuite créer une date.
// > var calendar = JulianCalendar.Instance;
// > var date = calendar.NewCalendarDate(y, m, d)
// > var date = calendar.GetCalendarDate(dayNumber)
// GetCalendarDate() plutôt que NewCalendarDate() pour mettre en évidence
// qu'il ne s'agit d'un "ctor" mais qu'il y a une transformation cachée,
// p.ex. (y, doy) -> (y, m, d).
// À ce propos, on peut toujours convertir une date après coup, mais cette
// fois il s'agit de méthodes de l'object date:
// > date.ToDayNumber()
// > date.ToCalendarDay()
// > date.ToOrdinalDate()
// Remarque :
// Si on avait un ctor avec pour params (y, m, d, Calendar), on devrait
// vérifier que Calendar n'est pas nul, ce qui est parfaitement inutile
// dans la très grand majorité des cas.

/// <summary>Represents a calendar date.
/// <para><see cref="CalendarDate"/> is an immutable struct.</para></summary>
public readonly partial struct CalendarDate : ISimpleDate<CalendarDate>
{
    /// <summary>Represents the internal binary representation.</summary>
    private readonly Yemodax _bin; // 4 bytes

    /// <summary>Initializes a new instance of the <see cref="CalendarDate"/> struct to the
    /// specified date parts in the Gregorian calendar.
    /// <para>To create an instance for another calendar, see
    /// <see cref="SimpleCalendar.GetDate(int, int, int)"/>.</para>
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a valid date or
    /// <paramref name="year"/> is outside the range of years supported by the Gregorian calendar.
    /// </exception>
    public CalendarDate(int year, int month, int day)
    {
        GregorianProlepticScope.ValidateYearMonthDay(year, month, day);

        _bin = new Yemodax(year, month, day, (int)Cuid.Gregorian);
    }

    /// <summary>Initializes a new instance of the <see cref="CalendarDate"/> struct.
    /// <para>This constructor does NOT validate its parameters.</para></summary>
    internal CalendarDate(int y, int m, int d, Cuid cuid)
    {
        _bin = new Yemodax(y, m, d, (int)cuid);
    }

    /// <summary>Initializes a new instance of the <see cref="CalendarDate"/> struct.
    /// <para>This constructor does NOT validate its parameters.</para></summary>
    internal CalendarDate(Yemoda ymd, Cuid cuid)
    {
        _bin = new Yemodax(ymd, (int)cuid);
    }

    /// <inheritdoc />
    public DayNumber DayNumber
    {
        get
        {
            _bin.Deconstruct(out int y, out int m, out int d);
            ref readonly var chr = ref CalendarRef;
            return chr.Epoch + chr.Schema.CountDaysSinceEpoch(y, m, d);
        }
    }

    /// <inheritdoc />
    public int DaysSinceEpoch
    {
        get
        {
            _bin.Deconstruct(out int y, out int m, out int d);
            ref readonly var chr = ref CalendarRef;
            return chr.Schema.CountDaysSinceEpoch(y, m, d);
        }
    }

    /// <inheritdoc />
    public Ord CenturyOfEra => Ord.FromInt32(Century);

    /// <inheritdoc />
    public int Century => YearNumbering.GetCentury(Year);

    /// <inheritdoc />
    public Ord YearOfEra => Ord.FromInt32(Year);

    /// <inheritdoc />
    public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);

    /// <inheritdoc />
    public int Year => _bin.Year;

    /// <inheritdoc />
    public int Month => _bin.Month;

    /// <inheritdoc />
    public int DayOfYear
    {
        get
        {
            _bin.Deconstruct(out int y, out int m, out int d);
            ref readonly var chr = ref CalendarRef;
            return chr.Schema.GetDayOfYear(y, m, d);
        }
    }

    /// <inheritdoc />
    public int Day => _bin.Day;

    /// <inheritdoc />
    public DayOfWeek DayOfWeek
    {
        get
        {
            ref readonly var chr = ref CalendarRef;
            return chr.GetDayOfWeek(this);
        }
    }

    /// <inheritdoc />
    public bool IsIntercalary
    {
        get
        {
            _bin.Deconstruct(out int y, out int m, out int d);
            ref readonly var chr = ref CalendarRef;
            return chr.Schema.IsIntercalaryDay(y, m, d);
        }
    }

    /// <inheritdoc />
    public bool IsSupplementary
    {
        get
        {
            _bin.Deconstruct(out int y, out int m, out int d);
            ref readonly var chr = ref CalendarRef;
            return chr.Schema.IsSupplementaryDay(y, m, d);
        }
    }

    /// <inheritdoc />
    public SimpleCalendar Calendar => SimpleCatalog.GetCalendarUnchecked(_bin.Extra);

    /// <inheritdoc />
    public CalendarYear CalendarYear => new(Year, Cuid);

    /// <inheritdoc />
    public CalendarMonth CalendarMonth => new(_bin.Yemo, Cuid);

    /// <summary>
    /// Gets the date parts of current instance.
    /// </summary>
    internal Yemoda Parts => _bin.Yemoda;

    /// <summary>Gets the ID of the calendar to which belongs the current instance.</summary>
    internal Cuid Cuid => (Cuid)_bin.Extra;

    /// <summary>Gets a read-only reference to the calendar to which belongs the current instance.
    /// </summary>
    internal ref readonly SimpleCalendar CalendarRef
    {
        // CIL code size = 17 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref SimpleCatalog.GetCalendarUnsafe(_bin.Extra);
    }

    /// <summary>Returns a culture-independent string representation of the current instance.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        _bin.Deconstruct(out int y, out int m, out int d);
        ref readonly var chr = ref CalendarRef;
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({chr})");
    }

    /// <inheritdoc />
    public void Deconstruct(out int year, out int month, out int day) =>
        _bin.Deconstruct(out year, out month, out day);

    /// <inheritdoc />
    public void Deconstruct(out int year, out int dayOfYear)
    {
        _bin.Deconstruct(out year, out int m, out int d);
        ref readonly var chr = ref CalendarRef;
        dayOfYear = chr.Schema.GetDayOfYear(year, m, d);
    }
}

public partial struct CalendarDate // Factories, conversions, adjustments, etc.
{
    #region Binary serialization

    // Pour pouvoir faire la même chose avec un calendrier défini par
    // l'utilisateur, il faudrait offrir au préalable la possibilité de
    // choisir l'ID du calendrier. Cependant, je ne crois pas que ce soit
    // une bonne idée de faire cela (ici en tout cas). En effet, on
    // donnerait ainsi l'impression que FromBinary() marche de manière
    // transparente, alors que pour pouvoir restaurer l'objet il est
    // nécessaire que tous les acteurs se soient accordés quant aux IDs de
    // tous les calendriers volatiles ; c'est bien trop demander.
    // De plus, il nous faudrait utiliser SimpleCatalog.GetCalendar(cuid)
    // qui retournerait null si le calendrier correspondant n'est pas déjà
    // chargé en mémoire.

    // REVIEW(api): FromBinary() throws an arg exn for param "ident", not "data";
    // idem with the other datatypes.

    /// <summary>Deserializes a 32-bit binary value and recreates an original serialized
    /// <see cref="CalendarDate"/> object.</summary>
    /// <exception cref="AoorException">The matching calendar is not a system calendar.</exception>
    [Pure]
    public static CalendarDate FromBinary(int data)
    {
        var bin = new Yemodax(data);
        bin.Deconstruct(out int y, out int m, out int d);
        var ident = (CalendarId)bin.Extra;
        return SimpleCatalog.GetSystemCalendar(ident).GetDate(y, m, d);
    }

    /// <summary>Serializes the current <see cref="CalendarDate"/> object to a 32-bit binary value
    /// that subsequently can be used to recreate the <see cref="CalendarDate"/> object.</summary>
    /// <exception cref="NotSupportedException">Binary serialization is only supported for dates
    /// belonging to a system calendar.</exception>
    [Pure]
    public int ToBinary() => Cuid.IsFixed() ? _bin.ToBinary() : Throw.NotSupported<int>();

    #endregion
    #region Conversions

    /// <inheritdoc />
    [Pure]
    public CalendarDay ToCalendarDay() => new(DaysSinceEpoch, Cuid);

    [Pure]
    CalendarDate ISimpleDate.ToCalendarDate() => this;

    /// <inheritdoc />
    [Pure]
    public OrdinalDate ToOrdinalDate()
    {
        _bin.Deconstruct(out int y, out int m, out int d);
        ref readonly var chr = ref CalendarRef;
        int doy = chr.Schema.GetDayOfYear(y, m, d);
        return new OrdinalDate(y, doy, Cuid);
    }

    #endregion
    #region Counting

    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInYear()
    {
        _bin.Deconstruct(out int y, out int m, out int d);
        ref readonly var chr = ref CalendarRef;
        return chr.Schema.CountDaysInYearBefore(y, m, d);
    }

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInYear()
    {
        _bin.Deconstruct(out int y, out int m, out int d);
        ref readonly var chr = ref CalendarRef;
        return chr.Schema.CountDaysInYearAfter(y, m, d);
    }

    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInMonth() => Day - 1;

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInMonth()
    {
        _bin.Deconstruct(out int y, out int m, out int d);
        ref readonly var chr = ref CalendarRef;
        return chr.Schema.CountDaysInMonthAfter(y, m, d);
    }

    #endregion
    #region Adjustments

    /// <inheritdoc />
    [Pure]
    public CalendarDate Previous(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        ref readonly var chr = ref CalendarRef;
        int δ = dayOfWeek - chr.GetDayOfWeek(this);
        // 0 <= dayOfWeek <= 6  =>  |δ| <= 6  =>  -7 <= (δ - 7 or δ) <= -1.
        var ymd = chr.Arithmetic.AddDaysViaDayOfMonth(Parts, δ >= 0 ? δ - CalendricalConstants.DaysInWeek : δ);
        return new CalendarDate(ymd, Cuid);
    }

    /// <inheritdoc />
    [Pure]
    public CalendarDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        ref readonly var chr = ref CalendarRef;
        int δ = dayOfWeek - chr.GetDayOfWeek(this);
        if (δ == 0) { return this; }
        // |δ| <= 6 and δ != 0  =>  -6 <= (δ - 7 or δ) <= -1.
        var ymd = chr.Arithmetic.AddDaysViaDayOfMonth(Parts, δ > 0 ? δ - CalendricalConstants.DaysInWeek : δ);
        return new CalendarDate(ymd, Cuid);
    }

    /// <inheritdoc />
    [Pure]
    public CalendarDate Nearest(DayOfWeek dayOfWeek)
    {
        ref readonly var chr = ref CalendarRef;
        _bin.Deconstruct(out int y, out int m, out int d);
        var dayNumber = chr.Epoch + chr.Schema.CountDaysSinceEpoch(y, m, d);
        var nearest = dayNumber.Nearest(dayOfWeek);
        chr.Domain.CheckOverflow(nearest);
        var ymd = chr.Schema.GetDateParts(nearest - chr.Epoch);
        return new CalendarDate(ymd, Cuid);
    }

    /// <inheritdoc />
    [Pure]
    public CalendarDate NextOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        ref readonly var chr = ref CalendarRef;
        int δ = dayOfWeek - chr.GetDayOfWeek(this);
        if (δ == 0) { return this; }
        // |δ| <= 6 and δ != 0  =>  1 <= (δ + 7 or δ) <= 6.
        var ymd = chr.Arithmetic.AddDaysViaDayOfMonth(Parts, δ < 0 ? δ + CalendricalConstants.DaysInWeek : δ);
        return new CalendarDate(ymd, Cuid);
    }

    /// <inheritdoc />
    [Pure]
    public CalendarDate Next(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        ref readonly var chr = ref CalendarRef;
        int δ = dayOfWeek - chr.GetDayOfWeek(this);
        // |δ| <= 6  =>  1 <= (δ + 7 or δ) <= 7.
        var ymd = chr.Arithmetic.AddDaysViaDayOfMonth(Parts, δ <= 0 ? δ + CalendricalConstants.DaysInWeek : δ);
        return new CalendarDate(ymd, Cuid);
    }

    #endregion
}

public partial struct CalendarDate // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(CalendarDate left, CalendarDate right) => left._bin == right._bin;

    /// <inheritdoc />
    public static bool operator !=(CalendarDate left, CalendarDate right) => left._bin != right._bin;

    /// <inheritdoc />
    [Pure]
    public bool Equals(CalendarDate other) => _bin == other._bin;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is CalendarDate date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _bin.GetHashCode();
}

public partial struct CalendarDate // IComparable
{
    /// <inheritdoc />
    /// <exception cref="ArgumentException"><paramref name="right"/> belongs to a different calendar
    /// from that of <paramref name="left"/>.</exception>
    public static bool operator <(CalendarDate left, CalendarDate right) =>
        left.CompareTo(right) < 0;

    /// <inheritdoc />
    /// <exception cref="ArgumentException"><paramref name="right"/> belongs to a different calendar
    /// from that of <paramref name="left"/>.</exception>
    public static bool operator <=(CalendarDate left, CalendarDate right) =>
        left.CompareTo(right) <= 0;

    /// <inheritdoc />
    /// <exception cref="ArgumentException"><paramref name="right"/> belongs to a different calendar
    /// from that of <paramref name="left"/>.</exception>
    public static bool operator >(CalendarDate left, CalendarDate right) =>
        left.CompareTo(right) > 0;

    /// <inheritdoc />
    /// <exception cref="ArgumentException"><paramref name="right"/> belongs to a different calendar
    /// from that of <paramref name="left"/>.</exception>
    public static bool operator >=(CalendarDate left, CalendarDate right) =>
        left.CompareTo(right) >= 0;

    /// <inheritdoc />
    /// <exception cref="ArgumentException"><paramref name="y"/> belongs to a different calendar
    /// from that of <paramref name="y"/>.</exception>
    [Pure]
    public static CalendarDate Min(CalendarDate x, CalendarDate y) =>
        x.CompareTo(y) < 0 ? x : y;

    /// <inheritdoc />
    /// <exception cref="ArgumentException"><paramref name="y"/> belongs to a different calendar
    /// from that of <paramref name="y"/>.</exception>
    [Pure]
    public static CalendarDate Max(CalendarDate x, CalendarDate y) =>
        x.CompareTo(y) > 0 ? x : y;

    /// <inheritdoc />
    /// <exception cref="ArgumentException"><paramref name="other"/> does not belong to the calendar
    /// of the current instance.</exception>
    [Pure]
    public int CompareTo(CalendarDate other)
    {
        if (other.Cuid != Cuid) ThrowHelpers.BadCuid(nameof(other), Cuid, other.Cuid);

        return Parts.CompareTo(other.Parts);
    }

    /// <summary>Indicates whether this instance is earlier, later or the same as the specified one.
    /// <para>This method does NOT validate its parameter.</para></summary>
    [Pure]
    internal int CompareFast(CalendarDate other)
    {
        Debug.Assert(other.Cuid == Cuid);

        return Parts.CompareTo(other.Parts);
    }

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is CalendarDate date ? CompareTo(date)
        : Throw.NonComparable(typeof(CalendarDate), obj);
}

public partial struct CalendarDate // Math ops
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) ✓
    // Friendly alternates do exist but use domain-specific names.

    /// <summary>Subtracts the two specified dates and returns the number of days between them.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="right"/> belongs to a different calendar
    /// from that of <paramref name="left"/>.</exception>
    public static int operator -(CalendarDate left, CalendarDate right) =>
        left.CountDaysSince(right);

    /// <summary>Adds a number of days to the specified date, yielding a new date.</summary>
    /// <exception cref="OverflowException">The operation would overflow either the capacity of
    /// <see cref="Int32"/> or the range of supported dates.</exception>
    public static CalendarDate operator +(CalendarDate value, int days) => value.PlusDays(days);

    /// <summary>Subtracts a number of days to the specified date, yielding a new date.</summary>
    /// <exception cref="OverflowException">The operation would overflow either the capacity of
    /// <see cref="Int32"/> or the range of supported dates.</exception>
    public static CalendarDate operator -(CalendarDate value, int days) => value.PlusDays(-days);

    /// <summary>Adds one day to the specified date, yielding a new date.</summary>
    /// <exception cref="OverflowException">The operation would overflow the latest supported date.
    /// </exception>
    public static CalendarDate operator ++(CalendarDate value) => value.NextDay();

    /// <summary>Subtracts one day to the specified date, yielding a new date.</summary>
    /// <exception cref="OverflowException">The operation would overflow the earliest supported date.
    /// </exception>
    public static CalendarDate operator --(CalendarDate value) => value.PreviousDay();

#pragma warning restore CA2225

    /// <inheritdoc />
    [Pure]
    public int CountDaysSince(CalendarDate other)
    {
        if (other.Cuid != Cuid) ThrowHelpers.BadCuid(nameof(other), Cuid, other.Cuid);

        ref readonly var chr = ref CalendarRef;
        return chr.Arithmetic.CountDaysBetween(other.Parts, Parts);
    }

    /// <inheritdoc />
    [Pure]
    public CalendarDate PlusDays(int days)
    {
        ref readonly var chr = ref CalendarRef;
        var ymd = chr.Arithmetic.AddDays(Parts, days);
        return new CalendarDate(ymd, Cuid);
    }

    /// <inheritdoc />
    [Pure]
    public CalendarDate NextDay()
    {
        ref readonly var chr = ref CalendarRef;
        var ymd = chr.Arithmetic.NextDay(Parts);
        return new CalendarDate(ymd, Cuid);
    }

    /// <inheritdoc />
    [Pure]
    public CalendarDate PreviousDay()
    {
        ref readonly var chr = ref CalendarRef;
        var ymd = chr.Arithmetic.PreviousDay(Parts);
        return new CalendarDate(ymd, Cuid);
    }

    /// <summary>Counts the number of months elapsed since the specified date.</summary>
    /// <exception cref="ArgumentException"><paramref name="other"/> does not belong to the calendar
    /// of the current instance.</exception>
    [Pure]
    public int CountMonthsSince(CalendarDate other)
    {
        if (other.Cuid != Cuid) ThrowHelpers.BadCuid(nameof(other), Cuid, other.Cuid);

        ref readonly var chr = ref CalendarRef;
        return chr.Math.CountMonthsBetweenCore(other, this, out _);
    }

    /// <summary>Adds a number of months to the month field of this date instance, yielding a new
    /// date.</summary>
    [Pure]
    public CalendarDate PlusMonths(int months)
    {
        ref readonly var chr = ref CalendarRef;
        return chr.Math.AddMonthsCore(this, months);
    }

    /// <summary>Counts the number of years elapsed since the specified date.</summary>
    /// <exception cref="ArgumentException"><paramref name="other"/> does not belong to the calendar
    /// of the current instance.</exception>
    [Pure]
    public int CountYearsSince(CalendarDate other)
    {
        if (other.Cuid != Cuid) ThrowHelpers.BadCuid(nameof(other), Cuid, other.Cuid);

        ref readonly var chr = ref CalendarRef;
        return chr.Math.CountYearsBetweenCore(other, this, out _);
    }

    /// <summary>Adds a number of years to the year field of this date instance, yielding a new date.
    /// </summary>
    [Pure]
    public CalendarDate PlusYears(int years)
    {
        ref readonly var chr = ref CalendarRef;
        return chr.Math.AddYearsCore(this, years);
    }
}
