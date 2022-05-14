// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

/// <summary>
/// Provides math facts about <see cref="CalendarDate"/>.
/// </summary>
public abstract partial class CalendarDateMathFacts<TDataSet>
    where TDataSet : IAdvancedMathDataSet, ISingleton<TDataSet>
{
    protected CalendarDateMathFacts(Calendar calendar!!)
    {
        CalendarUT = calendar;
    }

    protected static TDataSet DataSet { get; } = TDataSet.Instance;

    protected Calendar CalendarUT { get; }

    protected CalendarDate GetCalendarDate(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return CalendarUT.GetCalendarDate(y, m, d);
    }

    public static TheoryData<Yemoda, Yemoda, int> AddYearsData => DataSet.AddYearsData;
    public static TheoryData<Yemoda, Yemoda, int> AddMonthsData => DataSet.AddMonthsData;
}

public partial class CalendarDateMathFacts<TDataSet>
{
    [Theory, MemberData(nameof(AddMonthsData))]
    public void PlusMonths(Yemoda ymd, Yemoda result, int months)
    {
        var date = GetCalendarDate(ymd);
        var exp = GetCalendarDate(result);
        // Act & Assert
        Assert.Equal(exp, date.PlusMonths(months));
    }

    [Theory, MemberData(nameof(AddMonthsData))]
    public void CountMonthsSince(Yemoda ymd, Yemoda result, int months)
    {
        var date = GetCalendarDate(ymd);
        var exp = GetCalendarDate(result);
        // Act & Assert
        Assert.Equal(months, exp.CountMonthsSince(date));
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void PlusYears(Yemoda ymd, Yemoda result, int years)
    {
        var date = GetCalendarDate(ymd);
        var exp = GetCalendarDate(result);
        // Act & Assert
        Assert.Equal(exp, date.PlusYears(years));
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void CountYearsSince(Yemoda ymd, Yemoda result, int years)
    {
        var date = GetCalendarDate(ymd);
        var exp = GetCalendarDate(result);
        // Act & Assert
        Assert.Equal(years, exp.CountYearsSince(date));
    }
}
