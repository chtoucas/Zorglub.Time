// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#if false

namespace Zorglub.Time.Simple
{
    /// <summary>
    /// Provides helpers to compute the difference between various calendrical
    /// types.
    /// </summary>
    public static class DateDifference
    {
        /// <summary>
        /// Calculates the exact difference (expressed in years, months and days)
        /// between the two specified dates.
        /// </summary>
        // Pour obtenir en plus le nombre de semaines :
        // > (years, months, days / DaysPerWeek, days % DaysPerWeek);
        [Pure]
        public static (int years, int months, int days) Between(CalendarDate start, CalendarDate end)
        {
            ValidateCuid(start.Cuid, end.Cuid);

            var chr = CalendarCatalog.GetCalendar(start.Cuid);
            return chr.Ops.GetDifferenceBetween(start, end);
        }

        /// <summary>
        /// Counts the number of years between the two specified dates.
        /// </summary>
        [Pure]
        public static int InYears(CalendarDate start, CalendarDate end, out CalendarDate newStart)
        {
            ValidateCuid(start.Cuid, end.Cuid);

            var chr = CalendarCatalog.GetCalendar(start.Cuid);
            int years = chr.Ops.CountYearsBetween(start, end, out newStart);
            return years;
        }

        /// <summary>
        /// Counts the number of months between the two specified dates.
        /// </summary>
        [Pure]
        public static int InMonths(CalendarDate start, CalendarDate end, out CalendarDate newStart)
        {
            ValidateCuid(start.Cuid, end.Cuid);

            var chr = CalendarCatalog.GetCalendar(start.Cuid);
            int months = chr.Ops.CountMonthsBetween(start, end, out newStart);
            return months;
        }

        /// <summary>
        /// Counts the number of days between the two specified dates.
        /// </summary>
        // Pour obtenir le nombre de semaines :
        //   CountDaysBetween(start, end) / DaysPerWeek;
        // Alias pour CalendarDate.Minus().
        [Pure]
        public static int InDays(CalendarDate start, CalendarDate end) => end - start;

        //
        // Mois.
        //

        ///// <summary>
        ///// Counts the number of years between the two specified calendar
        ///// months.
        ///// </summary>
        //// Fonctionne parce que CountYearsBetween() ne tient pas compte des
        //// champs Month et Day.
        //[Pure]
        //public static int InYears(CalendarMonth start, CalendarMonth end)
        //{
        //    ValidateCuid(start.Cuid, end.Cuid);

        //    return start.Calendar.Ops.CountYearsBetween(start, end);
        //}

        ///// <summary>
        ///// Counts the number of months between the two specified calendar
        ///// months.
        ///// <para>Alias for <see cref="CalendarMonth.Minus(CalendarMonth)"/>.
        ///// </para>
        ///// </summary>
        //[Pure]
        //public static int InMonths(CalendarMonth start, CalendarMonth end) => end - start;

        ////
        //// Années.
        ////

        ///// <summary>
        ///// Counts the number of years between the two specified calendar
        ///// years.
        ///// <para>Alias for <see cref="CalendarYear.Minus(CalendarYear)"/>.
        ///// </para>
        ///// </summary>
        //[Pure]
        //public static int InYears(CalendarYear start, CalendarYear end) => end - start;

        private static void ValidateCuid(Cuid start, Cuid end)
        {
            if (start != end)
            {
                Throw.InvalidEnum(nameof(end), start, end);
            }
        }
    }
}

#endif