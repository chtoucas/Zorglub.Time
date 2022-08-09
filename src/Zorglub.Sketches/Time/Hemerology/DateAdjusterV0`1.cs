﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Hemerology.Scopes;

    // This adjuster is "problematic" as it does not throw the expected arg exn.

    /// <summary>
    /// Provides a default implementation for <see cref="IDateAdjuster{TDate}"/>.
    /// </summary>
    /// <typeparam name="TDate">The type of date object.</typeparam>
    [Obsolete("Use DateAdjuster instead.")]
    public class DateAdjusterV0<TDate> : IDateAdjuster<TDate>
        where TDate : IDate<TDate>
    {
        /// <summary>
        /// Represents the calendar.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendar<TDate> _calendar;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateAdjusterV0{TDate}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        public DateAdjusterV0(ICalendar<TDate> calendar)
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
            var dayNumber = _calendar.GetDayNumber(newYear, m, d);
            return TDate.FromDayNumber(dayNumber);
        }

        /// <inheritdoc />
        [Pure]
        public TDate AdjustMonth(TDate date, int newMonth)
        {
            var (y, _, d) = date;
            var dayNumber = _calendar.GetDayNumber(y, newMonth, d);
            return TDate.FromDayNumber(dayNumber);
        }

        /// <inheritdoc />
        [Pure]
        public TDate AdjustDay(TDate date, int newDay)
        {
            var (y, m, _) = date;
            var dayNumber = _calendar.GetDayNumber(y, m, newDay);
            return TDate.FromDayNumber(dayNumber);
        }

        /// <inheritdoc />
        [Pure]
        public TDate AdjustDayOfYear(TDate date, int newDayOfYear)
        {
            var y = date.Year;
            var dayNumber = _calendar.GetDayNumber(y, newDayOfYear);
            return TDate.FromDayNumber(dayNumber);
        }
    }
}