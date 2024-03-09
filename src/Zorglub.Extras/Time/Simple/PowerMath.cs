// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Simple;

using Zorglub.Time.Core;

// TODO(code): use a custom exn when using the rule AdditionRule.Throw.

/// <summary>
/// Provides non-standard mathematical operations suitable for use with a given calendar.
/// <para>This class allows to customize the <see cref="AdditionRuleset"/> used to resolve
/// ambiguities.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class PowerMath : CalendarMath
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PowerMath"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
    public PowerMath(SimpleCalendar calendar, AdditionRuleset additionRuleset)
        : base(calendar, additionRuleset) { }

    /// <inheritdoc/>
    [Pure]
    protected internal override CalendarDate AddYearsCore(CalendarDate date, int years)
    {
        Debug.Assert(date.Cuid == Cuid);

        var ymd = Arithmetic.AddYears(date.Parts, years, out int roundoff);
        if (roundoff > 0) { ymd = Adjust(ymd, roundoff); }

        return new CalendarDate(ymd, Cuid);
    }

    /// <inheritdoc/>
    [Pure]
    protected internal override CalendarDate AddMonthsCore(CalendarDate date, int months)
    {
        Debug.Assert(date.Cuid == Cuid);

        var ymd = Arithmetic.AddMonths(date.Parts, months, out int roundoff);
        if (roundoff > 0) { ymd = Adjust(ymd, roundoff); }

        return new CalendarDate(ymd, Cuid);
    }

    /// <inheritdoc/>
    [Pure]
    protected internal override OrdinalDate AddYearsCore(OrdinalDate date, int years)
    {
        Debug.Assert(date.Cuid == Cuid);

        var ydoy = Arithmetic.AddYears(date.Parts, years, out int roundoff);
        if (roundoff > 0) { ydoy = Adjust(ydoy, roundoff); }

        return new OrdinalDate(ydoy, Cuid);
    }

    /// <inheritdoc/>
    [Pure]
    protected internal override CalendarMonth AddYearsCore(CalendarMonth month, int years)
    {
        Debug.Assert(month.Cuid == Cuid);

        var ym = Arithmetic.AddYears(month.Parts, years, out int roundoff);
        if (roundoff > 0) { ym = Adjust(ym, roundoff); }

        return new CalendarMonth(ym, Cuid);
    }

    //
    // Adjustments
    //
    // Résultat rectifié en fonction de la règle sélectionnée.

    [Pure]
    private Yemoda Adjust(Yemoda ymd, int roundoff)
    {
        // Si on ne filtrait pas roundoff > 0, il faudrait prendre en compte
        // le cas roundoff = 0 et retourner ymd (résultat exact).
        Debug.Assert(roundoff > 0);

        // NB: according to CalendricalMath, ymd is the last day of the month.
        return AdditionRuleset.DateRule switch
        {
            AdditionRule.Truncate => ymd,
            AdditionRule.Overspill => Arithmetic.AddDays(ymd, 1),
            AdditionRule.Exact => Arithmetic.AddDays(ymd, roundoff),
            AdditionRule.Overflow => Throw.DateOverflow<Yemoda>(),

            _ => Throw.InvalidOperation<Yemoda>(),
        };
    }

    [Pure]
    private Yedoy Adjust(Yedoy ydoy, int roundoff)
    {
        // Si on ne filtrait pas roundoff > 0, il faudrait prendre en compte
        // le cas roundoff = 0 et retourner ydoy (résultat exact).
        Debug.Assert(roundoff > 0);

        // NB: according to CalendricalMath, ydoy is the last day of the year.
        return AdditionRuleset.OrdinalRule switch
        {
            AdditionRule.Truncate => ydoy,
            AdditionRule.Overspill => Arithmetic.AddDays(ydoy, 1),
            AdditionRule.Exact => Arithmetic.AddDays(ydoy, roundoff),
            AdditionRule.Overflow => Throw.DateOverflow<Yedoy>(),

            _ => Throw.InvalidOperation<Yedoy>(),
        };
    }

    [Pure]
    private Yemo Adjust(Yemo ym, int roundoff)
    {
        // Si on ne filtrait pas roundoff > 0, il faudrait prendre en compte
        // le cas roundoff = 0 et retourner ym (résultat exact).
        Debug.Assert(roundoff > 0);

        // NB: according to CalendricalMath, ym is the last month of the year.
        return AdditionRuleset.MonthRule switch
        {
            AdditionRule.Truncate => ym,
            AdditionRule.Overspill => Arithmetic.AddMonths(ym, 1),
            AdditionRule.Exact => Arithmetic.AddMonths(ym, roundoff),
            AdditionRule.Overflow => Throw.MonthOverflow<Yemo>(),

            _ => Throw.InvalidOperation<Yemo>(),
        };
    }
}
