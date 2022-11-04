// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

// REVIEW(perf): optimize WithXXX(), idem with the other adjusters.

/// <summary>Provides a set of common adjusters for <see cref="CalendarDate"/>.
/// <para>This class cannot be inherited.</para></summary>
public static class CalendarDateAdjusters
{
    /// <summary>Obtains the start of the year to which belongs the specified date.</summary>
    [Pure]
    public static CalendarDate GetStartOfYear(this CalendarDate date) =>
        new(date.Parts.StartOfYear, date.Cuid);

    /// <summary>Obtains the end of the year to which belongs the specified date.</summary>
    [Pure]
    public static CalendarDate GetEndOfYear(this CalendarDate date)
    {
        ref readonly var chr = ref date.CalendarRef;
        var ymd = chr.Schema.GetDatePartsAtEndOfYear(date.Year);
        return new CalendarDate(ymd, date.Cuid);
    }

    /// <summary>Obtains the start of the month to which belongs the specified date.</summary>
    [Pure]
    public static CalendarDate GetStartOfMonth(this CalendarDate date) =>
        new(date.Parts.StartOfMonth, date.Cuid);

    /// <summary>Obtains the end of the month to which belongs the specified date.</summary>
    [Pure]
    public static CalendarDate GetEndOfMonth(this CalendarDate date)
    {
        date.Parts.Unpack(out int y, out int m);
        ref readonly var chr = ref date.CalendarRef;
        var ymd = chr.Schema.GetDatePartsAtEndOfMonth(y, m);
        return new CalendarDate(ymd, date.Cuid);
    }

    /// <summary>Adjusts the year field to the specified value, yielding a new date.</summary>
    /// <exception cref="AoorException">The resulting date would be invalid.</exception>
    [Pure]
    public static CalendarDate WithYear(this CalendarDate date, int newYear)
    {
        date.Parts.Deconstruct(out int _, out int m, out int d);
        ref readonly var chr = ref date.CalendarRef;
        // Even if we knew that "newYear" is valid, we must re-check "m" &
        // "d", nevertheless we don't write
        // > return chr.GetCalendarDate(newYear, m, d);
        // because we want to throw an AoorException for "newYear".
        chr.Scope.ValidateYearMonthDay(newYear, m, d, nameof(newYear));
        return new CalendarDate(newYear, m, d, date.Cuid);
    }

    /// <summary>Adjusts the month field to the specified value, yielding a new date.</summary>
    /// <exception cref="AoorException">The resulting date would be invalid.</exception>
    [Pure]
    public static CalendarDate WithMonth(this CalendarDate date, int newMonth)
    {
        date.Parts.Deconstruct(out int y, out int _, out int d);
        ref readonly var chr = ref date.CalendarRef;
        // We only need to check "newMonth" & "d".
        chr.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));
        return new CalendarDate(y, newMonth, d, date.Cuid);
    }

    /// <summary>Adjusts the day of the month field to the specified value, yielding a new date.
    /// </summary>
    /// <exception cref="AoorException">The resulting date would be invalid.</exception>
    [Pure]
    public static CalendarDate WithDay(this CalendarDate date, int newDay)
    {
        date.Parts.Unpack(out int y, out int m);
        ref readonly var chr = ref date.CalendarRef;
        // We already know that "y" & "m" are valid, we only need to check
        // "newDay".
        chr.ValidateDayOfMonth(y, m, newDay, nameof(newDay));
        return new CalendarDate(y, m, newDay, date.Cuid);
    }

    /// <summary>Adjusts the day of the year field to the specified value, yielding a new date.
    /// </summary>
    /// <exception cref="AoorException">The resulting date would be invalid.</exception>
    [Pure]
    public static CalendarDate WithDayOfYear(this CalendarDate date, int newDayOfYear) =>
        date.ToOrdinalDate().WithDayOfYear(newDayOfYear).ToCalendarDate();
}
