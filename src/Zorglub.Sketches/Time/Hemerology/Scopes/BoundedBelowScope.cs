// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Scopes
{
    using Zorglub.Time;
    using Zorglub.Time.Core;

    /// <summary>
    /// Represents a scope with dates on or after a given date, but not the first day of a year.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class BoundedBelowScope : CalendarScope
    {
        /// <summary>
        /// Represents the earliest supported year.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _minYear;

        /// <summary>
        /// Represents the earliest supported "month".
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly MonthParts _minMonthParts;

        /// <summary>
        /// Represents the earliest supported "date".
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly DateParts _minDateParts;

        /// <summary>
        /// Represents the earliest supported ordinal "date".
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly OrdinalParts _minOrdinalParts;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundedBelowScope"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="AoorException"><paramref name="year"/> or <paramref name="maxYear"/> is
        /// outside the range of years supported by <paramref name="schema"/>.
        /// </exception>
        public BoundedBelowScope(
            ICalendricalSchema schema,
            DayNumber epoch,
            int year,
            int month,
            int day,
            int? maxYear)
            : base(schema, epoch, CalendricalSegment.Create(schema, year, month, day, maxYear))
        {
            var seg = Segment;
            _minYear = seg.SupportedYears.Min;
            _minDateParts = seg.MinMaxDateParts.LowerValue;
            _minOrdinalParts = seg.MinMaxOrdinalParts.LowerValue;
            _minMonthParts = _minDateParts.MonthParts;
        }

        /// <inheritdoc />
        public sealed override void ValidateYearMonth(int year, int month, string? paramName = null)
        {
            if (SupportedYears.Contains(year) == false) Throw.YearOutOfRange(year, paramName);
            PreValidator.ValidateMonth(year, month, paramName);

            // Tiny optimization: we first check "year".
            if (year == _minYear && new MonthParts(year, month) < _minMonthParts)
            {
                Throw.MonthOutOfRange(month, paramName);
            }
        }

        /// <inheritdoc />
        public sealed override void ValidateYearMonthDay(int year, int month, int day, string? paramName = null)
        {
            if (SupportedYears.Contains(year) == false) Throw.YearOutOfRange(year, paramName);
            PreValidator.ValidateMonthDay(year, month, day, paramName);

            // Tiny optimization: we first check "year".
            if (year == _minYear)
            {
                // We check the month parts first even if it is not necessary.
                // Reason: identify the guilty part.
                var parts = new DateParts(year, month, day);
                if (parts.MonthParts < _minMonthParts)
                {
                    Throw.MonthOutOfRange(month, paramName);
                }
                else if (parts < _minDateParts)
                {
                    Throw.DayOutOfRange(day, paramName);
                }
            }
        }

        /// <inheritdoc />
        public sealed override void ValidateOrdinal(int year, int dayOfYear, string? paramName = null)
        {
            if (SupportedYears.Contains(year) == false) Throw.YearOutOfRange(year, paramName);
            PreValidator.ValidateDayOfYear(year, dayOfYear, paramName);

            // Tiny optimization: we first check "year".
            if (year == _minYear && new OrdinalParts(year, dayOfYear) < _minOrdinalParts)
            {
                Throw.DayOfYearOutOfRange(dayOfYear, paramName);
            }
        }
    }
}
