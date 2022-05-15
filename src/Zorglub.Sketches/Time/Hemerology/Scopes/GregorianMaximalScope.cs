// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Scopes
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Schemas;

    using static Zorglub.Time.Core.CalendricalConstants;

    // TODO(api): remove...

    /// <summary>
    /// Represents the maximal scope of the Gregorian schema.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed partial class GregorianMaximalScope : ICalendarScope
    {
        /// <summary>
        /// Represents the latest supported year.
        /// <para>This field is a constant equal to 999_999.</para>
        /// </summary>
        internal const int MaxYear = GregorianSchema.MaxYear;

        /// <summary>
        /// Represents the earliest supported year.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _minYear;

        /// <summary>
        /// Initializes a new instance of the <see cref="GregorianMaximalScope"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        public GregorianMaximalScope(GregorianSchema schema, DayNumber epoch, bool widest)
        {
            Requires.NotNull(schema);
            Debug.Assert(schema.Profile == CalendricalProfile.Solar12);

            Epoch = epoch;

            _minYear = widest ? GregorianSchema.MinYear : 1;
            SupportedYears = Range.CreateLeniently(_minYear, MaxYear);

            var minDaysSinceEpoch = schema.GetStartOfYear(_minYear);
            var maxDaysSinceEpoch = schema.GetEndOfYear(MaxYear);

            Domain = Range.Create(epoch + minDaysSinceEpoch, epoch + maxDaysSinceEpoch);
        }

        /// <summary>
        /// Gets the checker for overflows of the range of years.
        /// </summary>
        internal IOverflowChecker<int> YearOverflowChecker => new YearOverflowChecker_(_minYear);

        /// <inheritdoc />
        public DayNumber Epoch { get; }

        /// <inheritdoc />
        public Range<DayNumber> Domain { get; }

        /// <inheritdoc />
        public Range<int> SupportedYears { get; }

        private sealed class YearOverflowChecker_ : IOverflowChecker<int>
        {
            private readonly int _minYear;

            public YearOverflowChecker_(int minYear) { _minYear = minYear; }

            public void Check(int year)
            {
                if (year < _minYear || year > MaxYear) Throw.DateOverflow();
            }

            public void CheckUpperBound(int year)
            {
                if (year > MaxYear) Throw.DateOverflow();
            }

            public void CheckLowerBound(int year)
            {
                if (year < _minYear) Throw.DateOverflow();
            }
        }
    }

    public partial class GregorianMaximalScope
    {
        /// <summary>
        /// Validates the specified Gregorian date.
        /// </summary>
        /// <exception cref="AoorException">The validation failed.</exception>
        // <para>Only used by
        // <see cref="Advanced.WideDate.WideDate(int, int, int)"/>:
        // option widest = true which matches the scope of
        // <see cref="Advanced.WideCalendar.Gregorian"/>.</para>
        internal static void ValidateWideYearMonthDay(int year, int month, int day, string? paramName = null)
        {
            if (year < GregorianSchema.MinYear || year > MaxYear)
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

        /// <inheritdoc />
        public void ValidateYear(int year, string? paramName = null)
        {
            if (year < _minYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year, paramName);
            }
        }

        /// <inheritdoc />
        public void ValidateYearMonth(int year, int month, string? paramName = null)
        {
            if (year < _minYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year, paramName);
            }
            if (month < 1 || month > Solar12.MonthsInYear)
            {
                Throw.MonthOutOfRange(month, paramName);
            }
        }

        /// <inheritdoc />
        public void ValidateYearMonthDay(int year, int month, int day, string? paramName = null)
        {
            if (year < _minYear || year > MaxYear)
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

        /// <inheritdoc />
        public void ValidateOrdinal(int year, int dayOfYear, string? paramName = null)
        {
            if (year < _minYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year, paramName);
            }
            if (dayOfYear < 1
                || (dayOfYear > Solar.MinDaysInYear
                    && dayOfYear > GregorianFormulae.CountDaysInYear(year)))
            {
                Throw.DayOfYearOutOfRange(dayOfYear, paramName);
            }
        }
    }
}
