// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Hemerology.Scopes;

    // FIXME(code): this code only works with date types linked to a mono-calendar
    // system; see TDate.FromDayNumber().

    /// <summary>
    /// Provides a default implementation for <see cref="IDateAdjuster{TDate}"/>.
    /// <para>This class works best when <typeparamref name="TDate"/> is based on the count of
    /// consecutive days since the epoch.</para>
    /// </summary>
    /// <typeparam name="TDate">The type of date object.</typeparam>
    [Obsolete("Broken as it.")]
    public class FixedDateAdjuster<TDate> : IDateAdjuster<TDate>
        where TDate : IFixedDate<TDate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FixedDateAdjuster{TDate}"/> class.
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

        /// <inheritdoc />
        [Pure]
        public TDate GetStartOfYear(TDate date)
        {
            var dayNumber = date.ToDayNumber();
            int y = Schema.GetYear(dayNumber - Epoch, out _);
            int daysSinceEpoch = Schema.GetStartOfYear(y);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate GetEndOfYear(TDate date)
        {
            var dayNumber = date.ToDayNumber();
            int y = Schema.GetYear(dayNumber - Epoch, out _);
            int daysSinceEpoch = Schema.GetEndOfYear(y);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate GetStartOfMonth(TDate date)
        {
            var dayNumber = date.ToDayNumber();
            Schema.GetDateParts(dayNumber - Epoch, out int y, out int m, out _);
            int daysSinceEpoch = Schema.GetStartOfMonth(y, m);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate GetEndOfMonth(TDate date)
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
        public TDate AdjustYear(TDate date, int newYear)
        {
            var dayNumber = date.ToDayNumber();
            Schema.GetDateParts(dayNumber - Epoch, out _, out int m, out int d);
            AdjustYearValidate(newYear, m, d);

            var daysSinceEpoch = Schema.CountDaysSinceEpoch(newYear, m, d);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate AdjustMonth(TDate date, int newMonth)
        {
            var dayNumber = date.ToDayNumber();
            Schema.GetDateParts(dayNumber - Epoch, out int y, out _, out int d);
            AdjustMonthValidate(y, newMonth, d);

            var daysSinceEpoch = Schema.CountDaysSinceEpoch(y, newMonth, d);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate AdjustDay(TDate date, int newDay)
        {
            var dayNumber = date.ToDayNumber();
            Schema.GetDateParts(dayNumber - Epoch, out int y, out int m, out _);
            AdjustDayValidate(y, m, newDay);

            var daysSinceEpoch = Schema.CountDaysSinceEpoch(y, m, newDay);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate AdjustDayOfYear(TDate date, int newDayOfYear)
        {
            var dayNumber = date.ToDayNumber();
            int y = Schema.GetYear(dayNumber - Epoch, out _);
            AdjustDayOfYearValidate(y, newDayOfYear);

            var daysSinceEpoch = Schema.CountDaysSinceEpoch(y, newDayOfYear);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        //
        // Validation helpers
        //

        protected void AdjustYearValidate(int newYear, int m, int d) =>
            // We MUST re-validate the entire date.
            Scope.ValidateYearMonthDay(newYear, m, d, nameof(newYear));

        protected void AdjustMonthValidate(int y, int newMonth, int d) =>
            // We only need to validate "newMonth" and "d".
            Schema.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));

        /// <summary>
        /// Validates the specified day of the month.
        /// <para>This method does NOT validate <paramref name="y"/>.</para>
        /// <para>This method does NOT validate <paramref name="m"/>.</para>
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int32"/>.</exception>
        /// <exception cref="AoorException">The validation failed.</exception>
        protected void AdjustDayValidate(int y, int m, int dayOfMonth)
        {
            // We only need to validate "newDay".
            if (dayOfMonth < 1
                || (dayOfMonth > Schema.MinDaysInMonth
                    && dayOfMonth > Schema.CountDaysInMonth(y, m)))
            {
                Throw.ArgumentOutOfRange(nameof(dayOfMonth));
            }
        }

        protected void AdjustDayOfYearValidate(int y, int newDayOfYear) =>
            // We only need to validate "newDayOfYear".
            Schema.PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));
    }
}
