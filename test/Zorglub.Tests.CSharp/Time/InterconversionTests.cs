// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time;

using Zorglub.Time.Simple;

// We cannot guarantee that all interconversions will succeed for years
// in the range [1, 9999].
// We could compute the limits automatically.

public static class InterconversionTests
{
    // All calendar epochs can be interconverted into the Gregorian calendar.
    [Fact]
    public static void Epoch_ToGregorian()
    {
        foreach (var chr in CalendarCatalog.SystemCalendars)
        {
            var date = chr.GetCalendarDate(chr.Epoch);
            try
            {
                _ = date.WithCalendar(SimpleGregorian.Instance);
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.Fails($"Interconversion failed from {chr} to Gregorian.");
            }
        }
    }

    // The Gregorian epoch can be interconverted into any calendar whose
    // epoch is BCE.
    [Fact]
    public static void StartOfGregorianYear1_ToAny()
    {
        var year = SimpleGregorian.Instance.GetCalendarYear(1);
        var date = year.FirstDay;
        foreach (var chr in CalendarCatalog.SystemCalendars)
        {
            if (chr.Epoch > DayZero.NewStyle) { continue; }
            try
            {
                _ = date.WithCalendar(chr);
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.Fails($"Interconversion failed from {chr} to Gregorian.");
            }
        }
    }

    // All date within the Gregorian range [1582, 3000] can be
    // interconverted into any calendar.
    [Fact]
    public static void GregorianYear1582To3000_ToAny()
    {
        var gr = SimpleGregorian.Instance;
        var start = gr.GetCalendarYear(1582).FirstDay; // Gregorian reform
        var end = gr.GetCalendarYear(3000).LastDay;
        foreach (var chr in CalendarCatalog.SystemCalendars)
        {
            try
            {
                _ = start.WithCalendar(chr);
                _ = end.WithCalendar(chr);
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.Fails($"Interconversion failed from {chr} to Gregorian.");
            }
        }
    }

    [Fact]
    public static void Start_DoesNotThrow() => TestMutualInterconversion(1000, true);

    [Fact]
    public static void End_DoesNotThrow() => TestMutualInterconversion(9000, false);

    private static void TestMutualInterconversion(int y, bool startOfYear)
    {
        foreach (var chr in CalendarCatalog.SystemCalendars)
        {
            var year = chr.GetCalendarYear(y);
            var date = startOfYear ? year.FirstDay : year.LastDay;
            foreach (var other in CalendarCatalog.SystemCalendars)
            {
                try
                {
                    _ = date.WithCalendar(other);
                }
                catch (ArgumentOutOfRangeException)
                {
                    Assert.Fails($"Interconversion failed from {chr} to {other}.");
                }
            }
        }
    }
}
