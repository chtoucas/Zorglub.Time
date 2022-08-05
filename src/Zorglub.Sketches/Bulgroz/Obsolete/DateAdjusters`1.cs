// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Obsolete
{
    using Zorglub.Time;
    using Zorglub.Time.Core;
    using Zorglub.Time.Hemerology;
    using Zorglub.Time.Hemerology.Scopes;

    // The code works because we use TDate.FromDayNumber() which validates
    // the dayNumber, but it also makes the code unefficient when the
    // calendar is complete (the validation is unnecessary).
    //
    // Do NOT use, this class does nothing more than Hemerology.DateAdjusters.
    // Maybe it's a bit more efficient when the calendar is not complete
    // (see the validation in BoundedBelowNakedCalendar).
    // Furthemore, for date types based on a y/m/d repr, there is a better way
    // to implement IDateAdjusters; see for instance MyDate.

    public static class DateAdjusters
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DateAdjusters{TDate}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        public static DateAdjusters<TDate> Create<TDate>(ICalendar<TDate> calendar)
            where TDate : IDate<TDate>
        {
            return new(calendar?.Scope!);
        }
    }

    /// <summary>
    /// Provides a default implementation for <see cref="IDateAdjusters{TDate}"/>.
    /// <para>This class works best when <typeparamref name="TDate"/> is based on the count of
    /// consecutive days since the epoch.</para>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DateAdjusters{TDate}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        public DateAdjusters(CalendarScope scope)
        {
            Requires.NotNull(scope);

            _epoch = scope.Epoch;
            _schema = scope.Schema;
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
    }
}
