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

    /// <summary>
    /// Represents the Tabular Islamic calendar.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class TabularIslamicCalendar :
        SpecialCalendar<TabularIslamicDate>,
        IRegularFeaturette
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabularIslamicCalendar"/> class.
        /// </summary>
        public TabularIslamicCalendar() : this(new TabularIslamicSchema()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabularIslamicCalendar"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        internal TabularIslamicCalendar(TabularIslamicSchema schema)
            : base("Tabular Islamic", new StandardScope(schema, CalendarEpoch.TabularIslamic))
        {
            MonthsInYear = schema.MonthsInYear;
        }

        /// <inheritdoc/>
        public int MonthsInYear { get; }

        /// <inheritdoc/>
        [Pure]
        protected sealed override TabularIslamicDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
    }

    /// <summary>
    /// Provides common adjusters for <see cref="TabularIslamicDate"/>.
    /// </summary>
    public sealed class TabularIslamicAdjusters : SpecialAdjusters<TabularIslamicDate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabularIslamicAdjusters"/> class.
        /// </summary>
        public TabularIslamicAdjusters() : this(TabularIslamicDate.Calendar) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabularIslamicAdjusters"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        internal TabularIslamicAdjusters(TabularIslamicCalendar calendar) : base(calendar) { }

        /// <inheritdoc/>
        [Pure]
        protected sealed override TabularIslamicDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
    }

    /// <summary>
    /// Represents the Tabular Islamic date.
    /// <para><see cref="TabularIslamicDate"/> is an immutable struct.</para>
    /// </summary>
    public readonly partial struct TabularIslamicDate :
        IDate<TabularIslamicDate, TabularIslamicCalendar>,
        IDateableOrdinally
    {
        // NB: the order in which the static fields are written is important.

        /// <summary>
        /// Represents the schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly TabularIslamicSchema s_Schema = new();

        /// <summary>
        /// Represents the calendar.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly TabularIslamicCalendar s_Calendar = new(s_Schema);

        /// <summary>
        /// Represents the scope.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly CalendarScope s_Scope = s_Calendar.Scope;

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
        /// Represents the date adjusters.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly TabularIslamicAdjusters s_Adjusters = new(s_Calendar);

        /// <summary>
        /// Represents the smallest possible value of a <see cref="TabularIslamicDate"/>.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly TabularIslamicDate s_MinValue = new(s_Domain.Min - s_Epoch);

        /// <summary>
        /// Represents the largest possible value of a <see cref="TabularIslamicDate"/>.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly TabularIslamicDate s_MaxValue = new(s_Domain.Max - s_Epoch);

        /// <summary>
        /// Represents the count of days since the Tabular Islamic epoch.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _daysSinceEpoch;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabularIslamicDate"/> struct to the specified
        /// date parts.
        /// </summary>
        /// <exception cref="AoorException">The specified components do not form a valid date or
        /// <paramref name="year"/> is outside the range of years supported by
        /// <see cref="TabularIslamicCalendar"/>.</exception>
        public TabularIslamicDate(int year, int month, int day)
        {
            s_Scope.ValidateYearMonthDay(year, month, day);

            _daysSinceEpoch = s_Schema.CountDaysSinceEpoch(year, month, day);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabularIslamicDate"/> struct to the specified
        /// ordinal date parts.
        /// </summary>
        /// <exception cref="AoorException">The specified components do not form a valid ordinal
        /// date or <paramref name="year"/> is outside the range of years supported by
        /// <see cref="TabularIslamicCalendar"/>.</exception>
        public TabularIslamicDate(int year, int dayOfYear)
        {
            s_Scope.ValidateOrdinal(year, dayOfYear);

            _daysSinceEpoch = s_Schema.CountDaysSinceEpoch(year, dayOfYear);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabularIslamicDate"/> struct.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// supported values.</exception>
        public TabularIslamicDate(DayNumber dayNumber)
        {
            s_Domain.Validate(dayNumber);

            _daysSinceEpoch = dayNumber - s_Epoch;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabularIslamicDate"/> struct.
        /// <para>This method does NOT validate its parameter.</para>
        /// </summary>
        internal TabularIslamicDate(int daysSinceEpoch)
        {
            _daysSinceEpoch = daysSinceEpoch;
        }

        /// <summary>
        /// Gets the smallest possible value of a <see cref="TabularIslamicDate"/>.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static TabularIslamicDate MinValue => s_MinValue;

        /// <summary>
        /// Gets the largest possible value of a <see cref="TabularIslamicDate"/>.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static TabularIslamicDate MaxValue => s_MaxValue;

        /// <summary>
        /// Gets the date adjusters.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static TabularIslamicAdjusters Adjusters => s_Adjusters;

        /// <inheritdoc />
        public static TabularIslamicCalendar Calendar => s_Calendar;

        /// <inheritdoc />
        public DayNumber DayNumber => s_Epoch + _daysSinceEpoch;

        /// <inheritdoc />
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
        public int Year => s_Schema.GetYear(_daysSinceEpoch);

        /// <inheritdoc />
        public int Month
        {
            get
            {
                s_Schema.GetDateParts(_daysSinceEpoch, out _, out int m, out _);
                return m;
            }
        }

        /// <inheritdoc />
        public int DayOfYear
        {
            get
            {
                _ = s_Schema.GetYear(_daysSinceEpoch, out int doy);
                return doy;
            }
        }

        /// <inheritdoc />
        public int Day
        {
            get
            {
                s_Schema.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
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
                s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
                return s_Schema.IsIntercalaryDay(y, m, d);
            }
        }

        /// <inheritdoc />
        public bool IsSupplementary
        {
            get
            {
                s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
                return s_Schema.IsSupplementaryDay(y, m, d);
            }
        }

        /// <summary>
        /// Returns a culture-independent string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString()
        {
            s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({s_Calendar})");
        }

        /// <inheritdoc />
        public void Deconstruct(out int year, out int month, out int day) =>
            s_Schema.GetDateParts(_daysSinceEpoch, out year, out month, out day);

        /// <inheritdoc />
        public void Deconstruct(out int year, out int dayOfYear) =>
            year = s_Schema.GetYear(_daysSinceEpoch, out dayOfYear);
    }

    public partial struct TabularIslamicDate // Conversions, adjustments...
    {
        #region Factories

        /// <summary>
        /// Obtains the current date in the Tabular Islamic calendar on this machine,
        /// expressed in local time, not UTC.
        /// </summary>
        [Pure]
        public static TabularIslamicDate Today() => new(DayNumber.Today() - s_Epoch);

        #endregion
        #region Conversions

        [Pure]
        static TabularIslamicDate IFixedDate<TabularIslamicDate>.FromDayNumber(DayNumber dayNumber) =>
            new(dayNumber);

        [Pure]
        DayNumber IFixedDate.ToDayNumber() => DayNumber;

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
        #region Adjust the day of the week

        /// <inheritdoc />
        [Pure]
        public TabularIslamicDate Previous(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.Previous(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new TabularIslamicDate(dayNumber - s_Epoch);
        }

        /// <inheritdoc />
        [Pure]
        public TabularIslamicDate PreviousOrSame(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new TabularIslamicDate(dayNumber - s_Epoch);
        }

        /// <inheritdoc />
        [Pure]
        public TabularIslamicDate Nearest(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.Nearest(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new TabularIslamicDate(dayNumber - s_Epoch);
        }

        /// <inheritdoc />
        [Pure]
        public TabularIslamicDate NextOrSame(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.NextOrSame(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new TabularIslamicDate(dayNumber - s_Epoch);
        }

        /// <inheritdoc />
        [Pure]
        public TabularIslamicDate Next(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.Next(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new TabularIslamicDate(dayNumber - s_Epoch);
        }

        #endregion
    }

    public partial struct TabularIslamicDate // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="TabularIslamicDate"/> are equal.
        /// </summary>
        public static bool operator ==(TabularIslamicDate left, TabularIslamicDate right) =>
            left._daysSinceEpoch == right._daysSinceEpoch;

        /// <summary>
        /// Determines whether two specified instances of <see cref="TabularIslamicDate"/> are not equal.
        /// </summary>
        public static bool operator !=(TabularIslamicDate left, TabularIslamicDate right) =>
            left._daysSinceEpoch != right._daysSinceEpoch;

        /// <inheritdoc />
        [Pure]
        public bool Equals(TabularIslamicDate other) => _daysSinceEpoch == other._daysSinceEpoch;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is TabularIslamicDate date && Equals(date);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => _daysSinceEpoch;
    }

    public partial struct TabularIslamicDate // IComparable
    {
        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly earlier than the
        /// right one.
        /// </summary>
        public static bool operator <(TabularIslamicDate left, TabularIslamicDate right) =>
            left._daysSinceEpoch < right._daysSinceEpoch;

        /// <summary>
        /// Compares the two specified instances to see if the left one is earlier than or equal to
        /// the right one.
        /// </summary>
        public static bool operator <=(TabularIslamicDate left, TabularIslamicDate right) =>
            left._daysSinceEpoch <= right._daysSinceEpoch;

        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly later than the
        /// right one.
        /// </summary>
        public static bool operator >(TabularIslamicDate left, TabularIslamicDate right) =>
            left._daysSinceEpoch > right._daysSinceEpoch;

        /// <summary>
        /// Compares the two specified instances to see if the left one is later than or equal to
        /// the right one.
        /// </summary>
        public static bool operator >=(TabularIslamicDate left, TabularIslamicDate right) =>
            left._daysSinceEpoch >= right._daysSinceEpoch;

        /// <summary>
        /// Obtains the earlier date of two specified dates.
        /// </summary>
        [Pure]
        public static TabularIslamicDate Min(TabularIslamicDate x, TabularIslamicDate y) => x < y ? x : y;

        /// <summary>
        /// Obtains the later date of two specified dates.
        /// </summary>
        [Pure]
        public static TabularIslamicDate Max(TabularIslamicDate x, TabularIslamicDate y) => x > y ? x : y;

        /// <summary>
        /// Indicates whether this instance is earlier, later or the same as the specified one.
        /// </summary>
        [Pure]
        public int CompareTo(TabularIslamicDate other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

        /// <inheritdoc />
        [Pure]
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is TabularIslamicDate date ? CompareTo(date)
            : Throw.NonComparable(typeof(TabularIslamicDate), obj);
    }

    public partial struct TabularIslamicDate // Math ops
    {
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) ✓
        // Friendly alternates do exist but use domain-specific names.

        /// <summary>
        /// Subtracts the two specified dates and returns the number of days between them.
        /// </summary>
        public static int operator -(TabularIslamicDate left, TabularIslamicDate right) =>
            left.CountDaysSince(right);

        /// <summary>
        /// Adds a number of days to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow either the capacity of
        /// <see cref="Int32"/> or the range of supported dates.</exception>
        public static TabularIslamicDate operator +(TabularIslamicDate value, int days) => value.PlusDays(days);

        /// <summary>
        /// Subtracts a number of days to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow either the capacity of
        /// <see cref="Int32"/> or the range of supported dates.</exception>
        public static TabularIslamicDate operator -(TabularIslamicDate value, int days) => value.PlusDays(-days);

        /// <summary>
        /// Adds one day to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the latest supported
        /// date.</exception>
        public static TabularIslamicDate operator ++(TabularIslamicDate value) => value.NextDay();

        /// <summary>
        /// Subtracts one day to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the earliest supported
        /// date.</exception>
        public static TabularIslamicDate operator --(TabularIslamicDate value) => value.PreviousDay();

#pragma warning restore CA2225

        /// <inheritdoc />
        [Pure]
        public int CountDaysSince(TabularIslamicDate other) =>
            // No need to use a checked context here.
            _daysSinceEpoch - other._daysSinceEpoch;

        /// <inheritdoc />
        [Pure]
        public TabularIslamicDate PlusDays(int days)
        {
            int daysSinceEpoch = checked(_daysSinceEpoch + days);
            s_Scope.DaysValidator.CheckOverflow(daysSinceEpoch);
            return new(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TabularIslamicDate NextDay() =>
            this == s_MaxValue ? Throw.DateOverflow<TabularIslamicDate>() : new TabularIslamicDate(_daysSinceEpoch + 1);

        /// <inheritdoc />
        [Pure]
        public TabularIslamicDate PreviousDay() =>
            this == s_MinValue ? Throw.DateOverflow<TabularIslamicDate>() : new TabularIslamicDate(_daysSinceEpoch - 1);
    }
}
