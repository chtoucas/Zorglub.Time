// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Scopes
{
    using Zorglub.Time;
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Hemerology;

    /// <summary>
    /// Represents the scope of a schema, an interval of days, and provides a base for derived
    /// classes.
    /// </summary>
    public abstract partial class CalendarScope : ICalendarScope
    {
        /// <summary>
        /// Called from constructors in derived classes to initialize the
        /// <see cref="CalendarScope"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="segment"/> is null.</exception>
        protected CalendarScope(
            ICalendricalSchema schema,
            DayNumber epoch,
            CalendricalSegment segment)
        {
            Schema = schema ?? throw new ArgumentNullException(nameof(schema));

            Requires.NotNull(segment);
            if (ReferenceEquals(segment.Schema, schema) == false) Throw.Argument(nameof(segment));
            Segment = segment;

            Epoch = epoch;
            PreValidator = schema.PreValidator;
            SupportedYears = segment.SupportedYears;
            Domain = segment.GetFixedDomain(epoch);
        }

        /// <inheritdoc />
        public DayNumber Epoch { get; }

        /// <inheritdoc />
        public Range<DayNumber> Domain { get; }

        /// <inheritdoc />
        public Range<int> SupportedYears { get; }

        /// <inheritdoc />
        public CalendricalSegment Segment { get; }

        /// <summary>
        /// Gets the pre-validator.
        /// </summary>
        protected ICalendricalPreValidator PreValidator { get; }

        /// <summary>
        /// Gets the underlying schema.
        /// </summary>
        protected internal ICalendricalSchema Schema { get; }

        /// <inheritdoc />
        public void ValidateYear(int year, string? paramName = null)
        {
            if (SupportedYears.Contains(year) == false) Throw.YearOutOfRange(year, paramName);
        }

        /// <inheritdoc />
        public abstract void ValidateYearMonth(int year, int month, string? paramName = null);

        /// <inheritdoc />
        public abstract void ValidateYearMonthDay(int year, int month, int day, string? paramName = null);

        /// <inheritdoc />
        public abstract void ValidateOrdinal(int year, int dayOfYear, string? paramName = null);
    }
}
