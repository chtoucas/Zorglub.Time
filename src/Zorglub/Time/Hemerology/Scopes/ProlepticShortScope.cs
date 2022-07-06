// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Scopes
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Schemas;

    /// <summary>
    /// Represents the proleptic short scope of a schema.
    /// <para>A proleptic scope supports dates within the interval [-9998..9999] of years.</para>
    /// </summary>
    /// <remarks>
    /// <para>This is the scope used by <see cref="Simple.Calendar"/> in the Gregorian and
    /// Julian cases.</para>
    /// </remarks>
    public sealed class ProlepticShortScope : ICalendarScope
    {
        /// <summary>
        /// Represents the earliest supported year.
        /// <para>This field is a constant equal to -9998.</para>
        /// </summary>
        public const int MinYear = -9998;

        /// <summary>
        /// Represents the latest supported year.
        /// <para>This field is a constant equal to 9999.</para>
        /// </summary>
        public const int MaxYear = 9999;

        /// <summary>
        /// Represents the pre-validator.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendricalPreValidator _preValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProlepticShortScope"/> class with the
        /// specified schema and epoch.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> does not contain the interval [-9998..9999].</exception>
        public ProlepticShortScope(ICalendricalSchema schema, DayNumber epoch)
        {
            Requires.NotNull(schema);

            // NB: don't write
            // > if (minYear < schema.SupportedYears.Min) Throw.ArgumentOutOfRange(nameof(minYear));
            // > if (schema.SupportedYears.Max < MaxYear) Throw.Argument(nameof(schema));
            // This class is internal and the derived classes have a fixed min year,
            // which means that the culprit will be the schema not the specified minYear.
            var range = Range.Create(MinYear, MaxYear);
            if (range.IsSubsetOf(schema.SupportedYears) == false) Throw.Argument(nameof(schema));

            Epoch = epoch;
            _preValidator = schema.PreValidator;

            SupportedYears = Range.CreateLeniently(MinYear, MaxYear);

            var minDaysSinceEpoch = schema.GetStartOfYear(MinYear);
            var maxDaysSinceEpoch = schema.GetEndOfYear(MaxYear);

            Domain = Range.CreateLeniently(epoch + minDaysSinceEpoch, epoch + maxDaysSinceEpoch);
        }

        /// <inheritdoc />
        public DayNumber Epoch { get; }

        /// <inheritdoc />
        public Range<DayNumber> Domain { get; }

        /// <inheritdoc />
        public Range<int> SupportedYears { get; }

        /// <summary>
        /// Gets the checker for overflows of the range of years.
        /// </summary>
        internal static IOverflowChecker<int> YearOverflowChecker { get; } = new YearOverflowChecker_();

        /// <inheritdoc />
        public void ValidateYear(int year, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year, paramName);
            }
        }

        /// <inheritdoc />
        public void ValidateYearMonth(int year, int month, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year, paramName);
            }
            _preValidator.ValidateMonth(year, month, paramName);
        }

        /// <inheritdoc />
        public void ValidateYearMonthDay(int year, int month, int day, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year, paramName);
            }
            _preValidator.ValidateMonthDay(year, month, day, paramName);
        }

        /// <inheritdoc />
        public void ValidateOrdinal(int year, int dayOfYear, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year, paramName);
            }
            _preValidator.ValidateDayOfYear(year, dayOfYear, paramName);
        }

        private sealed class YearOverflowChecker_ : IOverflowChecker<int>
        {
            public void Check(int year)
            {
                if (year < MinYear || year > MaxYear) Throw.DateOverflow();
            }

            public void CheckUpperBound(int year)
            {
                if (year > MaxYear) Throw.DateOverflow();
            }

            public void CheckLowerBound(int year)
            {
                if (year < MinYear) Throw.DateOverflow();
            }
        }
    }
}
