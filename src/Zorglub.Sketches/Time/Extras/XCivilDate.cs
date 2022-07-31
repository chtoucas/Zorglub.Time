// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

//#define CIVILDATE_PLAIN_DAYOFWEEK

// Optimised Gregorian date type. Useful for performance testing.

// We restrict dates to years in the range from 1 to 9999, nevertheless all
// formulae give a meaningful result even for years less than 0, but only if we
// replace the division (/) and modulo (%) operations by the genuine mathematical
// ones --- the right-shift's and left-shift's do not need such adjustments;
// see MathOperations. It also does not make sense to go beyond the 9999 limit.
// By the time we reach that point in time (and if we still use the Gregorian
// calendar) the leap rule will certainly have changed long time ago just to
// stay in sync with the solar year.
//
// We choose to represent a date by a triple (year, month, day), not by a count
// of days since a fixed date. This is the most natural choice, but this means
// that sometimes we do have to compute the property DaysSinceEpoch on-the-fly.
// This is mostly the case for things related to the day of the week or when
// adding days or subtracting two dates.
// DateTime from the BCL use the opposite strategy. It makes perfect sense
// since internally a DateTime is always of Gregorian type, and support for any
// other calendar is done by interconversion.
//
// References:
// - DateTime from the BCL
// - [NodaTime](https://github.com/nodatime/nodatime)
// - [chrono](https://github.com/HowardHinnant/date) (C++ library)
// - [Doomsday algorithm](http://rudy.ca/doomsday.html)
//   and [Doomsday rule](https://en.wikipedia.org/wiki/Doomsday_rule)
// - https://en.wikipedia.org/wiki/Civil_calendar

namespace Zorglub.Time.Extras
{
    using System.ComponentModel;

    using Zorglub.Time;
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Schemas;
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Hemerology;

    // TODO(code): use CivilFormulae and unsigned int.

    /// <summary>
    /// Represents a date within the non-proleptic (but retropolated) Gregorian calendar.
    /// <para><see cref="XCivilDate"/> is an immutable struct.</para>
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public readonly partial struct XCivilDate :
        IDate<XCivilDate>,
        IAdjustableDate<XCivilDate>,
        IMinMaxValue<XCivilDate>
    {
        /// <summary>
        /// Represents the smallest possible value of the number of consecutive days since the epoch
        /// of the Gregorian calendar.
        /// <para>This field is a constant equal to 0.</para>
        /// </summary>
        // 01/01/0001.
        internal const int MinDaysSinceEpoch = 0;

        /// <summary>
        /// Represents the largest possible value of the number of consecutive days since the epoch
        /// of the Gregorian calendar.
        /// <para>This field is a constant equal to 3_652_058.</para>
        /// </summary>
        // 31/12/9999.
        internal const int MaxDaysSinceEpoch = 3_652_058;

        /// <summary>
        /// Represents the earliest supported year.
        /// <para>This field is a constant equal to 1.</para>
        /// </summary>
        public const int MinYear = 1;

        /// <summary>
        /// Represents the latest supported year.
        /// <para>This field is a constant equal to 9999.</para>
        /// </summary>
        public const int MaxYear = 9999;

        /// <summary>
        /// Represents the minimum total number of days there is at least in a year.
        /// <para>This field is a constant equal to 365.</para>
        /// </summary>
        private const int MinDaysInYear = GJSchema.DaysInCommonYear;

        /// <summary>
        /// Represents the minimum total number of days there is at least in a month.
        /// <para>This field is a constant equal to 28.</para>
        /// </summary>
        private const int MinDaysInMonth = 28;

        /// <summary>
        /// Represents the epoch.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly DayNumber s_Epoch = DayZero.NewStyle;

        /// <summary>
        /// Represents the domain, the interval of supported <see cref="DayNumber"/>.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly Range<DayNumber> s_Domain =
            Range.Create(s_Epoch + MinDaysSinceEpoch, s_Epoch + MaxDaysSinceEpoch);

        /// <summary>
        /// Represents the smallest possible value of a <see cref="XCivilDate"/>.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly XCivilDate s_MinValue = new(MinYear, 1, 1);

        /// <summary>
        /// Represents the largest possible value of a <see cref="XCivilDate"/>.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly XCivilDate s_MaxValue = new(MaxYear, 12, 31);

        /// <summary>
        /// Represents the binary data stored in the current instance.
        /// </summary>
        private readonly int _bin;

        /// <summary>
        /// Initializes a new instance of the <see cref="XCivilDate"/> struct from the specified year,
        /// month and day.
        /// </summary>
        /// <exception cref="AoorException">The specified date parts do not form a valid Gregorian
        /// date within the Common Era on or before year 9999.</exception>
        public XCivilDate(int year, int month, int day)
        {
            if (year < MinYear || year > MaxYear) Throw.ArgumentOutOfRange(nameof(year));
            if (month < 1 || month > 12) Throw.ArgumentOutOfRange(nameof(month));
            // In the vast majority of cases, we can avoid the computation of
            // the exact number of days in the month.
            if (day < 1
                || (day > MinDaysInMonth && day > GregorianFormulae.CountDaysInMonth(year, month)))
            {
                Throw.ArgumentOutOfRange(nameof(day));
            }

            _bin = Pack(year, month, day);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XCivilDate"/> struct directly from the
        /// specified binary data.
        /// <para>This constructor does NOT validate its parameter.</para>
        /// <para>See also <seealso cref="Pack(int, int, int)"/>.</para>
        /// </summary>
        private XCivilDate(int bin)
        {
            DebugCheckBinaryData(bin);
            _bin = bin;
        }

        /// <summary>
        /// Gets the domain, the interval of supported <see cref="DayNumber"/>.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static Range<DayNumber> Domain => s_Domain;

        /// <summary>
        /// Gets the earliest date supported by the <see cref="XCivilDate"/> type: Monday, January
        /// 1st, 1 CE.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static XCivilDate MinValue => s_MinValue;

        /// <summary>
        /// Gets the latest date supported by the <see cref="XCivilDate"/> type: Friday, December 31,
        /// 9999 CE.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static XCivilDate MaxValue => s_MaxValue;

        /// <inheritdoc />
        public Ord CenturyOfEra => Ord.FromInt32(Century);

        /// <inheritdoc />
        public int Century => MathN.AdjustedDivide(Year, 100);

        /// <inheritdoc />
        public Ord YearOfEra => Ord.FromInt32(Year);

        /// <inheritdoc />
        public int YearOfCentury => MathN.AdjustedModulo(Year, 100);

        /// <inheritdoc />
        /// <remarks>
        /// <para>The result is in the range from 1 to 9999.</para>
        /// </remarks>
        public int Year => 1 + (_bin >> 9);

        /// <inheritdoc />
        /// <remarks>
        /// <para>The result is in the range from 1 to 12.</para>
        /// </remarks>
        public int Month => 1 + ((_bin >> 5) & MonthMask);

        /// <inheritdoc />
        /// <remarks>
        /// <para>The result is in the range from 1 to 366.</para>
        /// </remarks>
        public int DayOfYear
        {
            get
            {
                Unpack(out int y, out int m, out int d);
                return GregorianFormulae.CountDaysInYearBeforeMonth(y, m) + d;
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// <para>The result is in the range from 1 to 31.</para>
        /// </remarks>
        public int Day => 1 + (_bin & DayMask);

        /// <inheritdoc />
        public DayOfWeek DayOfWeek
        {
            get
            {
#if CIVILDATE_PLAIN_DAYOFWEEK
                // The epoch of the Gregorian calendar is a Monday.
                return (DayOfWeek)((uint)((int)DayOfWeek.Monday + DaysSinceEpoch) % 7);
#else
                Unpack(out int y, out int m, out int d);
                uint doomsday = GetDoomsday(y, m);
                return (DayOfWeek)((doomsday + (uint)d) % 7);
#endif
            }
        }

        /// <summary>
        /// Gets the ISO day of the week.
        /// </summary>
        public IsoDayOfWeek IsoDayOfWeek
        {
            get
            {
#if CIVILDATE_PLAIN_DAYOFWEEK
                return (IsoDayOfWeek)MathN.AdjustedModulo((int)DayOfWeek.Monday + DaysSinceEpoch, 7);
#else
                Unpack(out int y, out int m, out int d);
                uint doomsday = GetDoomsday(y, m);
                return (IsoDayOfWeek)MathU.AdjustedModulo(doomsday + (uint)d, 7);
#endif
            }
        }

        /// <summary>
        /// Gets the week of the year.
        /// <para>This is NOT the ISO week number.</para>
        /// </summary>
        /// <remarks>
        /// <para>A week starts on Monday and weeks at both ends of a year may be incomplete (less
        /// than 7 days, even a one-day week is OK).</para>
        /// <para>In the vast majority of cases, we count 52 whole weeks in a year and one
        /// incomplete week.</para>
        /// <para>Long years are 54 weeks long (52 whole weeks and 2 one-day weeks), something that
        /// can only happen with a leap year starting on a Sunday --- long years always end on a
        /// Monday.</para>
        /// </remarks>
        public int WeekOfYear
        {
            get
            {
                uint dow = GetIsoDayOfWeekAtStartOfYear(Year);
                return (int)(((uint)DayOfYear + 5 + dow) / 7);
            }
        }

        /// <inheritdoc />
        public bool IsIntercalary => ObMonthDay == __IntercalaryDay;

        /// <inheritdoc />
        public bool IsSupplementary => false;

        /// <summary>
        /// Gets the number of consecutive days since the epoch of the Gregorian calendar.
        /// <para>The result is in the range from <see cref="MinDaysSinceEpoch"/> to
        /// <see cref="MaxDaysSinceEpoch"/>.</para>
        /// </summary>
        internal int DaysSinceEpoch
        {
            get
            {
                Unpack(out int y, out int m, out int d);
                return CivilFormulae.CountDaysSinceEpoch(y, m, d);
            }
        }

        /// <summary>
        /// Gets the year-month part of the binary data.
        /// </summary>
        private int ObYearMonth => _bin >> 5;

        /// <summary>
        /// Gets the month-day part of the binary data.
        /// </summary>
        private int ObMonthDay => _bin & ((1 << 9) - 1);

        /// <summary>
        /// Gets a string representation of the binary data stored in the current instance.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string DebuggerDisplay => Convert.ToString(_bin, 2);

        /// <summary>
        /// Returns a culture-independent string representation of the current instance.
        /// <para>The result is compatible with the (extended) ISO 8601 format.
        /// </para>
        /// </summary>
        [Pure]
        public override string ToString()
        {
            Unpack(out int y, out int m, out int d);
            return FormattableString.Invariant($"{y:D4}-{m:D2}-{d:D2}");
        }

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        public void Deconstruct(out int year, out int month, out int day) =>
            Unpack(out year, out month, out day);

        /// <summary>
        /// Obtains the current date on this machine, expressed in local time, not UTC.
        /// </summary>
        [Pure]
        public static XCivilDate Today()
        {
            var now = DateTime.Now;
            return new XCivilDate(Pack(now.Year, now.Month, now.Day));
        }

        /// <summary>
        /// Obtains the current date on this machine, expressed as UTC.
        /// </summary>
        [Pure]
        public static XCivilDate UtcToday()
        {
            var now = DateTime.UtcNow;
            return new XCivilDate(Pack(now.Year, now.Month, now.Day));
        }

        /// <summary>
        /// Creates a new instance of <see cref="XCivilDate"/> from the specified ordinal date.
        /// </summary>
        /// <exception cref="AoorException">The specified ordinal date parts do not form a valid
        /// Gregorian date within the Common Era on or before year 9999.</exception>
        [Pure]
        public static XCivilDate FromOrdinalDate(int year, int dayOfYear)
        {
            if (year < MinYear || year > MaxYear) Throw.ArgumentOutOfRange(nameof(year));
            if (dayOfYear < 1
                || (dayOfYear > MinDaysInYear
                    && dayOfYear > GregorianFormulae.CountDaysInYear(year)))
            {
                Throw.ArgumentOutOfRange(nameof(dayOfYear));
            }

            return FromOrdinalDateImpl(year, dayOfYear);
        }

        /// <summary>
        /// Creates a new instance of <see cref="XCivilDate"/> from the specified number of
        /// consecutive days since the epoch of the Gregorian calendar.
        /// <para>This method does NOT validate its parameter.</para>
        /// </summary>
        [Pure]
        internal static XCivilDate FromDaysSinceEpoch(int daysSinceEpoch)
        {
            CivilFormulae.GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
            return new(Pack(y, m, d));
        }

        /// <summary>
        /// Creates a new instance of <see cref="XCivilDate"/> from the specified ordinal date.
        /// <para>This method does NOT validate its parameter.</para>
        /// </summary>
        [Pure]
        private static XCivilDate FromOrdinalDateImpl(int y, int doy)
        {
            if (doy < 60)
            {
                doy--;
                return new(Pack(y, 1 + doy / 31, 1 + doy % 31));
            }
            else if (doy == 60)
            {
                int bin = ((y - 1) << 9)
                    | (GregorianFormulae.IsLeapYear(y) ? __IntercalaryDay : __StartOfMarch);
                return new(bin);
            }
            else
            {
                doy -= GregorianFormulae.IsLeapYear(y) ? 61 : 60;

                int m = (int)((uint)(5 * doy + 2) / 153);
                int d = 1 + doy - (int)((uint)(153 * m + 2) / 5);

                return new(Pack(y, m + 3, d));
            }
        }
    }

    public partial struct XCivilDate // Binary data helpers
    {
        private const int MonthMask = (1 << 4) - 1;
        private const int DayMask = (1 << 5) - 1;

        // Sentinels: binary data for selected values of (month, day).
        private const int __EndOfFebruary = ((2 - 1) << 5) | (28 - 1);
        private const int __IntercalaryDay = ((2 - 1) << 5) | (29 - 1);
        private const int __StartOfMarch = ((3 - 1) << 5) | (1 - 1);
        private const int __StartOfYear = ((1 - 1) << 5) | (1 - 1);
        private const int __EndOfYear = ((12 - 1) << 5) | (31 - 1);

        /// <summary>
        /// Deserializes a 32-bit binary value and recreates the original
        /// serialized <see cref="XCivilDate"/> object.
        /// </summary>
        /// <exception cref="ArgumentException">The specified binary data is
        /// not well-formed.</exception>
        [Pure]
        public static XCivilDate FromBinary(int data)
        {
            ValidateBinaryData(data);
            return new(data);
        }

        // REVIEW: serialization, use uint?
        // We don't use the high bits... bin is always >= 0.
        // It would make ValidateBinaryData() a bit simpler.

        /// <summary>
        /// Serializes the current instance to a 32-bit binary value that
        /// subsequently can be used to recreate the <see cref="XCivilDate"/>
        /// object using <see cref="FromBinary(int)"/>.
        /// </summary>
        [Pure]
        public int ToBinary() => _bin;

        /// <summary>
        /// Validates the specified binary data.
        /// </summary>
        /// <exception cref="ArgumentException">The specified binary data is
        /// not well-formed.</exception>
        private static void ValidateBinaryData(int data)
        {
            if (data >> 23 != 0)
            {
                // The 9 high bits are always equal to zero; see comments in Pack().
                Throw.BadBinaryInput();
            }

            int y = 1 + (data >> 9);
            if (y < MinYear || y > MaxYear)
            {
                Throw.BadBinaryInput();
            }

            int m = 1 + ((data >> 5) & MonthMask);
            if (m < 1 || m > 12)
            {
                Throw.BadBinaryInput();
            }

            int d = 1 + (data & DayMask);
            if (d < 1 || (d > MinDaysInMonth && d > GregorianFormulae.CountDaysInMonth(y, m)))
            {
                Throw.BadBinaryInput();
            }
        }

        [Conditional("DEBUG")]
        [ExcludeFromCodeCoverage]
        private static void DebugCheckBinaryData(int bin) => ValidateBinaryData(bin);

        /// <summary>
        /// Packs the specified date parts.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The data is organised as follows:
        /// <code><![CDATA[
        ///   Year - 1  0000 0000 0bbb bbbb bbbb bbb
        ///   Month - 1                             b bbb
        ///   Day - 1                                    b bbbb
        /// ]]></code>
        /// </para>
        /// </remarks>
        [Pure]
        // CIL code size = 17 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Pack(int y, int m, int d) =>
            // Internal representation (from left to right):
            //   -      9-bit 2's complement signed integer
            //   year  14-bit unsigned integer
            //   month  4-bit unsigned integer
            //   day    5-bit unsigned integer
            // At worst the year will only fill 14 bits out of the 23 possible,
            // which leaves a bit more than one blank byte.
            ((y - 1) << 9) | ((m - 1) << 5) | (d - 1);

        /// <summary>
        /// Unpacks the binary data.
        /// </summary>
        //
        // CIL code size = 34 bytes > 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Unpack(out int y, out int m, out int d)
        {
            // If we do not copy _bin locally, the code size increases and the
            // chances for the method to be inlined decrease.
            int bin = _bin;

            y = 1 + (bin >> 9);
            m = 1 + ((bin >> 5) & MonthMask);
            d = 1 + (bin & DayMask);
        }
    }

    // NB: All static methods taken from GregorianSchema do work "optimally"
    // with positive years.
    public partial struct XCivilDate // Static helpers
    {
        /// <summary>
        /// Obtains the ISO day of the week of the first day of the specified
        /// year.
        /// </summary>
        [Pure]
        internal static uint GetIsoDayOfWeekAtStartOfYear(int y)
        {
#if CIVILDATE_PLAIN_DAYOFWEEK
            var startOfYear = new CivilDate((y << 9) | __StartOfYear);
            return (uint)startOfYear.IsoDayOfWeek;
#else
            // Calculation of IsoDayOfWeek with m = d = 1.
            uint uy = (uint)y; // y >= 1
            uy--;
            uint c = uy / 100;
            return MathU.AdjustedModulo(1 + uy + (uy >> 2) - c + (c >> 2), 7);
#endif
        }

        // Copied from DoomsdayRule.GetGregorianDoomsday(), but here y >= 1.
        [Pure]
        private static uint GetDoomsday(int y, int m)
        {
            uint uy = (uint)y; // y >= 1
            uint α = (uint)(23 * m) / 9;
            if (m < 3)
            {
                uy--; // uy >= 1
                α -= 2;
            }
            else
            {
                α += 2;
            }

            uint c = uy / 100;

            // The operation does not overflow (>= 0).
            return α + uy + (uy >> 2) - c + (c >> 2);
        }
    }

    public partial struct XCivilDate // Conversions, adjustments...
    {
        /// <summary>
        /// Creates a new instance of <see cref="XCivilDate"/> from the
        /// specified day number.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// supported values.</exception>
        [Pure]
        public static XCivilDate FromDayNumber(DayNumber dayNumber)
        {
            s_Domain.Validate(dayNumber);
            return FromDaysSinceEpoch(dayNumber - s_Epoch);
        }

        /// <inheritdoc />
        [Pure]
        public DayNumber ToDayNumber() => s_Epoch + DaysSinceEpoch;

        /// <summary>
        /// Creates a new instance of <see cref="XCivilDate"/> from the
        /// specified <see cref="DateTime"/> object.
        /// </summary>
        [Pure]
        public static XCivilDate FromDateTime(DateTime date) =>
            new(date.Year, date.Month, date.Day);

        /// <summary>
        /// Converts the current instance to a <see cref="DateTime"/> object.
        /// </summary>
        [Pure]
        public DateTime ToDateTime()
        {
            Unpack(out int y, out int m, out int d);
            return new DateTime(y, m, d);
        }

        /// <inheritdoc />
        [Pure]
        public int CountElapsedDaysInYear()
        {
            Unpack(out int y, out int m, out int d);
            return GregorianFormulae.CountDaysInYearBeforeMonth(y, m) + d - 1;
        }

        /// <inheritdoc />
        [Pure]
        public int CountRemainingDaysInYear()
        {
            Unpack(out int y, out int m, out int d);
            return GregorianFormulae.CountDaysInYear(y)
                - GregorianFormulae.CountDaysInYearBeforeMonth(y, m) - d;
        }

        /// <inheritdoc />
        [Pure]
        public int CountElapsedDaysInMonth() => Day - 1;

        /// <inheritdoc />
        [Pure]
        public int CountRemainingDaysInMonth()
        {
            Unpack(out int y, out int m, out int d);
            return GregorianFormulae.CountDaysInMonth(y, m) - d;
        }

        #region Adjusters

        /// <summary>
        /// Obtains the first day of the year to which belongs the specified day.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="date"/> is null.</exception>
        [Pure]
        public static XCivilDate GetStartOfYear(XCivilDate date) =>
            new(((date.Year - 1) << 9) | __StartOfYear);

        /// <summary>
        /// Obtains the last day of the year to which belongs the specified day.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="date"/> is null.</exception>
        [Pure]
        public static XCivilDate GetEndOfYear(XCivilDate date) =>
            new(((date.Year - 1) << 9) | __EndOfYear);

        /// <summary>
        /// Obtains the first day of the month to which belongs the specified day.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="date"/> is null.</exception>
        [Pure]
        public static XCivilDate GetStartOfMonth(XCivilDate date)
        {
            date.Unpack(out int y, out int m, out _);
            return new(Pack(y, m, 1));
        }

        /// <summary>
        /// Obtains the last day of the month to which belongs the specified day.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="date"/> is null.</exception>
        [Pure]
        public static XCivilDate GetEndOfMonth(XCivilDate date)
        {
            date.Unpack(out int y, out int m, out _);
            int d = GregorianFormulae.CountDaysInMonth(y, m);
            return new(Pack(y, m, d));
        }

        #endregion
        #region Adjustments

        /// <inheritdoc/>
        [Pure]
        public XCivilDate Adjust(Func<DateParts, DateParts> adjuster)
        {
            Requires.NotNull(adjuster);

            Unpack(out int y, out int m, out int d);
            var (y1, m1, d1) = adjuster.Invoke(new DateParts(y, m, d));
            return new XCivilDate(y1, m1, d1);
        }

        /// <summary>
        /// Adjusts the year field of the specified date to the specified value,
        /// yielding a new date.
        /// </summary>
        /// <exception cref="AoorException">The method would create an invalid
        /// Gregorian date within the Common Era on or before year 9999.
        /// </exception>
        [Pure]
        public XCivilDate WithYear(int newYear)
        {
            if (newYear < MinYear || newYear > MaxYear)
            {
                Throw.ArgumentOutOfRange(nameof(newYear));
            }

            int bMD = ObMonthDay;
            if (bMD == __IntercalaryDay && GregorianFormulae.IsLeapYear(newYear) == false)
            {
                Throw.ArgumentOutOfRange(nameof(newYear));
            }

            return new(((newYear - 1) << 9) | bMD);
        }

        /// <summary>
        /// Adjusts the month field of the specified date to the specified value,
        /// yielding a new date.
        /// </summary>
        /// <exception cref="AoorException">The method would create an invalid
        /// Gregorian date within the Common Era on or before year 9999.
        /// </exception>
        [Pure]
        public XCivilDate WithMonth(int newMonth)
        {
            if (newMonth < 1 || newMonth > 12)
            {
                Throw.ArgumentOutOfRange(nameof(newMonth));
            }

            int y = Year;
            int d = Day;

            if (d > GregorianFormulae.CountDaysInMonth(y, newMonth))
            {
                Throw.ArgumentOutOfRange(nameof(newMonth));
            }

            return new(Pack(y, newMonth, d));
        }

        /// <summary>
        /// Adjusts the day field of the specified date to the specified value,
        /// yielding a new date.
        /// </summary>
        /// <exception cref="AoorException">The method would create an invalid
        /// Gregorian date within the Common Era on or before year 9999.
        /// </exception>
        [Pure]
        public XCivilDate WithDay(int newDay)
        {
            if (newDay < 1)
            {
                Throw.ArgumentOutOfRange(nameof(newDay));
            }

            int y = Year;
            int m = Month;

            if (newDay > GregorianFormulae.CountDaysInMonth(y, m))
            {
                Throw.ArgumentOutOfRange(nameof(newDay));
            }

            return new(Pack(y, m, newDay));
        }

        #endregion
        #region Adjust the day of the week

        // With CIVILDATE_PLAIN_DAYOFWEEK, we do not use the math op Plus()
        //   first we must compute DaysSinceEpoch, shift it, then convert the
        //   result back to a (y, m, d).
        // Without CIVILDATE_PLAIN_DAYOFWEEK (default),
        //   we only need DayOfWeek which, thanks to the Doosmday rule, is
        //   slightly faster to compute than DaysSinceEpoch, then use Plus()
        //   which is rather optimised for this type of situation.
        // See comments in DayNumberX.

        private static readonly XCivilDate s_ThreeDaysBeforeMaxValue = s_MaxValue - 3;

        /// <inheritdoc />
        [Pure]
        public XCivilDate Previous(DayOfWeek dayOfWeek)
#if CIVILDATE_PLAIN_DAYOFWEEK
            => NextOrSameCore(dayOfWeek, -7);
#else
        {
            Requires.Defined(dayOfWeek);

            int δ = dayOfWeek - DayOfWeek;
            return AddDaysViaDayOfMonth(δ >= 0 ? δ - 7 : δ);
        }
#endif

        /// <inheritdoc />
        [Pure]
        public XCivilDate PreviousOrSame(DayOfWeek dayOfWeek)
#if CIVILDATE_PLAIN_DAYOFWEEK
            => NextOrSameCore(dayOfWeek, -6);
#else
        {
            Requires.Defined(dayOfWeek);

            int δ = dayOfWeek - DayOfWeek;
            return δ == 0 ? this : AddDaysViaDayOfMonth(δ > 0 ? δ - 7 : δ);
        }
#endif

        /// <inheritdoc />
        [Pure]
        public XCivilDate Nearest(DayOfWeek dayOfWeek) =>
            // REVIEW: On convertit en un DayNumber et on utilise la méthode Nearest() ?
            this > s_ThreeDaysBeforeMaxValue
            ? NextOrSameCore(dayOfWeek, -3)
            : PreviousOrSameCore(dayOfWeek, 3);

        /// <inheritdoc />
        [Pure]
        public XCivilDate NextOrSame(DayOfWeek dayOfWeek)
#if CIVILDATE_PLAIN_DAYOFWEEK
            => PreviousOrSameCore(dayOfWeek, 6);
#else
        {
            Requires.Defined(dayOfWeek);

            int δ = dayOfWeek - DayOfWeek;
            return δ == 0 ? this : AddDaysViaDayOfMonth(δ < 0 ? δ + 7 : δ);
        }
#endif

        /// <inheritdoc />
        [Pure]
        public XCivilDate Next(DayOfWeek dayOfWeek)
#if CIVILDATE_PLAIN_DAYOFWEEK
            => PreviousOrSameCore(dayOfWeek, 7);
#else
        {
            Requires.Defined(dayOfWeek);

            int δ = dayOfWeek - DayOfWeek;
            return AddDaysViaDayOfMonth(δ <= 0 ? δ + 7 : δ);
        }
#endif

        [Pure]
        private XCivilDate PreviousOrSameCore(DayOfWeek dayOfWeek, int dayShift)
        {
            Debug.Assert(dayShift >= 3);
            Requires.Defined(dayOfWeek);

            int daysSinceEpoch = DaysSinceEpoch + dayShift;
            daysSinceEpoch -= MathZ.Modulo(daysSinceEpoch + (DayOfWeek.Monday - dayOfWeek), 7);
            if (daysSinceEpoch < MinDaysSinceEpoch || daysSinceEpoch > MaxDaysSinceEpoch)
            {
                Throw.DateOverflow();
            }

            return FromDaysSinceEpoch(daysSinceEpoch);
        }

        [Pure]
        private XCivilDate NextOrSameCore(DayOfWeek dayOfWeek, int dayShift)
        {
            Debug.Assert(dayShift <= -3);
            Requires.Defined(dayOfWeek);

            int daysSinceEpoch = DaysSinceEpoch + dayShift;
            daysSinceEpoch += MathZ.Modulo(-daysSinceEpoch - (DayOfWeek.Monday - dayOfWeek), 7);
            if (daysSinceEpoch < MinDaysSinceEpoch || daysSinceEpoch > MaxDaysSinceEpoch)
            {
                Throw.DateOverflow();
            }

            return FromDaysSinceEpoch(daysSinceEpoch);
        }

        #endregion
    }

    public partial struct XCivilDate // Enumerate days in a month or a year
    {
        /// <summary>
        /// Obtains the collection of all dates in the specified year.
        /// </summary>
        /// <exception cref="AoorException">The specified year is not in the
        /// range from 1 to 9999.</exception>
        [Pure]
        public static IEnumerable<XCivilDate> GetDaysInYear(int year)
        {
            // Check arg eagerly.
            if (year < MinYear || year > MaxYear)
            {
                Throw.ArgumentOutOfRange(nameof(year));
            }

            return Iterator();

            IEnumerable<XCivilDate> Iterator()
            {
                int bY = (year - 1) << 9;

                for (int m = 1; m <= 12; m++)
                {
                    int bYM = bY | ((m - 1) << 5);

                    int daysInMonth = GregorianFormulae.CountDaysInMonth(year, m);
                    for (int d = 1; d <= daysInMonth; d++)
                    {
                        yield return new(bYM | (d - 1));
                    }
                }
            }
        }

        /// <summary>
        /// Obtains the collection of all dates in the specified month.
        /// </summary>
        /// <exception cref="AoorException">The specified year is not in the
        /// range from 1 to 9999.</exception>
        /// <exception cref="AoorException">The specified month is not in the
        /// range from 1 to 12.</exception>
        [Pure]
        public static IEnumerable<XCivilDate> GetDaysInMonth(int year, int month)
        {
            // Check args eagerly.
            if (year < MinYear || year > MaxYear)
            {
                Throw.ArgumentOutOfRange(nameof(year));
            }
            if (month < 1 || month > 12)
            {
                Throw.ArgumentOutOfRange(nameof(month));
            }

            return Iterator();

            IEnumerable<XCivilDate> Iterator()
            {
                int bYM = ((year - 1) << 9) | ((month - 1) << 5);

                int daysInMonth = GregorianFormulae.CountDaysInMonth(year, month);
                for (int d = 1; d <= daysInMonth; d++)
                {
                    yield return new(bYM | (d - 1));
                }
            }
        }
    }

    public partial struct XCivilDate // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="XCivilDate"/>
        /// are equal.
        /// </summary>
        public static bool operator ==(XCivilDate left, XCivilDate right) =>
            left._bin == right._bin;

        /// <summary>
        /// Determines whether two specified instances of <see cref="XCivilDate"/>
        /// are not equal.
        /// </summary>
        public static bool operator !=(XCivilDate left, XCivilDate right) =>
            left._bin != right._bin;

        /// <inheritdoc />
        [Pure]
        public bool Equals(XCivilDate other) => _bin == other._bin;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is XCivilDate date && Equals(date);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => _bin;
    }

    public partial struct XCivilDate // IComparable
    {
        /// <summary>
        /// Compares the two specified date instances to see if the left one is
        /// strictly earlier than the right one.
        /// </summary>
        public static bool operator <(XCivilDate left, XCivilDate right) =>
            left._bin < right._bin;

        /// <summary>
        /// Compares the two specified date instances to see if the left one is
        /// earlier than or equal to the right one.
        /// </summary>
        public static bool operator <=(XCivilDate left, XCivilDate right) =>
            left._bin <= right._bin;

        /// <summary>
        /// Compares the two specified date instances to see if the left one is
        /// strictly later than the right one.
        /// </summary>
        public static bool operator >(XCivilDate left, XCivilDate right) =>
            left._bin > right._bin;

        /// <summary>
        /// Compares the two specified date instances to see if the left one is
        /// later than or equal to the right one.
        /// </summary>
        public static bool operator >=(XCivilDate left, XCivilDate right) =>
            left._bin >= right._bin;

        /// <summary>
        /// Obtains the earlier date of two specified dates.
        /// </summary>
        [Pure]
        public static XCivilDate Min(XCivilDate x, XCivilDate y) => x < y ? x : y;

        /// <summary>
        /// Obtains the later date of two specified dates.
        /// </summary>
        [Pure]
        public static XCivilDate Max(XCivilDate x, XCivilDate y) => x > y ? x : y;

        /// <summary>
        /// Indicates whether this date instance is earlier, later or the same
        /// as the specified one.
        /// </summary>
        [Pure]
        public int CompareTo(XCivilDate other) => _bin.CompareTo(other._bin);

        /// <inheritdoc />
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is XCivilDate date ? _bin.CompareTo(date._bin)
            : Throw.NonComparable(typeof(XCivilDate), obj);
    }

    // Standard math ops, those based on the day, the base unit of a calendar.
    public partial struct XCivilDate // Standard math ops
    {
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage)

        /// <summary>
        /// Subtracts the two specified dates and returns the number of days
        /// between them at midnight (0h).
        /// </summary>
        public static int operator -(XCivilDate left, XCivilDate right) =>
            left.CountDaysSince(right);

        /// <summary>
        /// Adds a number of days to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// dates.</exception>
        public static XCivilDate operator +(XCivilDate value, int days) =>
            value.PlusDays(days);

        /// <summary>
        /// Subtracts a number of days to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// dates.</exception>
        public static XCivilDate operator -(XCivilDate value, int days) =>
            value.PlusDays(-days);

        /// <summary>
        /// Adds one day to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the
        /// latest supported date.</exception>
        public static XCivilDate operator ++(XCivilDate value) => value.NextDay();

        /// <summary>
        /// Subtracts one day to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow
        /// the earliest supported date.</exception>
        public static XCivilDate operator --(XCivilDate value) => value.PreviousDay();

#pragma warning restore CA2225

        /// <inheritdoc />
        [Pure]
        public int CountDaysSince(XCivilDate other) =>
            ObYearMonth == other.ObYearMonth ? Day - other.Day
            : DaysSinceEpoch - other.DaysSinceEpoch;

        /// <inheritdoc />
        [Pure]
        public XCivilDate PlusDays(int days)
        {
            // Fast tracks.
            if (-MinDaysInMonth <= days && days <= MinDaysInMonth)
            {
                return AddDaysViaDayOfMonth(days);
            }

            if (-MinDaysInYear <= days && days <= MinDaysInYear)
            {
                return AddDaysViaDayOfYear(days);
            }

            // Slow track.
            int daysSinceEpoch = checked(DaysSinceEpoch + days);
            if (daysSinceEpoch < MinDaysSinceEpoch || daysSinceEpoch > MaxDaysSinceEpoch)
            {
                Throw.DateOverflow();
            }

            return FromDaysSinceEpoch(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public XCivilDate NextDay()
        {
            Unpack(out int y, out int m, out int d);

            return
                d < MinDaysInMonth || d < GregorianFormulae.CountDaysInMonth(y, m)
                ? new(Pack(y, m, d + 1))
                : m < 12 ? new(Pack(y, m + 1, 1))
                : y < MaxYear ? new((y << 9) | __StartOfYear) // read (y + 1) - 1
                : Throw.DateOverflow<XCivilDate>();
        }

        /// <inheritdoc />
        [Pure]
        public XCivilDate PreviousDay()
        {
            Unpack(out int y, out int m, out int d);

            return d > 1 ? new(Pack(y, m, d - 1))
                : m > 1 ? new(Pack(y, m - 1, GregorianFormulae.CountDaysInMonth(y, m - 1)))
                : y > MinYear ? new(((y - 2) << 9) | __EndOfYear) // read (y - 1) - 1
                : Throw.DateOverflow<XCivilDate>();
        }

        [Pure]
        private XCivilDate AddDaysViaDayOfMonth(int days)
        {
            Debug.Assert(-MinDaysInMonth <= days);
            Debug.Assert(days <= MinDaysInMonth);

            Unpack(out int y, out int m, out int d);

            int dom = d + days;
            if (dom < 1)
            {
                if (m == 1)
                {
                    if (y == MinYear) Throw.DateOverflow();
                    y--;
                    m = 12;
                    dom += 31;
                }
                else
                {
                    m--;
                    dom += GregorianFormulae.CountDaysInMonth(y, m);
                }
            }
            else if (dom > MinDaysInMonth)
            {
                int daysInMonth = GregorianFormulae.CountDaysInMonth(y, m);
                if (dom > daysInMonth)
                {
                    dom -= daysInMonth;
                    if (m == 12)
                    {
                        if (y == MaxYear) Throw.DateOverflow();
                        y++;
                        m = 1;
                    }
                    else
                    {
                        m++;
                    }
                }
            }

            return new XCivilDate(Pack(y, m, dom));
        }

        [Pure]
        private XCivilDate AddDaysViaDayOfYear(int days)
        {
            Debug.Assert(days >= -MinDaysInYear);
            Debug.Assert(days <= MinDaysInYear);

            int y = Year;
            int doy = DayOfYear + days;
            if (doy < 1)
            {
                // Previous year.
                if (y == MinYear) Throw.DateOverflow();
                y--;
                doy += GregorianFormulae.CountDaysInYear(y);
            }
            else
            {
                int daysInYear = GregorianFormulae.CountDaysInYear(y);
                if (doy > daysInYear)
                {
                    // Next year.
                    if (y == MaxYear) Throw.DateOverflow();
                    y++;
                    doy -= daysInYear;
                }
            }

            return FromOrdinalDateImpl(y, doy);
        }
    }

    public partial struct XCivilDate // More math ops
    {
        /// <summary>
        /// Subtracts the two specified dates and returns the number of years,
        /// months and days between them.
        /// </summary>
        [Pure]
        public static (int Years, int Months, int Days) Subtract(
            XCivilDate left, XCivilDate right)
        {
            // At first, I counted the years, then the months and finally the days
            // but this is WRONG because doing so we lose information on the way,
            // we kind of accumulate "rounding errors". For instance, with
            //   start = 29/02/0008
            //   end   = 28/02/0012
            // we would conclude that there are between them
            //   3 years   (29/02/0008 -> 28/02/0011)
            //   12 months (28/02/0011 -> 28/02/0012)
            //   and 0 days
            // The problem is that "start" is intercalary and the two dates are
            // separated by at least one common year (otherwise it would work).
            // What we really should do is:
            //   totalMonths = 47 (29/02/0008 -> 29/01/0012)
            //   days        = 30 (29/01/0012 -> 28/02/0012)
            // which gives us 3 years, 11 months and 30 days.
            // TODO: explain.

            left.Unpack(out int y, out int m, out int d);
            right.Unpack(out int y0, out int m0, out int d0);

            // We use an unpatched count; see CountMonthsSince().
            int χ = left >= right ? ((d - d0) >> 31) : -((d0 - d) >> 31);
            int months = 12 * (y - y0) + (m - m0) + χ;

            int days = left - right.PlusMonths(months);

            // Reminder: C# integer division rounds towards zero, which is
            // exactly what we want here.
            return (months / 12, months % 12, days);
        }

        #region AddYears(), PlusYears() & CountYearsSince()

        // There is only one case where the month-day part changes: going
        // from an intercalary day to a common year.
        // Let y' := y + years,
        //   if d/m = 29/2
        //     if y' is leap    return 29/2/y'
        //     otherwise        return ??/?/y'
        //   otherwise          return  d/m/y'
        // The usual answer to the unsolved case is 28/2/y', but returning
        // 1/3/y' seems equally valid. Here our answer is the end of February.
        //
        // Then, there is the problem of counting the number of years between
        // two dates. Here, we choose to define the op such that:
        //   (date + years) - date -> years
        //   if (date - other) -> years then (other - date) -> -years

        /// <summary>
        /// Adds a number of years to the year field of the specified date
        /// instance, yielding a new date and also returns the cut-off "error"
        /// in an output parameter.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow either the capacity of
        /// <see cref="Int32"/> or the range of supported dates.</exception>
        [Pure]
        public static XCivilDate AddYears(XCivilDate date, int years, out int cutoff)
        {
            int y = checked(date.Year + years);

            if (y < MinYear || y > MaxYear) Throw.DateOverflow();

            int bMD = date.ObMonthDay;
            if (bMD == __IntercalaryDay && GregorianFormulae.IsLeapYear(y) == false)
            {
                bMD = __EndOfFebruary;
                cutoff = 1;
            }
            else
            {
                cutoff = 0;
            }

            return new XCivilDate(((y - 1) << 9) | bMD);
        }

        /// <summary>
        /// Counts the number of years from the specified date to this date
        /// instance.
        /// </summary>
        /// <remarks>
        /// This methods ensures that the following properties hold:
        /// <code>
        ///   date.PlusYears(years).CountYearsSince(date) == years;
        ///   date.CountYearsSince(other) == -other.CountYearsSince(date);
        /// </code>
        /// </remarks>
        [Pure]
        public int CountYearsSince(XCivilDate other)
        {
            int y = Year;
            int y0 = other.Year;
            int years = y - y0;

            if (years == 0) { return 0; }

            int bMD = ObMonthDay;
            int bMD0 = other.ObMonthDay;

            if (years > 0)
            {
                Patch(ref bMD0, y, bMD);
                // Trick: given "n" an Int32, if n >= 0 then n >> 31 = 0;
                // otherwise n >> 31 = -1. Instead of writing
                //   return years + (bMD < bMD0 ? -1 : 0);
                // and just for the fun (it might be slower actually), we use:
                return years + ((bMD - bMD0) >> 31);
            }
            else
            {
                Patch(ref bMD, y0, bMD0);
                // Same as above,
                //   return years + (bMD0 < bMD ? 1 : 0);
                return years - ((bMD0 - bMD) >> 31);
            }

            static void Patch(ref int md0, int y, int md)
            {
                if (md0 == __IntercalaryDay
                    && md == __EndOfFebruary
                    && GregorianFormulae.IsLeapYear(y) == false)
                {
                    // Trick: to compare md0 and md, we pretend that the
                    // intercalary day is a 28/02.
                    md0 = __EndOfFebruary;
                }
            }
        }

        /// <summary>
        /// Adds a number of years to the year field of this date instance,
        /// yielding a new date.
        /// <para>When this date instance is an intercalary day and, whether the
        /// target year is leap or not, this method returns the end of February.
        /// </para>
        /// <remarks>
        /// This method is NOT "algebraic". If a date is not an intercalary day,
        /// we do have:
        /// <code>
        ///   date.PlusYears(years).PlusYears(-years) == date;
        /// </code>
        /// but, if it is an intercalary day, the equality only holds when
        /// <paramref name="years"/> is such that the resulting date is
        /// intercalary too.
        /// </remarks>
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow either the capacity of
        /// <see cref="Int32"/> or the range of supported dates.</exception>
        [Pure]
        public XCivilDate PlusYears(int years)
        {
            int y = checked(Year + years);

            if (y < MinYear || y > MaxYear) Throw.DateOverflow();

            int bMD = ObMonthDay;
            if (bMD == __IntercalaryDay && GregorianFormulae.IsLeapYear(y) == false)
            {
                bMD = __EndOfFebruary;
            }

            return new XCivilDate(((y - 1) << 9) | bMD);
        }

        #endregion
        #region AddMonths(), PlusMonths() & CountMonthsSince()

        /// <summary>
        /// Adds a number of months to the month field of the specified date
        /// instance, yielding a new date.
        /// <para>This method also returns the cut-off "error" in an output
        /// parameter.</para>
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow either the capacity of
        /// <see cref="Int32"/> or the range of supported dates.</exception>
        [Pure]
        public static XCivilDate AddMonths(XCivilDate date, int months, out int cutoff)
        {
            date.Unpack(out int y, out int m, out int d);

            // Only the op that may overflow is wrapped in a checked context.
            m = 1 + MathZ.Modulo(checked(m - 1 + months), 12, out int y0);
            y += y0;

            if (y < MinYear || y > MaxYear) Throw.DateOverflow();

            int daysInMonth = GregorianFormulae.CountDaysInMonth(y, m);
            cutoff = Math.Max(0, d - daysInMonth);

            return new(Pack(y, m, cutoff > 0 ? daysInMonth : d));
        }

        /// <summary>
        /// Counts the number of months between the two specified dates.
        /// </summary>
        /// <remarks>
        /// This methods ensures that the following properties hold:
        /// <code>
        ///   date.PlusMonths(months).CountMonthsSince(date) == months;
        ///   date.CountMonthsSince(other) == -other.CountMonthsSince(date);
        /// </code>
        /// </remarks>
        [Pure]
        public int CountMonthsSince(XCivilDate other)
        {
            Unpack(out int y, out int m, out int d);
            other.Unpack(out int y0, out int m0, out int d0);

            int months = 12 * (y - y0) + (m - m0);

            // It works because of the particular form of the previous eq. and
            // because of the type of data involved.
            if (months == 0) { return 0; }

            if (this >= other)
            {
                Patch(ref d0, y, m, d);
                // See CountYearsSince() for an explanation.
                //   return months + (d < d0 ? -1 : 0);
                return months + ((d - d0) >> 31);
            }
            else
            {
                Patch(ref d, y0, m0, d0);
                // See CountYearsSince() for an explanation.
                //   return months + (d0 < d ? 1 : 0);
                return months - ((d0 - d) >> 31);
            }

            static void Patch(ref int d0, int y, int m, int d)
            {
                if (d0 > d && d == GregorianFormulae.CountDaysInMonth(y, m))
                {
                    d0 = d;
                }
            }
        }

        /// <summary>
        /// Adds a number of months to the month field of this date instance,
        /// yielding a new date.
        /// </summary>
        /// <remarks>
        /// This method is NOT "algebraic". In general, we cannot assert that:
        /// <code>
        ///   date.PlusMonths(months).PlusMonths(-months) == date;
        /// </code>
        /// </remarks>
        /// <exception cref="OverflowException">The operation would overflow either the capacity of
        /// <see cref="Int32"/> or the range of supported dates.</exception>
        [Pure]
        public XCivilDate PlusMonths(int months)
        {
            Unpack(out int y, out int m, out int d);

            // Only the op that may overflow is wrapped in a checked context.
            m = 1 + MathZ.Modulo(checked(m - 1 + months), 12, out int y0);
            y += y0;

            if (y < MinYear || y > MaxYear) Throw.DateOverflow();

            return new(Pack(y, m, Math.Min(d, GregorianFormulae.CountDaysInMonth(y, m))));
        }

        #endregion
    }
}
