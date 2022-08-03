﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Scopes
{
    using Zorglub.Time;
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Validation;

    /// <summary>
    /// Represents a scope for a calendar supporting <i>all</i> dates within a range of years.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class MinMaxYearScope : CalendarScope
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxYearScope"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="supportedYears"/> is NOT a
        /// subinterval of the range of supported years by <paramref name="schema"/>.</exception>
        public MinMaxYearScope(ICalendricalSchema schema, DayNumber epoch, Range<int> supportedYears)
            : base(epoch, CalendricalSegment.Create(schema, supportedYears)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxYearScope"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="segment"/> is null.</exception>
        private MinMaxYearScope(DayNumber epoch, CalendricalSegment segment)
            : base(epoch, segment) { }

        #region Factories

        /// <summary>
        /// Creates the default maximal scope for the specified schema and epoch.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of supported years by the schema
        /// does not contain the year 1 and <paramref name="onOrAfterEpoch"/> is equal to true.
        /// </exception>
        [Pure]
        public static MinMaxYearScope WithMaximalRange(
            ICalendricalSchema schema, DayNumber epoch, bool onOrAfterEpoch = false)
        {
            var seg = CalendricalSegment.CreateMaximal(schema, onOrAfterEpoch);

            return new MinMaxYearScope(epoch, seg);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="MinMaxYearScope"/> class with dates on or after
        /// the specified year.
        /// </summary>
        [Pure]
        public static MinMaxYearScope WithMinYear(
            ICalendricalSchema schema, DayNumber epoch, int minYear)
        {
            var builder = new CalendricalSegmentBuilder(schema);
            builder.SetMinToStartOfYear(minYear);
            builder.SetMaxToEndOfMaxSupportedYear();
            var segment = builder.BuildSegment();

            return new MinMaxYearScope(epoch, segment);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="MinMaxYearScope"/> class with dates on or
        /// before the specified year.
        /// </summary>
        [Pure]
        public static MinMaxYearScope WithMaxYear(
            ICalendricalSchema schema, DayNumber epoch, int maxYear)
        {
            var builder = new CalendricalSegmentBuilder(schema);
            builder.SetMinToStartOfMinSupportedYear();
            builder.SetMaxToEndOfYear(maxYear);
            var segment = builder.BuildSegment();

            return new MinMaxYearScope(epoch, segment);
        }

        #endregion

        /// <inheritdoc />
        public sealed override void ValidateYearMonth(int year, int month, string? paramName = null)
        {
            YearsValidator.Validate(year, paramName);
            PreValidator.ValidateMonth(year, month, paramName);
        }

        /// <inheritdoc />
        public sealed override void ValidateYearMonthDay(int year, int month, int day, string? paramName = null)
        {
            YearsValidator.Validate(year, paramName);
            PreValidator.ValidateMonthDay(year, month, day, paramName);
        }

        /// <inheritdoc />
        public sealed override void ValidateOrdinal(int year, int dayOfYear, string? paramName = null)
        {
            YearsValidator.Validate(year, paramName);
            PreValidator.ValidateDayOfYear(year, dayOfYear, paramName);
        }
    }
}
