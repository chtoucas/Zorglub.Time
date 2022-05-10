// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Samples;

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Utilities;
using Zorglub.Time.Hemerology;

// Verification that one can create a calendar type without having access to
// the internals.

public sealed partial class MyCalendar : BasicCalendar, ICalendar<MyDate>
{
    private readonly SystemSchema _schema;

    internal MyCalendar(SystemSchema schema, DayNumber epoch)
        : base(schema, MinMaxYearScope.WithMinYear(schema, epoch, 1))
    {
        Debug.Assert(schema != null);

        _schema = schema;

        MinMaxDate = Domain.Endpoints.Select(GetDateOn);
    }

    public OrderedPair<MyDate> MinMaxDate { get; }
}

public partial class MyCalendar // Year, month or day infos
{
    [Pure]
    public override int CountMonthsInYear(int year)
    {
        Scope.ValidateYear(year);
        return Schema.CountMonthsInYear(year);
    }

    [Pure]
    public override int CountDaysInYear(int year)
    {
        Scope.ValidateYear(year);
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
    public MyDate Today() => GetDateOn(DayNumber.Today());
}

public partial class MyCalendar // Conversions
{
    [Pure]
    public MyDate GetDateOn(DayNumber dayNumber)
    {
        Domain.Validate(dayNumber);
        var ymd = _schema.GetDateParts(dayNumber - Epoch);
        return new MyDate(ymd);
    }
}

public partial class MyCalendar // Dates in a given year or month
{
    [Pure]
    public IEnumerable<MyDate> GetDaysInYear(int year)
    {
        // Check arg eagerly.
        Scope.ValidateYear(year);

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
        Scope.ValidateYear(year);
        var ymd = _schema.GetStartOfYearParts(year);
        return new MyDate(ymd);
    }

    [Pure]
    public MyDate GetEndOfYear(int year)
    {
        Scope.ValidateYear(year);
        var ymd = _schema.GetEndOfYearParts(year);
        return new MyDate(ymd);
    }

    [Pure]
    public MyDate GetStartOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        var ymd = _schema.GetStartOfMonthParts(year, month);
        return new MyDate(ymd);
    }

    [Pure]
    public MyDate GetEndOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        var ymd = _schema.GetEndOfMonthParts(year, month);
        return new MyDate(ymd);
    }
}
