// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    public sealed class ZDateAdjusters :
        IYearEndpointsProvider<ZDate>,
        IMonthEndpointsProvider<ZDate>
    {
        /// <summary>
        /// Obtains the start of the year to which belongs the specified date.
        /// </summary>
        [Pure]
        public static ZDate GetStartOfYear(ZDate day)
        {
            ref readonly var chr = ref day.CalendarRef;
            var sch = chr.Schema;
            int y = sch.GetYear(day.DaysSinceEpoch, out _);
            var startOfYear = sch.GetStartOfYear(y);
            return new(startOfYear, day.Cuid);
        }

        /// <summary>
        /// Obtains the end of the year to which belongs the specified date.
        /// </summary>
        [Pure]
        public static ZDate GetEndOfYear(ZDate day)
        {
            ref readonly var chr = ref day.CalendarRef;
            var sch = chr.Schema;
            int y = sch.GetYear(day.DaysSinceEpoch, out _);
            var endOfYear = sch.GetEndOfYear(y);
            return new(endOfYear, day.Cuid);
        }

        /// <summary>
        /// Obtains the start of the month to which belongs the specified date.
        /// </summary>
        [Pure]
        public static ZDate GetStartOfMonth(ZDate day)
        {
            ref readonly var chr = ref day.CalendarRef;
            var sch = chr.Schema;
            sch.GetDateParts(day.DaysSinceEpoch, out int y, out int m, out _);
            var startOfMonth = sch.GetStartOfMonth(y, m);
            return new(startOfMonth, day.Cuid);
        }

        /// <summary>
        /// Obtains the end of the month to which belongs the specified date.
        /// </summary>
        [Pure]
        public static ZDate GetEndOfMonth(ZDate day)
        {
            ref readonly var chr = ref day.CalendarRef;
            var sch = chr.Schema;
            sch.GetDateParts(day.DaysSinceEpoch, out int y, out int m, out _);
            var endOfMonth = sch.GetEndOfMonth(y, m);
            return new(endOfMonth, day.Cuid);
        }

    }
}
