// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    /// <summary>
    /// Provides common adjusters for the simple date types.
    /// </summary>
    public static partial class DateAdjusters { }

    public partial class DateAdjusters // CalendarDate
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
    }

    public partial class DateAdjusters // CalendarDay
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

    public partial class DateAdjusters // CalendarDate
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
    }
}
