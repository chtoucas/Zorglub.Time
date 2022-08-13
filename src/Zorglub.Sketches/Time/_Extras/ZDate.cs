// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Schemas;
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Hemerology;
    using Zorglub.Time.Hemerology.Scopes;

    // TODO(code): ordinal ctor, serialization, adjustments, non-standard arithmetic.

    // ZDate is modeled after CalendarDay, not CalendarDate.
    // - faster standard arithmetic
    // - no need for a custom arithmetic object which requires Yemoda & co
    // - faster adjustment of the day of the week
    // - faster interconversion
    // - default value = 1/1/1
    // Slow operations:
    // - obtaining the y/m/d representation
    // - non-standard arithmetic

    /// <summary>
    /// Represents a date within a calendar system of type <see cref="ZCalendar"/>.
    /// <para><see cref="ZDate"/> is an immutable struct.</para>
    /// </summary>
    public readonly partial struct ZDate : IDate<ZDate>
    {
        /// <summary>
        /// Represents the count of consecutive days since the epoch of the calendar to which belongs
        /// the current instance.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _daysSinceEpoch;

        /// <summary>
        /// Represents the ID of the calendar to which belongs the current instance.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _cuid;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZDate"/> struct to the specified day
        /// number in the Gregorian calendar.
        /// <para>To create an instance for another calendar, see
        /// <see cref="ZCalendar.GetDate(DayNumber)"/>.</para>
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// values supported by the Gregorian calendar.</exception>
        public ZDate(DayNumber dayNumber)
        {
            // We don't use:
            // > ZCalendar.Gregorian.Domain.Validate(dayNumber);
            // but GregorianProlepticScope which does support the exact same
            // range of days.
            GregorianProlepticScope.DefaultDomain.Validate(dayNumber);

            _daysSinceEpoch = dayNumber - DayZero.NewStyle;
            _cuid = (int)CalendarId.Gregorian;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZDate"/> struct to the specified date
        /// parts in the Gregorian calendar.
        /// </summary>
        public ZDate(int year, int month, int day)
        {
            GregorianProlepticScope.ValidateYearMonthDay(year, month, day);

            _daysSinceEpoch = GregorianFormulae.CountDaysSinceEpoch(year, month, day);
            _cuid = (int)CalendarId.Gregorian;
        }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="ZDate"/> struct to the specified ordinal
        ///// date parts in the Gregorian calendar.
        ///// </summary>
        //public ZDate(int year, int dayOfYear)
        //{
        //    GregorianProlepticScope.ValidateOrdinal(year, dayOfYear);

        //    _daysSinceEpoch = GregorianFormulae.GetStartOfYear(year) + dayOfYear - 1;
        //    _cuid = (int)CalendarId.Gregorian;
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="ZDate"/> struct.
        /// <para>This constructor does NOT validate its parameters.</para>
        /// </summary>
        internal ZDate(int daysSinceEpoch, int cuid)
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
                ref readonly var chr = ref CalendarRef;
                return chr.Epoch + _daysSinceEpoch;
            }
        }

        /// <summary>
        /// Gets the count of days since the epoch of the calendar to which belongs the current
        /// instance.
        /// </summary>
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
        public int Year
        {
            get
            {
                ref readonly var chr = ref CalendarRef;
                return chr.Schema.GetYear(_daysSinceEpoch, out _);
            }
        }

        /// <inheritdoc />
        public int Month
        {
            get
            {
                ref readonly var chr = ref CalendarRef;
                chr.Schema.GetDateParts(_daysSinceEpoch, out _, out int m, out _);
                return m;
            }
        }

        /// <inheritdoc />
        public int DayOfYear
        {
            get
            {
                ref readonly var chr = ref CalendarRef;
                _ = chr.Schema.GetYear(_daysSinceEpoch, out int doy);
                return doy;
            }
        }

        /// <inheritdoc />
        public int Day
        {
            get
            {
                ref readonly var chr = ref CalendarRef;
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
                ref readonly var chr = ref CalendarRef;
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
                ref readonly var chr = ref CalendarRef;
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
        public ZCalendar Calendar => ZCatalog.GetCalendarUnchecked(_cuid);

        /// <summary>
        /// Gets the ID of the calendar to which belongs the current instance.
        /// </summary>
        internal int Cuid => _cuid;

        /// <summary>
        /// Gets a read-only reference to the calendar to which belongs the current instance.
        /// </summary>
        internal ref readonly ZCalendar CalendarRef
        {
            // CIL code size = XXX bytes <= 32 bytes.
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref ZCatalog.GetCalendarUnsafe(_cuid);
        }

        /// <summary>
        /// Returns a culture-independent string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString()
        {
            ref readonly var chr = ref CalendarRef;
            chr.Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({chr})");
        }

        /// <inheritdoc />
        public void Deconstruct(out int year, out int month, out int day)
        {
            ref readonly var chr = ref CalendarRef;
            chr.Schema.GetDateParts(_daysSinceEpoch, out year, out month, out day);
        }

        /// <inheritdoc />
        public void Deconstruct(out int year, out int dayOfYear)
        {
            ref readonly var chr = ref CalendarRef;
            year = chr.Schema.GetYear(_daysSinceEpoch, out dayOfYear);
        }
    }

    public partial struct ZDate // Factories, conversions, adjustments, etc.
    {
        #region Binary serialization
        // Peut faire mieux, en particulier pour vérifier qu'il s'agit d'un
        // calendrier système ou lors de la conversion de extraData à CalendarId.

        ///// <summary>
        ///// Deserializes a 32-bit binary value and recreates an original serialized
        ///// <see cref="Simple.CalendarDate"/> object.
        ///// </summary>
        ///// <exception cref="AoorException">The matching calendar is not a system calendar.
        ///// </exception>
        //[Pure]
        //public static ZDate FromBinary(long data)
        //{
        //    var bin = Yemoda.FromBinary(data, out uint extraData);
        //    bin.Deconstruct(out int y, out int m, out int d);
        //    if (extraData > Byte.MaxValue) Throw.ArgumentOutOfRange(nameof(data));
        //    var id = (CalendarId)extraData;
        //    return ZCatalog.GetSystemCalendar(id).GetDate(y, m, d);
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
        /// Obtains the current date in the <i>Gregorian</i> calendar on this machine, expressed in
        /// local time, not UTC.
        /// </summary>
        [Pure]
        public static ZDate Today() => new(DayNumber.Today().DaysSinceZero, (int)CalendarId.Gregorian);

        #endregion
        #region Conversions

        /// <inheritdoc />
        [Pure]
        DayNumber IFixedDate.ToDayNumber() => DayNumber;

        /// <summary>
        /// Interconverts the current instance to a date within a different calendar.
        /// </summary>
        /// <remarks>
        /// <para>This method always performs the conversion whether it's necessary or not. To avoid
        /// an expensive operation, it's better to check before that <paramref name="newCalendar"/>
        /// is actually different from the calendar of the current instance.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
        /// </exception>
        /// <exception cref="AoorException">The specified date cannot be converted into the new
        /// calendar, the resulting date would be outside its range of years.</exception>
        [Pure]
        public ZDate WithCalendar(ZCalendar newCalendar)
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
            ref readonly var chr = ref CalendarRef;
            _ = chr.Schema.GetYear(_daysSinceEpoch, out int doy);
            return doy - 1;
        }

        /// <inheritdoc />
        [Pure]
        public int CountRemainingDaysInYear()
        {
            ref readonly var chr = ref CalendarRef;
            var sch = chr.Schema;
            int y = sch.GetYear(_daysSinceEpoch, out int doy);
            return sch.CountDaysInYear(y) - doy;
        }

        /// <inheritdoc />
        [Pure]
        public int CountElapsedDaysInMonth()
        {
            ref readonly var chr = ref CalendarRef;
            chr.Schema.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
            return d - 1;
        }

        /// <inheritdoc />
        [Pure]
        public int CountRemainingDaysInMonth()
        {
            ref readonly var chr = ref CalendarRef;
            var sch = chr.Schema;
            sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return sch.CountDaysInMonth(y, m) - d;
        }

        #endregion
        #region Adjustments

        /// <summary>
        /// Adjusts the current instance using the specified adjuster.
        /// <para>If the adjuster throws, this method will propagate the exception.</para>
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="adjuster"/> is null.</exception>
        [Pure]
        public ZDate Adjust(Func<ZDate, ZDate> adjuster)
        {
            Requires.NotNull(adjuster);

            return adjuster.Invoke(this);
        }

        /// <summary>
        /// Adjusts the day number field to the specified values, yielding a new calendar day.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="newDayNumber"/> is outside the range of
        /// supported values.</exception>
        [Pure]
        [Obsolete("To be removed.")]
        public ZDate WithDayNumber(DayNumber newDayNumber)
        {
            ref readonly var chr = ref CalendarRef;
            chr.Domain.Validate(newDayNumber);
            return new ZDate(newDayNumber - chr.Epoch, _cuid);
        }

        ///// <inheritdoc/>
        //[Pure]
        //public ZDate WithYear(int newYear)
        //{
        //    _bin.Unpack(out _, out int m, out int d);
        //    var chr = Calendar;
        //    chr.Scope.ValidateYearMonthDay(newYear, m, d, nameof(newYear));
        //    return new ZDate(newYear, m, d, _cuid);
        //}

        ///// <inheritdoc/>
        //[Pure]
        //public ZDate WithMonth(int newMonth)
        //{
        //    _bin.Unpack(out int y, out _, out int d);
        //    var chr = Calendar;
        //    chr.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));
        //    return new ZDate(y, newMonth, d, _cuid);
        //}

        ///// <inheritdoc/>
        //[Pure]
        //public ZDate WithDay(int newDay)
        //{
        //    _bin.Unpack(out int y, out int m);
        //    var chr = Calendar;
        //    chr.ValidateDayOfMonth(y, m, newDay, nameof(newDay));
        //    return new ZDate(y, m, newDay, _cuid);
        //}

        //
        // Adjust the day of the week
        //

        /// <inheritdoc />
        [Pure]
        public ZDate Previous(DayOfWeek dayOfWeek)
        {
            Requires.Defined(dayOfWeek);

            int δ = dayOfWeek - DayOfWeek;
            return PlusDays(δ >= 0 ? δ - CalendricalConstants.DaysInWeek : δ);
        }

        /// <inheritdoc />
        [Pure]
        public ZDate PreviousOrSame(DayOfWeek dayOfWeek)
        {
            Requires.Defined(dayOfWeek);

            int δ = dayOfWeek - DayOfWeek;
            return δ == 0 ? this : PlusDays(δ > 0 ? δ - CalendricalConstants.DaysInWeek : δ);
        }

        /// <inheritdoc />
        [Pure]
        public ZDate Nearest(DayOfWeek dayOfWeek)
        {
            ref readonly var chr = ref CalendarRef;
            var epoch = chr.Epoch;
            var dayNumber = epoch + _daysSinceEpoch;
            var nearest = dayNumber.Nearest(dayOfWeek);
            chr.Domain.CheckOverflow(nearest);
            return new ZDate(nearest - epoch, _cuid);
        }

        /// <inheritdoc />
        [Pure]
        public ZDate NextOrSame(DayOfWeek dayOfWeek)
        {
            Requires.Defined(dayOfWeek);

            int δ = dayOfWeek - DayOfWeek;
            return δ == 0 ? this : PlusDays(δ < 0 ? δ + CalendricalConstants.DaysInWeek : δ);
        }

        /// <inheritdoc />
        [Pure]
        public ZDate Next(DayOfWeek dayOfWeek)
        {
            Requires.Defined(dayOfWeek);

            int δ = dayOfWeek - DayOfWeek;
            return PlusDays(δ <= 0 ? δ + CalendricalConstants.DaysInWeek : δ);
        }

        #endregion
    }

    public partial struct ZDate // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="ZDate"/> are equal.
        /// </summary>
        public static bool operator ==(ZDate left, ZDate right) =>
            left._daysSinceEpoch == right._daysSinceEpoch && left._cuid == right._cuid;

        /// <summary>
        /// Determines whether two specified instances of <see cref="ZDate"/> are not equal.
        /// </summary>
        public static bool operator !=(ZDate left, ZDate right) => !(left == right);

        /// <inheritdoc />
        [Pure]
        public bool Equals(ZDate other) => this == other;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is ZDate date && this == date;

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => HashCode.Combine(_daysSinceEpoch, _cuid);
    }

    public partial struct ZDate // IComparable
    {
        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly earlier than the
        /// right one.
        /// </summary>
        public static bool operator <(ZDate left, ZDate right) => left.CompareTo(right) < 0;

        /// <summary>
        /// Compares the two specified instances to see if the left one is earlier than or equal to
        /// the right one.
        /// </summary>
        public static bool operator <=(ZDate left, ZDate right) => left.CompareTo(right) <= 0;

        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly later than the
        /// right one.
        /// </summary>
        public static bool operator >(ZDate left, ZDate right) => left.CompareTo(right) > 0;

        /// <summary>
        /// Compares the two specified instances to see if the left one is later than or equal to
        /// the right one.
        /// </summary>
        public static bool operator >=(ZDate left, ZDate right) => left.CompareTo(right) >= 0;

        /// <summary>
        /// Obtains the earlier date of two specified dates.
        /// </summary>
        [Pure]
        public static ZDate Min(ZDate x, ZDate y) => x.CompareTo(y) < 0 ? x : y;

        /// <summary>
        /// Obtains the later date of two specified dates.
        /// </summary>
        [Pure]
        public static ZDate Max(ZDate x, ZDate y) => x.CompareTo(y) > 0 ? x : y;

        /// <summary>
        /// Indicates whether this instance is earlier, later or the same as the specified one.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="other"/> does not belong to the
        /// calendar of the current instance.</exception>
        [Pure]
        public int CompareTo(ZDate other)
        {
            if (other._cuid != _cuid) Throw.BadCuid(nameof(other), _cuid, other._cuid);

            return _daysSinceEpoch.CompareTo(other._daysSinceEpoch);
        }

        /// <summary>
        /// Indicates whether this instance is earlier, later or the same as the specified one.
        /// <para>This method does NOT validate its parameter.</para>
        /// </summary>
        [Pure]
        internal int CompareFast(ZDate other)
        {
            Debug.Assert(other._cuid == _cuid);

            return _daysSinceEpoch.CompareTo(other._daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is ZDate date ? CompareTo(date)
            : Throw.NonComparable(typeof(ZDate), obj);
    }

    public partial struct ZDate // Math ops
    {
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) ✓
        // Friendly alternates do exist but use domain-specific names.

        /// <summary>
        /// Subtracts the two specified dates and returns the number of days between them.
        /// </summary>
        public static int operator -(ZDate left, ZDate right) => left.CountDaysSince(right);

        /// <summary>
        /// Adds a number of days to the specified date, yielding a new date.
        /// </summary>
        public static ZDate operator +(ZDate value, int days) => value.PlusDays(days);

        /// <summary>
        /// Subtracts a number of days to the specified date, yielding a new date.
        /// </summary>
        public static ZDate operator -(ZDate value, int days) => value.PlusDays(-days);

        /// <summary>
        /// Adds one day to the specified date, yielding a new date.
        /// </summary>
        public static ZDate operator ++(ZDate value) => value.NextDay();

        /// <summary>
        /// Subtracts one day to the specified date, yielding a new date.
        /// </summary>
        public static ZDate operator --(ZDate value) => value.PreviousDay();

#pragma warning restore CA2225

        /// <inheritdoc />
        [Pure]
        public int CountDaysSince(ZDate other)
        {
            if (other._cuid != _cuid) Throw.BadCuid(nameof(other), _cuid, other._cuid);

            return checked(_daysSinceEpoch - other._daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public ZDate PlusDays(int days)
        {
            var daysSinceEpoch = checked(_daysSinceEpoch + days);
            ref readonly var chr = ref CalendarRef;
            chr.Domain.CheckOverflow(chr.Epoch + daysSinceEpoch);
            return new ZDate(daysSinceEpoch, _cuid);
        }

        /// <inheritdoc />
        [Pure]
        public ZDate NextDay()
        {
            int daysSinceEpoch = _daysSinceEpoch + 1;
            ref readonly var chr = ref CalendarRef;
            chr.Domain.CheckUpperBound(chr.Epoch + daysSinceEpoch);
            return new ZDate(daysSinceEpoch, _cuid);
        }

        /// <inheritdoc />
        [Pure]
        public ZDate PreviousDay()
        {
            int daysSinceEpoch = _daysSinceEpoch - 1;
            ref readonly var chr = ref CalendarRef;
            chr.Domain.CheckLowerBound(chr.Epoch + daysSinceEpoch);
            return new ZDate(daysSinceEpoch, _cuid);
        }
    }
}
