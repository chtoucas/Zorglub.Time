﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Play.Demo;

using Samples;

using Zorglub.Time;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Extras;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Simple;

using static System.Console;

public static class Showcase
{
    private static readonly SimpleCalendar s_Tropicalia3031 =
        Tropicalia3031Schema.GetInstance().CreateCalendar("Tropicália 30-31", DayZero.NewStyle);

    private static readonly SimpleCalendar s_Tropicalia3130 =
        Tropicalia3130Schema.GetInstance().CreateCalendar("Tropicália 31-30", DayZero.NewStyle);

    private static readonly SimpleCalendar s_Tropicalia3130TE =
        Tropicalia3130Schema.GetInstance().CreateCalendar("Tropicália 31-30 TE", CalendarEpoch2.Tropicalia);

    public static void Run()
    {
        var today = DayNumber.Today();

        WriteLine($"Day number: {today}\n");

        WriteLine("Egyptian-like calendars.");
        WriteLine($"    {CalendarZoo.Egyptian.GetCalendarDate(today)}");
        WriteLine($"    {ArmenianCalendar.Instance.GetCalendarDate(today)}");
        WriteLine($"    {ZoroastrianCalendar.Instance.GetCalendarDate(today)}");

        WriteLine();
        WriteLine("Coptic-like calendars.");
        WriteLine($"    {EthiopicCalendar.Instance.GetCalendarDate(today)}");
        WriteLine($"    {CopticCalendar.Instance.GetCalendarDate(today)}");
        WriteLine($"    {CalendarZoo.FrenchRepublican.GetCalendarDate(today)}");

        WriteLine();
        WriteLine("Gregorian-like calendars.");
        WriteLine($"    {GregorianCalendar.Instance.GetCalendarDate(today)}");
        WriteLine($"    {ZCalendar.Gregorian.GetDate(today)}");
        WriteLine($"    {CalendarZoo.GenuineGregorian.GetDateParts(today)} ({CalendarZoo.GenuineGregorian})");
        WriteLine($"    {CalendarZoo.Minguo.GetDate(today)}");
        WriteLine($"    {CalendarZoo.Holocene.GetDate(today)}");
        WriteLine($"    {CalendarZoo.ThaiSolar.GetDate(today)}");

        WriteLine();
        WriteLine("Tropicalista calendars.");
        WriteLine($"    {CalendarZoo.Tropicalia.GetCalendarDate(today)}");
        WriteLine($"    {s_Tropicalia3031.GetCalendarDate(today)}");
        WriteLine($"    {s_Tropicalia3130.GetCalendarDate(today)}");
        WriteLine($"    {s_Tropicalia3130TE.GetCalendarDate(today)}");

        WriteLine();
        WriteLine("Other calendars.");
        WriteLine($"    {JulianCalendar.Instance.GetCalendarDate(today)}");
        WriteLine($"    {CalendarZoo.LongJulian.GetDate(today)}");
        WriteLine($"    {CalendarZoo.GenuineJulian.GetDateParts(today)} ({CalendarZoo.GenuineJulian})");
        WriteLine($"    {TabularIslamicCalendar.Instance.GetCalendarDate(today)}");
        WriteLine($"    {CalendarZoo.Persian2820.GetCalendarDate(today)}");

        WriteLine();
        WriteLine("Reforms.");
        WriteLine($"    {CalendarZoo.InternationalFixed.GetCalendarDate(today)}");
        //WriteLine($"    {CalendarZoo.Pax.GetCalendarDate(today)}");
        WriteLine($"    {CalendarZoo.Positivist.GetCalendarDate(today)}");
        WriteLine($"    {CalendarZoo.RevisedWorld.GetCalendarDate(today)}");
        WriteLine($"    {CalendarZoo.World.GetCalendarDate(today)}");
    }
}
