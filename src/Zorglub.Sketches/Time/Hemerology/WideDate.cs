// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Schemas;

    using static Zorglub.Time.Core.Domains.DomainExtensions;

    // TODO(code): ordinal ctor, serialization, adjustments, non-standard arithmetic.

    // WideDate is modeled after CalendarDay, not CalendarDate.
    // - faster standard arithmetic
    // - no need for a custom arithmetic object which requires Yemoda & co
    // - faster adjustment of the day of the week
    // - faster interconversion
    // - default value = 1/1/1
    // Slow operations:
    // - obtaining the y/m/d representation
    // - non-standard arithmetic

    /// <summary>
    /// Represents a date within a calendar system of type <see cref="WideCalendar"/>.
    /// <para><see cref="WideDate"/> is an immutable struct.</para>
    /// </summary>
    public readonly partial struct WideDate :
        IDate<WideDate>,
        IYearEndpointsProvider<WideDate>,
        IMonthEndpointsProvider<WideDate>,
        //IAdjustableDate<WideDate>,
        ISubtractionOperators<WideDate, int, WideDate>
    {
        /// <summary>
        /// Represents the count of days since the epoch of the calendar to which belongs the current
        /// instance.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _daysSinceEpoch; // 4 bytes

        /// <summary>
        /// Represents the internal identifier of the calendar to which belongs the current instance.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _cuid; // 4 bytes

        /// <summary>
        /// Initializes a new instance of the <see cref="WideDate"/> struct to the specified day
        /// number in the Gregorian calendar.
        /// <para>To create an instance for another calendar, see
        /// <see cref="WideCalendar.GetDate(DayNumber)"/>.</para>
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// values supported by the Gregorian calendar.</exception>
        public WideDate(DayNumber dayNumber)
        {
            WideCalendar.Gregorian.Domain.Validate(dayNumber);

            _daysSinceEpoch = dayNumber - DayZero.NewStyle;
            _cuid = (int)CalendarId.Gregorian;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WideDate"/> struct to the specified date
        /// parts in the Gregorian calendar.
        /// </summary>
        public WideDate(int year, int month, int day)
        {
            WideCalendar.ValidateGregorianParts(year, month, day);

            _daysSinceEpoch = GregorianFormulae.CountDaysSinceEpoch(year, month, day);
            _cuid = (int)CalendarId.Gregorian;
        }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="WideDate"/> struct to the specified
        ///// ordinal date parts in the Gregorian calendar.
        ///// </summary>
        //public WideDate(int year, int dayOfYear)
        //{
        //    WideCalendar.ValidateGregorianOrdinalParts(year, dayOfYear);

        //    _daysSinceEpoch = GregorianFormulae.GetStartOfYear(year) + dayOfYear - 1;
        //    _cuid = (int)CalendarId.Gregorian;
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="WideDate"/> struct.
        /// <para>This constructor does NOT validate its parameters.</para>
        /// </summary>
        internal WideDate(int daysSinceEpoch, int cuid)
        {
            _daysSinceEpoch = daysSinceEpoch;
            _cuid = cuid;
        }

        /// <summary>
        /// Gets the day number.
        /// </summary>
        public DayNumber DayNumber
        {
            get
            {
                var chr = Calendar;
                return chr.Epoch + _daysSinceEpoch;
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
        public int Year
        {
            get
            {
                var chr = Calendar;
                return chr.Schema.GetYear(_daysSinceEpoch, out _);
            }
        }

        /// <inheritdoc />
        public int Month
        {
            get
            {
                var chr = Calendar;
                chr.Schema.GetDateParts(_daysSinceEpoch, out _, out int m, out _);
                return m;
            }
        }

        /// <inheritdoc />
        public int DayOfYear
        {
            get
            {
                var chr = Calendar;
                _ = chr.Schema.GetYear(_daysSinceEpoch, out int doy);
                return doy;
            }
        }

        /// <inheritdoc />
        public int Day
        {
            get
            {
                var chr = Calendar;
                chr.Schema.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
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
                var chr = Calendar;
                var sch = chr.Schema;
                sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
                return sch.IsIntercalaryDay(y, m, d);
            }
        }

        /// <inheritdoc />
        public bool IsSupplementary
        {
            get
            {
                var chr = Calendar;
                var sch = chr.Schema;
                sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
                return sch.IsSupplementaryDay(y, m, d);
            }
        }

        /// <summary>
        /// Gets the calendar to which belongs the current instance.
        /// </summary>
        /// <remarks>
        /// <para>Performance tip: cache this property locally if used repeatedly within a code
        /// block.</para>
        /// </remarks>
        public WideCalendar Calendar => WideCatalog.GetCalendarUnchecked(_cuid);

        /// <summary>
        /// Gets the internal ID of the calendar to which belongs the current instance.
        /// </summary>
        internal int Cuid => _cuid;

        /// <summary>
        /// Returns a culture-independent string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString()
        {
            var chr = Calendar;
            chr.Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({chr})");
        }

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        public void Deconstruct(out int year, out int month, out int day)
        {
            var chr = Calendar;
            chr.Schema.GetDateParts(_daysSinceEpoch, out year, out month, out day);
        }
    }

    public partial struct WideDate // Conversions, adjustments...
    {
        #region Binary serialization
        // TODO: peut faire mieux, en particulier pour vérifier qu'il s'agit
        // d'un calendrier système ou lors de la conversion de extraData à
        // CalendarId.

        ///// <summary>
        ///// Deserializes a 32-bit binary value and recreates an original serialized
        ///// <see cref="Simple.CalendarDate"/> object.
        ///// </summary>
        ///// <exception cref="AoorException">The matching calendar is not a system calendar.
        ///// </exception>
        //[Pure]
        //public static WideDate FromBinary(long data)
        //{
        //    var bin = Yemoda.FromBinary(data, out uint extraData);
        //    bin.Deconstruct(out int y, out int m, out int d);
        //    if (extraData > Byte.MaxValue) Throw.ArgumentOutOfRange(nameof(data));
        //    var id = (CalendarId)extraData;
        //    return WideCatalog.GetSystemCalendar(id).GetDate(y, m, d);
        //}

        ///// <summary>
        ///// Serializes the current <see cref="Simple.CalendarDate"/> object to a 32-bit binary value
        ///// that subsequently can be used to recreate the <see cref="Simple.CalendarDate"/> object.
        ///// </summary>
        ///// <exception cref="NotSupportedException">Binary serialization is only supported for dates
        ///// belonging to a system calendar.</exception>
        //[Pure]
        //public long ToBinary() =>
        //    _cuid > (int)CalendarId.Zoroastrian ? Throw.NotSupported<long>()
        //        : _bin.ToBinary((uint)_cuid);

        #endregion
        #region Factories

        /// <summary>
        /// Obtains the current date in the Gregorian calendar on this machine, expressed in local
        /// time, not UTC.
        /// </summary>
        [Pure]
        public static WideDate Today() => new(DayNumber.Today());

        #endregion
        #region Conversions

        /// <inheritdoc />
        [Pure]
        static WideDate IFixedDay<WideDate>.FromDayNumber(DayNumber dayNumber) => new(dayNumber);

        /// <inheritdoc />
        [Pure]
        DayNumber IFixedDay.ToDayNumber() => DayNumber;

        /// <summary>
        /// Interconverts the current instance to a date within a different calendar.
        /// </summary>
        /// <remarks>
        /// This method always performs the conversion whether it's necessary or not. To avoid an
        /// expensive operation, it's better to check before that <paramref name="newCalendar"/> is
        /// actually different from the calendar of the current instance.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
        /// </exception>
        /// <exception cref="AoorException">The specified date cannot be converted into the new
        /// calendar, the resulting date would be outside its range of years.</exception>
        [Pure]
        public WideDate WithCalendar(WideCalendar newCalendar)
        {
            Requires.NotNull(newCalendar);

            return newCalendar.GetDate(DayNumber);
        }

        #endregion
        #region Counting

        /// <inheritdoc />
        [Pure]
        public int CountElapsedDaysInYear()
        {
            var chr = Calendar;
            _ = chr.Schema.GetYear(_daysSinceEpoch, out int doy);
            return doy - 1;
        }

        /// <inheritdoc />
        [Pure]
        public int CountRemainingDaysInYear()
        {
            var chr = Calendar;
            var sch = chr.Schema;
            int y = sch.GetYear(_daysSinceEpoch, out int doy);
            return sch.CountDaysInYear(y) - doy;
        }

        /// <inheritdoc />
        [Pure]
        public int CountElapsedDaysInMonth()
        {
            var chr = Calendar;
            chr.Schema.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
            return d - 1;
        }

        /// <inheritdoc />
        [Pure]
        public int CountRemainingDaysInMonth()
        {
            var chr = Calendar;
            var sch = chr.Schema;
            sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return sch.CountDaysInMonth(y, m) - d;
        }

        #endregion
        #region Year and month boundaries

        // REVIEW(api): on a déjà Calendar.GetStartOfYear()
        // Le seul avantage à avoir ces méthodes ici est qu'on n'a pas à
        // revalider les paramètres.
        // On pourrait rajouter
        // > T GetStartOfYear(T)
        // à l'API de ICalendar<T>. Cela nous permettrait de gérer le cas de
        // DayNumber pour lequel on ne dispose pas de méthode équivalente.

        /// <inheritdoc />
        [Pure]
        public static WideDate GetStartOfYear(WideDate day)
        {
            var chr = day.Calendar;
            var sch = chr.Schema;
            int y = sch.GetYear(day._daysSinceEpoch, out _);
            var startOfYear = sch.GetStartOfYear(y);
            return new(startOfYear, day._cuid);
        }

        /// <inheritdoc />
        [Pure]
        public static WideDate GetEndOfYear(WideDate day)
        {
            var chr = day.Calendar;
            var sch = chr.Schema;
            int y = sch.GetYear(day._daysSinceEpoch, out _);
            var endOfYear = sch.GetEndOfYear(y);
            return new(endOfYear, day._cuid);
        }

        /// <inheritdoc />
        [Pure]
        public static WideDate GetStartOfMonth(WideDate day)
        {
            var chr = day.Calendar;
            var sch = chr.Schema;
            sch.GetDateParts(day._daysSinceEpoch, out int y, out int m, out _);
            var startOfYear = sch.GetStartOfMonth(y, m);
            return new(startOfYear, day._cuid);
        }

        /// <inheritdoc />
        [Pure]
        public static WideDate GetEndOfMonth(WideDate day)
        {
            var chr = day.Calendar;
            var sch = chr.Schema;
            sch.GetDateParts(day._daysSinceEpoch, out int y, out int m, out _);
            var startOfYear = sch.GetEndOfMonth(y, m);
            return new(startOfYear, day._cuid);
        }

        #endregion
        #region Adjustments

        /// <summary>
        /// Adjusts the day number field to the specified values, yielding a new calendar day.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="newDayNumber"/> is outside the range of
        /// supported values.</exception>
        [Pure]
        public WideDate WithDayNumber(DayNumber newDayNumber)
        {
            var chr = Calendar;
            chr.Domain.Validate(newDayNumber);
            return new WideDate(newDayNumber - chr.Epoch, Cuid);
        }

        ///// <inheritdoc/>
        //[Pure]
        //public WideDate Adjust(Func<DateParts, DateParts> adjuster)
        //{
        //    Requires.NotNull(adjuster);

        //    var chr = Calendar;
        //    var ymd = adjuster.Invoke(new DateParts(Parts)).ToYemoda(chr.Scope);
        //    return new WideDate(ymd, Cuid);
        //}

        ///// <inheritdoc/>
        //[Pure]
        //public WideDate WithYear(int newYear)
        //{
        //    _bin.Unpack(out _, out int m, out int d);
        //    var chr = Calendar;
        //    chr.Scope.ValidateYearMonthDay(newYear, m, d, nameof(newYear));
        //    return new WideDate(newYear, m, d, Cuid);
        //}

        ///// <inheritdoc/>
        //[Pure]
        //public WideDate WithMonth(int newMonth)
        //{
        //    _bin.Unpack(out int y, out _, out int d);
        //    var chr = Calendar;
        //    chr.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));
        //    return new WideDate(y, newMonth, d, Cuid);
        //}

        ///// <inheritdoc/>
        //[Pure]
        //public WideDate WithDay(int newDay)
        //{
        //    _bin.Unpack(out int y, out int m);
        //    var chr = Calendar;
        //    chr.ValidateDayOfMonth(y, m, newDay, nameof(newDay));
        //    return new WideDate(y, m, newDay, Cuid);
        //}

        //
        // Adjust the day of the week
        //

        /// <inheritdoc />
        [Pure]
        public WideDate Previous(DayOfWeek dayOfWeek)
        {
            Requires.Defined(dayOfWeek);

            int δ = dayOfWeek - DayOfWeek;
            return PlusDays(δ >= 0 ? δ - CalendricalConstants.DaysInWeek : δ);
        }

        /// <inheritdoc />
        [Pure]
        public WideDate PreviousOrSame(DayOfWeek dayOfWeek)
        {
            Requires.Defined(dayOfWeek);

            int δ = dayOfWeek - DayOfWeek;
            return δ == 0 ? this : PlusDays(δ > 0 ? δ - CalendricalConstants.DaysInWeek : δ);
        }

        /// <inheritdoc />
        [Pure]
        public WideDate Nearest(DayOfWeek dayOfWeek)
        {
            var chr = Calendar;
            var epoch = chr.Epoch;
            var dayNumber = epoch + _daysSinceEpoch;
            var nearest = dayNumber.Nearest(dayOfWeek);
            chr.Domain.CheckOverflow(nearest);
            return new(nearest - epoch, Cuid);
        }

        /// <inheritdoc />
        [Pure]
        public WideDate NextOrSame(DayOfWeek dayOfWeek)
        {
            Requires.Defined(dayOfWeek);

            int δ = dayOfWeek - DayOfWeek;
            return δ == 0 ? this : PlusDays(δ < 0 ? δ + CalendricalConstants.DaysInWeek : δ);
        }

        /// <inheritdoc />
        [Pure]
        public WideDate Next(DayOfWeek dayOfWeek)
        {
            Requires.Defined(dayOfWeek);

            int δ = dayOfWeek - DayOfWeek;
            return PlusDays(δ <= 0 ? δ + CalendricalConstants.DaysInWeek : δ);
        }

        #endregion
    }

    public partial struct WideDate // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="WideDate"/> are equal.
        /// </summary>
        public static bool operator ==(WideDate left, WideDate right) =>
            left._daysSinceEpoch == right._daysSinceEpoch && left._cuid == right._cuid;

        /// <summary>
        /// Determines whether two specified instances of <see cref="WideDate"/> are not equal.
        /// </summary>
        public static bool operator !=(WideDate left, WideDate right) => !(left == right);

        /// <inheritdoc />
        [Pure]
        public bool Equals(WideDate other) => this == other;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is WideDate date && this == date;

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => HashCode.Combine(_daysSinceEpoch, _cuid);
    }

    public partial struct WideDate // IComparable
    {
        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly earlier than the
        /// right one.
        /// </summary>
        public static bool operator <(WideDate left, WideDate right) => left.CompareTo(right) < 0;

        /// <summary>
        /// Compares the two specified instances to see if the left one is earlier than or equal to
        /// the right one.
        /// </summary>
        public static bool operator <=(WideDate left, WideDate right) => left.CompareTo(right) <= 0;

        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly later than the
        /// right one.
        /// </summary>
        public static bool operator >(WideDate left, WideDate right) => left.CompareTo(right) > 0;

        /// <summary>
        /// Compares the two specified instances to see if the left one is later than or equal to
        /// the right one.
        /// </summary>
        public static bool operator >=(WideDate left, WideDate right) => left.CompareTo(right) >= 0;

        /// <summary>
        /// Obtains the earlier date of two specified dates.
        /// </summary>
        [Pure]
        public static WideDate Min(WideDate x, WideDate y) => x.CompareTo(y) < 0 ? x : y;

        /// <summary>
        /// Obtains the later date of two specified dates.
        /// </summary>
        [Pure]
        public static WideDate Max(WideDate x, WideDate y) => x.CompareTo(y) > 0 ? x : y;

        /// <summary>
        /// Indicates whether this instance is earlier, later or the same as the specified one.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="other"/> does not belong to the
        /// calendar of the current instance.</exception>
        [Pure]
        public int CompareTo(WideDate other)
        {
            if (other._cuid != _cuid) Throw.BadCuid(nameof(other), _cuid, other._cuid);

            return _daysSinceEpoch.CompareTo(other._daysSinceEpoch);
        }

        /// <summary>
        /// Indicates whether this instance is earlier, later or the same as the specified one.
        /// <para>This method does NOT validate its parameter.</para>
        /// </summary>
        [Pure]
        internal int CompareFast(WideDate other)
        {
            Debug.Assert(other._cuid == _cuid);

            return _daysSinceEpoch.CompareTo(other._daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is WideDate date ? CompareTo(date)
            : Throw.NonComparable(typeof(WideDate), obj);
    }

    public partial struct WideDate // Math ops
    {
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage)
        // Friendly alternates do exist but use domain-specific names.

        /// <summary>
        /// Subtracts the two specified dates and returns the number of days between them.
        /// </summary>
        public static int operator -(WideDate left, WideDate right) =>
            left.CountDaysSince(right);

        /// <summary>
        /// Adds a number of days to the specified date, yielding a new date.
        /// </summary>
        public static WideDate operator +(WideDate value, int days) => value.PlusDays(days);

        /// <summary>
        /// Subtracts a number of days to the specified date, yielding a new date.
        /// </summary>
        public static WideDate operator -(WideDate value, int days) => value.PlusDays(-days);

        /// <summary>
        /// Adds one day to the specified date, yielding a new date.
        /// </summary>
        public static WideDate operator ++(WideDate value) => value.NextDay();

        /// <summary>
        /// Subtracts one day to the specified date, yielding a new date.
        /// </summary>
        public static WideDate operator --(WideDate value) => value.PreviousDay();

#pragma warning restore CA2225

        /// <inheritdoc />
        [Pure]
        public int CountDaysSince(WideDate other)
        {
            if (other._cuid != _cuid) Throw.BadCuid(nameof(other), _cuid, other._cuid);

            return checked(_daysSinceEpoch - other._daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public WideDate PlusDays(int days)
        {
            var daysSinceEpoch = checked(_daysSinceEpoch + days);
            var chr = Calendar;
            chr.Domain.CheckOverflow(chr.Epoch + daysSinceEpoch);
            return new WideDate(daysSinceEpoch, Cuid);
        }

        /// <inheritdoc />
        [Pure]
        public WideDate NextDay()
        {
            int daysSinceEpoch = _daysSinceEpoch + 1;
            var chr = Calendar;
            chr.Domain.CheckUpperBound(chr.Epoch + daysSinceEpoch);
            return new WideDate(daysSinceEpoch, Cuid);
        }

        /// <inheritdoc />
        [Pure]
        public WideDate PreviousDay()
        {
            int daysSinceEpoch = _daysSinceEpoch - 1;
            var chr = Calendar;
            chr.Domain.CheckLowerBound(chr.Epoch + daysSinceEpoch);
            return new WideDate(daysSinceEpoch, Cuid);
        }
    }
}
