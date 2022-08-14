// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    /// <summary>
    /// Provides common adjusters for <see cref="OrdinalDate"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class OrdinalDateAdjusters
    {
        /// <summary>
        /// Obtains the start of the year to which belongs the specified date.
        /// </summary>
        [Pure]
        public static OrdinalDate GetStartOfYear(this OrdinalDate date) =>
            new(date.Parts.StartOfYear, date.Cuid);

        /// <summary>
        /// Obtains the end of the year to which belongs the specified date.
        /// </summary>
        [Pure]
        public static OrdinalDate GetEndOfYear(this OrdinalDate date)
        {
            ref readonly var chr = ref date.CalendarRef;
            var ydoy = chr.Schema.GetOrdinalPartsAtEndOfYear(date.Year);
            return new OrdinalDate(ydoy, date.Cuid);
        }

        /// <summary>
        /// Obtains the start of the month to which belongs the specified date.
        /// </summary>
        [Pure]
        public static OrdinalDate GetStartOfMonth(this OrdinalDate date)
        {
            date.Parts.Unpack(out int y, out int doy);
            ref readonly var chr = ref date.CalendarRef;
            var sch = chr.Schema;
            int m = sch.GetMonth(y, doy, out _);
            var ydoy = sch.GetOrdinalPartsAtStartOfMonth(y, m);
            return new OrdinalDate(ydoy, date.Cuid);
        }

        /// <summary>
        /// Obtains the end of the month to which belongs the specified date.
        /// </summary>
        [Pure]
        public static OrdinalDate GetEndOfMonth(this OrdinalDate date)
        {
            date.Parts.Unpack(out int y, out int doy);
            ref readonly var chr = ref date.CalendarRef;
            var sch = chr.Schema;
            int m = sch.GetMonth(y, doy, out _);
            var ydoy = sch.GetOrdinalPartsAtEndOfMonth(y, m);
            return new OrdinalDate(ydoy, date.Cuid);
        }

        /// <summary>
        /// Adjusts the month field to the specified value, yielding a new date.
        /// </summary>
        /// <exception cref="AoorException">The resulting date would be invalid.</exception>
        [Pure]
        public static OrdinalDate WithMonth(this OrdinalDate date, int newMonth) =>
            date.ToCalendarDate().WithMonth(newMonth).ToOrdinalDate();

        /// Adjusts the day of the month field to the specified value, yielding a new date.
        /// </summary>
        /// <exception cref="AoorException">The resulting date would be invalid.</exception>
        [Pure]
        public static OrdinalDate WithDay(this OrdinalDate date, int newDay) =>
            date.ToCalendarDate().WithDay(newDay).ToOrdinalDate();
    }
}
