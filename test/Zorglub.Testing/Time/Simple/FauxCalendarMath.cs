// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

public sealed class FauxCalendarMath : CalendarMath
{
    public FauxCalendarMath() : this(new FauxUserCalendar(), default) { }

    // Base constructors.
    public FauxCalendarMath(SimpleCalendar calendar, AdditionRuleset additionRuleset) : base(calendar, additionRuleset) { }

    // Constructors in order to test the base constructors.
    public FauxCalendarMath(SimpleCalendar calendar) : this(calendar, default) { }
    public FauxCalendarMath(AdditionRuleset additionRuleset) : this(new FauxUserCalendar(), additionRuleset) { }

    public bool AddYearsCoreDateWasCalled { get; private set; }
    public bool AddMonthsCoreDateWasCalled { get; private set; }

    public bool AddYearsCoreOrdinalDateWasCalled { get; private set; }

    public bool AddYearsCoreMonthWasCalled { get; private set; }

    [Pure]
    protected internal override CalendarDate AddYearsCore(CalendarDate date, int years)
    { AddYearsCoreDateWasCalled = true; return date; }

    [Pure]
    protected internal override CalendarDate AddMonthsCore(CalendarDate date, int months)
    { AddMonthsCoreDateWasCalled = true; return date; }

    [Pure]
    protected internal override OrdinalDate AddYearsCore(OrdinalDate date, int years)
    { AddYearsCoreOrdinalDateWasCalled = true; return date; }

    [Pure]
    protected internal override CalendarMonth AddYearsCore(CalendarMonth month, int years)
    { AddYearsCoreMonthWasCalled = true; return month; }
}
