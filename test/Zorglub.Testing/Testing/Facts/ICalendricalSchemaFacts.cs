// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts;

using Zorglub.Testing.Data;
using Zorglub.Time.Core.Intervals;

/// <summary>
/// Provides facts about <see cref="ICalendricalSchema"/>.
/// </summary>
public abstract partial class ICalendricalSchemaFacts<TSchema, TDataSet> :
    ICalendricalKernelFacts<TSchema, TDataSet>
    where TSchema : ICalendricalSchema
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected ICalendricalSchemaFacts(TSchema schema) : base(schema)
    {
        MinDaysSinceEpoch = schema.GetStartOfYear(MinYear);
        MaxDaysSinceEpoch = schema.GetEndOfYear(MaxYear);

        MinMonthsSinceEpoch = schema.GetStartOfYearInMonths(MinYear);
        MaxMonthsSinceEpoch = schema.GetEndOfYearInMonths(MaxYear);
    }

    protected int MinDaysSinceEpoch { get; }
    protected int MaxDaysSinceEpoch { get; }

    protected int MinMonthsSinceEpoch { get; }
    protected int MaxMonthsSinceEpoch { get; }

    [Fact] public abstract void PreValidator_Prop();

    protected void VerifyThatPreValidatorIs<T>() => Assert.IsType<T>(SchemaUT.PreValidator);
}

public partial class ICalendricalSchemaFacts<TSchema, TDataSet> // Properties
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void MinDaysInYear_Prop_IsLessThanOrEqualTo_DaysInYear(YearInfo info) =>
        Assert.True(SchemaUT.MinDaysInYear <= info.DaysInYear);

    [Theory, MemberData(nameof(MonthInfoData))]
    public void MinDaysInMonth_Prop_IsLessThanOrEqualTo_DaysInMonth(MonthInfo info) =>
        Assert.True(SchemaUT.MinDaysInMonth <= info.DaysInMonth);

    [Fact]
    public void Domain_Prop()
    {
        var domain = new Range<int>(MinDaysSinceEpoch, MaxDaysSinceEpoch);
        // Act & Assert
        Assert.Equal(domain, SchemaUT.Domain);
        // Lazy prop: we duplicate the test to ensure full test coverage.
        Assert.Equal(domain, SchemaUT.Domain);
    }

    [Fact]
    public void MonthDomain_Prop()
    {
        var domain = new Range<int>(MinMonthsSinceEpoch, MaxMonthsSinceEpoch);
        // Act & Assert
        Assert.Equal(domain, SchemaUT.MonthDomain);
        // Lazy prop: we duplicate the test to ensure full test coverage.
        Assert.Equal(domain, SchemaUT.MonthDomain);
    }
}

public partial class ICalendricalSchemaFacts<TSchema, TDataSet> // Methods
{
    #region CountDaysInYearBeforeMonth()

    [RedundantTest]
    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYearBeforeMonth_AtStartOfYear(YearInfo info)
    {
        // Act
        int actual = SchemaUT.CountDaysInYearBeforeMonth(info.Year, 1);
        // Assert
        Assert.Equal(0, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInYearBeforeMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Act
        int actual = SchemaUT.CountDaysInYearBeforeMonth(y, m);
        // Assert
        Assert.Equal(info.DaysInYearBeforeMonth, actual);
    }

    #endregion

    #region CountMonthsSinceEpoch()

    [Fact]
    public void CountMonthsSinceEpoch_AtEpoch_IsZero()
    {
        // Act
        int actual = SchemaUT.CountMonthsSinceEpoch(1, 1);
        // Assert
        Assert.Equal(0, actual);
    }

    [Theory, MemberData(nameof(MonthsSinceEpochInfoData))]
    public void CountMonthsSinceEpoch(MonthsSinceEpochInfo info)
    {
        var (monthsSinceEpoch, y, m) = info;
        // Act
        int actual = SchemaUT.CountMonthsSinceEpoch(y, m);
        // Assert
        Assert.Equal(monthsSinceEpoch, actual);
    }

    #endregion
    #region CountDaysSinceEpoch﹍DateParts()

    [Fact]
    public void CountDaysSinceEpoch﹍DateParts_AtEpoch_IsZero()
    {
        // Act
        int actual = SchemaUT.CountDaysSinceEpoch(1, 1, 1);
        // Assert
        Assert.Equal(0, actual);
    }

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void CountDaysSinceEpoch﹍DateParts(DaysSinceEpochInfo info)
    {
        var (daysSinceEpoch, y, m, d) = info;
        // Act
        int actual = SchemaUT.CountDaysSinceEpoch(y, m, d);
        // Assert
        Assert.Equal(daysSinceEpoch, actual);
    }

    #endregion
    #region CountDaysSinceEpoch﹍OrdinalParts()

    [Fact]
    public void CountDaysSinceEpoch﹍OrdinalParts_AtEpoch_IsZero()
    {
        // Act
        int actual = SchemaUT.CountDaysSinceEpoch(1, 1);
        // Assert
        Assert.Equal(0, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountDaysSinceEpoch﹍OrdinalParts(DateInfo info)
    {
        var (y, m, d, doy) = info;
        int daysSinceEpoch = SchemaUT.CountDaysSinceEpoch(y, m, d);
        // Act
        int actual = SchemaUT.CountDaysSinceEpoch(y, doy);
        // Assert
        Assert.Equal(daysSinceEpoch, actual);
    }

    #endregion
    #region GetMonthParts()

    [Theory, MemberData(nameof(MonthsSinceEpochInfoData))]
    public void GetMonthParts(MonthsSinceEpochInfo info)
    {
        var (monthsSinceEpoch, y, m) = info;
        // Act
        SchemaUT.GetMonthParts(monthsSinceEpoch, out int yA, out int mA);
        // Assert
        Assert.Equal(y, yA);
        Assert.Equal(m, mA);
    }

    #endregion
    #region GetDateParts()

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void GetDateParts(DaysSinceEpochInfo info)
    {
        var (daysSinceEpoch, y, m, d) = info;
        // Act
        SchemaUT.GetDateParts(daysSinceEpoch, out int yA, out int mA, out int dA);
        // Assert
        Assert.Equal(y, yA);
        Assert.Equal(m, mA);
        Assert.Equal(d, dA);
    }

    #endregion
    #region GetYear()

    [RedundantTest]
    [Theory, MemberData(nameof(DateInfoData))]
    public void GetYear(DateInfo info)
    {
        var (y, m, d, doy) = info;
        int daysSinceEpoch = SchemaUT.CountDaysSinceEpoch(y, m, d);
        // Act
        int yA = SchemaUT.GetYear(daysSinceEpoch, out int doyA);
        // Assert
        Assert.Equal(y, yA);
        Assert.Equal(doy, doyA);
    }

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void GetYear_DaysSinceEpoch(DaysSinceEpochInfo info)
    {
        // Act
        int yA = SchemaUT.GetYear(info.DaysSinceEpoch, out _);
        // Assert
        Assert.Equal(info.Yemoda.Year, yA);
    }

    #endregion
    #region GetMonth()

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetMonth(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Act
        int mA = SchemaUT.GetMonth(y, doy, out int dA);
        // Assert
        Assert.Equal(m, mA);
        Assert.Equal(d, dA);
    }

    #endregion
    #region GetDayOfYear()

    [RedundantTest]
    [Theory, MemberData(nameof(YearInfoData))]
    public void GetDayOfYear_AtStartOfYear(YearInfo info)
    {
        // Act
        int actual = SchemaUT.GetDayOfYear(info.Year, 1, 1);
        // Assert
        Assert.Equal(1, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetDayOfYear(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Act
        int actual = SchemaUT.GetDayOfYear(y, m, d);
        // Assert
        Assert.Equal(doy, actual);
    }

    #endregion

    #region GetStartOfYearInMonths()

    [Fact]
    public void GetStartOfYearInMonths_AtYear1_IsZero()
    {
        // Act
        int actual = SchemaUT.GetStartOfYearInMonths(1);
        // Assert
        Assert.Equal(0, actual);
    }

    [Theory, MemberData(nameof(StartOfYearMonthsSinceEpochData))]
    public void GetStartOfYearInMonths(YearMonthsSinceEpoch info)
    {
        // Act
        var actual = SchemaUT.GetStartOfYearInMonths(info.Year);
        // Assert
        Assert.Equal(info.MonthsSinceEpoch, actual);
    }

    #endregion
    #region GetEndOfYearInMonths()

    [Theory, MemberData(nameof(EndOfYearMonthsSinceEpochData))]
    public void GetEndOfYearInMonths(YearMonthsSinceEpoch info)
    {
        // Act
        var actual = SchemaUT.GetEndOfYearInMonths(info.Year);
        // Assert
        Assert.Equal(info.MonthsSinceEpoch, actual);
    }

    #endregion
    #region GetStartOfYear()

    [Fact]
    public void GetStartOfYear_AtYear1_IsZero()
    {
        // Act
        int actual = SchemaUT.GetStartOfYear(1);
        // Assert
        Assert.Equal(0, actual);
    }

    [Theory, MemberData(nameof(StartOfYearDaysSinceEpochData))]
    public void GetStartOfYear(YearDaysSinceEpoch info)
    {
        // Act
        var actual = SchemaUT.GetStartOfYear(info.Year);
        // Assert
        Assert.Equal(info.DaysSinceEpoch, actual);
    }

    #endregion
    #region GetEndOfYear()

    [Theory, MemberData(nameof(EndOfYearDaysSinceEpochData))]
    public void GetEndOfYear(YearDaysSinceEpoch info)
    {
        // Act
        var actual = SchemaUT.GetEndOfYear(info.Year);
        // Assert
        Assert.Equal(info.DaysSinceEpoch, actual);
    }

    // When we don't have any test data, we check that the method returns a
    // result in agreement with what we know.

    [RedundantTest]
    [Theory, MemberData(nameof(EndOfYearPartsData))]
    public void GetEndOfYear_CountDaysSinceEpoch(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        int endOfYear = SchemaUT.CountDaysSinceEpoch(y, m, d);
        // Act
        int actual = SchemaUT.GetEndOfYear(y);
        // Assert
        Assert.Equal(endOfYear, actual);
    }

    [RedundantTest]
    [Theory, MemberData(nameof(YearInfoData))]
    public void GetEndOfYear_EndOfYear_Equals_StartOfNextYearMinusOne(YearInfo info)
    {
        int y = info.Year;
        if (y >= MaxYear) { return; }
        int startOfNextYearMinusOne = SchemaUT.GetStartOfYear(y + 1) - 1;
        // Act
        int endOfYear = SchemaUT.GetEndOfYear(y);
        // Assert
        Assert.Equal(startOfNextYearMinusOne, endOfYear);
    }

    #endregion
    #region GetStartOfMonth()

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int startOfMonth = SchemaUT.GetStartOfYear(y) + info.DaysInYearBeforeMonth;
        // Act
        int actual = SchemaUT.GetStartOfMonth(y, m);
        // Assert
        Assert.Equal(startOfMonth, actual);
    }

    [Fact]
    public void GetStartOfMonth_AtFirstMonthOfYear1_IsZero()
    {
        // Act
        int actual = SchemaUT.GetStartOfMonth(1, 1);
        // Assert
        Assert.Equal(0, actual);
    }

    #endregion
    #region GetEndOfMonth()

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int endOfMonth = SchemaUT.GetStartOfYear(y) + info.DaysInYearBeforeMonth + info.DaysInMonth - 1;
        // Act
        int actual = SchemaUT.GetEndOfMonth(y, m);
        // Assert
        Assert.Equal(endOfMonth, actual);
    }

    [RedundantTest]
    [Theory, MemberData(nameof(YearInfoData))]
    public void GetEndOfMonth_EndOfMonth_Equals_StartOfNextMonthMinusOne(YearInfo info)
    {
        int y = info.Year;
        int monthsInYear = SchemaUT.CountMonthsInYear(y);
        for (int m = 1; m < monthsInYear; m++)
        {
            int startOfNextMonthMinusOne = SchemaUT.CountDaysSinceEpoch(y, m + 1, 1) - 1;
            // Act
            int endOfMonth = SchemaUT.GetEndOfMonth(y, m);
            // Assert
            Assert.Equal(startOfNextMonthMinusOne, endOfMonth);
        }
    }

    #endregion
}

public partial class ICalendricalSchemaFacts<TSchema, TDataSet> // Overflows
{
    [Fact] public void CountDaysInYearBeforeMonth_DoesNotUnderflow() => _ = SchemaUT.CountDaysInYearBeforeMonth(MinYear, 1);
    [Fact] public void CountDaysInYearBeforeMonth_DoesNotOverflow() => _ = SchemaUT.CountDaysInYearBeforeMonth(MaxYear, MaxMonth);

    [Fact] public void CountDaysSinceEpoch﹍DateParts_DoesNotUnderflow() => _ = SchemaUT.CountDaysSinceEpoch(MinYear, 1, 1);
    [Fact] public void CountDaysSinceEpoch﹍DateParts_DoesNotOverflow() => _ = SchemaUT.CountDaysSinceEpoch(MaxYear, MaxMonth, MaxDay);

    [Fact] public void CountDaysSinceEpoch﹍OrdinalParts_DoesNotUnderflow() => _ = SchemaUT.CountDaysSinceEpoch(MinYear, 1);
    [Fact] public void CountDaysSinceEpoch﹍OrdinalParts_DoesNotOverflow() => _ = SchemaUT.CountDaysSinceEpoch(MaxYear, MaxDayOfYear);

    [Fact] public void GetDateParts_DoesNotUnderflow() => SchemaUT.GetDateParts(MinDaysSinceEpoch, out _, out _, out _);
    [Fact] public void GetDateParts_DoesNotOverflow() => SchemaUT.GetDateParts(MaxDaysSinceEpoch, out _, out _, out _);

    [Fact] public void GetYear_DoesNotUnderflow() => _ = SchemaUT.GetYear(MinDaysSinceEpoch, out _);
    [Fact] public void GetYear_DoesNotOverflow() => _ = SchemaUT.GetYear(MaxDaysSinceEpoch, out _);

    [Fact] public void GetMonth_DoesNotUnderflow() => _ = SchemaUT.GetMonth(MinYear, 1, out _);
    [Fact] public void GetMonth_DoesNotOverflow() => _ = SchemaUT.GetMonth(MaxYear, MaxDayOfYear, out _);

    [Fact] public void GetDayOfYear_DoesNotUnderflow() => _ = SchemaUT.GetDayOfYear(MinYear, 1, 1);
    [Fact] public void GetDayOfYear_DoesNotOverflow() => _ = SchemaUT.GetDayOfYear(MaxYear, MaxMonth, MaxDay);

    [Fact] public void GetStartOfYearInMonths_DoesNotUnderflow() => _ = SchemaUT.GetStartOfYearInMonths(MinYear);
    [Fact] public void GetStartOfYearInMonths_DoesNotOverflow() => _ = SchemaUT.GetStartOfYearInMonths(MaxYear);

    [Fact] public void GetEndOfYearInMonths_DoesNotUnderflow() => _ = SchemaUT.GetEndOfYearInMonths(MinYear);
    [Fact] public void GetEndOfYearInMonths_DoesNotOverflow() => _ = SchemaUT.GetEndOfYearInMonths(MaxYear);

    [Fact] public void GetStartOfYear_DoesNotUnderflow() => _ = SchemaUT.GetStartOfYear(MinYear);
    [Fact] public void GetStartOfYear_DoesNotOverflow() => _ = SchemaUT.GetStartOfYear(MaxYear);

    [Fact] public void GetEndOfYear_DoesNotUnderflow() => _ = SchemaUT.GetEndOfYear(MinYear);
    [Fact] public void GetEndOfYear_DoesNotOverflow() => _ = SchemaUT.GetEndOfYear(MaxYear);

    [Fact] public void GetStartOfMonth_DoesNotUnderflow() => _ = SchemaUT.GetStartOfMonth(MinYear, 1);
    [Fact] public void GetStartOfMonth_DoesNotOverflow() => _ = SchemaUT.GetStartOfMonth(MaxYear, MaxMonth);

    [Fact] public void GetEndOfMonth_DoesNotUnderflow() => _ = SchemaUT.GetEndOfMonth(MinYear, 1);
    [Fact] public void GetEndOfMonth_DoesNotOverflow() => _ = SchemaUT.GetEndOfMonth(MaxYear, MaxMonth);
}