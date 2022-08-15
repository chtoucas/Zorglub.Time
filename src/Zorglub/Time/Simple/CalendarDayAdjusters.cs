// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    /// <summary>
    /// Provides a set of common adjusters for <see cref="CalendarDay"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class CalendarDayAdjusters
    {
        /// <summary>
        /// Obtains the start of the year to which belongs the specified date.
        /// </summary>
        [Pure]
        public static CalendarDay GetStartOfYear(this CalendarDay date)
        {
            ref readonly var chr = ref date.CalendarRef;
            int daysSinceEpoch = chr.Schema.GetStartOfYear(date.Year);
            return new CalendarDay(daysSinceEpoch, date.Cuid);
        }

        /// <summary>
        /// Obtains the end of the year to which belongs the specified date.
        /// </summary>
        [Pure]
        public static CalendarDay GetEndOfYear(this CalendarDay date)
        {
            ref readonly var chr = ref date.CalendarRef;
            int daysSinceEpoch = chr.Schema.GetEndOfYear(date.Year);
            return new CalendarDay(daysSinceEpoch, date.Cuid);
        }

        /// <summary>
        /// Obtains the start of the month to which belongs the specified date.
        /// </summary>
        [Pure]
        public static CalendarDay GetStartOfMonth(this CalendarDay date)
        {
            ref readonly var chr = ref date.CalendarRef;
            date.Unpack(chr, out int y, out int m, out _);
            int daysSinceEpoch = chr.Schema.GetStartOfMonth(y, m);
            return new CalendarDay(daysSinceEpoch, date.Cuid);
        }

        /// <summary>
        /// Obtains the end of the month to which belongs the specified date.
        /// </summary>
        [Pure]
        public static CalendarDay GetEndOfMonth(this CalendarDay date)
        {
            ref readonly var chr = ref date.CalendarRef;
            date.Unpack(chr, out int y, out int m, out _);
            int daysSinceEpoch = chr.Schema.GetEndOfMonth(y, m);
            return new CalendarDay(daysSinceEpoch, date.Cuid);
        }

        /// <summary>
        /// Adjusts the year field to the specified value, yielding a new date.
        /// </summary>
        /// <exception cref="AoorException">The resulting date would be invalid.</exception>
        [Pure]
        public static CalendarDay WithYear(this CalendarDay date, int newYear) =>
            date.ToCalendarDate().WithYear(newYear).ToCalendarDay();

        /// <summary>
        /// Adjusts the month field to the specified value, yielding a new date.
        /// </summary>
        /// <exception cref="AoorException">The resulting date would be invalid.</exception>
        [Pure]
        public static CalendarDay WithMonth(this CalendarDay date, int newMonth) =>
            date.ToCalendarDate().WithMonth(newMonth).ToCalendarDay();

        /// <summary>
        /// Adjusts the day of the month field to the specified value, yielding a new date.
        /// </summary>
        /// <exception cref="AoorException">The resulting date would be invalid.</exception>
        [Pure]
        public static CalendarDay WithDay(this CalendarDay date, int newDay) =>
            date.ToCalendarDate().WithDay(newDay).ToCalendarDay();

        /// <summary>
        /// Adjusts the day of the year field to the specified value, yielding a new date.
        /// </summary>
        /// <exception cref="AoorException">The resulting date would be invalid.</exception>
        [Pure]
        public static CalendarDay WithDayOfYear(this CalendarDay date, int newDayOfYear) =>
            date.ToOrdinalDate().WithDayOfYear(newDayOfYear).ToCalendarDay();
    }
}
