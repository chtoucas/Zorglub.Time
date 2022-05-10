// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts;

using Zorglub.Testing.Data;

/// <summary>
/// Provides facts about <see cref="ICalendricalSchemaPlus"/>.
/// </summary>
public abstract partial class ICalendricalSchemaPlusFacts<TSchema, TDataSet> :
    ICalendricalSchemaFacts<TSchema, TDataSet>
    where TSchema : ICalendricalSchemaPlus
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected ICalendricalSchemaPlusFacts(TSchema schema) : base(schema) { }
}

public partial class ICalendricalSchemaPlusFacts<TSchema, TDataSet> // Methods
{
    #region CountDaysInYearAfterMonth()

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInYearAfterMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Arrange
        int daysInYearAfterMonth = SchemaUT.CountDaysInYear(y) - info.DaysInMonth - info.DaysInYearBeforeMonth;
        // Act
        int actual = SchemaUT.CountDaysInYearAfterMonth(y, m);
        // Assert
        Assert.Equal(daysInYearAfterMonth, actual);
    }

    #endregion

    #region CountDaysInYearBefore()

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountDaysInYearBefore﹍DateParts(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Arrange
        int daysInYearBefore = doy - 1;
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(y, m, d);
        // Assert
        Assert.Equal(daysInYearBefore, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInYearBefore﹍DateParts_DaysInYearBeforeMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;

        int daysInYearBeforeMonth = info.DaysInYearBeforeMonth;
        // Act
        int actual1 = SchemaUT.CountDaysInYearBefore(y, m, 1);
        int actual2 = SchemaUT.CountDaysInYearBefore(y, m, 2);
        int actual3 = SchemaUT.CountDaysInYearBefore(y, m, 3);
        int actual4 = SchemaUT.CountDaysInYearBefore(y, m, 4);
        int actual5 = SchemaUT.CountDaysInYearBefore(y, m, 5);
        // We can't go further because of the schemas w/ a virtual thirteen month.
        // Assert
        Assert.Equal(daysInYearBeforeMonth, actual1);
        Assert.Equal(daysInYearBeforeMonth + 1, actual2);
        Assert.Equal(daysInYearBeforeMonth + 2, actual3);
        Assert.Equal(daysInYearBeforeMonth + 3, actual4);
        Assert.Equal(daysInYearBeforeMonth + 4, actual5);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountDaysInYearBefore﹍OrdinalParts(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        // Arrange
        int daysInYearBefore = doy - 1;
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(y, doy);
        // Assert
        Assert.Equal(daysInYearBefore, actual);
    }

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void CountDaysInYearBefore﹍DaysSinceEpoch(DaysSinceEpochInfo info)
    {
        var (daysSinceEpoch, y, m, d) = info;
        // Arrange
        int daysInYearBefore = SchemaUT.GetDayOfYear(y, m, d) - 1;
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(daysSinceEpoch);
        // Assert
        Assert.Equal(daysInYearBefore, actual);
    }

    #endregion
    #region CountDaysInYearAfter()

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountDaysInYearAfter﹍DateParts(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Arrange
        int daysInYearAfter = SchemaUT.CountDaysInYear(y) - doy;
        // Act
        int actual = SchemaUT.CountDaysInYearAfter(y, m, d);
        // Assert
        Assert.Equal(daysInYearAfter, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountDaysInYearAfter﹍OrdinalParts(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        // Arrange
        int daysInYearAfter = SchemaUT.CountDaysInYear(y) - doy;
        // Act
        int actual = SchemaUT.CountDaysInYearAfter(y, doy);
        // Assert
        Assert.Equal(daysInYearAfter, actual);
    }

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void CountDaysInYearAfter﹍DaysSinceEpoch(DaysSinceEpochInfo info)
    {
        var (daysSinceEpoch, y, m, d) = info;
        // Arrange
        int daysInYearAfter = SchemaUT.CountDaysInYear(y) - SchemaUT.GetDayOfYear(y, m, d);
        // Act
        int actual = SchemaUT.CountDaysInYearAfter(daysSinceEpoch);
        // Assert
        Assert.Equal(daysInYearAfter, actual);
    }

    #endregion
    #region CountDaysInMonthBefore()

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountDaysInMonthBefore﹍DateParts(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Arrange
        int daysInMonthBefore = d - 1;
        // Act
        int actual = SchemaUT.CountDaysInMonthBefore(y, m, d);
        // Assert
        Assert.Equal(daysInMonthBefore, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountDaysInMonthBefore﹍OrdinalParts(DateInfo info)
    {
        var (y, _, d, doy) = info;
        // Arrange
        int daysInMonthBefore = d - 1;
        // Act
        int actual = SchemaUT.CountDaysInMonthBefore(y, doy);
        // Assert
        Assert.Equal(daysInMonthBefore, actual);
    }

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void CountDaysInMonthBefore﹍DaysSinceEpoch(DaysSinceEpochInfo info)
    {
        var (daysSinceEpoch, _, _, d) = info;
        // Arrange
        int daysInMonthBefore = d - 1;
        // Act
        int actual = SchemaUT.CountDaysInMonthBefore(daysSinceEpoch);
        // Assert
        Assert.Equal(daysInMonthBefore, actual);
    }

    #endregion
    #region CountDaysInMonthAfter()

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountDaysInMonthAfter﹍DateParts(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Arrange
        int daysInMonthAfter = SchemaUT.CountDaysInMonth(y, m) - d;
        // Act
        int actual = SchemaUT.CountDaysInMonthAfter(y, m, d);
        // Assert
        Assert.Equal(daysInMonthAfter, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInMonthAfter﹍DateParts_DaysInMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;

        int daysInMonth = info.DaysInMonth;
        // Act
        int actual1 = SchemaUT.CountDaysInMonthAfter(y, m, 1);
        int actual2 = SchemaUT.CountDaysInMonthAfter(y, m, 2);
        int actual3 = SchemaUT.CountDaysInMonthAfter(y, m, 3);
        int actual4 = SchemaUT.CountDaysInMonthAfter(y, m, 4);
        int actual5 = SchemaUT.CountDaysInMonthAfter(y, m, 5);
        // We can't go further because of the schemas w/ a virtual thirteen month.
        // Assert
        Assert.Equal(daysInMonth - 1, actual1);
        Assert.Equal(daysInMonth - 2, actual2);
        Assert.Equal(daysInMonth - 3, actual3);
        Assert.Equal(daysInMonth - 4, actual4);
        Assert.Equal(daysInMonth - 5, actual5);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountDaysInMonthAfter﹍OrdinalParts(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Arrange
        int daysInMonthAfter = SchemaUT.CountDaysInMonth(y, m) - d;
        // Act
        int actual = SchemaUT.CountDaysInMonthAfter(y, doy);
        // Assert
        Assert.Equal(daysInMonthAfter, actual);
    }

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void CountDaysInMonthAfter﹍DaysSinceEpoch(DaysSinceEpochInfo info)
    {
        var (daysSinceEpoch, y, m, d) = info;
        // Arrange
        int daysInMonthAfter = SchemaUT.CountDaysInMonth(y, m) - d;
        // Act
        int actual = SchemaUT.CountDaysInMonthAfter(daysSinceEpoch);
        // Assert
        Assert.Equal(daysInMonthAfter, actual);
    }

    #endregion
}

public partial class ICalendricalSchemaPlusFacts<TSchema, TDataSet> // Overflows
{
    [Fact] public void CountDaysInYearAfterMonth_DoesNotUnderflow() => _ = SchemaUT.CountDaysInYearAfterMonth(MinYear, 1);
    [Fact] public void CountDaysInYearAfterMonth_DoesNotOverflow() => _ = SchemaUT.CountDaysInYearAfterMonth(MaxYear, MaxMonth);

    [Fact] public void CountDaysInYearBefore﹍DateParts_DoesNotUnderflow() => _ = SchemaUT.CountDaysInYearBefore(MinYear, 1, 1);
    [Fact] public void CountDaysInYearBefore﹍DateParts_DoesNotOverflow() => _ = SchemaUT.CountDaysInYearBefore(MaxYear, 1, 1);
    [Fact] public void CountDaysInYearBefore﹍OrdinalParts_DoesNotUnderflow() => _ = SchemaUT.CountDaysInYearBefore(MinYear, 1);
    [Fact] public void CountDaysInYearBefore﹍OrdinalParts_DoesNotOverflow() => _ = SchemaUT.CountDaysInYearBefore(MaxYear, MaxDayOfYear);
    [Fact] public void CountDaysInYearBefore﹍DaysSinceEpoch_DoesNotUnderflow() => _ = SchemaUT.CountDaysInYearBefore(MinDaysSinceEpoch);
    [Fact] public void CountDaysInYearBefore﹍DaysSinceEpoch_DoesNotOverflow() => _ = SchemaUT.CountDaysInYearBefore(MaxDaysSinceEpoch);

    [Fact] public void CountDaysInYearAfter﹍DateParts_DoesNotUnderflow() => _ = SchemaUT.CountDaysInYearAfter(MinYear, 1, 1);
    [Fact] public void CountDaysInYearAfter﹍DateParts_DoesNotOverflow() => _ = SchemaUT.CountDaysInYearAfter(MaxYear, 1, 1);
    [Fact] public void CountDaysInYearAfter﹍OrdinalParts_DoesNotUnderflow() => _ = SchemaUT.CountDaysInYearAfter(MinYear, 1);
    [Fact] public void CountDaysInYearAfter﹍OrdinalParts_DoesNotOverflow() => _ = SchemaUT.CountDaysInYearAfter(MaxYear, MaxDayOfYear);
    [Fact] public void CountDaysInYearAfter﹍DaysSinceEpoch_DoesNotUnderflow() => _ = SchemaUT.CountDaysInYearAfter(MinDaysSinceEpoch);
    [Fact] public void CountDaysInYearAfter﹍DaysSinceEpoch_DoesNotOverflow() => _ = SchemaUT.CountDaysInYearAfter(MaxDaysSinceEpoch);

    [Fact] public void CountDaysInMonthBefore﹍DateParts_DoesNotUnderflow() => _ = SchemaUT.CountDaysInMonthBefore(MinYear, 1, 1);
    [Fact] public void CountDaysInMonthBefore﹍DateParts_DoesNotOverflow() => _ = SchemaUT.CountDaysInMonthBefore(MaxYear, 1, 1);
    [Fact] public void CountDaysInMonthBefore﹍OrdinalParts_DoesNotUnderflow() => _ = SchemaUT.CountDaysInMonthBefore(MinYear, 1);
    [Fact] public void CountDaysInMonthBefore﹍OrdinalParts_DoesNotOverflow() => _ = SchemaUT.CountDaysInMonthBefore(MaxYear, MaxDayOfYear);
    [Fact] public void CountDaysInMonthBefore﹍DaysSinceEpoch_DoesNotUnderflow() => _ = SchemaUT.CountDaysInMonthBefore(MinDaysSinceEpoch);
    [Fact] public void CountDaysInMonthBefore﹍DaysSinceEpoch_DoesNotOverflow() => _ = SchemaUT.CountDaysInMonthBefore(MaxDaysSinceEpoch);

    [Fact] public void CountDaysInMonthAfter﹍DateParts_DoesNotUnderflow() => _ = SchemaUT.CountDaysInMonthAfter(MinYear, 1, 1);
    [Fact] public void CountDaysInMonthAfter﹍DateParts_DoesNotOverflow() => _ = SchemaUT.CountDaysInMonthAfter(MaxYear, 1, 1);
    [Fact] public void CountDaysInMonthAfter﹍OrdinalParts_DoesNotUnderflow() => _ = SchemaUT.CountDaysInMonthAfter(MinYear, 1);
    [Fact] public void CountDaysInMonthAfter﹍OrdinalParts_DoesNotOverflow() => _ = SchemaUT.CountDaysInMonthAfter(MaxYear, MaxDayOfYear);
    [Fact] public void CountDaysInMonthAfter﹍DaysSinceEpoch_DoesNotUnderflow() => _ = SchemaUT.CountDaysInMonthAfter(MinDaysSinceEpoch);
    [Fact] public void CountDaysInMonthAfter﹍DaysSinceEpoch_DoesNotOverflow() => _ = SchemaUT.CountDaysInMonthAfter(MaxDaysSinceEpoch);
}
