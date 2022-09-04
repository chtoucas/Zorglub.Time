// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    using System.Linq;

    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Simple;
    using Zorglub.Time.Specialized;

    public static class Folklore // Friday the 13th (Gregorian calendar)
    {
        //
        // Specialized date types
        //

        [Pure]
        public static bool IsUnluckyFriday(this CivilDate date) =>
            // On vérifie d'abord le jour de la semaine (propriété la plus rapide à obtenir).
            date.DayOfWeek == DayOfWeek.Friday
            && date.Day == 13;

        [Pure]
        public static bool IsUnluckyFriday(this GregorianDate date) =>
            // On vérifie d'abord le jour de la semaine (propriété la plus rapide à obtenir).
            date.DayOfWeek == DayOfWeek.Friday
            && date.Day == 13;

        //
        // Simple date types
        //

        [Pure]
        public static bool IsUnluckyFriday(this CalendarDate date)
        {
            var chr = date.Calendar;
            return !chr.IsUserDefined
                && chr.PermanentId == CalendarId.Gregorian
                // On vérifie d'abord le jour du mois (propriété la plus rapide à obtenir).
                && date.Day == 13
                && date.DayOfWeek == DayOfWeek.Friday;
        }

        [Pure]
        public static bool IsUnluckyFriday(this CalendarDay date)
        {
            var chr = date.Calendar;
            return !chr.IsUserDefined
                && chr.PermanentId == CalendarId.Gregorian
                // On vérifie d'abord le jour de la semaine (propriété la plus rapide à obtenir).
                && date.DayOfWeek == DayOfWeek.Friday
                && date.Day == 13;
        }

        [Pure]
        public static bool IsUnluckyFriday(this OrdinalDate date)
        {
            var chr = date.Calendar;
            return !chr.IsUserDefined
                && chr.PermanentId == CalendarId.Gregorian
                && date.Day == 13
                && date.DayOfWeek == DayOfWeek.Friday;
        }

        //
        // Ranges
        //
        // No FindUnluckyFridays(Range<CivilDate> range). With CalendarDate,
        // it's useful because we have to check the Cuid.

        [Pure]
        public static IEnumerable<CalendarDate> FindUnluckyFridays(this CalendarYear year)
        {
            var chr = year.Calendar;
            if (chr.IsUserDefined == false || chr.PermanentId != CalendarId.Gregorian)
            {
                return Enumerable.Empty<CalendarDate>();
            }

            return Iterator();

            IEnumerable<CalendarDate> Iterator()
            {
                foreach (var month in year.GetAllMonths())
                {
                    // On utilise CalendarDate, mais ça aurait aussi bien marché
                    // avec OrdinalDate ou CalendarDay.
                    var date = month.GetDayOfMonth(13);
                    if (date.DayOfWeek == DayOfWeek.Friday)
                    {
                        yield return date;
                    }
                }
            }
        }

        [Pure]
        public static IEnumerable<CalendarDate> FindUnluckyFridays(this Range<CalendarDate> range)
        {
            var chr = range.GetCalendar();
            if (chr.IsUserDefined == false || chr.PermanentId != CalendarId.Gregorian)
            {
                return Enumerable.Empty<CalendarDate>();
            }

            return Iterator();

            IEnumerable<CalendarDate> Iterator()
            {
                foreach (var date in range.ToEnumerable())
                {
                    if (date.Day == 13 && date.DayOfWeek == DayOfWeek.Friday)
                    {
                        yield return date;
                    }
                }
            }
        }

        //[Pure]
        //public static IEnumerable<CalendarDate> FindUnluckyFridays(int year)
        //{
        //    for (int m = 1; m <= 12; m++)
        //    {
        //        var date = new CalendarDate(year, m, 13);
        //        if (date.DayOfWeek == DayOfWeek.Friday)
        //        {
        //            yield return date;
        //        }
        //    }
        //}
    }
}