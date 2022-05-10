// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple.Specialized;

public static class RomanKalendarTests
{
    public static TheoryData<int, int> EpiphanyData { get; } = new()
    {
        { 2013, 6 },
        { 2019, 6 },
        { 2020, 5 },
        { 2021, 3 },
    };

    // Cf. http://www.dioceserimouski.com/ch/paqueslimit.html
    public static TheoryData<int, int, int> EasterData { get; } = new()
    {
        // Mois de mars.
        { 1598, 3, 22 },
        { 1913, 3, 23 },
        { 1799, 3, 24 },
        { 1951, 3, 25 },
        { 1967, 3, 26 },
        { 1910, 3, 27 },
        { 1937, 3, 28 },
        { 1959, 3, 29 },
        { 1902, 3, 30 },
        { 1907, 3, 31 },

        // Mois d'avril.
        { 1923, 4, 1 },
        { 1961, 4, 2 },
        { 1904, 4, 3 },
        { 1915, 4, 4 },
        { 1931, 4, 5 },
        { 1947, 4, 6 },
        { 1901, 4, 7 },
        { 1917, 4, 8 },
        { 1939, 4, 9 },
        { 1955, 4, 10 },
        { 1909, 4, 11 },
        { 1903, 4, 12 },
        { 1941, 4, 13 },
        { 1963, 4, 14 },
        { 1900, 4, 15 },
        { 1911, 4, 16 },
        { 1927, 4, 17 },
        { 1954, 4, 18 },
        { 1908, 4, 19 },
        { 1919, 4, 20 },
        { 1935, 4, 21 },
        { 1962, 4, 22 },
        { 1905, 4, 23 },
        { 2095, 4, 24 },
        { 1943, 4, 25 },

        // 20ème siècle.
        { 2000, 4, 23 },
        { 2001, 4, 15 },
        { 2002, 3, 31 },
        { 2003, 4, 20 },
        { 2004, 4, 11 },
        { 2005, 3, 27 },
        { 2006, 4, 16 },
        { 2007, 4, 8 },
        { 2008, 3, 23 },
        { 2009, 4, 12 },

        { 2010, 4, 4 },
        { 2011, 4, 24 },
        { 2012, 4, 8 },
        { 2013, 3, 31 },
        { 2014, 4, 20 },
        { 2015, 4, 5 },
        { 2016, 3, 27 },
        { 2017, 4, 16 },
        { 2018, 4, 1 },
        { 2019, 4, 21 },

        { 2020, 4, 12 },
        { 2021, 4, 4 },
        { 2022, 4, 17 },
        { 2023, 4, 9 },
        { 2024, 3, 31 },
        { 2025, 4, 20 },
    };

    // D.&R. p. 120
    public static TheoryData<int, int, int> PaschalMoonData { get; } = new()
    {
        { 9, 3, 29 },
        { 10, 4, 17 },
        { 11, 4, 6 },
        { 12, 3, 26 },

        { 13, 4, 14 },
        { 14, 4, 3 },
        { 15, 3, 23 },
        { 16, 4, 11 },

        { 17, 3, 31 },
        { 18, 4, 19 },
        { 19, 4, 7 },
        { 20, 3, 27 },

        { 21, 4, 15 },
        { 22, 4, 4 },
        { 23, 3, 24 },
        { 24, 4, 12 },

        { 25, 4, 1 },
        { 26, 4, 20 },
        { 27, 4, 9 },
        { 28, 3, 29 },

        { 29, 4, 17 },
        { 30, 4, 6 },
        { 31, 3, 26 },
        { 32, 4, 14 },

        { 33, 4, 3 },
        { 34, 3, 23 },
        { 35, 4, 11 },
        { 36, 3, 31 },

        { 37, 4, 19 },
        { 38, 4, 7 },
        { 39, 3, 27 },
        { 40, 4, 15 },
    };

    [Theory, MemberData(nameof(EpiphanyData))]
    public static void EpiphanySunday_Prop(int y, int d)
    {
        // Arrange
        var epiphanySunday = new CalendarDate(y, 1, d);
        // Act
        var actual = new RomanKalendar(y).EpiphanySunday;
        // Assert
        Assert.Equal(epiphanySunday, actual);
        Assert.Equal(DayOfWeek.Sunday, actual.DayOfWeek);
    }

    [Theory, MemberData(nameof(EasterData))]
    public static void Easter_Prop(int y, int m, int d)
    {
        // Arrange
        var easter = new CalendarDate(y, m, d);
        // Act
        var actual = new RomanKalendar(y).Easter;
        // Assert
        Assert.Equal(easter, actual);
        Assert.Equal(DayOfWeek.Sunday, actual.DayOfWeek);
    }

    [Theory(Skip = "D&R data does not match our definition of the Paschal Moon?"), MemberData(nameof(PaschalMoonData))]
    public static void PaschalMoon_Prop(int y, int m, int d)
    {
        // Arrange
        var moon = new CalendarDate(y, m, d);
        // Act
        var actual = new RomanKalendar(y).PaschalMoon;
        // Assert
        Assert.Equal(moon, actual);
    }
}
