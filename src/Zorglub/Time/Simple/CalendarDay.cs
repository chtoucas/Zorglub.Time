// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Hemerology;
    using Zorglub.Time.Hemerology.Scopes;

    /// <summary>
    /// Represents a calendar day, that is a date within a calendar system.
    /// <para><see cref="CalendarDay"/> is an immutable struct.</para>
    /// </summary>
    public readonly partial struct CalendarDay : ISimpleDate<CalendarDay>
    {
        #region Bit settings

        // Vérifions que daysSinceEpoch a assez de place.
        //  DaysSinceEpochBits = 32 - CuidBits = 25
        // On sait que -9998 <= y <= 9999 et que 1 <= doy <= 1024, donc
        //  daysSinceEpoch >= -9998 * 1024 = -10 237 952 > -16 777 216 = MinDaysSinceEpoch
        //  daysSinceEpoch <=  9999 * 1024 =  10 238 976 <  16 777 215 = MaxDaysSinceEpoch

        /// <summary>
        /// <see cref="Cuid"/> is a 7-bit unsigned integer.
        /// <para>This field is a constant equal to 7.</para>
        /// </summary>
        private const int CuidBits = Yemodax.ExtraBits;

        /// <summary>This field is a constant equal to 7.</summary>
        private const int DaysSinceEpochShift = CuidBits;

        /// <summary>This field is a constant equal to 127.</summary>
        private const int CuidMask = (1 << CuidBits) - 1;

        #endregion

        /// <summary>
        /// Represents the internal binary representation.
        /// <para>This field is read-only.</para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// The data is organised as follows:
        /// <code><![CDATA[
        ///   DaysSinceEpoch bbbb bbbb bbbb bbbb bbbb bbbb b
        ///   Cuid                                          bbb bbbb
        /// ]]></code>
        /// </para>
        /// </remarks>
        private readonly int _bin; // 4 bytes

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarDay"/> struct to the specified day
        /// number in the Gregorian calendar.
        /// <para>To create an instance for another calendar, see
        /// <see cref="Calendar.GetCalendarDay(DayNumber)"/>.</para>
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// values supported by the Gregorian calendar.</exception>
        public CalendarDay(DayNumber dayNumber)
        {
            GregorianProlepticScope.DefaultDomain.Validate(dayNumber);

            _bin = Pack(dayNumber - DayZero.NewStyle, (byte)Cuid.Gregorian);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarDay"/> struct.
        /// <para>This method does NOT validate its parameter.</para>
        /// </summary>
        internal CalendarDay(int daysSinceEpoch, Cuid cuid)
        {
            _bin = Pack(daysSinceEpoch, (byte)cuid);
        }

        /// <summary>
        /// Gets the day number.
        /// </summary>
        public DayNumber DayNumber
        {
            get
            {
                ref readonly var chr = ref CalendarRef;
                return chr.Epoch + DaysSinceEpoch;
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
        /// <remarks>
        /// <para>You should only use this property if you don't need to know the value of
        /// <see cref="Month"/> or <see cref="Day"/> too; otherwise you should use
        /// <see cref="Deconstruct(out int, out int, out int)"/>
        /// instead.</para>
        /// </remarks>
        public int Year
        {
            get
            {
                ref readonly var chr = ref CalendarRef;
                return chr.Schema.GetYear(DaysSinceEpoch);
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// <para>You should only use this property if you don't need to know the value of
        /// <see cref="Year"/> or <see cref="Day"/> too; otherwise you should use
        /// <see cref="Deconstruct(out int, out int, out int)"/>
        /// instead.</para>
        /// </remarks>
        public int Month
        {
            get
            {
                ref readonly var chr = ref CalendarRef;
                chr.Schema.GetDateParts(DaysSinceEpoch, out _, out int m, out _);
                return m;
            }
        }

        /// <inheritdoc />
        public int DayOfYear
        {
            get
            {
                ref readonly var chr = ref CalendarRef;
                _ = chr.Schema.GetYear(DaysSinceEpoch, out int doy);
                return doy;
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// <para>You should only use this property if you don't need to know the value of
        /// <see cref="Year"/> or <see cref="Month"/> too; otherwise you should use
        /// <see cref="Deconstruct(out int, out int, out int)"/> instead.</para>
        /// </remarks>
        public int Day
        {
            get
            {
                ref readonly var chr = ref CalendarRef;
                chr.Schema.GetDateParts(DaysSinceEpoch, out _, out _, out int d);
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
                ref readonly var chr = ref CalendarRef;
                Unpack(chr, out int y, out int m, out int d);
                return chr.Schema.IsIntercalaryDay(y, m, d);
            }
        }

        /// <inheritdoc />
        public bool IsSupplementary
        {
            get
            {
                ref readonly var chr = ref CalendarRef;
                Unpack(chr, out int y, out int m, out int d);
                return chr.Schema.IsSupplementaryDay(y, m, d);
            }
        }

        /// <inheritdoc />
        public CalendarYear CalendarYear => new(Year, Cuid);

        /// <inheritdoc />
        public CalendarMonth CalendarMonth
        {
            get
            {
                ref readonly var chr = ref CalendarRef;
                Unpack(chr, out int y, out int m, out _);
                return new CalendarMonth(y, m, Cuid);
            }
        }

        /// <inheritdoc />
        public Calendar Calendar => CalendarCatalog.GetCalendarUnchecked(unchecked(_bin & CuidMask));

        /// <summary>
        /// Gets the count of days since the epoch of the calendar to which belongs the current
        /// instance.
        /// </summary>
        internal int DaysSinceEpoch => unchecked(_bin >> DaysSinceEpochShift);

        /// <summary>
        /// Gets the ID of the calendar to which belongs the current instance.
        /// </summary>
        internal Cuid Cuid => unchecked((Cuid)(_bin & CuidMask));

        /// <summary>
        /// Gets a read-only reference to the calendar to which belongs the current instance.
        /// </summary>
        internal ref readonly Calendar CalendarRef
        {
            // CIL code size = 15 bytes <= 32 bytes.
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref CalendarCatalog.GetCalendarUnsafe(unchecked(_bin & CuidMask));
        }

        /// <summary>
        /// Returns a culture-independent string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString()
        {
            ref readonly var chr = ref CalendarRef;
            chr.Schema.GetDateParts(DaysSinceEpoch, out int y, out int m, out int d);
            return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({chr})");
        }

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        public void Deconstruct(out int year, out int month, out int day)
        {
            ref readonly var chr = ref CalendarRef;
            chr.Schema.GetDateParts(DaysSinceEpoch, out year, out month, out day);
        }

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        public void Deconstruct(out int year, out int dayOfYear)
        {
            ref readonly var chr = ref CalendarRef;
            year = chr.Schema.GetYear(DaysSinceEpoch, out dayOfYear);
        }

        /// <summary>
        /// Packs the specified month parts into a single 32-bit word.
        /// </summary>
        [Pure]
        // CIL code size = 6 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Pack(int daysSinceEpoch, byte cuid)
        {
            unchecked
            {
                return (daysSinceEpoch << DaysSinceEpochShift) | cuid;
            }
        }

        /// <summary>
        /// Unpacks the year/month/day components of the current instance.
        /// </summary>
        // CIL code size = 22 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Unpack(Calendar chr, out int year, out int month, out int day) =>
            chr.Schema.GetDateParts(DaysSinceEpoch, out year, out month, out day);
    }

    public partial struct CalendarDay // Factories, conversions, adjustments, etc.
    {
        #region Binary serialization

        /// <summary>
        /// Deserializes a 32-bit binary value and recreates an original serialized
        /// <see cref="CalendarDay"/> object.
        /// </summary>
        /// <exception cref="AoorException">The matching calendar is not a system calendar.
        /// </exception>
        [Pure]
        public static CalendarDay FromBinary(int data)
        {
            unchecked
            {
                int daysSinceEpoch = data >> DaysSinceEpochShift;
                var ident = (CalendarId)(data & CuidMask);
                var chr = CalendarCatalog.GetSystemCalendar(ident);
                return chr.GetCalendarDay(chr.Epoch + daysSinceEpoch);
            }
        }

        /// <summary>
        /// Serializes the current <see cref="CalendarDay"/> object to a 32-bit binary value that
        /// subsequently can be used to recreate the <see cref="CalendarDay"/> object.
        /// </summary>
        /// <exception cref="NotSupportedException">Binary serialization is only supported for dates
        /// belonging to a system calendar.</exception>
        [Pure]
        public int ToBinary() => Cuid.IsFixed() ? _bin : Throw.NotSupported<int>();

        #endregion
        #region Factories

        /// <summary>
        /// Obtains the current date in the Gregorian calendar on this machine, expressed in local
        /// time, not UTC.
        /// <para>To obtain the current date in another calendar, see
        /// <see cref="Calendar.GetCurrentDay()"/>.</para>
        /// </summary>
        [Pure]
        public static CalendarDay Today() =>
            new(DayNumber.Today() - DayZero.NewStyle, Cuid.Gregorian);

        #endregion
        #region Conversions

        [Pure]
        static CalendarDay IFixedDay<CalendarDay>.FromDayNumber(DayNumber dayNumber) =>
            new(dayNumber);

        [Pure]
        DayNumber IFixedDay.ToDayNumber() => DayNumber;

        [Pure]
        CalendarDay ISimpleDate.ToCalendarDay() => this;

        /// <inheritdoc />
        [Pure]
        public CalendarDate ToCalendarDate()
        {
            ref readonly var chr = ref CalendarRef;
            var ymd = chr.Schema.GetDateParts(DaysSinceEpoch);
            return new CalendarDate(ymd, Cuid);
        }

        /// <inheritdoc />
        [Pure]
        public OrdinalDate ToOrdinalDate()
        {
            ref readonly var chr = ref CalendarRef;
            var ydoy = chr.Schema.GetOrdinalParts(DaysSinceEpoch);
            return new OrdinalDate(ydoy, Cuid);
        }

        /// <inheritdoc />
        [Pure]
        public CalendarDay WithCalendar(Calendar newCalendar)
        {
            Requires.NotNull(newCalendar);

            return newCalendar.GetCalendarDay(DayNumber);
        }

        #endregion
        #region Counting

        /// <inheritdoc />
        [Pure]
        public int CountElapsedDaysInYear()
        {
            ref readonly var chr = ref CalendarRef;
            return chr.Schema.CountDaysInYearBefore(DaysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public int CountRemainingDaysInYear()
        {
            ref readonly var chr = ref CalendarRef;
            return chr.Schema.CountDaysInYearAfter(DaysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public int CountElapsedDaysInMonth()
        {
            ref readonly var chr = ref CalendarRef;
            return chr.Schema.CountDaysInMonthBefore(DaysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public int CountRemainingDaysInMonth()
        {
            ref readonly var chr = ref CalendarRef;
            return chr.Schema.CountDaysInMonthAfter(DaysSinceEpoch);
        }

        #endregion
        #region Adjustments

        /// <summary>
        /// Adjusts the day number field to the specified values, yielding a new calendar day.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="newDayNumber"/> is outside the range of
        /// supported values.</exception>
        [Pure]
        public CalendarDay WithDayNumber(DayNumber newDayNumber)
        {
            ref readonly var chr = ref CalendarRef;
            chr.Domain.Validate(newDayNumber);
            return new CalendarDay(newDayNumber - chr.Epoch, Cuid);
        }

        //
        // Adjust the day of the week
        //

        /// <inheritdoc />
        [Pure]
        public CalendarDay Previous(DayOfWeek dayOfWeek)
        {
            Requires.Defined(dayOfWeek);

            // We don't use the methods from DayNumber to avoid the double
            // validation, one from DayNumber and the other using the scope of
            // the calendar,
            // > return new(dayNumber.Previous(dayOfWeek) - chr.Epoch, Cuid);
            int δ = dayOfWeek - DayOfWeek;
            return PlusDays(δ >= 0 ? δ - CalendricalConstants.DaysInWeek : δ);
        }

        /// <inheritdoc />
        [Pure]
        public CalendarDay PreviousOrSame(DayOfWeek dayOfWeek)
        {
            Requires.Defined(dayOfWeek);

            int δ = dayOfWeek - DayOfWeek;
            return δ == 0 ? this : PlusDays(δ > 0 ? δ - CalendricalConstants.DaysInWeek : δ);
        }

        /// <inheritdoc />
        [Pure]
        public CalendarDay Nearest(DayOfWeek dayOfWeek)
        {
            ref readonly var chr = ref CalendarRef;
            var epoch = chr.Epoch;
            var dayNumber = epoch + DaysSinceEpoch;
            var nearest = dayNumber.Nearest(dayOfWeek);
            chr.Domain.CheckOverflow(nearest);
            return new(nearest - epoch, Cuid);
        }

        /// <inheritdoc />
        [Pure]
        public CalendarDay NextOrSame(DayOfWeek dayOfWeek)
        {
            Requires.Defined(dayOfWeek);

            int δ = dayOfWeek - DayOfWeek;
            return δ == 0 ? this : PlusDays(δ < 0 ? δ + CalendricalConstants.DaysInWeek : δ);
        }

        /// <inheritdoc />
        [Pure]
        public CalendarDay Next(DayOfWeek dayOfWeek)
        {
            Requires.Defined(dayOfWeek);

            int δ = dayOfWeek - DayOfWeek;
            return PlusDays(δ <= 0 ? δ + CalendricalConstants.DaysInWeek : δ);
        }

        #endregion
    }

    public partial struct CalendarDay // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="CalendarDay"/> are equal.
        /// </summary>
        public static bool operator ==(CalendarDay left, CalendarDay right) =>
            left._bin == right._bin;

        /// <summary>
        /// Determines whether two specified instances of <see cref="CalendarDay"/> are not equal.
        /// </summary>
        public static bool operator !=(CalendarDay left, CalendarDay right) =>
            left._bin != right._bin;

        /// <inheritdoc />
        [Pure]
        public bool Equals(CalendarDay other) => _bin == other._bin;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is CalendarDay date && Equals(date);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => _bin.GetHashCode();
    }

    public partial struct CalendarDay // IComparable
    {
        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly earlier than the
        /// right one.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="right"/> belongs to a different
        /// calendar from that of <paramref name="left"/>.</exception>
        public static bool operator <(CalendarDay left, CalendarDay right) => left.CompareTo(right) < 0;

        /// <summary>
        /// Compares the two specified instances to see if the left one is earlier than or equal to
        /// the right one.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="right"/> belongs to a different
        /// calendar from that of <paramref name="left"/>.</exception>
        public static bool operator <=(CalendarDay left, CalendarDay right) => left.CompareTo(right) <= 0;

        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly later than the
        /// right one.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="right"/> belongs to a different
        /// calendar from that of <paramref name="left"/>.</exception>
        public static bool operator >(CalendarDay left, CalendarDay right) => left.CompareTo(right) > 0;

        /// <summary>
        /// Compares the two specified instances to see if the left one is later than or equal to
        /// the right one.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="right"/> belongs to a different
        /// calendar from that of <paramref name="left"/>.</exception>
        public static bool operator >=(CalendarDay left, CalendarDay right) => left.CompareTo(right) >= 0;

        /// <summary>
        /// Obtains the earlier date of two specified dates.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="y"/> belongs to a different calendar
        /// from that of <paramref name="y"/>.</exception>
        [Pure]
        public static CalendarDay Min(CalendarDay x, CalendarDay y) =>
            x.CompareTo(y) < 0 ? x : y;

        /// <summary>
        /// Obtains the later date of two specified dates.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="y"/> belongs to a different calendar
        /// from that of <paramref name="y"/>.</exception>
        [Pure]
        public static CalendarDay Max(CalendarDay x, CalendarDay y) =>
            x.CompareTo(y) > 0 ? x : y;

        /// <summary>
        /// Indicates whether this instance is earlier, later or the same as the specified one.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="other"/> does not belong to the
        /// calendar of the current instance.</exception>
        [Pure]
        public int CompareTo(CalendarDay other)
        {
            if (other.Cuid != Cuid) Throw.BadCuid(nameof(other), Cuid, other.Cuid);

            return DaysSinceEpoch.CompareTo(other.DaysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is CalendarDay date ? CompareTo(date)
            : Throw.NonComparable(typeof(CalendarDay), obj);
    }

    public partial struct CalendarDay // Math ops
    {
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage)
        // Friendly alternates do exist but use domain-specific names.

        /// <summary>
        /// Subtracts the two specified dates and returns the number of days between them.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="right"/> belongs to a different
        /// calendar from that of <paramref name="left"/>.</exception>
        public static int operator -(CalendarDay left, CalendarDay right) => left.CountDaysSince(right);

        /// <summary>
        /// Adds a number of days to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow either the capacity of
        /// <see cref="Int32"/> or the range of supported dates.</exception>
        public static CalendarDay operator +(CalendarDay value, int days) => value.PlusDays(days);

        /// <summary>
        /// Subtracts a number of days to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow either the capacity of
        /// <see cref="Int32"/> or the range of supported dates.</exception>
        public static CalendarDay operator -(CalendarDay value, int days) => value.PlusDays(-days);

        /// <summary>
        /// Adds one day to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the latest supported
        /// date.</exception>
        public static CalendarDay operator ++(CalendarDay value) => value.NextDay();

        /// <summary>
        /// Subtracts one day to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the earliest supported
        /// date.</exception>
        public static CalendarDay operator --(CalendarDay value) => value.PreviousDay();

#pragma warning restore CA2225

        /// <inheritdoc />
        [Pure]
        public int CountDaysSince(CalendarDay other)
        {
            if (other.Cuid != Cuid) Throw.BadCuid(nameof(other), Cuid, other.Cuid);

            return checked(DaysSinceEpoch - other.DaysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public CalendarDay PlusDays(int days)
        {
            int daysSinceEpoch = checked(DaysSinceEpoch + days);
            ref readonly var chr = ref CalendarRef;
            chr.SupportedDays.Check(daysSinceEpoch);
            return new CalendarDay(daysSinceEpoch, Cuid);
        }

        /// <inheritdoc />
        [Pure]
        public CalendarDay NextDay()
        {
            ref readonly var chr = ref CalendarRef;
            int daysSinceEpoch = DaysSinceEpoch + 1;
            chr.SupportedDays.CheckUpperBound(daysSinceEpoch);
            return new CalendarDay(daysSinceEpoch, Cuid);
        }

        /// <inheritdoc />
        [Pure]
        public CalendarDay PreviousDay()
        {
            ref readonly var chr = ref CalendarRef;
            int daysSinceEpoch = DaysSinceEpoch - 1;
            chr.SupportedDays.CheckLowerBound(daysSinceEpoch);
            return new CalendarDay(daysSinceEpoch, Cuid);
        }
    }
}
