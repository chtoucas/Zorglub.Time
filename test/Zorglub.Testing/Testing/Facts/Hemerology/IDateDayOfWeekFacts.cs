// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Hemerology;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides more facts about <see cref="IDate{TSelf}"/>.
/// </summary>
public abstract partial class IDateDayOfWeekFacts<TDate, TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDate : IDate<TDate>
    where TDataSet : ICalendarDataSet, IDayOfWeekDataSet, ISingleton<TDataSet>
{
    protected IDateDayOfWeekFacts() { }

    protected abstract TDate GetDate(int y, int m, int d);

    protected TDate GetDate(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return GetDate(y, m, d);
    }

    public static DataGroup<YemodaAnd<DayOfWeek>> DayOfWeekData => DataSet.DayOfWeekData;
    public static DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_Before_Data => DataSet.DayOfWeek_Before_Data;
    public static DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_OnOrBefore_Data => DataSet.DayOfWeek_OnOrBefore_Data;
    public static DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_Nearest_Data => DataSet.DayOfWeek_Nearest_Data;
    public static DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_OnOrAfter_Data => DataSet.DayOfWeek_OnOrAfter_Data;
    public static DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_After_Data => DataSet.DayOfWeek_After_Data;
}

public partial class IDateDayOfWeekFacts<TDate, TDataSet> // Prelude
{
    [Theory, MemberData(nameof(DayOfWeekData))]
    public void DayOfWeek_Prop(YemodaAnd<DayOfWeek> info)
    {
        var (y, m, d, dayOfWeek) = info;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(dayOfWeek, date.DayOfWeek);
    }
}

public partial class IDateDayOfWeekFacts<TDate, TDataSet> // Adjust the day of the week
{
    [Theory, MemberData(nameof(DayOfWeek_Before_Data))]
    public void Previous(YemodaPairAnd<DayOfWeek> info)
    {
        var (ymd, ymdExp, dayOfWeek) = info;
        var date = GetDate(ymd);
        var exp = GetDate(ymdExp);
        // Act
        var actual = date.Previous(dayOfWeek);
        // Assert
        Assert.Equal(exp, actual);
    }

    [Theory, MemberData(nameof(DayOfWeek_OnOrBefore_Data))]
    public void PreviousOrSame(YemodaPairAnd<DayOfWeek> info)
    {
        var (ymd, ymdExp, dayOfWeek) = info;
        var date = GetDate(ymd);
        var exp = GetDate(ymdExp);
        // Act
        var actual = date.PreviousOrSame(dayOfWeek);
        // Assert
        Assert.Equal(exp, actual);
    }

    [Theory, MemberData(nameof(DayOfWeek_Nearest_Data))]
    public void Nearest(YemodaPairAnd<DayOfWeek> info)
    {
        var (ymd, ymdExp, dayOfWeek) = info;
        var date = GetDate(ymd);
        var exp = GetDate(ymdExp);
        // Act
        var actual = date.Nearest(dayOfWeek);
        // Assert
        Assert.Equal(exp, actual);
    }

    [Theory, MemberData(nameof(DayOfWeek_OnOrAfter_Data))]
    public void NextOrSame(YemodaPairAnd<DayOfWeek> info)
    {
        var (ymd, ymdExp, dayOfWeek) = info;
        var date = GetDate(ymd);
        var exp = GetDate(ymdExp);
        // Act
        var actual = date.NextOrSame(dayOfWeek);
        // Assert
        Assert.Equal(exp, actual);
    }

    [Theory, MemberData(nameof(DayOfWeek_After_Data))]
    public void Next(YemodaPairAnd<DayOfWeek> info)
    {
        var (ymd, ymdExp, dayOfWeek) = info;
        var date = GetDate(ymd);
        var exp = GetDate(ymdExp);
        // Act
        var actual = date.Next(dayOfWeek);
        // Assert
        Assert.Equal(exp, actual);
    }
}
