﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Samples;

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using Zorglub.Time;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

using static Zorglub.Time.Extensions.Unboxing;

// Verification that one can create a calendar type without having access to
// the internals.

public sealed class MyCivilCalendar : BasicCalendar, ICalendar<MyCivilDate>
{
    public MyCivilCalendar() : this(GetCivilSchema()) { }

    public MyCivilCalendar(CivilSchema schema)
        : base("Gregorian", new StandardScope(schema, DayZero.NewStyle)) { }

    private static CivilSchema GetCivilSchema() => CivilSchema.GetInstance().Unbox();

    //
    // Year, month or day infos
    //

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

    //
    // Factories
    //

    [Pure]
    MyCivilDate ICalendar<MyCivilDate>.Today() => MyCivilDate.Today();

    //
    // Dates in a given year or month
    //

    [Pure]
    public IEnumerable<MyCivilDate> GetDaysInYear(int year)
    {
        SupportedYears.Validate(year);

        int startOfYear = Schema.GetStartOfYear(year);
        int daysInYear = Schema.CountDaysInYear(year);

        return from daysSinceZero
               in Enumerable.Range(startOfYear, daysInYear)
               select new MyCivilDate(daysSinceZero);
    }

    [Pure]
    public IEnumerable<MyCivilDate> GetDaysInMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);

        int startOfMonth = Schema.GetStartOfMonth(year, month);
        int daysInMonth = Schema.CountDaysInMonth(year, month);

        return from daysSinceZero
               in Enumerable.Range(startOfMonth, daysInMonth)
               select new MyCivilDate(daysSinceZero);
    }

    [Pure]
    public MyCivilDate GetStartOfYear(int year)
    {
        SupportedYears.Validate(year);
        int daysSinceZero = Schema.GetStartOfYear(year);
        return new MyCivilDate(daysSinceZero);
    }

    [Pure]
    public MyCivilDate GetEndOfYear(int year)
    {
        SupportedYears.Validate(year);
        int daysSinceZero = Schema.GetEndOfYear(year);
        return new MyCivilDate(daysSinceZero);
    }

    [Pure]
    public MyCivilDate GetStartOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        int daysSinceZero = Schema.GetStartOfMonth(year, month);
        return new MyCivilDate(daysSinceZero);
    }

    [Pure]
    public MyCivilDate GetEndOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        int daysSinceZero = Schema.GetEndOfMonth(year, month);
        return new MyCivilDate(daysSinceZero);
    }
}
