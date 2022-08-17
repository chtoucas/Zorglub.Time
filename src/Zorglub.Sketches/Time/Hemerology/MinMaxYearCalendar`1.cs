// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using System.Linq;

    using Zorglub.Time.Hemerology.Scopes;

    /// <summary>
    /// Represents a calendar with dates within a range of years.
    /// </summary>
    /// <typeparam name="TDate">The type of date object.</typeparam>
    public abstract class MinMaxYearCalendar<TDate> : MinMaxYearBasicCalendar, ICalendar<TDate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxYearCalendar{TDate}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        protected MinMaxYearCalendar(string name, MinMaxYearScope scope) : base(name, scope) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxYearCalendar{TDate}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="scope"/> is not complete.</exception>
        protected MinMaxYearCalendar(string name, CalendarScope scope) : base(name, scope) { }

        /// <summary>
        /// Creates a new instance of <typeparamref name="TDate"/> from the specified count of
        /// consecutive days since the epoch.
        /// </summary>
        [Pure]
        protected abstract TDate GetDate(int daysSinceEpoch);

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<TDate> GetDaysInYear(int year)
        {
            YearsValidator.Validate(year);

            int startOfYear = Schema.GetStartOfYear(year);
            int daysInYear = Schema.CountDaysInYear(year);

            return from daysSinceEpoch
                   in Enumerable.Range(startOfYear, daysInYear)
                   select GetDate(daysSinceEpoch);
        }

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<TDate> GetDaysInMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);

            int startOfMonth = Schema.GetStartOfMonth(year, month);
            int daysInMonth = Schema.CountDaysInMonth(year, month);

            return from daysSinceEpoch
                   in Enumerable.Range(startOfMonth, daysInMonth)
                   select GetDate(daysSinceEpoch);
        }

        /// <inheritdoc/>
        [Pure]
        public TDate GetStartOfYear(int year)
        {
            YearsValidator.Validate(year);
            int daysSinceEpoch = Schema.GetStartOfYear(year);
            return GetDate(daysSinceEpoch);
        }

        /// <inheritdoc/>
        [Pure]
        public TDate GetEndOfYear(int year)
        {
            YearsValidator.Validate(year);
            int daysSinceEpoch = Schema.GetEndOfYear(year);
            return GetDate(daysSinceEpoch);
        }

        /// <inheritdoc/>
        [Pure]
        public TDate GetStartOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            int daysSinceEpoch = Schema.GetStartOfMonth(year, month);
            return GetDate(daysSinceEpoch);
        }

        /// <inheritdoc/>
        [Pure]
        public TDate GetEndOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            int daysSinceEpoch = Schema.GetEndOfMonth(year, month);
            return GetDate(daysSinceEpoch);
        }
    }
}