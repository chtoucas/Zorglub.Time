// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Schemas;
    using Zorglub.Time.Hemerology.Scopes;

    // TODO(api): advanced math, we must finish CalendricalMath first.

    public readonly partial struct CivilDay :
        IDate<CivilDay>,
        IYearEndpointsProvider<CivilDay>,
        IMonthEndpointsProvider<CivilDay>,
        IMinMaxValue<CivilDay>,
        ISubtractionOperators<CivilDay, int, CivilDay>
    {
        /// <summary>
        /// Represents the earliest supported year.
        /// <para>This field is a constant equal to 1.</para>
        /// </summary>
        public const int MinYear = StandardShortScope.MinYear;

        /// <summary>
        /// Represents the latest supported year.
        /// <para>This field is a constant equal to 9999.</para>
        /// </summary>
        public const int MaxYear = StandardShortScope.MaxYear;

        /// <summary>
        /// Represents the count of days since the Gregorian epoch.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _daysSinceEpoch;

        /// <summary>
        /// Initializes a new instance of the <see cref="CivilDay"/> struct to the specified
        /// date parts.
        /// </summary>
        public CivilDay(int year, int month, int day)
        {
            GregorianStandardShortScope.ValidateYearMonthDayImpl(year, month, day);

            _daysSinceEpoch = GregorianFormulaeAfterYear0.CountDaysSinceEpoch(year, month, day);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CivilDay"/> struct.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// supported values.</exception>
        public CivilDay(DayNumber dayNumber)
        {
            Domain.Validate(dayNumber);

            _daysSinceEpoch = dayNumber - Epoch;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CivilDay"/> struct.
        /// <para>This method does NOT validate its parameter.</para>
        /// </summary>
        private CivilDay(int daysSinceEpoch)
        {
            _daysSinceEpoch = daysSinceEpoch;
        }

        /// <summary>
        /// Gets the epoch.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static DayNumber Epoch { get; } = DayZero.NewStyle;

        /// <summary>
        /// Gets the domain, the interval of supported <see cref="DayNumber"/>.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static Range<DayNumber> Domain { get; } = GregorianStandardShortScope.DefaultDomain;

        /// <summary>
        /// Gets the smallest possible value of a <see cref="CivilDay"/>.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static CivilDay MinValue { get; } = new(Domain.Min - Epoch);

        /// <summary>
        /// Gets the largest possible value of a <see cref="CivilDay"/>.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static CivilDay MaxValue { get; } = new(Domain.Max - Epoch);

        /// <summary>
        /// Gets the Gregorian schema.
        /// </summary>
        private static readonly GregorianSchema s_Schema = new();

        /// <summary>
        /// Gets the day number.
        /// </summary>
        public DayNumber DayNumber => Epoch + _daysSinceEpoch;

        /// <inheritdoc />
        public Ord CenturyOfEra => Ord.FromInt32(Century);

        /// <inheritdoc />
        public int Century => YearNumbering.GetCentury(Year);

        /// <inheritdoc />
        public Ord YearOfEra => Ord.FromInt32(Year);

        /// <inheritdoc />
        public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);

        /// <inheritdoc />
        public int Year => GregorianFormulaeAfterYear0.GetYear(_daysSinceEpoch);

        /// <inheritdoc />
        public int Month
        {
            get
            {
                GregorianFormulaeAfterYear0.GetDateParts(_daysSinceEpoch, out _, out int m, out _);
                return m;
            }
        }

        /// <inheritdoc />
        public int DayOfYear
        {
            get
            {
                _ = GregorianFormulaeAfterYear0.GetYear(_daysSinceEpoch, out int doy);
                return doy;
            }
        }

        /// <inheritdoc />
        public int Day
        {
            get
            {
                GregorianFormulaeAfterYear0.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
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
                GregorianFormulaeAfterYear0.GetDateParts(_daysSinceEpoch, out _, out int m, out int d);
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
            GregorianFormulaeAfterYear0.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} (Gregorian)");
        }

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        public void Deconstruct(out int year, out int month, out int day) =>
            GregorianFormulaeAfterYear0.GetDateParts(_daysSinceEpoch, out year, out month, out day);
    }

    public partial struct CivilDay // Conversions, adjustments...
    {
        #region Factories

        /// <summary>
        /// Obtains the current date in the Gregorian calendar on this machine,
        /// expressed in local time, not UTC.
        /// </summary>
        [Pure]
        public static CivilDay Today() => new(DayNumber.Today() - Epoch);

        #endregion
        #region Conversions

        [Pure]
        static CivilDay IFixedDay<CivilDay>.FromDayNumber(DayNumber dayNumber) => new(dayNumber);

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

        /// <inheritdoc />
        [Pure]
        public static CivilDay GetStartOfYear(CivilDay day)
        {
            int daysSinceEpoch = GregorianFormulaeAfterYear0.GetStartOfYear(day.Year);
            return new CivilDay(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public static CivilDay GetEndOfYear(CivilDay day)
        {
            int daysSinceEpoch = s_Schema.GetEndOfYear(day.Year);
            return new CivilDay(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public static CivilDay GetStartOfMonth(CivilDay day)
        {
            GregorianFormulaeAfterYear0.GetDateParts(day._daysSinceEpoch, out int y, out int m, out _);
            int daysSinceEpoch = s_Schema.GetStartOfMonth(y, m);
            return new CivilDay(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public static CivilDay GetEndOfMonth(CivilDay day)
        {
            GregorianFormulaeAfterYear0.GetDateParts(day._daysSinceEpoch, out int y, out int m, out _);
            int daysSinceEpoch = s_Schema.GetEndOfMonth(y, m);
            return new CivilDay(daysSinceEpoch);
        }

        #endregion
        #region Adjust the day of the week

        /// <inheritdoc />
        [Pure]
        public CivilDay Previous(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.Previous(dayOfWeek);
            if (Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new CivilDay(dayNumber - Epoch);
        }

        /// <inheritdoc />
        [Pure]
        public CivilDay PreviousOrSame(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
            if (Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new CivilDay(dayNumber - Epoch);
        }

        /// <inheritdoc />
        [Pure]
        public CivilDay Nearest(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.Nearest(dayOfWeek);
            if (Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new CivilDay(dayNumber - Epoch);
        }

        /// <inheritdoc />
        [Pure]
        public CivilDay NextOrSame(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.NextOrSame(dayOfWeek);
            if (Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new CivilDay(dayNumber - Epoch);
        }

        /// <inheritdoc />
        [Pure]
        public CivilDay Next(DayOfWeek dayOfWeek)
        {
            var dayNumber = DayNumber.Next(dayOfWeek);
            if (Domain.Contains(dayNumber) == false) { Throw.DateOverflow(); }
            return new CivilDay(dayNumber - Epoch);
        }

        #endregion
    }

    public partial struct CivilDay // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="CivilDay"/> are equal.
        /// </summary>
        public static bool operator ==(CivilDay left, CivilDay right) =>
            left._daysSinceEpoch == right._daysSinceEpoch;

        /// <summary>
        /// Determines whether two specified instances of <see cref="CivilDay"/> are not equal.
        /// </summary>
        public static bool operator !=(CivilDay left, CivilDay right) =>
            left._daysSinceEpoch != right._daysSinceEpoch;

        /// <inheritdoc />
        [Pure]
        public bool Equals(CivilDay other) => _daysSinceEpoch == other._daysSinceEpoch;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is CivilDay date && Equals(date);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => _daysSinceEpoch;
    }

    public partial struct CivilDay // IComparable
    {
        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly earlier than the
        /// right one.
        /// </summary>
        public static bool operator <(CivilDay left, CivilDay right) =>
            left._daysSinceEpoch < right._daysSinceEpoch;

        /// <summary>
        /// Compares the two specified instances to see if the left one is earlier than or equal to
        /// the right one.
        /// </summary>
        public static bool operator <=(CivilDay left, CivilDay right) =>
            left._daysSinceEpoch <= right._daysSinceEpoch;

        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly later than the
        /// right one.
        /// </summary>
        public static bool operator >(CivilDay left, CivilDay right) =>
            left._daysSinceEpoch > right._daysSinceEpoch;

        /// <summary>
        /// Compares the two specified instances to see if the left one is later than or equal to
        /// the right one.
        /// </summary>
        public static bool operator >=(CivilDay left, CivilDay right) =>
            left._daysSinceEpoch >= right._daysSinceEpoch;

        /// <summary>
        /// Obtains the earlier date of two specified dates.
        /// </summary>
        [Pure]
        public static CivilDay Min(CivilDay x, CivilDay y) => x < y ? x : y;

        /// <summary>
        /// Obtains the later date of two specified dates.
        /// </summary>
        [Pure]
        public static CivilDay Max(CivilDay x, CivilDay y) => x > y ? x : y;

        /// <summary>
        /// Indicates whether this instance is earlier, later or the same as the specified one.
        /// </summary>
        [Pure]
        public int CompareTo(CivilDay other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

        /// <inheritdoc />
        [Pure]
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is CivilDay date ? CompareTo(date)
            : Throw.NonComparable(typeof(CivilDay), obj);
    }

    public partial struct CivilDay // Math ops
    {
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage)

        /// <summary>
        /// Subtracts the two specified dates and returns the number of days between them.
        /// </summary>
        public static int operator -(CivilDay left, CivilDay right) => left.CountDaysSince(right);

        /// <summary>
        /// Adds a number of days to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow either the capacity of
        /// <see cref="Int32"/> or the range of supported dates.</exception>
        public static CivilDay operator +(CivilDay value, int days) => value.PlusDays(days);

        /// <summary>
        /// Subtracts a number of days to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow either the capacity of
        /// <see cref="Int32"/> or the range of supported dates.</exception>
        public static CivilDay operator -(CivilDay value, int days) => value.PlusDays(-days);

        /// <summary>
        /// Adds one day to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the latest supported
        /// date.</exception>
        public static CivilDay operator ++(CivilDay value) => value.NextDay();

        /// <summary>
        /// Subtracts one day to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the earliest supported
        /// date.</exception>
        public static CivilDay operator --(CivilDay value) => value.PreviousDay();

#pragma warning restore CA2225

        /// <inheritdoc />
        [Pure]
        public int CountDaysSince(CivilDay other) =>
            // No need to use a checked context here.
            _daysSinceEpoch - other._daysSinceEpoch;

        /// <inheritdoc />
        [Pure]
        public CivilDay PlusDays(int days)
        {
            int daysSinceEpoch = checked(_daysSinceEpoch + days);
            GregorianStandardShortScope.CheckOverflow(daysSinceEpoch);
            return new(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public CivilDay NextDay() =>
            this == MaxValue ? Throw.DateOverflow<CivilDay>() : new CivilDay(_daysSinceEpoch + 1);

        /// <inheritdoc />
        [Pure]
        public CivilDay PreviousDay() =>
            this == MinValue ? Throw.DateOverflow<CivilDay>() : new CivilDay(_daysSinceEpoch - 1);
    }
}
