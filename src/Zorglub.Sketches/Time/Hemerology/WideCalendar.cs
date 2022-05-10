// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Schemas;

    using static Zorglub.Time.Core.CalendricalConstants;

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
    public partial class WideCalendar : BasicCalendar, ICalendar<WideDate>
    {
        /// <summary>
        /// Represents the integer value of the day of the week of the epoch of the calendar.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _epochDayOfWeek;

        /// <summary>
        /// Initializes a new instance of <see cref="WideCalendar"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        internal WideCalendar(
            int id,
            string name,
            ICalendricalSchema schema,
            DayNumber epoch,
            bool widest,
            bool userDefined)
            : this(
                  id,
                  name,
                  schema,
                  // NB: MinMaxYearScope ensures years outside the range defined
                  // by Yemoda are not allowed.
                  MinMaxYearScope.WithMaximalRange(schema, epoch, widest),
                  userDefined)
        { }

        private WideCalendar(
            int id,
            string name!!,
            ICalendricalSchema schema,
            MinMaxYearScope scope,
            bool userDefined)
            : base(schema, scope)
        {
            Debug.Assert(schema != null);

            Name = name;

            Id = id;
            IsUserDefined = userDefined;

            _epochDayOfWeek = (int)Epoch.DayOfWeek;

            DayProvider = new MinMaxYearDayProvider(scope);

            Arithmetic = schema.Arithmetic;
            YearOverflowChecker = scope.YearOverflowChecker;

            MinMaxDate = from parts in scope.MinMaxDateParts select new WideDate(parts, id);
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
        /// Gets the name of the current instance.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the unique key of the current instance.
        /// </summary>
        /// <exception cref="NotSupportedException">The current instance is
        /// transient.</exception>
        public string Key => IsVolatile ? Throw.NotSupported<string>() : Name;

        /// <summary>
        /// Returns true if the current instance is user-defined; otherwise
        /// returns false.
        /// </summary>
        public bool IsUserDefined { get; }

        /// <summary>
        /// Returns true if the current instance is volatile; otherwise returns
        /// false.
        /// </summary>
        public bool IsVolatile { get; protected init; }

        /// <summary>
        /// Gets the pair of earliest and latest supported <see cref="WideDate"/>.
        /// </summary>
        public OrderedPair<WideDate> MinMaxDate { get; }

        /// <summary>
        /// Gets a provider for day numbers in a year or a month.
        /// </summary>
        public IDayProvider<DayNumber> DayProvider { get; }

        /// <summary>
        /// Gets the ID of the current instance.
        /// </summary>
        internal int Id { get; }

        /// <summary>
        /// Gets or sets the underlying arithmetic engine.
        /// </summary>
        internal ICalendricalArithmetic Arithmetic { get; }

        /// <summary>
        /// Gets the checker for overflows of the range of years.
        /// </summary>
        internal IOverflowChecker<int> YearOverflowChecker { get; }

        /// <summary>
        /// Returns a string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString() => Name;
    }

    public partial class WideCalendar // Year or month infos
    {
        /// <inheritdoc />
        [Pure]
        public override int CountMonthsInYear(int year)
        {
            Scope.ValidateYear(year);
            return Schema.CountMonthsInYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public override int CountDaysInYear(int year)
        {
            Scope.ValidateYear(year);
            return Schema.CountDaysInYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public override int CountDaysInMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return Schema.CountDaysInMonth(year, month);
        }
    }

    public partial class WideCalendar // Factories
    {
        /// <summary>
        /// Creates a new instance of the <see cref="WideDate"/> struct from
        /// its components.
        /// </summary>
        [Pure]
        public WideDate GetWideDate(int year, int month, int day)
        {
            Scope.ValidateYearMonthDay(year, month, day);
            return new WideDate(year, month, day, Id);
        }

        /// <inheritdoc />
        [Pure]
        public WideDate Today() => GetWideDateOn(DayNumber.Today());
    }

    public partial class WideCalendar // Conversions
    {
        /// <summary>
        /// Converts the specified ordinal date to a date object.
        /// </summary>
        /// <exception cref="AoorException">The ordinal date is either invalid or outside the range
        /// of supported dates.</exception>
        [Pure]
        public WideDate GetWideDateOn(int year, int dayOfYear)
        {
            Scope.ValidateOrdinal(year, dayOfYear);
            int m = Schema.GetMonth(year, dayOfYear, out int d);
            return new WideDate(new Yemoda(year, m, d), Id);
        }

        /// <summary>
        /// Converts the specified day number to a date object.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// values supported by this calendar.</exception>
        [Pure]
        public WideDate GetWideDateOn(DayNumber dayNumber)
        {
            Domain.Validate(dayNumber);
            Schema.GetDateParts(dayNumber - Epoch, out int y, out int m, out int d);
            return new WideDate(new Yemoda(y, m, d), Id);
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
                int monthsInYear = Schema.CountMonthsInYear(year);

                for (int m = 1; m <= monthsInYear; m++)
                {
                    int daysInMonth = Schema.CountDaysInMonth(year, m);

                    for (int d = 1; d <= daysInMonth; d++)
                    {
                        yield return new WideDate(year, m, d, Id);
                    }
                }
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
                int daysInMonth = Schema.CountDaysInMonth(year, month);

                for (int d = 1; d <= daysInMonth; d++)
                {
                    yield return new WideDate(year, month, d, Id);
                }
            }
        }

        /// <inheritdoc />
        [Pure]
        public WideDate GetStartOfYear(int year)
        {
            Scope.ValidateYear(year);
            return new WideDate(Yemoda.AtStartOfYear(year), Id);
        }

        /// <inheritdoc />
        [Pure]
        public WideDate GetEndOfYear(int year)
        {
            Scope.ValidateYear(year);
            //Schema.GetEndOfYearParts(year, out int m, out int d);
            int m = Schema.CountMonthsInYear(year);
            int d = Schema.CountDaysInMonth(year, m);
            return new WideDate(new Yemoda(year, m, d), Id);
        }

        /// <inheritdoc />
        [Pure]
        public WideDate GetStartOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return new WideDate(Yemoda.AtStartOfMonth(year, month), Id);
        }

        /// <inheritdoc />
        [Pure]
        public WideDate GetEndOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            int d = Schema.CountDaysInMonth(year, month);
            return new WideDate(year, month, d, Id);
        }
    }

    public partial class WideCalendar // Internal helpers
    {
        /// <summary>
        /// Validates the specified Gregorian date.
        /// </summary>
        /// <exception cref="AoorException">The validation failed.</exception>
        internal static void ValidateGregorianYearMonthDay(int year, int month, int day, string? paramName = null)
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

        /// <summary>
        /// Validates the specified day of the month.
        /// <para>This method does NOT validate <paramref name="y"/>.</para>
        /// <para>This method does NOT validate <paramref name="m"/>.</para>
        /// </summary>
        /// <exception cref="OverflowException">The operation overflows the
        /// capacity of <see cref="Int32"/>.</exception>
        /// <exception cref="AoorException">The validation failed.</exception>
        internal void ValidateDayOfMonth(int y, int m, int dayOfMonth, string? paramName = null)
        {
            if (dayOfMonth < 1 || dayOfMonth > Schema.CountDaysInMonth(y, m))
            {
                Throw.ArgumentOutOfRange(paramName ?? nameof(dayOfMonth));
            }
        }

        [Pure]
        internal DayNumber GetDayNumber(WideDate date)
        {
            Debug.Assert(date.Cuid == Id);

            date.Parts.Unpack(out int y, out int m, out int d);
            return Epoch + Schema.CountDaysSinceEpoch(y, m, d);
        }

        [Pure]
        internal DayOfWeek GetDayOfWeek(WideDate date)
        {
            Debug.Assert(date.Cuid == Id);

            // Copied from CalendricalSchema.GetDayOfWeek().
            date.Parts.Unpack(out int y, out int m, out int d);
            int daysSinceEpoch = Schema.CountDaysSinceEpoch(y, m, d);
            return (DayOfWeek)MathZ.Modulo(
                checked(_epochDayOfWeek + daysSinceEpoch),
                CalendricalConstants.DaysInWeek);
        }
    }
}
