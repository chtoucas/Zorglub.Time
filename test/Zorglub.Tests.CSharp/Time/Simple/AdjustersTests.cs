// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

using Zorglub.Time.Hemerology.Scopes;

public static class OrdinalDateAdjustersTests
{
    private static readonly GregorianCalendar s_Calendar = GregorianCalendar.Instance;

    [Fact]
    public static void WithYear_InvalidResult()
    {
        var ordate = s_Calendar.GetOrdinalDate(4, 366);
        // Act & Assert
        Assert.ThrowsAoorexn("newYear", () => ordate.WithYear(3));
    }

    [Fact]
    public static void WithYear()
    {
        var ordate = s_Calendar.GetOrdinalDate(3, 45);
        var dateE = s_Calendar.GetOrdinalDate(4, 45);
        // Act & Assert
        Assert.Equal(dateE, ordate.WithYear(4));
    }

    [Fact]
    public static void WithDayOfYear()
    {
        var ordate = s_Calendar.GetOrdinalDate(3, 45);
        var dateE = s_Calendar.GetOrdinalDate(3, 54);
        // Act & Assert
        Assert.Equal(dateE, ordate.WithDayOfYear(54));
    }
}

public static class CalendarMonthAdjustersTests
{
    private static readonly GregorianCalendar s_Calendar = GregorianCalendar.Instance;

    [Theory]
    [InlineData(1, 1, 12)]
    [InlineData(2, 2, 11)]
    [InlineData(3, 3, 10)]
    [InlineData(4, 4, 9)]
    [InlineData(5, 5, 8)]
    [InlineData(6, 6, 7)]
    [InlineData(7, 7, 6)]
    [InlineData(8, 8, 5)]
    [InlineData(9, 9, 4)]
    [InlineData(10, 10, 3)]
    [InlineData(11, 11, 2)]
    [InlineData(12, 12, 1)]
    // Pas de changement d'année.
    [InlineData(12, 12, 12)]
    public static void WithYear(int y, int m, int newYear)
    {
        var month = s_Calendar.GetCalendarMonth(y, m);
        var newMonth = s_Calendar.GetCalendarMonth(newYear, m);
        // Act & Assert
        Assert.Equal(newMonth, month.WithYear(newYear));
    }

    [Theory]
    [InlineData(1, 1, 12)]
    [InlineData(1, 2, 11)]
    [InlineData(1, 3, 10)]
    [InlineData(1, 4, 9)]
    [InlineData(1, 5, 8)]
    [InlineData(1, 6, 7)]
    [InlineData(1, 7, 6)]
    [InlineData(1, 8, 5)]
    [InlineData(1, 9, 4)]
    [InlineData(1, 10, 3)]
    [InlineData(1, 11, 2)]
    [InlineData(1, 12, 1)]
    // Pas de changement de mois.
    [InlineData(1, 12, 12)]
    public static void WithMonthOfYear(int y, int m, int newM)
    {
        var month = s_Calendar.GetCalendarMonth(y, m);
        var newMonth = s_Calendar.GetCalendarMonth(y, newM);
        // Act & Assert
        Assert.Equal(newMonth, month.WithMonthOfYear(newM));
    }
}

public static class CalendarYearAdjustersTests
{
    private static readonly GregorianCalendar s_Calendar = GregorianCalendar.Instance;

    [Theory]
    [InlineData(1, 12)]
    [InlineData(2, 11)]
    [InlineData(3, 10)]
    [InlineData(4, 9)]
    [InlineData(5, 8)]
    [InlineData(6, 7)]
    [InlineData(7, 6)]
    [InlineData(8, 5)]
    [InlineData(9, 4)]
    [InlineData(10, 3)]
    [InlineData(11, 2)]
    [InlineData(12, 1)]
    // Pas de changement d'année.
    [InlineData(12, 12)]
    public static void WithYear(int y, int newY)
    {
        var year = s_Calendar.GetCalendarYear(y);
        var newYear = s_Calendar.GetCalendarYear(newY);
        // Act & Assert
        Assert.Equal(newYear, year.WithYear(newY));
    }
}
