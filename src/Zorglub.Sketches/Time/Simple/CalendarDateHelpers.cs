// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;

    /// <summary>
    /// Provides static helpers related to <see cref="CalendarDate"/>.
    /// </summary>
    public static partial class CalendarDateHelpers { }

    public partial class CalendarDateHelpers // CalendarYear
    {
        /// <summary>
        /// Obtains the first day of the specified year.
        /// </summary>
        [Pure]
        public static CalendarDate GetStartOfYear(CalendarYear year) =>
            new(Yemoda.AtStartOfYear(year.Year), year.Cuid);

        /// <summary>
        /// Obtains the date corresponding to the specified day of the specified
        /// year.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfYear"/> is
        /// outside the range of valid values.</exception>
        [Pure]
        public static CalendarDate GetDayOfYear(CalendarYear year, int dayOfYear)
        {
            ref readonly var chr = ref year.CalendarRef;
            chr.PreValidator.ValidateDayOfYear(year.Year, dayOfYear);
            var ymd = chr.Schema.GetDateParts(year.Year, dayOfYear);
            return new CalendarDate(ymd, year.Cuid);
        }

        /// <summary>
        /// Obtains the last day of the specified year.
        /// </summary>
        [Pure]
        public static CalendarDate GetEndOfYear(CalendarYear year)
        {
            ref readonly var chr = ref year.CalendarRef;
            var ymd = chr.Schema.GetEndOfYearParts(year.Year);
            return new CalendarDate(ymd, year.Cuid);
        }
    }

    public partial class CalendarDateHelpers // CalendarMonth.
    {
#if false
        // Il me semble plus naturel et plus logique d'écrire :
        // - month.CalendarYear.GetStartOfYear()
        // - month.CalendarYear.GetEndOfYear()

        /// <summary>
        /// Obtains the first day of the year to which belongs the specified month.
        /// </summary>
        [Pure]
        public static CalendarDate GetStartOfYear(CalendarMonth month) =>
            new(month.Parts.StartOfYear, month.Cuid);

        /// <summary>
        /// Obtains the last day of the year to which belongs the specified month.
        /// </summary>
        [Pure]
        public static CalendarDate GetEndOfYear(CalendarMonth month)
        {
            var ymd = month.Calendar.Schema.GetEndOfYearParts(month.Year);
            return new CalendarDate(ymd, month.Cuid);
        }

#endif

        //
        // Dates in a given month.
        //
        // Now these are methods/props on CalendarMonth.

        /// <summary>
        /// Obtains the first day of the specified month.
        /// </summary>
        [Pure]
        public static CalendarDate GetStartOfMonth(CalendarMonth month) => month.FirstDay;

        /// <summary>
        /// Obtains the date corresponding to the specified day of the specified
        /// month.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfMonth"/> is
        /// outside the range of valid values.</exception>
        [Pure]
        public static CalendarDate GetDayOfMonth(CalendarMonth month, int dayOfMonth) =>
            month.GetDayOfMonth(dayOfMonth);

        /// <summary>
        /// Obtains the last day of the specified month.
        /// </summary>
        [Pure]
        public static CalendarDate GetEndOfMonth(CalendarMonth month) => month.LastDay;
    }
}
