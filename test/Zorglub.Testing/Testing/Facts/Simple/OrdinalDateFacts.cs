// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

// Tests indirects : OrdinalDate utilise la repr (y, doy) mais ici on en
// passe systématiquement par NewDate(y, m, d).

/// <summary>
/// Provides facts about <see cref="OrdinalDate"/>.
/// </summary>
[TestExcludeFrom(TestExcludeFrom.Smoke)] // Indirect tests
public abstract class OrdinalDateFacts<TDataSet> : IDateFacts<OrdinalDate, TDataSet>
    where TDataSet :
        ICalendarDataSet,
        IDaysAfterDataSet,
        IMathDataSet,
        IDayOfWeekDataSet,
        ISingleton<TDataSet>
{
    protected OrdinalDateFacts(Calendar calendar) : this(calendar, CreateCtorArgs(calendar)) { }

    private OrdinalDateFacts(Calendar calendar, CtorArgs args) : base(args)
    {
        CalendarUT = calendar;

        (MinDate, MaxDate) = calendar.MinMaxOrdinal;
    }

    protected Calendar CalendarUT { get; }

    protected sealed override OrdinalDate MinDate { get; }
    protected sealed override OrdinalDate MaxDate { get; }

    protected sealed override OrdinalDate CreateDate(int y, int m, int d) =>
        CalendarUT.GetCalendarDate(y, m, d).ToOrdinalDate();
}

///// <summary>
///// Provides data-driven tests for <see cref="OrdinalDate"/> and provides a
///// base for derived classes.
///// </summary>
//internal abstract partial class AbstractOrdinalDateTests<TDataSet>
//    : AbstractIsaFixedDateTests<OrdinalDate, TDataSet>
//    where TDataSet : CalendricalData, IMostCalendricalData, ISingleton<TDataSet>
//{
//    protected AbstractOrdinalDateTests(Calendar calendar)
//        : this(calendar, NewCtorArgs(calendar)) { }

//    protected AbstractOrdinalDateTests(Calendar calendar, CtorArgs args) : base(args)
//    {
//        CalendarUT = calendar;

//        var minYear = calendar.NewCalendarYear(calendar.MinSupportedYear);
//        var maxYear = calendar.NewCalendarYear(calendar.MaxSupportedYear);

//        MinDate = OrdinalDate.AtStartOfYear(minYear);
//        MaxDate = OrdinalDate.AtEndOfYear(maxYear);
//    }

//    protected Calendar CalendarUT { get; }

//    protected sealed override OrdinalDate MinDate { get; }
//    protected sealed override OrdinalDate MaxDate { get; }

//    protected sealed override OrdinalDate NewDate(int y, int m, int d) =>
//        CalendarUT.NewCalendarDate(y, m, d).ToOrdinalDate();

//    protected sealed override FixedDateProxy<OrdinalDate> NewDateProxy(int y, int m, int d) =>
//        FixedDateProxy.Of(CalendarUT.NewCalendarDate(y, m, d).ToOrdinalDate());

//    protected sealed override OrdinalDate Op_Addition(OrdinalDate date, int days) => date + days;
//    protected sealed override OrdinalDate Op_Subtraction(OrdinalDate date, int days) => date - days;
//    protected sealed override int Op_Subtraction(OrdinalDate date, OrdinalDate other) => date - other;

//    protected sealed override OrdinalDate Op_Increment(OrdinalDate date)
//    {
//        date++;
//        return date;
//    }

//    protected sealed override OrdinalDate Op_Decrement(OrdinalDate date)
//    {
//        date--;
//        return date;
//    }
//}
