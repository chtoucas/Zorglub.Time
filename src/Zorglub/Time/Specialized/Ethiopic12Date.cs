// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized
{
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Schemas;
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Hemerology;
    using Zorglub.Time.Hemerology.Scopes;

    /// <summary>
    /// Represents the Ethiopic calendar.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class Ethiopic12Calendar : MinMaxYearCalendar<Ethiopic12Date>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ethiopic12Calendar"/> class.
        /// </summary>
        public Ethiopic12Calendar() : this(new Coptic12Schema()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ethiopic12Calendar"/> class.
        /// </summary>
        // Constructor for Ethiopic12Date.
        internal Ethiopic12Calendar(Coptic12Schema schema)
            : base("Ethiopic", new StandardScope(schema, CalendarEpoch.Ethiopic)) { }

        /// <inheritdoc/>
        [Pure]
        protected sealed override Ethiopic12Date GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
    }

    /// <summary>
    /// Represents the Ethiopic date.
    /// <para><see cref="Ethiopic12Date"/> is an immutable struct.</para>
    /// </summary>
    public readonly partial struct Ethiopic12Date :
        IDate<Ethiopic12Date>,
        IEpagomenalDate<Ethiopic12Date>,
        IMinMaxValue<Ethiopic12Date>
    {
        // NB: the order in which the static fields are written is important.

        /// <summary>
        /// Represents the Egyptian12 schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly Coptic12Schema s_Schema = new();

        /// <summary>
        /// Represents the Ethiopic calendar.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly Ethiopic12Calendar s_Calendar = new(s_Schema);

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
        /// Represents the smallest possible value of a <see cref="Ethiopic12Date"/>.
        /// <para>This is also the default value for <see cref="Ethiopic12Date"/>.</para>
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly Ethiopic12Date s_MinValue = new(s_Domain.Min - s_Epoch);

        /// <summary>
        /// Represents the largest possible value of a <see cref="Ethiopic12Date"/>.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly Ethiopic12Date s_MaxValue = new(s_Domain.Max - s_Epoch);

        /// <summary>
        /// Represents the count of days since the Ethiopic epoch.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _daysSinceEpoch;

        /// <summary>
        /// Initializes a new instance of the <see cref="Ethiopic12Date"/> struct to the specified
        /// date parts.
        /// </summary>
        /// <exception cref="AoorException">The specified components do not form a valid date or
        /// <paramref name="year"/> is outside the range of years supported by
        /// <see cref="Ethiopic12Calendar"/>.</exception>
        public Ethiopic12Date(int year, int month, int day)
        {
            s_Scope.ValidateYearMonthDay(year, month, day);

            _daysSinceEpoch = s_Schema.CountDaysSinceEpoch(year, month, day);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ethiopic12Date"/> struct to the specified
        /// ordinal date parts.
        /// </summary>
        /// <exception cref="AoorException">The specified components do not form a valid ordinal
        /// date or <paramref name="year"/> is outside the range of years supported by
        /// <see cref="Ethiopic12Calendar"/>.</exception>
        public Ethiopic12Date(int year, int dayOfYear)
        {
            s_Scope.ValidateOrdinal(year, dayOfYear);

            _daysSinceEpoch = s_Schema.CountDaysSinceEpoch(year, dayOfYear);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ethiopic12Date"/> struct.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// supported values.</exception>
        public Ethiopic12Date(DayNumber dayNumber)
        {
            s_Domain.Validate(dayNumber);

            _daysSinceEpoch = dayNumber - s_Epoch;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ethiopic12Date"/> struct.
        /// <para>This method does NOT validate its parameter.</para>
        /// </summary>
        internal Ethiopic12Date(int daysSinceEpoch)
        {
            _daysSinceEpoch = daysSinceEpoch;
        }

        /// <summary>
        /// Gets the smallest possible value of a <see cref="Ethiopic12Date"/>.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static Ethiopic12Date MinValue => s_MinValue;

        /// <summary>
        /// Gets the largest possible value of a <see cref="Ethiopic12Date"/>.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static Ethiopic12Date MaxValue => s_MaxValue;

        /// <summary>
        /// Gets the calendar to which belongs the current instance.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static Ethiopic12Calendar Calendar => s_Calendar;

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
        /// Gets the count of days since the Ethiopic epoch.
        /// </summary>
        internal int DaysSinceEpoch => _daysSinceEpoch;

        /// <summary>
        /// Returns a culture-independent string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString()
        {
            s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({s_Calendar})");
        }

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        public void Deconstruct(out int year, out int month, out int day) =>
            s_Schema.GetDateParts(_daysSinceEpoch, out year, out month, out day);

        /// <inheritdoc />
        [Pure]
        public bool IsEpagomenal(out int epagomenalNumber)
        {
            s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
        }
    }

    public partial struct Ethiopic12Date // Conversions, adjustments...
    {
        #region Factories

        /// <summary>
        /// Obtains the current date in the Ethiopic calendar on this machine,
        /// expressed in local time, not UTC.
        /// </summary>
        [Pure]
        public static Ethiopic12Date Today() => new(DayNumber.Today() - s_Epoch);

        #endregion
        #region Conversions

        [Pure]
        static Ethiopic12Date IFixedDay<Ethiopic12Date>.FromDayNumber(DayNumber dayNumber) =>
            new(dayNumber);

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
        #region Adjust the day of the week

        /// <inheritdoc />
        [Pure]
        public Ethiopic12Date Previous(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.Previous(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new Ethiopic12Date(dayNumber - s_Epoch);
        }

        /// <inheritdoc />
        [Pure]
        public Ethiopic12Date PreviousOrSame(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new Ethiopic12Date(dayNumber - s_Epoch);
        }

        /// <inheritdoc />
        [Pure]
        public Ethiopic12Date Nearest(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.Nearest(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new Ethiopic12Date(dayNumber - s_Epoch);
        }

        /// <inheritdoc />
        [Pure]
        public Ethiopic12Date NextOrSame(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.NextOrSame(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new Ethiopic12Date(dayNumber - s_Epoch);
        }

        /// <inheritdoc />
        [Pure]
        public Ethiopic12Date Next(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.Next(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new Ethiopic12Date(dayNumber - s_Epoch);
        }

        #endregion
    }

    public partial struct Ethiopic12Date // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="Ethiopic12Date"/> are equal.
        /// </summary>
        public static bool operator ==(Ethiopic12Date left, Ethiopic12Date right) =>
            left._daysSinceEpoch == right._daysSinceEpoch;

        /// <summary>
        /// Determines whether two specified instances of <see cref="Ethiopic12Date"/> are not equal.
        /// </summary>
        public static bool operator !=(Ethiopic12Date left, Ethiopic12Date right) =>
            left._daysSinceEpoch != right._daysSinceEpoch;

        /// <inheritdoc />
        [Pure]
        public bool Equals(Ethiopic12Date other) => _daysSinceEpoch == other._daysSinceEpoch;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Ethiopic12Date date && Equals(date);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => _daysSinceEpoch;
    }

    public partial struct Ethiopic12Date // IComparable
    {
        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly earlier than the
        /// right one.
        /// </summary>
        public static bool operator <(Ethiopic12Date left, Ethiopic12Date right) =>
            left._daysSinceEpoch < right._daysSinceEpoch;

        /// <summary>
        /// Compares the two specified instances to see if the left one is earlier than or equal to
        /// the right one.
        /// </summary>
        public static bool operator <=(Ethiopic12Date left, Ethiopic12Date right) =>
            left._daysSinceEpoch <= right._daysSinceEpoch;

        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly later than the
        /// right one.
        /// </summary>
        public static bool operator >(Ethiopic12Date left, Ethiopic12Date right) =>
            left._daysSinceEpoch > right._daysSinceEpoch;

        /// <summary>
        /// Compares the two specified instances to see if the left one is later than or equal to
        /// the right one.
        /// </summary>
        public static bool operator >=(Ethiopic12Date left, Ethiopic12Date right) =>
            left._daysSinceEpoch >= right._daysSinceEpoch;

        /// <summary>
        /// Obtains the earlier date of two specified dates.
        /// </summary>
        [Pure]
        public static Ethiopic12Date Min(Ethiopic12Date x, Ethiopic12Date y) => x < y ? x : y;

        /// <summary>
        /// Obtains the later date of two specified dates.
        /// </summary>
        [Pure]
        public static Ethiopic12Date Max(Ethiopic12Date x, Ethiopic12Date y) => x > y ? x : y;

        /// <summary>
        /// Indicates whether this instance is earlier, later or the same as the specified one.
        /// </summary>
        [Pure]
        public int CompareTo(Ethiopic12Date other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

        /// <inheritdoc />
        [Pure]
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is Ethiopic12Date date ? CompareTo(date)
            : Throw.NonComparable(typeof(Ethiopic12Date), obj);
    }

    public partial struct Ethiopic12Date // Math ops
    {
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage)

        /// <summary>
        /// Subtracts the two specified dates and returns the number of days between them.
        /// </summary>
        public static int operator -(Ethiopic12Date left, Ethiopic12Date right) =>
            left.CountDaysSince(right);

        /// <summary>
        /// Adds a number of days to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow either the capacity of
        /// <see cref="Int32"/> or the range of supported dates.</exception>
        public static Ethiopic12Date operator +(Ethiopic12Date value, int days) => value.PlusDays(days);

        /// <summary>
        /// Subtracts a number of days to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow either the capacity of
        /// <see cref="Int32"/> or the range of supported dates.</exception>
        public static Ethiopic12Date operator -(Ethiopic12Date value, int days) => value.PlusDays(-days);

        /// <summary>
        /// Adds one day to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the latest supported
        /// date.</exception>
        public static Ethiopic12Date operator ++(Ethiopic12Date value) => value.NextDay();

        /// <summary>
        /// Subtracts one day to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the earliest supported
        /// date.</exception>
        public static Ethiopic12Date operator --(Ethiopic12Date value) => value.PreviousDay();

#pragma warning restore CA2225

        /// <inheritdoc />
        [Pure]
        public int CountDaysSince(Ethiopic12Date other) =>
            // No need to use a checked context here.
            _daysSinceEpoch - other._daysSinceEpoch;

        /// <inheritdoc />
        [Pure]
        public Ethiopic12Date PlusDays(int days)
        {
            int daysSinceEpoch = checked(_daysSinceEpoch + days);
            s_Scope.DaysValidator.CheckOverflow(daysSinceEpoch);
            return new(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public Ethiopic12Date NextDay() =>
            this == s_MaxValue ? Throw.DateOverflow<Ethiopic12Date>() : new Ethiopic12Date(_daysSinceEpoch + 1);

        /// <inheritdoc />
        [Pure]
        public Ethiopic12Date PreviousDay() =>
            this == s_MinValue ? Throw.DateOverflow<Ethiopic12Date>() : new Ethiopic12Date(_daysSinceEpoch - 1);
    }
}
