// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using System.Linq;

    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Schemas;
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Hemerology.Scopes;

    using static Zorglub.Time.Core.CalendricalConstants;

    // REVIEW(api): prop Name (get and init) with default value = Key.
    // Instead of a maximal range of days, use a custom range of days? Hum,
    // I'm not sure that it's possible here (see CalendarCatalog).

    #region Developer Notes

    // Comparison between Calendar and WideCalendar.
    //
    // Calendar pros:
    // - Custom types for ordinal dates, calendar days, months and years.
    // - More capable arithmetic: PlusMonths(), PlusYears(), etc.
    // - Ability to define your own arithmetical operations.
    // - Custom methods specific to a calendar.
    // - Calendar has fixed boundaries (Min/MaxYear are constants) which,
    //   performance-wise, can help.
    // - A Calendar has a permanent ID whereas a WideCalendar has a transient ID.
    // - Simple binary serialization.
    // - CalendarCatalog.GetSystemCalendar() uses a fast array lookup.
    // WideCalendar pros:
    // - WideCalendar accepts any type of schema.
    // Common features:
    // - Set of pre-defined calendars.

    #endregion

    /// <summary>
    /// Represents a wide calendar.
    /// </summary>
    public partial class WideCalendar : ICalendar<WideDate>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="WideCalendar"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        internal WideCalendar(
            int id,
            string key,
            ICalendricalSchema schema,
            DayNumber epoch,
            bool widest,
            bool userDefined)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Schema = schema ?? throw new ArgumentNullException(nameof(schema));

            Id = id;
            IsUserDefined = userDefined;

            var scope = MinMaxYearScope.WithMaximalRange(schema, epoch, onOrAfterEpoch: !widest);

            Scope = scope;
            Epoch = scope.Epoch;
            SupportedYears = scope.Segment.SupportedYears.Range;
            Domain = scope.Domain;

            MinMaxDate = from dayNumber in scope.Domain.Endpoints select new WideDate(dayNumber - Epoch, id);
        }

        #region System calendars

        /// <summary>
        /// Gets the proleptic Gregorian calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static WideCalendar Gregorian => WideCatalog.Gregorian;

        /// <summary>
        /// Gets the proleptic Julian calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static WideCalendar Julian => WideJulianCalendar.Instance;

        /// <summary>
        /// Gets the Armenian calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static WideCalendar Armenian => WideArmenianCalendar.Instance;

        /// <summary>
        /// Gets the Coptic calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static WideCalendar Coptic => WideCopticCalendar.Instance;

        /// <summary>
        /// Gets the Ethiopic calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static WideCalendar Ethiopic => WideEthiopicCalendar.Instance;

        /// <summary>
        /// Gets the Tabular Islamic calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static WideCalendar TabularIslamic => WideTabularIslamicCalendar.Instance;

        /// <summary>
        /// Gets the Zoroastrian calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static WideCalendar Zoroastrian => WideZoroastrianCalendar.Instance;

        #endregion

        /// <summary>
        /// Gets the unique key of the current instance.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Returns true if the current instance is user-defined; otherwise
        /// returns false.
        /// </summary>
        public bool IsUserDefined { get; }

        /// <summary>
        /// Gets the pair of earliest and latest supported <see cref="WideDate"/>.
        /// </summary>
        public OrderedPair<WideDate> MinMaxDate { get; }

        /// <inheritdoc />
        public DayNumber Epoch { get; }

        /// <inheritdoc />
        public CalendricalAlgorithm Algorithm => Schema.Algorithm;

        /// <inheritdoc />
        public CalendricalFamily Family => Schema.Family;

        /// <inheritdoc />
        public CalendricalAdjustments PeriodicAdjustments => Schema.PeriodicAdjustments;

        /// <inheritdoc />
        public Range<int> SupportedYears { get; }

        /// <inheritdoc />
        public Range<DayNumber> Domain { get; }

        /// <inheritdoc />
        public CalendarScope Scope { get; }

        /// <summary>
        /// Gets the underlying schema.
        /// </summary>
        protected internal ICalendricalSchema Schema { get; }

        /// <summary>
        /// Gets the ID of the current instance.
        /// </summary>
        internal int Id { get; }

        /// <summary>
        /// Returns a string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString() => Key;

        /// <inheritdoc />
        [Pure]
        public bool IsRegular(out int monthsInYear) => Schema.IsRegular(out monthsInYear);
    }

    public partial class WideCalendar // Year, month or day infos
    {
#pragma warning disable CA1725 // Parameter names should match base declaration (Naming)

        /// <inheritdoc />
        /// <exception cref="AoorException">The year is outside the range of supported years.
        /// </exception>
        [Pure]
        public bool IsLeapYear(int year)
        {
            Scope.ValidateYear(year);
            return Schema.IsLeapYear(year);
        }

        /// <inheritdoc />
        /// <exception cref="AoorException">The month is either invalid or outside the range of
        /// supported months.</exception>
        [Pure]
        public bool IsIntercalaryMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return Schema.IsIntercalaryMonth(year, month);
        }

        /// <inheritdoc />
        /// <exception cref="AoorException">The date is either invalid or outside the range of
        /// supported dates.</exception>
        [Pure]
        public bool IsIntercalaryDay(int year, int month, int day)
        {
            Scope.ValidateYearMonthDay(year, month, day);
            return Schema.IsIntercalaryDay(year, month, day);
        }

        /// <inheritdoc />
        /// <exception cref="AoorException">The date is either invalid or outside the range of
        /// supported dates.</exception>
        [Pure]
        public bool IsSupplementaryDay(int year, int month, int day)
        {
            Scope.ValidateYearMonthDay(year, month, day);
            return Schema.IsSupplementaryDay(year, month, day);
        }

        /// <inheritdoc />
        /// <exception cref="AoorException">The year is outside the range of supported years.
        /// </exception>
        [Pure]
        public int CountMonthsInYear(int year)
        {
            Scope.ValidateYear(year);
            return Schema.CountMonthsInYear(year);
        }

        /// <inheritdoc />
        /// <exception cref="AoorException">The year is outside the range of supported years.
        /// </exception>
        [Pure]
        public int CountDaysInYear(int year)
        {
            Scope.ValidateYear(year);
            return Schema.CountDaysInYear(year);
        }

        /// <inheritdoc />
        /// <exception cref="AoorException">The month is either invalid or outside the range of
        /// supported months.</exception>
        [Pure]
        public int CountDaysInMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return Schema.CountDaysInMonth(year, month);
        }

#pragma warning restore CA1725 // Parameter names should match base declaration
    }

    public partial class WideCalendar // Factories, conversions
    {
        /// <summary>
        /// Creates a new instance of the <see cref="WideDate"/> struct.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// values supported by this calendar.</exception>
        [Pure]
        public WideDate GetDate(DayNumber dayNumber)
        {
            Domain.Validate(dayNumber);
            return new WideDate(dayNumber - Epoch, Id);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="WideDate"/> struct.
        /// </summary>
        /// <exception cref="AoorException">The date is either invalid or outside the range of
        /// supported dates.</exception>
        [Pure]
        public WideDate GetDate(int year, int month, int day)
        {
            Scope.ValidateYearMonthDay(year, month, day);
            int daysSinceEpoch = Schema.CountDaysSinceEpoch(year, month, day);
            return new WideDate(daysSinceEpoch, Id);
        }

        /// <summary>
        /// Converts the specified ordinal date to a date object.
        /// </summary>
        /// <exception cref="AoorException">The ordinal date is either invalid or outside the range
        /// of supported dates.</exception>
        [Pure]
        public WideDate GetDate(int year, int dayOfYear)
        {
            Scope.ValidateOrdinal(year, dayOfYear);
            int daysSinceEpoch = Schema.CountDaysSinceEpoch(year, dayOfYear);
            return new WideDate(daysSinceEpoch, Id);
        }

        /// <inheritdoc />
        [Pure]
        public WideDate Today() => GetDate(DayNumber.Today());

        /// <inheritdoc />
        [Pure]
        public DayNumber GetDayNumberOn(int year, int month, int day)
        {
            Scope.ValidateYearMonthDay(year, month, day);
            return Epoch + Schema.CountDaysSinceEpoch(year, month, day);
        }

        /// <inheritdoc />
        [Pure]
        public DayNumber GetDayNumberOn(int year, int dayOfYear)
        {
            Scope.ValidateOrdinal(year, dayOfYear);
            return Epoch + Schema.CountDaysSinceEpoch(year, dayOfYear);
        }
    }

    public partial class WideCalendar // Dates in a given year or month
    {
        /// <inheritdoc />
        [Pure]
        public IEnumerable<WideDate> GetDaysInYear(int year)
        {
            // Check arg eagerly.
            Scope.ValidateYear(year);

            return Iterator();

            IEnumerable<WideDate> Iterator()
            {
                int cuid = Id;
                var sch = Schema;
                int startOfYear = sch.GetStartOfYear(year);
                int daysInYear = sch.CountDaysInYear(year);

                return from daysSinceEpoch
                       in Enumerable.Range(startOfYear, daysInYear)
                       select new WideDate(daysSinceEpoch, cuid);
            }
        }

        /// <inheritdoc />
        [Pure]
        public IEnumerable<WideDate> GetDaysInMonth(int year, int month)
        {
            // Check arg eagerly.
            Scope.ValidateYearMonth(year, month);

            return Iterator();

            IEnumerable<WideDate> Iterator()
            {
                var cuid = Id;
                var sch = Schema;
                int startOfMonth = sch.GetStartOfMonth(year, month);
                int daysInMonth = sch.CountDaysInMonth(year, month);

                return from daysSinceEpoch
                       in Enumerable.Range(startOfMonth, daysInMonth)
                       select new WideDate(daysSinceEpoch, cuid);
            }
        }

        /// <inheritdoc />
        [Pure]
        public WideDate GetStartOfYear(int year)
        {
            Scope.ValidateYear(year);
            int daysSinceEpoch = Schema.GetStartOfYear(year);
            return new WideDate(daysSinceEpoch, Id);
        }

        /// <inheritdoc />
        [Pure]
        public WideDate GetEndOfYear(int year)
        {
            Scope.ValidateYear(year);
            int daysSinceEpoch = Schema.GetEndOfYear(year);
            return new WideDate(daysSinceEpoch, Id);
        }

        /// <inheritdoc />
        [Pure]
        public WideDate GetStartOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            int daysSinceEpoch = Schema.GetStartOfMonth(year, month);
            return new WideDate(daysSinceEpoch, Id);
        }

        /// <inheritdoc />
        [Pure]
        public WideDate GetEndOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            int daysSinceEpoch = Schema.GetEndOfMonth(year, month);
            return new WideDate(daysSinceEpoch, Id);
        }
    }

    public partial class WideCalendar // Internal helpers
    {
        /// <summary>
        /// Validates the specified Gregorian date.
        /// </summary>
        /// <exception cref="AoorException">The validation failed.</exception>
        internal static void ValidateGregorianParts(int year, int month, int day, string? paramName = null)
        {
            if (year < GregorianSchema.MinYear || year > GregorianSchema.MaxYear)
            {
                Throw.YearOutOfRange(year, paramName);
            }
            if (month < 1 || month > Solar12.MonthsInYear)
            {
                Throw.MonthOutOfRange(month, paramName);
            }
            if (day < 1
                || (day > Solar.MinDaysInMonth
                    && day > GregorianFormulae.CountDaysInMonth(year, month)))
            {
                Throw.DayOutOfRange(day, paramName);
            }
        }
    }
}
