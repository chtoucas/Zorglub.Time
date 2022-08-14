// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    // REVIEW(code): optimize WithXXX(), idem with the other adjusters.

    /// <summary>
    /// Provides common adjusters for <see cref="CalendarDate"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class CalendarDateAdjusters
    {
        /// <summary>
        /// Obtains the start of the year to which belongs the specified date.
        /// </summary>
        [Pure]
        public static CalendarDate GetStartOfYear(this CalendarDate date) =>
            new(date.Parts.StartOfYear, date.Cuid);

        /// <summary>
        /// Obtains the end of the year to which belongs the specified date.
        /// </summary>
        [Pure]
        public static CalendarDate GetEndOfYear(this CalendarDate date)
        {
            ref readonly var chr = ref date.CalendarRef;
            var ymd = chr.Schema.GetDatePartsAtEndOfYear(date.Year);
            return new CalendarDate(ymd, date.Cuid);
        }

        /// <summary>
        /// Obtains the start of the month to which belongs the specified date.
        /// </summary>
        [Pure]
        public static CalendarDate GetStartOfMonth(this CalendarDate date) =>
            new(date.Parts.StartOfMonth, date.Cuid);

        /// <summary>
        /// Obtains the end of the month to which belongs the specified date.
        /// </summary>
        [Pure]
        public static CalendarDate GetEndOfMonth(this CalendarDate date)
        {
            date.Parts.Unpack(out int y, out int m);
            ref readonly var chr = ref date.CalendarRef;
            var ymd = chr.Schema.GetDatePartsAtEndOfMonth(y, m);
            return new CalendarDate(ymd, date.Cuid);
        }

        /// <summary>
        /// Adjusts the day of the year field to the specified value, yielding a new date.
        /// </summary>
        /// <exception cref="AoorException">The resulting date would be invalid.</exception>
        [Pure]
        public static CalendarDate WithDayOfYear(this CalendarDate date, int newDayOfYear) =>
            date.ToOrdinalDate().WithDayOfYear(newDayOfYear).ToCalendarDate();
    }
}
