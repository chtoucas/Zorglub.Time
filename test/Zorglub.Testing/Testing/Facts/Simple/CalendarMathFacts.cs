// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using System.Linq;

using Zorglub.Testing.Data;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Simple;

// TODO(fact): test the basic properties (commutatibity, zero, behaviour at the
// limits, etc.); see CalendarYear/MonthFacts and IDateFacts
// We should use custom AddYearsMonthData and AddMonthsMonthData.
// Plan: update this one then CalendarMathAdvancedFacts, CalendarDateFacts, OrdinalDateFacts, etc.

/// <summary>
/// Provides facts about <see cref="CalendarMath"/>; <i>unambiguous</i> math.
/// </summary>
public abstract partial class CalendarMathFacts<TMath, TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TMath : CalendarMath
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarMathFacts(TMath math)
    {
        MathUT = math ?? throw new ArgumentNullException(nameof(math));
        Calendar = math.Calendar;

        (MinDate, MaxDate) = Calendar.MinMaxDate;
        (MinOrdinal, MaxOrdinal) = Calendar.MinMaxOrdinal;
        (MinMonth, MaxMonth) = Calendar.MinMaxMonth;
    }

    protected TMath MathUT { get; }
    protected Calendar Calendar { get; }

    protected CalendarDate MinDate { get; }
    protected CalendarDate MaxDate { get; }

    protected OrdinalDate MinOrdinal { get; }
    protected OrdinalDate MaxOrdinal { get; }

    protected CalendarMonth MinMonth { get; }
    protected CalendarMonth MaxMonth { get; }

    /// <summary>
    /// We only use this sample year when its value matters (mathops); otherwise
    /// just use the first month of the year 1. It is initialized to ensure that
    /// the math operations we are going to perform will work.
    /// </summary>
    protected CalendarMonth GetSampleMonth() => Calendar.GetCalendarMonth(1234, 2);

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

public partial class CalendarMathFacts<TMath, TDataSet> // CalendarDate
{
    //
    // Month unit
    //

    #region AddMonths() and CalendarDate.PlusMonths()

    [Fact]
    public void AddMonths﹍CalendarDate_Overflows()
    {
        var date = Calendar.GetCalendarDate(1, 1, 1);
        // Act & Assert
        Assert.Overflows(() => MathUT.AddMonths(date, Int32.MinValue));
        Assert.Overflows(() => MathUT.AddMonths(date, Int32.MaxValue));
        // CalendarDate
        Assert.Overflows(() => date.PlusMonths(Int32.MinValue));
        Assert.Overflows(() => date.PlusMonths(Int32.MaxValue));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AddMonths﹍CalendarDate_Zero_IsNeutral(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = Calendar.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, MathUT.AddMonths(date, 0));
        // CalendarDate
        Assert.Equal(date, date.PlusMonths(0));
    }

    [Theory, MemberData(nameof(AddMonthsData))]
    public void AddMonths﹍CalendarDate(YemodaPairAnd<int> info)
    {
        int ms = info.Value;
        var date = GetDate(info.First);
        var other = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(other, MathUT.AddMonths(date, ms));
        Assert.Equal(date, MathUT.AddMonths(other, -ms));
        // CalendarDate
        Assert.Equal(other, date.PlusMonths(ms));
        Assert.Equal(date, other.PlusMonths(-ms));
    }

    #endregion
    #region CountMonthsBetween() and CalendarDate.CountMonthsSince()

    [Fact]
    public void CountMonthsBetween﹍CalendarDate_DoesNotOverflow()
    {
        _ = MathUT.CountMonthsBetween(MinDate, MaxDate);
        _ = MathUT.CountMonthsBetween(MaxDate, MinDate);
        // CalendarDate
        _ = MaxDate.CountMonthsSince(MinDate);
        _ = MinDate.CountMonthsSince(MaxDate);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountMonthsBetween﹍CalendarDate_WhenSame_IsZero(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = Calendar.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(0, MathUT.CountMonthsBetween(date, date));
        // CalendarDate
        Assert.Equal(0, date.CountMonthsSince(date));
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

    #endregion

    //
    // Year unit
    //

    #region AddYears() and CalendarDate.PlusYears()

    [Fact]
    public void AddYears﹍CalendarDate_Overflows()
    {
        var date = Calendar.GetCalendarDate(1, 1, 1);
        // Act & Assert
        Assert.Overflows(() => MathUT.AddYears(date, Int32.MinValue));
        Assert.Overflows(() => MathUT.AddYears(date, Int32.MaxValue));
        // CalendarDate
        Assert.Overflows(() => date.PlusYears(Int32.MinValue));
        Assert.Overflows(() => date.PlusYears(Int32.MaxValue));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AddYears﹍CalendarDate_Zero_IsNeutral(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = Calendar.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, MathUT.AddYears(date, 0));
        // CalendarDate
        Assert.Equal(date, date.PlusYears(0));
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void AddYears﹍CalendarDate(YemodaPairAnd<int> info)
    {
        int ys = info.Value;
        var date = GetDate(info.First);
        var other = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(other, MathUT.AddYears(date, ys));
        Assert.Equal(date, MathUT.AddYears(other, -ys));
        // CalendarDate
        Assert.Equal(other, date.PlusYears(ys));
        Assert.Equal(date, other.PlusYears(-ys));
    }

    #endregion
    #region CountYearsBetween() and CalendarDate.CountYearsSince()

    [Fact]
    public void CountYearsBetween﹍CalendarDate_DoesNotOverflow()
    {
        int ys = Calendar.SupportedYears.Count() - 1;
        // Act & Assert
        Assert.Equal(ys, MathUT.CountYearsBetween(MinDate, MaxDate));
        Assert.Equal(-ys, MathUT.CountYearsBetween(MaxDate, MinDate));
        // CalendarDate
        Assert.Equal(ys, MaxDate.CountYearsSince(MinDate));
        Assert.Equal(-ys, MinDate.CountYearsSince(MaxDate));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountYearsBetween﹍CalendarDate_WhenSame_IsZero(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = Calendar.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(0, MathUT.CountYearsBetween(date, date));
        // CalendarDate
        Assert.Equal(0, date.CountYearsSince(date));
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

    #endregion

}

public partial class CalendarMathFacts<TMath, TDataSet> // OrdinalDate
{
    //
    // Year unit
    //

    #region AddYears() and OrdinalDate.PlusYears()

    [Fact]
    public void AddYears﹍OrdinalDate_Overflows()
    {
        var date = Calendar.GetOrdinalDate(1, 1);
        // Act & Assert
        Assert.Overflows(() => MathUT.AddYears(date, Int32.MinValue));
        Assert.Overflows(() => MathUT.AddYears(date, Int32.MaxValue));
        // OrdinalDate
        Assert.Overflows(() => date.PlusYears(Int32.MinValue));
        Assert.Overflows(() => date.PlusYears(Int32.MaxValue));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AddYears﹍OrdinalDate_Zero_IsNeutral(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = Calendar.GetOrdinalDate(y, doy);
        // Act & Assert
        Assert.Equal(date, MathUT.AddYears(date, 0));
        // OrdinalDate
        Assert.Equal(date, date.PlusYears(0));
    }

    [Theory, MemberData(nameof(AddYearsOrdinalData))]
    public void AddYears﹍OrdinalDate(YedoyPairAnd<int> info)
    {
        int ys = info.Value;
        var date = GetDate(info.First);
        var other = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(other, MathUT.AddYears(date, ys));
        Assert.Equal(date, MathUT.AddYears(other, -ys));
        // OrdinalDate
        Assert.Equal(other, date.PlusYears(ys));
        Assert.Equal(date, other.PlusYears(-ys));
    }

    #endregion
    #region CountYearsBetween() and OrdinalDate.CountYearsSince()

    [Fact]
    public void CountYearsBetween﹍OrdinalDate_DoesNotOverflow()
    {
        int ys = Calendar.SupportedYears.Count() - 1;
        // Act & Assert
        Assert.Equal(ys, MathUT.CountYearsBetween(MinOrdinal, MaxOrdinal));
        Assert.Equal(-ys, MathUT.CountYearsBetween(MaxOrdinal, MinOrdinal));
        // OrdinalDate
        Assert.Equal(ys, MaxOrdinal.CountYearsSince(MinOrdinal));
        Assert.Equal(-ys, MinOrdinal.CountYearsSince(MaxOrdinal));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountYearsBetween﹍OrdinalDate_WhenSame_IsZero(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = Calendar.GetOrdinalDate(y, doy);
        // Act & Assert
        Assert.Equal(0, MathUT.CountYearsBetween(date, date));
        // OrdinalDate
        Assert.Equal(0, date.CountYearsSince(date));
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

    #endregion
}

public partial class CalendarMathFacts<TMath, TDataSet> // CalendarMonth
{
    //
    // Year unit
    //

    #region AddYears() and CalendarMonth.PlusYears()

    [Fact]
    public void AddYears﹍CalendarMonth_Overflows()
    {
        var month = Calendar.GetCalendarMonth(1, 1);
        // Act & Assert
        Assert.Overflows(() => MathUT.AddYears(month, Int32.MinValue));
        Assert.Overflows(() => MathUT.AddYears(month, Int32.MaxValue));
        // CalendarMonth
        Assert.Overflows(() => month.PlusYears(Int32.MinValue));
        Assert.Overflows(() => month.PlusYears(Int32.MaxValue));
    }

    [Fact]
    public void AddYears﹍CalendarMonth_WithLimitValues()
    {
        var month = GetSampleMonth();
        int minYs = MinMonth.Year - month.Year;
        int maxYs = MaxMonth.Year - month.Year;
        // NB: for calendars with a variable number of months per year, this
        // might not work.
        var minValue = MinMonth.WithMonth(month.Month);
        var maxValue = MaxMonth.WithMonth(month.Month);
        // Act & Assert
        Assert.Overflows(() => MathUT.AddYears(month, minYs - 1));
        Assert.Equal(minValue, MathUT.AddYears(month, minYs));
        Assert.Equal(maxValue, MathUT.AddYears(month, maxYs));
        Assert.Overflows(() => MathUT.AddYears(month, maxYs + 1));
        // CalendarMonth
        Assert.Overflows(() => month.PlusYears(minYs - 1));
        Assert.Equal(minValue, month.PlusYears(minYs));
        Assert.Equal(maxValue, month.PlusYears(maxYs));
        Assert.Overflows(() => month.PlusYears(maxYs + 1));
    }

    [Fact]
    public void AddYears﹍CalendarMonth_AtMinValue()
    {
        int ys = Calendar.SupportedYears.Count() - 1;
        // NB: for calendars with a variable number of months per year, this
        // might not work.
        var maxValue = MaxMonth.WithMonth(MinMonth.Month);
        // Act & Assert
        Assert.Overflows(() => MathUT.AddYears(MinMonth, -1));
        Assert.Equal(MinMonth, MathUT.AddYears(MinMonth, 0));
        Assert.Equal(maxValue, MathUT.AddYears(MinMonth, ys));
        Assert.Overflows(() => MathUT.AddYears(MinMonth, ys + 1));
        // CalendarMonth
        Assert.Overflows(() => MinMonth.PlusYears(-1));
        Assert.Equal(MinMonth, MinMonth.PlusYears(0));
        Assert.Equal(maxValue, MinMonth.PlusYears(ys));
        Assert.Overflows(() => MinMonth.PlusYears(ys + 1));
    }

    [Fact]
    public void AddYears﹍CalendarMonth_AtMaxValue()
    {
        int ys = Calendar.SupportedYears.Count() - 1;
        // NB: for calendars with a variable number of months per year, this
        // might not work.
        var minValue = MinMonth.WithMonth(MaxMonth.Month);
        // Act & Assert
        Assert.Overflows(() => MathUT.AddYears(MaxMonth, -ys - 1));
        Assert.Equal(minValue, MathUT.AddYears(MaxMonth, -ys));
        Assert.Equal(MaxMonth, MathUT.AddYears(MaxMonth, 0));
        Assert.Overflows(() => MathUT.AddYears(MaxMonth, 1));
        // CalendarMonth
        Assert.Overflows(() => MaxMonth.PlusYears(-ys - 1));
        Assert.Equal(minValue, MaxMonth.PlusYears(-ys));
        Assert.Equal(MaxMonth, MaxMonth.PlusYears(0));
        Assert.Overflows(() => MaxMonth.PlusYears(1));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void AddYears﹍CalendarMonth_Zero_IsNeutral(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = Calendar.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(month, MathUT.AddYears(month, 0));
        // CalendarMonth
        Assert.Equal(month, month.PlusYears(0));
    }

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

    #endregion
    #region CountYearsBetween()  and CalendarMonth.CountYearsSince()

    [Fact]
    public void CountYearsBetween﹍CalendarMonth_DoesNotOverflow()
    {
        int ys = Calendar.SupportedYears.Count() - 1;
        // Act & Assert
        Assert.Equal(ys, MathUT.CountYearsBetween(MinMonth, MaxMonth));
        Assert.Equal(-ys, MathUT.CountYearsBetween(MaxMonth, MinMonth));
        // CalendarMonth
        Assert.Equal(ys, MaxMonth.CountYearsSince(MinMonth));
        Assert.Equal(-ys, MinMonth.CountYearsSince(MaxMonth));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountYearsBetween﹍CalendarMonth_WhenSame_IsZero(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = Calendar.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(0, MathUT.CountYearsBetween(month, month));
        // CalendarMonth
        Assert.Equal(0, month.CountYearsSince(month));
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

    #endregion

    //
    // Month unit
    //
    // More or less a standard unit: we also test the math ops +, -, ++, --.

    #region CalendarMonth.NextMonth()

    [Fact]
    public void Increment﹍CalendarMonth_Overflows_AtMaxValue()
    {
        var max = MaxMonth;
        // Act & Assert
        Assert.Overflows(() => max++);
    }

    [Fact]
    public void NextMonth﹍CalendarMonth_Overflows_AtMaxValue() =>
        Assert.Overflows(() => MaxMonth.NextMonth());

    [Fact]
    public void Increment﹍CalendarMonth()
    {
        var month = GetSampleMonth();
        var monthAfter = Calendar.GetCalendarMonth(month.Year, month.Month + 1);
        // Act & Assert
        Assert.Equal(monthAfter, ++month);
    }

    [Fact]
    public void NextMonth﹍CalendarMonth()
    {
        var month = GetSampleMonth();
        var monthAfter = Calendar.GetCalendarMonth(month.Year, month.Month + 1);
        // Act & Assert
        Assert.Equal(monthAfter, month.NextMonth());
    }

    #endregion
    #region CalendarMonth.PreviousMonth()

    [Fact]
    public void Decrement﹍CalendarMonth_Overflows_AtMinValue()
    {
        var min = MinMonth;
        // Act & Assert
        Assert.Overflows(() => min--);
    }

    [Fact]
    public void PreviousMonth﹍CalendarMonth_Overflows_AtMinValue() =>
        Assert.Overflows(() => MinMonth.PreviousMonth());

    [Fact]
    public void Decrement()
    {
        var month = GetSampleMonth();
        var monthBefore = Calendar.GetCalendarMonth(month.Year, month.Month - 1);
        // Act & Assert
        Assert.Equal(monthBefore, --month);
    }

    [Fact]
    public void PreviousYear()
    {
        var month = GetSampleMonth();
        var monthBefore = Calendar.GetCalendarMonth(month.Year, month.Month - 1);
        // Act & Assert
        Assert.Equal(monthBefore, month.PreviousMonth());
    }

    #endregion

    #region AddMonths() and CalendarMonth.PlusMonths()

    [Fact]
    public void AddMonths﹍CalendarMonth_Overflows()
    {
        var month = Calendar.GetCalendarMonth(1, 1);
        // Act & Assert
        Assert.Overflows(() => MathUT.AddMonths(month, Int32.MinValue));
        Assert.Overflows(() => MathUT.AddMonths(month, Int32.MaxValue));
        // CalendarMonth
        Assert.Overflows(() => month + Int32.MinValue);
        Assert.Overflows(() => month + Int32.MaxValue);
        Assert.Overflows(() => month.PlusMonths(Int32.MinValue));
        Assert.Overflows(() => month.PlusMonths(Int32.MaxValue));
    }

    [Fact]
    public void AddMonths﹍CalendarMonth_WithLimitValues()
    {
        var supportedYears = Calendar.SupportedYears;
        var month = GetSampleMonth();
        int minMs = MinMonth - month;
        int maxMs = MaxMonth - month;
        // Act & Assert
        Assert.Overflows(() => MathUT.AddMonths(month, minMs - 1));
        Assert.Equal(MinMonth, MathUT.AddMonths(month, minMs));
        Assert.Equal(MaxMonth, MathUT.AddMonths(month, maxMs));
        Assert.Overflows(() => MathUT.AddMonths(month, maxMs + 1));
        // CalendarMonth
        Assert.Overflows(() => month + (minMs - 1));
        Assert.Equal(MinMonth, month + minMs);
        Assert.Equal(MaxMonth, month + maxMs);
        Assert.Overflows(() => month + (maxMs + 1));
        Assert.Overflows(() => month.PlusMonths(minMs - 1));
        Assert.Equal(MinMonth, month.PlusMonths(minMs));
        Assert.Equal(MaxMonth, month.PlusMonths(maxMs));
        Assert.Overflows(() => month.PlusMonths(maxMs + 1));
    }

    [Fact]
    public void AddMonths﹍CalendarMonth_AtMinValue()
    {
        int ms = MaxMonth - MinMonth;
        // Act & Assert
        Assert.Overflows(() => MathUT.AddMonths(MinMonth, -1));
        Assert.Equal(MinMonth, MathUT.AddMonths(MinMonth, 0));
        Assert.Equal(MaxMonth, MathUT.AddMonths(MinMonth, ms));
        Assert.Overflows(() => MathUT.AddMonths(MinMonth, ms + 1));
        // CalendarMonth
        Assert.Overflows(() => MinMonth - 1);
        Assert.Equal(MinMonth, MinMonth + 0);
        Assert.Equal(MaxMonth, MinMonth + ms);
        Assert.Overflows(() => MinMonth + (ms + 1));
        Assert.Overflows(() => MinMonth.PlusMonths(-1));
        Assert.Equal(MinMonth, MinMonth.PlusMonths(0));
        Assert.Equal(MaxMonth, MinMonth.PlusMonths(ms));
        Assert.Overflows(() => MinMonth.PlusMonths(ms + 1));
    }

    [Fact]
    public void AddMonths﹍CalendarMonth_AtMaxValue()
    {
        int ms = MaxMonth - MinMonth;
        // Act & Assert
        Assert.Overflows(() => MathUT.AddMonths(MaxMonth, -ms - 1));
        Assert.Equal(MinMonth, MathUT.AddMonths(MaxMonth, -ms));
        Assert.Equal(MaxMonth, MathUT.AddMonths(MaxMonth, 0));
        Assert.Overflows(() => MathUT.AddMonths(MaxMonth, 1));
        // CalendarMonth
        Assert.Overflows(() => MaxMonth - (ms + 1));
        Assert.Equal(MinMonth, MaxMonth - ms);
        Assert.Equal(MaxMonth, MaxMonth + 0);
        Assert.Overflows(() => MaxMonth + 1);
        Assert.Overflows(() => MaxMonth.PlusMonths(-ms - 1));
        Assert.Equal(MinMonth, MaxMonth.PlusMonths(-ms));
        Assert.Equal(MaxMonth, MaxMonth.PlusMonths(0));
        Assert.Overflows(() => MaxMonth.PlusMonths(1));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void AddMonths﹍CalendarMonth_Zero_IsNeutral(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = Calendar.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(month, MathUT.AddMonths(month, 0));
        // CalendarMonth
        Assert.Equal(month, month.PlusMonths(0));
    }

    [Theory, MemberData(nameof(AddMonthsData))]
    public void AddMonths﹍CalendarMonth(YemodaPairAnd<int> info)
    {
        int ms = info.Value;
        var month = GetMonth(info.First);
        var other = GetMonth(info.Second);
        // Act & Assert
        Assert.Equal(other, MathUT.AddMonths(month, ms));
        Assert.Equal(month, MathUT.AddMonths(other, -ms));
        // CalendarMonth
        Assert.Equal(other, month + ms);
        Assert.Equal(month, other - ms);
        Assert.Equal(other, month - (-ms));
        Assert.Equal(other, month.PlusMonths(ms));
        Assert.Equal(month, other.PlusMonths(-ms));
    }

    #endregion
    #region CountMonthsBetween() and CalendarMonth.CountMonthsSince()

    [Fact]
    public void CountMonthsBetween﹍CalendarMonth_DoesNotOverflow()
    {
        _ = MathUT.CountMonthsBetween(MinMonth, MaxMonth);
        _ = MathUT.CountMonthsBetween(MaxMonth, MinMonth);
        // CalendarMonth
        _ = MaxMonth - MinMonth;
        _ = MinMonth - MaxMonth;
        _ = MaxMonth.CountMonthsSince(MinMonth);
        _ = MinMonth.CountMonthsSince(MaxMonth);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountMonthsBetween﹍CalendarMonth_WhenSame_IsZero(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = Calendar.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(0, MathUT.CountMonthsBetween(month, month));
        // CalendarMonth
        Assert.Equal(0, month - month);
        Assert.Equal(0, month.CountMonthsSince(month));
    }

    [Theory, MemberData(nameof(AddMonthsData))]
    public void CountMonthsBetween﹍CalendarMonth(YemodaPairAnd<int> info)
    {
        int ms = info.Value;
        var start = GetMonth(info.First);
        var end = GetMonth(info.Second);
        // Act & Assert
        Assert.Equal(ms, MathUT.CountMonthsBetween(start, end));
        Assert.Equal(-ms, MathUT.CountMonthsBetween(end, start));
        // CalendarMonth
        Assert.Equal(ms, end - start);
        Assert.Equal(-ms, start - end);
        Assert.Equal(ms, end.CountMonthsSince(start));
        Assert.Equal(-ms, start.CountMonthsSince(end));
    }

    #endregion
}
