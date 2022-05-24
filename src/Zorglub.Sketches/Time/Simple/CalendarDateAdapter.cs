// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;

    /// <summary>
    /// Provides methods to obtain new <see cref="CalendarDate"/> instances for a given year or month.
    /// </summary>
    public sealed partial class CalendarDateAdapter : ISimpleDateProvider<CalendarDate> { }

    public partial class CalendarDateAdapter // CalendarYear
    {
        /// <summary>
        /// Obtains the sequence of days in the specified year.
        /// </summary>
        [Pure]
        public IEnumerable<CalendarDate> GetDaysInYear(CalendarYear year)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtains the first day of the specified year.
        /// </summary>
        [Pure]
        public CalendarDate GetStartOfYear(CalendarYear year) =>
            new(Yemoda.AtStartOfYear(year.Year), year.Cuid);

        /// <summary>
        /// Obtains the date corresponding to the specified day of the specified year.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfYear"/> is outside the range of
        /// valid values.</exception>
        [Pure]
        public CalendarDate GetDayOfYear(CalendarYear year, int dayOfYear)
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
        public CalendarDate GetEndOfYear(CalendarYear year)
        {
            ref readonly var chr = ref year.CalendarRef;
            var ymd = chr.Schema.GetEndOfYearParts(year.Year);
            return new CalendarDate(ymd, year.Cuid);
        }
    }

    public partial class CalendarDateAdapter // CalendarMonth
    {
#if false
        // Il me semble plus naturel et plus logique d'écrire :
        // - month.CalendarYear.GetStartOfYear()
        // - month.CalendarYear.GetEndOfYear()

        /// <summary>
        /// Obtains the first day of the year to which belongs the specified month.
        /// </summary>
        [Pure]
        public CalendarDate GetStartOfYear(CalendarMonth month) =>
            new(month.Parts.StartOfYear, month.Cuid);

        /// <summary>
        /// Obtains the last day of the year to which belongs the specified month.
        /// </summary>
        [Pure]
        public CalendarDate GetEndOfYear(CalendarMonth month)
        {
            var ymd = month.Calendar.Schema.GetEndOfYearParts(month.Year);
            return new CalendarDate(ymd, month.Cuid);
        }

#endif

        /// <summary>
        /// Obtains the sequence of days in the specified month.
        /// </summary>
        [Pure]
        public IEnumerable<CalendarDate> GetDaysInMonth(CalendarMonth month) =>
            month.GetAllDays();

        /// <summary>
        /// Obtains the first day of the specified month.
        /// </summary>
        [Pure]
        public CalendarDate GetStartOfMonth(CalendarMonth month) => month.FirstDay;

        /// <summary>
        /// Obtains the date corresponding to the specified day of the specified month.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfMonth"/> is outside the range of
        /// valid values.</exception>
        [Pure]
        public CalendarDate GetDayOfMonth(CalendarMonth month, int dayOfMonth) =>
            month.GetDayOfMonth(dayOfMonth);

        /// <summary>
        /// Obtains the last day of the specified month.
        /// </summary>
        [Pure]
        public CalendarDate GetEndOfMonth(CalendarMonth month) => month.LastDay;
    }
}
