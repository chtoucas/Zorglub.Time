// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Hemerology;
    using Zorglub.Time.Hemerology.Scopes;
    using Zorglub.Time.Horology;

    #region Developer Notes

    // Normally, we should implement
    // - ICalendar<CalendarDate>
    // - ICalendar<CalendarDay>
    // - ICalendar<OrdinalDate>
    // but CalendarYear and CalendarMonth make this unnecessary.
    //
    //   Today()            Calendar.GetCurrentXXX()
    //
    //   GetDaysInYear()    DateRange iter
    //                      CalendarDate.GetDaysInYear()
    //   GetDaysInMonth()   DateRange iter
    //
    //   GetStartOfYear()   ISimpleDate<>.AtStartOfYear()
    //   GetEndOfYear()     ISimpleDate<>.AtEndOfYear()
    //   GetStartOfMonth()  ISimpleDate<>.AtStartOfMonth()
    //   GetEndOfMonth()    ISimpleDate<>.AtEndOfMonth()
    //
    //                      ISimpleDate<>.AtDayOfYear()
    //                      ISimpleDate<>.AtDayOfMonth()
    //
    // Conversions
    // -----------
    // - Conversion between CalendarDate, OrdinalDate and CalendarDay are
    //   methods on the objects themselves; see IDateXXX<>.ToXXX().
    // - Conversion from a date object to a DayNumber is a method on the
    //   object himself; see ToDayNumber().
    // - According to our definition above, Today() is a conversion op,
    //   except for CalendarDay...
    // We have a static method FromDayNumber() for each calendrical type
    // BUT it only deals with the Gregorian calendar.

    #endregion

    /// <summary>
    /// Defines a calendar to which the system assigns a unique ID and provides a base for derived
    /// classes.
    /// <para>This class can ONLY be inherited from within friend assemblies.</para>
    /// </summary>
    public partial class SimpleCalendar : ICalendar
    {
        /// <summary>
        /// Represents the integer value of the day of the week of the epoch of the calendar.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _epochDayOfWeek;

        /// <summary>
        /// Represents the mathematical rules followed by the calendar.
        /// </summary>
        private CalendarMath _math;

        /// <summary>
        /// Called from constructors in derived classes to initialize the <see cref="SimpleCalendar"/>
        /// class.
        /// <para>Constructor for (built-in) system calendars.</para>
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        private protected SimpleCalendar(
            CalendarId ident,
            SystemSchema schema,
            DayNumber epoch,
            bool proleptic)
            : this(
                  (Cuid)ident,
                  ident.ToCalendarKey(),
                  schema,
                  epoch,
                  proleptic,
                  userDefined: false)
        {
            Debug.Assert(ident.IsInvalid() == false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCalendar"/> class.
        /// <para>Constructor for user-defined calendars.</para>
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> does not contain the interval [-9998..9999] in the proleptic
        /// case -or- the interval [1..9999] in the standard.</exception>
        internal SimpleCalendar(
            Cuid id,
            string key,
            SystemSchema schema,
            DayNumber epoch,
            bool proleptic)
            : this(
                  id,
                  key,
                  schema,
                  epoch,
                  proleptic,
                  userDefined: true)
        {
            Debug.Assert(id >= Cuid.MinUser);
            Debug.Assert(id == Cuid.Invalid || id <= Cuid.Max);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCalendar"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> does not contain the interval [-9998..9999] in the proleptic
        /// case -or- the interval [1..9999] in the standard.</exception>
        private SimpleCalendar(
            Cuid id,
            string key,
            SystemSchema schema,
            DayNumber epoch,
            bool proleptic,
            bool userDefined)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Schema = schema ?? throw new ArgumentNullException(nameof(schema));

            Id = id;
            Epoch = epoch;
            _epochDayOfWeek = (int)epoch.DayOfWeek;

            IsProleptic = proleptic;
            IsUserDefined = userDefined;

            Scope = proleptic ? ProlepticScope.Create(schema, epoch)
                : StandardScope.Create(schema, epoch);

            SystemSegment = SystemSegment.Create(schema, YearsValidator.Range);
            Arithmetic = SystemArithmetic.CreateDefault(SystemSegment);

            // Keep this at the end of the constructor: before using "this",
            // all props should be initialized.
            _math = CalendarMath.CreateDefault(this);

            DefaultClock = SimpleClock.CreateDefault(this, SystemClock.Default);
            UtcClock = SimpleClock.CreateDefault(this, SystemClock.Utc);
        }

        #region System calendars

        /// <summary>
        /// Gets the proleptic Gregorian calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static SimpleCalendar Gregorian => GregorianSimpleCalendar.Instance;

        /// <summary>
        /// Gets the proleptic Julian calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static SimpleCalendar Julian => JulianSimpleCalendar.Instance;

        /// <summary>
        /// Gets the Civil calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static SimpleCalendar Civil => CivilSimpleCalendar.Instance;

        /// <summary>
        /// Gets the Armenian calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static SimpleCalendar Armenian => ArmenianSimpleCalendar.Instance;

        /// <summary>
        /// Gets the Coptic calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static SimpleCalendar Coptic => CopticSimpleCalendar.Instance;

        /// <summary>
        /// Gets the Ethiopic calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static SimpleCalendar Ethiopic => EthiopicSimpleCalendar.Instance;

        /// <summary>
        /// Gets the Tabular Islamic calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static SimpleCalendar TabularIslamic => TabularIslamicSimpleCalendar.Instance;

        /// <summary>
        /// Gets the Zoroastrian calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static SimpleCalendar Zoroastrian => ZoroastrianSimpleCalendar.Instance;

        #endregion

        /// <summary>
        /// Gets the unique key of the current instance.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Returns true if the current instance is user-defined; otherwise returns false.
        /// </summary>
        public bool IsUserDefined { get; }

        /// <summary>
        /// Gets the permanent unique identifier of the current instance.
        /// </summary>
        /// <exception cref="NotSupportedException">The current instance does not have a permanent
        /// unique identifier.</exception>
        public CalendarId PermanentId =>
            IsUserDefined ? Throw.NotSupported<CalendarId>() : (CalendarId)Id;

        /// <inheritdoc />
        public DayNumber Epoch { get; }

        /// <summary>
        /// Returns true if the current instance is proleptic; otherwise returns false.
        /// <para>A calendar is said to be (strongly) proleptic if it applies to dates before its
        /// epoch.</para>
        /// </summary>
        /// <remarks>
        /// <para>A calendar is <i>weakly proleptic</i> or simply <i>retropolated</i> if it is
        /// extended backward to the dates before its official introduction but only on or after its
        /// epoch.</para>
        /// <para>When we say that a calendar is <i>proleptic</i> without any further detail, we
        /// always mean strongly proleptic.</para>
        /// </remarks>
        // > [En parlant d'un phénomène, d'un fait historique] Qui est fixé ou
        // > daté d'après une méthode ou une ère chronologique qui n'était pas
        // > encore établie au moment où le fait ou le phénomène concerné se
        // > produisait. C'est ainsi qu'on fixe les événements de l'antiquité
        // > biblique, religieux ou profanes, en années juliennes proleptiques
        // > (Foit.11968).
        // https://www.cnrtl.fr/definition/proleptique
        //
        // NB: on n'ajoute pas cette propriété à ICalendar car notre définition
        // dit implicitement que "toutes" les dates avant l'epoch doivent être
        // acceptées. Il est donc nécessaire mais pas suffisant de vérifier que
        // MinDayNumber < Epoch (ou MinYear < 1). En même temps, quelle pourrait
        // être une limite basse acceptable pour tout type de ICalendar ?
        // Dans le cas présent, c'est plus simple, on s'accorde pour dire que
        // -9998 est la première année valide.
        public bool IsProleptic { get; }

        /// <inheritdoc />
        public CalendricalAlgorithm Algorithm => Schema.Algorithm;

        /// <inheritdoc />
        public CalendricalFamily Family => Schema.Family;

        /// <inheritdoc />
        public CalendricalAdjustments PeriodicAdjustments => Schema.PeriodicAdjustments;

        #region Domain & Min/Max Values
        // For CalendarYear & co, do not use a range. It's meaningless since
        // one cannot create objects outside these ranges. It's really the
        // equivalent of IMinMaxValue<>.

        /// <inheritdoc />
        public Range<DayNumber> Domain => Scope.Domain;

        private OrderedPair<CalendarYear> _minMaxYear;
        /// <summary>
        /// Gets the pair of earliest and latest supported <see cref="CalendarYear"/>.
        /// </summary>
        /// <returns>The pair (-9998, 9999) if the current instance is proleptic; otherwise the
        /// pair (1, 9999).</returns>
        public OrderedPair<CalendarYear> MinMaxYear
        {
            get { EnsureInitialized(); return _minMaxYear; }
        }

        private OrderedPair<CalendarMonth> _minMaxMonth;
        /// <summary>
        /// Gets the pair of earliest and latest supported <see cref="CalendarMonth"/>.
        /// </summary>
        public OrderedPair<CalendarMonth> MinMaxMonth
        {
            get { EnsureInitialized(); return _minMaxMonth; }
        }

        private OrderedPair<CalendarDate> _minMaxDate;
        /// <summary>
        /// Gets the pair of earliest and latest supported <see cref="CalendarDate"/>.
        /// </summary>
        public OrderedPair<CalendarDate> MinMaxDate
        {
            get { EnsureInitialized(); return _minMaxDate; }
        }

        private OrderedPair<CalendarDay> _minMaxDay;
        /// <summary>
        /// Gets the pair of earliest and latest supported <see cref="CalendarDay"/>.
        /// </summary>
        public OrderedPair<CalendarDay> MinMaxDay
        {
            get { EnsureInitialized(); return _minMaxDay; }
        }

        private OrderedPair<OrdinalDate> _minMaxOrdinal;
        /// <summary>
        /// Gets the pair of earliest and latest supported <see cref="OrdinalDate"/>.
        /// </summary>
        public OrderedPair<OrdinalDate> MinMaxOrdinal
        {
            get { EnsureInitialized(); return _minMaxOrdinal; }
        }

        private bool _initialized;
        private void EnsureInitialized()
        {
            if (_initialized) return;

            _minMaxYear =
                from y in YearsValidator.Range.Endpoints select new CalendarYear(y, Id);
            _minMaxDay =
                from dayNumber in Domain.Endpoints select new CalendarDay(dayNumber - Epoch, Id);

            var seg = SystemSegment;
            _minMaxMonth = from ym in seg.MinMaxMonthParts select new CalendarMonth(ym, Id);
            _minMaxDate = from ymd in seg.MinMaxDateParts select new CalendarDate(ymd, Id);
            _minMaxOrdinal = from ydoy in seg.MinMaxOrdinalParts select new OrdinalDate(ydoy, Id);

            _initialized = true;
        }

        #endregion

        /// <summary>
        /// Gets the calendar scope.
        /// </summary>
        public MinMaxYearScope Scope { get; }

        CalendarScope ICalendar.Scope => Scope;

        /// <summary>
        /// Gets or sets the mathematical rules followed by the current instance.
        /// <para>WARNING: since the calendars are singletons, setting this property will change the
        /// related mathematical operations for ALL objects using this calendar instance.</para>
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public CalendarMath Math
        {
            get => _math;
            set
            {
                Requires.NotNull(value);
                if (value.Cuid != Id) Throw.Argument(nameof(value));

                _math = value;
            }
        }

        /// <summary>
        /// Gets the ID of the current instance.
        /// </summary>
        internal Cuid Id { get; }

        /// <summary>
        /// Gets the calendrical schema.
        /// </summary>
        internal SystemSchema Schema { get; }

        /// <summary>
        /// Gets the pre-validator.
        /// </summary>
        internal ICalendricalPreValidator PreValidator => Schema.PreValidator;

        /// <summary>
        /// Gets the segment of supported days.
        /// </summary>
        internal SystemSegment SystemSegment { get; }

        /// <summary>
        /// Gets the arithmetical operators.
        /// </summary>
        internal SystemArithmetic Arithmetic { get; }

        /// <summary>
        /// Gets the validator for the range of supported years.
        /// </summary>
        internal IYearsValidator YearsValidator => Scope.YearsValidator;

        /// <summary>
        /// Gets the validator for the range of supported days.
        /// </summary>
        internal DaysValidator DaysValidator => Scope.DaysValidator;

        /// <summary>
        /// Returns a culture-independent string representation of this calendar.
        /// </summary>
        [Pure]
        public sealed override string ToString() => Key;

        /// <inheritdoc />
        [Pure]
        public bool IsRegular(out int monthsInYear) => Schema.IsRegular(out monthsInYear);

        /// <summary>
        /// Validates the specified <see cref="Cuid"/>.
        /// </summary>
        /// <exception cref="ArgumentException">The validation failed.</exception>
        private protected void ValidateCuid(Cuid cuid, string paramName)
        {
            if (cuid != Id) Throw.BadCuid(paramName, Id, cuid);
        }
    }

    public partial class SimpleCalendar // Clocks
    {
        /// <summary>
        /// Gets an instance of the <see cref="SimpleClock"/> class for the system clock using the
        /// default timezone.
        /// </summary>
        public SimpleClock DefaultClock { get; }

        /// <summary>
        /// Gets an instance of the <see cref="SimpleClock"/> class for the system clock using the
        /// UTC timezone.
        /// </summary>
        public SimpleClock UtcClock { get; }

        /// <summary>
        /// Obtains an instance of the <see cref="SimpleClock"/> class for the specified timepiece.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="timepiece"/> is null.</exception>
        [Pure]
        public virtual SimpleClock GetClock(ITimepiece timepiece) =>
            SimpleClock.CreateDefault(this, timepiece);
    }

    public partial class SimpleCalendar // Year or month infos
    {
        // Hidden, we have "better" alternatives.

        /// <inheritdoc />
        /// <exception cref="AoorException">The year is outside the range of supported years.
        /// </exception>
        [Pure]
        [SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Justification = "See CalendarYear.IsLeap.")]
        bool ICalendricalKernel.IsLeapYear(int year)
        {
            YearsValidator.Validate(year);
            return Schema.IsLeapYear(year);
        }

        /// <inheritdoc />
        /// <exception cref="AoorException">The month is either invalid or outside the range of
        /// supported months.</exception>
        [Pure]
        [SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Justification = "See CalendarMonth.IsIntercalary.")]
        bool ICalendricalKernel.IsIntercalaryMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return Schema.IsIntercalaryMonth(year, month);
        }

        /// <inheritdoc />
        [Pure]
        [SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Justification = "See {Date}.IsIntercalary.")]
        bool ICalendricalKernel.IsIntercalaryDay(int year, int month, int day)
        {
            Scope.ValidateYearMonthDay(year, month, day);
            return Schema.IsIntercalaryDay(year, month, day);
        }

        /// <inheritdoc />
        [Pure]
        [SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Justification = "See {Date}.IsSupplementaryDay.")]
        bool ICalendricalKernel.IsSupplementaryDay(int year, int month, int day)
        {
            Scope.ValidateYearMonthDay(year, month, day);
            return Schema.IsSupplementaryDay(year, month, day);
        }

        /// <inheritdoc />
        /// <exception cref="AoorException">The year is outside the range of supported years.
        /// </exception>
        [Pure]
        [SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Justification = "See CalendarYear.CountMonthsInYear().")]
        int ICalendricalKernel.CountMonthsInYear(int year)
        {
            YearsValidator.Validate(year);
            return Schema.CountMonthsInYear(year);
        }

        /// <inheritdoc />
        /// <exception cref="AoorException">The year is outside the range of supported years.
        /// </exception>
        [Pure]
        [SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Justification = "See CalendarYear.CountDaysInYear().")]
        int ICalendricalKernel.CountDaysInYear(int year)
        {
            YearsValidator.Validate(year);
            return Schema.CountDaysInYear(year);
        }

        /// <inheritdoc />
        /// <exception cref="AoorException">The month is either invalid or outside the range of
        /// supported months.</exception>
        [Pure]
        [SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Justification = "See CalendarMonth.CountDaysInMonth().")]
        int ICalendricalKernel.CountDaysInMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return Schema.CountDaysInMonth(year, month);
        }
    }

    public partial class SimpleCalendar // Factories
    {
        /// <summary>
        /// Creates a new instance of the <see cref="CalendarYear"/> struct from its components.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="year"/> is outside the range of years
        /// supported by the current instance.</exception>
        [Pure]
        public CalendarYear GetCalendarYear(int year)
        {
            YearsValidator.Validate(year);
            return new CalendarYear(year, Id);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="CalendarMonth"/> struct from its components.
        /// </summary>
        /// <exception cref="AoorException">The specified components do not form a valid month or
        /// <paramref name="year"/> is outside the range of years supported by the current instance.
        /// </exception>
        [Pure]
        public CalendarMonth GetCalendarMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return new CalendarMonth(year, month, Id);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="CalendarDate"/> struct from its components.
        /// </summary>
        /// <exception cref="AoorException">The specified components do not form a valid date or
        /// <paramref name="year"/> is outside the range of years supported by the current instance.
        /// </exception>
        [Pure]
        public CalendarDate GetDate(int year, int month, int day)
        {
            Scope.ValidateYearMonthDay(year, month, day);
            return new CalendarDate(year, month, day, Id);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OrdinalDate"/> struct from its components.
        /// </summary>
        /// <exception cref="AoorException">The specified components do not form a valid ordinal
        /// date or <paramref name="year"/> is outside the range of years supported by the current
        /// instance.</exception>
        [Pure]
        public OrdinalDate GetDate(int year, int dayOfYear)
        {
            Scope.ValidateOrdinal(year, dayOfYear);
            return new OrdinalDate(year, dayOfYear, Id);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="CalendarDay"/> struct from its components.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// values supported by this calendar.</exception>
        [Pure]
        public CalendarDay GetDate(DayNumber dayNumber)
        {
            Domain.Validate(dayNumber);
            return new CalendarDay(dayNumber - Epoch, Id);
        }
    }

    public partial class SimpleCalendar // Conversions
    {
        /// <summary>
        /// Obtains the calendar year on the specified day number.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// values supported by this calendar.</exception>
        [Pure]
        public CalendarYear GetCalendarYear(DayNumber dayNumber)
        {
            Domain.Validate(dayNumber);
            int y = Schema.GetYear(dayNumber - Epoch);
            return new CalendarYear(y, Id);
        }

        /// <summary>
        /// Obtains the calendar month on the specified day number.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// values supported by this calendar.</exception>
        [Pure]
        public CalendarMonth GetCalendarMonth(DayNumber dayNumber)
        {
            Domain.Validate(dayNumber);
            var ymd = Schema.GetDateParts(dayNumber - Epoch);
            return new CalendarMonth(ymd.Yemo, Id);
        }

        /// <summary>
        /// Obtains the calendar date on the specified day number.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// values supported by this calendar.</exception>
        [Pure]
        public CalendarDate GetCalendarDate(DayNumber dayNumber)
        {
            Domain.Validate(dayNumber);
            var ymd = Schema.GetDateParts(dayNumber - Epoch);
            return new CalendarDate(ymd, Id);
        }

        /// <summary>
        /// Obtains the ordinal date on the specified day number.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// values supported by this calendar.</exception>
        [Pure]
        public OrdinalDate GetOrdinalDate(DayNumber dayNumber)
        {
            Domain.Validate(dayNumber);
            var ydoy = Schema.GetOrdinalParts(dayNumber - Epoch);
            return new OrdinalDate(ydoy, Id);
        }

        // GetCalendarDay(DayNumber dayNumber) -> see the factory method above.

        [Pure]
        [SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Justification = "Use GetDate() then the prop DayNumber, or better ToCalendarDay().")]
        DayNumber ICalendar.GetDayNumber(int year, int month, int day)
        {
            // Hidden since we have a custom day object (CalendarDay).
            // The idea is that one should always create an object using its
            // "natural" parts.
            Scope.ValidateYearMonthDay(year, month, day);
            return Epoch + Schema.CountDaysSinceEpoch(year, month, day);
        }

        [Pure]
        [SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Justification = "Use GetDate() then the prop DayNumber, or better ToCalendarDay().")]
        DayNumber ICalendar.GetDayNumber(int year, int dayOfYear)
        {
            // Hidden since we have a custom day object (CalendarDay).
            // The idea is that one should always create an object using its
            // "natural" parts.
            Scope.ValidateOrdinal(year, dayOfYear);
            return Epoch + Schema.CountDaysSinceEpoch(year, dayOfYear);
        }
    }

    public partial class SimpleCalendar // Internal helpers
    {
        // Internal helpers for use by the calendrical objects.
        // - ValidateDayOfMonth(...)
        // - GetDayOfWeek(...) because of _epochDayOfWeek.

        /// <summary>
        /// Validates the specified day of the month.
        /// <para>This method does NOT validate <paramref name="y"/>.</para>
        /// <para>This method does NOT validate <paramref name="m"/>.</para>
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int32"/>.</exception>
        /// <exception cref="AoorException">The validation failed.</exception>
        internal void ValidateDayOfMonth(int y, int m, int dayOfMonth, string? paramName = null)
        {
            if (dayOfMonth < 1
                || (dayOfMonth > Schema.MinDaysInMonth
                    && dayOfMonth > Schema.CountDaysInMonth(y, m)))
            {
                Throw.ArgumentOutOfRange(paramName ?? nameof(dayOfMonth));
            }
        }

        /// <summary>
        /// Obtains the day of the week of the specified date.
        /// <para>This method MUST be overriden by proleptic calendars.</para>
        /// </summary>
        [Pure]
        internal virtual DayOfWeek GetDayOfWeek(CalendarDate date)
        {
            // NB: if we disallow proleptic user-defined calendars, we can use
            // the unsigned modulo operator; idem with the other GetDayOfWeek().
            Debug.Assert(date.Cuid == Id);
            //Debug.Assert(!IsProleptic);

            date.Parts.Unpack(out int y, out int m, out int d);
            int days = _epochDayOfWeek + Schema.CountDaysSinceEpoch(y, m, d);
            //return (DayOfWeek)((uint)days % CalendricalConstants.DaysInWeek);
            return (DayOfWeek)MathZ.Modulo(days, CalendricalConstants.DaysInWeek);
        }

        /// <summary>
        /// Obtains the day of the week of the specified ordinal date.
        /// <para>This method MUST be overriden by proleptic calendars.</para>
        /// </summary>
        [Pure]
        internal virtual DayOfWeek GetDayOfWeek(OrdinalDate date)
        {
            Debug.Assert(date.Cuid == Id);

            date.Parts.Unpack(out int y, out int doy);
            int days = _epochDayOfWeek + Schema.CountDaysSinceEpoch(y, doy);
            return (DayOfWeek)MathZ.Modulo(days, CalendricalConstants.DaysInWeek);
        }
    }
}
