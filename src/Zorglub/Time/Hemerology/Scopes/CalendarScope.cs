// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Scopes
{
    using Zorglub.Time;
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Validation;

    // Range of supported years
    // - ProlepticScope     [-9998..9999]
    // - StandardScope      [1..9999]
    // - MinMaxYearScope    [minYear..maxYear]
    // - BoundedBelowScope  [minDate.Year..maxYear], the first year is not complete

    /// <summary>
    /// Defines the scope of application of a calendar, an interval of days, and provides a base for
    /// derived classes.
    /// </summary>
    public abstract partial class CalendarScope : ICalendricalValidator, ISchemaBound
    {
        /// <summary>
        /// Represents the underlying schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendricalSchema _schema;

        /// <summary>
        /// Called from constructors in derived classes to initialize the <see cref="CalendarScope"/>
        /// class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="segment"/> is null.</exception>
        protected CalendarScope(DayNumber epoch, CalendricalSegment segment)
        {
            Segment = segment ?? throw new ArgumentNullException(nameof(segment));

            _schema = segment.Schema;

            Epoch = epoch;

            DaysValidator = new DaysValidator(segment.SupportedDays);
            MonthsValidator = new MonthsValidator(segment.SupportedMonths);
            YearsValidator = new YearsValidator(segment.SupportedYears);

            Domain = DaysValidator.ToDomain(epoch);
        }

        /// <summary>
        /// Gets the epoch.
        /// </summary>
        public DayNumber Epoch { get; }

        /// <summary>
        /// Gets the range of supported <see cref="DayNumber"/> values.
        /// </summary>
        public Range<DayNumber> Domain { get; }

        /// <summary>
        /// Gets the segment of supported days.
        /// </summary>
        public CalendricalSegment Segment { get; }

        /// <summary>
        /// Gets the validator for the range of supported days.
        /// </summary>
        public DaysValidator DaysValidator { get; }

        /// <summary>
        /// Gets the validator for the range of supported months.
        /// </summary>
        public MonthsValidator MonthsValidator { get; }

        /// <summary>
        /// Gets the validator for the range of supported years.
        /// </summary>
        public YearsValidator YearsValidator { get; }

        /// <summary>
        /// Returns true if this scope is complete; otherwise returns false.
        /// <para>A segment is said to be <i>complete</i> if it spans all days of a range of years.
        /// </para>
        /// </summary>
        public bool IsComplete => Segment.IsComplete;

        /// <summary>
        /// Gets the pre-validator.
        /// </summary>
        protected ICalendricalPreValidator PreValidator => _schema.PreValidator;

        /// <summary>
        /// Gets the underlying schema.
        /// </summary>
        protected internal ICalendricalSchema Schema => _schema;

        ICalendricalSchema ISchemaBound.Schema => _schema;

        /// <inheritdoc />
        public abstract void ValidateYearMonth(int year, int month, string? paramName = null);

        /// <inheritdoc />
        public abstract void ValidateYearMonthDay(int year, int month, int day, string? paramName = null);

        /// <inheritdoc />
        public abstract void ValidateOrdinal(int year, int dayOfYear, string? paramName = null);
    }
}
