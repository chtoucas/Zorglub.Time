// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Hemerology.Scopes;

    // REVIEW(code): this adjuster does not throw the expected arg name.

    /// <summary>
    /// Provides a plain implementation for <see cref="IDateAdjuster{TDate}"/>.
    /// </summary>
    /// <typeparam name="TDate">The type of date object.</typeparam>
    public class DateAdjuster<TDate, TCalendar> : IDateAdjuster<TDate>
        where TDate : IDateable
        where TCalendar : ICalendar<TDate>, IDateFactory<TDate>
    {
        /// <summary>
        /// Represents the calendar.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly TCalendar _calendar;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateAdjusterV0{TDate}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        public DateAdjuster(TCalendar calendar)
        {
            _calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
        }

        /// <inheritdoc />
        public CalendarScope Scope => _calendar.Scope;

        /// <inheritdoc />
        [Pure]
        public TDate GetStartOfYear(TDate date) => _calendar.GetStartOfYear(date.Year);

        /// <inheritdoc />
        [Pure]
        public TDate GetEndOfYear(TDate date) => _calendar.GetEndOfYear(date.Year);

        /// <inheritdoc />
        [Pure]
        public TDate GetStartOfMonth(TDate date)
        {
            var (y, m, _) = date;
            return _calendar.GetStartOfMonth(y, m);
        }

        /// <inheritdoc />
        [Pure]
        public TDate GetEndOfMonth(TDate date)
        {
            var (y, m, _) = date;
            return _calendar.GetEndOfMonth(y, m);
        }

        //
        // Adjustments for the core parts
        //

        /// <inheritdoc />
        [Pure]
        public TDate AdjustYear(TDate date, int newYear)
        {
            var (_, m, d) = date;
            return _calendar.GetDate(newYear, m, d);
        }

        /// <inheritdoc />
        [Pure]
        public TDate AdjustMonth(TDate date, int newMonth)
        {
            var (y, _, d) = date;
            return _calendar.GetDate(y, newMonth, d);
        }

        /// <inheritdoc />
        [Pure]
        public TDate AdjustDay(TDate date, int newDay)
        {
            var (y, m, _) = date;
            return _calendar.GetDate(y, m, newDay);
        }

        /// <inheritdoc />
        [Pure]
        public TDate AdjustDayOfYear(TDate date, int newDayOfYear) =>
            _calendar.GetDate(date.Year, newDayOfYear);
    }
}
