// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    using System.Linq;

    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Hemerology;
    using Zorglub.Time.Hemerology.Scopes;

    // FIXME(api): prop Name (get and init) with default value = Key with the
    // ability to set it afterward.
    // Constructor throws "name" vs "key".

    #region Developer Notes

    // We use a CalendarScope, not a MinMaxYearScope to be able to use
    // ProlepticScope and StandardScope.
    //
    // Comparison between SimpleCalendar and ZCalendar.
    //
    // SimpleCalendar pros:
    // - Custom types for ordinal dates, calendar days, months and years.
    // - SimpleCalendar has fixed boundaries (Min/MaxYear are constants) which,
    //   performance-wise, can help.
    // - More capable arithmetic: PlusMonths(), PlusYears(), etc.
    // - Ability to define your own math operations.
    // - Simple binary serialization.
    // ZCalendar pros:
    // - ZCalendar accepts any type of schema.
    // - Min/MaxYear are not fixed.
    // - Not limited to 64 user-defined calendars.
    // Common features:
    // - Set of pre-defined calendars.
    // Common features to both types:
    // - System calendars have a permanent ID.
    // - User-defined calendars have a transient ID.
    // - GetSystemCalendar() uses a fast array lookup.
    // Missing features for both types:
    // - Custom methods specific to a calendar.

    #endregion

    /// <summary>
    /// Represents a calendar.
    /// <para>This class can ONLY be inherited from within friend assemblies.</para>
    /// </summary>
    public partial class ZCalendar :
        MinMaxYearCalendar,
        ICalendar<ZDate>,
        IDateFactory<ZDate>
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
            // methods here (infos, IDayProvider, ValidateGregorianParts()) but
            // also with counting methods in ZDate, and with  ZDateAdjusters.
            if (scope.IsComplete == false) Throw.Argument(nameof(scope));

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
        /// Returns a culture-independent string representation of the current instance.
        /// </summary>
        [Pure]
        public sealed override string ToString() => Key;
    }

    public partial class ZCalendar // Factories, conversions
    {
        /// <inheritdoc/>
        [Pure]
        public ZDate GetDate(DayNumber dayNumber)
        {
            Domain.Validate(dayNumber);
            return new ZDate(dayNumber - Epoch, Id);
        }

        /// <inheritdoc/>
        [Pure]
        public ZDate GetDate(int year, int month, int day)
        {
            Scope.ValidateYearMonthDay(year, month, day);
            int daysSinceEpoch = Schema.CountDaysSinceEpoch(year, month, day);
            return new ZDate(daysSinceEpoch, Id);
        }

        /// <inheritdoc/>
        [Pure]
        public ZDate GetDate(int year, int dayOfYear)
        {
            Scope.ValidateOrdinal(year, dayOfYear);
            int daysSinceEpoch = Schema.CountDaysSinceEpoch(year, dayOfYear);
            return new ZDate(daysSinceEpoch, Id);
        }

        /// <inheritdoc/>
        [Pure]
        public ZDate Today()
        {
            var today = DayNumber.Today();
            if (IsUserDefined) { Domain.Validate(today); }
            return new(today - Epoch, Id);
        }
    }

    public partial class ZCalendar // Dates in a given year or month
    {
        /// <inheritdoc/>
        [Pure]
        public IEnumerable<ZDate> GetDaysInYear(int year)
        {
            YearsValidator.Validate(year);

            int startOfYear = Schema.GetStartOfYear(year);
            int daysInYear = Schema.CountDaysInYear(year);

            return from daysSinceEpoch
                   in Enumerable.Range(startOfYear, daysInYear)
                   select new ZDate(daysSinceEpoch, Id);
        }

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<ZDate> GetDaysInMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);

            int startOfMonth = Schema.GetStartOfMonth(year, month);
            int daysInMonth = Schema.CountDaysInMonth(year, month);

            return from daysSinceEpoch
                   in Enumerable.Range(startOfMonth, daysInMonth)
                   select new ZDate(daysSinceEpoch, Id);
        }

        /// <inheritdoc/>
        [Pure]
        public ZDate GetStartOfYear(int year)
        {
            YearsValidator.Validate(year);
            int daysSinceEpoch = Schema.GetStartOfYear(year);
            return new ZDate(daysSinceEpoch, Id);
        }

        /// <inheritdoc/>
        [Pure]
        public ZDate GetEndOfYear(int year)
        {
            YearsValidator.Validate(year);
            int daysSinceEpoch = Schema.GetEndOfYear(year);
            return new ZDate(daysSinceEpoch, Id);
        }

        /// <inheritdoc/>
        [Pure]
        public ZDate GetStartOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            int daysSinceEpoch = Schema.GetStartOfMonth(year, month);
            return new ZDate(daysSinceEpoch, Id);
        }

        /// <inheritdoc/>
        [Pure]
        public ZDate GetEndOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            int daysSinceEpoch = Schema.GetEndOfMonth(year, month);
            return new ZDate(daysSinceEpoch, Id);
        }
    }
}
