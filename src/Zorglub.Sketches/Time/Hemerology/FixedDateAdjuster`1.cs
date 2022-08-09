﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Hemerology.Scopes;

    /// <summary>
    /// Provides a default implementation for <see cref="IDateAdjuster{TDate}"/>.
    /// <para>This class works best when <typeparamref name="TDate"/> is based on the count of
    /// consecutive days since the epoch.</para>
    /// </summary>
    /// <typeparam name="TDate">The type of date object.</typeparam>
    public class FixedDateAdjuster<TDate> : DateAdjuster<TDate>
        where TDate : IFixedDate<TDate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FixedDateAdjuster{TDate}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        public FixedDateAdjuster(CalendarScope scope) : base(scope) { }

        /// <inheritdoc />
        [Pure]
        public sealed override TDate GetStartOfYear(TDate date)
        {
            var dayNumber = date.ToDayNumber();
            int y = Schema.GetYear(dayNumber - Epoch, out _);
            int daysSinceEpoch = Schema.GetStartOfYear(y);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override TDate GetEndOfYear(TDate date)
        {
            var dayNumber = date.ToDayNumber();
            int y = Schema.GetYear(dayNumber - Epoch, out _);
            int daysSinceEpoch = Schema.GetEndOfYear(y);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override TDate GetStartOfMonth(TDate date)
        {
            var dayNumber = date.ToDayNumber();
            Schema.GetDateParts(dayNumber - Epoch, out int y, out int m, out _);
            int daysSinceEpoch = Schema.GetStartOfMonth(y, m);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override TDate GetEndOfMonth(TDate date)
        {
            var dayNumber = date.ToDayNumber();
            Schema.GetDateParts(dayNumber - Epoch, out int y, out int m, out _);
            int daysSinceEpoch = Schema.GetEndOfMonth(y, m);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        //
        // Adjustments for the core parts
        //

        /// <inheritdoc />
        [Pure]
        public sealed override TDate AdjustYear(TDate date, int newYear)
        {
            var dayNumber = date.ToDayNumber();
            Schema.GetDateParts(dayNumber - Epoch, out _, out int m, out int d);
            AdjustYearValidate(newYear, m, d);

            var daysSinceEpoch = Schema.CountDaysSinceEpoch(newYear, m, d);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override TDate AdjustMonth(TDate date, int newMonth)
        {
            var dayNumber = date.ToDayNumber();
            Schema.GetDateParts(dayNumber - Epoch, out int y, out _, out int d);
            AdjustMonthValidate(y, newMonth, d);

            var daysSinceEpoch = Schema.CountDaysSinceEpoch(y, newMonth, d);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override TDate AdjustDay(TDate date, int newDay)
        {
            var dayNumber = date.ToDayNumber();
            Schema.GetDateParts(dayNumber - Epoch, out int y, out int m, out _);
            AdjustDayValidate(y, m, newDay);

            var daysSinceEpoch = Schema.CountDaysSinceEpoch(y, m, newDay);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override TDate AdjustDayOfYear(TDate date, int newDayOfYear)
        {
            var dayNumber = date.ToDayNumber();
            int y = Schema.GetYear(dayNumber - Epoch, out _);
            AdjustDayOfYearValidate(y, newDayOfYear);

            var daysSinceEpoch = Schema.CountDaysSinceEpoch(y, newDayOfYear);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }
    }
}
