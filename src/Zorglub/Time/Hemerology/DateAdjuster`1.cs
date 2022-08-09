// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Hemerology.Scopes;

    /// <summary>
    /// Defines the common adjusters for <typeparamref name="TDate"/> and provides a base for
    /// derived classes.
    /// </summary>
    /// <typeparam name="TDate">The type of date object.</typeparam>
    public abstract class DateAdjuster<TDate> : IDateAdjuster<TDate>
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

        /// <inheritdoc />
        [Pure]
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "VB.NET Date.")]
        public abstract TDate GetStartOfYear(TDate date);

        /// <inheritdoc />
        [Pure]
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "VB.NET Date.")]
        public abstract TDate GetEndOfYear(TDate date);

        /// <inheritdoc />
        [Pure]
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "VB.NET Date.")]
        public abstract TDate GetStartOfMonth(TDate date);

        /// <inheritdoc />
        [Pure]
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "VB.NET Date.")]
        public abstract TDate GetEndOfMonth(TDate date);

        //
        // Adjustments for the core parts
        //

        /// <inheritdoc />
        [Pure]
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "VB.NET Date.")]
        public abstract TDate AdjustYear(TDate date, int newYear);

        /// <inheritdoc />
        [Pure]
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "VB.NET Date.")]
        public abstract TDate AdjustMonth(TDate date, int newMonth);

        /// <inheritdoc />
        [Pure]
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "VB.NET Date.")]
        public abstract TDate AdjustDay(TDate date, int newDay);

        /// <inheritdoc />
        [Pure]
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "VB.NET Date.")]
        public abstract TDate AdjustDayOfYear(TDate date, int newDayOfYear);

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
