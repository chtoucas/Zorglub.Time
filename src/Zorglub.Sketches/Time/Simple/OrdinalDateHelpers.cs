// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using System.Linq;

    using Zorglub.Time.Core;

    /// <summary>
    /// Provides static helpers related to <see cref="OrdinalDate"/>.
    /// </summary>
    public static partial class OrdinalDateHelpers { }

    public partial class OrdinalDateHelpers // CalendarYear.
    {
        // Now these are methods/props on CalendarMonth.

        /// <summary>
        /// Obtains the first day of the specified year.
        /// </summary>
        [Pure]
        public static OrdinalDate GetStartOfYear(CalendarYear year) => year.FirstDay;

        /// <summary>
        /// Obtains the ordinal date corresponding to the specified day of this
        /// year instance.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfYear"/> is
        /// outside the range of valid values.</exception>
        [Pure]
        public static OrdinalDate GetDayOfYear(CalendarYear year, int dayOfYear) =>
            year.GetDayOfYear(dayOfYear);

        /// <summary>
        /// Obtains the last day of the specified year.
        /// </summary>
        [Pure]
        public static OrdinalDate GetEndOfYear(CalendarYear year) => year.LastDay;
    }

    public partial class OrdinalDateHelpers // CalendarMonth.
    {
#if false

        /// <summary>
        /// Obtains the first day of the year to which belongs the specified month.
        /// </summary>
        [Pure]
        public static OrdinalDate GetStartOfYear(CalendarMonth month)
        {
            var ydoy = month.Calendar.Schema.GetStartOfYearOrdinalParts(month.Year);
            return new OrdinalDate(ydoy, month.Cuid);
        }

        /// <summary>
        /// Obtains the last day of the year to which belongs the specified month.
        /// </summary>
        [Pure]
        public static OrdinalDate GetEndOfYear(CalendarMonth month)
        {
            var ydoy = month.Calendar.Schema.GetEndOfYearOrdinalParts(month.Year);
            return new OrdinalDate(ydoy, month.Cuid);
        }

#endif

        /// <summary>
        /// Obtains the sequence of days in the specified month.
        /// </summary>
        [Pure]
        public static IEnumerable<OrdinalDate> GetDaysInMonth(CalendarMonth month)
        {
            var sch = month.Calendar.Schema;
            month.Parts.Unpack(out int y, out int m);
            int startOfMonth = sch.CountDaysInYearBeforeMonth(y, m);
            int daysInMonth = sch.CountDaysInMonth(y, m);

            for (int d = 1; d <= daysInMonth; d++)
            {
                yield return new OrdinalDate(y, startOfMonth + d, month.Cuid);
            }
        }

        /// <summary>
        /// Obtains the first day of the specified month.
        /// </summary>
        [Pure]
        public static OrdinalDate GetStartOfMonth(CalendarMonth month)
        {
            month.Parts.Unpack(out int y, out int m);
            ref readonly var chr = ref month.CalendarRef;
            var ydoy = chr.Schema.GetStartOfMonthOrdinalParts(y, m);
            return new OrdinalDate(ydoy, month.Cuid);
        }

        /// <summary>
        /// Obtains the date corresponding to the specified day of the specified
        /// month.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfMonth"/> is
        /// outside the range of valid values.</exception>
        [Pure]
        public static OrdinalDate GetDayOfMonth(CalendarMonth month, int dayOfMonth)
        {
            month.Parts.Unpack(out int y, out int m);
            ref readonly var chr = ref month.CalendarRef;
            chr.ValidateDayOfMonth(y, m, dayOfMonth);
            int doy = chr.Schema.GetDayOfYear(y, m, dayOfMonth);
            return new OrdinalDate(new Yedoy(y, doy), month.Cuid);
        }

        /// <summary>
        /// Obtains the last day of the specified month.
        /// </summary>
        [Pure]
        public static OrdinalDate GetEndOfMonth(CalendarMonth month)
        {
            month.Parts.Unpack(out int y, out int m);
            ref readonly var chr = ref month.CalendarRef;
            var ydoy = chr.Schema.GetEndOfMonthOrdinalParts(y, m);
            return new OrdinalDate(ydoy, month.Cuid);
        }
    }
}
