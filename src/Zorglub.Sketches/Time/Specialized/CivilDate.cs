// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized
{
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Schemas;
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Hemerology;
    using Zorglub.Time.Hemerology.Scopes;

    // TODO(api): non-standard math, providers; idem with GregorianDate.

    public readonly partial struct CivilDate :
        IDate<CivilDate>,
        //IYearEndpointsProvider<CivilDate>,
        //IMonthEndpointsProvider<CivilDate>,
        IMinMaxValue<CivilDate>
    {
        /// <summary>
        /// Represents the Gregorian schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly CivilSchema s_Schema = new();

        /// <summary>
        /// Represents the Gregorian calendar.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly CivilCalendar s_Calendar = new(s_Schema);

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
        /// Represents the count of days since the Gregorian epoch.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _daysSinceEpoch;

        /// <summary>
        /// Initializes a new instance of the <see cref="CivilDate"/> struct to the specified
        /// date parts.
        /// </summary>
        /// <exception cref="AoorException">The specified components do not form a valid date or
        /// <paramref name="year"/> is outside the range of years supported by
        /// <see cref="CivilCalendar"/>.</exception>
        public CivilDate(int year, int month, int day)
        {
            // s_Calendar.Scope "=" GregorianStandardScope.
            GregorianStandardScope.ValidateYearMonthDay(year, month, day);

            _daysSinceEpoch = CivilFormulae.CountDaysSinceEpoch(year, month, day);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CivilDate"/> struct.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// supported values.</exception>
        public CivilDate(DayNumber dayNumber)
        {
            s_Domain.Validate(dayNumber);

            _daysSinceEpoch = dayNumber - s_Epoch;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CivilDate"/> struct.
        /// <para>This method does NOT validate its parameter.</para>
        /// </summary>
        internal CivilDate(int daysSinceEpoch)
        {
            _daysSinceEpoch = daysSinceEpoch;
        }

        /// <summary>
        /// Gets the smallest possible value of a <see cref="CivilDate"/>.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static CivilDate MinValue { get; } = new(s_Domain.Min - s_Epoch);

        /// <summary>
        /// Gets the largest possible value of a <see cref="CivilDate"/>.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static CivilDate MaxValue { get; } = new(s_Domain.Max - s_Epoch);

        /// <summary>
        /// Gets the day number.
        /// </summary>
        public DayNumber DayNumber => s_Epoch + _daysSinceEpoch;

        /// <inheritdoc />
        public Ord CenturyOfEra => Ord.FromInt32(Century);

        /// <inheritdoc />
        public int Century => YearNumbering.GetCentury(Year);

        /// <inheritdoc />
        public Ord YearOfEra => Ord.FromInt32(Year);

        /// <inheritdoc />
        public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);

        /// <inheritdoc />
        public int Year => CivilFormulae.GetYear(_daysSinceEpoch);

        /// <inheritdoc />
        public int Month
        {
            get
            {
                CivilFormulae.GetDateParts(_daysSinceEpoch, out _, out int m, out _);
                return m;
            }
        }

        /// <inheritdoc />
        public int DayOfYear
        {
            get
            {
                _ = CivilFormulae.GetYear(_daysSinceEpoch, out int doy);
                return doy;
            }
        }

        /// <inheritdoc />
        public int Day
        {
            get
            {
                CivilFormulae.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
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
                CivilFormulae.GetDateParts(_daysSinceEpoch, out _, out int m, out int d);
                return GregorianFormulae.IsIntercalaryDay(m, d);
            }
        }

        /// <inheritdoc />
        public bool IsSupplementary => false;

        /// <summary>
        /// Returns a culture-independent string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString()
        {
            CivilFormulae.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({s_Calendar})");
        }

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        public void Deconstruct(out int year, out int month, out int day) =>
            CivilFormulae.GetDateParts(_daysSinceEpoch, out year, out month, out day);
    }

    public partial struct CivilDate // Conversions, adjustments...
    {
        #region Factories

        /// <summary>
        /// Obtains the current date in the Gregorian calendar on this machine,
        /// expressed in local time, not UTC.
        /// </summary>
        [Pure]
        public static CivilDate Today() => new(DayNumber.Today() - s_Epoch);

        #endregion
        #region Conversions

        [Pure]
        static CivilDate IFixedDay<CivilDate>.FromDayNumber(DayNumber dayNumber) => new(dayNumber);

        [Pure]
        DayNumber IFixedDay.ToDayNumber() => DayNumber;

        #endregion
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
        #region Year and month boundaries

        ///// <inheritdoc />
        //[Pure]
        //public static CivilDate GetStartOfYear(CivilDate day)
        //{
        //    int daysSinceEpoch = CivilFormulae.GetStartOfYear(day.Year);
        //    return new CivilDate(daysSinceEpoch);
        //}

        ///// <inheritdoc />
        //[Pure]
        //public static CivilDate GetEndOfYear(CivilDate day)
        //{
        //    int daysSinceEpoch = s_Schema.GetEndOfYear(day.Year);
        //    return new CivilDate(daysSinceEpoch);
        //}

        ///// <inheritdoc />
        //[Pure]
        //public static CivilDate GetStartOfMonth(CivilDate day)
        //{
        //    CivilFormulae.GetDateParts(day._daysSinceEpoch, out int y, out int m, out _);
        //    int daysSinceEpoch = s_Schema.GetStartOfMonth(y, m);
        //    return new CivilDate(daysSinceEpoch);
        //}

        ///// <inheritdoc />
        //[Pure]
        //public static CivilDate GetEndOfMonth(CivilDate day)
        //{
        //    CivilFormulae.GetDateParts(day._daysSinceEpoch, out int y, out int m, out _);
        //    int daysSinceEpoch = s_Schema.GetEndOfMonth(y, m);
        //    return new CivilDate(daysSinceEpoch);
        //}

        #endregion
        #region Adjust the day of the week

        /// <inheritdoc />
        [Pure]
        public CivilDate Previous(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.Previous(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new CivilDate(dayNumber - s_Epoch);
        }

        /// <inheritdoc />
        [Pure]
        public CivilDate PreviousOrSame(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new CivilDate(dayNumber - s_Epoch);
        }

        /// <inheritdoc />
        [Pure]
        public CivilDate Nearest(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.Nearest(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new CivilDate(dayNumber - s_Epoch);
        }

        /// <inheritdoc />
        [Pure]
        public CivilDate NextOrSame(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.NextOrSame(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new CivilDate(dayNumber - s_Epoch);
        }

        /// <inheritdoc />
        [Pure]
        public CivilDate Next(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.Next(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new CivilDate(dayNumber - s_Epoch);
        }

        #endregion
    }

    public partial struct CivilDate // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="CivilDate"/> are equal.
        /// </summary>
        public static bool operator ==(CivilDate left, CivilDate right) =>
            left._daysSinceEpoch == right._daysSinceEpoch;

        /// <summary>
        /// Determines whether two specified instances of <see cref="CivilDate"/> are not equal.
        /// </summary>
        public static bool operator !=(CivilDate left, CivilDate right) =>
            left._daysSinceEpoch != right._daysSinceEpoch;

        /// <inheritdoc />
        [Pure]
        public bool Equals(CivilDate other) => _daysSinceEpoch == other._daysSinceEpoch;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is CivilDate date && Equals(date);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => _daysSinceEpoch;
    }

    public partial struct CivilDate // IComparable
    {
        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly earlier than the
        /// right one.
        /// </summary>
        public static bool operator <(CivilDate left, CivilDate right) =>
            left._daysSinceEpoch < right._daysSinceEpoch;

        /// <summary>
        /// Compares the two specified instances to see if the left one is earlier than or equal to
        /// the right one.
        /// </summary>
        public static bool operator <=(CivilDate left, CivilDate right) =>
            left._daysSinceEpoch <= right._daysSinceEpoch;

        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly later than the
        /// right one.
        /// </summary>
        public static bool operator >(CivilDate left, CivilDate right) =>
            left._daysSinceEpoch > right._daysSinceEpoch;

        /// <summary>
        /// Compares the two specified instances to see if the left one is later than or equal to
        /// the right one.
        /// </summary>
        public static bool operator >=(CivilDate left, CivilDate right) =>
            left._daysSinceEpoch >= right._daysSinceEpoch;

        /// <summary>
        /// Obtains the earlier date of two specified dates.
        /// </summary>
        [Pure]
        public static CivilDate Min(CivilDate x, CivilDate y) => x < y ? x : y;

        /// <summary>
        /// Obtains the later date of two specified dates.
        /// </summary>
        [Pure]
        public static CivilDate Max(CivilDate x, CivilDate y) => x > y ? x : y;

        /// <summary>
        /// Indicates whether this instance is earlier, later or the same as the specified one.
        /// </summary>
        [Pure]
        public int CompareTo(CivilDate other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

        /// <inheritdoc />
        [Pure]
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is CivilDate date ? CompareTo(date)
            : Throw.NonComparable(typeof(CivilDate), obj);
    }

    public partial struct CivilDate // Math ops
    {
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage)

        /// <summary>
        /// Subtracts the two specified dates and returns the number of days between them.
        /// </summary>
        public static int operator -(CivilDate left, CivilDate right) => left.CountDaysSince(right);

        /// <summary>
        /// Adds a number of days to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow either the capacity of
        /// <see cref="Int32"/> or the range of supported dates.</exception>
        public static CivilDate operator +(CivilDate value, int days) => value.PlusDays(days);

        /// <summary>
        /// Subtracts a number of days to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow either the capacity of
        /// <see cref="Int32"/> or the range of supported dates.</exception>
        public static CivilDate operator -(CivilDate value, int days) => value.PlusDays(-days);

        /// <summary>
        /// Adds one day to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the latest supported
        /// date.</exception>
        public static CivilDate operator ++(CivilDate value) => value.NextDay();

        /// <summary>
        /// Subtracts one day to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the earliest supported
        /// date.</exception>
        public static CivilDate operator --(CivilDate value) => value.PreviousDay();

#pragma warning restore CA2225

        /// <inheritdoc />
        [Pure]
        public int CountDaysSince(CivilDate other) =>
            // No need to use a checked context here.
            _daysSinceEpoch - other._daysSinceEpoch;

        /// <inheritdoc />
        [Pure]
        public CivilDate PlusDays(int days)
        {
            int daysSinceEpoch = checked(_daysSinceEpoch + days);
            GregorianStandardScope.DaysValidator.CheckOverflow(daysSinceEpoch);
            return new(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public CivilDate NextDay() =>
            this == MaxValue ? Throw.DateOverflow<CivilDate>() : new CivilDate(_daysSinceEpoch + 1);

        /// <inheritdoc />
        [Pure]
        public CivilDate PreviousDay() =>
            this == MinValue ? Throw.DateOverflow<CivilDate>() : new CivilDate(_daysSinceEpoch - 1);
    }
}
