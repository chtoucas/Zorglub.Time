// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

// We could compute the limits automatically.

// We cannot guarantee that all interconversions will succeed for years
// in the range [1, 9999].
public static class InterconversionTests
{
    // All calendar epochs can be interconverted into the Gregorian calendar.
    [Fact]
    public static void Epoch_ToGregorian()
    {
        foreach (var chr in CalendarCatalog.SystemCalendars)
        {
            var date = chr.GetCalendarDateOn(chr.Epoch);
            try
            {
                _ = date.WithCalendar(GregorianCalendar.Instance);
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
        var year = GregorianCalendar.Instance.GetCalendarYear(1);
        var date = CalendarDate.AtStartOfYear(year);
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
        var gr = GregorianCalendar.Instance;
        var start = CalendarDate.AtStartOfYear(gr.GetCalendarYear(1582)); // Gregorian reform
        var end = CalendarDate.AtEndOfYear(gr.GetCalendarYear(3000));
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

    private static void TestMutualInterconversion(int year, bool startOfYear)
    {
        foreach (var chr in CalendarCatalog.SystemCalendars)
        {
            var yo = chr.GetCalendarYear(year);
            var date = startOfYear ? CalendarDate.AtStartOfYear(yo) : CalendarDate.AtEndOfYear(yo);
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
