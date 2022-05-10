// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

using Zorglub.Time.Simple;

public static class EnumDataSet
{
    public static readonly TheoryData<DayOfWeek> InvalidDayOfWeekData =
        new() { (DayOfWeek)(-1), (DayOfWeek)7 };

    public static readonly TheoryData<CalendarId> InvalidCalendarIdData =
        new() { (CalendarId)(-1), (CalendarId)(int)(1 + Cuid.MaxSystem) };

    public static readonly TheoryData<CalendarId> CalendarIdData = new()
    {
        CalendarId.Armenian,
        CalendarId.Coptic,
        CalendarId.Ethiopic,
        CalendarId.Gregorian,
        CalendarId.Julian,
        CalendarId.TabularIslamic,
        CalendarId.Zoroastrian,
    };

    public static readonly TheoryData<CalendricalAlgorithm> CalendricalAlgorithmData = new()
    {
        CalendricalAlgorithm.Arithmetical,
        CalendricalAlgorithm.Astronomical,
        CalendricalAlgorithm.Observational,
        CalendricalAlgorithm.Unknown,
    };

    public static readonly TheoryData<CalendricalFamily> CalendricalFamilyData = new()
    {
        CalendricalFamily.AnnusVagus,
        CalendricalFamily.Lunar,
        CalendricalFamily.Lunisolar,
        CalendricalFamily.Other,
        CalendricalFamily.Solar,
    };

    // All pre-defined values.
    // Being a flag enum, other combinations are legitimate.
    public static readonly TheoryData<CalendricalAdjustments> CalendricalAdjustmentsData = new()
    {
        CalendricalAdjustments.Days,
        CalendricalAdjustments.DaysAndMonths,
        CalendricalAdjustments.Months,
        CalendricalAdjustments.None,
        CalendricalAdjustments.Weeks,
    };
}
