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

    // We use daysSinceZero instead of daysSinceEpoch because s_Calendar.Epoch
    // is equal to DayNumber.Zero.

    /// <summary>
    /// Represents the Gregorian calendar.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class GregorianCalendar :
        SpecialCalendar<GregorianDate>,
        IRegularFeaturette
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GregorianCalendar"/> class.
        /// </summary>
        public GregorianCalendar() : this(new GregorianSchema()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GregorianCalendar"/> class.
        /// </summary>
        internal GregorianCalendar(GregorianSchema schema)
            : base("Gregorian", MinMaxYearScope.CreateMaximal(schema, DayZero.NewStyle))
        {
            MonthsInYear = schema.MonthsInYear;
        }

        /// <inheritdoc/>
        public int MonthsInYear { get; }

        /// <inheritdoc/>
        [Pure]
        private protected sealed override GregorianDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
    }

    /// <summary>
    /// Provides common adjusters for <see cref="GregorianDate"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class GregorianAdjuster : SpecialAdjuster<GregorianDate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GregorianAdjuster"/> class.
        /// </summary>
        public GregorianAdjuster() : base(GregorianDate.Calendar.Scope) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GregorianAdjuster"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        internal GregorianAdjuster(CalendarScope scope) : base(scope) { }

        /// <inheritdoc/>
        [Pure]
        private protected sealed override GregorianDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
    }

    /// <summary>
    /// Represents the Gregorian date.
    /// <para><see cref="GregorianDate"/> is an immutable struct.</para>
    /// </summary>
    public readonly partial struct GregorianDate :
        IDate<GregorianDate, GregorianCalendar>,
        IDateableOrdinally
    {
        // NB: the order in which the static fields are written is important.

        /// <summary>
        /// Represents the schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly GregorianSchema s_Schema = new();

        /// <summary>
        /// Represents the calendar.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly GregorianCalendar s_Calendar = new(s_Schema);

        /// <summary>
        /// Represents the scope.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly CalendarScope s_Scope = s_Calendar.Scope;

        /// <summary>
        /// Represents the domain, the interval of supported <see cref="DayNumber"/>.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly Range<DayNumber> s_Domain = s_Calendar.Domain;

        /// <summary>
        /// Represents the date adjuster.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly GregorianAdjuster s_Adjuster = new(s_Scope);

        /// <summary>
        /// Represents the smallest possible value of a <see cref="GregorianDate"/>.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly GregorianDate s_MinValue = new(s_Domain.Min.DaysSinceZero);

        /// <summary>
        /// Represents the largest possible value of a <see cref="GregorianDate"/>.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly GregorianDate s_MaxValue = new(s_Domain.Max.DaysSinceZero);

        /// <summary>
        /// Represents the count of days since the Gregorian epoch.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _daysSinceZero;

        /// <summary>
        /// Initializes a new instance of the <see cref="GregorianDate"/> struct to the specified
        /// date parts.
        /// </summary>
        /// <exception cref="AoorException">The specified components do not form a valid date or
        /// <paramref name="year"/> is outside the range of years supported by
        /// <see cref="GregorianCalendar"/>.</exception>
        public GregorianDate(int year, int month, int day)
        {
            s_Scope.ValidateYearMonthDay(year, month, day);

            _daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(year, month, day);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GregorianDate"/> struct to the specified
        /// ordinal date parts.
        /// </summary>
        /// <exception cref="AoorException">The specified components do not form a valid ordinal
        /// date or <paramref name="year"/> is outside the range of years supported by
        /// <see cref="GregorianCalendar"/>.</exception>
        public GregorianDate(int year, int dayOfYear)
        {
            s_Scope.ValidateOrdinal(year, dayOfYear);

            _daysSinceZero = s_Schema.CountDaysSinceEpoch(year, dayOfYear);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GregorianDate"/> struct.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// supported values.</exception>
        public GregorianDate(DayNumber dayNumber)
        {
            s_Domain.Validate(dayNumber);

            _daysSinceZero = dayNumber.DaysSinceZero;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GregorianDate"/> struct.
        /// <para>This method does NOT validate its parameter.</para>
        /// </summary>
        internal GregorianDate(int daysSinceZero)
        {
            _daysSinceZero = daysSinceZero;
        }

        /// <summary>
        /// Gets the smallest possible value of a <see cref="GregorianDate"/>.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static GregorianDate MinValue => s_MinValue;

        /// <summary>
        /// Gets the largest possible value of a <see cref="GregorianDate"/>.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static GregorianDate MaxValue => s_MaxValue;

        /// <summary>
        /// Gets the date adjuster.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static GregorianAdjuster Adjuster => s_Adjuster;

        /// <inheritdoc />
        public static GregorianCalendar Calendar => s_Calendar;

        /// <inheritdoc />
        public DayNumber DayNumber => new(_daysSinceZero);

        int IDateableOrdinally.DaysSinceEpoch => _daysSinceZero;

        /// <summary>
        /// Gets the count of days since the Gregorian epoch.
        /// </summary>
        public int DaysSinceZero => _daysSinceZero;

        /// <inheritdoc />
        public Ord CenturyOfEra => Ord.FromInt32(Century);

        /// <inheritdoc />
        public int Century => YearNumbering.GetCentury(Year);

        /// <inheritdoc />
        public Ord YearOfEra => Ord.FromInt32(Year);

        /// <inheritdoc />
        public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);

        /// <inheritdoc />
        public int Year => GregorianFormulae.GetYear(_daysSinceZero);

        /// <inheritdoc />
        public int Month
        {
            get
            {
                GregorianFormulae.GetDateParts(_daysSinceZero, out _, out int m, out _);
                return m;
            }
        }

        /// <inheritdoc />
        public int DayOfYear
        {
            get
            {
                _ = GregorianFormulae.GetYear(_daysSinceZero, out int doy);
                return doy;
            }
        }

        /// <inheritdoc />
        public int Day
        {
            get
            {
                GregorianFormulae.GetDateParts(_daysSinceZero, out _, out _, out int d);
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
                GregorianFormulae.GetDateParts(_daysSinceZero, out _, out int m, out int d);
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
            GregorianFormulae.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
            return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({s_Calendar})");
        }

        /// <inheritdoc />
        public void Deconstruct(out int year, out int month, out int day) =>
            GregorianFormulae.GetDateParts(_daysSinceZero, out year, out month, out day);

        /// <inheritdoc />
        public void Deconstruct(out int year, out int dayOfYear) =>
            year = GregorianFormulae.GetYear(_daysSinceZero, out dayOfYear);
    }

    public partial struct GregorianDate // Conversions, adjustments...
    {
        #region Factories

        /// <summary>
        /// Obtains the current date in the Gregorian calendar on this machine, expressed in local
        /// time, not UTC.
        /// </summary>
        [Pure]
        public static GregorianDate Today() => new(DayNumber.Today().DaysSinceZero);

        #endregion
        #region Conversions

        [Pure]
        static GregorianDate IFixedDate<GregorianDate>.FromDayNumber(DayNumber dayNumber) =>
            new(dayNumber);

        [Pure]
        DayNumber IFixedDate.ToDayNumber() => DayNumber;

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
        #region Adjustments

        /// <summary>
        /// Adjusts the current instance using the specified adjuster.
        /// <para>If the adjuster throws, this method will propagate the exception.</para>
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="adjuster"/> is null.</exception>
        [Pure]
        public GregorianDate Adjust(Func<GregorianDate, GregorianDate> adjuster)
        {
            Requires.NotNull(adjuster);

            return adjuster.Invoke(this);
        }

        //
        // Adjust the day of the week
        //

        /// <inheritdoc />
        [Pure]
        public GregorianDate Previous(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.Previous(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new GregorianDate(dayNumber.DaysSinceZero);
        }

        /// <inheritdoc />
        [Pure]
        public GregorianDate PreviousOrSame(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new GregorianDate(dayNumber.DaysSinceZero);
        }

        /// <inheritdoc />
        [Pure]
        public GregorianDate Nearest(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.Nearest(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new GregorianDate(dayNumber.DaysSinceZero);
        }

        /// <inheritdoc />
        [Pure]
        public GregorianDate NextOrSame(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.NextOrSame(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new GregorianDate(dayNumber.DaysSinceZero);
        }

        /// <inheritdoc />
        [Pure]
        public GregorianDate Next(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.Next(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new GregorianDate(dayNumber.DaysSinceZero);
        }

        #endregion
    }

    public partial struct GregorianDate // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="GregorianDate"/> are equal.
        /// </summary>
        public static bool operator ==(GregorianDate left, GregorianDate right) =>
            left._daysSinceZero == right._daysSinceZero;

        /// <summary>
        /// Determines whether two specified instances of <see cref="GregorianDate"/> are not equal.
        /// </summary>
        public static bool operator !=(GregorianDate left, GregorianDate right) =>
            left._daysSinceZero != right._daysSinceZero;

        /// <inheritdoc />
        [Pure]
        public bool Equals(GregorianDate other) => _daysSinceZero == other._daysSinceZero;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is GregorianDate date && Equals(date);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => _daysSinceZero;
    }

    public partial struct GregorianDate // IComparable
    {
        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly earlier than the
        /// right one.
        /// </summary>
        public static bool operator <(GregorianDate left, GregorianDate right) =>
            left._daysSinceZero < right._daysSinceZero;

        /// <summary>
        /// Compares the two specified instances to see if the left one is earlier than or equal to
        /// the right one.
        /// </summary>
        public static bool operator <=(GregorianDate left, GregorianDate right) =>
            left._daysSinceZero <= right._daysSinceZero;

        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly later than the
        /// right one.
        /// </summary>
        public static bool operator >(GregorianDate left, GregorianDate right) =>
            left._daysSinceZero > right._daysSinceZero;

        /// <summary>
        /// Compares the two specified instances to see if the left one is later than or equal to
        /// the right one.
        /// </summary>
        public static bool operator >=(GregorianDate left, GregorianDate right) =>
            left._daysSinceZero >= right._daysSinceZero;

        /// <summary>
        /// Obtains the earlier date of two specified dates.
        /// </summary>
        [Pure]
        public static GregorianDate Min(GregorianDate x, GregorianDate y) => x < y ? x : y;

        /// <summary>
        /// Obtains the later date of two specified dates.
        /// </summary>
        [Pure]
        public static GregorianDate Max(GregorianDate x, GregorianDate y) => x > y ? x : y;

        /// <summary>
        /// Indicates whether this instance is earlier, later or the same as the specified one.
        /// </summary>
        [Pure]
        public int CompareTo(GregorianDate other) => _daysSinceZero.CompareTo(other._daysSinceZero);

        /// <inheritdoc />
        [Pure]
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is GregorianDate date ? CompareTo(date)
            : Throw.NonComparable(typeof(GregorianDate), obj);
    }

    public partial struct GregorianDate // Math ops
    {
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) ✓
        // Friendly alternates do exist but use domain-specific names.

        /// <summary>
        /// Subtracts the two specified dates and returns the number of days between them.
        /// </summary>
        public static int operator -(GregorianDate left, GregorianDate right) =>
            left.CountDaysSince(right);

        /// <summary>
        /// Adds a number of days to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow either the capacity of
        /// <see cref="Int32"/> or the range of supported dates.</exception>
        public static GregorianDate operator +(GregorianDate value, int days) => value.PlusDays(days);

        /// <summary>
        /// Subtracts a number of days to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow either the capacity of
        /// <see cref="Int32"/> or the range of supported dates.</exception>
        public static GregorianDate operator -(GregorianDate value, int days) => value.PlusDays(-days);

        /// <summary>
        /// Adds one day to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the latest supported
        /// date.</exception>
        public static GregorianDate operator ++(GregorianDate value) => value.NextDay();

        /// <summary>
        /// Subtracts one day to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the earliest supported
        /// date.</exception>
        public static GregorianDate operator --(GregorianDate value) => value.PreviousDay();

#pragma warning restore CA2225

        /// <inheritdoc />
        [Pure]
        public int CountDaysSince(GregorianDate other) =>
            // No need to use a checked context here.
            _daysSinceZero - other._daysSinceZero;

        /// <inheritdoc />
        [Pure]
        public GregorianDate PlusDays(int days)
        {
            int daysSinceZero = checked(_daysSinceZero + days);
            // We don't write:
            // > Domain.CheckOverflow(Epoch + daysSinceEpoch);
            // The addition may also overflow...
            s_Scope.DaysValidator.CheckOverflow(daysSinceZero);
            return new(daysSinceZero);
        }

        /// <inheritdoc />
        [Pure]
        public GregorianDate NextDay() =>
            this == s_MaxValue ? Throw.DateOverflow<GregorianDate>() : new GregorianDate(_daysSinceZero + 1);

        /// <inheritdoc />
        [Pure]
        public GregorianDate PreviousDay() =>
            this == s_MinValue ? Throw.DateOverflow<GregorianDate>() : new GregorianDate(_daysSinceZero - 1);
    }
}
