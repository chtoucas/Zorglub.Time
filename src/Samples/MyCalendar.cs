// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Samples;

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

using Zorglub.Time.Core;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Core.Utilities;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

// Verification that one can create a calendar type without having access to
// the internals.

public sealed partial class MyCalendar : BasicCalendar<CalendarScope>, ICalendar<MyDate>
{
    private readonly SystemSchema _schema;

    public MyCalendar() : this(MyDate.Context) { }

    private MyCalendar(CalendarScope<GregorianSchema> context) : base("MyCalendar", context.Scope)
    {
        Debug.Assert(context != null);

        _schema = context.Schema;

        MinMaxDate = Domain.Endpoints.Select(MyDate.FromDayNumber);
    }

    public OrderedPair<MyDate> MinMaxDate { get; }
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
        // Check arg eagerly.
        SupportedYears.Validate(year);

        return Iterator();

        IEnumerable<MyDate> Iterator()
        {
            int monthsInYear = Schema.CountMonthsInYear(year);

            for (int m = 1; m <= monthsInYear; m++)
            {
                int daysInMonth = Schema.CountDaysInMonth(year, m);

                for (int d = 1; d <= daysInMonth; d++)
                {
                    yield return new MyDate(year, m, d);
                }
            }
        }
    }

    [Pure]
    public IEnumerable<MyDate> GetDaysInMonth(int year, int month)
    {
        // Check args eagerly.
        Scope.ValidateYearMonth(year, month);

        return Iterator();

        IEnumerable<MyDate> Iterator()
        {
            int daysInMonth = Schema.CountDaysInMonth(year, month);

            for (int d = 1; d <= daysInMonth; d++)
            {
                yield return new MyDate(year, month, d);
            }
        }
    }

    [Pure]
    public MyDate GetStartOfYear(int year)
    {
        SupportedYears.Validate(year);
        var ymd = _schema.GetDatePartsAtStartOfYear(year);
        return new MyDate(ymd);
    }

    [Pure]
    public MyDate GetEndOfYear(int year)
    {
        SupportedYears.Validate(year);
        var ymd = _schema.GetDatePartsAtEndOfYear(year);
        return new MyDate(ymd);
    }

    [Pure]
    public MyDate GetStartOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        var ymd = _schema.GetDatePartsAtStartOfMonth(year, month);
        return new MyDate(ymd);
    }

    [Pure]
    public MyDate GetEndOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        var ymd = _schema.GetDatePartsAtEndOfMonth(year, month);
        return new MyDate(ymd);
    }
}
