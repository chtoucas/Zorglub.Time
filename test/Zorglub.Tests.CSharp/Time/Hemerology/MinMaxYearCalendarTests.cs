// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology;

using Zorglub.Testing.Data.Bounded;
using Zorglub.Testing.Data.Unbounded;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Schemas;

public static class MinMaxYearCalendarTests
{
    private static readonly ICalendricalSchema s_Schema = new GregorianSchema();

    [Fact]
    public static void Constructor()
    {
        string name = "name";
        var epoch = DayZero.NewStyle + 123456789;
        int minYear = 3;
        int maxYear = 5;
        // Act
        var chr = new MinMaxYearCalendar(name, s_Schema, epoch, minYear, maxYear);
        // Assert
        Assert.Equal(name, chr.Name);
        Assert.Equal(epoch, chr.Epoch);
        Assert.Equal(s_Schema, chr.Schema);
        Assert.Equal(minYear, chr.Scope.SupportedYears.Min);
        Assert.Equal(maxYear, chr.Scope.SupportedYears.Max);
    }

    [Fact]
    public static void WithMinYear_NullSchema()
    {
        Assert.ThrowsAnexn("schema",
            () => MinMaxYearCalendar.WithMinYear("Gregorian", null!, DayZero.NewStyle, 1));
    }

    [Fact]
    public static void WithMinYear()
    {
        string name = "name";
        var epoch = DayZero.NewStyle + 123456789;
        int minYear = 3;
        // Act
        var chr = MinMaxYearCalendar.WithMinYear(name, s_Schema, epoch, minYear);
        // Assert
        Assert.Equal(name, chr.Name);
        Assert.Equal(epoch, chr.Epoch);
        Assert.Equal(s_Schema, chr.Schema);
        Assert.Equal(minYear, chr.Scope.SupportedYears.Min);
        Assert.Equal(s_Schema.SupportedYears.Max, chr.Scope.SupportedYears.Max);
    }

    [Fact]
    public static void WithMaxYear_NullSchema()
    {
        Assert.ThrowsAnexn("schema",
            () => MinMaxYearCalendar.WithMaxYear("Gregorian", null!, DayZero.NewStyle, 9999));
    }

    [Fact]
    public static void WithMaxYear()
    {
        string name = "name";
        var epoch = DayZero.NewStyle + 123456789;
        int maxYear = 5;
        // Act
        var chr = MinMaxYearCalendar.WithMaxYear(name, s_Schema, epoch, maxYear);
        // Assert
        Assert.Equal(name, chr.Name);
        Assert.Equal(epoch, chr.Epoch);
        Assert.Equal(s_Schema, chr.Schema);
        Assert.Equal(s_Schema.SupportedYears.Min, chr.Scope.SupportedYears.Min);
        Assert.Equal(maxYear, chr.Scope.SupportedYears.Max);
    }
}

public class GregorianMinMaxYearCalendarDataSet :
    MinMaxYearCalendarDataSet<UnboundedGregorianDataSet>,
    ISingleton<GregorianMinMaxYearCalendarDataSet>
{
    public GregorianMinMaxYearCalendarDataSet()
        : base(
            UnboundedGregorianDataSet.Instance,
            GregorianMinMaxYearCalendarTests.FirstYear,
            GregorianMinMaxYearCalendarTests.LastYear)
    { }

    public static GregorianMinMaxYearCalendarDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly GregorianMinMaxYearCalendarDataSet Instance = new();
        static Singleton() { }
    }
}

// TODO(code): à améliorer.
public sealed class GregorianMinMaxYearCalendarTests :
    NakedCalendarFacts<MinMaxYearCalendar, GregorianMinMaxYearCalendarDataSet>
{
    // On triche un peu, les années de début et de fin ont été choisies de
    // telle sorte que les tests marchent... (cf. GregorianData).
    public const int FirstYear = 1;
    public const int LastYear = 123_456;

    public GregorianMinMaxYearCalendarTests() : base(MakeCalendar()) { }

    private static MinMaxYearCalendar MakeCalendar() =>
        new("Gregorian", new GregorianSchema(), DayZero.NewStyle, FirstYear, LastYear);

    [Fact]
    public void MinYear_Prop() =>
        Assert.Equal(FirstYear, CalendarUT.Scope.SupportedYears.Min);

    [Fact]
    public void MaxYear_Prop() =>
        Assert.Equal(LastYear, CalendarUT.Scope.SupportedYears.Max);
}
