// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

using Zorglub.Time.Simple;

public static class EnumDataSet
{
    public static TheoryData<DayOfWeek> InvalidDayOfWeekData { get; } = new()
    {
        (DayOfWeek)(-1),
        (DayOfWeek)7
    };

    public static TheoryData<CalendarId> InvalidCalendarIdData { get; } = new()
    {
        (CalendarId)(-1),
        (CalendarId)(int)(1 + Cuid.MaxSystem)
    };

    public static TheoryData<CalendarId> CalendarIdData { get; } = new()
    {
        CalendarId.Armenian,
        CalendarId.Coptic,
        CalendarId.Ethiopic,
        CalendarId.Gregorian,
        CalendarId.Julian,
        CalendarId.TabularIslamic,
        CalendarId.Zoroastrian,
    };

    public static TheoryData<CalendricalAlgorithm> CalendricalAlgorithmData { get; } = new()
    {
        CalendricalAlgorithm.Arithmetical,
        CalendricalAlgorithm.Astronomical,
        CalendricalAlgorithm.Observational,
        CalendricalAlgorithm.Unknown,
    };

    public static TheoryData<CalendricalFamily> CalendricalFamilyData { get; } = new()
    {
        CalendricalFamily.AnnusVagus,
        CalendricalFamily.Lunar,
        CalendricalFamily.Lunisolar,
        CalendricalFamily.Other,
        CalendricalFamily.Solar,
    };

    // All pre-defined values.
    // Being a flag enum, other combinations are legitimate.
    public static TheoryData<CalendricalAdjustments> CalendricalAdjustmentsData { get; } = new()
    {
        CalendricalAdjustments.Days,
        CalendricalAdjustments.DaysAndMonths,
        CalendricalAdjustments.Months,
        CalendricalAdjustments.None,
        CalendricalAdjustments.Weeks,
    };
}
