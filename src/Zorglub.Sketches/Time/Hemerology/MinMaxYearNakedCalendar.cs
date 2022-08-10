// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Hemerology.Scopes;

    /// <summary>
    /// Represents a calendar with dates within a range of years.
    /// </summary>
    public partial class MinMaxYearNakedCalendar : NakedCalendar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxYearCalendar"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        public MinMaxYearNakedCalendar(string name, MinMaxYearScope scope) : base(name, scope)
        {
            DayCalendar = new MinMaxYearDayCalendar(name, scope);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxYearNakedCalendar"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="scope"/> is not complete.</exception>
        public MinMaxYearNakedCalendar(string name, CalendarScope scope) : base(name, scope)
        {
            DayCalendar = new MinMaxYearDayCalendar(name, scope);
        }

        /// <summary>
        /// Gets a provider for day numbers in a year or a month.
        /// </summary>
        public MinMaxYearDayCalendar DayCalendar { get; }
    }

    public partial class MinMaxYearNakedCalendar // Year, month, day infos
    {
        /// <inheritdoc />
        [Pure]
        public sealed override int CountMonthsInYear(int year)
        {
            YearsValidator.Validate(year);
            return Schema.CountMonthsInYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysInYear(int year)
        {
            YearsValidator.Validate(year);
            return Schema.CountDaysInYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysInMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return Schema.CountDaysInMonth(year, month);
        }
    }

    public partial class MinMaxYearNakedCalendar // Dates in a given year or month
    {
        /// <inheritdoc />
        [Pure]
        public sealed override IEnumerable<DateParts> GetDaysInYear(int year)
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
        public sealed override IEnumerable<DateParts> GetDaysInMonth(int year, int month)
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
        public sealed override DateParts GetStartOfYear(int year)
        {
            YearsValidator.Validate(year);
            return DateParts.AtStartOfYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override DateParts GetEndOfYear(int year)
        {
            YearsValidator.Validate(year);
            return PartsAdapter.GetDatePartsAtEndOfYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override DateParts GetStartOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return DateParts.AtStartOfMonth(year, month);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override DateParts GetEndOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return PartsAdapter.GetDatePartsAtEndOfMonth(year, month);
        }
    }
}
