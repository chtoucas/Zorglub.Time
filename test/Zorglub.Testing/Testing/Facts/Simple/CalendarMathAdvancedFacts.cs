// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

// FIXME(fact): how to filter data? Update the TestSuite afterwards.
// 1) non-standard math for the Gregorian calendar
// 2) non-standard math for more calendars (missing data)

/// <summary>
/// Provides facts about <see cref="CalendarMath"/>; <i>ambiguous</i> cases.
/// <para>This class also includes tests for <see cref="CalendarDate"/>, <see cref="OrdinalDate"/>,
/// <see cref="CalendarMonth"/> and <see cref="CalendarYear"/>.</para>
/// </summary>
public abstract partial class CalendarMathAdvancedFacts<TDataSet>
    where TDataSet : IAdvancedMathDataSet, ISingleton<TDataSet>
{
    protected CalendarMathAdvancedFacts(Calendar calendar)
        : this(calendar?.Math ?? throw new ArgumentNullException(nameof(calendar))) { }

    protected CalendarMathAdvancedFacts(CalendarMath math)
    {
        MathUT = math ?? throw new ArgumentNullException(nameof(math));
        Calendar = math.Calendar;

        CalendarIsRegular = Calendar.IsRegular(out _);
    }

    protected static TDataSet DataSet => TDataSet.Instance;

    protected static AdditionRuleset AdditionRuleset => DataSet.AdditionRuleset;

    public static DataGroup<YemodaPairAnd<int>> AddYearsData => DataSet.AddYearsData;
    public static DataGroup<YemodaPairAnd<int>> AddMonthsData => DataSet.AddMonthsData;
    public static DataGroup<YedoyPairAnd<int>> AddYearsOrdinalData => DataSet.AddYearsOrdinalData;

    protected CalendarMath MathUT { get; }
    protected Calendar Calendar { get; }

    // When the calendar is regular, CalendarMonth.AddYears() and CalendarMonth.CountYearsBetween()
    // are unambiguous.
    protected bool CalendarIsRegular { get; }

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

public partial class CalendarMathAdvancedFacts<TDataSet> // CalendarDate
{
    [Theory, MemberData(nameof(AddYearsData))]
    public void AddYears﹍CalendarDate(YemodaPairAnd<int> info)
    {
        int ys = info.Value;
        var date = GetDate(info.First);
        var other = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(other, MathUT.AddYears(date, ys));
        Assert.Equal(other, date.PlusYears(ys));
        if (AdditionRuleset.DateRule == AdditionRule.Exact)
        {
            Assert.Equal(date, MathUT.AddYears(other, -ys));
            Assert.Equal(date, other.PlusYears(-ys));
        }
    }

    [Theory, MemberData(nameof(AddMonthsData))]
    public void AddMonths﹍CalendarDate(YemodaPairAnd<int> info)
    {
        int ms = info.Value;
        var date = GetDate(info.First);
        var other = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(other, MathUT.AddMonths(date, ms));
        Assert.Equal(other, date.PlusMonths(ms));
        if (AdditionRuleset.DateRule == AdditionRule.Exact)
        {
            Assert.Equal(date, MathUT.AddMonths(other, -ms));
            Assert.Equal(date, other.PlusMonths(-ms));
        }
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void CountYearsBetween﹍CalendarDate(YemodaPairAnd<int> info)
    {
        int ys = info.Value;
        var start = GetDate(info.First);
        var end = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(ys, MathUT.CountYearsBetween(start, end, out _));
        Assert.Equal(ys, end.CountYearsSince(start));
        if (AdditionRuleset.DateRule == AdditionRule.Exact)
        {
            Assert.Equal(-ys, MathUT.CountYearsBetween(end, start, out _));
            Assert.Equal(-ys, start.CountYearsSince(end));
        }
    }

    [Theory, MemberData(nameof(AddMonthsData))]
    public void CountMonthsBetween﹍CalendarDate(YemodaPairAnd<int> info)
    {
        int ms = info.Value;
        var start = GetDate(info.First);
        var end = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(ms, MathUT.CountMonthsBetween(start, end, out _));
        Assert.Equal(ms, end.CountMonthsSince(start));
        if (AdditionRuleset.DateRule == AdditionRule.Exact)
        {
            Assert.Equal(-ms, MathUT.CountMonthsBetween(end, start, out _));
            Assert.Equal(-ms, start.CountMonthsSince(end));
        }
    }
}

public partial class CalendarMathAdvancedFacts<TDataSet> // OrdinalDate
{
    [Theory, MemberData(nameof(AddYearsOrdinalData))]
    public void AddYears﹍OrdinalDate(YedoyPairAnd<int> info)
    {
        int ys = info.Value;
        var date = GetDate(info.First);
        var other = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(other, MathUT.AddYears(date, ys));
        Assert.Equal(other, date.PlusYears(ys));
        if (AdditionRuleset.OrdinalRule == AdditionRule.Exact)
        {
            Assert.Equal(date, MathUT.AddYears(other, -ys));
            Assert.Equal(date, other.PlusYears(-ys));
        }
    }

    [Theory, MemberData(nameof(AddYearsOrdinalData))]
    public void CountYearsBetween﹍OrdinalDate(YedoyPairAnd<int> info)
    {
        int ys = info.Value;
        var start = GetDate(info.First);
        var end = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(ys, MathUT.CountYearsBetween(start, end, out _));
        Assert.Equal(ys, end.CountYearsSince(start));
        if (AdditionRuleset.OrdinalRule == AdditionRule.Exact)
        {
            Assert.Equal(-ys, MathUT.CountYearsBetween(end, start, out _));
            Assert.Equal(-ys, start.CountYearsSince(end));
        }
    }
}

public partial class CalendarMathAdvancedFacts<TDataSet> // CalendarMonth
{
    [Theory, MemberData(nameof(AddYearsData))]
    public void AddYears﹍CalendarMonth(YemodaPairAnd<int> info)
    {
        int ys = info.Value;
        var month = GetMonth(info.First);
        var other = GetMonth(info.Second);
        // Act & Assert
        Assert.Equal(other, MathUT.AddYears(month, ys));
        Assert.Equal(other, month.PlusYears(ys));
        if (CalendarIsRegular
            || AdditionRuleset.MonthRule == AdditionRule.Exact)
        {
            Assert.Equal(month, MathUT.AddYears(other, -ys));
            Assert.Equal(month, other.PlusYears(-ys));
        }
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void CountYearsBetween﹍CalendarMonth(YemodaPairAnd<int> info)
    {
        int ys = info.Value;
        var start = GetMonth(info.First);
        var end = GetMonth(info.Second);
        // Act & Assert
        Assert.Equal(ys, MathUT.CountYearsBetween(start, end, out _));
        // CalendarMonth
        Assert.Equal(ys, end.CountYearsSince(start));
        if (CalendarIsRegular
            || AdditionRuleset.MonthRule == AdditionRule.Exact)
        {
            Assert.Equal(-ys, MathUT.CountYearsBetween(end, start, out _));
            Assert.Equal(-ys, start.CountYearsSince(end));
        }
    }
}
