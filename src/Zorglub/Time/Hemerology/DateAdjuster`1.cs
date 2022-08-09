// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Hemerology.Scopes;

    // For date types based on a y/m/d repr, there is a better way
    // to implement IDateAdjuster; see for instance MyDate.

    /// <summary>
    /// Defines the common adjusters for <typeparamref name="TDate"/> and provides a base for
    /// derived classes.
    /// <para>This class works best when <typeparamref name="TDate"/> is based on the count of
    /// consecutive days since the epoch.</para>
    /// </summary>
    /// <typeparam name="TDate">The type of date object.</typeparam>
    public abstract class DateAdjuster<TDate> : IDateAdjuster<TDate>
        // NB: we could rmove the constraint on TDate but then we would have to
        // obtain the date parts (y, m, d, doy) manually using the underlying
        // schema.
        where TDate : IDateable
    {
        /// <summary>
        /// Called from constructors in derived classes to initialize the
        /// <see cref="DateAdjuster{TDate}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        protected DateAdjuster(CalendarScope scope)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

        /// <inheritdoc/>
        public CalendarScope Scope { get; }

        /// <summary>
        /// Gets the epoch.
        /// </summary>
        protected DayNumber Epoch => Scope.Epoch;

        /// <summary>
        /// Gets the schema.
        /// </summary>
        protected ICalendricalSchema Schema => Scope.Schema;

        /// <summary>
        /// Creates a new instance of <typeparamref name="TDate"/> from the specified count of
        /// consecutive days since the epoch.
        /// </summary>
        // When TDate is a fixed date, a default implementation is straightforward:
        // > TDate.FromDayNumber(_epoch + daysSinceEpoch);
        // but we don't add this constraint and we force a derived class to
        // provide its own version.
        // The idea is that if possible it should not perform any validation.
        // - When the scope is complete, GetDate() does not need to perform any
        //   further validation.
        // - When the scope is not complete, GetDate() MUST be validating.
        [Pure]
        protected abstract TDate GetDate(int daysSinceEpoch);

        /// <inheritdoc />
        [Pure]
        public  TDate GetStartOfYear(TDate date)
        {
            int daysSinceEpoch = Schema.GetStartOfYear(date.Year);
            return GetDate(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public  TDate GetEndOfYear(TDate date)
        {
            int daysSinceEpoch = Schema.GetEndOfYear(date.Year);
            return GetDate(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public  TDate GetStartOfMonth(TDate date)
        {
            var (y, m, _) = date;
            int daysSinceEpoch = Schema.GetStartOfMonth(y, m);
            return GetDate(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public  TDate GetEndOfMonth(TDate date)
        {
            var (y, m, _) = date;
            int daysSinceEpoch = Schema.GetEndOfMonth(y, m);
            return GetDate(daysSinceEpoch);
        }

        //
        // Adjustments for the core parts
        //

        /// <inheritdoc />
        [Pure]
        public TDate AdjustYear(TDate date, int newYear)
        {
            var (_, m, d) = date;
            // We MUST re-validate the entire date.
            Scope.ValidateYearMonthDay(newYear, m, d, nameof(newYear));

            int daysSinceEpoch = Schema.CountDaysSinceEpoch(newYear, m, d);
            return GetDate(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate AdjustMonth(TDate date, int newMonth)
        {
            var (y, _, d) = date;
            // We only need to validate "newMonth" and "d".
            Schema.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));

            int daysSinceEpoch = Schema.CountDaysSinceEpoch(y, newMonth, d);
            return GetDate(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate AdjustDay(TDate date, int newDay)
        {
            var (y, m, _) = date;
            // We only need to validate "newDay".
            if (newDay < 1
                || (newDay > Schema.MinDaysInMonth
                    && newDay > Schema.CountDaysInMonth(y, m)))
            {
                Throw.ArgumentOutOfRange(nameof(newDay));
            }

            int daysSinceEpoch = Schema.CountDaysSinceEpoch(y, m, newDay);
            return GetDate(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate AdjustDayOfYear(TDate date, int newDayOfYear)
        {
            int y = date.Year;
            // We only need to validate "newDayOfYear".
            Schema.PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));

            int daysSinceEpoch = Schema.CountDaysSinceEpoch(y, newDayOfYear);
            return GetDate(daysSinceEpoch);
        }

        //
        // Adjusters for the core parts
        //

        /// <summary>
        /// Obtains an adjuster for the year field of a date.
        /// </summary>
        [Pure]
        public Func<TDate, TDate> WithYear(int newYear) => x => AdjustYear(x, newYear);

        /// <summary>
        /// Obtains an adjuster for the month field of a date.
        /// </summary>
        [Pure]
        public Func<TDate, TDate> WithMonth(int newMonth) => x => AdjustMonth(x, newMonth);

        /// <summary>
        /// Obtains an adjuster for the day of the month field of a date.
        /// </summary>
        [Pure]
        public Func<TDate, TDate> WithDay(int newDay) => x => AdjustDay(x, newDay);

        /// <summary>
        /// Obtains an adjuster for the day of the year field of a date.
        /// </summary>
        [Pure]
        public Func<TDate, TDate> WithDayOfYear(int newDayOfYear) =>
            x => AdjustDayOfYear(x, newDayOfYear);
    }
}
