// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using System.Linq;

    using Zorglub.Time.Core.Schemas;
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Hemerology.Scopes;

    using static Zorglub.Time.Core.CalendricalConstants;

    // REVIEW(api): prop Name (get and init) with default value = Key with the
    // ability to set it afterward.

    #region Developer Notes

    // We use a CalendarScope, not a MinMaxYearScope to be able to use
    // ProlepticScope and StandardScope.
    //
    // Comparison between Calendar and ZCalendar.
    //
    // Calendar pros:
    // - Custom types for ordinal dates, calendar days, months and years.
    // - More capable arithmetic: PlusMonths(), PlusYears(), etc.
    // - Ability to define your own math operations.
    // - Custom methods specific to a calendar.
    // - Calendar has fixed boundaries (Min/MaxYear are constants) which,
    //   performance-wise, can help.
    // - A Calendar has a permanent ID whereas a ZCalendar has a transient ID.
    // - Simple binary serialization.
    // - CalendarCatalog.GetSystemCalendar() uses a fast array lookup.
    // ZCalendar pros:
    // - ZCalendar accepts any type of schema.
    // Common features:
    // - Set of pre-defined calendars.

    #endregion

    /// <summary>
    /// Represents a calendar.
    /// </summary>
    public partial class ZCalendar : BasicCalendar<CalendarScope>, ICalendar<ZDate>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ZCalendar"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="scope"/> is NOT complete.</exception>
        internal ZCalendar(int id, string key, CalendarScope scope, bool userDefined)
            : base(key, scope)
        {
            Debug.Assert(key != null);
            Debug.Assert(scope != null);

            // The scope MUST be complete, otherwise we have a problem with
            // methods here (infos, IDayProvider, ValidateGregorianParts())n but
            // also with counting methods in ZDate, and with  ZDateAdjusters.
            if (scope.Segment.IsComplete == false) Throw.Argument(nameof(scope));

            Key = key;
            Id = id;
            IsUserDefined = userDefined;

            MinMaxDate = from dayNumber in scope.Domain.Endpoints
                         select new ZDate(dayNumber - Epoch, id);
        }

        #region System calendars

        /// <summary>
        /// Gets the proleptic Gregorian calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static ZCalendar Gregorian => ZCatalog.Gregorian;

        /// <summary>
        /// Gets the proleptic Julian calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static ZCalendar Julian => JulianZCalendar.Instance;

        /// <summary>
        /// Gets the Armenian calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static ZCalendar Armenian => ArmenianZCalendar.Instance;

        /// <summary>
        /// Gets the Coptic calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static ZCalendar Coptic => CopticZCalendar.Instance;

        /// <summary>
        /// Gets the Ethiopic calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static ZCalendar Ethiopic => EthiopicZCalendar.Instance;

        /// <summary>
        /// Gets the Tabular Islamic calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static ZCalendar TabularIslamic => TabularIslamicZCalendar.Instance;

        /// <summary>
        /// Gets the Zoroastrian calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static ZCalendar Zoroastrian => ZoroastrianZCalendar.Instance;

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
        /// Gets the pair of earliest and latest supported <see cref="ZDate"/>.
        /// </summary>
        public OrderedPair<ZDate> MinMaxDate { get; }

        /// <summary>
        /// Gets the ID of the current instance.
        /// </summary>
        internal int Id { get; }

        /// <summary>
        /// Returns a string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString() => Key;
    }

    public partial class ZCalendar // Year, month or day infos
    {
        /// <inheritdoc />
        /// <exception cref="AoorException">The year is outside the range of supported years.
        /// </exception>
        [Pure]
        public sealed override int CountMonthsInYear(int year)
        {
            SupportedYears.Validate(year);
            return Schema.CountMonthsInYear(year);
        }

        /// <inheritdoc />
        /// <exception cref="AoorException">The year is outside the range of supported years.
        /// </exception>
        [Pure]
        public sealed override int CountDaysInYear(int year)
        {
            SupportedYears.Validate(year);
            return Schema.CountDaysInYear(year);
        }

        /// <inheritdoc />
        /// <exception cref="AoorException">The month is either invalid or outside the range of
        /// supported months.</exception>
        [Pure]
        public sealed override int CountDaysInMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return Schema.CountDaysInMonth(year, month);
        }
    }

    public partial class ZCalendar // Factories, conversions
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ZDate"/> struct.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// values supported by this calendar.</exception>
        [Pure]
        public ZDate GetDate(DayNumber dayNumber)
        {
            Domain.Validate(dayNumber);
            return new ZDate(dayNumber - Epoch, Id);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ZDate"/> struct.
        /// </summary>
        /// <exception cref="AoorException">The date is either invalid or outside the range of
        /// supported dates.</exception>
        [Pure]
        public ZDate GetDate(int year, int month, int day)
        {
            Scope.ValidateYearMonthDay(year, month, day);
            int daysSinceEpoch = Schema.CountDaysSinceEpoch(year, month, day);
            return new ZDate(daysSinceEpoch, Id);
        }

        /// <summary>
        /// Converts the specified ordinal date to a date object.
        /// </summary>
        /// <exception cref="AoorException">The ordinal date is either invalid or outside the range
        /// of supported dates.</exception>
        [Pure]
        public ZDate GetDate(int year, int dayOfYear)
        {
            Scope.ValidateOrdinal(year, dayOfYear);
            int daysSinceEpoch = Schema.CountDaysSinceEpoch(year, dayOfYear);
            return new ZDate(daysSinceEpoch, Id);
        }

        /// <inheritdoc />
        [Pure]
        public ZDate Today() => GetDate(DayNumber.Today());
    }

    public partial class ZCalendar // IDayProvider
    {
        /// <inheritdoc />
        [Pure]
        public IEnumerable<ZDate> GetDaysInYear(int year)
        {
            // Check arg eagerly.
            SupportedYears.Validate(year);

            return Iterator();

            IEnumerable<ZDate> Iterator()
            {
                int cuid = Id;
                var sch = Schema;
                int startOfYear = sch.GetStartOfYear(year);
                int daysInYear = sch.CountDaysInYear(year);

                return from daysSinceEpoch
                       in Enumerable.Range(startOfYear, daysInYear)
                       select new ZDate(daysSinceEpoch, cuid);
            }
        }

        /// <inheritdoc />
        [Pure]
        public IEnumerable<ZDate> GetDaysInMonth(int year, int month)
        {
            // Check arg eagerly.
            Scope.ValidateYearMonth(year, month);

            return Iterator();

            IEnumerable<ZDate> Iterator()
            {
                var cuid = Id;
                var sch = Schema;
                int startOfMonth = sch.GetStartOfMonth(year, month);
                int daysInMonth = sch.CountDaysInMonth(year, month);

                return from daysSinceEpoch
                       in Enumerable.Range(startOfMonth, daysInMonth)
                       select new ZDate(daysSinceEpoch, cuid);
            }
        }

        /// <inheritdoc />
        [Pure]
        public ZDate GetStartOfYear(int year)
        {
            SupportedYears.Validate(year);
            int daysSinceEpoch = Schema.GetStartOfYear(year);
            return new ZDate(daysSinceEpoch, Id);
        }

        /// <inheritdoc />
        [Pure]
        public ZDate GetEndOfYear(int year)
        {
            SupportedYears.Validate(year);
            int daysSinceEpoch = Schema.GetEndOfYear(year);
            return new ZDate(daysSinceEpoch, Id);
        }

        /// <inheritdoc />
        [Pure]
        public ZDate GetStartOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            int daysSinceEpoch = Schema.GetStartOfMonth(year, month);
            return new ZDate(daysSinceEpoch, Id);
        }

        /// <inheritdoc />
        [Pure]
        public ZDate GetEndOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            int daysSinceEpoch = Schema.GetEndOfMonth(year, month);
            return new ZDate(daysSinceEpoch, Id);
        }
    }

    public partial class ZCalendar // Internal helpers
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
