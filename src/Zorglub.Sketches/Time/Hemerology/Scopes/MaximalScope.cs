// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Scopes
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;

    /// <summary>
    /// Represents the maximal scope of a schema.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class MaximalScope : ICalendarScope
    {
        /// <summary>
        /// Represents the pre-validator.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendricalPreValidator _preValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaximalScope"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        public MaximalScope(ICalendricalSchema schema, DayNumber epoch, bool onOrAfterEpoch)
        {
            Requires.NotNull(schema);

            Epoch = epoch;
            _preValidator = schema.PreValidator;

            var (minYear, maxYear) = schema.SupportedYears.Endpoints;
            if (onOrAfterEpoch) { minYear = Math.Max(minYear, 1); }

            SupportedYears = Range.CreateLeniently(minYear, maxYear);

            var minDaysSinceEpoch = schema.GetStartOfYear(minYear);
            var maxDaysSinceEpoch = schema.GetEndOfYear(maxYear);
            Domain = Range.Create(epoch + minDaysSinceEpoch, epoch + maxDaysSinceEpoch);
        }

        /// <inheritdoc />
        public DayNumber Epoch { get; }

        /// <inheritdoc />
        public Range<DayNumber> Domain { get; }

        /// <inheritdoc />
        public Range<int> SupportedYears { get; }

        /// <inheritdoc />
        public void ValidateYear(int year, string? paramName = null)
        {
            if (SupportedYears.Contains(year) == false) Throw.YearOutOfRange(year, paramName);
        }

        /// <inheritdoc />
        public void ValidateYearMonth(int year, int month, string? paramName = null)
        {
            if (SupportedYears.Contains(year) == false) Throw.YearOutOfRange(year, paramName);
            _preValidator.ValidateMonth(year, month, paramName);
        }

        /// <inheritdoc />
        public void ValidateYearMonthDay(int year, int month, int day, string? paramName = null)
        {
            if (SupportedYears.Contains(year) == false) Throw.YearOutOfRange(year, paramName);
            _preValidator.ValidateMonthDay(year, month, day, paramName);
        }

        /// <inheritdoc />
        public void ValidateOrdinal(int year, int dayOfYear, string? paramName = null)
        {
            if (SupportedYears.Contains(year) == false) Throw.YearOutOfRange(year, paramName);
            _preValidator.ValidateDayOfYear(year, dayOfYear, paramName);
        }
    }
}
