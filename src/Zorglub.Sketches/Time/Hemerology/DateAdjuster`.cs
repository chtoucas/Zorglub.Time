// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time;
    using Zorglub.Time.Core;
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

    public static class DateAdjuster
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DateAdjuster{TDate}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        public static DateAdjuster<TDate> Create<TDate>(ICalendar<TDate> calendar)
            where TDate : IDate<TDate>
        {
            return new(calendar?.Scope!);
        }
    }

    /// <summary>
    /// Provides a default implementation for <see cref="IDateAdjuster{TDate}"/>.
    /// <para>This class works best when <typeparamref name="TDate"/> is based on the count of
    /// consecutive days since the epoch.</para>
    /// </summary>
    /// <typeparam name="TDate">The type of date object.</typeparam>
    public class DateAdjuster<TDate> : IDateAdjuster<TDate>
        // NB: we could relax the constraint on TDate and use IFixedDate<>
        // but then we would have to obtain manually the date parts (y, m, d, doy).
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DateAdjuster{TDate}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        public DateAdjuster(CalendarScope scope)
        {
            Requires.NotNull(scope);

            Scope = scope;

            _epoch = scope.Epoch;
            _schema = scope.Schema;
        }

        /// <inheritdoc />
        public CalendarScope Scope { get; }

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
            var (y, m, _) = date;
            int daysSinceEpoch = _schema.GetStartOfMonth(y, m);
            return TDate.FromDayNumber(_epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate GetEndOfMonth(TDate date)
        {
            var (y, m, _) = date;
            int daysSinceEpoch = _schema.GetEndOfMonth(y, m);
            return TDate.FromDayNumber(_epoch + daysSinceEpoch);
        }

        //
        // Adjustments for the core parts
        //

        /// <inheritdoc />
        [Pure]
        public TDate AdjustYear(TDate date, int newYear)
        {
            var (_, m, d) = date;
            Scope.ValidateYearMonthDay(newYear, m, d, nameof(newYear));
            var daysSinceEpoch = _schema.CountDaysSinceEpoch(newYear, m, d);
            return TDate.FromDayNumber(_epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate AdjustMonth(TDate date, int newMonth)
        {
            var (y, _, d) = date;
            _schema.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));
            var daysSinceEpoch = _schema.CountDaysSinceEpoch(y, newMonth, d);
            return TDate.FromDayNumber(_epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate AdjustDay(TDate date, int newDay)
        {
            var (y, m, _) = date;
            ValidateDayOfMonth(y, m, newDay, nameof(newDay));
            var daysSinceEpoch = _schema.CountDaysSinceEpoch(y, m, newDay);
            return TDate.FromDayNumber(_epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate AdjustDayOfYear(TDate date, int newDayOfYear)
        {
            int y = date.Year;
            _schema.PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));
            var daysSinceEpoch = _schema.CountDaysSinceEpoch(y, newDayOfYear);
            return TDate.FromDayNumber(_epoch + daysSinceEpoch);
        }

        private void ValidateDayOfMonth(int y, int m, int dayOfMonth, string paramName)
        {
            if (dayOfMonth < 1
                || (dayOfMonth > _schema.MinDaysInMonth
                    && dayOfMonth > _schema.CountDaysInMonth(y, m)))
            {
                Throw.ArgumentOutOfRange(paramName);
            }
        }
    }
}
