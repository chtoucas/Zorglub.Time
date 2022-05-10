// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

using Zorglub.Time.Hemerology.Scopes;

public sealed partial class CalendarMonthAdjustersTests : GregorianOnlyTesting
{
    public CalendarMonthAdjustersTests() : base(GregorianCalendar.Instance) { }
}

public partial class CalendarMonthAdjustersTests // Adjustments
{
    [Theory]
    // Année invalide.
    [InlineData(3, 4, ProlepticShortScope.MinYear - 1)]
    [InlineData(3, 4, ShortScope.MaxYear + 1)]
    public void WithYear_InvalidYear(int y, int m, int newYear)
    {
        var cmonth = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.ThrowsAoorexn("newYear", () => cmonth.WithYear(newYear));
    }

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
    public void WithYear(int y, int m, int newYear)
    {
        var cmonth = CalendarUT.GetCalendarMonth(y, m);
        var newCmonth = CalendarUT.GetCalendarMonth(newYear, m);
        // Act & Assert
        Assert.Equal(newCmonth, cmonth.WithYear(newYear));
    }

    [Theory]
    [InlineData(1, 1, 0)]
    [InlineData(1, 1, 13)]
    public void WithMonthOfYear_InvalidMonth(int y, int m, int newMonth)
    {
        var cmonth = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.ThrowsAoorexn("newMonth", () => cmonth.WithMonthOfYear(newMonth));
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
    public void WithMonthOfYear(int y, int m, int newMonth)
    {
        var cmonth = CalendarUT.GetCalendarMonth(y, m);
        var newCmonth = CalendarUT.GetCalendarMonth(y, newMonth);
        // Act & Assert
        Assert.Equal(newCmonth, cmonth.WithMonthOfYear(newMonth));
    }
}
