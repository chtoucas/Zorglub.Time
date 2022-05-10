// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

public sealed class FauxCalendarMath : CalendarMath
{
    public FauxCalendarMath(Calendar calendar) : base(calendar) { }

    public bool AddMonthsCoreWasCalled { get; private set; }
    public bool AddYearsCoreWasCalled { get; private set; }
    public bool CountYearsBetweenCoreWasCalled { get; private set; }
    public bool CountMonthsBetweenCoreWasCalled { get; private set; }

    public bool AddMonthsCore1WasCalled { get; private set; }
    public bool AddYearsCore1WasCalled { get; private set; }
    public bool CountYearsBetweenCore1WasCalled { get; private set; }
    public bool CountMonthsBetweenCore1WasCalled { get; private set; }

    public override AddAdjustment AddAdjustment => AddAdjustment.EndOfMonth;

    //
    // CalendarDate
    //

    [Pure]
    protected internal override CalendarDate AddYearsCore(CalendarDate date, int years)
    {
        AddYearsCoreWasCalled = true;
        return date;
    }

    [Pure]
    protected internal override CalendarDate AddMonthsCore(CalendarDate date, int months)
    {
        AddMonthsCoreWasCalled = true;
        return date;
    }

    [Pure]
    protected internal override int CountYearsBetweenCore(CalendarDate start, CalendarDate end)
    {
        CountYearsBetweenCoreWasCalled = true;
        return 0;
    }

    [Pure]
    protected internal override int CountMonthsBetweenCore(CalendarDate start, CalendarDate end)
    {
        CountMonthsBetweenCoreWasCalled = true;
        return 0;
    }

    //
    // OrdinalDate
    //

    [Pure]
    protected internal override OrdinalDate AddYearsCore(OrdinalDate date, int years)
    {
        throw new NotImplementedException();
    }

    [Pure]
    protected internal override int CountYearsBetweenCore(OrdinalDate start, OrdinalDate end)
    {
        throw new NotImplementedException();
    }

    //
    // CalendarMonth
    //

    [Pure]
    protected internal override CalendarMonth AddMonthsCore(CalendarMonth month, int months)
    {
        AddMonthsCore1WasCalled = true;
        return month;
    }

    [Pure]
    protected internal override CalendarMonth AddYearsCore(CalendarMonth month, int years)
    {
        AddYearsCore1WasCalled = true;
        return month;
    }

    [Pure]
    protected internal override int CountYearsBetweenCore(CalendarMonth start, CalendarMonth end)
    {
        CountYearsBetweenCore1WasCalled = false;
        return 0;
    }

    [Pure]
    protected internal override int CountMonthsBetweenCore(CalendarMonth start, CalendarMonth end)
    {
        CountMonthsBetweenCore1WasCalled = true;
        return 0;
    }
}
