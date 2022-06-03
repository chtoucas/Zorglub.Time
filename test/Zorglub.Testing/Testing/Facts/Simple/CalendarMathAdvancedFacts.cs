// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

// FIXME(fact): how to filter data (update CalendarMathTestSuite afterwards).
// Test AdditionRules.
// We should also test CalendricalArithmetic, CalendarDate, etc. and not just
// for the Gregorian calendar.

/// <summary>
/// Provides facts about <see cref="CalendarMath"/>; <i>ambiguous</i> cases.
/// <para>We also test the math part of <see cref="CalendarDate"/>, <see cref="OrdinalDate"/>,
/// <see cref="CalendarMonth"/> and <see cref="CalendarYear"/>.</para>
/// </summary>
public abstract partial class CalendarMathAdvancedFacts<TMath, TDataSet>
    where TMath : CalendarMath
    where TDataSet : IAdvancedMathDataSet, ISingleton<TDataSet>
{
    protected CalendarMathAdvancedFacts(TMath math)
    {
        MathUT = math ?? throw new ArgumentNullException(nameof(math));
        Calendar = math.Calendar;
    }

    protected static TDataSet DataSet => TDataSet.Instance;

    protected static AdditionRules AdditionRules => DataSet.AdditionRules;

    public static DataGroup<YemodaPairAnd<int>> AddYearsData => DataSet.AddYearsData;
    public static DataGroup<YemodaPairAnd<int>> AddMonthsData => DataSet.AddMonthsData;
    public static DataGroup<YedoyPairAnd<int>> AddYearsOrdinalData => DataSet.AddYearsOrdinalData;

    protected TMath MathUT { get; }
    protected Calendar Calendar { get; }

    protected CalendarDate GetDate(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return Calendar.GetCalendarDate(y, m, d);
    }

    protected OrdinalDate GetDate(Yedoy ydoy)
    {
        var (y, doy) = ydoy;
        return Calendar.GetOrdinalDate(y, doy);
    }

    protected CalendarMonth GetMonth(Yemoda ymd)
    {
        var (y, m, _) = ymd;
        return Calendar.GetCalendarMonth(y, m);
    }
}

public partial class CalendarMathAdvancedFacts<TMath, TDataSet> // CalendarDate
{
    [Theory, MemberData(nameof(AddYearsData))]
    public void AddYears﹍CalendarDate(YemodaPairAnd<int> info)
    {
        int ys = info.Value;
        var date = GetDate(info.First);
        var other = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(other, MathUT.AddYears(date, ys));
        //Assert.Equal(date, MathUT.AddYears(other, -ys));
        // CalendarDate
        Assert.Equal(other, date.PlusYears(ys));
        //Assert.Equal(date, other.PlusYears(-ys));
    }

    [Theory, MemberData(nameof(AddMonthsData))]
    public void AddMonths﹍CalendarDate(YemodaPairAnd<int> info)
    {
        int ms = info.Value;
        var date = GetDate(info.First);
        var other = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(other, MathUT.AddMonths(date, ms));
        //Assert.Equal(date, MathUT.AddMonths(other, -ms));
        // CalendarDate
        Assert.Equal(other, date.PlusMonths(ms));
        //Assert.Equal(date, other.PlusMonths(-ms));
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void CountYearsBetween﹍CalendarDate(YemodaPairAnd<int> info)
    {
        int ys = info.Value;
        var start = GetDate(info.First);
        var end = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(ys, MathUT.CountYearsBetween(start, end));
        Assert.Equal(-ys, MathUT.CountYearsBetween(end, start));
        // CalendarDate
        Assert.Equal(ys, end.CountYearsSince(start));
        Assert.Equal(-ys, start.CountYearsSince(end));
    }

    [Theory, MemberData(nameof(AddMonthsData))]
    public void CountMonthsBetween﹍CalendarDate(YemodaPairAnd<int> info)
    {
        int ms = info.Value;
        var start = GetDate(info.First);
        var end = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(ms, MathUT.CountMonthsBetween(start, end));
        Assert.Equal(-ms, MathUT.CountMonthsBetween(end, start));
        // CalendarDate
        Assert.Equal(ms, end.CountMonthsSince(start));
        Assert.Equal(-ms, start.CountMonthsSince(end));
    }
}

public partial class CalendarMathAdvancedFacts<TMath, TDataSet> // OrdinalDate
{
    [Theory, MemberData(nameof(AddYearsOrdinalData))]
    public void AddYears﹍OrdinalDate(YedoyPairAnd<int> info)
    {
        int ys = info.Value;
        var date = GetDate(info.First);
        var other = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(other, MathUT.AddYears(date, ys));
        //Assert.Equal(date, MathUT.AddYears(other, -ys));
        // OrdinalDate
        Assert.Equal(other, date.PlusYears(ys));
        //Assert.Equal(date, other.PlusYears(-ys));
    }

    [Theory, MemberData(nameof(AddYearsOrdinalData))]
    public void CountYearsBetween﹍OrdinalDate(YedoyPairAnd<int> info)
    {
        int ys = info.Value;
        var start = GetDate(info.First);
        var end = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(ys, MathUT.CountYearsBetween(start, end));
        Assert.Equal(-ys, MathUT.CountYearsBetween(end, start));
        // OrdinalDate
        Assert.Equal(ys, end.CountYearsSince(start));
        Assert.Equal(-ys, start.CountYearsSince(end));
    }
}

public partial class CalendarMathAdvancedFacts<TMath, TDataSet> // CalendarMonth
{
    [Theory, MemberData(nameof(AddYearsData))]
    public void AddYears﹍CalendarMonth(YemodaPairAnd<int> info)
    {
        int ys = info.Value;
        var month = GetMonth(info.First);
        var other = GetMonth(info.Second);
        // Act & Assert
        Assert.Equal(other, MathUT.AddYears(month, ys));
        Assert.Equal(month, MathUT.AddYears(other, -ys));
        // CalendarMonth
        Assert.Equal(other, month.PlusYears(ys));
        Assert.Equal(month, other.PlusYears(-ys));
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void CountYearsBetween﹍CalendarMonth(YemodaPairAnd<int> info)
    {
        int ys = info.Value;
        var start = GetMonth(info.First);
        var end = GetMonth(info.Second);
        // Act & Assert
        Assert.Equal(ys, MathUT.CountYearsBetween(start, end));
        Assert.Equal(-ys, MathUT.CountYearsBetween(end, start));
        // CalendarMonth
        Assert.Equal(ys, end.CountYearsSince(start));
        Assert.Equal(-ys, start.CountYearsSince(end));
    }
}
