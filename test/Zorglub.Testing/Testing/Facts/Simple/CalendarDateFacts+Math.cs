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
    protected CalendarDateMathFacts(Calendar calendar)
    {
        CalendarUT = calendar ?? throw new ArgumentNullException(nameof(calendar));
    }

    protected static TDataSet DataSet { get; } = TDataSet.Instance;

    protected Calendar CalendarUT { get; }

    protected CalendarDate GetCalendarDate(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return CalendarUT.GetCalendarDate(y, m, d);
    }

    public static DataGroup<YemodaPairAnd<int>> AddYearsData => DataSet.AddYearsData;
    public static DataGroup<YemodaPairAnd<int>> AddMonthsData => DataSet.AddMonthsData;
}

public partial class CalendarDateMathFacts<TDataSet>
{
    [Theory, MemberData(nameof(AddMonthsData))]
    public void PlusMonths(YemodaPairAnd<int> info)
    {
        var (start, end, months) = info;
        var date = GetCalendarDate(start);
        var exp = GetCalendarDate(end);
        // Act & Assert
        Assert.Equal(exp, date.PlusMonths(months));
    }

    [Theory, MemberData(nameof(AddMonthsData))]
    public void CountMonthsSince(YemodaPairAnd<int> info)
    {
        var (start, end, months) = info;
        var date = GetCalendarDate(start);
        var exp = GetCalendarDate(end);
        // Act & Assert
        Assert.Equal(months, exp.CountMonthsSince(date));
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void PlusYears(YemodaPairAnd<int> info)
    {
        var (start, end, years) = info;
        var date = GetCalendarDate(start);
        var exp = GetCalendarDate(end);
        // Act & Assert
        Assert.Equal(exp, date.PlusYears(years));
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void CountYearsSince(YemodaPairAnd<int> info)
    {
        var (start, end, years) = info;
        var date = GetCalendarDate(start);
        var exp = GetCalendarDate(end);
        // Act & Assert
        Assert.Equal(years, exp.CountYearsSince(date));
    }
}
