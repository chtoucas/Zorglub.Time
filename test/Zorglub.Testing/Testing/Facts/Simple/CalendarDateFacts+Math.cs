// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

/// <summary>
/// Provides facts about <see cref="CalendarDate"/> and its non-standard mathematical operations.
/// </summary>
public abstract partial class CalendarDateMathFacts<TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, IAdvancedMathDataSet, ISingleton<TDataSet>
{
    protected CalendarDateMathFacts(Calendar calendar)
    {
        CalendarUT = calendar ?? throw new ArgumentNullException(nameof(calendar));
    }

    protected Calendar CalendarUT { get; }

    protected CalendarDate GetDate(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return CalendarUT.GetCalendarDate(y, m, d);
    }

    public static DataGroup<YemodaPairAnd<int>> AddYearsData => DataSet.AddYearsData;
    public static DataGroup<YemodaPairAnd<int>> AddMonthsData => DataSet.AddMonthsData;
}

public partial class CalendarDateMathFacts<TDataSet> // PlusMonths()
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void PlusMonths_Zero_IsNeutral(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CalendarUT.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, date.PlusMonths(0));
    }

    [Theory, MemberData(nameof(AddMonthsData))]
    public void PlusMonths(YemodaPairAnd<int> info)
    {
        int months = info.Value;
        var date = GetDate(info.First);
        var other = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(other, date.PlusMonths(months));
    }
}

public partial class CalendarDateMathFacts<TDataSet> // CountMonthsSince()
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void CountMonthsSince_WhenSame_IsZero(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CalendarUT.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(0, date.CountMonthsSince(date));
    }

    [Theory, MemberData(nameof(AddMonthsData))]
    public void CountMonthsSince(YemodaPairAnd<int> info)
    {
        int months = info.Value;
        var date = GetDate(info.First);
        var other = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(months, other.CountMonthsSince(date));
    }
}

public partial class CalendarDateMathFacts<TDataSet> // PlusYears()
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void PlusYears_Zero_IsNeutral(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CalendarUT.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, date.PlusYears(0));
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void PlusYears(YemodaPairAnd<int> info)
    {
        int years = info.Value;
        var date = GetDate(info.First);
        var other = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(other, date.PlusYears(years));
    }
}

public partial class CalendarDateMathFacts<TDataSet> // CountYearsSince()
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void CountYearsSince_WhenSame_IsZero(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CalendarUT.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(0, date.CountYearsSince(date));
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void CountYearsSince(YemodaPairAnd<int> info)
    {
        int years = info.Value;
        var date = GetDate(info.First);
        var other = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(years, other.CountYearsSince(date));
    }
}
