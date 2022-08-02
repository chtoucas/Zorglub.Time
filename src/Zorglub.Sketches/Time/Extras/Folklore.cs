// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
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
        public static bool IsUnluckyFriday(this CivilDate @this) =>
            // On vérifie d'abord le jour de la semaine (propriété la plus rapide à obtenir).
            @this.DayOfWeek == DayOfWeek.Friday
            && @this.Day == 13;

        [Pure]
        public static bool IsUnluckyFriday(this GregorianDate @this) =>
            // On vérifie d'abord le jour de la semaine (propriété la plus rapide à obtenir).
            @this.DayOfWeek == DayOfWeek.Friday
            && @this.Day == 13;

        [Pure]
        public static bool IsUnluckyFriday(this XCivilDate @this) =>
            // On vérifie d'abord le jour de la semaine (propriété la plus rapide à obtenir).
            @this.DayOfWeek == DayOfWeek.Friday
            && @this.Day == 13;

        //
        // Simple date types
        //

        [Pure]
        public static bool IsUnluckyFriday(this CalendarDate @this)
        {
            var chr = @this.Calendar;
            return !chr.IsUserDefined
                && chr.PermanentId == CalendarId.Gregorian
                // On vérifie d'abord le jour du mois (propriété la plus rapide à obtenir).
                && @this.Day == 13
                && @this.DayOfWeek == DayOfWeek.Friday;
        }

        [Pure]
        public static bool IsUnluckyFriday(this CalendarDay @this)
        {
            var chr = @this.Calendar;
            return !chr.IsUserDefined
                && chr.PermanentId == CalendarId.Gregorian
                // On vérifie d'abord le jour de la semaine (propriété la plus rapide à obtenir).
                && @this.DayOfWeek == DayOfWeek.Friday
                && @this.Day == 13;
        }

        [Pure]
        public static bool IsUnluckyFriday(this OrdinalDate @this)
        {
            var chr = @this.Calendar;
            return !chr.IsUserDefined
                && chr.PermanentId == CalendarId.Gregorian
                && @this.Day == 13
                && @this.DayOfWeek == DayOfWeek.Friday;
        }

        //
        // Ranges
        //
        // No FindUnluckyFridays(Range<CivilDate> range). With CalendarDate,
        // it's useful because we have to check the Cuid.

        [Pure]
        public static IEnumerable<CalendarDate> FindUnluckyFridays(this CalendarYear @this)
        {
            var chr = @this.Calendar;
            if (chr.IsUserDefined == false || chr.PermanentId != CalendarId.Gregorian)
            {
                return Enumerable.Empty<CalendarDate>();
            }

            return Iterator();

            IEnumerable<CalendarDate> Iterator()
            {
                foreach (var month in @this.GetAllMonths())
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
        public static IEnumerable<CalendarDate> FindUnluckyFridays(this Range<CalendarDate> @this)
        {
            var chr = @this.GetCalendar();
            if (chr.IsUserDefined == false || chr.PermanentId != CalendarId.Gregorian)
            {
                return Enumerable.Empty<CalendarDate>();
            }

            return Iterator();

            IEnumerable<CalendarDate> Iterator()
            {
                foreach (var date in @this.ToEnumerable())
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