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
            var seg = CalendricalSection.Create(schema, supportedYears);

            Debug.Assert(schema != null);
            _preValidator = schema.PreValidator;

            SupportedYears = supportedYears;
            Domain = seg.Domain;
            MonthDomain = seg.MonthDomain;
        }

        /// <inheritdoc />
        public Range<int> SupportedYears { get; }

        public Range<int> Domain { get; }

        public Range<int> MonthDomain { get; }

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

        /// <summary>
        /// Validates the specified number of consecutive months from the epoch.
        /// </summary>
        /// <exception cref="AoorException">The validation failed.</exception>
        public void ValidateMonthsSinceEpoch(int monthsSinceEpoch)
        {
            if (MonthDomain.Contains(monthsSinceEpoch) == false)
            {
                Throw.ArgumentOutOfRange(nameof(monthsSinceEpoch));
            }
        }

        /// <summary>
        /// Validates the specified number of consecutive days from the epoch.
        /// </summary>
        /// <exception cref="AoorException">The validation failed.</exception>
        public void ValidateDaysSinceEpoch(int daysSinceEpoch)
        {
            if (Domain.Contains(daysSinceEpoch) == false)
            {
                Throw.ArgumentOutOfRange(nameof(daysSinceEpoch));
            }
        }
    }
}
