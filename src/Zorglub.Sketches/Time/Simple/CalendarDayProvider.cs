// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using System.Linq;

    /// <summary>
    /// Provides methods to obtain new <see cref="CalendarDay"/> instances for a given year or month.
    /// </summary>
    public sealed partial class CalendarDayProvider : IDateProvider<CalendarDay> { }

    public partial class CalendarDayProvider // CalendarYear
    {
        /// <inheritdoc/>
        [Pure]
        public IEnumerable<CalendarDay> GetDaysInYear(CalendarYear year)
        {
            var cuid = year.Cuid;
            var sch = year.Calendar.Schema;
            int startOfYear = sch.GetStartOfYear(year.Year);
            int daysInYear = sch.CountDaysInYear(year.Year);

            return from daysSinceEpoch
                   in Enumerable.Range(startOfYear, daysInYear)
                   select new CalendarDay(daysSinceEpoch, cuid);
        }

        /// <inheritdoc/>
        [Pure]
        public CalendarDay GetStartOfYear(CalendarYear year)
        {
            ref readonly var chr = ref year.CalendarRef;
            int daysSinceEpoch = chr.Schema.GetStartOfYear(year.Year);
            return new CalendarDay(daysSinceEpoch, year.Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        public CalendarDay GetDayOfYear(CalendarYear year, int dayOfYear)
        {
            ref readonly var chr = ref year.CalendarRef;
            chr.PreValidator.ValidateDayOfYear(year.Year, dayOfYear);
            int daysSinceEpoch = chr.Schema.CountDaysSinceEpoch(year.Year, dayOfYear);
            return new CalendarDay(daysSinceEpoch, year.Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        public CalendarDay GetEndOfYear(CalendarYear year)
        {
            ref readonly var chr = ref year.CalendarRef;
            int daysSinceEpoch = chr.Schema.GetEndOfYear(year.Year);
            return new CalendarDay(daysSinceEpoch, year.Cuid);
        }
    }

    public partial class CalendarDayProvider // CalendarMonth
    {
#if false

        /// <summary>
        /// Obtains the first day of the year to which belongs the specified month.
        /// </summary>
        [Pure]
        public CalendarDay GetStartOfYear(CalendarMonth month)
        {
            int daysSinceEpoch = month.Calendar.Schema.GetStartOfYear(month.Year);
            return new CalendarDay(daysSinceEpoch, month.Cuid);
        }

        /// <summary>
        /// Obtains the last day of the year to which belongs the specified month.
        /// </summary>
        [Pure]
        public CalendarDay GetEndOfYear(CalendarMonth month)
        {
            int daysSinceEpoch = month.Calendar.Schema.GetEndOfYear(month.Year);
            return new CalendarDay(daysSinceEpoch, month.Cuid);
        }

#endif

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<CalendarDay> GetDaysInMonth(CalendarMonth month)
        {
            var cuid = month.Cuid;
            var sch = month.Calendar.Schema;
            month.Parts.Unpack(out int y, out int m);
            int startOfMonth = sch.GetStartOfMonth(y, m);
            int daysInMonth = sch.CountDaysInMonth(y, m);

            return from daysSinceEpoch
                   in Enumerable.Range(startOfMonth, daysInMonth)
                   select new CalendarDay(daysSinceEpoch, cuid);
        }

        /// <inheritdoc/>
        [Pure]
        public CalendarDay GetStartOfMonth(CalendarMonth month)
        {
            month.Parts.Unpack(out int y, out int m);
            ref readonly var chr = ref month.CalendarRef;
            int daysSinceEpoch = chr.Schema.GetStartOfMonth(y, m);
            return new CalendarDay(daysSinceEpoch, month.Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        public CalendarDay GetDayOfMonth(CalendarMonth month, int dayOfMonth)
        {
            month.Parts.Unpack(out int y, out int m);
            ref readonly var chr = ref month.CalendarRef;
            chr.ValidateDayOfMonth(y, m, dayOfMonth);
            int daysSinceEpoch = chr.Schema.CountDaysSinceEpoch(y, m, dayOfMonth);
            return new CalendarDay(daysSinceEpoch, month.Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        public CalendarDay GetEndOfMonth(CalendarMonth month)
        {
            month.Parts.Unpack(out int y, out int m);
            ref readonly var chr = ref month.CalendarRef;
            int daysSinceEpoch = chr.Schema.GetEndOfMonth(y, m);
            return new CalendarDay(daysSinceEpoch, month.Cuid);
        }
    }
}
