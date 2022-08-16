// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time;

using System.Diagnostics.Contracts;

using Zorglub.Time.Core.Schemas;

// There is only one advantage to have these methods: data is already validated.
// Notice that it's true because years are complete which means, for instance,
// that GetStartOfYear() is always valid.

public static class ZDateAdjusters
{
    [Pure]
    public static ZDate GetStartOfYear(this ZDate date)
    {
        ref readonly var chr = ref date.CalendarRef;
        var sch = chr.Schema;
        int y = sch.GetYear(date.DaysSinceEpoch, out _);
        var startOfYear = sch.GetStartOfYear(y);
        return new ZDate(startOfYear, date.Cuid);
    }

    [Pure]
    public static ZDate GetEndOfYear(this ZDate date)
    {
        ref readonly var chr = ref date.CalendarRef;
        var sch = chr.Schema;
        int y = sch.GetYear(date.DaysSinceEpoch, out _);
        var endOfYear = sch.GetEndOfYear(y);
        return new ZDate(endOfYear, date.Cuid);
    }

    [Pure]
    public static ZDate GetStartOfMonth(this ZDate date)
    {
        ref readonly var chr = ref date.CalendarRef;
        var sch = chr.Schema;
        sch.GetDateParts(date.DaysSinceEpoch, out int y, out int m, out _);
        var startOfMonth = sch.GetStartOfMonth(y, m);
        return new ZDate(startOfMonth, date.Cuid);
    }

    [Pure]
    public static ZDate GetEndOfMonth(this ZDate date)
    {
        ref readonly var chr = ref date.CalendarRef;
        var sch = chr.Schema;
        sch.GetDateParts(date.DaysSinceEpoch, out int y, out int m, out _);
        var endOfMonth = sch.GetEndOfMonth(y, m);
        return new ZDate(endOfMonth, date.Cuid);
    }

    [Pure]
    public static ZDate WithYear(this ZDate date, int newYear)
    {
        ref readonly var chr = ref date.CalendarRef;
        var sch = chr.Schema;
        sch.GetDateParts(date.DaysSinceEpoch, out _, out int m, out int d);
        chr.Scope.ValidateYearMonthDay(newYear, m, d, nameof(newYear));
        int daysSinceEpoch = sch.CountDaysSinceEpoch(newYear, m, d);
        return new ZDate(daysSinceEpoch, date.Cuid);
    }

    [Pure]
    public static ZDate WithMonth(this ZDate date, int newMonth)
    {
        ref readonly var chr = ref date.CalendarRef;
        var sch = chr.Schema;
        sch.GetDateParts(date.DaysSinceEpoch, out int y, out int m, out int d);
        sch.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));
        int daysSinceEpoch = sch.CountDaysSinceEpoch(y, newMonth, d);
        return new ZDate(daysSinceEpoch, date.Cuid);
    }

    [Pure]
    public static ZDate WithDay(this ZDate date, int newDay)
    {
        ref readonly var chr = ref date.CalendarRef;
        var sch = chr.Schema;
        sch.GetDateParts(date.DaysSinceEpoch, out int y, out int m, out _);
        if (newDay < 1
            || (newDay > sch.MinDaysInMonth
                && newDay > sch.CountDaysInMonth(y, m)))
        {
            Throw.ArgumentOutOfRange(nameof(newDay));
        }
        int daysSinceEpoch = sch.CountDaysSinceEpoch(y, m, newDay);
        return new ZDate(daysSinceEpoch, date.Cuid);
    }

    [Pure]
    public static ZDate WithDayOfYear(this ZDate date, int newDayOfYear)
    {
        ref readonly var chr = ref date.CalendarRef;
        var sch = chr.Schema;
        int y = sch.GetYear(date.DaysSinceEpoch, out _);
        sch.PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));
        int daysSinceEpoch = sch.CountDaysSinceEpoch(y, newDayOfYear);
        return new ZDate(daysSinceEpoch, date.Cuid);
    }
}
