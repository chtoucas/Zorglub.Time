// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    /// <summary>
    /// Provides helpers to compute the difference between various calendrical types.
    /// </summary>
    public sealed class DateDifference
    {
        /// <summary>
        /// Calculates the exact difference (expressed in years, months and days) between the two
        /// specified dates.
        /// </summary>
        // Pour obtenir en plus le nombre de semaines :
        // > (years, months, days / DaysPerWeek, days % DaysPerWeek);
        [Pure]
        public static (int years, int months, int days) Between(CalendarDate start, CalendarDate end)
        {
            ValidateCuid(start.Cuid, end.Cuid);

            var math = start.Calendar.Math;

            // Le résultat final est exact car on effectue les calculs de proche en proche.

            int years = math.CountYearsBetweenCore(end, start, out CalendarDate newStart1);
            int months = math.CountMonthsBetweenCore(end, newStart1, out CalendarDate newStart2);
            int days = start - newStart2;

            return (years, months, days);
        }

        /// <summary>
        /// Counts the number of years between the two specified dates.
        /// </summary>
        [Pure]
        public static int InYears(CalendarDate start, CalendarDate end, out CalendarDate newStart) =>
            start.Calendar.Math.CountYearsBetween(start, end, out newStart);

        /// <summary>
        /// Counts the number of months between the two specified dates.
        /// </summary>
        [Pure]
        public static int InMonths(CalendarDate start, CalendarDate end, out CalendarDate newStart) =>
            start.Calendar.Math.CountMonthsBetween(start, end, out newStart);

        /// <summary>
        /// Counts the number of days between the two specified dates.
        /// <para>Alias for <see cref="CalendarDate.CountDaysSince(CalendarDate)"/>.</para>
        /// </summary>
        // Pour obtenir le nombre de semaines :
        //   CountDaysBetween(start, end) / DaysPerWeek;
        [Pure]
        public static int InDays(CalendarDate start, CalendarDate end) => end - start;

        //
        // CalendarMonth
        //

        /// <summary>
        /// Counts the number of years between the two specified calendar months.
        /// </summary>
        // Fonctionne parce que CountYearsBetween() ne tient pas compte des
        // champs Month et Day.
        [Pure]
        public static int InYears(CalendarMonth start, CalendarMonth end, out CalendarMonth newStart) =>
            start.Calendar.Math.CountYearsBetween(start, end, out newStart);

        /// <summary>
        /// Counts the number of months between the two specified calendar months.
        /// <para>Alias for <see cref="CalendarMonth.CountMonthsSince(CalendarMonth)"/>.</para>
        /// </summary>
        [Pure]
        public static int InMonths(CalendarMonth start, CalendarMonth end) => end - start;

        //
        // CalendarYear
        //

        /// <summary>
        /// Counts the number of years between the two specified calendar years.
        /// <para>Alias for <see cref="CalendarYear.CountYearsSince(CalendarYear)"/>.</para>
        /// </summary>
        [Pure]
        public static int InYears(CalendarYear start, CalendarYear end) => end - start;

        private static void ValidateCuid(Cuid start, Cuid end)
        {
            if (start != end) Throw.BadCuid(nameof(end), start, end);
        }
    }
}
