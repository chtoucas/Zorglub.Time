// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core;

    // Do not use this class for date types based on a y/m/d repr. For them,
    // there is a better way to implement IDateAdjusters; see for instance
    // MyDate.

    /// <summary>
    /// Provides a default implementation for <see cref="IDateAdjusters{TDate}"/>.
    /// <para>This class works best when <typeparamref name="TDate"/> is based on the count of
    /// consecutive days since the epoch.</para>
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    /// <typeparam name="TDate">The type of date object.</typeparam>
    public class DateAdjusters<TDate> : IDateAdjusters<TDate>
        where TDate : IDate<TDate>
    {
        /// <summary>
        /// Represents the schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendricalSchema _schema;

        /// <summary>
        /// Represents the epoch.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly DayNumber _epoch;

        // FIXME(api): ctor should use a calendar ICalendar<TDate>.
        // The code works because we use TDate.FromDayNumber() that validates
        // the dayNumber, but it also makes the code unefficient when the
        // calendar is complete (the validation is unnecessary); see
        // MinMaxYearCalendar<T>. Maybe we could restrict ourselves to complete
        // calendars.

        /// <summary>
        /// Initializes a new instance of the <see cref="DateAdjusters{TDate}"/> class.
        /// </summary>
        public DateAdjusters(ICalendar<TDate> calendar)
        {
            Requires.NotNull(calendar);

            var chr = calendar as ISchemaBound;
            if (chr is null) Throw.Argument(nameof(calendar));
            _schema = chr.Schema;

            _epoch = calendar.Epoch;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateAdjusters{TDate}"/> class.
        /// </summary>
        public DateAdjusters(DayNumber epoch, ICalendricalSchema schema)
        {
            Requires.NotNull(schema);

            _epoch = epoch;
            _schema = schema;
        }

        /// <inheritdoc />
        [Pure]
        public TDate GetStartOfYear(TDate date)
        {
            int daysSinceEpoch = _schema.GetStartOfYear(date.Year);
            return TDate.FromDayNumber(_epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate GetEndOfYear(TDate date)
        {
            int daysSinceEpoch = _schema.GetEndOfYear(date.Year);
            return TDate.FromDayNumber(_epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate GetStartOfMonth(TDate date)
        {
            var dayNumber = date.ToDayNumber();
            _schema.GetDateParts(dayNumber - _epoch, out int y, out int m, out _);
            int daysSinceEpoch = _schema.GetStartOfMonth(y, m);
            return TDate.FromDayNumber(_epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate GetEndOfMonth(TDate date)
        {
            var dayNumber = date.ToDayNumber();
            _schema.GetDateParts(dayNumber - _epoch, out int y, out int m, out _);
            int daysSinceEpoch = _schema.GetEndOfMonth(y, m);
            return TDate.FromDayNumber(_epoch + daysSinceEpoch);
        }
    }
}
