// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Play.Demo;

using Samples;

using Zorglub.Time;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Simple;

using static System.Console;

public static class Showcase
{
    private static readonly Calendar s_Tropicalia3031 =
        Tropicalia3031Schema.GetInstance().CreateCalendar("Tropicália 30-31", DayZero.NewStyle);

    private static readonly Calendar s_Tropicalia3130 =
        Tropicalia3130Schema.GetInstance().CreateCalendar("Tropicália 31-30", DayZero.NewStyle);

    private static readonly Calendar s_Tropicalia3130TE =
        Tropicalia3130Schema.GetInstance().CreateCalendar("Tropicália 31-30 TE", CalendarEpoch2.Tropicalia);

    public static void Run()
    {
        var today = DayNumber.Today();

        WriteLine($"Day number: {today}\n");

        WriteLine("Egyptian-like calendars.");
        WriteLine($"    {CalendarZoo.Egyptian.GetCalendarDateOn(today)}");
        WriteLine($"    {ArmenianCalendar.Instance.GetCalendarDateOn(today)}");
        WriteLine($"    {ZoroastrianCalendar.Instance.GetCalendarDateOn(today)}");

        WriteLine();
        WriteLine("Coptic-like calendars.");
        WriteLine($"    {EthiopicCalendar.Instance.GetCalendarDateOn(today)}");
        WriteLine($"    {CopticCalendar.Instance.GetCalendarDateOn(today)}");
        WriteLine($"    {CalendarZoo.FrenchRepublican.GetCalendarDateOn(today)}");

        WriteLine();
        WriteLine("Gregorian-like calendars.");
        WriteLine($"    {GregorianCalendar.Instance.GetCalendarDateOn(today)}");
        WriteLine($"    {Zorglub.Time.Extras.ZCalendar.Gregorian.GetDate(today)}");
        WriteLine($"    {CalendarZoo.GenuineGregorian.GetDateParts(today)} ({CalendarZoo.GenuineGregorian})");
        WriteLine($"    {CalendarZoo.Minguo.GetDate(today)}");
        WriteLine($"    {CalendarZoo.Holocene.GetDate(today)}");
        WriteLine($"    {CalendarZoo.ThaiSolar.GetDate(today)}");

        WriteLine();
        WriteLine("Tropicalista calendars.");
        WriteLine($"    {CalendarZoo.Tropicalia.GetCalendarDateOn(today)}");
        WriteLine($"    {s_Tropicalia3031.GetCalendarDateOn(today)}");
        WriteLine($"    {s_Tropicalia3130.GetCalendarDateOn(today)}");
        WriteLine($"    {s_Tropicalia3130TE.GetCalendarDateOn(today)}");

        WriteLine();
        WriteLine("Other calendars.");
        WriteLine($"    {JulianCalendar.Instance.GetCalendarDateOn(today)}");
        WriteLine($"    {CalendarZoo.LongJulian.GetDate(today)}");
        WriteLine($"    {CalendarZoo.GenuineJulian.GetDateParts(today)} ({CalendarZoo.GenuineJulian})");
        WriteLine($"    {TabularIslamicCalendar.Instance.GetCalendarDateOn(today)}");
        WriteLine($"    {CalendarZoo.Persian2820.GetCalendarDateOn(today)}");

        WriteLine();
        WriteLine("Reforms.");
        WriteLine($"    {CalendarZoo.InternationalFixed.GetCalendarDateOn(today)}");
        //WriteLine($"    {CalendarZoo.Pax.GetCalendarDateOn(today)}");
        WriteLine($"    {CalendarZoo.Positivist.GetCalendarDateOn(today)}");
        WriteLine($"    {CalendarZoo.RevisedWorld.GetCalendarDateOn(today)}");
        WriteLine($"    {CalendarZoo.World.GetCalendarDateOn(today)}");
    }
}
