// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;

    /// <summary>
    /// Provides methods to obtain new <see cref="CalendarDate"/> instances in a given year or month.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed partial class CalendarDateProviders : IDateProviders<CalendarDate>
    {
        [ExcludeFromCodeCoverage(Justification = "Pseudo-static class implementing a static-only interface.")]
        private CalendarDateProviders() { }
    }

    public partial class CalendarDateProviders // CalendarYear
    {
        /// <inheritdoc/>
        [Pure]
        public static IEnumerable<CalendarDate> GetDaysInYear(CalendarYear year)
        {
            var cuid = year.Cuid;
            var sch = year.Calendar.Schema;
            int y = year.Year;

            int monthsInYear = sch.CountMonthsInYear(y);

            for (int m = 1; m <= monthsInYear; m++)
            {
                int daysInMonth = sch.CountDaysInMonth(y, m);

                for (int d = 1; d <= daysInMonth; d++)
                {
                    yield return new CalendarDate(new Yemoda(y, m, d), cuid);
                }
            }
        }

        /// <inheritdoc/>
        [Pure]
        public static CalendarDate GetStartOfYear(CalendarYear year)
        {
            var ymd = Yemoda.AtStartOfYear(year.Year);
            return new(ymd, year.Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        public static CalendarDate GetDayOfYear(CalendarYear year, int dayOfYear)
        {
            ref readonly var chr = ref year.CalendarRef;
            chr.PreValidator.ValidateDayOfYear(year.Year, dayOfYear);
            var ymd = chr.Schema.GetDateParts(year.Year, dayOfYear);
            return new CalendarDate(ymd, year.Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        public static CalendarDate GetEndOfYear(CalendarYear year)
        {
            ref readonly var chr = ref year.CalendarRef;
            var ymd = chr.Schema.GetDatePartsAtEndOfYear(year.Year);
            return new CalendarDate(ymd, year.Cuid);
        }
    }

    public partial class CalendarDateProviders // CalendarMonth
    {
        /// <inheritdoc/>
        [Pure]
        public static CalendarDate GetStartOfYear(CalendarMonth month) =>
            new(month.Parts.StartOfYear, month.Cuid);

        /// <inheritdoc/>
        [Pure]
        public static CalendarDate GetEndOfYear(CalendarMonth month)
        {
            var ymd = month.Calendar.Schema.GetDatePartsAtEndOfYear(month.Year);
            return new CalendarDate(ymd, month.Cuid);
        }

        [Pure]
        static IEnumerable<CalendarDate> IDateProviders<CalendarDate>.GetDaysInMonth(CalendarMonth month) =>
            month.GetAllDays();

        [Pure]
        static CalendarDate IDateProviders<CalendarDate>.GetStartOfMonth(CalendarMonth month) =>
            month.FirstDay;

        [Pure]
        static CalendarDate IDateProviders<CalendarDate>.GetDayOfMonth(CalendarMonth month, int dayOfMonth) =>
            month.GetDayOfMonth(dayOfMonth);

        [Pure]
        static CalendarDate IDateProviders<CalendarDate>.GetEndOfMonth(CalendarMonth month) =>
            month.LastDay;
    }
}
