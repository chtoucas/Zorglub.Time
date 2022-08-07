// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    /// <summary>
    /// Provides common adjusters for <see cref="OrdinalDate"/>.
    /// </summary>
    public static class OrdinalDateAdjusters
    {
        /// <summary>
        /// Obtains the start of the year to which belongs the specified date.
        /// </summary>
        [Pure]
        public static OrdinalDate GetStartOfYear(OrdinalDate date) =>
            new(date.Parts.StartOfYear, date.Cuid);

        /// <summary>
        /// Obtains the end of the year to which belongs the specified date.
        /// </summary>
        [Pure]
        public static OrdinalDate GetEndOfYear(OrdinalDate date)
        {
            ref readonly var chr = ref date.CalendarRef;
            var ydoy = chr.Schema.GetOrdinalPartsAtEndOfYear(date.Year);
            return new OrdinalDate(ydoy, date.Cuid);
        }

        /// <summary>
        /// Obtains the start of the month to which belongs the specified date.
        /// </summary>
        [Pure]
        public static OrdinalDate GetStartOfMonth(OrdinalDate date)
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
        public static OrdinalDate GetEndOfMonth(OrdinalDate date)
        {
            date.Parts.Unpack(out int y, out int doy);
            ref readonly var chr = ref date.CalendarRef;
            var sch = chr.Schema;
            int m = sch.GetMonth(y, doy, out _);
            var ydoy = sch.GetOrdinalPartsAtEndOfMonth(y, m);
            return new OrdinalDate(ydoy, date.Cuid);
        }

        [Pure]
        public static Func<OrdinalDate, OrdinalDate> YearAdjuster(int newYear) =>
            x => x.WithYear(newYear);

        [Pure]
        public static Func<OrdinalDate, OrdinalDate> MonthAdjuster(int newMonth) =>
            x => x.ToCalendarDate().WithMonth(newMonth).ToOrdinalDate();

        [Pure]
        public static Func<OrdinalDate, OrdinalDate> DayAdjuster(int newDay) =>
            x => x.ToCalendarDate().WithDay(newDay).ToOrdinalDate();

        [Pure]
        public static Func<OrdinalDate, OrdinalDate> DayOfYearAdjuster(int newDayOfYear) =>
            x => x.WithDayOfYear(newDayOfYear);
    }
}
