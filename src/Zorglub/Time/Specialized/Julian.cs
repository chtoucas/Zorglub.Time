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
    using Zorglub.Time.Horology;

    // TODO(api): non-standard math. Providers. Idem with the other date types.
    // Use (enhance) JulianFormulae?
    //
    // Custom methods on calendars:
    // - IRegularFeaturette       -> idem, optimization using MonthsPerYear
    // - IVirtualMonthFeaturette  -> idem, optimization using a constant
    // Custom methods on dates:
    // - IBlankDayFeaturette      -> IBlankDay
    // - IEpagomenalDayFeaturette -> IEpagomenalDay
    // Other methods specific to a single schema. See WorldCalendar.
    //
    // Featurettes: only if C# supported "static abstract" methods, we could
    // define static properties instead of instance props.
    //
    // CountDaysSince(XXXDate other) checked context or not? do we test it?
    // Add method Adjust(Func<TDate, TDate>).
    //
    // Other date types? See CalendarZoo.

    /// <summary>
    /// Represents the Julian calendar.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class JulianCalendar :
        SpecialCalendar<JulianDate>,
        IRegularFeaturette
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JulianCalendar"/> class.
        /// </summary>
        public JulianCalendar() : this(new JulianSchema()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="JulianCalendar"/> class.
        /// </summary>
        internal JulianCalendar(JulianSchema schema)
            : base("Julian", MinMaxYearScope.CreateMaximal(schema, DayZero.OldStyle))
        {
            MonthsInYear = schema.MonthsInYear;
        }

        /// <inheritdoc/>
        public int MonthsInYear { get; }

        /// <inheritdoc/>
        [Pure]
        private protected sealed override JulianDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
    }

    /// <summary>
    /// Provides common adjusters for <see cref="JulianDate"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class JulianAdjuster : SpecialAdjuster<JulianDate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JulianAdjuster"/> class.
        /// </summary>
        public JulianAdjuster() : base(JulianDate.Calendar.Scope) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="JulianAdjuster"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        internal JulianAdjuster(MinMaxYearScope scope) : base(scope) { }

        /// <inheritdoc/>
        [Pure]
        private protected sealed override JulianDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
    }

    /// <summary>
    /// Represents a clock for the Julian calendar.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class JulianClock
    {
        /// <summary>
        /// Represents the clock.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly IClock _clock;

        /// <summary>
        /// Represents the epoch.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly DayNumber _epoch;

        /// <summary>
        /// Initializes a new instance of the <see cref="JulianClock"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="clock"/> is null.</exception>
        public JulianClock(IClock clock) : this(JulianDate.Calendar.Epoch, clock) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="JulianClock"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="clock"/> is null.</exception>
        private JulianClock(DayNumber epoch, IClock clock)
        {
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
            _epoch = epoch;
        }

        /// <summary>
        /// Gets an instance of the <see cref="JulianClock"/> class for the system clock using the
        /// current time zone setting on this machine.
        /// </summary>
        public static JulianClock Local { get; } = new(SystemClocks.Local);

        /// <summary>
        /// Gets an instance of the <see cref="JulianClock"/> class for the system clock using the UTC
        /// time zone.
        /// </summary>
        public static JulianClock Utc { get; } = new(SystemClocks.Utc);

        /// <summary>
        /// Obtains an instance of the <see cref="JulianClock"/> class for the specified clock.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="clock"/> is null.</exception>
        [Pure]
        public static JulianClock GetClock(IClock clock) => new(clock);

        /// <summary>
        /// Obtains a <see cref="JulianDate"/> value representing the current date.
        /// </summary>
        [Pure]
        public JulianDate GetCurrentDate() => new(_clock.Today() - _epoch);
    }

    /// <summary>
    /// Represents the Julian date.
    /// <para><see cref="JulianDate"/> is an immutable struct.</para>
    /// </summary>
    public readonly partial struct JulianDate :
        IDate<JulianDate, JulianCalendar>,
        IAdjustable<JulianDate>
    {
        // NB: the order in which the static fields are written is important.

        /// <summary>
        /// Represents the schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly JulianSchema s_Schema = new();

        /// <summary>
        /// Represents the calendar.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly JulianCalendar s_Calendar = new(s_Schema);

        /// <summary>
        /// Represents the scope.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly MinMaxYearScope s_Scope = s_Calendar.Scope;

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
        /// Represents the date adjuster.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly JulianAdjuster s_Adjuster = new(s_Scope);

        /// <summary>
        /// Represents the smallest possible value of a <see cref="JulianDate"/>.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly JulianDate s_MinValue = new(s_Domain.Min - s_Epoch);

        /// <summary>
        /// Represents the largest possible value of a <see cref="JulianDate"/>.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly JulianDate s_MaxValue = new(s_Domain.Max - s_Epoch);

        /// <summary>
        /// Represents the count of days since the Julian epoch.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _daysSinceEpoch;

        /// <summary>
        /// Initializes a new instance of the <see cref="JulianDate"/> struct to the specified
        /// date parts.
        /// </summary>
        /// <exception cref="AoorException">The specified components do not form a valid date or
        /// <paramref name="year"/> is outside the range of years supported by
        /// <see cref="JulianCalendar"/>.</exception>
        public JulianDate(int year, int month, int day)
        {
            s_Scope.ValidateYearMonthDay(year, month, day);

            _daysSinceEpoch = s_Schema.CountDaysSinceEpoch(year, month, day);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JulianDate"/> struct to the specified
        /// ordinal date parts.
        /// </summary>
        /// <exception cref="AoorException">The specified components do not form a valid ordinal
        /// date or <paramref name="year"/> is outside the range of years supported by
        /// <see cref="JulianCalendar"/>.</exception>
        public JulianDate(int year, int dayOfYear)
        {
            s_Scope.ValidateOrdinal(year, dayOfYear);

            _daysSinceEpoch = s_Schema.CountDaysSinceEpoch(year, dayOfYear);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JulianDate"/> struct.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// supported values.</exception>
        public JulianDate(DayNumber dayNumber)
        {
            s_Domain.Validate(dayNumber);

            _daysSinceEpoch = dayNumber - s_Epoch;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JulianDate"/> struct.
        /// <para>This method does NOT validate its parameter.</para>
        /// </summary>
        internal JulianDate(int daysSinceEpoch)
        {
            _daysSinceEpoch = daysSinceEpoch;
        }

        /// <summary>
        /// Gets the smallest possible value of a <see cref="JulianDate"/>.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static JulianDate MinValue => s_MinValue;

        /// <summary>
        /// Gets the largest possible value of a <see cref="JulianDate"/>.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static JulianDate MaxValue => s_MaxValue;

        /// <summary>
        /// Gets the date adjuster.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static JulianAdjuster Adjuster => s_Adjuster;

        /// <inheritdoc />
        public static JulianCalendar Calendar => s_Calendar;

        /// <inheritdoc />
        public DayNumber DayNumber => s_Epoch + _daysSinceEpoch;

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
        public bool IsSupplementary => false;

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

    public partial struct JulianDate // Conversions, adjustments...
    {
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
        #region Adjustments

        /// <inheritdoc />
        /// <remarks>See also <seealso cref="Adjuster"/>.</remarks>
        [Pure]
        public JulianDate Adjust(Func<JulianDate, JulianDate> adjuster)
        {
            Requires.NotNull(adjuster);

            return adjuster.Invoke(this);
        }

        //
        // Adjust the day of the week
        //

        /// <inheritdoc />
        [Pure]
        public JulianDate Previous(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.Previous(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new JulianDate(dayNumber - s_Epoch);
        }

        /// <inheritdoc />
        [Pure]
        public JulianDate PreviousOrSame(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new JulianDate(dayNumber - s_Epoch);
        }

        /// <inheritdoc />
        [Pure]
        public JulianDate Nearest(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.Nearest(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new JulianDate(dayNumber - s_Epoch);
        }

        /// <inheritdoc />
        [Pure]
        public JulianDate NextOrSame(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.NextOrSame(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new JulianDate(dayNumber - s_Epoch);
        }

        /// <inheritdoc />
        [Pure]
        public JulianDate Next(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.Next(dayOfWeek);
            if (s_Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new JulianDate(dayNumber - s_Epoch);
        }

        #endregion
    }

    public partial struct JulianDate // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="JulianDate"/> are equal.
        /// </summary>
        public static bool operator ==(JulianDate left, JulianDate right) =>
            left._daysSinceEpoch == right._daysSinceEpoch;

        /// <summary>
        /// Determines whether two specified instances of <see cref="JulianDate"/> are not equal.
        /// </summary>
        public static bool operator !=(JulianDate left, JulianDate right) =>
            left._daysSinceEpoch != right._daysSinceEpoch;

        /// <inheritdoc />
        [Pure]
        public bool Equals(JulianDate other) => _daysSinceEpoch == other._daysSinceEpoch;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is JulianDate date && Equals(date);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => _daysSinceEpoch;
    }

    public partial struct JulianDate // IComparable
    {
        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly earlier than the
        /// right one.
        /// </summary>
        public static bool operator <(JulianDate left, JulianDate right) =>
            left._daysSinceEpoch < right._daysSinceEpoch;

        /// <summary>
        /// Compares the two specified instances to see if the left one is earlier than or equal to
        /// the right one.
        /// </summary>
        public static bool operator <=(JulianDate left, JulianDate right) =>
            left._daysSinceEpoch <= right._daysSinceEpoch;

        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly later than the
        /// right one.
        /// </summary>
        public static bool operator >(JulianDate left, JulianDate right) =>
            left._daysSinceEpoch > right._daysSinceEpoch;

        /// <summary>
        /// Compares the two specified instances to see if the left one is later than or equal to
        /// the right one.
        /// </summary>
        public static bool operator >=(JulianDate left, JulianDate right) =>
            left._daysSinceEpoch >= right._daysSinceEpoch;

        /// <summary>
        /// Obtains the earlier date of two specified dates.
        /// </summary>
        [Pure]
        public static JulianDate Min(JulianDate x, JulianDate y) => x < y ? x : y;

        /// <summary>
        /// Obtains the later date of two specified dates.
        /// </summary>
        [Pure]
        public static JulianDate Max(JulianDate x, JulianDate y) => x > y ? x : y;

        /// <summary>
        /// Indicates whether this instance is earlier, later or the same as the specified one.
        /// </summary>
        [Pure]
        public int CompareTo(JulianDate other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

        /// <inheritdoc />
        [Pure]
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is JulianDate date ? CompareTo(date)
            : Throw.NonComparable(typeof(JulianDate), obj);
    }

    public partial struct JulianDate // Math ops
    {
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) ✓
        // Friendly alternates do exist but use domain-specific names.

        /// <summary>
        /// Subtracts the two specified dates and returns the number of days between them.
        /// </summary>
        public static int operator -(JulianDate left, JulianDate right) =>
            left.CountDaysSince(right);

        /// <summary>
        /// Adds a number of days to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow either the capacity of
        /// <see cref="Int32"/> or the range of supported dates.</exception>
        public static JulianDate operator +(JulianDate value, int days) => value.PlusDays(days);

        /// <summary>
        /// Subtracts a number of days to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow either the capacity of
        /// <see cref="Int32"/> or the range of supported dates.</exception>
        public static JulianDate operator -(JulianDate value, int days) => value.PlusDays(-days);

        /// <summary>
        /// Adds one day to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the latest supported
        /// date.</exception>
        public static JulianDate operator ++(JulianDate value) => value.NextDay();

        /// <summary>
        /// Subtracts one day to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the earliest supported
        /// date.</exception>
        public static JulianDate operator --(JulianDate value) => value.PreviousDay();

#pragma warning restore CA2225

        /// <inheritdoc />
        [Pure]
        public int CountDaysSince(JulianDate other) =>
            // No need to use a checked context here.
            _daysSinceEpoch - other._daysSinceEpoch;

        /// <inheritdoc />
        [Pure]
        public JulianDate PlusDays(int days)
        {
            int daysSinceEpoch = checked(_daysSinceEpoch + days);
            s_Scope.DaysValidator.CheckOverflow(daysSinceEpoch);
            return new(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public JulianDate NextDay() =>
            this == s_MaxValue ? Throw.DateOverflow<JulianDate>() : new JulianDate(_daysSinceEpoch + 1);

        /// <inheritdoc />
        [Pure]
        public JulianDate PreviousDay() =>
            this == s_MinValue ? Throw.DateOverflow<JulianDate>() : new JulianDate(_daysSinceEpoch - 1);
    }
}
