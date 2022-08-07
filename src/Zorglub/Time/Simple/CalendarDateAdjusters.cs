// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    /// <summary>
    /// Provides common adjusters for <see cref="CalendarDate"/>.
    /// </summary>
    public static class CalendarDateAdjusters
    {
        /// <summary>
        /// Obtains the start of the year to which belongs the specified date.
        /// </summary>
        [Pure]
        public static CalendarDate GetStartOfYear(CalendarDate date) =>
            new(date.Parts.StartOfYear, date.Cuid);

        /// <summary>
        /// Obtains the end of the year to which belongs the specified date.
        /// </summary>
        [Pure]
        public static CalendarDate GetEndOfYear(CalendarDate date)
        {
            ref readonly var chr = ref date.CalendarRef;
            var ymd = chr.Schema.GetDatePartsAtEndOfYear(date.Year);
            return new CalendarDate(ymd, date.Cuid);
        }

        /// <summary>
        /// Obtains the start of the month to which belongs the specified date.
        /// </summary>
        [Pure]
        public static CalendarDate GetStartOfMonth(CalendarDate date) =>
            new(date.Parts.StartOfMonth, date.Cuid);

        /// <summary>
        /// Obtains the end of the month to which belongs the specified date.
        /// </summary>
        [Pure]
        public static CalendarDate GetEndOfMonth(CalendarDate date)
        {
            date.Parts.Unpack(out int y, out int m);
            ref readonly var chr = ref date.CalendarRef;
            var ymd = chr.Schema.GetDatePartsAtEndOfMonth(y, m);
            return new CalendarDate(ymd, date.Cuid);
        }

        [Pure]
        public static Func<CalendarDate, CalendarDate> YearAdjuster(int newYear) =>
            x => x.WithYear(newYear);

        [Pure]
        public static Func<CalendarDate, CalendarDate> MonthAdjuster(int newMonth) =>
            x => x.WithMonth(newMonth);

        [Pure]
        public static Func<CalendarDate, CalendarDate> DayAdjuster(int newDay) =>
            x => x.WithDay(newDay);

        [Pure]
        public static Func<CalendarDate, CalendarDate> DayOfYearAdjuster(int newDayOfYear) =>
            x => x.ToOrdinalDate().WithDayOfYear(newDayOfYear).ToCalendarDate();
    }
}
