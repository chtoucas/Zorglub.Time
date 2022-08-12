// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Naked
{
    using System.Linq;

    using Zorglub.Time;
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Hemerology.Scopes;

    /// <summary>
    /// Represents a calendar with dates within a range of years.
    /// </summary>
    public sealed class MinMaxYearDayCalendar : MinMaxYearCalendar, ICalendar<DayNumber>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxYearDayCalendar"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        public MinMaxYearDayCalendar(string name, MinMaxYearScope scope) : base(name, scope) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxYearDayCalendar"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="scope"/> is not complete.</exception>
        public MinMaxYearDayCalendar(string name, CalendarScope scope) : base(name, scope) { }

        /// <summary>
        /// Obtains the current day number on this machine.
        /// </summary>
        /// <exception cref="AoorException">Today is not within the calendar boundaries.</exception>
        [Pure]
        public DayNumber Today()
        {
            // We do not know in advance if today is valid.
            var today = DayNumber.Today();
            Domain.Validate(today);
            return today;
        }

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<DayNumber> GetDaysInYear(int year)
        {
            YearsValidator.Validate(year);

            int startOfYear = Schema.GetStartOfYear(year);
            int daysInYear = Schema.CountDaysInYear(year);

            return from daysSinceEpoch
                   in Enumerable.Range(startOfYear, daysInYear)
                   select Epoch + daysSinceEpoch;
        }

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<DayNumber> GetDaysInMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);

            int startOfMonth = Schema.GetStartOfMonth(year, month);
            int daysInMonth = Schema.CountDaysInMonth(year, month);

            return from daysSinceEpoch
                   in Enumerable.Range(startOfMonth, daysInMonth)
                   select Epoch + daysSinceEpoch;
        }

        /// <inheritdoc/>
        [Pure]
        public DayNumber GetStartOfYear(int year)
        {
            YearsValidator.Validate(year);
            int daysSinceEpoch = Schema.GetStartOfYear(year);
            return Epoch + daysSinceEpoch;
        }

        /// <inheritdoc/>
        [Pure]
        public DayNumber GetEndOfYear(int year)
        {
            YearsValidator.Validate(year);
            int daysSinceEpoch = Schema.GetEndOfYear(year);
            return Epoch + daysSinceEpoch;
        }

        /// <inheritdoc/>
        [Pure]
        public DayNumber GetStartOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            int daysSinceEpoch = Schema.GetStartOfMonth(year, month);
            return Epoch + daysSinceEpoch;
        }

        /// <inheritdoc/>
        [Pure]
        public DayNumber GetEndOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            int daysSinceEpoch = Schema.GetEndOfMonth(year, month);
            return Epoch + daysSinceEpoch;
        }
    }
}
