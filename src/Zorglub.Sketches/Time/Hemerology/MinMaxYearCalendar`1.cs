// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Zorglub.Time.Hemerology.Scopes;

    /// <summary>
    /// Represents a calendar with dates within a range of years.
    /// <para>This class works best when <typeparamref name="TDate"/> is based on the count of
    /// consecutive days since the epoch.</para>
    /// </summary>
    /// <typeparam name="TDate">The type of date object.</typeparam>
    public class MinMaxYearCalendar<TDate> : MinMaxYearCalendar, ICalendar<TDate>
        where TDate : IFixedDate<TDate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxYearCalendar{TDate}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        public MinMaxYearCalendar(string name, MinMaxYearScope scope) : base(name, scope) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxYearCalendar{TDate}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="scope"/> is not complete.</exception>
        public MinMaxYearCalendar(string name, CalendarScope scope) : base(name, scope) { }

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<TDate> GetDaysInYear(int year)
        {
            YearsValidator.Validate(year);

            int startOfYear = Schema.GetStartOfYear(year);
            int daysInYear = Schema.CountDaysInYear(year);

            return from daysSinceEpoch
                   in Enumerable.Range(startOfYear, daysInYear)
                   select TDate.FromDayNumber(Epoch + daysSinceEpoch);
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
                   select TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc/>
        [Pure]
        public TDate GetStartOfYear(int year)
        {
            YearsValidator.Validate(year);
            int daysSinceEpoch = Schema.GetStartOfYear(year);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc/>
        [Pure]
        public TDate GetEndOfYear(int year)
        {
            YearsValidator.Validate(year);
            int daysSinceEpoch = Schema.GetEndOfYear(year);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc/>
        [Pure]
        public TDate GetStartOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            int daysSinceEpoch = Schema.GetStartOfMonth(year, month);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc/>
        [Pure]
        public TDate GetEndOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            int daysSinceEpoch = Schema.GetEndOfMonth(year, month);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }
    }
}