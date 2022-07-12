// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using Zorglub.Time.Core.Intervals;

    // FIXME(code): que faire de la propriété Arithmetic?

    public abstract partial class CalendricalSchemaValidated : ICalendricalSchema
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendricalSchemaValidated"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        protected CalendricalSchemaValidated(ICalendricalSchema schema)
        {
            Requires.NotNull(schema);

            Schema = schema is CalendricalSchemaValidated sch ? sch.Schema : schema;
        }

        /// <inheritdoc />
        public CalendricalAlgorithm Algorithm => Schema.Algorithm;

        /// <inheritdoc />
        public CalendricalFamily Family => Schema.Family;

        /// <inheritdoc />
        public CalendricalAdjustments PeriodicAdjustments => Schema.PeriodicAdjustments;

        /// <inheritdoc />
        public int MinDaysInYear => Schema.MinDaysInYear;

        /// <inheritdoc />
        public int MinDaysInMonth => Schema.MinDaysInMonth;

        /// <inheritdoc />
        public Range<int> SupportedYears => Schema.SupportedYears;

        /// <inheritdoc />
        public Range<int> SupportedDays => Schema.SupportedDays;

        /// <inheritdoc />
        public Range<int> SupportedMonths => Schema.SupportedMonths;

        /// <inheritdoc />
        public ICalendricalPreValidator PreValidator => Schema.PreValidator;

        /// <summary>
        /// Gets the original <see cref="ICalendricalSchema"/>.
        /// </summary>
        protected internal ICalendricalSchema Schema { get; }

        [Pure]
        public bool IsRegular(out int monthsInYear) => Schema.IsRegular(out monthsInYear);

        protected abstract void ValidateYear(int y);
        protected abstract void ValidateYearMonth(int y, int m);
        protected abstract void ValidateYearMonthDay(int y, int m, int d);
        protected abstract void ValidateOrdinal(int y, int doy);
        protected abstract void ValidateMonthsSinceEpoch(int monthsSinceEpoch);
        protected abstract void ValidateDaysSinceEpoch(int daysSinceEpoch);

        /// <summary>
        /// Creates a new instance of the <see cref="CalendricalSchemaValidated"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        public static CalendricalSchemaValidated Create(ICalendricalSchema schema, bool strict) =>
            strict ? new StrictlyValidated(schema) : new PreValidated(schema);

        private sealed class PreValidated : CalendricalSchemaValidated
        {
            public PreValidated(ICalendricalSchema schema) : base(schema) { }

            protected override void ValidateYear(int y) { }

            protected override void ValidateYearMonth(int y, int m) =>
                PreValidator.ValidateMonth(y, m);

            protected override void ValidateYearMonthDay(int y, int m, int d) =>
                PreValidator.ValidateMonthDay(y, m, d);

            protected override void ValidateOrdinal(int y, int doy) =>
                PreValidator.ValidateDayOfYear(y, doy);

            protected override void ValidateMonthsSinceEpoch(int monthsSinceEpoch) { }

            protected override void ValidateDaysSinceEpoch(int daysSinceEpoch) { }
        }

        private sealed class StrictlyValidated : CalendricalSchemaValidated
        {
            private readonly CalendricalValidator _validator;

            public StrictlyValidated(ICalendricalSchema schema) : base(schema)
            {
                Debug.Assert(schema != null);

                _validator = new CalendricalValidator(schema, schema.SupportedYears);
            }

            protected override void ValidateYear(int y) =>
                _validator.ValidateYear(y);

            protected override void ValidateYearMonth(int y, int m) =>
                _validator.ValidateYearMonth(y, m);

            protected override void ValidateYearMonthDay(int y, int m, int d) =>
                _validator.ValidateYearMonthDay(y, m, d);

            protected override void ValidateOrdinal(int y, int doy) =>
                _validator.ValidateOrdinal(y, doy);

            protected override void ValidateMonthsSinceEpoch(int monthsSinceEpoch) =>
                _validator.Segment.SupportedMonths.Validate(monthsSinceEpoch);

            protected override void ValidateDaysSinceEpoch(int daysSinceEpoch) =>
                _validator.Segment.SupportedDays.Validate(daysSinceEpoch);
        }
    }

    public partial class CalendricalSchemaValidated // Year, month or day infos
    {
        [Pure]
        public bool IsLeapYear(int y)
        {
            ValidateYear(y);
            return Schema.IsLeapYear(y);
        }

        [Pure]
        public bool IsIntercalaryMonth(int y, int m)
        {
            ValidateYearMonth(y, m);
            return Schema.IsIntercalaryMonth(y, m);
        }

        [Pure]
        public bool IsIntercalaryDay(int y, int m, int d)
        {
            ValidateYearMonthDay(y, m, d);
            return Schema.IsIntercalaryDay(y, m, d);
        }

        [Pure]
        public bool IsSupplementaryDay(int y, int m, int d)
        {
            ValidateYearMonthDay(y, m, d);
            return Schema.IsSupplementaryDay(y, m, d);
        }
    }

    public partial class CalendricalSchemaValidated // Counting months and days within a year or a month
    {
        [Pure]
        public int CountMonthsInYear(int y)
        {
            ValidateYear(y);
            return Schema.CountMonthsInYear(y);
        }

        [Pure]
        public int CountDaysInYear(int y)
        {
            ValidateYear(y);
            return Schema.CountDaysInYear(y);
        }

        [Pure]
        public int CountDaysInMonth(int y, int m)
        {
            ValidateYearMonth(y, m);
            return Schema.CountDaysInMonth(y, m);
        }

        [Pure]
        public int CountDaysInYearBeforeMonth(int y, int m)
        {
            ValidateYearMonth(y, m);
            return Schema.CountDaysInYearBeforeMonth(y, m);
        }

#if false // ICalendricalSchemaPlus
        [Pure]
        public int CountDaysInYearAfterMonth(int y, int m) =>
            Schema.CountDaysInYearAfterMonth(y, m);

        #region CountDaysInYearBefore()

        [Pure]
        public int CountDaysInYearBefore(int y, int m, int d) =>
            Schema.CountDaysInYearBefore(y, m, d);

        [Pure]
        public int CountDaysInYearBefore(int y, int doy) =>
            Schema.CountDaysInYearBefore(y, doy);

        [Pure]
        public int CountDaysInYearBefore(int daysSinceEpoch) =>
            Schema.CountDaysInYearBefore(daysSinceEpoch);

        #endregion
        #region CountDaysInYearAfter()

        [Pure]
        public int CountDaysInYearAfter(int y, int m, int d) =>
            Schema.CountDaysInYearAfter(y, m, d);

        [Pure]
        public int CountDaysInYearAfter(int y, int doy) =>
            Schema.CountDaysInYearAfter(y, doy);

        [Pure]
        public int CountDaysInYearAfter(int daysSinceEpoch) =>
            Schema.CountDaysInYearAfter(daysSinceEpoch);

        #endregion
        #region CountDaysInMonthBefore()

        [Pure]
        int CountDaysInMonthBefore(int y, int m, int d) =>
            Schema.CountDaysInMonthBefore(y, m, d);

        [Pure]
        int CountDaysInMonthBefore(int y, int doy) =>
            Schema.CountDaysInMonthBefore(y, doy);

        [Pure]
        int CountDaysInMonthBefore(int daysSinceEpoch) =>
            Schema.CountDaysInMonthBefore(daysSinceEpoch);

        #endregion
        #region CountDaysInMonthAfter()

        [Pure]
        int CountDaysInMonthAfter(int y, int m, int d) =>
            Schema.CountDaysInMonthAfter(y, m, d);

        [Pure]
        int CountDaysInMonthAfter(int y, int doy) =>
            Schema.CountDaysInMonthAfter(y, doy);

        [Pure]
        int CountDaysInMonthAfter(int daysSinceEpoch) =>
            Schema.CountDaysInMonthAfter(daysSinceEpoch);

        #endregion
#endif
    }

    public partial class CalendricalSchemaValidated // Conversions
    {
        [Pure]
        public int CountMonthsSinceEpoch(int y, int m)
        {
            ValidateYearMonth(y, m);
            return Schema.CountMonthsSinceEpoch(y, m);
        }

        [Pure]
        public int CountDaysSinceEpoch(int y, int m, int d)
        {
            ValidateYearMonthDay(y, m, d);
            return Schema.CountDaysSinceEpoch(y, m, d);
        }

        [Pure]
        public int CountDaysSinceEpoch(int y, int doy)
        {
            ValidateOrdinal(y, doy);
            return Schema.CountDaysSinceEpoch(y, doy);
        }

        public void GetMonthParts(int monthsSinceEpoch, out int y, out int m)
        {
            ValidateMonthsSinceEpoch(monthsSinceEpoch);
            Schema.GetMonthParts(monthsSinceEpoch, out y, out m);
        }

        public void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
        {
            ValidateDaysSinceEpoch(daysSinceEpoch);
            Schema.GetDateParts(daysSinceEpoch, out y, out m, out d);
        }

        [Pure]
        public int GetYear(int daysSinceEpoch, out int doy)
        {
            ValidateDaysSinceEpoch(daysSinceEpoch);
            return Schema.GetYear(daysSinceEpoch, out doy);
        }

        [Pure]
        public int GetMonth(int y, int doy, out int d)
        {
            ValidateOrdinal(y, doy);
            return Schema.GetMonth(y, doy, out d);
        }

        [Pure]
        public int GetDayOfYear(int y, int m, int d)
        {
            ValidateYearMonthDay(y, m, d);
            return Schema.GetDayOfYear(y, m, d);
        }
    }

    public partial class CalendricalSchemaValidated //
    {
        [Pure]
        public int GetStartOfYearInMonths(int y)
        {
            ValidateYear(y);
            return Schema.GetStartOfYearInMonths(y);
        }

        [Pure]
        public int GetEndOfYearInMonths(int y)
        {
            ValidateYear(y);
            return Schema.GetEndOfYearInMonths(y);
        }

        [Pure]
        public int GetStartOfYear(int y)
        {
            ValidateYear(y);
            return Schema.GetStartOfYear(y);
        }

        [Pure]
        public int GetEndOfYear(int y)
        {
            ValidateYear(y);
            return Schema.GetEndOfYear(y);
        }

        [Pure]
        public int GetStartOfMonth(int y, int m)
        {
            ValidateYearMonth(y, m);
            return Schema.GetStartOfMonth(y, m);
        }

        [Pure]
        public int GetEndOfMonth(int y, int m)
        {
            ValidateYearMonth(y, m);
            return Schema.GetEndOfMonth(y, m);
        }
    }
}
