// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

using Zorglub.Time.Core;

/// <summary>
/// Defines the mathematical operations suitable for use by regular calendars and provides a
/// base for derived classes.
/// <para>This class uses the default <see cref="AdditionRuleset"/> to resolve ambiguities.</para>
/// </summary>
internal sealed class RegularMath : CalendarMath
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegularMath"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="calendar"/> is not regular.
    /// </exception>
    public RegularMath(SimpleCalendar calendar)
        : base(
              calendar,
              // Regular calendar => month addition is NOT ambiguous.
              // The result is exact and matches the last month of the year,
              // therefore we could specify either AdditionRule.Exact
              // or AdditionRule.Truncate. I prefer the later to
              // emphasize that this math object uses the default rules.
              default)
    {
        Debug.Assert(calendar != null);
        if (calendar.IsRegular(out _) == false) Throw.Argument(nameof(calendar));
    }

    /// <inheritdoc />
    [Pure]
    protected internal sealed override CalendarDate AddYearsCore(CalendarDate date, int years)
    {
        Debug.Assert(date.Cuid == Cuid);

        date.Parts.Unpack(out int y, out int m, out int d);
        y = checked(y + years);

        YearsValidator.CheckOverflow(y);

        // NB: AdditionRule.Truncate.
        d = Math.Min(d, Schema.CountDaysInMonth(y, m));
        return new CalendarDate(new Yemoda(y, m, d), Cuid);
    }

    /// <inheritdoc />
    [Pure]
    protected internal sealed override CalendarDate AddMonthsCore(CalendarDate date, int months)
    {
        Debug.Assert(date.Cuid == Cuid);

        var (y, m) = Arithmetic.AddMonths(date.Parts.Yemo, months);

        // NB: AdditionRule.Truncate.
        int d = Math.Min(date.Day, Schema.CountDaysInMonth(y, m));
        return new CalendarDate(new Yemoda(y, m, d), Cuid);
    }

    /// <inheritdoc />
    [Pure]
    protected internal sealed override OrdinalDate AddYearsCore(OrdinalDate date, int years)
    {
        Debug.Assert(date.Cuid == Cuid);

        date.Parts.Unpack(out int y, out int doy);
        y = checked(y + years);

        YearsValidator.CheckOverflow(y);

        // NB: AdditionRule.Truncate.
        doy = Math.Min(doy, Schema.CountDaysInYear(y));
        return new OrdinalDate(new Yedoy(y, doy), Cuid);
    }

    /// <inheritdoc />
    [Pure]
    protected internal sealed override CalendarMonth AddYearsCore(CalendarMonth month, int years)
    {
        Debug.Assert(month.Cuid == Cuid);

        month.Parts.Unpack(out int y, out int m);
        y = checked(y + years);

        YearsValidator.CheckOverflow(y);

        // NB: AdditionRule.Truncate.
        // The operation is always exact, and it's compatible with "EndOfYear".
        return new CalendarMonth(new Yemo(y, m), Cuid);
    }
}
