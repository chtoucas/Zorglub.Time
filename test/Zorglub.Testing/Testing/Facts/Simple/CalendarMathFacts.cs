// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using System.Linq;

using Zorglub.Testing.Data;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Simple;

// CalendarMathFacts is our main testing class for math-related operations.
//
// What's not in CalendarMathFacts:
// - Standard methods/ops for CalendarDate, CalendarDay and OrdinalDate.
//   The tests are to be found in their specialized test class, inherited from
//   SimpleDateFacts and IDateFacts.
// - CalendarMathAdvancedFacts for when the result is _ambiguous_.

/// <summary>
/// Provides facts about <see cref="CalendarMath"/>; <i>unambiguous</i> math.
/// <para>This class also includes tests for <see cref="CalendarDate"/>, <see cref="OrdinalDate"/>,
/// <see cref="CalendarMonth"/> and <see cref="CalendarYear"/>.</para>
/// </summary>
public abstract partial class CalendarMathFacts<TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarMathFacts(SimpleCalendar calendar)
        : this(calendar?.Math ?? throw new ArgumentNullException(nameof(calendar))) { }

    protected CalendarMathFacts(CalendarMath math)
    {
        MathUT = math ?? throw new ArgumentNullException(nameof(math));
        Calendar = math.Calendar;

        (MinDate, MaxDate) = Calendar.MinMaxDate;
        (MinOrdinal, MaxOrdinal) = Calendar.MinMaxOrdinal;
        (MinMonth, MaxMonth) = Calendar.MinMaxMonth;
    }

    protected CalendarMath MathUT { get; }
    protected SimpleCalendar Calendar { get; }

    protected CalendarDate MinDate { get; }
    protected CalendarDate MaxDate { get; }

    protected OrdinalDate MinOrdinal { get; }
    protected OrdinalDate MaxOrdinal { get; }

    protected CalendarMonth MinMonth { get; }
    protected CalendarMonth MaxMonth { get; }

    protected CalendarDate GetDate(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return Calendar.GetDate(y, m, d);
    }

    protected OrdinalDate GetDate(Yedoy ydoy)
    {
        var (y, doy) = ydoy;
        return Calendar.GetDate(y, doy);
    }

    protected CalendarMonth GetMonth(Yemo ym)
    {
        var (y, m) = ym;
        return Calendar.GetCalendarMonth(y, m);
    }

    /// <summary>
    /// We only use this sample year when its value matters (mathops); otherwise
    /// just use the first month of the year 1. It is initialized to ensure that
    /// the math operations we are going to perform will work.
    /// </summary>
    private CalendarMonth GetSampleMonth() => Calendar.GetCalendarMonth(1234, 2);
}

public partial class CalendarMathFacts<TDataSet> // CalendarDate
{
    //
    // Year unit
    //
    // We also test
    // - CalendarDate.PlusYears()
    // - CalendarDate.CountYearsSince()

    #region AddYears()

    [Fact]
    public void AddYears﹍CalendarDate_Overflows_WithMaxYears()
    {
        var date = Calendar.GetDate(1, 1, 1);
        // Act & Assert
        Assert.Overflows(() => MathUT.AddYears(date, Int32.MinValue));
        Assert.Overflows(() => MathUT.AddYears(date, Int32.MaxValue));
        // CalendarDate
        Assert.Overflows(() => date.PlusYears(Int32.MinValue));
        Assert.Overflows(() => date.PlusYears(Int32.MaxValue));
    }

    [Fact]
    public void AddYears﹍CalendarDate_AtMinDate()
    {
        int ys = Calendar.YearsValidator.Range.Count() - 1;
        // Act & Assert
        Assert.Overflows(() => MathUT.AddYears(MinDate, -1));
        Assert.Equal(MinDate, MathUT.AddYears(MinDate, 0));
        _ = MathUT.AddYears(MinDate, ys);
        Assert.Overflows(() => MathUT.AddYears(MinDate, ys + 1));
        // OrdinalDate
        Assert.Overflows(() => MinDate.PlusYears(-1));
        Assert.Equal(MinDate, MinDate.PlusYears(0));
        _ = MinDate.PlusYears(ys);
        Assert.Overflows(() => MinDate.PlusYears(ys + 1));
    }

    [Fact]
    public void AddYears﹍CalendarDate_AtMaxDate()
    {
        int ys = Calendar.YearsValidator.Range.Count() - 1;
        // Act & Assert
        Assert.Overflows(() => MathUT.AddYears(MaxDate, -ys - 1));
        _ = MathUT.AddYears(MaxDate, -ys);
        Assert.Equal(MaxDate, MathUT.AddYears(MaxDate, 0));
        Assert.Overflows(() => MathUT.AddYears(MaxDate, 1));
        // OrdinalDate
        Assert.Overflows(() => MaxDate.PlusYears(-ys - 1));
        _ = MaxDate.PlusYears(-ys);
        Assert.Equal(MaxDate, MaxDate.PlusYears(0));
        Assert.Overflows(() => MaxDate.PlusYears(1));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AddYears﹍CalendarDate_Zero_IsNeutral(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = Calendar.GetDate(y, m, d);
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
    #region CountYearsBetween()

    [Fact]
    public void CountYearsBetween﹍CalendarDate_DoesNotOverflow()
    {
        int ys = Calendar.YearsValidator.Range.Count() - 1;
        // Act & Assert
        Assert.Equal(ys, MathUT.CountYearsBetween(MinDate, MaxDate, out var newStart));
        Assert.Equal(MinDate.PlusYears(ys), newStart);
        Assert.Equal(-ys, MathUT.CountYearsBetween(MaxDate, MinDate, out newStart));
        Assert.Equal(MaxDate.PlusYears(-ys), newStart);
        // CalendarDate
        Assert.Equal(ys, MaxDate.CountYearsSince(MinDate));
        Assert.Equal(-ys, MinDate.CountYearsSince(MaxDate));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountYearsBetween﹍CalendarDate_WhenSame_IsZero(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = Calendar.GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(0, MathUT.CountYearsBetween(date, date, out var newStart));
        Assert.Equal(date, newStart);
        // CalendarDate
        Assert.Equal(0, date.CountYearsSince(date));
    }

    [Theory, MemberData(nameof(CountYearsBetweenData))]
    public void CountYearsBetween﹍CalendarDate(YemodaPairAnd<int> info)
    {
        int ys = info.Value;
        var start = GetDate(info.First);
        var end = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(ys, MathUT.CountYearsBetween(start, end, out var newStart));
        Assert.Equal(start.PlusYears(ys), newStart);
        Assert.Equal(-ys, MathUT.CountYearsBetween(end, start, out newStart));
        Assert.Equal(end.PlusYears(-ys), newStart);
        // TODO(fact): test that PlusYears(ys + 1 or maybe - 1) > end; idem with the other
        // methods and in CalendarMathAdvancedFacts.
        //if (ys >= 0)
        //{
        //    Assert.True(newStart < end);
        //    Assert.True(start.PlusYears(ys + 1) > end);
        //    Assert.True(end.PlusYears(-ys - 1) < start);
        //}
        //else
        //{
        //    //Assert.True(start.PlusYears(ys - 1) < end);
        //    //Assert.True(end.PlusYears(-ys + 1) > start);
        //}
        // CalendarDate
        Assert.Equal(ys, end.CountYearsSince(start));
        Assert.Equal(-ys, start.CountYearsSince(end));
    }

    #endregion

    //
    // Month unit
    //
    // We also test
    // - CalendarDate.PlusMonths()
    // - CalendarDate.CountMonthsSince()

    #region AddMonths()

    [Fact]
    public void AddMonths﹍CalendarDate_Overflows_WithMaxMonths()
    {
        var date = Calendar.GetDate(1, 1, 1);
        // Act & Assert
        Assert.Overflows(() => MathUT.AddMonths(date, Int32.MinValue));
        Assert.Overflows(() => MathUT.AddMonths(date, Int32.MaxValue));
        // CalendarDate
        Assert.Overflows(() => date.PlusMonths(Int32.MinValue));
        Assert.Overflows(() => date.PlusMonths(Int32.MaxValue));
    }

    [Fact]
    public void AddMonths﹍CalendarDate_AtMinDate()
    {
        // Act & Assert
        Assert.Overflows(() => MathUT.AddMonths(MinDate, -1));
        Assert.Equal(MinDate, MathUT.AddMonths(MinDate, 0));
        // CalendarMonth
        Assert.Overflows(() => MinDate.PlusMonths(-1));
        Assert.Equal(MinDate, MinDate.PlusMonths(0));
    }

    [Fact]
    public void AddMonths﹍CalendarDate_AtMaxDate()
    {
        // Act & Assert
        Assert.Equal(MaxDate, MathUT.AddMonths(MaxDate, 0));
        Assert.Overflows(() => MathUT.AddMonths(MaxDate, 1));
        // CalendarMonth
        Assert.Equal(MaxDate, MaxDate.PlusMonths(0));
        Assert.Overflows(() => MaxDate.PlusMonths(1));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AddMonths﹍CalendarDate_Zero_IsNeutral(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = Calendar.GetDate(y, m, d);
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
    #region CountMonthsBetween()

    [Fact]
    public void CountMonthsBetween﹍CalendarDate_DoesNotOverflow()
    {
        int ms = MathUT.CountMonthsBetween(MinDate, MaxDate, out var newStart);
        Assert.Equal(MinDate.PlusMonths(ms), newStart);
        int ms1 = MathUT.CountMonthsBetween(MaxDate, MinDate, out newStart);
        Assert.Equal(-ms, ms1);
        Assert.Equal(MaxDate.PlusMonths(-ms), newStart);
        // CalendarDate
        _ = MaxDate.CountMonthsSince(MinDate);
        _ = MinDate.CountMonthsSince(MaxDate);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountMonthsBetween﹍CalendarDate_WhenSame_IsZero(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = Calendar.GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(0, MathUT.CountMonthsBetween(date, date, out var newStart));
        Assert.Equal(date, newStart);
        // CalendarDate
        Assert.Equal(0, date.CountMonthsSince(date));
    }

    [Theory, MemberData(nameof(CountMonthsBetweenData))]
    public void CountMonthsBetween﹍CalendarDate(YemodaPairAnd<int> info)
    {
        int ms = info.Value;
        var start = GetDate(info.First);
        var end = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(ms, MathUT.CountMonthsBetween(start, end, out var newStart));
        Assert.Equal(start.PlusMonths(ms), newStart);
        Assert.Equal(-ms, MathUT.CountMonthsBetween(end, start, out newStart));
        Assert.Equal(end.PlusMonths(-ms), newStart);
        // CalendarDate
        Assert.Equal(ms, end.CountMonthsSince(start));
        Assert.Equal(-ms, start.CountMonthsSince(end));
    }

    #endregion
}

public partial class CalendarMathFacts<TDataSet> // OrdinalDate
{
    //
    // Year unit
    //
    // We also test
    // - OrdinalDate.PlusYears()
    // - OrdinalDate.CountYearsSince()

    #region AddYears()

    [Fact]
    public void AddYears﹍OrdinalDate_Overflows_WithMaxYears()
    {
        var date = Calendar.GetDate(1, 1);
        // Act & Assert
        Assert.Overflows(() => MathUT.AddYears(date, Int32.MinValue));
        Assert.Overflows(() => MathUT.AddYears(date, Int32.MaxValue));
        // OrdinalDate
        Assert.Overflows(() => date.PlusYears(Int32.MinValue));
        Assert.Overflows(() => date.PlusYears(Int32.MaxValue));
    }

    [Fact]
    public void AddYears﹍OrdinalDate_AtMinOrdinal()
    {
        int ys = Calendar.YearsValidator.Range.Count() - 1;
        // Act & Assert
        Assert.Overflows(() => MathUT.AddYears(MinOrdinal, -1));
        Assert.Equal(MinOrdinal, MathUT.AddYears(MinOrdinal, 0));
        _ = MathUT.AddYears(MinOrdinal, ys);
        Assert.Overflows(() => MathUT.AddYears(MinOrdinal, ys + 1));
        // OrdinalDate
        Assert.Overflows(() => MinOrdinal.PlusYears(-1));
        Assert.Equal(MinOrdinal, MinOrdinal.PlusYears(0));
        _ = MinOrdinal.PlusYears(ys);
        Assert.Overflows(() => MinOrdinal.PlusYears(ys + 1));
    }

    [Fact]
    public void AddYears﹍OrdinalDate_AtMaxOrdinal()
    {
        int ys = Calendar.YearsValidator.Range.Count() - 1;
        // Act & Assert
        Assert.Overflows(() => MathUT.AddYears(MaxOrdinal, -ys - 1));
        _ = MathUT.AddYears(MaxOrdinal, -ys);
        Assert.Equal(MaxOrdinal, MathUT.AddYears(MaxOrdinal, 0));
        Assert.Overflows(() => MathUT.AddYears(MaxOrdinal, 1));
        // OrdinalDate
        Assert.Overflows(() => MaxOrdinal.PlusYears(-ys - 1));
        _ = MaxOrdinal.PlusYears(-ys);
        Assert.Equal(MaxOrdinal, MaxOrdinal.PlusYears(0));
        Assert.Overflows(() => MaxOrdinal.PlusYears(1));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AddYears﹍OrdinalDate_Zero_IsNeutral(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = Calendar.GetDate(y, doy);
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
    #region CountYearsBetween()

    [Fact]
    public void CountYearsBetween﹍OrdinalDate_DoesNotOverflow()
    {
        int ys = Calendar.YearsValidator.Range.Count() - 1;
        // Act & Assert
        Assert.Equal(ys, MathUT.CountYearsBetween(MinOrdinal, MaxOrdinal, out var newStart));
        Assert.Equal(MinOrdinal.PlusYears(ys), newStart);
        Assert.Equal(-ys, MathUT.CountYearsBetween(MaxOrdinal, MinOrdinal, out newStart));
        Assert.Equal(MaxOrdinal.PlusYears(-ys), newStart);
        // OrdinalDate
        Assert.Equal(ys, MaxOrdinal.CountYearsSince(MinOrdinal));
        Assert.Equal(-ys, MinOrdinal.CountYearsSince(MaxOrdinal));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountYearsBetween﹍OrdinalDate_WhenSame_IsZero(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = Calendar.GetDate(y, doy);
        // Act & Assert
        Assert.Equal(0, MathUT.CountYearsBetween(date, date, out var newStart));
        Assert.Equal(date, newStart);
        // OrdinalDate
        Assert.Equal(0, date.CountYearsSince(date));
    }

    [Theory, MemberData(nameof(CountYearsBetweenOrdinalData))]
    public void CountYearsBetween﹍OrdinalDate(YedoyPairAnd<int> info)
    {
        int ys = info.Value;
        var start = GetDate(info.First);
        var end = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(ys, MathUT.CountYearsBetween(start, end, out var newStart));
        Assert.Equal(start.PlusYears(ys), newStart);
        Assert.Equal(-ys, MathUT.CountYearsBetween(end, start, out newStart));
        Assert.Equal(end.PlusYears(-ys), newStart);
        // OrdinalDate
        Assert.Equal(ys, end.CountYearsSince(start));
        Assert.Equal(-ys, start.CountYearsSince(end));
    }

    #endregion
}

public partial class CalendarMathFacts<TDataSet> // CalendarMonth
{
    //
    // Year unit
    //
    // We also test
    // - CalendarMonth.PlusYears()
    // - CalendarMonth.CountYearsSince()

    #region AddYears()

    [Fact]
    public void AddYears﹍CalendarMonth_Overflows_WithMaxYears()
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
    public void AddYears﹍CalendarMonth_AtMinMonth()
    {
        int ys = Calendar.YearsValidator.Range.Count() - 1;
        // Act & Assert
        Assert.Overflows(() => MathUT.AddYears(MinMonth, -1));
        Assert.Equal(MinMonth, MathUT.AddYears(MinMonth, 0));
        _ = MathUT.AddYears(MinMonth, ys);
        Assert.Overflows(() => MathUT.AddYears(MinMonth, ys + 1));
        // CalendarMonth
        Assert.Overflows(() => MinMonth.PlusYears(-1));
        Assert.Equal(MinMonth, MinMonth.PlusYears(0));
        _ = MinMonth.PlusYears(ys);
        Assert.Overflows(() => MinMonth.PlusYears(ys + 1));
    }

    [Fact]
    public void AddYears﹍CalendarMonth_AtMaxMonth()
    {
        int ys = Calendar.YearsValidator.Range.Count() - 1;
        // Act & Assert
        Assert.Overflows(() => MathUT.AddYears(MaxMonth, -ys - 1));
        _ = MathUT.AddYears(MaxMonth, -ys);
        Assert.Equal(MaxMonth, MathUT.AddYears(MaxMonth, 0));
        Assert.Overflows(() => MathUT.AddYears(MaxMonth, 1));
        // CalendarMonth
        Assert.Overflows(() => MaxMonth.PlusYears(-ys - 1));
        _ = MaxMonth.PlusYears(-ys);
        Assert.Equal(MaxMonth, MaxMonth.PlusYears(0));
        Assert.Overflows(() => MaxMonth.PlusYears(1));
    }

    [Fact]
    public void AddYears﹍CalendarMonth_WithLimitYears()
    {
        var month = GetSampleMonth();
        int minYs = MinMonth.Year - month.Year;
        int maxYs = MaxMonth.Year - month.Year;
        // Act & Assert
        Assert.Overflows(() => MathUT.AddYears(month, minYs - 1));
        _ = MathUT.AddYears(month, minYs);
        _ = MathUT.AddYears(month, maxYs);
        Assert.Overflows(() => MathUT.AddYears(month, maxYs + 1));
        // CalendarMonth
        Assert.Overflows(() => month.PlusYears(minYs - 1));
        _ = month.PlusYears(minYs);
        _ = month.PlusYears(maxYs);
        Assert.Overflows(() => month.PlusYears(maxYs + 1));
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

    [Theory, MemberData(nameof(AddYearsMonthData))]
    public void AddYears﹍CalendarMonth(YemoPairAnd<int> info)
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
    #region CountYearsBetween()

    [Fact]
    public void CountYearsBetween﹍CalendarMonth_DoesNotOverflow()
    {
        int ys = Calendar.YearsValidator.Range.Count() - 1;
        // Act & Assert
        Assert.Equal(ys, MathUT.CountYearsBetween(MinMonth, MaxMonth, out var newStart));
        Assert.Equal(MinMonth.PlusYears(ys), newStart);
        Assert.Equal(-ys, MathUT.CountYearsBetween(MaxMonth, MinMonth, out newStart));
        Assert.Equal(MaxMonth.PlusYears(-ys), newStart);
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
        Assert.Equal(0, MathUT.CountYearsBetween(month, month, out var newStart));
        Assert.Equal(month, newStart);
        // CalendarMonth
        Assert.Equal(0, month.CountYearsSince(month));
    }

    [Theory, MemberData(nameof(CountYearsBetweenMonthData))]
    public void CountYearsBetween﹍CalendarMonth(YemoPairAnd<int> info)
    {
        int ys = info.Value;
        var start = GetMonth(info.First);
        var end = GetMonth(info.Second);
        // Act & Assert
        Assert.Equal(ys, MathUT.CountYearsBetween(start, end, out var newStart));
        Assert.Equal(start.PlusYears(ys), newStart);
        Assert.Equal(-ys, MathUT.CountYearsBetween(end, start, out newStart));
        Assert.Equal(end.PlusYears(-ys), newStart);
        // CalendarMonth
        Assert.Equal(ys, end.CountYearsSince(start));
        Assert.Equal(-ys, start.CountYearsSince(end));
    }

    #endregion

    //
    // Month (base) unit
    //

    #region AddMonths() & CountMonthsBetween()
    // TODO(fact): move the tests to CalendricalArithmeticFacts.
#if false
#pragma warning disable CS0618 // Type or member is obsolete

    [Fact]
    public void AddMonths﹍CalendarMonth_Overflows_WithMaxMonths()
    {
        var month = Calendar.GetCalendarMonth(1, 1);
        // Act & Assert
        Assert.Overflows(() => MathUT.AddMonths(month, Int32.MinValue));
        Assert.Overflows(() => MathUT.AddMonths(month, Int32.MaxValue));
    }

    [Fact]
    public void AddMonths﹍CalendarMonth_AtMinMonth()
    {
        int ms = MaxMonth - MinMonth;
        // Act & Assert
        Assert.Overflows(() => MathUT.AddMonths(MinMonth, -1));
        Assert.Equal(MinMonth, MathUT.AddMonths(MinMonth, 0));
        Assert.Equal(MaxMonth, MathUT.AddMonths(MinMonth, ms));
        Assert.Overflows(() => MathUT.AddMonths(MinMonth, ms + 1));
    }

    [Fact]
    public void AddMonths﹍CalendarMonth_AtMaxMonth()
    {
        int ms = MaxMonth - MinMonth;
        // Act & Assert
        Assert.Overflows(() => MathUT.AddMonths(MaxMonth, -ms - 1));
        Assert.Equal(MinMonth, MathUT.AddMonths(MaxMonth, -ms));
        Assert.Equal(MaxMonth, MathUT.AddMonths(MaxMonth, 0));
        Assert.Overflows(() => MathUT.AddMonths(MaxMonth, 1));
    }

    [Fact]
    public void AddMonths﹍CalendarMonth_WithLimitMonths()
    {
        var supportedYears = Calendar.YearsValidatorImpl.Range;
        var month = GetSampleMonth();
        int minMs = MinMonth - month;
        int maxMs = MaxMonth - month;
        // Act & Assert
        Assert.Overflows(() => MathUT.AddMonths(month, minMs - 1));
        Assert.Equal(MinMonth, MathUT.AddMonths(month, minMs));
        Assert.Equal(MaxMonth, MathUT.AddMonths(month, maxMs));
        Assert.Overflows(() => MathUT.AddMonths(month, maxMs + 1));
    }

    [Fact]
    public void CountMonthsBetween﹍CalendarMonth_DoesNotOverflow()
    {
        _ = MathUT.CountMonthsBetween(MinMonth, MaxMonth);
        _ = MathUT.CountMonthsBetween(MaxMonth, MinMonth);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void AddMonths﹍CalendarMonth_Zero_IsNeutral(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = Calendar.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(month, MathUT.AddMonths(month, 0));
        Assert.Equal(0, MathUT.CountMonthsBetween(month, month));
    }

    [Theory, MemberData(nameof(AddMonthsMonthData))]
    public void AddMonths﹍CalendarMonth(YemoPairAnd<int> info)
    {
        int ms = info.Value;
        var month = GetMonth(info.First);
        var other = GetMonth(info.Second);
        // Act & Assert
        Assert.Equal(other, MathUT.AddMonths(month, ms));
        Assert.Equal(month, MathUT.AddMonths(other, -ms));

        Assert.Equal(ms, MathUT.CountMonthsBetween(month, other));
        Assert.Equal(-ms, MathUT.CountMonthsBetween(other, month));
    }

#pragma warning restore CS0618
#endif
    #endregion
}