// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Naked
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Hemerology.Scopes;

    /// <summary>
    /// Represents a calendar with dates within a range of years.
    /// </summary>
    public partial class MinMaxYearNakedCalendar : MinMaxYearCalendar, INakedCalendar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxYearCalendar"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        public MinMaxYearNakedCalendar(string name, MinMaxYearScope scope) : base(name, scope)
        {
            PartsAdapter = new PartsAdapter(Schema);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxYearNakedCalendar"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="scope"/> is not complete.</exception>
        public MinMaxYearNakedCalendar(string name, CalendarScope scope) : base(name, scope)
        {
            PartsAdapter = new PartsAdapter(Schema);
        }

        /// <summary>
        /// Gets the adapter for calendrical parts.
        /// </summary>
        protected PartsAdapter PartsAdapter { get; }
    }

    public partial class MinMaxYearNakedCalendar // Factories, conversions
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

    public partial class MinMaxYearNakedCalendar // Dates in a given year or month
    {
        /// <inheritdoc />
        [Pure]
        public IEnumerable<DateParts> GetDaysInYear(int year)
        {
            // Check arg eagerly.
            YearsValidator.Validate(year);

            return Iterator();

            IEnumerable<DateParts> Iterator()
            {
                int monthsInYear = Schema.CountMonthsInYear(year);

                for (int m = 1; m <= monthsInYear; m++)
                {
                    int daysInMonth = Schema.CountDaysInMonth(year, m);

                    for (int d = 1; d <= daysInMonth; d++)
                    {
                        yield return new DateParts(year, m, d);
                    }
                }
            }
        }

        /// <inheritdoc />
        [Pure]
        public IEnumerable<DateParts> GetDaysInMonth(int year, int month)
        {
            // Check arg eagerly.
            Scope.ValidateYearMonth(year, month);

            return Iterator();

            IEnumerable<DateParts> Iterator()
            {
                int daysInMonth = Schema.CountDaysInMonth(year, month);

                for (int d = 1; d <= daysInMonth; d++)
                {
                    yield return new DateParts(year, month, d);
                }
            }
        }

        /// <inheritdoc />
        [Pure]
        public DateParts GetStartOfYear(int year)
        {
            YearsValidator.Validate(year);
            return DateParts.AtStartOfYear(year);
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
            return DateParts.AtStartOfMonth(year, month);
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
