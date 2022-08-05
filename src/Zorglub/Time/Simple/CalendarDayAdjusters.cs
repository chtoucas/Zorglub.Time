// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    /// <summary>
    /// Provides common adjusters for <see cref="CalendarDay"/>.
    /// </summary>
    public static class CalendarDayAdjusters
    {
        /// <summary>
        /// Obtains the start of the year to which belongs the specified date.
        /// </summary>
        [Pure]
        public static CalendarDay GetStartOfYear(CalendarDay date)
        {
            ref readonly var chr = ref date.CalendarRef;
            int daysSinceEpoch = chr.Schema.GetStartOfYear(date.Year);
            return new CalendarDay(daysSinceEpoch, date.Cuid);
        }

        /// <summary>
        /// Obtains the end of the year to which belongs the specified date.
        /// </summary>
        [Pure]
        public static CalendarDay GetEndOfYear(CalendarDay date)
        {
            ref readonly var chr = ref date.CalendarRef;
            int daysSinceEpoch = chr.Schema.GetEndOfYear(date.Year);
            return new CalendarDay(daysSinceEpoch, date.Cuid);
        }

        /// <summary>
        /// Obtains the start of the month to which belongs the specified date.
        /// </summary>
        [Pure]
        public static CalendarDay GetStartOfMonth(CalendarDay date)
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
        public static CalendarDay GetEndOfMonth(CalendarDay date)
        {
            ref readonly var chr = ref date.CalendarRef;
            date.Unpack(chr, out int y, out int m, out _);
            int daysSinceEpoch = chr.Schema.GetEndOfMonth(y, m);
            return new CalendarDay(daysSinceEpoch, date.Cuid);
        }
    }
}
