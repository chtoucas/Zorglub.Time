// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core;

    /// <summary>
    /// Provides common adjusters for <typeparamref name="TDate"/>.
    /// <para>This class can ONLY be inherited from within friend assemblies.</para>
    /// </summary>
    /// <typeparam name="TDate">The type of date object.</typeparam>
    public abstract class MinMaxYearDateAdjusters<TDate> : IDateAdjusters<TDate>
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

        // "private protected" because the abstract method GetDate() does NOT
        // validate its parameter.
        //
        // For this class to work, an ICalendar is all we need but it seems more
        // natural to use an ICalendar<TDate>, this way we emphasize that the
        // class works for a specific type of date. In fact, we only need a
        // complete scope but it would only make sense to have a ctor with just
        // a scope if we added the ctor with an ICalendar.

        /// <summary>
        /// Initializes a new instance of the <see cref="DateAdjusters{TDate}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        /// <exception cref="ArgumentException">The scope of <paramref name="calendar"/> is NOT
        /// complete.</exception>
        private protected MinMaxYearDateAdjusters(ICalendar<TDate> calendar)
        {
            Requires.NotNull(calendar);

            var scope = calendar.Scope;
            // To avoid an unnecessary validation, a derived class is expected
            // to override GetDate(), but this can only be justified when the
            // scope is complete.
            if (scope.IsComplete == false) Throw.Argument(nameof(calendar));

            _epoch = scope.Epoch;
            _schema = scope.Schema;
        }

        /// <summary>
        /// Creates a new instance of <typeparamref name="TDate"/> from the specified count of
        /// consecutive days since the epoch.
        /// <para>This method does NOT validate its parameter.</para>
        /// </summary>
        // A default implementation is straightforward:
        // > TDate.FromDayNumber(_epoch + daysSinceEpoch);
        // but we force a derived class to provide its own version.
        // The idea is that it should not perform any validation; otherwise
        // there is no reason at all to use MinMaxYearDateAdjusters instead of
        // the default impl DateAdjusters.
        [Pure]
        protected abstract TDate GetDate(int daysSinceEpoch);

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
