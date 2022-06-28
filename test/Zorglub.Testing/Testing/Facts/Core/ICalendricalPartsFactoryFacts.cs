// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Core;

using Zorglub.Testing.Data;

// Sync with SystemSchemaFacts.

/// <summary>
/// Provides facts about <see cref="ICalendricalPartsFactory"/>.
/// </summary>
public partial class ICalendricalPartsFactoryFacts<TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected ICalendricalPartsFactoryFacts(ICalendricalPartsFactory factory)
    {
        FactoryUT = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    /// <summary>
    /// Gets the factory under test.
    /// </summary>
    protected ICalendricalPartsFactory FactoryUT { get; }
}

public partial class ICalendricalPartsFactoryFacts<TDataSet> // Conversions
{
    #region GetDateParts﹍DaysSinceEpoch()

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void GetDateParts﹍DaysSinceEpoch(DaysSinceEpochInfo info)
    {
        // Act
        var actual = FactoryUT.GetDateParts(info.DaysSinceEpoch);
        // Assert
        Assert.Equal(info.Yemoda, actual);
    }

    #endregion
    #region GetOrdinalParts﹍DaysSinceEpoch()

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void GetOrdinalParts﹍DaysSinceEpoch(DaysSinceEpochInfo info)
    {
        var (daysSinceEpoch, y, m, d) = info;
        var ydoy = FactoryUT.GetOrdinalParts(y, m, d);
        // Act
        var actual = FactoryUT.GetOrdinalParts(daysSinceEpoch);
        // Assert
        Assert.Equal(ydoy, actual);
    }

    #endregion
    #region GetOrdinalParts﹍DateParts()

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetOrdinalParts﹍DateParts(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        var actual = FactoryUT.GetOrdinalParts(y, m, d);
        // Assert
        Assert.Equal(info.Yedoy, actual);
    }

    #endregion
    #region GetDateParts﹍OrdinalParts()

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetDateParts﹍OrdinalParts(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        // Act
        var actual = FactoryUT.GetDateParts(y, doy);
        // Assert
        Assert.Equal(info.Yemoda, actual);
    }

    #endregion
}

public partial class ICalendricalPartsFactoryFacts<TDataSet> // Dates in a given year or month
{

    #region GetDatePartsAtStartOfYear()

    [Fact]
    public void GetDatePartsAtStartOfYear_AtYear1()
    {
        var ymd = new Yemoda(1, 1, 1);
        // Act
        var actual = FactoryUT.GetDatePartsAtStartOfYear(1);
        // Assert
        Assert.Equal(ymd, actual);
    }

    [Theory, MemberData(nameof(StartOfYearPartsData))]
    public void GetDatePartsAtStartOfYear(Yemoda ymd)
    {
        // Act
        var actual = FactoryUT.GetDatePartsAtStartOfYear(ymd.Year);
        // Assert
        Assert.Equal(ymd, actual);
    }

    #endregion
    #region GetOrdinalPartsAtStartOfYear()

    [Fact]
    public void GetOrdinalPartsAtStartOfYear_AtYear1()
    {
        var ydoy = new Yedoy(1, 1);
        // Act
        var actual = FactoryUT.GetOrdinalPartsAtStartOfYear(1);
        // Assert
        Assert.Equal(ydoy, actual);
    }

    [Theory, MemberData(nameof(StartOfYearDaysSinceEpochData))]
    public void GetOrdinalPartsAtStartOfYear(YearDaysSinceEpoch info)
    {
        int y = info.Year;
        var ydoy = new Yedoy(y, 1);
        // Act
        var actual = FactoryUT.GetOrdinalPartsAtStartOfYear(y);
        // Assert
        Assert.Equal(ydoy, actual);
    }

    #endregion

    #region GetDatePartsAtEndOfYear()

    [Theory, MemberData(nameof(EndOfYearPartsData))]
    public void GetDatePartsAtEndOfYear(Yemoda ymd)
    {
        // Act
        var actual = FactoryUT.GetDatePartsAtEndOfYear(ymd.Year);
        // Assert
        Assert.Equal(ymd, actual);
    }

    #endregion
    #region GetOrdinalPartsAtEndOfYear()

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetOrdinalPartsAtEndOfYear(YearInfo info)
    {
        int y = info.Year;
        var ydoy = new Yedoy(y, info.DaysInYear);
        // Act
        var actual = FactoryUT.GetOrdinalPartsAtEndOfYear(y);
        // Assert
        Assert.Equal(ydoy, actual);
    }

    #endregion

    #region GetDatePartsAtStartOfMonth()

    [Fact]
    public void GetDatePartsAtStartOfMonth_AtFirstMonthOfYear1()
    {
        var ymd = new Yemoda(1, 1, 1);
        // Act
        var actual = FactoryUT.GetDatePartsAtStartOfMonth(1, 1);
        // Assert
        Assert.Equal(ymd, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetDatePartsAtStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var ymd = new Yemoda(y, m, 1);
        // Act
        var actual = FactoryUT.GetDatePartsAtStartOfMonth(y, m);
        // Assert
        Assert.Equal(ymd, actual);
    }

    #endregion
    #region GetOrdinalPartsAtStartOfMonth()

    [Fact]
    public void GetOrdinalPartsAtStartOfMonth_AtFirstMonthOfYear1()
    {
        var ydoy = new Yedoy(1, 1);
        // Act
        var actual = FactoryUT.GetOrdinalPartsAtStartOfMonth(1, 1);
        // Assert
        Assert.Equal(ydoy, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetOrdinalPartsAtStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var ydoy = new Yedoy(y, info.DaysInYearBeforeMonth + 1);
        // Act
        var actual = FactoryUT.GetOrdinalPartsAtStartOfMonth(y, m);
        // Assert
        Assert.Equal(ydoy, actual);
    }

    #endregion

    #region GetDatePartsAtEndOfMonth()

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetDatePartsAtEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var ymd = new Yemoda(y, m, info.DaysInMonth);
        // Act
        var actual = FactoryUT.GetDatePartsAtEndOfMonth(y, m);
        // Assert
        Assert.Equal(ymd, actual);
    }

    #endregion
    #region GetOrdinalPartsAtEndOfMonth()

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetOrdinalPartsAtEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var ydoy = new Yedoy(y, info.DaysInYearBeforeMonth + info.DaysInMonth);
        // Act
        var actual = FactoryUT.GetOrdinalPartsAtEndOfMonth(y, m);
        // Assert
        Assert.Equal(ydoy, actual);
    }

    #endregion
}
