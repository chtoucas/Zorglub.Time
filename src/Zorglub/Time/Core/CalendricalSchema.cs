// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using Zorglub.Time.Core.Arithmetic;
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Schemas;
    using Zorglub.Time.Core.Validation;

    using static Zorglub.Time.Core.CalendricalConstants;

    // TODO(doc): XML doc, overflows (validator), no array lookup.

    // WARNING: A schema should avoid public static methods, otherwise it
    // would defeat our "box" principle; see below.

    #region Developer Notes

    // CalendricalSchema is one way to implement an arithmetical schema,
    // but surely there are others.
    //
    // Contract
    // --------
    // There is a hidden contract stating that:
    // - the system only passes valid parameters from a calendrical point of
    //   view. In particular, there is no need for a method to validate its
    //   parameters.
    // - a schema MUST produce valid data from a calendrical point of view.
    // - finally, all methods MUST not overflow with years within the range of
    //   years defined by the schema.
    //
    // Derived classes
    // ---------------
    // In practice, only four methods may require non-trivial work:
    // - IsLeapYear()
    // - GetYear()
    // - GetMonth()
    // - GetStartOfYear()
    // Of course, this is not true if we override the virtual methods.
    //
    // Speaking of virtual methods, we expect that most classes will override
    // CountDaysSinceEpoch() and GetDateParts(), while they won't touch GetYear().
    //
    // If possible, derived classes should implement CountDaysInYearBeforeMonth()
    // and CountDaysInMonth() without resorting to table lookups.
    // Actually, for CountDaysInMonth(), it is more or less an hidden contract.
    //
    // Performance
    // -----------
    // Validation:
    // - CountDaysInMonth()
    // - CountDaysInYear()
    // There is also CountMonthsInYear() BUT most schemas implement IRegularSchema.
    //
    // Other methods to pay attention to:
    // - IsLeapYear()
    // - CountDaysInYearBeforeMonth()
    // - CountDaysSinceEpoch()
    //   This one is particularly important. We use it for interconversion, math
    //   operations, computation or adjustment of the day of the week.
    // - GetDateParts()
    //   Reverse of CountDaysSinceEpoch().
    // - GetYear()
    //   Mostly if we don't override GetYear(y, out doy).
    // - GetStartOfYear()
    //
    // Access modifiers
    // ----------------
    // To limit the scope of schemas, all final schemas SHOULD NOT expose a
    // public ctor (internal is OK) --- the schemas themselves can't be internal
    // if we want to be able to define strongly-typed calendars (e.g. see
    // Calendar). Of course, if you don't intend to publish your
    // schema, you can ignore this recommendation.
    //
    // Pre-defined schemas provide a static "factory" method:
    //   public static Box<MySchema> GetInstance() =>
    //       Box.Of(new MySchema());
    // This way, schemas are public but one can NOT use instance methods
    // regardless of their access modifiers.
    //
    // Static methods CAN be public but only if they are guaranteed to work
    // whatever input we give to them (overflows).
    //
    // Design
    // ------
    // To be able to extend this class outside this project, we ensure that
    // no abstract or virtual method returns a Yemoda/Yemo/Yedoy --- no public
    // ctor, just a factory method that does unnecessary param check if the
    // extender respects the contract defined below: the results must be
    // representable by the system.
    //
    // If we had only one base class for all calendars, we would have used the
    // Template Method Pattern, but we don't. In other words, we would have not
    // needed a separate class hierarchy from Calendar.
    //
    // It would have been possible to provide default implementations for:
    // - CountDaysInYearBeforeMonth()
    // - GetEndOfYearParts()
    // but better to left them out since they would have been very inefficient.
    //
    // Do NOT use Yemoda as input. It has a major drawback, we could pack/unpack
    // the same data multiple times without even realizing it.
    //
    // What's not here
    // ---------------
    // - GetDayOfWeek()
    //   Missing requirement: an epoch.
    // - Everything arithmetic
    //   Missing requirement: a "scope".
    // All this will be part of a calendar or a date object.

    #endregion

    /// <summary>
    /// Represents a calendrical schema and provides a base for derived classes.
    /// </summary>
    public abstract partial class CalendricalSchema : ICalendricalSchema, ICalendricalSchemaPlus
    {
        /// <summary>
        /// Called from constructors in derived classes to initialize the
        /// <see cref="CalendricalSchema"/> class.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="minDaysInYear"/> or
        /// <paramref name="minDaysInMonth"/> is a negative integer.</exception>
        protected CalendricalSchema(Range<int> supportedYears, int minDaysInYear, int minDaysInMonth)
        {
            if (minDaysInYear <= 0) Throw.ArgumentOutOfRange(nameof(minDaysInYear));
            if (minDaysInMonth <= 0) Throw.ArgumentOutOfRange(nameof(minDaysInMonth));

            SupportedYears = supportedYears;
            MinDaysInYear = minDaysInYear;
            MinDaysInMonth = minDaysInMonth;
        }

        private ICalendricalPreValidator? _validator;
        /// <inheritdoc />
        public ICalendricalPreValidator PreValidator
        {
            get => _validator ??= ICalendricalPreValidator.CreateDefault(this);
            protected init
            {
                Requires.NotNull(value);
                _validator = value;
            }
        }

        ICalendricalArithmetic ICalendricalSchema.Arithmetic => Arithmetic;

        private ICalendricalArithmeticPlus? _arithmetic;
        /// <summary>
        /// Gets the arithmetic for this schema.
        /// </summary>
        public ICalendricalArithmeticPlus Arithmetic
        {
            get => _arithmetic ??= ICalendricalArithmeticPlus.CreateDefault(this);
            protected init
            {
                Requires.NotNull(value);
                _arithmetic = value;
            }
        }

        private CalendricalProfile? _profile;
        /// <summary>
        /// Gets the schema profile.
        /// </summary>
        internal CalendricalProfile Profile => _profile ??= FindProfile();

        /// <inheritdoc />
        [Pure]
        public abstract bool IsRegular(out int monthsInYear);

        [Pure]
        private CalendricalProfile FindProfile()
        {
            // The schema is not regular iff monthsInYear = 0.
            _ = IsRegular(out int monthsInYear);

            // TODO(code): set Lunisolar.MinDaysInYear to 354, then it's easy to
            // distinguish Lunar from Lunisolar, since one is regular and the
            // other is not.

            // WARNING: the order is important, higher values of MinDaysInYear
            // MUST come first, then those of MinDaysInMonth.
            return this switch
            {
                {
                    MinDaysInYear: >= Solar.MinDaysInYear,      // 365
                    MinDaysInMonth: >= Solar.MinDaysInMonth,    // 28
                } => monthsInYear switch
                {
                    Solar12.MonthsInYear => CalendricalProfile.Solar12,
                    Solar13.MonthsInYear => CalendricalProfile.Solar13,
                    // Notice that MinDaysInMonth >= 7.
                    _ => CalendricalProfile.Other,
                },

                {
                    MinDaysInYear: >= Lunar.MinDaysInYear,      // 354
                    MinDaysInMonth: >= Lunar.MinDaysInMonth,    // 29
                } => monthsInYear == Lunar.MonthsInYear
                    ? CalendricalProfile.Lunar
                    // Notice that MinDaysInMonth >= 7.
                    : CalendricalProfile.Other,

                {
                    MinDaysInYear: >= Lunisolar.MinDaysInYear,  // 353
                    MinDaysInMonth: >= Lunisolar.MinDaysInMonth,// 29
                } => monthsInYear == 0
                    ? CalendricalProfile.Lunisolar
                    // Notice that MinDaysInMonth >= 7.
                    : CalendricalProfile.Other,

                _ => CalendricalProfile.Other,
            };
        }
    }

    public partial class CalendricalSchema // Properties
    {
        /// <inheritdoc />
        public CalendricalAlgorithm Algorithm => CalendricalAlgorithm.Arithmetical;

        /// <inheritdoc />
        public abstract CalendricalFamily Family { get; }

        /// <inheritdoc />
        public abstract CalendricalAdjustments PeriodicAdjustments { get; }

        /// <inheritdoc />
        public int MinDaysInYear { get; }

        /// <inheritdoc />
        public int MinDaysInMonth { get; }

        /// <inheritdoc />
        /// <remarks>
        /// <para>For methods expecting a month or day parameters, we assume that they are within
        /// the range defined by <see cref="Yemoda"/>.</para>
        /// </remarks>
        public Range<int> SupportedYears { get; }

        private Range<int>? _domain;
        /// <inheritdoc />
        public Range<int> Domain =>
            _domain ??= new Range<int>(SupportedYears.Endpoints.Select(GetStartOfYear, GetEndOfYear));
    }

    public partial class CalendricalSchema // Year, month or day infos
    {
        /// <inheritdoc />
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int32"/>.</exception>
        [Pure] public abstract bool IsLeapYear(int y);

        /// <inheritdoc />
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int32"/>.</exception>
        [Pure] public abstract bool IsIntercalaryMonth(int y, int m);

        /// <inheritdoc />
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int32"/>.</exception>
        [Pure] public abstract bool IsIntercalaryDay(int y, int m, int d);

        /// <inheritdoc />
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int32"/>.</exception>
        [Pure] public abstract bool IsSupplementaryDay(int y, int m, int d);
    }

    public partial class CalendricalSchema // Counting months and days within a year or a month
    {
        /// <inheritdoc />
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int32"/>.</exception>
        [Pure] public abstract int CountMonthsInYear(int y);

        /// <inheritdoc />
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int32"/>.</exception>
        [Pure] public abstract int CountDaysInYear(int y);

        /// <inheritdoc />
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int32"/>.</exception>
        [Pure] public abstract int CountDaysInMonth(int y, int m);

        /// <inheritdoc />
        // Notice that CountDaysInYearBeforeMonth() is entirely determined
        // by CountDaysInMonth(): sum of CountDaysInMonth(y, i) for 0 < i < m.
        // Nevertheless, this method being used by the default implementation of
        // CountDaysSinceEpoch() and other methods, we need a more efficient
        // solution.
        [Pure] public abstract int CountDaysInYearBeforeMonth(int y, int m);

        /// <inheritdoc />
        [Pure]
        public int CountDaysInYearAfterMonth(int y, int m) =>
            // We could have writen:
            // > return CountDaysInYear(y) - CountDaysInMonth(y, m)
            // >   - CountDaysInYearBeforeMonth(y, m);
            // but I am pretty sure it is slower --- CountMonthsInYear() is
            // almost always a constant and all three methods CountDaysIn...()
            // have to check whether the year is leap or not.
            // WARNING: Below we would expect an equality test not >=, but it
            // might be problematic when testing overflows, indeed Yemo(y, m + 1)
            // might not be representable (no longer the case).
            m >= CountMonthsInYear(y) ? 0
            : CountDaysInYear(y) - CountDaysInYearBeforeMonth(y, m + 1);

        #region CountDaysInYearBefore()

        /// <inheritdoc />
        [Pure]
        public int CountDaysInYearBefore(int y, int m, int d) =>
            // > GetDayOfYear(y, m, d) - 1
            CountDaysInYearBeforeMonth(y, m) + d - 1;

        /// <inheritdoc />
        [Pure]
        public int CountDaysInYearBefore(int y, int doy) => doy - 1;

        /// <inheritdoc />
        [Pure]
        public int CountDaysInYearBefore(int daysSinceEpoch)
        {
            // Quick check: we should obtain 0 for the first day of the year.
            _ = GetYear(daysSinceEpoch, out int doy);
            return doy - 1;
        }

        #endregion
        #region CountDaysInYearAfter()

        /// <inheritdoc />
        [Pure]
        public int CountDaysInYearAfter(int y, int m, int d) =>
            CountDaysInYear(y) - CountDaysInYearBeforeMonth(y, m) - d;

        /// <inheritdoc />
        [Pure]
        public int CountDaysInYearAfter(int y, int doy) => CountDaysInYear(y) - doy;

        /// <inheritdoc />
        [Pure]
        public int CountDaysInYearAfter(int daysSinceEpoch)
        {
            // Quick check: we should obtain (daysInYear - 1) for the first day
            // of the year. Formula: "daysInYear - dayOfYear" where
            // > dayOfYear = 1 + daysSinceEpoch - GetStartOfYear(y);
            // See comments within GetDayOfYear(y, m, d).
            // The simple implementation goes like this
            // > GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
            // > return CountDaysInYearAfter(y, m, d);
            // but computing m and d is just unnecessary.
            int y = GetYear(daysSinceEpoch, out int doy);
            return CountDaysInYear(y) - doy;
        }

        #endregion
        #region CountDaysInMonthBefore()

        /// <inheritdoc />
        [Pure]
        public int CountDaysInMonthBefore(int y, int m, int d) => d - 1;

        /// <inheritdoc />
        [Pure]
        public int CountDaysInMonthBefore(int y, int doy)
        {
            _ = GetMonth(y, doy, out int d);
            return d - 1;
        }

        /// <inheritdoc />
        [Pure]
        public int CountDaysInMonthBefore(int daysSinceEpoch)
        {
            // Straightforward implementation but I doubt that one can do better
            // than that; the result is bound to the y/m/d representation.
            GetDateParts(daysSinceEpoch, out _, out _, out int d);
            return d - 1;
        }

        #endregion
        #region CountDaysInMonthAfter()

        /// <inheritdoc />
        [Pure]
        public int CountDaysInMonthAfter(int y, int m, int d) => CountDaysInMonth(y, m) - d;

        /// <inheritdoc />
        [Pure]
        public int CountDaysInMonthAfter(int y, int doy)
        {
            int m = GetMonth(y, doy, out int d);
            return CountDaysInMonth(y, m) - d;
        }

        /// <inheritdoc />
        [Pure]
        public int CountDaysInMonthAfter(int daysSinceEpoch)
        {
            // Straightforward implementation but I doubt that one can do better
            // than that; the result is bound to the y/m/d representation.
            GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
            return CountDaysInMonth(y, m) - d;
        }

        #endregion
    }

    public partial class CalendricalSchema // Conversions
    {
        /// <inheritdoc />
        [Pure]
        public abstract int CountMonthsSinceEpoch(int y, int m);

        /// <inheritdoc />
        [Pure]
        public virtual int CountDaysSinceEpoch(int y, int m, int d) =>
            GetStartOfYear(y) + CountDaysInYearBeforeMonth(y, m) + d - 1;

        /// <inheritdoc />
        [Pure]
        public int CountDaysSinceEpoch(int y, int doy) => GetStartOfYear(y) + doy - 1;

        /// <inheritdoc />
        public abstract void GetMonthParts(int monthsSinceEpoch, out int y, out int m);

        /// <inheritdoc />
        public virtual void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
        {
            // This is just daysSinceEpoch -> (y, doy) -> (y, m, d), that is
            // 1. Find the year; "doy" is then a given.
            // 2. Find the month; "d" is then a given.
            // I doubt that one can do better than that. Even in the Gregorian
            // and Julian schemas where the impl seems different, the principle
            // is actually identical; the computation is just done using a
            // different month numbering.

            // Conversion daysSinceEpoch -> (y, doy).
            y = GetYear(daysSinceEpoch, out int doy);
            // Conversion (y, doy) -> (y, m, d).
            m = GetMonth(y, doy, out d);
        }

        /// <inheritdoc />
        [Pure] public abstract int GetYear(int daysSinceEpoch, out int doy);

        /// <inheritdoc />
        // GetMonth() without an out param could be useful for OrdinalDate but,
        // in practice, it seems simpler and better to keep the computations of
        // "d" & "m" together.
        // REVIEW(code): we encountered a similar situation with GetYear(),
        // except that we often compute its result without having to compute
        // GetStartOfYear(); see GetYear(int daysSinceEpoch, out int doy).
        // Once we know "m", it is easy to compute "d".
        //   d = doy - CountDaysInYearBeforeMonth(y, m);
        [Pure] public abstract int GetMonth(int y, int doy, out int d);

        /// <inheritdoc />
        [Pure]
        public int GetDayOfYear(int y, int m, int d) =>
            // NB: when one already has also "daysSinceEpoch" at its disposal,
            // it is faster to compute:
            // > 1 + daysSinceEpoch - GetStartOfYear(y);
            // This is just CountDaysInYearBefore(y, m, d) + 1
            CountDaysInYearBeforeMonth(y, m) + d;
    }

    public partial class CalendricalSchema //
    {
        /// <inheritdoc />
        // Even if it is just CountDaysSinceEpoch(y, 1, 1), this method MUST
        // be implemented independently. Indeed, we use it to provide a default
        // impl for CountDaysSinceEpoch(); see also GetYear().
        // Idem w/ GetEndOfYear(y - 1) + 1, moreover it could overflow at MinYear.
        [Pure] public abstract int GetStartOfYear(int y);

        /// <inheritdoc />
        // We could use GetStartOfYear(y + 1) - 1 but it might overflow at MaxYear.
        [Pure]
        public int GetEndOfYear(int y) => GetStartOfYear(y) + CountDaysInYear(y) - 1;

        /// <inheritdoc />
        [Pure]
        public int GetStartOfMonth(int y, int m) =>
            GetStartOfYear(y) + CountDaysInYearBeforeMonth(y, m);

        /// <inheritdoc />
        [Pure]
        public int GetEndOfMonth(int y, int m) =>
            GetStartOfMonth(y, m) + CountDaysInMonth(y, m) - 1;
    }
}
