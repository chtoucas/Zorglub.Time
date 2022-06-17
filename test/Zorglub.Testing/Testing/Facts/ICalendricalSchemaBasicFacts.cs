// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts;

using Zorglub.Testing.Data;

/// <summary>
/// Provides basic facts about <see cref="ICalendricalSchema"/>.
/// </summary>
public abstract class ICalendricalSchemaBasicFacts<TSchema, TDataSet> :
    ICalendricalKernelFacts<TSchema, TDataSet>
    where TSchema : ICalendricalSchema
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected ICalendricalSchemaBasicFacts(TSchema schema) : base(schema) { }

    protected bool TestGetYearAnyway { get; init; }
    protected bool TestGetMonthAnyway { get; init; }

    public override void Algorithm_Prop() { }
    public override void Family_Prop() { }
    public override void PeriodicAdjustments_Prop() { }
    public override void SupportedYears_Prop() { }

    public override void IsRegular() { }

    [Fact] public virtual void PreValidator_Prop() { }
    [Fact] public virtual void Arithmetic_Prop() { }

    // We only test the core virtual methods.

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInYearBeforeMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Act
        int actual = SchemaUT.CountDaysInYearBeforeMonth(y, m);
        // Assert
        Assert.Equal(info.DaysInYearBeforeMonth, actual);
    }

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void GetYear(DaysSinceEpochInfo info)
    {
        if (TestGetYearAnyway == false)
        {
            Assert.Throws<NotImplementedException>(() => SchemaUT.GetYear(info.DaysSinceEpoch, out _));
            return;
        }

        // Act
        int actual = SchemaUT.GetYear(info.DaysSinceEpoch, out _);
        // Assert
        Assert.Equal(info.Yemoda.Year, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetYear_DateParts(DateInfo info)
    {
        if (TestGetYearAnyway == false) { return; }

        var (y, m, d, doy) = info;
        int daysSinceEpoch = SchemaUT.CountDaysSinceEpoch(y, m, d);
        // Act
        int yA = SchemaUT.GetYear(daysSinceEpoch, out int doyA);
        // Assert
        Assert.Equal(y, yA);
        Assert.Equal(doy, doyA);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetMonth(DateInfo info)
    {
        var (y, m, d, doy) = info;
        if (TestGetMonthAnyway == false)
        {
            Assert.Throws<NotImplementedException>(() => SchemaUT.GetMonth(y, doy, out _));
            return;
        }

        // Act
        int mA = SchemaUT.GetMonth(y, doy, out int dA);
        // Assert
        Assert.Equal(m, mA);
        Assert.Equal(d, dA);
    }

    [Theory, MemberData(nameof(StartOfYearDaysSinceEpochData))]
    public void GetStartOfYear(YearDaysSinceEpoch info)
    {
        // Act
        var actual = SchemaUT.GetStartOfYear(info.Year);
        // Assert
        Assert.Equal(info.DaysSinceEpoch, actual);
    }
}
