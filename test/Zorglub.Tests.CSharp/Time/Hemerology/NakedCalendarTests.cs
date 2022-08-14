// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology;

using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology.Scopes;

public static class NakedCalendarTests
{
    private static readonly GregorianSchema s_Schema = new();

    [Fact]
    public static void Constructor_InvalidName() =>
        Assert.ThrowsAnexn("name",
            () => new FauxNakedCalendar(
                null!, new FauxCalendarScope(s_Schema, 1, 2)));

    [Fact]
    public static void Constructor_InvalidScope() =>
        Assert.ThrowsAnexn("scope", () => new FauxNakedCalendar("name", null!));

    [Fact]
    public static void Constructor()
    {
        string name = "name";
        var epoch = DayZero.NewStyle + 123456789;
        var scope = new FauxCalendarScope(s_Schema, epoch, 1, 2);
        // Act
        var chr = new FauxNakedCalendar(name, scope);
        // Assert
        Assert.Equal(name, chr.Name);
        Assert.Equal(epoch, chr.Epoch);
        Assert.Equal(s_Schema, chr.Schema);
        Assert.Equal(scope, chr.Scope);
    }
}
