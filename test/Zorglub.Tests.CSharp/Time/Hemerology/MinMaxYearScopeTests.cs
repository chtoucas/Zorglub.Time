// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology;

using Zorglub.Time.Core;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology.Scopes;

public static class MinMaxYearScopeTests
{
    private static readonly ICalendricalSchema s_Schema = new GregorianSchema();

    [Fact]
    public static void Create_NullSchema() =>
        Assert.ThrowsAnexn("schema",
            () => MinMaxYearScope.Create(null!, DayZero.NewStyle, Range.Create(1, 100)));

    [Fact]
    public static void StartingAtYear()
    {
        // Act
        var scope = MinMaxYearScope.StartingAt(s_Schema, DayZero.NewStyle, 100);
        // Assert
        Assert.NotNull(scope);
        Assert.Equal(100, scope.YearsValidator.MinYear);
        Assert.Equal(s_Schema.SupportedYears.Max, scope.YearsValidator.MaxYear);
    }

    [Fact]
    public static void EndingAtYear()
    {
        // Act
        var scope = MinMaxYearScope.EndingAt(s_Schema, DayZero.NewStyle, 100);
        // Assert
        Assert.NotNull(scope);
        Assert.Equal(s_Schema.SupportedYears.Min, scope.YearsValidator.MinYear);
        Assert.Equal(100, scope.YearsValidator.MaxYear);
    }
}
