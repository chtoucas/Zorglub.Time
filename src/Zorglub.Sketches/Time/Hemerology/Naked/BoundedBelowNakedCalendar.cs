// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Naked
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Hemerology.Scopes;

    /// <summary>
    /// Represents a calendar with dates on or after a given date.
    /// <para>The aforementioned date can NOT be the start of a year.</para>
    /// </summary>
    public partial class BoundedBelowNakedCalendar : BoundedBelowCalendar, INakedCalendar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoundedBelowNakedCalendar"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        public BoundedBelowNakedCalendar(string name, BoundedBelowScope scope) : base(name, scope)
        {
            PartsAdapter = new PartsAdapter(Schema);
        }

        /// <summary>
        /// Gets the adapter for calendrical parts.
        /// </summary>
        protected PartsAdapter PartsAdapter { get; }
    }

    public partial class BoundedBelowNakedCalendar // Factories, conversions
    {
        /// <inheritdoc />
        [Pure]
        public DateParts Today() => GetDateParts(DayNumber.Today());

        /// <inheritdoc />
        [Pure]
        public DateParts GetDateParts(DayNumber dayNumber)
        {
            Domain.Validate(dayNumber);
            return PartsAdapter.GetDateParts(dayNumber - Epoch);
        }

        /// <inheritdoc />
        [Pure]
        public DateParts GetDateParts(int year, int dayOfYear)
        {
            Scope.ValidateOrdinal(year, dayOfYear);
            return PartsAdapter.GetDateParts(year, dayOfYear);
        }

        /// <inheritdoc />
        [Pure]
        public OrdinalParts GetOrdinalParts(DayNumber dayNumber)
        {
            Domain.Validate(dayNumber);
            return PartsAdapter.GetOrdinalParts(dayNumber - Epoch);
        }

        /// <inheritdoc />
        [Pure]
        public OrdinalParts GetOrdinalParts(int year, int month, int day)
        {
            Scope.ValidateYearMonthDay(year, month, day);
            return PartsAdapter.GetOrdinalParts(year, month, day);
        }
    }

    public partial class BoundedBelowNakedCalendar // Dates in a given year or month
    {
        /// <inheritdoc />
        [Pure]
        public IEnumerable<DateParts> GetDaysInYear(int year)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        [Pure]
        public IEnumerable<DateParts> GetDaysInMonth(int year, int month)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        [Pure]
        public DateParts GetStartOfYear(int year)
        {
            YearsValidator.Validate(year);
            return year == MinYear
                ? Throw.ArgumentOutOfRange<DateParts>(nameof(year))
                : DateParts.AtStartOfYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public DateParts GetEndOfYear(int year)
        {
            YearsValidator.Validate(year);
            return PartsAdapter.GetDatePartsAtEndOfYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public DateParts GetStartOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return new MonthParts(year, month) == MinMonthParts
                ? Throw.ArgumentOutOfRange<DateParts>(nameof(month))
                : DateParts.AtStartOfMonth(year, month);
        }

        /// <inheritdoc />
        [Pure]
        public DateParts GetEndOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return PartsAdapter.GetDatePartsAtEndOfMonth(year, month);
        }
    }
}
