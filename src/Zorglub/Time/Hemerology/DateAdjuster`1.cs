// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    /// <summary>
    /// Provides a default implementation for <see cref="IDateAdjuster{TDate}"/>.
    /// </summary>
    /// <typeparam name="TDate">The type of date object.</typeparam>
    public class DateAdjuster<TDate> : IDateAdjuster<TDate> where TDate : IDateable
    {
        /// <summary>
        /// Represents the calendar.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendar<TDate> _calendar;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateAdjuster{TDate}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        public DateAdjuster(ICalendar<TDate> calendar)
        {
            _calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
        }

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
    }
}
