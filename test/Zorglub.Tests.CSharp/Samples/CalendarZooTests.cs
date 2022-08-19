// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Samples;

using global::Samples;

using Zorglub.Time;
using Zorglub.Time.Core;
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
        var parts = chr.MinDateParts;
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(new DateParts(1582, 10, 15), parts);
        Assert.Equal(DefaultMaxYear, chr.Scope.MaxYear);
    }

    [Fact]
    public static void GenuineGregorian_Repeated() =>
        Assert.Same(CalendarZoo.GenuineGregorian, CalendarZoo.GenuineGregorian);

    [Fact]
    public static void GenuineJulian()
    {
        // Act
        var chr = CalendarZoo.GenuineJulian;
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(8, chr.Scope.MinYear);
        Assert.Equal(DefaultMaxYear, chr.Scope.MaxYear);
    }

    [Fact]
    public static void GenuineJulian_Repeated() =>
        Assert.Same(CalendarZoo.GenuineJulian, CalendarZoo.GenuineJulian);

    [Fact]
    public static void FrenchRevolutionary()
    {
        // Act
        var chr = CalendarZoo.FrenchRevolutionary;
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(1, chr.Scope.MinYear);
        Assert.Equal(14, chr.Scope.MaxYear);
    }

    [Fact]
    public static void FrenchRevolutionary_Repeated() =>
        Assert.Same(CalendarZoo.FrenchRevolutionary, CalendarZoo.FrenchRevolutionary);
}

// Proleptic calendars.
public partial class CalendarZooTests
{
    [Fact]
    public static void Tropicalia()
    {
        // Act
        var chr = CalendarZoo.Tropicalia;
        var (minYear, maxYear) = chr.MinMaxYear.Select(x => x.Year);
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(ProlepticScope.MinSupportedYear, minYear);
        Assert.Equal(ProlepticScope.MaxSupportedYear, maxYear);
    }

    [Fact]
    public static void Tropicalia_Repeated() =>
        Assert.Same(CalendarZoo.Tropicalia, CalendarZoo.Tropicalia);

    [Fact]
    public static void LongGregorian()
    {
        // Act
        var chr = CalendarZoo.LongGregorian;
        var (minYear, maxYear) = chr.MinMaxDate.Select(x => x.Year);
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(DefaultMinYear, minYear);
        Assert.Equal(DefaultMaxYear, maxYear);
    }

    [Fact]
    public static void LongGregorian_Repeated() =>
        Assert.Same(CalendarZoo.LongGregorian, CalendarZoo.LongGregorian);

    [Fact]
    public static void LongJulian()
    {
        // Act
        var chr = CalendarZoo.LongJulian;
        var (minYear, maxYear) = chr.MinMaxDate.Select(x => x.Year);
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(DefaultMinYear, minYear);
        Assert.Equal(DefaultMaxYear, maxYear);
    }

    [Fact]
    public static void LongJulian_Repeated() =>
        Assert.Same(CalendarZoo.LongJulian, CalendarZoo.LongJulian);
}

// Retropolated calendars.
public partial class CalendarZooTests
{
    [Fact]
    public static void Egyptian()
    {
        // Act
        var chr = CalendarZoo.Egyptian;
        var (minYear, maxYear) = chr.MinMaxYear.Select(x => x.Year);
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(StandardScope.MinSupportedYear, minYear);
        Assert.Equal(StandardScope.MaxSupportedYear, maxYear);
    }

    [Fact]
    public static void Egyptian_Repeated() =>
        Assert.Same(CalendarZoo.Egyptian, CalendarZoo.Egyptian);

    [Fact]
    public static void FrenchRepublican()
    {
        // Act
        var chr = CalendarZoo.FrenchRepublican;
        var (minYear, maxYear) = chr.MinMaxYear.Select(x => x.Year);
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(StandardScope.MinSupportedYear, minYear);
        Assert.Equal(StandardScope.MaxSupportedYear, maxYear);
    }

    [Fact]
    public static void FrenchRepublican_Repeated() =>
        Assert.Same(CalendarZoo.FrenchRepublican, CalendarZoo.FrenchRepublican);

    [Fact]
    public static void InternationalFixed()
    {
        // Act
        var chr = CalendarZoo.InternationalFixed;
        var (minYear, maxYear) = chr.MinMaxYear.Select(x => x.Year);
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(StandardScope.MinSupportedYear, minYear);
        Assert.Equal(StandardScope.MaxSupportedYear, maxYear);
    }

    [Fact]
    public static void InternationalFixed_Repeated() =>
        Assert.Same(CalendarZoo.InternationalFixed, CalendarZoo.InternationalFixed);

    [Fact]
    public static void Persian2820()
    {
        // Act
        var chr = CalendarZoo.Persian2820;
        var (minYear, maxYear) = chr.MinMaxYear.Select(x => x.Year);
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(StandardScope.MinSupportedYear, minYear);
        Assert.Equal(StandardScope.MaxSupportedYear, maxYear);
    }

    [Fact]
    public static void Persian2820_Repeated() =>
        Assert.Same(CalendarZoo.Persian2820, CalendarZoo.Persian2820);

    [Fact]
    public static void Positivist()
    {
        // Act
        var chr = CalendarZoo.Positivist;
        var (minYear, maxYear) = chr.MinMaxYear.Select(x => x.Year);
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(StandardScope.MinSupportedYear, minYear);
        Assert.Equal(StandardScope.MaxSupportedYear, maxYear);
    }

    [Fact]
    public static void Positivist_Repeated() =>
        Assert.Same(CalendarZoo.Positivist, CalendarZoo.Positivist);

    [Fact]
    public static void RevisedWorld()
    {
        // Act
        var chr = CalendarZoo.RevisedWorld;
        var (minYear, maxYear) = chr.MinMaxYear.Select(x => x.Year);
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(StandardScope.MinSupportedYear, minYear);
        Assert.Equal(StandardScope.MaxSupportedYear, maxYear);
    }

    [Fact]
    public static void RevisedWorld_Repeated() =>
        Assert.Same(CalendarZoo.RevisedWorld, CalendarZoo.RevisedWorld);

    [Fact]
    public static void World()
    {
        // Act
        var chr = CalendarZoo.World;
        var (minYear, maxYear) = chr.MinMaxYear.Select(x => x.Year);
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(StandardScope.MinSupportedYear, minYear);
        Assert.Equal(StandardScope.MaxSupportedYear, maxYear);
    }

    [Fact]
    public static void World_Repeated() =>
        Assert.Same(CalendarZoo.World, CalendarZoo.World);
}

// Offset calendars.
public partial class CalendarZooTests
{
    [Fact]
    public static void Holocene()
    {
        // Act
        var chr = CalendarZoo.Holocene;
        var (minYear, maxYear) = chr.MinMaxDate.Select(x => x.Year);
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(1, chr.Scope.MinYear);
        Assert.Equal(DefaultMaxYear, maxYear);
    }

    [Fact]
    public static void Holocene_Repeated() =>
        Assert.Same(CalendarZoo.Holocene, CalendarZoo.Holocene);

    [Fact]
    public static void Minguo()
    {
        // Act
        var chr = CalendarZoo.Minguo;
        var (minYear, maxYear) = chr.MinMaxDate.Select(x => x.Year);
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(1, minYear);
        Assert.Equal(DefaultMaxYear, maxYear);
    }

    [Fact]
    public static void Minguo_Repeated() =>
        Assert.Same(CalendarZoo.Minguo, CalendarZoo.Minguo);

    [Fact]
    public static void ThaiSolar()
    {
        // Act
        var chr = CalendarZoo.ThaiSolar;
        var (minYear, maxYear) = chr.MinMaxDate.Select(x => x.Year);
        // Assert
        Assert.NotNull(chr);
        Assert.Equal(1, minYear);
        Assert.Equal(DefaultMaxYear, maxYear);
    }

    [Fact]
    public static void ThaiSolar_Repeated() =>
        Assert.Same(CalendarZoo.ThaiSolar, CalendarZoo.ThaiSolar);
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
        Assert.Same(CalendarZoo.Pax, CalendarZoo.Pax);
#endif
}
