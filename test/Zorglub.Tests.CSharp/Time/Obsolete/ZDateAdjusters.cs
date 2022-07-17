// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology;

using System.Diagnostics.Contracts;

// There is only one advantage to have these methods: data is already validated.
// Notice that it's true because years are complete which means, for instance,
// that GetStartOfYear() is always valid.
[Obsolete("ZDate is part of the ZCalendar system which implements IDayProvider.")]
public static class ZDateAdjusters
{
    /// <summary>
    /// Obtains the start of the year to which belongs the specified date.
    /// </summary>
    [Pure]
    public static ZDate GetStartOfYear(ZDate date)
    {
        ref readonly var chr = ref date.CalendarRef;
        var sch = chr.Schema;
        int y = sch.GetYear(date.DaysSinceEpoch, out _);
        var startOfYear = sch.GetStartOfYear(y);
        return new ZDate(startOfYear, date.ZIdent);
    }

    /// <summary>
    /// Obtains the end of the year to which belongs the specified date.
    /// </summary>
    [Pure]
    public static ZDate GetEndOfYear(ZDate date)
    {
        ref readonly var chr = ref date.CalendarRef;
        var sch = chr.Schema;
        int y = sch.GetYear(date.DaysSinceEpoch, out _);
        var endOfYear = sch.GetEndOfYear(y);
        return new ZDate(endOfYear, date.ZIdent);
    }

    /// <summary>
    /// Obtains the start of the month to which belongs the specified date.
    /// </summary>
    [Pure]
    public static ZDate GetStartOfMonth(ZDate date)
    {
        ref readonly var chr = ref date.CalendarRef;
        var sch = chr.Schema;
        sch.GetDateParts(date.DaysSinceEpoch, out int y, out int m, out _);
        var startOfMonth = sch.GetStartOfMonth(y, m);
        return new ZDate(startOfMonth, date.ZIdent);
    }

    /// <summary>
    /// Obtains the end of the month to which belongs the specified date.
    /// </summary>
    [Pure]
    public static ZDate GetEndOfMonth(ZDate date)
    {
        ref readonly var chr = ref date.CalendarRef;
        var sch = chr.Schema;
        sch.GetDateParts(date.DaysSinceEpoch, out int y, out int m, out _);
        var endOfMonth = sch.GetEndOfMonth(y, m);
        return new ZDate(endOfMonth, date.ZIdent);
    }

}
