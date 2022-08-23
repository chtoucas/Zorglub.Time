// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Play.Demo;

using Samples;

using Zorglub.Time;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Horology;
using Zorglub.Time.Simple;

using static System.Console;

public static class Showcase
{
    private static readonly SimpleCalendar s_Tropicalia3031 =
        Tropicalia3031Schema.GetInstance().CreateCalendar("Tropicália 30-31", DayZero.NewStyle);

    private static readonly SimpleCalendar s_Tropicalia3130 =
        Tropicalia3130Schema.GetInstance().CreateCalendar("Tropicália 31-30", DayZero.NewStyle);

    private static readonly SimpleCalendar s_Tropicalia3130TE =
        Tropicalia3130Schema.GetInstance().CreateCalendar("Tropicália 31-30 TE", DayZero.Tropicalia);

    public static void Run()
    {
        var today = SystemClock.Default.Today();

        WriteLine($"Day number: {today}\n");

        WriteLine("Egyptian-like calendars.");
        WriteLine($"    {CalendarZoo.Egyptian.GetDate(today)}");
        WriteLine($"    {SimpleCalendar.Armenian.GetDate(today)}");
        WriteLine($"    {SimpleCalendar.Zoroastrian.GetDate(today)}");

        WriteLine();
        WriteLine("Coptic-like calendars.");
        WriteLine($"    {SimpleCalendar.Ethiopic.GetDate(today)}");
        WriteLine($"    {SimpleCalendar.Coptic.GetDate(today)}");
        WriteLine($"    {CalendarZoo.FrenchRepublican.GetDate(today)}");

        WriteLine();
        WriteLine("Gregorian-like calendars.");
        WriteLine($"    {SimpleCalendar.Gregorian.GetDate(today)}");
        WriteLine($"    {ZCalendar.Gregorian.GetDate(today)}");
        WriteLine($"    {CalendarZoo.GenuineGregorian.GetDateParts(today)} ({CalendarZoo.GenuineGregorian})");
        WriteLine($"    {CalendarZoo.Minguo.GetDate(today)}");
        WriteLine($"    {CalendarZoo.Holocene.GetDate(today)}");
        WriteLine($"    {CalendarZoo.ThaiSolar.GetDate(today)}");

        WriteLine();
        WriteLine("Tropicalista calendars.");
        WriteLine($"    {CalendarZoo.Tropicalia.GetDate(today)}");
        WriteLine($"    {s_Tropicalia3031.GetDate(today)}");
        WriteLine($"    {s_Tropicalia3130.GetDate(today)}");
        WriteLine($"    {s_Tropicalia3130TE.GetDate(today)}");

        WriteLine();
        WriteLine("Other calendars.");
        WriteLine($"    {SimpleCalendar.Julian.GetDate(today)}");
        WriteLine($"    {CalendarZoo.LongJulian.GetDate(today)}");
        WriteLine($"    {CalendarZoo.GenuineJulian.GetDateParts(today)} ({CalendarZoo.GenuineJulian})");
        WriteLine($"    {SimpleCalendar.TabularIslamic.GetDate(today)}");
        WriteLine($"    {CalendarZoo.Persian2820.GetDate(today)}");

        WriteLine();
        WriteLine("Reforms.");
        WriteLine($"    {CalendarZoo.InternationalFixed.GetDate(today)}");
        //WriteLine($"    {CalendarZoo.Pax.GetDate(today)}");
        WriteLine($"    {CalendarZoo.Positivist.GetDate(today)}");
        WriteLine($"    {CalendarZoo.RevisedWorld.GetDate(today)}");
        WriteLine($"    {CalendarZoo.World.GetDate(today)}");
    }
}
