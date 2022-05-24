// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using System.Linq;

    /// <summary>
    /// Provides static helpers related to <see cref="CalendarDay"/>.
    /// </summary>
    public sealed partial class CalendarDayHelper : IDateHelper<CalendarDay>
    {
        public CalendarDayHelper(CalendarDay day)
        {
            Day = day;
        }

        public CalendarDay Day { get; }
    }

    public partial class CalendarDayHelper //
    {
        /// <inheritdoc/>
        [Pure]
        public CalendarDay GetStartOfYear()
        {
            ref readonly var chr = ref Day.CalendarRef;
            int daysSinceEpoch = chr.Schema.GetStartOfYear(Day.Year);
            return new CalendarDay(daysSinceEpoch, Day.Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        public CalendarDay GetEndOfYear()
        {
            ref readonly var chr = ref Day.CalendarRef;
            int daysSinceEpoch = chr.Schema.GetEndOfYear(Day.Year);
            return new CalendarDay(daysSinceEpoch, Day.Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        public CalendarDay GetStartOfMonth()
        {
            ref readonly var chr = ref Day.CalendarRef;
            Day.Unpack(chr, out int y, out int m, out _);
            int daysSinceEpoch = chr.Schema.GetStartOfMonth(y, m);
            return new CalendarDay(daysSinceEpoch, Day.Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        public CalendarDay GetEndOfMonth()
        {
            ref readonly var chr = ref Day.CalendarRef;
            Day.Unpack(chr, out int y, out int m, out _);
            int daysSinceEpoch = chr.Schema.GetEndOfMonth(y, m);
            return new CalendarDay(daysSinceEpoch, Day.Cuid);
        }
    }

    public partial class CalendarDayHelper // CalendarYear
    {
        /// <inheritdoc/>
        [Pure]
        public static IEnumerable<CalendarDay> GetDaysInYear(CalendarYear year)
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
        public static CalendarDay GetStartOfYear(CalendarYear year)
        {
            ref readonly var chr = ref year.CalendarRef;
            int daysSinceEpoch = chr.Schema.GetStartOfYear(year.Year);
            return new CalendarDay(daysSinceEpoch, year.Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        public static CalendarDay GetDayOfYear(CalendarYear year, int dayOfYear)
        {
            ref readonly var chr = ref year.CalendarRef;
            chr.PreValidator.ValidateDayOfYear(year.Year, dayOfYear);
            int daysSinceEpoch = chr.Schema.CountDaysSinceEpoch(year.Year, dayOfYear);
            return new CalendarDay(daysSinceEpoch, year.Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        public static CalendarDay GetEndOfYear(CalendarYear year)
        {
            ref readonly var chr = ref year.CalendarRef;
            int daysSinceEpoch = chr.Schema.GetEndOfYear(year.Year);
            return new CalendarDay(daysSinceEpoch, year.Cuid);
        }
    }

    public partial class CalendarDayHelper // CalendarMonth
    {
#if false

        /// <summary>
        /// Obtains the first day of the year to which belongs the specified month.
        /// </summary>
        [Pure]
        public static CalendarDay GetStartOfYear(CalendarMonth month)
        {
            int daysSinceEpoch = month.Calendar.Schema.GetStartOfYear(month.Year);
            return new CalendarDay(daysSinceEpoch, month.Cuid);
        }

        /// <summary>
        /// Obtains the last day of the year to which belongs the specified month.
        /// </summary>
        [Pure]
        public static CalendarDay GetEndOfYear(CalendarMonth month)
        {
            int daysSinceEpoch = month.Calendar.Schema.GetEndOfYear(month.Year);
            return new CalendarDay(daysSinceEpoch, month.Cuid);
        }

#endif

        /// <inheritdoc/>
        [Pure]
        public static IEnumerable<CalendarDay> GetDaysInMonth(CalendarMonth month)
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
        public static CalendarDay GetStartOfMonth(CalendarMonth month)
        {
            month.Parts.Unpack(out int y, out int m);
            ref readonly var chr = ref month.CalendarRef;
            int daysSinceEpoch = chr.Schema.GetStartOfMonth(y, m);
            return new CalendarDay(daysSinceEpoch, month.Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        public static CalendarDay GetDayOfMonth(CalendarMonth month, int dayOfMonth)
        {
            month.Parts.Unpack(out int y, out int m);
            ref readonly var chr = ref month.CalendarRef;
            chr.ValidateDayOfMonth(y, m, dayOfMonth);
            int daysSinceEpoch = chr.Schema.CountDaysSinceEpoch(y, m, dayOfMonth);
            return new CalendarDay(daysSinceEpoch, month.Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        public static CalendarDay GetEndOfMonth(CalendarMonth month)
        {
            month.Parts.Unpack(out int y, out int m);
            ref readonly var chr = ref month.CalendarRef;
            int daysSinceEpoch = chr.Schema.GetEndOfMonth(y, m);
            return new CalendarDay(daysSinceEpoch, month.Cuid);
        }
    }
}
