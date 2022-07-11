// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using Zorglub.Time.Core.Intervals;

    /// <summary>
    /// Provides a plain implementation for <see cref="ICalendricalValidator"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class CalendricalValidator : ICalendricalValidator, ISchemaBound
    {
        /// <summary>
        /// Represents the underlying schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendricalSchema _schema;

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
            _schema = schema ?? throw new ArgumentNullException(nameof(schema));

            _preValidator = schema.PreValidator;

            SupportedYears = supportedYears;
            Segment = CalendricalSegment.Create(schema, supportedYears);
        }

        /// <summary>
        /// Gets the range of supported years.
        /// </summary>
        public Range<int> SupportedYears { get; }

        /// <summary>
        /// Gets the range of supported days.
        /// </summary>
        public AffineDomain AffineDomain => Segment.AffineDomain;

        /// <summary>
        /// Gets the range of supported months.
        /// </summary>
        public MonthDomain MonthDomain => Segment.MonthDomain;

        /// <summary>
        /// Gets the segment of supported days.
        /// </summary>
        public CalendricalSegment Segment { get; }

        ICalendricalSchema ISchemaBound.Schema => _schema;

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
