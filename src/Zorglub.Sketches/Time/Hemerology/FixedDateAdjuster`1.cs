// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Hemerology.Scopes;

    // FIXME'code): this code does NOT work with date types linked to a
    // poly-calendar system; see TDate.FromDayNumber().
    // Another problem: validation of input. We assume that the "date" is valid.
    //
    // This class is only interesting when the scope is not complete.
    // The code works because TDate.FromDayNumber() is validating, but it also
    // makes it unefficient when the calendar is complete --- in that case, the
    // validation is simply unnecessary.
    //
    // For date types based on a y/m/d repr, there is a better way to
    // implement IDateAdjuster; see for instance MyDate in Samples.

    /// <summary>
    /// Defines common adjusters for <typeparamref name="TDate"/> and provides a base for derived
    /// classes.
    /// </summary>
    /// <typeparam name="TDate">The type of date object.</typeparam>
    [Obsolete("Broken.")]
    public class FixedDateAdjuster<TDate> : IDateAdjuster<TDate>
        // We could remove the IDateable constraint on TDate but it would make
        // things a bit harder than necessary. Indeed, without IDateable, we
        // would have to obtain the date parts (y, m, d, doy) by other means,
        // e.g. using the underlying schema.
        where TDate : IDate<TDate>
    {
        /// <summary>
        /// Called from constructors in derived classes to initialize the
        /// <see cref="FixedDateAdjuster{TDate}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        public FixedDateAdjuster(CalendarScope scope)
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
        // When TDate is also a fixed date, a default implementation is rather
        // straightforward:
        // > TDate.FromDayNumber(_epoch + daysSinceEpoch);
        // but we don't add this constraint and we force a derived class to
        // provide its own version.
        // The idea is that if possible GetDate() should not perform any
        // unnecessary validation. Indeed, when the scope is complete, then
        // GetDate() always receives a valid "daysSinceEpoch", whereas when it
        // is not, GetDate() MUST validate its input.
        // Anyway, the real reason is that it DOES NOT WORK when TDate is linked
        // to a poly-calendar system, and there is no constraint to prevent that.
        [Pure]
        protected virtual TDate GetDate(int daysSinceEpoch) =>
            TDate.FromDayNumber(Epoch + daysSinceEpoch);

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
    }
}
