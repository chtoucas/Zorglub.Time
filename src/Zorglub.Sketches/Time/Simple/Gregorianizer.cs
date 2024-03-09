// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Simple;

using static Zorglub.Time.Extensions.SimpleInterconversions;

// Version minimaliste de la chronologie julienne réformée.
public sealed class Gregorianizer
{
    private readonly OrdinalDate _lastJulianOrdinal;
    private readonly OrdinalDate _firstGregorianOrdinal;

    public Gregorianizer() : this("Papal States", GregorianReform.Official) { }

    public Gregorianizer(string? regionName, GregorianReform reform)
    {
        RegionName = regionName ?? "Unspecified region";
        Reform = reform ?? throw new ArgumentNullException(nameof(reform));

        _lastJulianOrdinal = reform.LastJulianDate.ToOrdinalDate();
        _firstGregorianOrdinal = reform.FirstGregorianDate.ToOrdinalDate();
    }

    public string RegionName { get; }

    public GregorianReform Reform { get; }

    public CalendarDate Gregorianize(DayNumber dayNumber) =>
        dayNumber < Reform.Switchover
        ? SimpleCalendar.Julian.GetDate(dayNumber).ToCalendarDate()
        : SimpleCalendar.Gregorian.GetDate(dayNumber).ToCalendarDate();

    [SuppressMessage("Style", "IDE0072:Add missing cases", Justification = "<Pending>")]
    public CalendarDay Gregorianize(CalendarDay date) =>
        date.Cuid switch
        {
            Cuid.Gregorian =>
                date.DayNumber >= Reform.Switchover ? date
                    : date.WithCalendar(SimpleCalendar.Julian),

            Cuid.Julian =>
                date.DayNumber < Reform.Switchover ? date
                    : date.WithCalendar(SimpleCalendar.Gregorian),

            _ => Throw.Argument<CalendarDay>(nameof(date)),
        };

    [SuppressMessage("Style", "IDE0072:Add missing cases", Justification = "<Pending>")]
    public CalendarDate Gregorianize(CalendarDate date) =>
        date.Cuid switch
        {
            Cuid.Gregorian =>
                date >= Reform.FirstGregorianDate ? date
                    : date.WithCalendar(SimpleCalendar.Julian).ToCalendarDate(),

            Cuid.Julian =>
                date <= Reform.LastJulianDate ? date
                    : date.WithCalendar(SimpleCalendar.Gregorian).ToCalendarDate(),

            _ => Throw.Argument<CalendarDate>(nameof(date)),
        };

    [SuppressMessage("Style", "IDE0072:Add missing cases", Justification = "<Pending>")]
    public OrdinalDate Gregorianize(OrdinalDate date) =>
        date.Cuid switch
        {
            Cuid.Gregorian =>
                date >= _firstGregorianOrdinal ? date
                    : date.WithCalendar(SimpleCalendar.Julian).ToOrdinalDate(),

            Cuid.Julian =>
                date <= _lastJulianOrdinal ? date
                    : date.WithCalendar(SimpleCalendar.Gregorian).ToOrdinalDate(),

            _ => Throw.Argument<OrdinalDate>(nameof(date)),
        };
}
