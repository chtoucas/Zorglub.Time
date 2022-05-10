// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Geometry;

using Zorglub.Testing.Data;
using Zorglub.Time.Geometry.Schemas;

/// <summary>
/// Provides facts about <see cref="SecondOrderSchema"/>.
/// </summary>
public abstract class SecondOrderSchemaFacts<TDataSet> :
    IGeometricSchemaFacts<TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected SecondOrderSchemaFacts(SecondOrderSchema schema) : base(schema)
    {
        SecondOrderSchemaUT = schema;
    }

    protected SecondOrderSchema SecondOrderSchemaUT { get; }

    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYear(YearInfo info)
    {
        int y = info.Year;

        // Act
        int actual = SecondOrderSchemaUT.CountDaysInYear(y);
        // Assert
        Assert.Equal(info.DaysInYear, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInYearBeforeMonth(MonthInfo info)
    {
        var (_, m) = info.Yemo;
        // Act
        int actual = SecondOrderSchemaUT.CountDaysInYearBeforeMonth(m);
        // Assert
        Assert.Equal(info.DaysInYearBeforeMonth, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetYear(DateInfo info)
    {
        var (y, m, d, doy) = info;
        int daysSinceEpoch = SchemaUT.CountDaysSinceEpoch(y, m, d);
        // Act
        int yA = SecondOrderSchemaUT.GetYear(daysSinceEpoch, out int doyA);
        // Assert
        Assert.Equal(y, yA);
        Assert.Equal(doy, doyA);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetMonth﹍Out(DateInfo info)
    {
        var (_, m, d, doy) = info;
        // Act
        int mA = SecondOrderSchemaUT.GetMonth(doy, out int dA);
        // Assert
        Assert.Equal(m, mA);
        Assert.Equal(d, dA);
    }

    [Theory, MemberData(nameof(StartOfYearDaysSinceEpochData))]
    public void GetStartOfYear(YearDaysSinceEpoch info)
    {
        // Act
        var actual = SecondOrderSchemaUT.GetStartOfYear(info.Year);
        // Assert
        Assert.Equal(info.DaysSinceEpoch, actual);
    }
}
