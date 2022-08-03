// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Schemas;

    // Do not use this class for date types based on a y/m/d repr. For them,
    // there is a better way to implement IDateAdjusters; see for instance
    // MyDate.

    // REVIEW(api): use prop DaysSinceEpoch? not yet possible.
    // I have serious doubts about GetDate(); see also MinMaxYearCalendar<>.

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
        /// Represents the Julian schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendricalSchema _schema;

        /// <summary>
        /// Represents the epoch.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly DayNumber _epoch;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateAdjusters{TDate}"/> class.
        /// </summary>
        public DateAdjusters(DayNumber epoch, ICalendricalSchema schema)
        {
            Requires.NotNull(schema);

            _epoch = epoch;
            _schema = schema;
        }

        /// <summary>
        /// Creates a new instance of <typeparamref name="TDate"/> from the specified count of
        /// consecutive days since the epoch.
        /// </summary>
        [Pure]
        protected virtual TDate GetDate(int daysSinceEpoch) =>
            TDate.FromDayNumber(_epoch + daysSinceEpoch);

        /// <inheritdoc />
        [Pure]
        public TDate GetStartOfYear(TDate date)
        {
            int daysSinceEpoch = _schema.GetStartOfYear(date.Year);
            return GetDate(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate GetEndOfYear(TDate date)
        {
            int daysSinceEpoch = _schema.GetEndOfYear(date.Year);
            return GetDate(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate GetStartOfMonth(TDate date)
        {
            var dayNumber = date.ToDayNumber();
            _schema.GetDateParts(dayNumber - _epoch, out int y, out int m, out _);
            int daysSinceEpoch = _schema.GetStartOfMonth(y, m);
            return GetDate(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate GetEndOfMonth(TDate date)
        {
            var dayNumber = date.ToDayNumber();
            _schema.GetDateParts(dayNumber - _epoch, out int y, out int m, out _);
            int daysSinceEpoch = _schema.GetEndOfMonth(y, m);
            return GetDate(daysSinceEpoch);
        }
    }
}
