// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extensions
{
    using Zorglub.Time.Simple;

    /// <summary>
    /// Provides static helpers for <see cref="CalendarYear"/> or
    /// <see cref="CalendarMonth"/>.
    /// </summary>
    [Obsolete("Use the methods from CalendarDay.")]
    public static partial class DayResult { }

    public partial class DayResult // CalendarYear.
    {
        /// <summary>
        /// Obtains the first day of the specified year.
        /// </summary>
        [Pure]
        public static CalendarDay GetStartOfYear(this CalendarYear @this) =>
            CalendarDay.AtStartOfYear(@this);

        /// <summary>
        /// Obtains the date corresponding to the specified day of the specified
        /// year.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfYear"/> is
        /// outside the range of valid values.</exception>
        [Pure]
        public static CalendarDay GetDayOfYear(this CalendarYear @this, int dayOfYear) =>
            CalendarDay.AtDayOfYear(@this, dayOfYear);

        /// <summary>
        /// Obtains the last day of the specified year.
        /// </summary>
        [Pure]
        public static CalendarDay GetEndOfYear(this CalendarYear @this) =>
            CalendarDay.AtEndOfYear(@this);
    }

    public partial class DayResult // CalendarMonth.
    {
#if false

        /// <summary>
        /// Obtains the first day of the year to which belongs the specified month.
        /// </summary>
        [Pure]
        public static CalendarDay GetStartOfYear(this CalendarMonth @this)
        {
            int daysSinceEpoch = @this.Calendar.Schema.GetStartOfYear(@this.Year);
            return new CalendarDay(daysSinceEpoch, @this.Cuid);
        }

        /// <summary>
        /// Obtains the last day of the year to which belongs the specified month.
        /// </summary>
        [Pure]
        public static CalendarDay GetEndOfYear(this CalendarMonth @this)
        {
            int daysSinceEpoch = @this.Calendar.Schema.GetEndOfYear(@this.Year);
            return new CalendarDay(daysSinceEpoch, @this.Cuid);
        }

#endif

        /// <summary>
        /// Obtains the first day of the specified month.
        /// </summary>
        [Pure]
        public static CalendarDay GetStartOfMonth(this CalendarMonth @this) =>
            CalendarDay.AtStartOfMonth(@this);

        /// <summary>
        /// Obtains the date corresponding to the specified day of the specified
        /// month.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfMonth"/> is
        /// outside the range of valid values.</exception>
        [Pure]
        public static CalendarDay GetDayOfMonth(this CalendarMonth @this, int dayOfMonth) =>
            CalendarDay.AtDayOfMonth(@this, dayOfMonth);

        /// <summary>
        /// Obtains the last day of the specified month.
        /// </summary>
        [Pure]
        public static CalendarDay GetEndOfMonth(this CalendarMonth @this) =>
            CalendarDay.AtEndOfMonth(@this);
    }
}
