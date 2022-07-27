// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Samples;

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

// Verification that one can create a calendar type without having access to
// the internals.

public sealed partial class MyCalendar : BasicCalendar, ICalendar<MyDate>
{
    public MyCalendar() : this(SchemaActivator.CreateInstance<CivilSchema>()) { }

    public MyCalendar(CivilSchema schema)
        : base("MyCalendar", new StandardScope(schema, DayZero.NewStyle)) { }
}

public partial class MyCalendar // Year, month or day infos
{
    [Pure]
    public override int CountMonthsInYear(int year)
    {
        SupportedYears.Validate(year);
        return Schema.CountMonthsInYear(year);
    }

    [Pure]
    public override int CountDaysInYear(int year)
    {
        SupportedYears.Validate(year);
        return Schema.CountDaysInYear(year);
    }

    [Pure]
    public override int CountDaysInMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        return Schema.CountDaysInMonth(year, month);
    }
}

public partial class MyCalendar // Factories
{
    [Pure]
    MyDate ICalendar<MyDate>.Today() => MyDate.Today();
}

public partial class MyCalendar // Dates in a given year or month
{
    [Pure]
    public IEnumerable<MyDate> GetDaysInYear(int year)
    {
        SupportedYears.Validate(year);

        int startOfYear = Schema.GetStartOfYear(year);
        int daysInYear = Schema.CountDaysInYear(year);

        return from daysSinceEpoch
               in Enumerable.Range(startOfYear, daysInYear)
               select new MyDate(daysSinceEpoch);
    }

    [Pure]
    public IEnumerable<MyDate> GetDaysInMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);

        int startOfMonth = Schema.GetStartOfMonth(year, month);
        int daysInMonth = Schema.CountDaysInMonth(year, month);

        return from daysSinceEpoch
               in Enumerable.Range(startOfMonth, daysInMonth)
               select new MyDate(daysSinceEpoch);
    }

    [Pure]
    public MyDate GetStartOfYear(int year)
    {
        SupportedYears.Validate(year);
        int daysSinceEpoch = Schema.GetStartOfYear(year);
        return new MyDate(daysSinceEpoch);
    }

    [Pure]
    public MyDate GetEndOfYear(int year)
    {
        SupportedYears.Validate(year);
        int daysSinceEpoch = Schema.GetEndOfYear(year);
        return new MyDate(daysSinceEpoch);
    }

    [Pure]
    public MyDate GetStartOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        int daysSinceEpoch = Schema.GetStartOfMonth(year, month);
        return new MyDate(daysSinceEpoch);
    }

    [Pure]
    public MyDate GetEndOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        int daysSinceEpoch = Schema.GetEndOfMonth(year, month);
        return new MyDate(daysSinceEpoch);
    }
}
