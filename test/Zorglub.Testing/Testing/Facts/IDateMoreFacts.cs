// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts;

// Hypothesis: see IDateFacts.

using Zorglub.Testing.Data;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Hemerology;

// TODO(fact): WideDateFacts and SimpleDateFacts.

/// <summary>
/// Provides facts about <see cref="IDate{TSelf}"/>.
/// </summary>
public abstract partial class IDateMoreFacts<TDate, TDataSet> : IDateFacts<TDate, TDataSet>
    where TDate : struct, IDate<TDate>
    where TDataSet :
        ICalendarDataSet,
        IMathDataSet,
        IDayOfWeekDataSet,
        ISingleton<TDataSet>
{
    protected IDateMoreFacts(Range<int> supportedYears, Range<DayNumber> domain)
        : base(supportedYears, domain) { }

    // IMathDataSet
    public static TheoryData<Yemoda, Yemoda, int> AddDaysData => DataSet.AddDaysData;
    public static TheoryData<Yemoda, Yemoda> ConsecutiveDaysData => DataSet.ConsecutiveDaysData;

    // IDayOfWeekDataSet
    public static TheoryData<YemodaAnd<DayOfWeek>> DayOfWeekData => DataSet.DayOfWeekData;
    public static TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_Before_Data => DataSet.DayOfWeek_Before_Data;
    public static TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_OnOrBefore_Data => DataSet.DayOfWeek_OnOrBefore_Data;
    public static TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_Nearest_Data => DataSet.DayOfWeek_Nearest_Data;
    public static TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_OnOrAfter_Data => DataSet.DayOfWeek_OnOrAfter_Data;
    public static TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_After_Data => DataSet.DayOfWeek_After_Data;
}

public partial class IDateMoreFacts<TDate, TDataSet> // Prelude
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

public partial class IDateMoreFacts<TDate, TDataSet> // Adjust the day of the week
{
    [Theory, MemberData(nameof(DayOfWeek_Before_Data))]
    public void Previous(Yemoda ymd, Yemoda ymdExp, DayOfWeek dayOfWeek)
    {
        var date = GetDate(ymd);
        var exp = GetDate(ymdExp);
        // Act
        var actual = date.Previous(dayOfWeek);
        // Assert
        Assert.Equal(exp, actual);
    }

    [Theory, MemberData(nameof(DayOfWeek_OnOrBefore_Data))]
    public void PreviousOrSame(Yemoda ymd, Yemoda ymdExp, DayOfWeek dayOfWeek)
    {
        var date = GetDate(ymd);
        var exp = GetDate(ymdExp);
        // Act
        var actual = date.PreviousOrSame(dayOfWeek);
        // Assert
        Assert.Equal(exp, actual);
    }

    [Theory, MemberData(nameof(DayOfWeek_Nearest_Data))]
    public void Nearest(Yemoda ymd, Yemoda ymdExp, DayOfWeek dayOfWeek)
    {
        var date = GetDate(ymd);
        var exp = GetDate(ymdExp);
        // Act
        var actual = date.Nearest(dayOfWeek);
        // Assert
        Assert.Equal(exp, actual);
    }

    [Theory, MemberData(nameof(DayOfWeek_OnOrAfter_Data))]
    public void NextOrSame(Yemoda ymd, Yemoda ymdExp, DayOfWeek dayOfWeek)
    {
        var date = GetDate(ymd);
        var exp = GetDate(ymdExp);
        // Act
        var actual = date.NextOrSame(dayOfWeek);
        // Assert
        Assert.Equal(exp, actual);
    }

    [Theory, MemberData(nameof(DayOfWeek_After_Data))]
    public void Next(Yemoda ymd, Yemoda ymdExp, DayOfWeek dayOfWeek)
    {
        var date = GetDate(ymd);
        var exp = GetDate(ymdExp);
        // Act
        var actual = date.Next(dayOfWeek);
        // Assert
        Assert.Equal(exp, actual);
    }
}

public partial class IDateMoreFacts<TDate, TDataSet> // Increment / decrement
{
    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void Increment(Yemoda ymd, Yemoda ymdAfter)
    {
        var date = GetDate(ymd);
        var dateAfter = GetDate(ymdAfter);
        // Act & Assert
        Assert.Equal(dateAfter, ++date);
    }

    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void Decrement(Yemoda ymd, Yemoda ymdAfter)
    {
        var date = GetDate(ymd);
        var dateAfter = GetDate(ymdAfter);
        // Act & Assert
        Assert.Equal(date, --dateAfter);
    }

    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void NextDay(Yemoda ymd, Yemoda ymdAfter)
    {
        var date = GetDate(ymd);
        var dateAfter = GetDate(ymdAfter);
        // Act & Assert
        Assert.Equal(dateAfter, date.NextDay());
    }

    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void PreviousDay(Yemoda ymd, Yemoda ymdAfter)
    {
        var date = GetDate(ymd);
        var dateAfter = GetDate(ymdAfter);
        // Act & Assert
        Assert.Equal(date, dateAfter.PreviousDay());
    }
}

public partial class IDateMoreFacts<TDate, TDataSet> // Addition
{
    [Theory, MemberData(nameof(AddDaysData))]
    public void PlusDays(Yemoda ymd, Yemoda ymdOther, int days)
    {
        var date = GetDate(ymd);
        var other = GetDate(ymdOther);
        // Act & Assert
        // 1) date + days -> other.
        Assert.Equal(other, date + days);
        Assert.Equal(other, date.PlusDays(days));

        // 2) other - days -> date.
        Assert.Equal(date, other - days);
        Assert.Equal(date, other.PlusDays(-days));
    }

    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void PlusDays_ViaConsecutiveDays(Yemoda ymd, Yemoda ymdAfter)
    {
        var date = GetDate(ymd);
        var dateAfter = GetDate(ymdAfter);
        // Act & Assert
        // 1) date + 1 -> dateAfter.
        Assert.Equal(dateAfter, date + 1);
        Assert.Equal(dateAfter, date.PlusDays(1));

        // 2) dateAfter - 1 -> date.
        Assert.Equal(date, dateAfter - 1);
        Assert.Equal(date, dateAfter.PlusDays(-1));
    }

    [Theory, MemberData(nameof(AddDaysData))]
    public void CountDaysSince(Yemoda ymd, Yemoda ymdOther, int days)
    {
        var date = GetDate(ymd);
        var other = GetDate(ymdOther);
        // Act & Assert
        // 3) other - date -> days.
        Assert.Equal(days, other - date);
        Assert.Equal(days, other.CountDaysSince(date));

        // 4) date - other -> -days.
        Assert.Equal(-days, date - other);
        Assert.Equal(-days, date.CountDaysSince(other));
    }

    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void CountDaysSince_ViaConsecutiveDays(Yemoda ymd, Yemoda ymdAfter)
    {
        var date = GetDate(ymd);
        var dateAfter = GetDate(ymdAfter);
        // Act & Assert
        // 3) dateAfter - date -> 1.
        Assert.Equal(1, dateAfter - date);
        Assert.Equal(1, dateAfter.CountDaysSince(date));

        // 4) date - dateAfter -> -1.
        Assert.Equal(-1, date - dateAfter);
        Assert.Equal(-1, date.CountDaysSince(dateAfter));
    }
}
