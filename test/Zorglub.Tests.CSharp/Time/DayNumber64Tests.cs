// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time;

using System.Globalization;

using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology;

public sealed class DayNumber64Tests
{
    [Fact]
    public static void Zero()
    {
        // Act
        DayNumber64.Zero.GetGregorianParts(out long y, out int m, out int d);
        // Assert
        Assert.Equal(0, DayNumber64.Zero.DaysSinceZero);
        Assert.Equal(Ord64.First, DayNumber64.Zero.Ordinal);
        Assert.Equal(DayOfWeek.Monday, DayNumber64.Zero.DayOfWeek);
        Assert.Equal(1, y);
        Assert.Equal(1, m);
        Assert.Equal(1, d);
    }

    [Fact]
    public static void DayBeforeZero()
    {
        // Act
        DayNumber64 dayBeforeZero = DayNumber64.Zero - 1;
        dayBeforeZero.GetGregorianParts(out long y, out int m, out int d);
        // Assert
        Assert.Equal(-1, dayBeforeZero.DaysSinceZero);
        Assert.Equal(Ord64.Zeroth, dayBeforeZero.Ordinal);
        Assert.Equal(DayOfWeek.Sunday, dayBeforeZero.DayOfWeek);
        Assert.Equal(0, y);
        Assert.Equal(12, m);
        Assert.Equal(31, d);
    }

    [Fact]
    public static void Today()
    {
        var today = CivilDate.Today();
        // Assert
        Assert.Equal(today.DaysSinceEpoch, DayNumber64.Today().DaysSinceZero);
    }

    [Fact]
    public static void MinGregorianValue()
    {
        // Act
        var dayNumber = DayNumber64.FromGregorianParts(DayNumber64.MinSupportedYear, 1, 1);
        var dayNumber1 = DayNumber64.FromGregorianOrdinalParts(DayNumber64.MinSupportedYear, 1);
        dayNumber.GetGregorianParts(out long y, out int m, out int d);
        dayNumber.GetGregorianOrdinalParts(out _, out int doy);

        // Assert
        Assert.Equal(DayNumber64.GregorianDomain.Min, dayNumber);
        Assert.Equal(DayNumber64.GregorianDomain.Min, dayNumber1);

        Assert.Equal(DayNumber64.MinSupportedYear, y);
        Assert.Equal(1, m);
        Assert.Equal(1, d);
        Assert.Equal(1, doy);
    }

    [Fact]
    public static void MaxGregorianValue()
    {
        const int DaysInYear = 366; // Leap year.

        // Act
        var dayNumber = DayNumber64.FromGregorianParts(DayNumber64.MaxSupportedYear, 12, 31);
        var dayNumber1 = DayNumber64.FromGregorianOrdinalParts(DayNumber64.MaxSupportedYear, DaysInYear);
        dayNumber.GetGregorianParts(out long y, out int m, out int d);
        dayNumber.GetGregorianOrdinalParts(out _, out int doy);

        // Assert
        Assert.True(GregorianFormulae.IsLeapYear(DayNumber64.MaxSupportedYear));

        Assert.Equal(DayNumber64.GregorianDomain.Max, dayNumber);
        Assert.Equal(DayNumber64.GregorianDomain.Max, dayNumber1);

        Assert.Equal(DayNumber64.MaxSupportedYear, y);
        Assert.Equal(12, m);
        Assert.Equal(31, d);
        Assert.Equal(DaysInYear, doy);
    }
}
