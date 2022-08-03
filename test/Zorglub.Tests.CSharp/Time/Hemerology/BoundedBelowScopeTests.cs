// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology;

using Zorglub.Time.Core;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology.Scopes;

// TODO: on ne vérifie pas le nom du paramètre de certaines exceptions car ce
// dernier varie pas mal: Yemoda.Of(), SystemSegmentBuilder, etc.

public sealed partial class BoundedBelowScopeTests : CalendricalDataConsumer<GregorianDataSet>
{
    private const int FirstYear = 3;
    private const int FirstMonth = 4;
    private const int FirstDay = 5;

    private static readonly ICalendricalSchema s_Schema = new GregorianSchema();

    [Fact]
    public static void Create_NullSchema() =>
        Assert.ThrowsAnexn("schema",
            () => BoundedBelowScope.Create(null!, DayZero.NewStyle, new(3, 4, 5), 9999));

    [Fact]
    public static void Create_InvalidMinYear() =>
        Assert.Throws<ArgumentOutOfRangeException>(
            () => BoundedBelowScope.Create(
                s_Schema, DayZero.NewStyle, new(s_Schema.SupportedYears.Min - 1, 1, 1), 9999));

    [Fact]
    public static void Create_InvalidMaxYear() =>
        Assert.Throws<ArgumentOutOfRangeException>(
            () => BoundedBelowScope.Create(
                s_Schema, DayZero.NewStyle, new(s_Schema.SupportedYears.Max + 1, 1, 1), 9999));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public static void Create_InvalidMonth(int y, int m) =>
        Assert.Throws<ArgumentOutOfRangeException>(
            () => BoundedBelowScope.Create(s_Schema, DayZero.NewStyle, new(y, m, 1), 9999));

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public static void Create_InvalidDay(int y, int m, int d) =>
        Assert.Throws<ArgumentOutOfRangeException>(
            () => BoundedBelowScope.Create(s_Schema, DayZero.NewStyle, new(y, m, d), 9999));

    [Theory, MemberData(nameof(DateInfoData))]
    public void Create(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Act
        var scope = BoundedBelowScope.Create(s_Schema, DayZero.NewStyle, new(y, m, d), 9999);
        var minDate = scope.Segment.MinMaxDateParts.LowerValue;
        var minOrdinalDate = scope.Segment.MinMaxOrdinalParts.LowerValue;
        // Assert
        Assert.NotNull(scope);
        Assert.Equal(y, scope.YearsValidator.MinYear);
        Assert.Equal(y, minDate.Year);
        Assert.Equal(m, minDate.Month);
        Assert.Equal(d, minDate.Day);
        Assert.Equal(9999, scope.YearsValidator.MaxYear);
        Assert.Equal(doy, minOrdinalDate.DayOfYear);
    }

    [Fact]
    public static void ValidateYearMonth()
    {
        var scope = BoundedBelowScope.Create(
            s_Schema, DayZero.NewStyle, new(FirstYear, FirstMonth, FirstDay), 9999);
        // Act
        scope.ValidateYearMonth(FirstYear, FirstMonth);
        Assert.ThrowsAoorexn("month", () => scope.ValidateYearMonth(FirstYear, FirstMonth - 1));
    }

    [Fact]
    public static void ValidateYearMonthDay()
    {
        var scope = BoundedBelowScope.Create(
            s_Schema, DayZero.NewStyle, new(FirstYear, FirstMonth, FirstDay), 9999);
        // Act
        scope.ValidateYearMonthDay(FirstYear, FirstMonth, FirstDay);
        scope.ValidateYearMonthDay(FirstYear, FirstMonth, FirstDay + 1);
        scope.ValidateYearMonthDay(FirstYear, FirstMonth + 1, 1);
        Assert.ThrowsAoorexn("month", () => scope.ValidateYearMonthDay(FirstYear, FirstMonth - 1, FirstDay));
        Assert.ThrowsAoorexn("day", () => scope.ValidateYearMonthDay(FirstYear, FirstMonth, FirstDay - 1));
    }

    [Fact]
    public static void ValidateOrdinal()
    {
        int firstDayOfYear = s_Schema.CountDaysInYearBeforeMonth(FirstYear, FirstMonth) + FirstDay;
        var scope = BoundedBelowScope.Create(
            s_Schema, DayZero.NewStyle, new(FirstYear, FirstMonth, FirstDay), 9999);
        // Act
        scope.ValidateOrdinal(FirstYear, firstDayOfYear);
        Assert.ThrowsAoorexn("dayOfYear", () => scope.ValidateOrdinal(FirstYear, firstDayOfYear - 1));
    }
}
