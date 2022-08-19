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

    public static TheoryData<IsoWeekday> InvalidIsoWeekdayData { get; } = new()
    {
        0,
        (IsoWeekday)8
    };

    public static TheoryData<IsoWeekday> IsoWeekdayData { get; } = new()
    {
        IsoWeekday.Monday,
        IsoWeekday.Tuesday,
        IsoWeekday.Wednesday,
        IsoWeekday.Thursday,
        IsoWeekday.Friday,
        IsoWeekday.Saturday,
        IsoWeekday.Sunday,
    };

    //
    // Zorglub.Time
    //

    public static TheoryData<AdditionRule> InvalidAdditionRuleData { get; } = new()
    {
        (AdditionRule)(-1),
        AdditionRule.Throw + 1,
    };

    public static TheoryData<AdditionRule> AdditionRuleData { get; } = new()
    {
        AdditionRule.Truncate,
        AdditionRule.Overspill,
        AdditionRule.Exact,
        AdditionRule.Throw,
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
        CalendarId.Civil,
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
        Cuid.Civil,
        Cuid.Coptic,
        Cuid.Ethiopic,
        Cuid.Gregorian,
        Cuid.Julian,
        Cuid.TabularIslamic,
        Cuid.Zoroastrian,
    };
}
