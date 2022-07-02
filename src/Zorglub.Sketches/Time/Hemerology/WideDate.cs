// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core;

    // Methods provided by CalendarDate that do not apply to a WideDate:
    // - ToOrdinalDate()
    // - math ops w/ months, years

    /// <summary>
    /// Represents a date within a calendar system of type <see cref="WideCalendar"/>.
    /// <para><see cref="WideDate"/> is an immutable struct.</para>
    /// </summary>
    public readonly partial struct WideDate :
        IDate<WideDate>,
        IYearEndpointsProvider<WideDate>,
        IMonthEndpointsProvider<WideDate>,
        IAdjustableDate<WideDate>,
        ISubtractionOperators<WideDate, int, WideDate>
    {
        /// <summary>
        /// Represents the internal binary representation of the year, month and day components of
        /// the current instance.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly Yemoda _bin; // 4 bytes

        /// <summary>
        /// Represents the internal identifier of the calendar to which belongs the current instance.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _cuid; // 4 bytes

        /// <summary>
        /// Initializes a new instance of the <see cref="WideDate"/> struct to the specified date
        /// parts in the Gregorian calendar.
        /// </summary>
        public WideDate(int year, int month, int day)
        {
            WideCalendar.ValidateGregorianYearMonthDay(year, month, day);

            _bin = new Yemoda(year, month, day);
            _cuid = (int)CalendarId.Gregorian;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WideDate"/> struct.
        /// <para>This constructor does NOT validate its parameters.</para>
        /// </summary>
        internal WideDate(int y, int m, int d, int cuid)
        {
            _bin = new Yemoda(y, m, d);
            _cuid = cuid;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WideDate"/> struct.
        /// <para>This constructor does NOT validate its parameters.</para>
        /// </summary>
        internal WideDate(Yemoda bin, int cuid)
        {
            _bin = bin;
            _cuid = cuid;
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
                _bin.Unpack(out int y, out int m, out int d);
                return Calendar.Schema.GetDayOfYear(y, m, d);
            }
        }

        /// <inheritdoc />
        public int Day => _bin.Day;

        /// <inheritdoc />
        public DayOfWeek DayOfWeek => Calendar.GetDayOfWeek(this);

        /// <inheritdoc />
        public bool IsIntercalary
        {
            get
            {
                _bin.Unpack(out int y, out int m, out int d);
                return Calendar.Schema.IsIntercalaryDay(y, m, d);
            }
        }

        /// <inheritdoc />
        public bool IsSupplementary
        {
            get
            {
                _bin.Unpack(out int y, out int m, out int d);
                return Calendar.Schema.IsSupplementaryDay(y, m, d);
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
        /// Gets the date parts of current instance.
        /// </summary>
        internal Yemoda Parts => _bin;

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
            _bin.Unpack(out int y, out int m, out int d);
            return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({Calendar})");
        }

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        public void Deconstruct(out int year, out int month, out int day) =>
            _bin.Unpack(out year, out month, out day);
    }

    public partial struct WideDate // Conversions, adjustments...
    {
        #region Binary serialization
        // TODO: peut faire mieux, en particulier pour vérifier qu'il s'agit
        // d'un calendrier système ou lors de la conversion de extraData à
        // CalendarId.

        /// <summary>
        /// Deserializes a 32-bit binary value and recreates an original serialized
        /// <see cref="Simple.CalendarDate"/> object.
        /// </summary>
        /// <exception cref="AoorException">The matching calendar is not a system calendar.
        /// </exception>
        [Pure]
        public static WideDate FromBinary(long data)
        {
            var bin = Yemoda.FromBinary(data, out uint extraData);
            bin.Deconstruct(out int y, out int m, out int d);
            if (extraData > Byte.MaxValue) Throw.ArgumentOutOfRange(nameof(data));
            var id = (CalendarId)extraData;
            return WideCatalog.GetSystemCalendar(id).GetWideDate(y, m, d);
        }

        /// <summary>
        /// Serializes the current <see cref="Simple.CalendarDate"/> object to a 32-bit binary value
        /// that subsequently can be used to recreate the <see cref="Simple.CalendarDate"/> object.
        /// </summary>
        /// <exception cref="NotSupportedException">Binary serialization is only supported for dates
        /// belonging to a system calendar.</exception>
        [Pure]
        public long ToBinary() =>
            _cuid > (int)CalendarId.Zoroastrian ? Throw.NotSupported<long>()
                : _bin.ToBinary((uint)_cuid);

        #endregion
        #region Factories

        // TODO(code): we can do better than that.
        /// <summary>
        /// Obtains the current date in the Gregorian calendar on this machine, expressed in local
        /// time, not UTC.
        /// </summary>
        [Pure]
        public static WideDate Today() =>
            WideCalendar.Gregorian.GetWideDateOn(DayNumber.Today());

        #endregion
        #region Conversions

        /// <inheritdoc />
        [Pure]
        public static WideDate FromDayNumber(DayNumber dayNumber) =>
            WideCalendar.Gregorian.GetWideDateOn(dayNumber);

        /// <inheritdoc />
        [Pure]
        public DayNumber ToDayNumber() => Calendar.GetDayNumber(this);

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

            return newCalendar.GetWideDateOn(ToDayNumber());
        }

        #endregion
        #region Counting

        /// <inheritdoc />
        [Pure]
        public int CountElapsedDaysInYear()
        {
            _bin.Unpack(out int y, out int m, out int d);
            //return Calendar.Schema.CountDaysInYearBefore(y, m, d);
            var sch = Calendar.Schema;
            int doy = sch.GetDayOfYear(y, m, d);
            return doy - 1;
        }

        /// <inheritdoc />
        [Pure]
        public int CountRemainingDaysInYear()
        {
            _bin.Unpack(out int y, out int m, out int d);
            //return Calendar.Schema.CountDaysInYearAfter(y, m, d);
            var sch = Calendar.Schema;
            int doy = sch.GetDayOfYear(y, m, d);
            return sch.CountDaysInYear(y) - doy;
        }

        /// <inheritdoc />
        [Pure]
        public int CountElapsedDaysInMonth() => Day - 1;

        /// <inheritdoc />
        [Pure]
        public int CountRemainingDaysInMonth()
        {
            _bin.Unpack(out int y, out int m, out int d);
            //return Calendar.Schema.CountDaysInMonthAfter(y, m, d);
            return Calendar.Schema.CountDaysInMonth(y, m) - d;
        }

        #endregion
        #region Year and month boundaries

        // REVIEW: on a déjà Calendar.GetStartOfYear()
        // Le seul avantage à avoir ces méthodes ici est qu'on n'a pas à
        // revalider les paramètres.
        // On pourrait rajouter
        // > T GetStartOfYear(T)
        // à l'API de ICalendar<T>. Cela nous permettrait de gérer le cas de
        // DayNumber pour lequel on ne dispose pas de méthode équivalente.

        // FIXME(code): validation, use PartsFactory.

        /// <inheritdoc />
        [Pure]
        public static WideDate GetStartOfYear(WideDate day) => new(day._bin.StartOfYear, day._cuid);

        /// <inheritdoc />
        [Pure]
        public static WideDate GetEndOfYear(WideDate day)
        {
            int y = day.Year;
            //day.Calendar.Schema.GetEndOfYearParts(y, out int m, out int d);
            var sch = day.Calendar.Schema;
            int m = sch.CountMonthsInYear(y);
            int d = sch.CountDaysInMonth(y, m);
            return new WideDate(y, m, d, day._cuid);
        }

        /// <inheritdoc />
        [Pure]
        public static WideDate GetStartOfMonth(WideDate day) => new(day._bin.StartOfMonth, day._cuid);

        /// <inheritdoc />
        [Pure]
        public static WideDate GetEndOfMonth(WideDate day)
        {
            day._bin.Unpack(out int y, out int m);
            int d = day.Calendar.Schema.CountDaysInMonth(y, m);
            return new WideDate(y, m, d, day._cuid);
        }

        #endregion
        #region Adjustments

        /// <inheritdoc/>
        [Pure]
        public WideDate Adjust(Func<DateParts, DateParts> adjuster)
        {
            Requires.NotNull(adjuster);

            var chr = Calendar;
            var ymd = adjuster.Invoke(new DateParts(Parts)).ToYemoda(chr.Scope);
            return new WideDate(ymd, Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        public WideDate WithYear(int newYear)
        {
            _bin.Unpack(out _, out int m, out int d);
            var chr = Calendar;
            chr.Scope.ValidateYearMonthDay(newYear, m, d, nameof(newYear));
            return new WideDate(newYear, m, d, Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        public WideDate WithMonth(int newMonth)
        {
            _bin.Unpack(out int y, out _, out int d);
            var chr = Calendar;
            chr.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));
            return new WideDate(y, newMonth, d, Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        public WideDate WithDay(int newDay)
        {
            _bin.Unpack(out int y, out int m);
            var chr = Calendar;
            chr.ValidateDayOfMonth(y, m, newDay, nameof(newDay));
            return new WideDate(y, m, newDay, Cuid);
        }

        //
        // Adjust the day of the week
        //

        /// <inheritdoc />
        [Pure]
        public WideDate Previous(DayOfWeek dayOfWeek)
        {
            Requires.Defined(dayOfWeek);

            var chr = Calendar;
            var ari = chr.Schema.Arithmetic;
            int δ = dayOfWeek - DayOfWeek;
            var ymd = ari.AddDays(_bin, δ >= 0 ? δ - CalendricalConstants.DaysInWeek : δ);
            chr.YearOverflowChecker.CheckLowerBound(ymd.Year);
            return new WideDate(ymd, _cuid);
        }

        /// <inheritdoc />
        [Pure]
        public WideDate PreviousOrSame(DayOfWeek dayOfWeek)
        {
            Requires.Defined(dayOfWeek);

            var chr = Calendar;
            var ari = chr.Schema.Arithmetic;
            int δ = dayOfWeek - DayOfWeek;
            if (δ == 0) { return this; }
            var ymd = ari.AddDays(_bin, δ > 0 ? δ - CalendricalConstants.DaysInWeek : δ);
            chr.YearOverflowChecker.CheckLowerBound(ymd.Year);
            return new WideDate(ymd, _cuid);
        }

        /// <inheritdoc />
        [Pure]
        public WideDate Nearest(DayOfWeek dayOfWeek)
        {
            var chr = Calendar;
            var nearest = chr.GetDayNumber(this).Nearest(dayOfWeek);
            chr.Domain.CheckOverflow(nearest);
            chr.Schema.GetDateParts(nearest - chr.Epoch, out int y, out int m, out int d);
            return new WideDate(new Yemoda(y, m, d), Cuid);
        }

        /// <inheritdoc />
        [Pure]
        public WideDate NextOrSame(DayOfWeek dayOfWeek)
        {
            Requires.Defined(dayOfWeek);

            var chr = Calendar;
            var ari = chr.Schema.Arithmetic;
            int δ = dayOfWeek - DayOfWeek;
            if (δ == 0) { return this; }
            var ymd = ari.AddDays(_bin, δ < 0 ? δ + CalendricalConstants.DaysInWeek : δ);
            chr.YearOverflowChecker.CheckUpperBound(ymd.Year);
            return new WideDate(ymd, _cuid);
        }

        /// <inheritdoc />
        [Pure]
        public WideDate Next(DayOfWeek dayOfWeek)
        {
            Requires.Defined(dayOfWeek);

            var chr = Calendar;
            var ari = chr.Schema.Arithmetic;
            int δ = dayOfWeek - DayOfWeek;
            var ymd = ari.AddDays(_bin, δ <= 0 ? δ + CalendricalConstants.DaysInWeek : δ);
            chr.YearOverflowChecker.CheckUpperBound(ymd.Year);
            return new WideDate(ymd, _cuid);
        }

        #endregion
    }

    public partial struct WideDate // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="WideDate"/> are equal.
        /// </summary>
        public static bool operator ==(WideDate left, WideDate right) =>
            left._bin == right._bin && left._cuid == right._cuid;

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
        public override int GetHashCode() => HashCode.Combine(_bin, _cuid);
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

            return _bin.CompareTo(other._bin);
        }

        /// <summary>
        /// Indicates whether this instance is earlier, later or the same as the specified one.
        /// <para>This method does NOT validate its parameter.</para>
        /// </summary>
        [Pure]
        internal int CompareFast(WideDate other)
        {
            Debug.Assert(other._cuid == _cuid);

            return _bin.CompareTo(other._bin);
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

            var chr = Calendar;
            var ari = chr.Schema.Arithmetic;
            return ari.CountDaysBetween(other._bin, _bin);
        }

        /// <inheritdoc />
        [Pure]
        public WideDate PlusDays(int days)
        {
            var chr = Calendar;
            var ari = chr.Schema.Arithmetic;
            var ymd = ari.AddDays(_bin, days);
            chr.YearOverflowChecker.Check(ymd.Year);
            return new WideDate(ymd, _cuid);
        }

        /// <inheritdoc />
        [Pure]
        public WideDate NextDay()
        {
            var chr = Calendar;
            var ari = chr.Schema.Arithmetic;
            var ymd = ari.NextDay(_bin);
            chr.YearOverflowChecker.CheckUpperBound(ymd.Year);
            return new WideDate(ymd, _cuid);
        }

        /// <inheritdoc />
        [Pure]
        public WideDate PreviousDay()
        {
            var chr = Calendar;
            var ari = chr.Schema.Arithmetic;
            var ymd = ari.PreviousDay(_bin);
            chr.YearOverflowChecker.CheckLowerBound(ymd.Year);
            return new WideDate(ymd, _cuid);
        }
    }
}
