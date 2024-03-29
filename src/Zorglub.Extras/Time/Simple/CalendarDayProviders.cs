﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Simple;

using System.Linq;

using Zorglub.Time.Core.Intervals;

/// <summary>
/// Provides methods to obtain new <see cref="CalendarDay"/> instances for a given year or month.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class CalendarDayProviders : IDateProviders<CalendarDay>
{
    [ExcludeFromCodeCoverage(Justification = "Pseudo-static class implementing a static-only interface.")]
    private CalendarDayProviders() { }
}

public partial class CalendarDayProviders // CalendarYear
{
    /// <inheritdoc/>
    [Pure]
    public static Range<CalendarDay> ConvertToRange(CalendarYear year) =>
        Range.Create(GetStartOfYear(year), GetEndOfYear(year));

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

public partial class CalendarDayProviders // CalendarMonth
{
    /// <inheritdoc/>
    [Pure]
    public static CalendarDay GetStartOfYear(CalendarMonth month)
    {
        int daysSinceEpoch = month.Calendar.Schema.GetStartOfYear(month.Year);
        return new CalendarDay(daysSinceEpoch, month.Cuid);
    }

    /// <inheritdoc/>
    [Pure]
    public static CalendarDay GetEndOfYear(CalendarMonth month)
    {
        int daysSinceEpoch = month.Calendar.Schema.GetEndOfYear(month.Year);
        return new CalendarDay(daysSinceEpoch, month.Cuid);
    }

    /// <inheritdoc/>
    [Pure]
    public static Range<CalendarDay> ConvertToRange(CalendarMonth month) =>
        Range.Create(GetStartOfMonth(month), GetEndOfMonth(month));

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
