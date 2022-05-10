// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Core;

using Zorglub.Testing.Data;
using Zorglub.Time.Core.Intervals;

/// <summary>
/// Provides facts about <see cref="SystemSchema"/>.
/// </summary>
public abstract partial class SystemSchemaFacts<TDataSet> :
    CalendricalSchemaFacts<SystemSchema, TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected SystemSchemaFacts(SystemSchema schema) : base(schema) { }

    // This property is actually part of CalendricalSchema but being internal
    // it's not publicly testable.
    [Fact] public abstract void Profile_Prop();

    [Fact] public abstract void TryGetCustomArithmetic();

    protected void VerifyThatTryGetCustomArithmeticSucceeds<T>()
    {
        Assert.True(SchemaUT.TryGetCustomArithmetic(out ICalendricalArithmetic? arithmetic));
        Assert.IsType<T>(arithmetic);
    }

    protected void VerifyThatTryGetCustomArithmeticFails()
    {
        Assert.False(SchemaUT.TryGetCustomArithmetic(out ICalendricalArithmetic? arithmetic));
        Assert.Null(arithmetic);
    }
}

public partial class SystemSchemaFacts<TDataSet> // Properties
{
    [Fact]
    public override void SupportedYears_Prop() =>
        Assert.Equal(SystemSchema.DefaultSupportedYears, SchemaUT.SupportedYears);

    [Fact]
    public virtual void SupportedYearsCore_Prop() =>
        Assert.Equal(Range.Maximal32, SchemaUT.SupportedYearsCore);

    [Fact]
    public void MinMaxDateParts_Prop()
    {
        var startOfYear = SchemaUT.GetStartOfYearParts(MinYear);
        var endOfYear = SchemaUT.GetEndOfYearParts(MaxYear);
        var minmax = OrderedPair.Create(startOfYear, endOfYear);
        // Act & Assert
        Assert.Equal(minmax, SchemaUT.MinMaxDateParts);
        // Lazy prop: we duplicate the test to ensure full test coverage.
        Assert.Equal(minmax, SchemaUT.MinMaxDateParts);
    }

    [Fact]
    public void MinMaxOrdinalParts_Prop()
    {
        var startOfYear = SchemaUT.GetStartOfYearOrdinalParts(MinYear);
        var endOfYear = SchemaUT.GetEndOfYearOrdinalParts(MaxYear);
        var minmax = OrderedPair.Create(startOfYear, endOfYear);
        // Act & Assert
        Assert.Equal(minmax, SchemaUT.MinMaxOrdinalParts);
        // Lazy prop: we duplicate the test to ensure full test coverage.
        Assert.Equal(minmax, SchemaUT.MinMaxOrdinalParts);
    }
}

public partial class SystemSchemaFacts<TDataSet> // Methods
{
    #region GetYear()

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetYear﹍Plain(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        int daysSinceEpoch = SchemaUT.CountDaysSinceEpoch(y, m, d);
        // Act
        int yA = SchemaUT.GetYear(daysSinceEpoch);
        // Assert
        Assert.Equal(y, yA);
    }

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void GetYear﹍Plain_DaysSinceEpoch(DaysSinceEpochInfo info)
    {
        // Act
        int yA = SchemaUT.GetYear(info.DaysSinceEpoch);
        // Assert
        Assert.Equal(info.Year, yA);
    }

    #endregion

    #region GetDateParts﹍DaysSinceEpoch()

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void GetDateParts﹍DaysSinceEpoch(DaysSinceEpochInfo info)
    {
        // Act
        var actual = SchemaUT.GetDateParts(info.DaysSinceEpoch);
        // Assert
        Assert.Equal(info.Yemoda, actual);
    }

    #endregion
    #region GetDateParts﹍OrdinalParts()

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetDateParts﹍OrdinalParts(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        // Act
        var actual = SchemaUT.GetDateParts(y, doy);
        // Assert
        Assert.Equal(info.Yemoda, actual);
    }

    #endregion

    #region GetOrdinalParts﹍DaysSinceEpoch()

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void GetOrdinalParts﹍DaysSinceEpoch(DaysSinceEpochInfo info)
    {
        var (daysSinceEpoch, y, m, d) = info;
        int doy = SchemaUT.GetDayOfYear(y, m, d);
        var parts = new Yedoy(y, doy);
        // Act
        var actual = SchemaUT.GetOrdinalParts(daysSinceEpoch);
        // Assert
        Assert.Equal(parts, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetOrdinalParts﹍DaysSinceEpoch_DateParts(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        int daysSinceEpoch = SchemaUT.CountDaysSinceEpoch(y, m, d);
        // Act
        var actual = SchemaUT.GetOrdinalParts(daysSinceEpoch);
        // Assert
        Assert.Equal(info.Yedoy, actual);
    }

    #endregion
    #region GetOrdinalParts﹍DateParts()

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetOrdinalParts﹍DateParts(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        var actual = SchemaUT.GetOrdinalParts(y, m, d);
        // Assert
        Assert.Equal(info.Yedoy, actual);
    }

    #endregion

    #region GetStartOfYearParts()

    [Fact]
    public void GetStartOfYearParts_AtYear1()
    {
        var ymd = new Yemoda(1, 1, 1);
        // Act
        var actual = SchemaUT.GetStartOfYearParts(1);
        // Assert
        Assert.Equal(ymd, actual);
    }

    [Theory, MemberData(nameof(StartOfYearPartsData))]
    public void GetStartOfYearParts(Yemoda ymd)
    {
        // Act
        var actual = SchemaUT.GetStartOfYearParts(ymd.Year);
        // Assert
        Assert.Equal(ymd, actual);
    }

    #endregion
    #region GetStartOfYearOrdinalParts()

    [Fact]
    public void GetStartOfYearOrdinalParts_AtYear1()
    {
        var ydoy = new Yedoy(1, 1);
        // Act
        var actual = SchemaUT.GetStartOfYearOrdinalParts(1);
        // Assert
        Assert.Equal(ydoy, actual);
    }

    [Theory, MemberData(nameof(StartOfYearDaysSinceEpochData))]
    public void GetStartOfYearOrdinalParts(YearDaysSinceEpoch info)
    {
        int y = info.Year;
        var ydoy = new Yedoy(y, 1);
        // Act
        var actual = SchemaUT.GetStartOfYearOrdinalParts(y);
        // Assert
        Assert.Equal(ydoy, actual);
    }

    #endregion

    #region GetEndOfYearParts()

    [Theory, MemberData(nameof(EndOfYearPartsData))]
    public void GetEndOfYearParts(Yemoda ymd)
    {
        // Act
        var actual = SchemaUT.GetEndOfYearParts(ymd.Year);
        // Assert
        Assert.Equal(ymd, actual);
    }

    #endregion
    #region GetEndOfYearOrdinalParts()

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetEndOfYearOrdinalParts(YearInfo info)
    {
        int y = info.Year;
        var ydoy = new Yedoy(y, info.DaysInYear);
        // Act
        var actual = SchemaUT.GetEndOfYearOrdinalParts(y);
        // Assert
        Assert.Equal(ydoy, actual);
    }

    #endregion

    #region GetStartOfMonthParts()

    [Fact]
    public void GetStartOfMonthParts_AtFirstMonthOfYear1()
    {
        var ymd = new Yemoda(1, 1, 1);
        // Act
        var actual = SchemaUT.GetStartOfMonthParts(1, 1);
        // Assert
        Assert.Equal(ymd, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetStartOfMonthParts(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var ymd = new Yemoda(y, m, 1);
        // Act
        var actual = SchemaUT.GetStartOfMonthParts(y, m);
        // Assert
        Assert.Equal(ymd, actual);
    }

    #endregion
    #region GetStartOfMonthOrdinalParts()

    [Fact]
    public void GetStartOfMonthOrdinalParts_AtFirstMonthOfYear1()
    {
        var ydoy = new Yedoy(1, 1);
        // Act
        var actual = SchemaUT.GetStartOfMonthOrdinalParts(1, 1);
        // Assert
        Assert.Equal(ydoy, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetStartOfMonthOrdinalParts(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var ydoy = new Yedoy(y, info.DaysInYearBeforeMonth + 1);
        // Act
        var actual = SchemaUT.GetStartOfMonthOrdinalParts(y, m);
        // Assert
        Assert.Equal(ydoy, actual);
    }

    #endregion

    #region GetEndOfMonthParts()

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetEndOfMonthParts(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var ymd = new Yemoda(y, m, info.DaysInMonth);
        // Act
        var actual = SchemaUT.GetEndOfMonthParts(y, m);
        // Assert
        Assert.Equal(ymd, actual);
    }

    #endregion
    #region GetEndOfMonthOrdinalParts()

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetEndOfMonthOrdinalParts(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var ydoy = new Yedoy(y, info.DaysInYearBeforeMonth + info.DaysInMonth);
        // Act
        var actual = SchemaUT.GetEndOfMonthOrdinalParts(y, m);
        // Assert
        Assert.Equal(ydoy, actual);
    }

    #endregion
}

public partial class SystemSchemaFacts<TDataSet> // Overflows
{
    [Fact]
    public sealed override void KernelDoesNotOverflow()
    {
        var (minYearCore, maxYearCore) = SchemaUT.SupportedYearsCore.Endpoints;

        _ = SchemaUT.IsLeapYear(minYearCore);
        _ = SchemaUT.IsLeapYear(maxYearCore);

        // NB: right now, it works w/ Int32.Min(Max)Year but it might change
        // in the future with new lunisolar schemas.
        _ = SchemaUT.CountMonthsInYear(Int32.MinValue);
        _ = SchemaUT.CountMonthsInYear(Int32.MaxValue);

        _ = SchemaUT.CountDaysInYear(minYearCore);
        _ = SchemaUT.CountDaysInYear(maxYearCore);

        for (int m = 1; m <= MaxMonth; m++)
        {
            _ = SchemaUT.IsIntercalaryMonth(Int32.MinValue, m);
            _ = SchemaUT.IsIntercalaryMonth(Int32.MaxValue, m);

            _ = SchemaUT.CountDaysInMonth(minYearCore, m);
            _ = SchemaUT.CountDaysInMonth(maxYearCore, m);

            for (int d = 1; d <= MaxDay; d++)
            {
                _ = SchemaUT.IsIntercalaryDay(Int32.MinValue, m, d);
                _ = SchemaUT.IsIntercalaryDay(Int32.MaxValue, m, d);

                _ = SchemaUT.IsSupplementaryDay(Int32.MinValue, m, d);
                _ = SchemaUT.IsSupplementaryDay(Int32.MaxValue, m, d);
            }
        }

        if (minYearCore != Int32.MinValue)
        {
            Assert.Overflows(() => SchemaUT.IsLeapYear(Int32.MinValue));
            Assert.Overflows(() => SchemaUT.CountDaysInYear(Int32.MinValue));
        }
        if (maxYearCore != Int32.MaxValue)
        {
            Assert.Overflows(() => SchemaUT.IsLeapYear(Int32.MaxValue));
            Assert.Overflows(() => SchemaUT.CountDaysInYear(Int32.MaxValue));
        }
    }

    [Fact] public void GetYear﹍Plain_DoesNotUnderflow() => _ = SchemaUT.GetYear(MinDaysSinceEpoch);
    [Fact] public void GetYear﹍Plain_DoesNotOverflow() => _ = SchemaUT.GetYear(MaxDaysSinceEpoch);

    [Fact] public void GetDateParts﹍DaysSinceEpoch_DoesNotUnderflow() => _ = SchemaUT.GetDateParts(MinDaysSinceEpoch);
    [Fact] public void GetDateParts﹍DaysSinceEpoch_DoesNotOverflow() => _ = SchemaUT.GetDateParts(MaxDaysSinceEpoch);

    [Fact] public void GetDateParts﹍OrdinalParts_DoesNotUnderflow() => _ = SchemaUT.GetDateParts(MinYear, 1);
    // Just for this test, we choose a custom value for MaxDayOfYear.
    [Fact] public void GetDateParts﹍OrdinalParts_DoesNotOverflow() => _ = SchemaUT.GetDateParts(MaxYear, 400);

    [Fact] public void GetOrdinalParts﹍DaysSinceEpoch_DoesNotUnderflow() => _ = SchemaUT.GetOrdinalParts(MinDaysSinceEpoch);
    [Fact] public void GetOrdinalParts﹍DaysSinceEpoch_DoesNotOverflow() => _ = SchemaUT.GetOrdinalParts(MaxDaysSinceEpoch);

    [Fact] public void GetOrdinalParts﹍DateParts_DoesNotUnderflow() => _ = SchemaUT.GetOrdinalParts(MinYear, 1, 1);
    [Fact] public void GetOrdinalParts﹍DateParts_DoesNotOverflow() => _ = SchemaUT.GetOrdinalParts(MaxYear, MaxMonth, MaxDay);

    [Fact] public void GetStartOfYearParts_DoesNotUnderflow() => _ = SchemaUT.GetStartOfYearParts(MinYear);
    [Fact] public void GetStartOfYearParts_DoesNotOverflow() => _ = SchemaUT.GetStartOfYearParts(MaxYear);
    [Fact] public void GetStartOfYearOrdinalParts_DoesNotUnderflow() => _ = SchemaUT.GetStartOfYearOrdinalParts(MinYear);
    [Fact] public void GetStartOfYearOrdinalParts_DoesNotOverflow() => _ = SchemaUT.GetStartOfYearOrdinalParts(MaxYear);

    [Fact] public void GetEndOfYearParts_DoesNotUnderflow() => _ = SchemaUT.GetEndOfYearParts(MinYear);
    [Fact] public void GetEndOfYearParts_DoesNotOverflow() => _ = SchemaUT.GetEndOfYearParts(MaxYear);
    [Fact] public void GetEndOfYearOrdinalParts_DoesNotUnderflow() => _ = SchemaUT.GetEndOfYearOrdinalParts(MinYear);
    [Fact] public void GetEndOfYearOrdinalParts_DoesNotOverflow() => _ = SchemaUT.GetEndOfYearOrdinalParts(MaxYear);

    [Fact] public void GetStartOfMonthParts_DoesNotUnderflow() => _ = SchemaUT.GetStartOfMonthParts(MinYear, 1);
    [Fact] public void GetStartOfMonthParts_DoesNotOverflow() => _ = SchemaUT.GetStartOfMonthParts(MaxYear, MaxMonth);
    [Fact] public void GetStartOfMonthOrdinalParts_DoesNotUnderflow() => _ = SchemaUT.GetStartOfMonthOrdinalParts(MinYear, 1);
    [Fact] public void GetStartOfMonthOrdinalParts_DoesNotOverflow() => _ = SchemaUT.GetStartOfMonthOrdinalParts(MaxYear, MaxMonth);

    [Fact] public void GetEndOfMonthParts_DoesNotUnderflow() => _ = SchemaUT.GetEndOfMonthParts(MinYear, 1);
    [Fact] public void GetEndOfMonthParts_DoesNotOverflow() => _ = SchemaUT.GetEndOfMonthParts(MaxYear, MaxMonth);
    [Fact] public void GetEndOfMonthOrdinalParts_DoesNotUnderflow() => _ = SchemaUT.GetEndOfMonthOrdinalParts(MinYear, 1);
    [Fact] public void GetEndOfMonthOrdinalParts_DoesNotOverflow() => _ = SchemaUT.GetEndOfMonthOrdinalParts(MaxYear, MaxMonth);
}