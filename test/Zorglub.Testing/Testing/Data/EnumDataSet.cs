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

    public static TheoryData<DayOfWeek> DayOfWeekData { get; } = new()
    {
        DayOfWeek.Monday,
        DayOfWeek.Tuesday,
        DayOfWeek.Wednesday,
        DayOfWeek.Thursday,
        DayOfWeek.Friday,
        DayOfWeek.Saturday,
        DayOfWeek.Sunday,
    };

    //
    // Zorglub.Time
    //

    public static TheoryData<DateAdditionRule> InvalidDateAdditionRuleData { get; } = new()
    {
        (DateAdditionRule)(-1),
        DateAdditionRule.Throw + 1,
    };

    public static TheoryData<DateAdditionRule> DateAdditionRuleData { get; } = new()
    {
        DateAdditionRule.EndOfMonth,
        DateAdditionRule.StartOfNextMonth,
        DateAdditionRule.Exact,
        DateAdditionRule.Throw,
    };

    public static TheoryData<OrdinalAdditionRule> InvalidOrdinalAdditionRuleData { get; } = new()
    {
        (OrdinalAdditionRule)(-1),
        OrdinalAdditionRule.Throw + 1,
    };

    public static TheoryData<OrdinalAdditionRule> OrdinalAdditionRuleData { get; } = new()
    {
        OrdinalAdditionRule.EndOfYear,
        OrdinalAdditionRule.StartOfNextYear,
        OrdinalAdditionRule.Exact,
        OrdinalAdditionRule.Throw,
    };

    public static TheoryData<MonthAdditionRule> InvalidMonthAdditionRuleData { get; } = new()
    {
        (MonthAdditionRule)(-1),
        MonthAdditionRule.Throw + 1,
    };

    public static TheoryData<MonthAdditionRule> MonthAdditionRuleData { get; } = new()
    {
        MonthAdditionRule.EndOfYear,
        MonthAdditionRule.StartOfNextYear,
        MonthAdditionRule.Exact,
        MonthAdditionRule.Throw,
    };

    public static TheoryData<CalendricalAlgorithm> InvalidCalendricalAlgorithmData { get; } = new()
    {
        (CalendricalAlgorithm)(-1),
        CalendricalAlgorithm.Observational + 1,
    };

    public static TheoryData<CalendricalAlgorithm> CalendricalAlgorithmData { get; } = new()
    {
        CalendricalAlgorithm.Arithmetical,
        CalendricalAlgorithm.Astronomical,
        CalendricalAlgorithm.Observational,
        CalendricalAlgorithm.Unknown,
    };

    public static TheoryData<CalendricalFamily> InvalidCalendricalFamilyData { get; } = new()
    {
        (CalendricalFamily)(-1),
        CalendricalFamily.Lunisolar + 1,
    };

    public static TheoryData<CalendricalFamily> CalendricalFamilyData { get; } = new()
    {
        CalendricalFamily.AnnusVagus,
        CalendricalFamily.Lunar,
        CalendricalFamily.Lunisolar,
        CalendricalFamily.Other,
        CalendricalFamily.Solar,
    };

    // All pre-defined values. Being a flag enum, other combinations are legit.
    public static TheoryData<CalendricalAdjustments> CalendricalAdjustmentsData { get; } = new()
    {
        CalendricalAdjustments.Days,
        CalendricalAdjustments.DaysAndMonths,
        CalendricalAdjustments.Months,
        CalendricalAdjustments.None,
        CalendricalAdjustments.Weeks,
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

    //
    // Zorglub.Time.Simple
    //

    internal static TheoryData<Cuid> UnfixedCuidData { get; } = new()
    {
        Cuid.MaxSystem + 1,
        Cuid.MinUser,
        Cuid.Max,
        Cuid.Invalid,
    };

    internal static TheoryData<Cuid> FixedCuidData { get; } = new()
    {
        Cuid.Armenian,
        Cuid.Coptic,
        Cuid.Ethiopic,
        Cuid.Gregorian,
        Cuid.Julian,
        Cuid.TabularIslamic,
        Cuid.Zoroastrian,
    };
}
