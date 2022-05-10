// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

using Zorglub.Time.Hemerology.Scopes;

public sealed partial class CalendarYearAdjustersTests : GregorianOnlyTesting
{
    public CalendarYearAdjustersTests() : base(GregorianCalendar.Instance) { }
}

public partial class CalendarYearAdjustersTests // Adjustments
{
    [Theory]
    // Année invalide.
    [InlineData(3, ProlepticShortScope.MinYear - 1)]
    [InlineData(3, ShortScope.MaxYear + 1)]
    public void WithYear_InvalidYear(int y, int newYear)
    {
        // Arrange
        var year = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.ThrowsAoorexn("newYear", () => year.WithYear(newYear));
    }

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
    public void WithYear(int y, int newYear)
    {
        // Arrange
        var cyear = CalendarUT.GetCalendarYear(y);
        var newCyear = CalendarUT.GetCalendarYear(newYear);
        // Act & Assert
        Assert.Equal(newCyear, cyear.WithYear(newYear));
    }
}
