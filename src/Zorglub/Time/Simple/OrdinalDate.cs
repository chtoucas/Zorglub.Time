// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Schemas;
    using Zorglub.Time.Hemerology;
    using Zorglub.Time.Hemerology.Scopes;

    /// <summary>
    /// Represents an ordinal date.
    /// <para><see cref="OrdinalDate"/> is an immutable struct.</para>
    /// </summary>
    public readonly partial struct OrdinalDate :
        ISimpleDate<OrdinalDate>,
        ISubtractionOperators<OrdinalDate, int, OrdinalDate>
    {
        /// <summary>
        /// Represents the internal binary representation.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly Yedoyx _bin; // 4 bytes

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdinalDate"/> struct to the specified date
        /// parts in the Gregorian calendar.
        /// <para>To create an instance for another calendar, see
        /// <see cref="Calendar.GetOrdinalDate(int, int)"/>.</para>
        /// </summary>
        /// <exception cref="AoorException">The specified components do not form a valid date or
        /// <paramref name="year"/> is outside the range of years supported by the Gregorian
        /// calendar.</exception>
        public OrdinalDate(int year, int dayOfYear)
        {
            GregorianProlepticShortScope.ValidateOrdinalImpl(year, dayOfYear);

            _bin = new Yedoyx(year, dayOfYear, (int)Cuid.Gregorian);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdinalDate"/> struct.
        /// <para>This constructor does NOT validate its parameters.</para>
        /// </summary>
        internal OrdinalDate(int y, int doy, Cuid cuid)
        {
            _bin = new Yedoyx(y, doy, (int)cuid);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdinalDate"/> struct.
        /// <para>This constructor does NOT validate its parameters.</para>
        /// </summary>
        internal OrdinalDate(Yedoy ydoy, Cuid cuid)
        {
            _bin = new Yedoyx(ydoy, (int)cuid);
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
        public int Month
        {
            get
            {
                _bin.Unpack(out int y, out int doy);
                ref readonly var chr = ref CalendarRef;
                return chr.Schema.GetMonth(y, doy, out _);
            }
        }

        /// <inheritdoc />
        public int DayOfYear => _bin.DayOfYear;

        /// <inheritdoc />
        public int Day
        {
            get
            {
                _bin.Unpack(out int y, out int doy);
                ref readonly var chr = ref CalendarRef;
                _ = chr.Schema.GetMonth(y, doy, out int d);
                return d;
            }
        }

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
                _bin.Unpack(out int y, out int doy);
                ref readonly var chr = ref CalendarRef;
                var sch = chr.Schema;
                int m = sch.GetMonth(y, doy, out int d);
                return sch.IsIntercalaryDay(y, m, d);
            }
        }

        /// <inheritdoc />
        public bool IsSupplementary
        {
            get
            {
                _bin.Unpack(out int y, out int doy);
                ref readonly var chr = ref CalendarRef;
                var sch = chr.Schema;
                int m = sch.GetMonth(y, doy, out int d);
                return sch.IsSupplementaryDay(y, m, d);
            }
        }

        /// <inheritdoc />
        public Calendar Calendar => CalendarCatalog.GetCalendarUnchecked(_bin.Extra);

        /// <inheritdoc />
        public CalendarYear CalendarYear => new(Year, Cuid);

        /// <inheritdoc />
        public CalendarMonth CalendarMonth => new(Year, Month, Cuid);

        /// <summary>
        /// Gets the date parts of current instance.
        /// </summary>
        internal Yedoy Parts => _bin.Yedoy;

        /// <summary>
        /// Gets the ID of the calendar to which belongs the current instance.
        /// </summary>
        internal Cuid Cuid => (Cuid)_bin.Extra;

        /// <summary>
        /// Gets a read-only reference to the calendar to which belongs the current instance.
        /// </summary>
        private ref readonly Calendar CalendarRef
        {
            // CIL code size = 17 bytes <= 32 bytes.
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref CalendarCatalog.GetCalendarUnsafe(_bin.Extra);
        }

        /// <summary>
        /// Returns a culture-independent string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString()
        {
            _bin.Unpack(out int y, out int doy);
            ref readonly var chr = ref CalendarRef;
            return FormattableString.Invariant($"{doy:D3}/{y:D4} ({chr})");
        }

        /// <inheritdoc />
        public void Deconstruct(out int year, out int month, out int day)
        {
            _bin.Unpack(out year, out int doy);
            ref readonly var chr = ref CalendarRef;
            month = chr.Schema.GetMonth(year, doy, out day);
        }

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        public void Deconstruct(out int year, out int dayOfYear) =>
            _bin.Unpack(out year, out dayOfYear);
    }

    public partial struct OrdinalDate // Factories, conversions, adjustments, etc.
    {
        #region Binary serialization

        /// <summary>
        /// Deserializes a 32-bit binary value and recreates an original serialized
        /// <see cref="OrdinalDate"/> object.
        /// </summary>
        /// <exception cref="AoorException">The matching calendar is not a system calendar.
        /// </exception>
        [Pure]
        public static OrdinalDate FromBinary(int data)
        {
            var bin = new Yedoyx(data);
            bin.Unpack(out int y, out int doy);
            var id = (CalendarId)bin.Extra;
            return CalendarCatalog.GetSystemCalendar(id).GetOrdinalDate(y, doy);
        }

        /// <summary>
        /// Serializes the current <see cref="OrdinalDate"/> object to a 32-bit binary value that
        /// subsequently can be used to recreate the <see cref="OrdinalDate"/> object.
        /// </summary>
        /// <exception cref="NotSupportedException">Binary serialization is only supported for dates
        /// belonging to a system calendar.</exception>
        [Pure]
        public int ToBinary() => Cuid.IsFixed() ? _bin.ToBinary() : Throw.NotSupported<int>();

        #endregion
        #region Factories

        /// <summary>
        /// Obtains the current date in the Gregorian calendar on this machine, expressed in local
        /// time, not UTC.
        /// <para>To obtain the current date in another calendar, see
        /// <see cref="Calendar.GetCurrentOrdinal()"/>.</para>
        /// </summary>
        [Pure]
        public static OrdinalDate Today()
        {
            var ydoy = GregorianFormulae.GetOrdinalParts(DayNumber.Today() - DayZero.NewStyle);
            return new OrdinalDate(ydoy, Cuid.Gregorian);
        }

        //
        // CalendarYear
        //

        ///// <summary>
        ///// Enumerates the days in the specified year.
        ///// </summary>
        //[Pure]
        //public static IEnumerable<OrdinalDate> GetDaysInYear(CalendarYear year)
        //{
        //    int daysInYear = year.Calendar.Schema.CountMonthsInYear(year.Year);

        //    for (int doy = 1; doy <= daysInYear; doy++)
        //    {
        //        yield return new OrdinalDate(year.Year, doy, year.Cuid);
        //    }
        //}

        /// <summary>
        /// Obtains the first day of the specified year.
        /// </summary>
        [Pure]
        public static OrdinalDate AtStartOfYear(CalendarYear year)
        {
            ref readonly var chr = ref year.CalendarRef;
            var ydoy = chr.Schema.GetStartOfYearOrdinalParts(year.Year);
            return new OrdinalDate(ydoy, year.Cuid);
        }

        /// <summary>
        /// Obtains the ordinal date corresponding to the specified day of the specified year.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfYear"/> is outside the range of
        /// valid values.</exception>
        [Pure]
        public static OrdinalDate AtDayOfYear(CalendarYear year, int dayOfYear)
        {
            ref readonly var chr = ref year.CalendarRef;
            chr.PreValidator.ValidateDayOfYear(year.Year, dayOfYear);
            return new OrdinalDate(year.Year, dayOfYear, year.Cuid);
        }

        /// <summary>
        /// Obtains the last day of the specified year.
        /// </summary>
        [Pure]
        public static OrdinalDate AtEndOfYear(CalendarYear year)
        {
            ref readonly var chr = ref year.CalendarRef;
            var ydoy = chr.Schema.GetEndOfYearOrdinalParts(year.Year);
            return new OrdinalDate(ydoy, year.Cuid);
        }

        //
        // CalendarMonth
        //

        ///// <summary>
        ///// Enumerates the days in the specified month.
        ///// </summary>
        //[Pure]
        //public static IEnumerable<OrdinalDate> GetDaysInMonth(CalendarMonth month)
        //{
        //    var sch = month.Calendar.Schema;
        //    month.Parts.Unpack(out int y, out int m);
        //    int startOfMonth = sch.CountDaysInYearBeforeMonth(y, m);
        //    int daysInMonth = sch.CountDaysInMonth(y, m);

        //    for (int d = 1; d <= daysInMonth; d++)
        //    {
        //        yield return new OrdinalDate(y, startOfMonth + d, month.Cuid);
        //    }
        //}

        /// <summary>
        /// Obtains the first day of the specified month.
        /// </summary>
        [Pure]
        public static OrdinalDate AtStartOfMonth(CalendarMonth month)
        {
            month.Parts.Unpack(out int y, out int m);
            ref readonly var chr = ref month.CalendarRef;
            var ydoy = chr.Schema.GetStartOfMonthOrdinalParts(y, m);
            return new OrdinalDate(ydoy, month.Cuid);
        }

        /// <summary>
        /// Obtains the ordinal date corresponding to the specified day of the specified month.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfMonth"/> is outside the range of
        /// valid values.</exception>
        [Pure]
        public static OrdinalDate AtDayOfMonth(CalendarMonth month, int dayOfMonth)
        {
            month.Parts.Unpack(out int y, out int m);
            ref readonly var chr = ref month.CalendarRef;
            chr.ValidateDayOfMonth(y, m, dayOfMonth);
            int doy = chr.Schema.GetDayOfYear(y, m, dayOfMonth);
            return new OrdinalDate(new Yedoy(y, doy), month.Cuid);
        }

        /// <summary>
        /// Obtains the last day of the specified month.
        /// </summary>
        [Pure]
        public static OrdinalDate AtEndOfMonth(CalendarMonth month)
        {
            month.Parts.Unpack(out int y, out int m);
            ref readonly var chr = ref month.CalendarRef;
            var ydoy = chr.Schema.GetEndOfMonthOrdinalParts(y, m);
            return new OrdinalDate(ydoy, month.Cuid);
        }

        #endregion
        #region Conversions

        /// <summary>
        /// Obtains a new <see cref="OrdinalDate"/> in the Gregorian calendar from the specified day
        /// number.
        /// </summary>
        /// <remarks>
        /// <para>To create from a day number an instance in another calendar, see
        /// <see cref="Calendar.GetOrdinalDateOn(DayNumber)"/>. A less direct way is first to create
        /// a <see cref="CalendarDay"/> and then to convert the result to a
        /// <see cref="OrdinalDate"/>.</para>
        /// </remarks>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// values supported by the Gregorian calendar.</exception>
        [Pure]
        public static OrdinalDate FromDayNumber(DayNumber dayNumber)
        {
            GregorianProlepticShortScope.DefaultDomain.Validate(dayNumber);
            var ydoy = GregorianFormulae.GetOrdinalParts(dayNumber - DayZero.NewStyle);
            return new OrdinalDate(ydoy, Cuid.Gregorian);
        }

        /// <inheritdoc />
        [Pure]
        public DayNumber ToDayNumber()
        {
            ref readonly var chr = ref CalendarRef;
            return chr.GetDayNumber(this);
        }

        /// <inheritdoc />
        [Pure]
        public CalendarDay ToCalendarDay()
        {
            _bin.Unpack(out int y, out int doy);
            ref readonly var chr = ref CalendarRef;
            int daysSinceEpoch = chr.Schema.CountDaysSinceEpoch(y, doy);
            return new CalendarDay(daysSinceEpoch, Cuid);
        }

        /// <inheritdoc />
        [Pure]
        public CalendarDate ToCalendarDate()
        {
            _bin.Unpack(out int y, out int doy);
            ref readonly var chr = ref CalendarRef;
            var ymd = chr.Schema.GetDateParts(y, doy);
            return new CalendarDate(ymd, Cuid);
        }

        /// <inheritdoc />
        [Pure]
        OrdinalDate ISimpleDate.ToOrdinalDate() => this;

        /// <inheritdoc />
        [Pure]
        public OrdinalDate WithCalendar(Calendar newCalendar!!) =>
            newCalendar.GetOrdinalDateOn(ToDayNumber());

        #endregion
        #region Counting

        /// <inheritdoc />
        [Pure]
        public int CountElapsedDaysInYear() => DayOfYear - 1;

        /// <inheritdoc />
        [Pure]
        public int CountRemainingDaysInYear()
        {
            _bin.Unpack(out int y, out int doy);
            ref readonly var chr = ref CalendarRef;
            return chr.Schema.CountDaysInYearAfter(y, doy);
        }

        /// <inheritdoc />
        [Pure]
        public int CountElapsedDaysInMonth()
        {
            _bin.Unpack(out int y, out int doy);
            ref readonly var chr = ref CalendarRef;
            return chr.Schema.CountDaysInMonthBefore(y, doy);
        }

        /// <inheritdoc />
        [Pure]
        public int CountRemainingDaysInMonth()
        {
            _bin.Unpack(out int y, out int doy);
            ref readonly var chr = ref CalendarRef;
            return chr.Schema.CountDaysInMonthAfter(y, doy);
        }

        #endregion
        #region Year and month boundaries

        /// <inheritdoc />
        [Pure]
        public static OrdinalDate GetStartOfYear(OrdinalDate day) =>
            new(day.Parts.StartOfYear, day.Cuid);

        /// <inheritdoc />
        [Pure]
        public static OrdinalDate GetEndOfYear(OrdinalDate day)
        {
            ref readonly var chr = ref day.CalendarRef;
            var ydoy = chr.Schema.GetEndOfYearOrdinalParts(day.Year);
            return new OrdinalDate(ydoy, day.Cuid);
        }

        /// <inheritdoc />
        [Pure]
        public static OrdinalDate GetStartOfMonth(OrdinalDate day)
        {
            day.Parts.Unpack(out int y, out int doy);
            ref readonly var chr = ref day.CalendarRef;
            var sch = chr.Schema;
            int m = sch.GetMonth(y, doy, out _);
            var ydoy = sch.GetStartOfMonthOrdinalParts(y, m);
            return new OrdinalDate(ydoy, day.Cuid);
        }

        /// <inheritdoc />
        [Pure]
        public static OrdinalDate GetEndOfMonth(OrdinalDate day)
        {
            day.Parts.Unpack(out int y, out int doy);
            ref readonly var chr = ref day.CalendarRef;
            var sch = chr.Schema;
            int m = sch.GetMonth(y, doy, out _);
            var ydoy = sch.GetEndOfMonthOrdinalParts(y, m);
            return new OrdinalDate(ydoy, day.Cuid);
        }

        #endregion
        #region Adjustments

        /// <summary>
        /// Adjusts the date fields to the specified values, yielding a new ordinal date.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="adjuster"/> is null.</exception>
        /// <exception cref="AoorException">The resulting date would be invalid.</exception>
        [Pure]
        public OrdinalDate Adjust(Func<OrdinalParts, OrdinalParts> adjuster!!)
        {
            ref readonly var chr = ref CalendarRef;
            var ydoy = adjuster.Invoke(new OrdinalParts(Parts)).ToYedoy(chr.Scope);
            return new OrdinalDate(ydoy, Cuid);
        }

        /// <summary>
        /// Adjusts the year field to the specified value, yielding a new ordinal date.
        /// </summary>
        /// <exception cref="AoorException">The resulting date would be invalid.</exception>
        [Pure]
        public OrdinalDate WithYear(int newYear)
        {
            int doy = DayOfYear;
            ref readonly var chr = ref CalendarRef;
            chr.Scope.ValidateOrdinal(newYear, doy, nameof(newYear));
            return new OrdinalDate(newYear, doy, Cuid);
        }

        /// <summary>
        /// Adjusts the day of the year field to the specified value, yielding a new ordinal date.
        /// </summary>
        /// <exception cref="AoorException">The resulting date would be invalid.</exception>
        [Pure]
        public OrdinalDate WithDayOfYear(int newDayOfYear)
        {
            int y = Year;
            ref readonly var chr = ref CalendarRef;
            chr.PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));
            return new OrdinalDate(y, newDayOfYear, Cuid);
        }

        //
        // Adjust the day of the week
        //

        /// <inheritdoc />
        [Pure]
        public OrdinalDate Previous(DayOfWeek dayOfWeek)
        {
            Requires.Defined(dayOfWeek);

            ref readonly var chr = ref CalendarRef;
            int δ = dayOfWeek - chr.GetDayOfWeek(this);
            _bin.Unpack(out int y, out int doy);
            var ydoy = chr.Arithmetic.AddDaysViaDayOfYear(y, doy, δ >= 0 ? δ - CalendricalConstants.DaysInWeek : δ);
            chr.YearOverflowChecker.CheckLowerBound(ydoy.Year);
            return new OrdinalDate(ydoy, Cuid);
        }

        /// <inheritdoc />
        [Pure]
        public OrdinalDate PreviousOrSame(DayOfWeek dayOfWeek)
        {
            Requires.Defined(dayOfWeek);

            ref readonly var chr = ref CalendarRef;
            int δ = dayOfWeek - chr.GetDayOfWeek(this);
            if (δ == 0) { return this; }
            _bin.Unpack(out int y, out int doy);
            var ydoy = chr.Arithmetic.AddDaysViaDayOfYear(y, doy, δ > 0 ? δ - CalendricalConstants.DaysInWeek : δ);
            chr.YearOverflowChecker.CheckLowerBound(ydoy.Year);
            return new OrdinalDate(ydoy, Cuid);
        }

        /// <inheritdoc />
        [Pure]
        public OrdinalDate Nearest(DayOfWeek dayOfWeek)
        {
            ref readonly var chr = ref CalendarRef;
            var nearest = chr.GetDayNumber(this).Nearest(dayOfWeek);
            return chr.GetOrdinalDateOn(nearest);
        }

        /// <inheritdoc />
        [Pure]
        public OrdinalDate NextOrSame(DayOfWeek dayOfWeek)
        {
            Requires.Defined(dayOfWeek);

            ref readonly var chr = ref CalendarRef;
            int δ = dayOfWeek - chr.GetDayOfWeek(this);
            if (δ == 0) { return this; }
            _bin.Unpack(out int y, out int doy);
            var ydoy = chr.Arithmetic.AddDaysViaDayOfYear(y, doy, δ < 0 ? δ + CalendricalConstants.DaysInWeek : δ);
            chr.YearOverflowChecker.CheckUpperBound(ydoy.Year);
            return new OrdinalDate(ydoy, Cuid);
        }

        /// <inheritdoc />
        [Pure]
        public OrdinalDate Next(DayOfWeek dayOfWeek)
        {
            Requires.Defined(dayOfWeek);

            ref readonly var chr = ref CalendarRef;
            int δ = dayOfWeek - chr.GetDayOfWeek(this);
            _bin.Unpack(out int y, out int doy);
            var ydoy = chr.Arithmetic.AddDaysViaDayOfYear(y, doy, δ <= 0 ? δ + CalendricalConstants.DaysInWeek : δ);
            chr.YearOverflowChecker.CheckUpperBound(ydoy.Year);
            return new OrdinalDate(ydoy, Cuid);
        }

        #endregion
    }

    public partial struct OrdinalDate // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="OrdinalDate"/> are equal.
        /// </summary>
        public static bool operator ==(OrdinalDate left, OrdinalDate right) =>
            left._bin == right._bin;

        /// <summary>
        /// Determines whether two specified instances of <see cref="OrdinalDate"/> are not equal.
        /// </summary>
        public static bool operator !=(OrdinalDate left, OrdinalDate right) =>
            left._bin != right._bin;

        /// <inheritdoc />
        [Pure]
        public bool Equals(OrdinalDate other) => _bin == other._bin;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is OrdinalDate date && Equals(date);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => HashCode.Combine(_bin, Cuid);
    }

    public partial struct OrdinalDate // IComparable
    {
        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly earlier than the
        /// right one.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="right"/> belongs to a different
        /// calendar than <paramref name="right"/>.</exception>
        public static bool operator <(OrdinalDate left, OrdinalDate right) =>
            left.CompareTo(right) < 0;

        /// <summary>
        /// Compares the two specified instances to see if the left one is earlier than or equal to
        /// the right one.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="right"/> belongs to a different
        /// calendar than <paramref name="right"/>.</exception>
        public static bool operator <=(OrdinalDate left, OrdinalDate right) =>
            left.CompareTo(right) <= 0;

        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly later than the
        /// right one.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="right"/> belongs to a different
        /// calendar than <paramref name="right"/>.</exception>
        public static bool operator >(OrdinalDate left, OrdinalDate right) =>
            left.CompareTo(right) > 0;

        /// <summary>
        /// Compares the two specified instances to see if the left one is later than or equal to
        /// the right one.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="right"/> belongs to a different
        /// calendar than <paramref name="right"/>.</exception>
        public static bool operator >=(OrdinalDate left, OrdinalDate right) =>
            left.CompareTo(right) >= 0;

        /// <summary>
        /// Obtains the earlier date of two specified dates.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="y"/> belongs to a different calendar
        /// than <paramref name="y"/>.</exception>
        [Pure]
        public static OrdinalDate Min(OrdinalDate x, OrdinalDate y) =>
            x.CompareTo(y) < 0 ? x : y;

        /// <summary>
        /// Obtains the later date of two specified dates.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="y"/> belongs to a different calendar
        /// than <paramref name="y"/>.</exception>
        [Pure]
        public static OrdinalDate Max(OrdinalDate x, OrdinalDate y) =>
            x.CompareTo(y) > 0 ? x : y;

        /// <summary>
        /// Indicates whether this instance is earlier, later or the same as the specified one.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="other"/> does not belong to the
        /// calendar of the current instance.</exception>
        [Pure]
        public int CompareTo(OrdinalDate other)
        {
            if (other.Cuid != Cuid) Throw.BadCuid(nameof(other), Cuid, other.Cuid);

            return Parts.CompareTo(other.Parts);
        }

        /// <summary>
        /// Indicates whether this instance is earlier, later or the same as the specified one.
        /// <para>This method does NOT validate its parameter.</para>
        /// </summary>
        [Pure]
        internal int CompareFast(OrdinalDate other)
        {
            Debug.Assert(other.Cuid == Cuid);

            return Parts.CompareTo(other.Parts);
        }

        /// <inheritdoc />
        [Pure]
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is OrdinalDate date ? CompareTo(date)
            : Throw.NonComparable(typeof(OrdinalDate), obj);
    }

    public partial struct OrdinalDate // Math ops
    {
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage)
        // Friendly alternates do exist but use domain-specific names.

        /// <summary>
        /// Subtracts the two specified dates and returns the number of days between them.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="right"/> belongs to a different
        /// calendar than <paramref name="right"/>.</exception>
        public static int operator -(OrdinalDate left, OrdinalDate right) =>
            left.CountDaysSince(right);

        /// <summary>
        /// Adds a number of days to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow either the capacity of
        /// <see cref="Int32"/> or the range of supported dates.</exception>
        public static OrdinalDate operator +(OrdinalDate value, int days) => value.PlusDays(days);

        /// <summary>
        /// Subtracts a number of days to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow either the capacity of
        /// <see cref="Int32"/> or the range of supported dates.</exception>
        public static OrdinalDate operator -(OrdinalDate value, int days) => value.PlusDays(-days);

        /// <summary>
        /// Adds one day to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the latest supported
        /// date.</exception>
        public static OrdinalDate operator ++(OrdinalDate value) => value.NextDay();

        /// <summary>
        /// Subtracts one day to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the earliest supported
        /// date.</exception>
        public static OrdinalDate operator --(OrdinalDate value) => value.PreviousDay();

#pragma warning restore CA2225

        /// <inheritdoc />
        [Pure]
        public int CountDaysSince(OrdinalDate other)
        {
            if (other.Cuid != Cuid) Throw.BadCuid(nameof(other), Cuid, other.Cuid);

            ref readonly var chr = ref CalendarRef;
            return chr.Arithmetic.CountDaysBetween(other.Parts, Parts);
        }

        /// <inheritdoc />
        [Pure]
        public OrdinalDate PlusDays(int days)
        {
            ref readonly var chr = ref CalendarRef;
            var ydoy = chr.Arithmetic.AddDays(Parts, days);
            chr.YearOverflowChecker.Check(ydoy.Year);
            return new OrdinalDate(ydoy, Cuid);
        }

        /// <inheritdoc />
        [Pure]
        public OrdinalDate NextDay()
        {
            ref readonly var chr = ref CalendarRef;
            var ydoy = chr.Arithmetic.NextDay(Parts);
            chr.YearOverflowChecker.CheckUpperBound(ydoy.Year);
            return new OrdinalDate(ydoy, Cuid);
        }

        /// <inheritdoc />
        [Pure]
        public OrdinalDate PreviousDay()
        {
            ref readonly var chr = ref CalendarRef;
            var ydoy = chr.Arithmetic.PreviousDay(Parts);
            chr.YearOverflowChecker.CheckLowerBound(ydoy.Year);
            return new OrdinalDate(ydoy, Cuid);
        }

        /// <summary>
        /// Counts the number of years elapsed since the specified date.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="other"/> does not belong to the
        /// calendar of the current instance.</exception>
        [Pure]
        public int CountYearsSince(OrdinalDate other)
        {
            if (other.Cuid != Cuid) Throw.BadCuid(nameof(other), Cuid, other.Cuid);

            ref readonly var chr = ref CalendarRef;
            return chr.Math.CountYearsBetweenCore(other, this);
        }

        /// <summary>
        /// Adds a number of years to the year field of this date instance, yielding a new date.
        /// </summary>
        [Pure]
        public OrdinalDate PlusYears(int years)
        {
            ref readonly var chr = ref CalendarRef;
            return chr.Math.AddYearsCore(this, years);
        }
    }
}
