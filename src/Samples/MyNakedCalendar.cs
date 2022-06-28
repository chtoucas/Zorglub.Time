// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Samples;

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Hemerology;

// Verification that one can create a calendar type without having access to
// the internals of the assembly Zorglub.

public partial class MyNakedCalendar : NakedCalendar
{
    public MyNakedCalendar(string name, ICalendricalSchema schema, DayNumber epoch)
        : this(name, schema, new MinMaxYearScope(schema, epoch, 1, 9999)) { }

    public MyNakedCalendar(string name, ICalendricalSchema schema, MinMaxYearScope scope)
        : base(name, schema, scope)
    {
        PartsFactory = ICalendricalPartsFactory.Create(schema);
    }

    protected ICalendricalPartsFactory PartsFactory { get; }
}

public partial class MyNakedCalendar // Year, month, day infos
{
    [Pure]
    public sealed override int CountMonthsInYear(int year)
    {
        Scope.ValidateYear(year);
        return Schema.CountMonthsInYear(year);
    }

    [Pure]
    public sealed override int CountDaysInYear(int year)
    {
        Scope.ValidateYear(year);
        return Schema.CountDaysInYear(year);
    }

    [Pure]
    public sealed override int CountDaysInMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        return Schema.CountDaysInMonth(year, month);
    }
}

public partial class MyNakedCalendar // Dates in a given year or month
{
    /// <inheritdoc />
    [Pure]
    public sealed override IEnumerable<DateParts> GetDaysInYear(int year)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    [Pure]
    public sealed override IEnumerable<DateParts> GetDaysInMonth(int year, int month)
    {
        throw new NotImplementedException();
    }

    [Pure]
    public sealed override DateParts GetStartOfYear(int year)
    {
        Scope.ValidateYear(year);
        var ymd = PartsFactory.GetDatePartsAtStartOfYear(year);
        return new DateParts(ymd);
    }

    [Pure]
    public sealed override DateParts GetEndOfYear(int year)
    {
        Scope.ValidateYear(year);
        var ymd = PartsFactory.GetDatePartsAtEndOfYear(year);
        return new DateParts(ymd);
    }

    [Pure]
    public sealed override DateParts GetStartOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        var ymd = PartsFactory.GetDatePartsAtStartOfMonth(year, month);
        return new DateParts(ymd);
    }

    [Pure]
    public sealed override DateParts GetEndOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        var ymd = PartsFactory.GetDatePartsAtEndOfMonth(year, month);
        return new DateParts(ymd);
    }
}
