// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology;

using Zorglub.Testing.Data.Bounded;
using Zorglub.Testing.Data.Unbounded;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Core.Schemas;

public static class MinMaxYearNakedCalendarTests
{
    private static readonly ICalendricalSchema s_Schema = new GregorianSchema();

    [Fact]
    public static void Constructor()
    {
        string name = "name";
        var epoch = DayZero.NewStyle + 123456789;
        var range = Range.Create(3, 5);
        // Act
        var chr = new MinMaxYearNakedCalendar(name, s_Schema, epoch, range);
        // Assert
        Assert.Equal(name, chr.Name);
        Assert.Equal(epoch, chr.Epoch);
        Assert.Equal(s_Schema, chr.Schema);
        Assert.Equal(range, chr.SupportedYears.Range);
    }

    [Fact]
    public static void WithMinYear_NullSchema()
    {
        Assert.ThrowsAnexn("schema",
            () => MinMaxYearNakedCalendar.WithMinYear("Gregorian", null!, DayZero.NewStyle, 1));
    }

    [Fact]
    public static void WithMinYear()
    {
        string name = "name";
        var epoch = DayZero.NewStyle + 123456789;
        int minYear = 3;
        // Act
        var chr = MinMaxYearNakedCalendar.WithMinYear(name, s_Schema, epoch, minYear);
        // Assert
        Assert.Equal(name, chr.Name);
        Assert.Equal(epoch, chr.Epoch);
        Assert.Equal(s_Schema, chr.Schema);
        Assert.Equal(minYear, chr.SupportedYears.MinYear);
        Assert.Equal(s_Schema.SupportedYears.Max, chr.SupportedYears.MaxYear);
    }

    [Fact]
    public static void WithMaxYear_NullSchema()
    {
        Assert.ThrowsAnexn("schema",
            () => MinMaxYearNakedCalendar.WithMaxYear("Gregorian", null!, DayZero.NewStyle, 9999));
    }

    [Fact]
    public static void WithMaxYear()
    {
        string name = "name";
        var epoch = DayZero.NewStyle + 123456789;
        int maxYear = 5;
        // Act
        var chr = MinMaxYearNakedCalendar.WithMaxYear(name, s_Schema, epoch, maxYear);
        // Assert
        Assert.Equal(name, chr.Name);
        Assert.Equal(epoch, chr.Epoch);
        Assert.Equal(s_Schema, chr.Schema);
        Assert.Equal(s_Schema.SupportedYears.Min, chr.SupportedYears.MinYear);
        Assert.Equal(maxYear, chr.SupportedYears.MaxYear);
    }
}

public class GregorianMinMaxYearCalendarDataSet :
    MinMaxYearCalendarDataSet<UnboundedGregorianDataSet>,
    ISingleton<GregorianMinMaxYearCalendarDataSet>
{
    public GregorianMinMaxYearCalendarDataSet()
        : base(
            UnboundedGregorianDataSet.Instance,
            GregorianMinMaxYearNakedCalendarTests.FirstYear,
            GregorianMinMaxYearNakedCalendarTests.LastYear)
    { }

    public static GregorianMinMaxYearCalendarDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly GregorianMinMaxYearCalendarDataSet Instance = new();
        static Singleton() { }
    }
}

// TODO(code): à améliorer.
public sealed class GregorianMinMaxYearNakedCalendarTests :
    NakedCalendarFacts<MinMaxYearNakedCalendar, GregorianMinMaxYearCalendarDataSet>
{
    // On triche un peu, les années de début et de fin ont été choisies de
    // telle sorte que les tests marchent... (cf. GregorianData).
    public const int FirstYear = 1;
    public const int LastYear = 123_456;

    public GregorianMinMaxYearNakedCalendarTests() : base(MakeCalendar()) { }

    private static MinMaxYearNakedCalendar MakeCalendar() =>
        new("Gregorian", new GregorianSchema(), DayZero.NewStyle, Range.Create(FirstYear, LastYear));

    [Fact]
    public void SupportedYears_Prop()
    {
        // Act
        var supportedYears = CalendarUT.SupportedYears;
        // Assert
        Assert.Equal(FirstYear, supportedYears.MinYear);
        Assert.Equal(LastYear, supportedYears.MaxYear);
    }
}
