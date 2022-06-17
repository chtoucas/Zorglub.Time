// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;

    /// <summary>
    /// Represents the scope of a schema, an interval of days, and provides a base for derived
    /// classes.
    /// <para>This class can only represent subintervals of <see cref="Yemoda.SupportedYears"/>.
    /// </para>
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

            Epoch = epoch;

            PreValidator = schema.PreValidator;

            MinMaxDateParts = segment.MinMaxDateParts;
            MinMaxOrdinalParts = segment.MinMaxOrdinalParts;

            SupportedYears = segment.SupportedYears;
            (MinYear, MaxYear) = segment.SupportedYears.Endpoints;

            Domain = Range.FromEndpoints(
                from daysSinceEpoch in segment.Domain.Endpoints select epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        public DayNumber Epoch { get; }

        /// <inheritdoc />
        public Range<DayNumber> Domain { get; }

        /// <inheritdoc />
        public Range<int> SupportedYears { get; }

        /// <summary>
        /// Gets the pair of earliest and latest supported date parts.
        /// </summary>
        public OrderedPair<Yemoda> MinMaxDateParts { get; }

        /// <summary>
        /// Gets the pair of earliest and latest supported ordinal date parts.
        /// </summary>
        public OrderedPair<Yedoy> MinMaxOrdinalParts { get; }

        /// <summary>
        /// Gets the earliest supported year.
        /// </summary>
        protected int MinYear { get; }

        /// <summary>
        /// Gets the latest supported year.
        /// </summary>
        protected int MaxYear { get; }

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
            if (year < MinYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year, paramName);
            }
        }

        /// <inheritdoc />
        public abstract void ValidateYearMonth(int year, int month, string? paramName = null);

        /// <inheritdoc />
        public abstract void ValidateYearMonthDay(int year, int month, int day, string? paramName = null);

        /// <inheritdoc />
        public abstract void ValidateOrdinal(int year, int dayOfYear, string? paramName = null);
    }
}
