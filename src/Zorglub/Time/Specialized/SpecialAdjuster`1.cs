// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Hemerology;
    using Zorglub.Time.Hemerology.Scopes;

    // Reasons to keep the constructor internal:
    // - we don't validate the input. Only for TDate developed whitin this
    //   project do we know that it's not possible to create an invalid date.
    //   In this project, we don't have an example based on IDateable but on
    //   IFixedDay. Indeed, a DayNumber exists beyond the scope of a calendar
    //   and therefore could be used as a type argument.
    // - This impl is only intersting if GetDate() is non-validating, otherwise
    //   we should simply use the methods provided by a calendar.
    // - this class works best for date types based on the count of days since
    //   the epoch which is the case for all date types in Specialized. For types
    //   using a y/m/d/doy repr. there is a better way of implementing
    //   IDateAdjuster<TDate>; see e.g. MyDate in Samples.
    // We could remove the constraint on TDate but it would make things a
    // bit harder than necessary. Without IDateable, we would have to obtain the
    // date parts (y, m, d, doy) by other means, e.g. using the underlying schema.

    /// <summary>
    /// Defines an adjuster for <typeparamref name="TDate"/> and provides a base for derived classes.
    /// <para>This class can ONLY be inherited from within friend assemblies.</para>
    /// </summary>
    /// <typeparam name="TDate">The type of date object.</typeparam>
    public abstract class SpecialAdjuster<TDate> : IDateAdjuster<TDate>
        where TDate : IDateable
    {
        /// <summary>
        /// Called from constructors in derived classes to initialize the
        /// <see cref="SpecialAdjuster{TDate}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        /// <exception cref="ArgumentException">paramref name="scope"/> is NOT complete.</exception>
        private protected SpecialAdjuster(MinMaxYearScope scope)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

        /// <summary>
        /// Called from constructors in derived classes to initialize the
        /// <see cref="SpecialAdjuster{TDate}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        /// <exception cref="ArgumentException">paramref name="scope"/> is NOT complete.</exception>
        private protected SpecialAdjuster(CalendarScope scope)
        {
            // TODO(api): remove this ctor. To do that, we must change SpecialCalendar:
            // calendar.Scope is a CalendarScope.
            Scope = MinMaxYearScope.Create(scope);
        }

        /// <inheritdoc/>
        public CalendarScope Scope { get; }

        /// <summary>
        /// Gets the schema.
        /// </summary>
        protected ICalendricalSchema Schema => Scope.Schema;

        /// <summary>
        /// Creates a new instance of <typeparamref name="TDate"/> from the specified count of
        /// consecutive days since the epoch.
        /// <para>This method does NOT validate its parameter.</para>
        /// </summary>
        [Pure]
        private protected abstract TDate GetDate(int daysSinceEpoch);

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
            var (y, m, _) = date;
            int daysSinceEpoch = Schema.GetStartOfMonth(y, m);
            return GetDate(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate GetEndOfMonth(TDate date)
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
