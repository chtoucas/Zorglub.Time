// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Hemerology;
    using Zorglub.Time.Hemerology.Scopes;

    /// <summary>
    /// Provides common adjusters for <typeparamref name="TDate"/> and provides a base for derived
    /// classes.
    /// <para>This class works best when <typeparamref name="TDate"/> is based on the count of
    /// consecutive days since the epoch.</para>
    /// <para>This class can ONLY be inherited from within friend assemblies.</para>
    /// </summary>
    /// <typeparam name="TDate">The type of date object.</typeparam>
    public abstract class SpecialAdjuster<TDate> : IDateAdjuster<TDate>
        where TDate : IDateable, IDateableOrdinally
    {
        // "private protected" because the abstract method GetDate() does NOT
        // validate its parameter.
        //
        // For this class to work, an ICalendar is all we need but it seems more
        // natural to use an ICalendar<TDate>, this way we emphasize that the
        // class works for a specific type of date. In fact, we only need a
        // complete scope but it would only make sense to have a ctor with just
        // a scope if we added the ctor with an ICalendar.

        /// <summary>
        /// Called from constructors in derived classes to initialize the
        /// <see cref="SpecialAdjuster{TDate}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        /// <exception cref="ArgumentException">The scope of <paramref name="calendar"/> is NOT
        /// complete.</exception>
        private protected SpecialAdjuster(ICalendar<TDate> calendar)
        {
            Requires.NotNull(calendar);

            var scope = calendar.Scope;
            // To avoid an unnecessary validation, a derived class is expected
            // to override GetDate(), but this can only be justified when the
            // scope is complete.
            if (scope.IsComplete == false) Throw.Argument(nameof(calendar));

            Scope = scope;
        }

        /// <inheritdoc/>
        public CalendarScope Scope { get; }

        /// <summary>
        /// Gets the schema.
        /// </summary>
        protected ICalendricalSchema Schema => Scope.Schema;

        internal Range<int> SupportedYears => Scope.Segment.SupportedYears;

        /// <summary>
        /// Creates a new instance of <typeparamref name="TDate"/> from the specified count of
        /// consecutive days since the epoch.
        /// <para>This method does NOT validate its parameter.</para>
        /// </summary>
        // A default implementation is straightforward:
        // > TDate.FromDayNumber(_epoch + daysSinceEpoch);
        // but we force a derived class to provide its own version.
        // The idea is that it should not perform any validation; otherwise
        // there is no reason at all to use SepcialAdjuster instead of the
        // default impl DateAdjuster.
        [Pure]
        protected abstract TDate GetDate(int daysSinceEpoch);

        /// <inheritdoc />
        [Pure]
        public TDate GetStartOfYear(TDate date)
        {
            int daysSinceEpoch = Schema.GetStartOfYear(date.Year);
            return GetDate(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate GetEndOfYear(TDate date)
        {
            int daysSinceEpoch = Schema.GetEndOfYear(date.Year);
            return GetDate(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate GetStartOfMonth(TDate date)
        {
            Schema.GetDateParts(date.DaysSinceEpoch, out int y, out int m, out _);
            int daysSinceEpoch = Schema.GetStartOfMonth(y, m);
            return GetDate(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate GetEndOfMonth(TDate date)
        {
            Schema.GetDateParts(date.DaysSinceEpoch, out int y, out int m, out _);
            int daysSinceEpoch = Schema.GetEndOfMonth(y, m);
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

        //
        // Adjustments for the core parts
        //

        /// <inheritdoc />
        [Pure]
        public TDate AdjustYear(TDate date, int newYear)
        {
            Schema.GetDateParts(date.DaysSinceEpoch, out _, out int m, out int d);
            // We MUST re-validate the entire date.
            Scope.ValidateYearMonthDay(newYear, m, d, nameof(newYear));

            int daysSinceEpoch = Schema.CountDaysSinceEpoch(newYear, m, d);
            return GetDate(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate AdjustMonth(TDate date, int newMonth)
        {
            Schema.GetDateParts(date.DaysSinceEpoch, out int y, out _, out int d);
            // We only need to validate "newMonth" and "d".
            Schema.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));

            int daysSinceEpoch = Schema.CountDaysSinceEpoch(y, newMonth, d);
            return GetDate(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate AdjustDay(TDate date, int newDay)
        {
            Schema.GetDateParts(date.DaysSinceEpoch, out int y, out int m, out _);
            // We only need to validate "newDay".
            ValidateDayOfMonth(y, m, newDay, nameof(newDay));

            int daysSinceEpoch = Schema.CountDaysSinceEpoch(y, m, newDay);
            return GetDate(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate AdjustDayOfYear(TDate date, int newDayOfYear)
        {
            int y = Schema.GetYear(date.DaysSinceEpoch, out _);
            // We only need to validate "newDayOfYear".
            Schema.PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));

            int daysSinceEpoch = Schema.CountDaysSinceEpoch(y, newDayOfYear);
            return GetDate(daysSinceEpoch);
        }

        /// <summary>
        /// Validates the specified day of the month.
        /// <para>This method does NOT validate <paramref name="y"/>.</para>
        /// <para>This method does NOT validate <paramref name="m"/>.</para>
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int32"/>.</exception>
        /// <exception cref="AoorException">The validation failed.</exception>
        private void ValidateDayOfMonth(int y, int m, int dayOfMonth, string? paramName = null)
        {
            if (dayOfMonth < 1
                || (dayOfMonth > Schema.MinDaysInMonth
                    && dayOfMonth > Schema.CountDaysInMonth(y, m)))
            {
                Throw.ArgumentOutOfRange(paramName ?? nameof(dayOfMonth));
            }
        }
    }
}
