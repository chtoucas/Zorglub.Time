// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Hemerology.Scopes;

    // The code works because we use TDate.FromDayNumber() which validates
    // the dayNumber, but it also makes the code unefficient when the
    // calendar is complete (the validation is unnecessary).
    //
    // Do NOT use, this class does nothing more than Hemerology.DateAdjuster.
    // Maybe it's a bit more efficient when the calendar is not complete
    // (see the validation in BoundedBelowNakedCalendar).
    // Furthemore, for date types based on a y/m/d repr, there is a better way
    // to implement IDateAdjuster; see for instance MyDate.

    public static class PlainDateAdjuster
    {
        /// <summary>
        /// Creates a new instance of the <see cref="PlainDateAdjuster{TDate}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        public static PlainDateAdjuster<TDate> Create<TDate>(ICalendar<TDate> calendar)
            where TDate : IDate<TDate>
        {
            return new(calendar?.Scope!);
        }
    }

    /// <summary>
    /// Provides a plain implementation for <see cref="IDateAdjuster{TDate}"/>.
    /// <para>This class works best when <typeparamref name="TDate"/> is based on the count of
    /// consecutive days since the epoch.</para>
    /// </summary>
    /// <typeparam name="TDate">The type of date object.</typeparam>
    public class PlainDateAdjuster<TDate> : DateAdjuster<TDate>
        // NB: we could relax the constraint on TDate and use IFixedDate<>
        // but then we would have to obtain manually the date parts (y, m, d, doy).
        where TDate : IDate<TDate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlainDateAdjuster{TDate}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        public PlainDateAdjuster(CalendarScope scope) : base(scope) { }

        /// <inheritdoc />
        [Pure]
        public sealed override TDate GetStartOfYear(TDate date)
        {
            int daysSinceEpoch = Schema.GetStartOfYear(date.Year);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override TDate GetEndOfYear(TDate date)
        {
            int daysSinceEpoch = Schema.GetEndOfYear(date.Year);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override TDate GetStartOfMonth(TDate date)
        {
            var (y, m, _) = date;
            int daysSinceEpoch = Schema.GetStartOfMonth(y, m);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override TDate GetEndOfMonth(TDate date)
        {
            var (y, m, _) = date;
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
            var (_, m, d) = date;
            AdjustYearValidate(newYear, m, d);

            var daysSinceEpoch = Schema.CountDaysSinceEpoch(newYear, m, d);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override TDate AdjustMonth(TDate date, int newMonth)
        {
            var (y, _, d) = date;
            AdjustMonthValidate(y, newMonth, d);

            var daysSinceEpoch = Schema.CountDaysSinceEpoch(y, newMonth, d);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override TDate AdjustDay(TDate date, int newDay)
        {
            var (y, m, _) = date;
            AdjustDayValidate(y, m, newDay);

            var daysSinceEpoch = Schema.CountDaysSinceEpoch(y, m, newDay);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override TDate AdjustDayOfYear(TDate date, int newDayOfYear)
        {
            int y = date.Year;
            AdjustDayOfYearValidate(y, newDayOfYear);

            var daysSinceEpoch = Schema.CountDaysSinceEpoch(y, newDayOfYear);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }
    }
}
