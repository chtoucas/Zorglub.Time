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

    [Theory, MemberData(nameof(DaysInYearAfterMonthData))]
    public void CountDaysInYearAfterMonth(YemoAnd<int> info)
    {
        var (y, m) = info.Yemo;
        int daysInYearAfterMonth = info.Value;
        // Act
        int actual = SchemaUT.CountDaysInYearAfterMonth(y, m);
        // Assert
        Assert.Equal(daysInYearAfterMonth, actual);
    }

    #endregion

    #region CountDaysInYearBefore()

    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYearBefore﹍DateParts_AtStartOfYear(YearInfo info)
    {
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(info.Year, 1, 1);
        // Assert
        Assert.Equal(0, actual);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYearBefore﹍OrdinalParts_AtStartOfYear(YearInfo info)
    {
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(info.Year, 1);
        // Assert
        Assert.Equal(0, actual);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYearBefore﹍DaysSinceEpoch_AtStartOfYear(YearInfo info)
    {
        int daysSinceEpoch = SchemaUT.GetStartOfYear(info.Year);
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(daysSinceEpoch);
        // Assert
        Assert.Equal(0, actual);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYearBefore﹍OrdinalParts_AtEndOfYear(YearInfo info)
    {
        int doy = info.DaysInYear;
        int daysInYearBefore = doy - 1;
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(info.Year, doy);
        // Assert
        Assert.Equal(daysInYearBefore, actual);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYearBefore﹍DaysSinceEpoch_AtEndOfYear(YearInfo info)
    {
        int daysSinceEpoch = SchemaUT.GetEndOfYear(info.Year);
        int daysInYearBefore = info.DaysInYear - 1;
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(daysSinceEpoch);
        // Assert
        Assert.Equal(daysInYearBefore, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInYearBefore﹍DateParts_AtStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int daysInYearBefore = info.DaysInYearBeforeMonth;
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(y, m, 1);
        // Assert
        Assert.Equal(daysInYearBefore, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInYearBefore﹍DaysSinceEpoch_AtStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int daysSinceEpoch = SchemaUT.GetStartOfMonth(y, m);
        int daysInYearBefore = info.DaysInYearBeforeMonth;
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(daysSinceEpoch);
        // Assert
        Assert.Equal(daysInYearBefore, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInYearBefore﹍DateParts_AtEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int d = info.DaysInMonth;
        int daysInYearBefore = info.DaysInYearBeforeMonth + d - 1;
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(y, m, d);
        // Assert
        Assert.Equal(daysInYearBefore, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInYearBefore﹍DaysSinceEpoch_AtEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int daysSinceEpoch = SchemaUT.GetEndOfMonth(y, m);
        int daysInYearBefore = info.DaysInYearBeforeMonth + info.DaysInMonth - 1;
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(daysSinceEpoch);
        // Assert
        Assert.Equal(daysInYearBefore, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountDaysInYearBefore﹍DateParts(DateInfo info)
    {
        var (y, m, d, doy) = info;
        int daysInYearBefore = doy - 1;
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(y, m, d);
        // Assert
        Assert.Equal(daysInYearBefore, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountDaysInYearBefore﹍OrdinalParts(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
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
        int daysInYearBefore = SchemaUT.GetDayOfYear(y, m, d) - 1;
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(daysSinceEpoch);
        // Assert
        Assert.Equal(daysInYearBefore, actual);
    }

    #endregion
    #region CountDaysInYearAfter()

    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYearAfter﹍DateParts_AtStartOfYear(YearInfo info)
    {
        int daysInYearAfter = info.DaysInYear - 1;
        // Act
        int actual = SchemaUT.CountDaysInYearAfter(info.Year, 1, 1);
        // Assert
        Assert.Equal(daysInYearAfter, actual);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYearAfter﹍OrdinalParts_AtStartOfYear(YearInfo info)
    {
        int daysInYearAfter = info.DaysInYear - 1;
        // Act
        int actual = SchemaUT.CountDaysInYearAfter(info.Year, 1);
        // Assert
        Assert.Equal(daysInYearAfter, actual);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYearAfter﹍DaysSinceEpoch_AtStartOfYear(YearInfo info)
    {
        int daysInYearAfter = info.DaysInYear - 1;
        int daysSinceEpoch = SchemaUT.GetStartOfYear(info.Year);
        // Act
        int actual = SchemaUT.CountDaysInYearAfter(daysSinceEpoch);
        // Assert
        Assert.Equal(daysInYearAfter, actual);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYearAfter﹍OrdinalParts_AtEndOfYear(YearInfo info)
    {
        // Act
        int actual = SchemaUT.CountDaysInYearAfter(info.Year, info.DaysInYear);
        // Assert
        Assert.Equal(0, actual);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYearAfter﹍DaysSinceEpoch_AtEndOfYear(YearInfo info)
    {
        int daysSinceEpoch = SchemaUT.GetEndOfYear(info.Year);
        // Act
        int actual = SchemaUT.CountDaysInYearAfter(daysSinceEpoch);
        // Assert
        Assert.Equal(0, actual);
    }

    [Theory, MemberData(nameof(DaysInYearAfterDateData))]
    public void CountDaysInYearAfter﹍DateParts(YemodaAnd<int> info)
    {
        var (y, m, d, daysInYearAfter) = info;
        // Act
        int actual = SchemaUT.CountDaysInYearAfter(y, m, d);
        // Assert
        Assert.Equal(daysInYearAfter, actual);
    }

    [Theory, MemberData(nameof(DaysInYearAfterDateData))]
    public void CountDaysInYearAfter﹍OrdinalParts(YemodaAnd<int> info)
    {
        var (y, m, d, daysInYearAfter) = info;
        int doy = SchemaUT.GetDayOfYear(y, m, d);
        // Act
        int actual = SchemaUT.CountDaysInYearAfter(y, doy);
        // Assert
        Assert.Equal(daysInYearAfter, actual);
    }

    [Theory, MemberData(nameof(DaysInYearAfterDateData))]
    public void CountDaysInYearAfter﹍DaysSinceEpoch(YemodaAnd<int> info)
    {
        var (y, m, d, daysInYearAfter) = info;
        int daysSinceEpoch = SchemaUT.CountDaysSinceEpoch(y, m, d);
        // Act
        int actual = SchemaUT.CountDaysInYearAfter(daysSinceEpoch);
        // Assert
        Assert.Equal(daysInYearAfter, actual);
    }

    #endregion
    #region CountDaysInMonthBefore()

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInMonthBefore﹍DateParts_AtStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Act
        int actual = SchemaUT.CountDaysInMonthBefore(y, m, 1);
        // Assert
        Assert.Equal(0, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInMonthBefore﹍DaysSinceEpoch_AtStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int daysSinceEpoch = SchemaUT.GetStartOfMonth(y, m);
        // Act
        int actual = SchemaUT.CountDaysInMonthBefore(daysSinceEpoch);
        // Assert
        Assert.Equal(0, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInMonthBefore﹍DateParts_AtEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int d = info.DaysInMonth;
        int daysInMonthBefore = d - 1;
        // Act
        int actual = SchemaUT.CountDaysInMonthBefore(y, m, d);
        // Assert
        Assert.Equal(daysInMonthBefore, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInMonthBefore﹍DaysSinceEpoch_AtEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int daysSinceEpoch = SchemaUT.GetEndOfMonth(y, m);
        int daysInMonthBefore = info.DaysInMonth - 1;
        // Act
        int actual = SchemaUT.CountDaysInMonthBefore(daysSinceEpoch);
        // Assert
        Assert.Equal(daysInMonthBefore, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountDaysInMonthBefore﹍DateParts(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
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
        int daysInMonthBefore = d - 1;
        // Act
        int actual = SchemaUT.CountDaysInMonthBefore(daysSinceEpoch);
        // Assert
        Assert.Equal(daysInMonthBefore, actual);
    }

    #endregion
    #region CountDaysInMonthAfter()

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInMonthAfter﹍DateParts_AtStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int daysInMonthAfter = info.DaysInMonth - 1;
        // Act
        int actual = SchemaUT.CountDaysInMonthAfter(y, m, 1);
        // Assert
        Assert.Equal(daysInMonthAfter, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInMonthAfter﹍DaysSinceEpoch_AtStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int daysSinceEpoch = SchemaUT.GetStartOfMonth(y, m);
        int daysInMonthAfter = info.DaysInMonth - 1;
        // Act
        int actual = SchemaUT.CountDaysInMonthAfter(daysSinceEpoch);
        // Assert
        Assert.Equal(daysInMonthAfter, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInMonthAfter﹍DateParts_AtEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int d = info.DaysInMonth;
        // Act
        int actual = SchemaUT.CountDaysInMonthAfter(y, m, d);
        // Assert
        Assert.Equal(0, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInMonthAfter﹍DaysSinceEpoch_AtEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int daysSinceEpoch = SchemaUT.GetEndOfMonth(y, m);
        // Act
        int actual = SchemaUT.CountDaysInMonthAfter(daysSinceEpoch);
        // Assert
        Assert.Equal(0, actual);
    }

    [Theory, MemberData(nameof(DaysInMonthAfterDateData))]
    public void CountDaysInMonthAfter﹍DateParts(YemodaAnd<int> info)
    {
        var (y, m, d, daysInMonthAfter) = info;
        // Act
        int actual = SchemaUT.CountDaysInMonthAfter(y, m, d);
        // Assert
        Assert.Equal(daysInMonthAfter, actual);
    }

    [Theory, MemberData(nameof(DaysInMonthAfterDateData))]
    public void CountDaysInMonthAfter﹍OrdinalParts(YemodaAnd<int> info)
    {
        var (y, m, d, daysInMonthAfter) = info;
        int doy = SchemaUT.GetDayOfYear(y, m, d);
        // Act
        int actual = SchemaUT.CountDaysInMonthAfter(y, doy);
        // Assert
        Assert.Equal(daysInMonthAfter, actual);
    }

    [Theory, MemberData(nameof(DaysInMonthAfterDateData))]
    public void CountDaysInMonthAfter﹍DaysSinceEpoch(YemodaAnd<int> info)
    {
        var (y, m, d, daysInMonthAfter) = info;
        int daysSinceEpoch = SchemaUT.CountDaysSinceEpoch(y, m, d);
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
