// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Samples;

using global::Samples;

using Zorglub.Time.Core;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

public static partial class CalendarZooTests
{
    private static readonly int DefaultMinYear = SystemSchema.DefaultSupportedYears.Min;
    private static readonly int DefaultMaxYear = SystemSchema.DefaultSupportedYears.Max;
}

// More historically accurate calendars.
public partial class CalendarZooTests
{
    [Fact]
    public static void GenuineGregorian()
    {
        // Act
        var chr = CalendarZoo.GenuineGregorian;
        var parts = chr.MinMaxDateParts.LowerValue;
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(1582, parts.Year);
        Assert.Equal(10, parts.Month);
        Assert.Equal(15, parts.Day);
        Assert.Equal(DefaultMaxYear, chr.SupportedYears.Max);
    }

    [Fact]
    public static void GenuineGregorian_Repeated() =>
        Assert.Equal(CalendarZoo.GenuineGregorian, CalendarZoo.GenuineGregorian);

    [Fact]
    public static void GenuineJulian()
    {
        // Act
        var chr = CalendarZoo.GenuineJulian;
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(8, chr.SupportedYears.Min);
        Assert.Equal(DefaultMaxYear, chr.SupportedYears.Max);
    }

    [Fact]
    public static void GenuineJulian_Repeated() =>
        Assert.Equal(CalendarZoo.GenuineJulian, CalendarZoo.GenuineJulian);

    [Fact]
    public static void FrenchRevolutionary()
    {
        // Act
        var chr = CalendarZoo.FrenchRevolutionary;
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(1, chr.SupportedYears.Min);
        Assert.Equal(14, chr.SupportedYears.Max);
    }

    [Fact]
    public static void FrenchRevolutionary_Repeated() =>
        Assert.Equal(CalendarZoo.FrenchRevolutionary, CalendarZoo.FrenchRevolutionary);
}

// Proleptic calendars.
public partial class CalendarZooTests
{
    [Fact]
    public static void Tropicalia()
    {
        // Act
        var chr = CalendarZoo.Tropicalia;
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(ProlepticScope.MinYear, chr.SupportedYears.Min);
        Assert.Equal(ProlepticScope.MaxYear, chr.SupportedYears.Max);
    }

    [Fact]
    public static void Tropicalia_Repeated() =>
        Assert.Equal(CalendarZoo.Tropicalia, CalendarZoo.Tropicalia);

    [Fact]
    public static void LongGregorian()
    {
        // Act
        var chr = WideCalendar.Gregorian;
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(DefaultMinYear, chr.SupportedYears.Min);
        Assert.Equal(DefaultMaxYear, chr.SupportedYears.Max);
    }

    [Fact]
    public static void LongGregorian_Repeated() =>
        Assert.Equal(WideCalendar.Gregorian, WideCalendar.Gregorian);

    [Fact]
    public static void LongJulian()
    {
        // Act
        var chr = CalendarZoo.LongJulian;
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(DefaultMinYear, chr.SupportedYears.Min);
        Assert.Equal(DefaultMaxYear, chr.SupportedYears.Max);
    }

    [Fact]
    public static void LongJulian_Repeated() =>
        Assert.Equal(CalendarZoo.LongJulian, CalendarZoo.LongJulian);
}

// Retropolated calendars.
public partial class CalendarZooTests
{
    [Fact]
    public static void Egyptian()
    {
        // Act
        var chr = CalendarZoo.Egyptian;
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(StandardScope.MinYear, chr.SupportedYears.Min);
        Assert.Equal(StandardScope.MaxYear, chr.SupportedYears.Max);
    }

    [Fact]
    public static void Egyptian_Repeated() =>
        Assert.Equal(CalendarZoo.Egyptian, CalendarZoo.Egyptian);

    [Fact]
    public static void FrenchRepublican()
    {
        // Act
        var chr = CalendarZoo.FrenchRepublican;
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(StandardScope.MinYear, chr.SupportedYears.Min);
        Assert.Equal(StandardScope.MaxYear, chr.SupportedYears.Max);
    }

    [Fact]
    public static void FrenchRepublican_Repeated() =>
        Assert.Equal(CalendarZoo.FrenchRepublican, CalendarZoo.FrenchRepublican);

    [Fact]
    public static void InternationalFixed()
    {
        // Act
        var chr = CalendarZoo.InternationalFixed;
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(StandardScope.MinYear, chr.SupportedYears.Min);
        Assert.Equal(StandardScope.MaxYear, chr.SupportedYears.Max);
    }

    [Fact]
    public static void InternationalFixed_Repeated() =>
        Assert.Equal(CalendarZoo.InternationalFixed, CalendarZoo.InternationalFixed);

    [Fact]
    public static void Persian2820()
    {
        // Act
        var chr = CalendarZoo.Persian2820;
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(StandardScope.MinYear, chr.SupportedYears.Min);
        Assert.Equal(StandardScope.MaxYear, chr.SupportedYears.Max);
    }

    [Fact]
    public static void Persian2820_Repeated() =>
        Assert.Equal(CalendarZoo.Persian2820, CalendarZoo.Persian2820);

    [Fact]
    public static void Positivist()
    {
        // Act
        var chr = CalendarZoo.Positivist;
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(StandardScope.MinYear, chr.SupportedYears.Min);
        Assert.Equal(StandardScope.MaxYear, chr.SupportedYears.Max);
    }

    [Fact]
    public static void Positivist_Repeated() =>
        Assert.Equal(CalendarZoo.Positivist, CalendarZoo.Positivist);

    [Fact]
    public static void RevisedWorld()
    {
        // Act
        var chr = CalendarZoo.RevisedWorld;
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(StandardScope.MinYear, chr.SupportedYears.Min);
        Assert.Equal(StandardScope.MaxYear, chr.SupportedYears.Max);
    }

    [Fact]
    public static void RevisedWorld_Repeated() =>
        Assert.Equal(CalendarZoo.RevisedWorld, CalendarZoo.RevisedWorld);

    [Fact]
    public static void World()
    {
        // Act
        var chr = CalendarZoo.World;
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(StandardScope.MinYear, chr.SupportedYears.Min);
        Assert.Equal(StandardScope.MaxYear, chr.SupportedYears.Max);
    }

    [Fact]
    public static void World_Repeated() =>
        Assert.Equal(CalendarZoo.World, CalendarZoo.World);
}

// Offset calendars.
public partial class CalendarZooTests
{
    [Fact]
    public static void Holocene()
    {
        // Act
        var chr = CalendarZoo.Holocene;
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(1, chr.SupportedYears.Min);
        Assert.Equal(DefaultMaxYear, chr.SupportedYears.Max);
    }

    [Fact]
    public static void Holocene_Repeated() =>
        Assert.Equal(CalendarZoo.Holocene, CalendarZoo.Holocene);

    [Fact]
    public static void Minguo()
    {
        // Act
        var chr = CalendarZoo.Minguo;
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(1, chr.SupportedYears.Min);
        Assert.Equal(DefaultMaxYear, chr.SupportedYears.Max);
    }

    [Fact]
    public static void Minguo_Repeated() =>
        Assert.Equal(CalendarZoo.Minguo, CalendarZoo.Minguo);

    [Fact]
    public static void ThaiSolar()
    {
        // Act
        var chr = CalendarZoo.ThaiSolar;
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(1, chr.SupportedYears.Min);
        Assert.Equal(DefaultMaxYear, chr.SupportedYears.Max);
    }

    [Fact]
    public static void ThaiSolar_Repeated() =>
        Assert.Equal(CalendarZoo.ThaiSolar, CalendarZoo.ThaiSolar);
}

// Other wide calendars.
public partial class CalendarZooTests
{
#if false // TODO(code): unfinished Pax schema.
    [Fact]
    public static void Pax()
    {
        // Act
        var chr = CalendarZoo.Pax;
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(1, chr.SupportedYears.Min);
        Assert.Equal(SystemSchema.DefaultMaxYear, chr.SupportedYears.Max);
    }

    [Fact]
    public static void Pax_Repeated() =>
        Assert.Equal(CalendarZoo.Pax, CalendarZoo.Pax);
#endif
}
