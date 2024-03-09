// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Simple;

// Méthodes Between() : les résultats sont exacts car on effectue les
// calculs de proche en proche.
//
// Pour obtenir en plus le nombre de semaines :
// > (years, months, days / DaysPerWeek, days % DaysPerWeek);
// > CountDaysBetween(start, end) / DaysPerWeek;
//
// Aliases: we copy the code from calenrical type. For instance, when
// CalendarDate throws it considers "start" to be culprit, whereas here
// "end" is the one we blame for the error.

/// <summary>
/// Provides helpers to compute the difference between various calendrical types.
/// </summary>
public static class Difference
{
    //
    // CalendarDate
    //

    /// <summary>
    /// Calculates the exact difference (expressed in years, months and days) between the two
    /// specified dates.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="end"/> belongs to a different
    /// calendar from that of <paramref name="start"/>.</exception>
    [Pure]
    public static (int Years, int Months, int Days) Between(CalendarDate start, CalendarDate end)
    {
        ValidateCuid(start.Cuid, end.Cuid);

        ref readonly var chr = ref start.CalendarRef;
        var math = chr.Math;
        int years = math.CountYearsBetweenCore(end, start, out CalendarDate newStart1);
        int months = math.CountMonthsBetweenCore(end, newStart1, out CalendarDate newStart2);
        int days = chr.Arithmetic.CountDaysBetween(end.Parts, newStart2.Parts);
        return (years, months, days);
    }

    /// <summary>
    /// Counts the number of years between the two specified dates.
    /// <para>Alias for
    /// <see cref="CalendarMath.CountYearsBetween(CalendarDate, CalendarDate, out CalendarDate)"/>.
    /// </para>
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="end"/> belongs to a different
    /// calendar from that of <paramref name="start"/>.</exception>
    [Pure]
    public static int InYears(CalendarDate start, CalendarDate end, out CalendarDate newStart)
    {
        ValidateCuid(start.Cuid, end.Cuid);

        ref readonly var chr = ref start.CalendarRef;
        return chr.Math.CountYearsBetweenCore(start, end, out newStart);
    }

    /// <summary>
    /// Counts the number of months between the two specified dates.
    /// <para>Alias for
    /// <see cref="CalendarMath.CountMonthsBetween(CalendarDate, CalendarDate, out CalendarDate)"/>.
    /// </para>
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="end"/> belongs to a different
    /// calendar from that of <paramref name="start"/>.</exception>
    [Pure]
    public static int InMonths(CalendarDate start, CalendarDate end, out CalendarDate newStart)
    {
        ValidateCuid(start.Cuid, end.Cuid);

        ref readonly var chr = ref start.CalendarRef;
        return chr.Math.CountMonthsBetweenCore(start, end, out newStart);
    }

    /// <summary>
    /// Counts the number of days between the two specified dates.
    /// <para>Alias for <see cref="CalendarDate.CountDaysSince(CalendarDate)"/>.</para>
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="end"/> belongs to a different
    /// calendar from that of <paramref name="start"/>.</exception>
    [Pure]
    public static int InDays(CalendarDate start, CalendarDate end)
    {
        ValidateCuid(start.Cuid, end.Cuid);

        ref readonly var chr = ref start.CalendarRef;
        return chr.Arithmetic.CountDaysBetween(end.Parts, start.Parts);
    }

    //
    // OrdinalDate
    //

    /// <summary>
    /// Calculates the exact difference (expressed in years, months and days) between the two
    /// specified ordinal dates.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="end"/> belongs to a different
    /// calendar from that of <paramref name="start"/>.</exception>
    // Pour obtenir en plus le nombre de semaines :
    // > (years, months, days / DaysPerWeek, days % DaysPerWeek);
    [Pure]
    public static (int Years, int Days) Between(OrdinalDate start, OrdinalDate end)
    {
        ValidateCuid(start.Cuid, end.Cuid);

        ref readonly var chr = ref start.CalendarRef;
        int years = chr.Math.CountYearsBetweenCore(end, start, out OrdinalDate newStart);
        int days = chr.Arithmetic.CountDaysBetween(end.Parts, newStart.Parts);
        return (years, days);
    }

    /// <summary>
    /// Counts the number of years between the two specified ordinal dates.
    /// <para>Alias for
    /// <see cref="CalendarMath.CountYearsBetween(OrdinalDate, OrdinalDate, out OrdinalDate)"/>.
    /// </para>
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="end"/> belongs to a different
    /// calendar from that of <paramref name="start"/>.</exception>
    [Pure]
    public static int InYears(OrdinalDate start, OrdinalDate end, out OrdinalDate newStart)
    {
        ValidateCuid(start.Cuid, end.Cuid);

        ref readonly var chr = ref start.CalendarRef;
        return chr.Math.CountYearsBetweenCore(start, end, out newStart);
    }

    /// <summary>
    /// Counts the number of days between the two specified ordinal dates.
    /// <para>Alias for <see cref="OrdinalDate.CountDaysSince(OrdinalDate)"/>.</para>
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="end"/> belongs to a different
    /// calendar from that of <paramref name="start"/>.</exception>
    // Pour obtenir le nombre de semaines :
    //   CountDaysBetween(start, end) / DaysPerWeek;
    [Pure]
    public static int InDays(OrdinalDate start, OrdinalDate end)
    {
        ValidateCuid(start.Cuid, end.Cuid);

        ref readonly var chr = ref start.CalendarRef;
        return chr.Arithmetic.CountDaysBetween(end.Parts, start.Parts);
    }

    //
    // CalendarMonth
    //

    /// <summary>
    /// Calculates the exact difference (expressed in years, months and days) between the two
    /// specified months.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="end"/> belongs to a different
    /// calendar from that of <paramref name="start"/>.</exception>
    [Pure]
    public static (int Years, int Months) Between(CalendarMonth start, CalendarMonth end)
    {
        ValidateCuid(start.Cuid, end.Cuid);

        ref readonly var chr = ref start.CalendarRef;
        int years = chr.Math.CountYearsBetweenCore(end, start, out CalendarMonth newStart);
        int months = chr.Arithmetic.CountMonthsBetween(end.Parts, newStart.Parts);
        return (years, months);
    }

    /// <summary>
    /// Counts the number of years between the two specified calendar months.
    /// <para>Alias for
    /// <see cref="CalendarMath.CountYearsBetween(CalendarMonth, CalendarMonth, out CalendarMonth)"/>.
    /// </para>
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="end"/> belongs to a different
    /// calendar from that of <paramref name="start"/>.</exception>
    [Pure]
    public static int InYears(CalendarMonth start, CalendarMonth end, out CalendarMonth newStart)
    {
        ValidateCuid(start.Cuid, end.Cuid);

        ref readonly var chr = ref start.CalendarRef;
        return chr.Math.CountYearsBetweenCore(start, end, out newStart);
    }

    /// <summary>
    /// Counts the number of months between the two specified calendar months.
    /// <para>Alias for <see cref="CalendarMonth.CountMonthsSince(CalendarMonth)"/>.</para>
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="end"/> belongs to a different
    /// calendar from that of <paramref name="start"/>.</exception>
    [Pure]
    public static int InMonths(CalendarMonth start, CalendarMonth end)
    {
        ValidateCuid(start.Cuid, end.Cuid);

        ref readonly var chr = ref start.CalendarRef;
        return chr.Arithmetic.CountMonthsBetween(end.Parts, start.Parts);
    }

    //
    // CalendarYear
    //

    /// <summary>
    /// Counts the number of years between the two specified calendar years.
    /// <para>Alias for <see cref="CalendarYear.CountYearsSince(CalendarYear)"/>.</para>
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="end"/> belongs to a different
    /// calendar from that of <paramref name="start"/>.</exception>
    [Pure]
    public static int InYears(CalendarYear start, CalendarYear end)
    {
        ValidateCuid(start.Cuid, end.Cuid);

        return end.Year - start.Year;
    }

    private static void ValidateCuid(Cuid start, Cuid end)
    {
        if (start != end) ThrowHelpers.BadCuid(nameof(end), start, end);
    }
}
