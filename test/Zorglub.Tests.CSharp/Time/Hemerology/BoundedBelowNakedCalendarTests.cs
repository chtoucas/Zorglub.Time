// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology;

using System.Linq;

using global::Samples;

using Zorglub.Testing.Data.Unbounded;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology.Scopes;

// Cf. aussi GregorianBoundedBelowNakedCalendarTests.
public static class BoundedBelowNakedCalendarTests
{
    // Exemple du calendrier grégorien qui débute officiellement le 15/10/1582.
    // En 1582, 3 mois, octobre à décembre.
    // En 1582, 78 jours = 17 (oct) + 30 (nov) + 31 (déc).

    [Fact]
    public static void CountMonthsInFirstYear()
    {
        // Act
        var chr = CalendarZoo.GenuineGregorian;
        int minYear = chr.MinYear;
        int monthsInFirstYear = 3;
        // Assert
        Assert.Equal(monthsInFirstYear, chr.CountMonthsInYear(minYear));
        Assert.Equal(monthsInFirstYear, chr.CountMonthsInFirstYear());
    }

    [Fact]
    public static void CountDaysInFirstYear()
    {
        // Act
        var chr = CalendarZoo.GenuineGregorian;
        int minYear = chr.MinYear;
        int daysInFirstYear = 78;
        // Assert
        Assert.Equal(daysInFirstYear, chr.CountDaysInYear(minYear));
        Assert.Equal(daysInFirstYear, chr.CountDaysInFirstYear());
    }

    [Fact]
    public static void CountDaysInFirstMonth()
    {
        // Act
        var chr = CalendarZoo.GenuineGregorian;
        int daysInFirstMonth = 17;
        var parts = chr.MinDateParts;
        // Assert
        Assert.Equal(daysInFirstMonth, chr.CountDaysInMonth(parts.Year, parts.Month));
        Assert.Equal(daysInFirstMonth, chr.CountDaysInFirstMonth());
    }
}

public sealed class GregorianBoundedBelowNakedCalendarTests :
    INakedCalendarFacts<BoundedBelowCalendar, UnboundedGregorianDataSet>
{
    private const int FirstYear = -123_456;
    private const int FirstMonth = 4;
    private const int FirstDay = 5;

    public GregorianBoundedBelowNakedCalendarTests()
        : base(MakeCalendar()) { }

    // On triche un peu, la date de début a été choisie de telle sorte que
    // les tests marchent... (cf. GregorianData).
    private static BoundedBelowCalendar MakeCalendar() =>
        new(
            "Gregorian",
            BoundedBelowScope.StartingAt(
                new GregorianSchema(),
                DayZero.NewStyle,
                new DateParts(FirstYear, FirstMonth, FirstDay)));

    [Fact]
    public void MinDateParts_Prop()
    {
        var parts = new DateParts(FirstYear, FirstMonth, FirstDay);
        var seg = CalendarUT.Scope.Segment;
        // Act
        Assert.Equal(parts, seg.MinMaxDateParts.LowerValue);
    }

    [Fact]
    public void GetDaysInYear_FirstYear()
    {
        DayNumber startOfYear = CalendarUT.Domain.Min;
        DayNumber endOfYear = CalendarUT.GetEndOfYear(FirstYear);
        int daysInFirstYear = CalendarUT.CountDaysInFirstYear();
        IEnumerable<DayNumber> list =
            from i in Enumerable.Range(0, daysInFirstYear)
            select startOfYear + i;
        // Act
        IEnumerable<DayNumber> actual = CalendarUT.GetDaysInYear(FirstYear);
        // Assert
        Assert.Equal(list, actual);
        Assert.Equal(daysInFirstYear, actual.Count());
        Assert.Equal(startOfYear, actual.First());
        Assert.Equal(endOfYear, actual.Last());
    }

    [Fact]
    public void GetDaysInMonth_FirstMonth()
    {
        DayNumber startofMonth = CalendarUT.Domain.Min;
        DayNumber endOfMonth = CalendarUT.GetEndOfMonth(FirstYear, FirstMonth);
        int daysInFirstMonth = CalendarUT.CountDaysInFirstMonth();
        IEnumerable<DayNumber> list =
            from i in Enumerable.Range(0, daysInFirstMonth)
            select startofMonth + i;
        // Act
        IEnumerable<DayNumber> actual = CalendarUT.GetDaysInMonth(FirstYear, FirstMonth);
        // Assert
        Assert.Equal(list, actual);
        Assert.Equal(daysInFirstMonth, actual.Count());
        Assert.Equal(startofMonth, actual.First());
        Assert.Equal(endOfMonth, actual.Last());
    }

    [Fact]
    public void GetStartOfYear_InvalidFirstYear() =>
        Assert.ThrowsAoorexn("year", () => CalendarUT.GetStartOfYear(FirstYear));

    //[Fact]
    //public void GetStartOfYearFields_InvalidFirstYear() =>
    //    Assert.ThrowsAoorexn("year", () => CalendarUT.GetStartOfYear(FirstYear));

    [Fact]
    public void GetStartOfMonth_InvalidFirstMonth() =>
        Assert.ThrowsAoorexn("month", () => CalendarUT.GetStartOfMonth(FirstYear, FirstMonth));

    //[Fact]
    //public void GetStartOfMonthFields_InvalidFirstMonth() =>
    //    Assert.ThrowsAoorexn("month", () => CalendarUT.GetStartOfMonth(FirstYear, FirstMonth));

    [Fact]
    public void CountMonthsInFirstYear()
    {
        // Act
        int monthsInFirstYear = 12 - (FirstMonth - 1);
        // Assert
        Assert.Equal(monthsInFirstYear, CalendarUT.CountMonthsInYear(FirstYear));
        Assert.Equal(monthsInFirstYear, CalendarUT.CountMonthsInFirstYear());
    }

    [Fact]
    public void CountDaysInFirstYear()
    {
        // Act
        var sch = CalendarUT.Schema;
        int daysInFirstYear = sch.CountDaysInYear(FirstYear)
            - sch.CountDaysInYearBeforeMonth(FirstYear, FirstMonth)
            - (FirstDay - 1);
        // Assert
        Assert.Equal(daysInFirstYear, CalendarUT.CountDaysInYear(FirstYear));
        Assert.Equal(daysInFirstYear, CalendarUT.CountDaysInFirstYear());
    }

    [Fact]
    public void CountDaysInFirstMonth()
    {
        // Act
        var sch = CalendarUT.Schema;
        int daysInFirstMonth = sch.CountDaysInMonth(FirstYear, FirstMonth) - (FirstDay - 1);
        // Assert
        Assert.Equal(daysInFirstMonth, CalendarUT.CountDaysInMonth(FirstYear, FirstMonth));
        Assert.Equal(daysInFirstMonth, CalendarUT.CountDaysInFirstMonth());
    }
}
