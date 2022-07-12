﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Validation
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
        /// Represents the range of supported years.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly SupportedYears _supportedYears;

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
            Segment = CalendricalSegment.Create(schema, supportedYears);
            _supportedYears = Segment.SupportedYears;
        }

        /// <summary>
        /// Gets the segment of supported days.
        /// </summary>
        public CalendricalSegment Segment { get; }

        ICalendricalSchema ISchemaBound.Schema => _schema;

        /// <inheritdoc />
        public void ValidateYearMonth(int year, int month, string? paramName = null)
        {
            _supportedYears.Validate(year, paramName);
            _preValidator.ValidateMonth(year, month, paramName);
        }

        /// <inheritdoc />
        public void ValidateYearMonthDay(int year, int month, int day, string? paramName = null)
        {
            _supportedYears.Validate(year, paramName);
            _preValidator.ValidateMonthDay(year, month, day, paramName);
        }

        /// <inheritdoc />
        public void ValidateOrdinal(int year, int dayOfYear, string? paramName = null)
        {
            _supportedYears.Validate(year, paramName);
            _preValidator.ValidateDayOfYear(year, dayOfYear, paramName);
        }
    }
}
