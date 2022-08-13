﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Hemerology;
    using Zorglub.Time.Hemerology.Scopes;

    /// <summary>
    /// Defines common adjusters for <typeparamref name="TDate"/> and provides a base for derived
    /// classes.
    /// <para>This class can ONLY be inherited from within friend assemblies.</para>
    /// </summary>
    /// <typeparam name="TDate">The type of date object.</typeparam>
    public abstract class SpecialAdjuster<TDate> : IDateAdjuster<TDate>
        // We could remove the constraint on TDate but it would make things a
        // bit harder than necessary.
        // IDateable:
        //   Without it, we would have to obtain the date parts (y, m, d, doy)
        //   by other means, e.g. using the underlying schema.
        // IDateableOrdinally:
        //   Not necessary, but it should largely prevent the use of this class
        //   with date types not based on daysSinceEpoch.
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
            Requires.NotNull(scope);
            // To avoid an unnecessary validation, a derived class is expected
            // to override GetDate(), but this can only be justified when the
            // scope is complete.
            if (scope.IsComplete == false) Throw.Argument(nameof(scope));

            Scope = scope;
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
        /// <para>This method does NOT validate its parameter.</para>
        /// </summary>
        [Pure]
        private protected abstract TDate GetDate(int daysSinceEpoch);

        /// <inheritdoc />
        [Pure]
        public TDate GetStartOfYear(TDate date)
        {
            // NB: we don't know if the start of the year is within the scope.
            int daysSinceEpoch = Schema.GetStartOfYear(date.Year);
            return GetDate(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate GetEndOfYear(TDate date)
        {
            // NB: we don't know if the end of the year is within the scope.
            int daysSinceEpoch = Schema.GetEndOfYear(date.Year);
            return GetDate(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate GetStartOfMonth(TDate date)
        {
            // NB: we don't know if the start of the month is within the scope.
            var (y, m, _) = date;
            int daysSinceEpoch = Schema.GetStartOfMonth(y, m);
            return GetDate(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate GetEndOfMonth(TDate date)
        {
            // NB: we don't know if the end of the month is within the scope.
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
