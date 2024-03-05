// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Samples;

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using Zorglub.Time;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

using static Zorglub.Time.Extensions.Unboxing;

public sealed class MyCivilCalendar : MinMaxYearBasicCalendar, ICalendar<MyCivilDate>
{
    public MyCivilCalendar() : this(GetCivilSchema()) { }

    public MyCivilCalendar(CivilSchema schema)
        : base("Gregorian", StandardScope.Create(schema, DayZero.NewStyle)) { }

    [Pure]
    private static CivilSchema GetCivilSchema() => CivilSchema.GetInstance().Unbox();

    /// <inheritdoc/>
    [Pure]
    public IEnumerable<MyCivilDate> GetDaysInYear(int year)
    {
        YearsValidator.Validate(year);

        int startOfYear = Schema.GetStartOfYear(year);
        int daysInYear = Schema.CountDaysInYear(year);

        return from daysSinceEpoch
               in Enumerable.Range(startOfYear, daysInYear)
               select new MyCivilDate(daysSinceEpoch);
    }

    /// <inheritdoc/>
    [Pure]
    public IEnumerable<MyCivilDate> GetDaysInMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);

        int startOfMonth = Schema.GetStartOfMonth(year, month);
        int daysInMonth = Schema.CountDaysInMonth(year, month);

        return from daysSinceEpoch
               in Enumerable.Range(startOfMonth, daysInMonth)
               select new MyCivilDate(daysSinceEpoch);
    }

    /// <inheritdoc/>
    [Pure]
    public MyCivilDate GetStartOfYear(int year)
    {
        YearsValidator.Validate(year);
        int daysSinceEpoch = Schema.GetStartOfYear(year);
        return new MyCivilDate(daysSinceEpoch);
    }

    /// <inheritdoc/>
    [Pure]
    public MyCivilDate GetEndOfYear(int year)
    {
        YearsValidator.Validate(year);
        int daysSinceEpoch = Schema.GetEndOfYear(year);
        return new MyCivilDate(daysSinceEpoch);
    }

    /// <inheritdoc/>
    [Pure]
    public MyCivilDate GetStartOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        int daysSinceEpoch = Schema.GetStartOfMonth(year, month);
        return new MyCivilDate(daysSinceEpoch);
    }

    /// <inheritdoc/>
    [Pure]
    public MyCivilDate GetEndOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        int daysSinceEpoch = Schema.GetEndOfMonth(year, month);
        return new MyCivilDate(daysSinceEpoch);
    }
}
