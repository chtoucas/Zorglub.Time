// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology;

// TODO(code): dépassements arithmétiques.

using DataSet = YearNumberingDataSet;

public static class YearNumberingTests
{
    [Theory, MemberData(nameof(DataSet.CenturyInfoData), MemberType = typeof(DataSet))]
    public static void GetCentury_Internal(CenturyInfo info) =>
        Assert.Equal(info.Century, YearNumbering.GetCentury(info.Year));

    [Theory, MemberData(nameof(DataSet.CenturyInfoData), MemberType = typeof(DataSet))]
    public static void GetYearOfCentury_Internal(CenturyInfo info) =>
        Assert.Equal(info.YearOfCentury, YearNumbering.GetYearOfCentury(info.Year));

    //
    // Decades of the century
    //

    [Theory, MemberData(nameof(DataSet.DecadeOfCenturyInfoData), MemberType = typeof(DataSet))]
    public static void GetDecadeOfCentury(DecadeOfCenturyInfo info)
    {
        var (y, century, decadeOfCentury, yearOfDecade) = info;
        // Act
        int decadeOfCenturyA = YearNumbering.GetDecadeOfCentury(
            y, out int centuryA, out int yearOfDecadeA);
        // Assert
        Assert.Equal(century, centuryA);
        Assert.Equal(decadeOfCentury, decadeOfCenturyA);
        Assert.Equal(yearOfDecade, yearOfDecadeA);
    }

    [Theory, MemberData(nameof(DataSet.IsoDecadeOfCenturyInfoData), MemberType = typeof(DataSet))]
    public static void GetIsoDecadeOfCentury(DecadeOfCenturyInfo info)
    {
        var (y, century, decadeOfCentury, yearOfDecade) = info;
        // Act
        int decadeOfCenturyA = YearNumbering.GetIsoDecadeOfCentury(
            y, out int centuryA, out int yearOfDecadeA);
        // Assert
        Assert.Equal(century, centuryA);
        Assert.Equal(decadeOfCentury, decadeOfCenturyA);
        Assert.Equal(yearOfDecade, yearOfDecadeA);
    }

    //
    // Decades
    //

    [Theory, MemberData(nameof(DataSet.DecadeInfoData), MemberType = typeof(DataSet))]
    public static void GetDecade(DecadeInfo info)
    {
        var (y, decade, yearOfDecade) = info;
        // Act
        int decadeA = YearNumbering.GetDecade(y, out int yearOfDecadeA);
        // Assert
        Assert.Equal(decade, decadeA);
        Assert.Equal(yearOfDecade, yearOfDecadeA);
    }

    [Theory, MemberData(nameof(DataSet.DecadeInfoData), MemberType = typeof(DataSet))]
    public static void GetYearFromDecade(DecadeInfo info)
    {
        var (y, decade, yearOfDecade) = info;
        // Act
        int actual = YearNumbering.GetYearFromDecade(decade, yearOfDecade);
        // Assert
        Assert.Equal(y, actual);
    }

    [Theory, MemberData(nameof(DataSet.IsoDecadeInfoData), MemberType = typeof(DataSet))]
    public static void GetIsoDecade(DecadeInfo info)
    {
        var (y, decade, yearOfDecade) = info;
        // Act
        int decadeA = YearNumbering.GetIsoDecade(y, out int yearOfDecadeA);
        // Assert
        Assert.Equal(decade, decadeA);
        Assert.Equal(yearOfDecade, yearOfDecadeA);
    }

    [Theory, MemberData(nameof(DataSet.IsoDecadeInfoData), MemberType = typeof(DataSet))]
    public static void GetYearFromIsoDecade(DecadeInfo info)
    {
        var (y, decade, yearOfDecade) = info;
        // Act
        int actual = YearNumbering.GetYearFromIsoDecade(decade, yearOfDecade);
        // Assert
        Assert.Equal(y, actual);
    }

    //
    // Centuries
    //

    [Theory, MemberData(nameof(DataSet.CenturyInfoData), MemberType = typeof(DataSet))]
    public static void GetCentury(CenturyInfo info)
    {
        var (y, century, yearOfCentury) = info;
        // Act
        int centuryA = YearNumbering.GetCentury(y, out int yearOfCenturyA);
        // Assert
        Assert.Equal(century, centuryA);
        Assert.Equal(yearOfCentury, yearOfCenturyA);
    }

    [Theory, MemberData(nameof(DataSet.CenturyInfoData), MemberType = typeof(DataSet))]
    public static void GetYearFromCentury(CenturyInfo info)
    {
        var (y, century, yearOfCentury) = info;
        // Act
        int actual = YearNumbering.GetYearFromCentury(century, yearOfCentury);
        // Assert
        Assert.Equal(y, actual);
    }

    [Theory, MemberData(nameof(DataSet.IsoCenturyInfoData), MemberType = typeof(DataSet))]
    public static void GetIsoCentury(CenturyInfo info)
    {
        var (y, century, yearOfCentury) = info;
        // Act
        int centuryA = YearNumbering.GetIsoCentury(y, out int yearOfCenturyA);
        // Assert
        Assert.Equal(century, centuryA);
        Assert.Equal(yearOfCentury, yearOfCenturyA);
    }

    [Theory, MemberData(nameof(DataSet.IsoCenturyInfoData), MemberType = typeof(DataSet))]
    public static void GetYearFromIsoCentury(CenturyInfo info)
    {
        var (y, century, yearOfCentury) = info;
        // Act
        int actual = YearNumbering.GetYearFromIsoCentury(century, yearOfCentury);
        // Assert
        Assert.Equal(y, actual);
    }

    //
    // Millennia
    //

    [Theory, MemberData(nameof(DataSet.MillenniumInfoData), MemberType = typeof(DataSet))]
    public static void GetMillennium(MillenniumInfo info)
    {
        var (y, millennium, yearOfMillennium) = info;
        // Act
        int millenniumA = YearNumbering.GetMillennium(y, out int yearOfMillenniumA);
        // Assert
        Assert.Equal(millennium, millenniumA);
        Assert.Equal(yearOfMillennium, yearOfMillenniumA);
    }

    [Theory, MemberData(nameof(DataSet.MillenniumInfoData), MemberType = typeof(DataSet))]
    public static void GetYearFromMillennium(MillenniumInfo info)
    {
        var (y, millennium, yearOfMillennium) = info;
        // Act
        int actual = YearNumbering.GetYearFromMillennium(millennium, yearOfMillennium);
        // Assert
        Assert.Equal(y, actual);
    }

    [Theory, MemberData(nameof(DataSet.IsoMillenniumInfoData), MemberType = typeof(DataSet))]
    public static void GetIsoMillennium(MillenniumInfo info)
    {
        var (y, millennium, yearOfMillennium) = info;
        // Act
        int millenniumA = YearNumbering.GetIsoMillennium(y, out int yearOfMillenniumA);
        // Assert
        Assert.Equal(millennium, millenniumA);
        Assert.Equal(yearOfMillennium, yearOfMillenniumA);
    }

    [Theory, MemberData(nameof(DataSet.IsoMillenniumInfoData), MemberType = typeof(DataSet))]
    public static void GetYearFromIsoMillennium(MillenniumInfo info)
    {
        var (y, millennium, yearOfMillennium) = info;
        // Act
        int actual = YearNumbering.GetYearFromIsoMillennium(millennium, yearOfMillennium);
        // Assert
        Assert.Equal(y, actual);
    }
}
