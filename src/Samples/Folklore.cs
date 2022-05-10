// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Samples;

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using Zorglub.Time;
using Zorglub.Time.Simple;

public static class Folklore // Friday the 13th (Gregorian calendar)
{
    [Pure]
    public static bool IsUnluckyFriday(CalendarDate date)
    {
        var chr = date.Calendar;
        return !chr.IsUserDefined
            && chr.PermanentId == CalendarId.Gregorian
            // On vérifie d'abord le jour du mois (propriété la plus rapide à obtenir).
            && date.Day == 13
            && date.DayOfWeek == DayOfWeek.Friday;
    }

    [Pure]
    public static bool IsUnluckyFriday(CalendarDay date)
    {
        var chr = date.Calendar;
        return !chr.IsUserDefined
            && chr.PermanentId == CalendarId.Gregorian
            // On vérifie d'abord le jour de la semaine (propriété la plus rapide à obtenir).
            && date.DayOfWeek == DayOfWeek.Friday
            && date.Day == 13;
    }

    [Pure]
    public static bool IsUnluckyFriday(OrdinalDate date)
    {
        var chr = date.Calendar;
        return !chr.IsUserDefined
            && chr.PermanentId == CalendarId.Gregorian
            && date.Day == 13
            && date.DayOfWeek == DayOfWeek.Friday;
    }

    [Pure]
    public static IEnumerable<CalendarDate> FindUnluckyFridays(DateRange interval!!)
    {
        var chr = interval.Calendar;
        if (!chr.IsUserDefined || chr.PermanentId != CalendarId.Gregorian)
        {
            return Enumerable.Empty<CalendarDate>();
        }

        return Iterator();

        IEnumerable<CalendarDate> Iterator()
        {
            foreach (var date in interval)
            {
                if (date.Day == 13 && date.DayOfWeek == DayOfWeek.Friday)
                {
                    yield return date;
                }
            }
        }
    }

    [Pure]
    public static IEnumerable<CalendarDate> FindUnluckyFridays(CalendarYear year)
    {
        var chr = year.Calendar;
        if (!chr.IsUserDefined || chr.PermanentId != CalendarId.Gregorian)
        {
            return Enumerable.Empty<CalendarDate>();
        }

        return Iterator();

        IEnumerable<CalendarDate> Iterator()
        {
            foreach (var month in year.GetMonthsInYear())
            {
                // On utilise CalendarDate, mais ça aurait aussi bien marché
                // avec OrdinalDate ou CalendarDay.
                var date = CalendarDate.AtDayOfMonth(month, 13);
                if (date.DayOfWeek == DayOfWeek.Friday)
                {
                    yield return date;
                }
            }
        }
    }

    [Pure]
    public static IEnumerable<CalendarDate> FindUnluckyFridays(int year)
    {
        for (int m = 1; m <= 12; m++)
        {
            var date = new CalendarDate(year, m, 13);
            if (date.DayOfWeek == DayOfWeek.Friday)
            {
                yield return date;
            }
        }
    }
}
