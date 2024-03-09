// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Hemerology;

using Zorglub.Testing.Data.Bounded;
using Zorglub.Testing.Data.Unbounded;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology.Scopes;

public static class MinMaxYearCalendarTests
{
    private static readonly ICalendricalSchema s_Schema = new GregorianSchema();

    [Fact]
    public static void Constructor()
    {
        string name = "name";
        var epoch = DayZero.NewStyle + 123456789;
        var range = Range.Create(3, 5);
        var scope = MinMaxYearScope.Create(s_Schema, epoch, range);
        // Act
        var chr = new MinMaxYearCalendar(name, scope);
        // Assert
        Assert.Equal(name, chr.Name);
        Assert.Equal(epoch, chr.Epoch);
        Assert.Equal(s_Schema, chr.Schema);
        Assert.Equal(range, chr.YearsValidator.Range);
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
    INakedCalendarFacts<MinMaxYearCalendar, GregorianMinMaxYearCalendarDataSet>
{
    // On triche un peu, les années de début et de fin ont été choisies de
    // telle sorte que les tests marchent... (cf. GregorianData).
    public const int FirstYear = 1;
    public const int LastYear = 123_456;

    public GregorianMinMaxYearCalendarTests() : base(MakeCalendar()) { }

    private static MinMaxYearCalendar MakeCalendar() =>
        new(
            "Gregorian",
            MinMaxYearScope.Create(new GregorianSchema(), DayZero.NewStyle, Range.Create(FirstYear, LastYear)));

    [Fact]
    public void SupportedYears_Prop()
    {
        // Act
        var supportedYears = CalendarUT.Scope;
        // Assert
        Assert.Equal(FirstYear, supportedYears.MinYear);
        Assert.Equal(LastYear, supportedYears.MaxYear);
    }
}
