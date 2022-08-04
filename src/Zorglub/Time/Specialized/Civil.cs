// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Schemas;
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Hemerology;
    using Zorglub.Time.Hemerology.Scopes;

    // We use GregorianStandardScope instead of s_Calendar.Scope because they
    // are strictly equivalent.
    // We use daysSinceZero instead of daysSinceEpoch because s_Calendar.Epoch
    // is equal to DayNumber.Zero.

    /// <summary>
    /// Represents the Civil calendar.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class CivilCalendar :
        MinMaxYearCalendar<CivilDate>,
        IRegularFeaturette
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CivilCalendar"/> class.
        /// </summary>
        public CivilCalendar() : this(new CivilSchema()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CivilCalendar"/> class.
        /// </summary>
        internal CivilCalendar(CivilSchema schema)
            : base("Gregorian", new StandardScope(schema, DayZero.NewStyle))
        {
            MonthsInYear = schema.MonthsInYear;
        }

        /// <inheritdoc/>
        public int MonthsInYear { get; }

        /// <inheritdoc/>
        [Pure]
        protected sealed override CivilDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
    }

    /// <summary>
    /// Provides common adjusters for <see cref="CivilDate"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class CivilAdjusters : IDateAdjusters<CivilDate>
    {
        /// <summary>
        /// Represents the Civil schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly CivilSchema _schema;

        /// <summary>
        /// Initializes a new instance of the <see cref="CivilAdjusters"/> class.
        /// </summary>
        public CivilAdjusters() : this(new CivilSchema()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CivilAdjusters"/> class.
        /// </summary>
        internal CivilAdjusters(CivilSchema schema)
        {
            Requires.NotNull(schema);

            _schema = schema;
        }

        /// <inheritdoc />
        [Pure]
        public CivilDate GetStartOfYear(CivilDate date)
        {
            int daysSinceZero = CivilFormulae.GetStartOfYear(date.Year);
            return new CivilDate(daysSinceZero);
        }

        /// <inheritdoc />
        [Pure]
        public CivilDate GetEndOfYear(CivilDate date)
        {
            int daysSinceZero = _schema.GetEndOfYear(date.Year);
            return new CivilDate(daysSinceZero);
        }

        /// <inheritdoc />
        [Pure]
        public CivilDate GetStartOfMonth(CivilDate date)
        {
            CivilFormulae.GetDateParts(date.DaysSinceZero, out int y, out int m, out _);
            int daysSinceZero = _schema.GetStartOfMonth(y, m);
            return new CivilDate(daysSinceZero);
        }

        /// <inheritdoc />
        [Pure]
        public CivilDate GetEndOfMonth(CivilDate date)
        {
            CivilFormulae.GetDateParts(date.DaysSinceZero, out int y, out int m, out _);
            int daysSinceZero = _schema.GetEndOfMonth(y, m);
            return new CivilDate(daysSinceZero);
        }
    }

    /// <summary>
    /// Represents the Civil date.
    /// <para><see cref="CivilDate"/> is an immutable struct.</para>
    /// </summary>
    public readonly partial struct CivilDate : IDate<CivilDate, CivilCalendar>
    {
        /// <summary>
        /// Represents the schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly CivilSchema s_Schema = new();

        /// <summary>
        /// Represents the calendar.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly CivilCalendar s_Calendar = new(s_Schema);

        /// <summary>
        /// Represents the domain, the interval of supported <see cref="DayNumber"/>.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly Range<DayNumber> s_Domain = s_Calendar.Domain;

        /// <summary>
        /// Represents the adjusters.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly CivilAdjusters s_Adjusters = new(s_Schema);

        /// <summary>
        /// Represents the smallest possible value of a <see cref="CivilDate"/>.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly CivilDate s_MinValue = new(s_Domain.Min.DaysSinceZero);

        /// <summary>
        /// Represents the largest possible value of a <see cref="CivilDate"/>.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly CivilDate s_MaxValue = new(s_Domain.Max.DaysSinceZero);

        /// <summary>
        /// Represents the count of days since the Gregorian epoch.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _daysSinceZero;

        /// <summary>
        /// Initializes a new instance of the <see cref="CivilDate"/> struct to the specified
        /// date parts.
        /// </summary>
        /// <exception cref="AoorException">The specified components do not form a valid date or
        /// <paramref name="year"/> is outside the range of years supported by
        /// <see cref="CivilCalendar"/>.</exception>
        public CivilDate(int year, int month, int day)
        {
            GregorianStandardScope.ValidateYearMonthDay(year, month, day);

            _daysSinceZero = CivilFormulae.CountDaysSinceEpoch(year, month, day);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CivilDate"/> struct to the specified
        /// ordinal date parts.
        /// </summary>
        /// <exception cref="AoorException">The specified components do not form a valid ordinal
        /// date or <paramref name="year"/> is outside the range of years supported by
        /// <see cref="CivilCalendar"/>.</exception>
        public CivilDate(int year, int dayOfYear)
        {
            GregorianStandardScope.ValidateOrdinal(year, dayOfYear);

            _daysSinceZero = s_Schema.CountDaysSinceEpoch(year, dayOfYear);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CivilDate"/> struct.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// supported values.</exception>
        public CivilDate(DayNumber dayNumber)
        {
            s_Domain.Validate(dayNumber);

            _daysSinceZero = dayNumber.DaysSinceZero;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CivilDate"/> struct.
        /// <para>This method does NOT validate its parameter.</para>
        /// </summary>
        internal CivilDate(int daysSinceZero)
        {
            _daysSinceZero = daysSinceZero;
        }

        /// <summary>
        /// Gets the smallest possible value of a <see cref="CivilDate"/>.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static CivilDate MinValue => s_MinValue;

        /// <summary>
        /// Gets the largest possible value of a <see cref="CivilDate"/>.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static CivilDate MaxValue => s_MaxValue;

        /// <summary>
        /// Gets the date adjusters.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static CivilAdjusters Adjusters => s_Adjusters;

        /// <inheritdoc />
        public static CivilCalendar Calendar => s_Calendar;

        /// <summary>
        /// Gets the day number.
        /// </summary>
        public DayNumber DayNumber => new(_daysSinceZero);

        /// <inheritdoc />
        public Ord CenturyOfEra => Ord.FromInt32(Century);

        /// <inheritdoc />
        public int Century => YearNumbering.GetCentury(Year);

        /// <inheritdoc />
        public Ord YearOfEra => Ord.FromInt32(Year);

        /// <inheritdoc />
        public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);

        /// <inheritdoc />
        public int Year => CivilFormulae.GetYear(_daysSinceZero);

        /// <inheritdoc />
        public int Month
        {
            get
            {
                CivilFormulae.GetDateParts(_daysSinceZero, out _, out int m, out _);
                return m;
            }
        }

        /// <inheritdoc />
        public int DayOfYear
        {
            get
            {
                _ = CivilFormulae.GetYear(_daysSinceZero, out int doy);
                return doy;
            }
        }

        /// <inheritdoc />
        public int Day
        {
            get
            {
                CivilFormulae.GetDateParts(_daysSinceZero, out _, out _, out int d);
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
                CivilFormulae.GetDateParts(_daysSinceZero, out _, out int m, out int d);
                return GregorianFormulae.IsIntercalaryDay(m, d);
            }
        }

        /// <inheritdoc />
        public bool IsSupplementary => false;

        /// <summary>
        /// Gets the count of days since the Gregorian epoch.
        /// </summary>
        internal int DaysSinceZero => _daysSinceZero;

        /// <summary>
        /// Returns a culture-independent string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString()
        {
            CivilFormulae.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
            return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({s_Calendar})");
        }

        /// <inheritdoc />
        public void Deconstruct(out int year, out int month, out int day) =>
            CivilFormulae.GetDateParts(_daysSinceZero, out year, out month, out day);

        /// <inheritdoc />
        public void Deconstruct(out int year, out int dayOfYear) =>
            year = CivilFormulae.GetYear(_daysSinceZero, out dayOfYear);
    }

    public partial struct CivilDate // Conversions, adjustments...
    {
        #region Factories

        /// <summary>
        /// Obtains the current date in the Gregorian calendar on this machine, expressed in local
        /// time, not UTC.
        /// </summary>
        [Pure]
        public static CivilDate Today() => new(DayNumber.Today().DaysSinceZero);

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
        public int CountElapsedDaysInYear() => s_Schema.CountDaysInYearBefore(_daysSinceZero);

        /// <inheritdoc />
        [Pure]
        public int CountRemainingDaysInYear() => s_Schema.CountDaysInYearAfter(_daysSinceZero);

        /// <inheritdoc />
        [Pure]
        public int CountElapsedDaysInMonth() => s_Schema.CountDaysInMonthBefore(_daysSinceZero);

        /// <inheritdoc />
        [Pure]
        public int CountRemainingDaysInMonth() => s_Schema.CountDaysInMonthAfter(_daysSinceZero);

        #endregion
        #region Adjust the day of the week

        /// <inheritdoc />
        [Pure]
        public CivilDate Previous(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.Previous(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new CivilDate(dayNumber.DaysSinceZero);
        }

        /// <inheritdoc />
        [Pure]
        public CivilDate PreviousOrSame(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new CivilDate(dayNumber.DaysSinceZero);
        }

        /// <inheritdoc />
        [Pure]
        public CivilDate Nearest(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.Nearest(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new CivilDate(dayNumber.DaysSinceZero);
        }

        /// <inheritdoc />
        [Pure]
        public CivilDate NextOrSame(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.NextOrSame(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new CivilDate(dayNumber.DaysSinceZero);
        }

        /// <inheritdoc />
        [Pure]
        public CivilDate Next(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.Next(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new CivilDate(dayNumber.DaysSinceZero);
        }

        #endregion
    }

    public partial struct CivilDate // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="CivilDate"/> are equal.
        /// </summary>
        public static bool operator ==(CivilDate left, CivilDate right) =>
            left._daysSinceZero == right._daysSinceZero;

        /// <summary>
        /// Determines whether two specified instances of <see cref="CivilDate"/> are not equal.
        /// </summary>
        public static bool operator !=(CivilDate left, CivilDate right) =>
            left._daysSinceZero != right._daysSinceZero;

        /// <inheritdoc />
        [Pure]
        public bool Equals(CivilDate other) => _daysSinceZero == other._daysSinceZero;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is CivilDate date && Equals(date);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => _daysSinceZero;
    }

    public partial struct CivilDate // IComparable
    {
        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly earlier than the
        /// right one.
        /// </summary>
        public static bool operator <(CivilDate left, CivilDate right) =>
            left._daysSinceZero < right._daysSinceZero;

        /// <summary>
        /// Compares the two specified instances to see if the left one is earlier than or equal to
        /// the right one.
        /// </summary>
        public static bool operator <=(CivilDate left, CivilDate right) =>
            left._daysSinceZero <= right._daysSinceZero;

        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly later than the
        /// right one.
        /// </summary>
        public static bool operator >(CivilDate left, CivilDate right) =>
            left._daysSinceZero > right._daysSinceZero;

        /// <summary>
        /// Compares the two specified instances to see if the left one is later than or equal to
        /// the right one.
        /// </summary>
        public static bool operator >=(CivilDate left, CivilDate right) =>
            left._daysSinceZero >= right._daysSinceZero;

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
        public int CompareTo(CivilDate other) => _daysSinceZero.CompareTo(other._daysSinceZero);

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
            _daysSinceZero - other._daysSinceZero;

        /// <inheritdoc />
        [Pure]
        public CivilDate PlusDays(int days)
        {
            int daysSinceZero = checked(_daysSinceZero + days);
            GregorianStandardScope.DaysValidator.CheckOverflow(daysSinceZero);
            return new(daysSinceZero);
        }

        /// <inheritdoc />
        [Pure]
        public CivilDate NextDay() =>
            this == s_MaxValue ? Throw.DateOverflow<CivilDate>() : new CivilDate(_daysSinceZero + 1);

        /// <inheritdoc />
        [Pure]
        public CivilDate PreviousDay() =>
            this == s_MinValue ? Throw.DateOverflow<CivilDate>() : new CivilDate(_daysSinceZero - 1);
    }
}
