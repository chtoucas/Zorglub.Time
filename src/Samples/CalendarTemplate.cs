// SPDX-License-Identifier: BSD-3-Clause
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

public sealed class CalendarTemplate : BasicCalendar, ICalendar<DateTemplate>
{
    public CalendarTemplate() : this(GetCivilSchema()) { }

    public CalendarTemplate(CivilSchema schema)
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
    DateTemplate ICalendar<DateTemplate>.Today() => DateTemplate.Today();

    //
    // Dates in a given year or month
    //

    [Pure]
    public IEnumerable<DateTemplate> GetDaysInYear(int year)
    {
        SupportedYears.Validate(year);

        int startOfYear = Schema.GetStartOfYear(year);
        int daysInYear = Schema.CountDaysInYear(year);

        return from daysSinceEpoch
               in Enumerable.Range(startOfYear, daysInYear)
               select new DateTemplate(daysSinceEpoch);
    }

    [Pure]
    public IEnumerable<DateTemplate> GetDaysInMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);

        int startOfMonth = Schema.GetStartOfMonth(year, month);
        int daysInMonth = Schema.CountDaysInMonth(year, month);

        return from daysSinceEpoch
               in Enumerable.Range(startOfMonth, daysInMonth)
               select new DateTemplate(daysSinceEpoch);
    }

    [Pure]
    public DateTemplate GetStartOfYear(int year)
    {
        SupportedYears.Validate(year);
        int daysSinceEpoch = Schema.GetStartOfYear(year);
        return new DateTemplate(daysSinceEpoch);
    }

    [Pure]
    public DateTemplate GetEndOfYear(int year)
    {
        SupportedYears.Validate(year);
        int daysSinceEpoch = Schema.GetEndOfYear(year);
        return new DateTemplate(daysSinceEpoch);
    }

    [Pure]
    public DateTemplate GetStartOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        int daysSinceEpoch = Schema.GetStartOfMonth(year, month);
        return new DateTemplate(daysSinceEpoch);
    }

    [Pure]
    public DateTemplate GetEndOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        int daysSinceEpoch = Schema.GetEndOfMonth(year, month);
        return new DateTemplate(daysSinceEpoch);
    }
}
