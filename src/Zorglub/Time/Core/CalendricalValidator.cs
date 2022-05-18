// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using Zorglub.Time.Core.Intervals;

    /// <summary>
    /// Provides a plain implementation for <see cref="ICalendricalValidator"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class CalendricalValidator : ICalendricalValidator
    {
        /// <summary>
        /// Represents the earliest supported year.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _minYear;

        /// <summary>
        /// Represents the latest supported year.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _maxYear;

        private readonly int _minDaysSinceEpoch;
        private readonly int _maxDaysSinceEpoch;

        /// <summary>
        /// Represents the pre-validator.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendricalPreValidator _preValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendricalValidator"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="AoorException"><paramref name="supportedYears"/> is NOT a subinterval
        /// of the range of supported years by <paramref name="schema"/>.</exception>
        public CalendricalValidator(ICalendricalSchema schema, Range<int> supportedYears)
        {
            Requires.NotNull(schema);
            if (supportedYears.IsSubsetOf(schema.SupportedYears) == false)
            {
                Throw.ArgumentOutOfRange(nameof(supportedYears));
            }

            SupportedYears = supportedYears;
            (_minYear, _maxYear) = supportedYears.Endpoints;
            _preValidator = schema.PreValidator;

            (_minDaysSinceEpoch, _maxDaysSinceEpoch) =
                supportedYears.Endpoints.Select(schema.GetStartOfYear, schema.GetEndOfYear);
        }

        /// <inheritdoc />
        public Range<int> SupportedYears { get; }

        /// <inheritdoc />
        public void ValidateYear(int year, string? paramName = null)
        {
            if (year < _minYear || year > _maxYear) Throw.YearOutOfRange(year, paramName);
        }

        /// <inheritdoc />
        public void ValidateYearMonth(int year, int month, string? paramName = null)
        {
            if (year < _minYear || year > _maxYear) Throw.YearOutOfRange(year, paramName);
            _preValidator.ValidateMonth(year, month, paramName);
        }

        /// <inheritdoc />
        public void ValidateYearMonthDay(int year, int month, int day, string? paramName = null)
        {
            if (year < _minYear || year > _maxYear) Throw.YearOutOfRange(year, paramName);
            _preValidator.ValidateMonthDay(year, month, day, paramName);
        }

        /// <inheritdoc />
        public void ValidateOrdinal(int year, int dayOfYear, string? paramName = null)
        {
            if (year < _minYear || year > _maxYear) Throw.YearOutOfRange(year, paramName);
            _preValidator.ValidateDayOfYear(year, dayOfYear, paramName);
        }

        /// <summary>
        /// Validates the specified number of consecutive days from the epoch.
        /// </summary>
        /// <exception cref="AoorException">The validation failed.</exception>
        public void ValidateDaysSinceEpoch(int daysSinceEpoch)
        {
            if (daysSinceEpoch < _minDaysSinceEpoch || daysSinceEpoch > _maxDaysSinceEpoch)
            {
                Throw.ArgumentOutOfRange(nameof(daysSinceEpoch));
            }
        }
    }
}
