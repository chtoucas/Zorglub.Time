// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

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
    protected CalendarMathFacts(Calendar calendar)
        : this(calendar?.Math ?? throw new ArgumentNullException(nameof(calendar))) { }

    protected CalendarMathFacts(CalendarMath math)
    {
        MathUT = math ?? throw new ArgumentNullException(nameof(math));
        Calendar = math.Calendar;

        (MinDate, MaxDate) = Calendar.MinMaxDate;
        (MinOrdinal, MaxOrdinal) = Calendar.MinMaxOrdinal;
        (MinMonth, MaxMonth) = Calendar.MinMaxMonth;
        (MinYear, MaxYear) = Calendar.MinMaxYear;
    }

    protected CalendarMath MathUT { get; }
    protected Calendar Calendar { get; }

    protected CalendarDate MinDate { get; }
    protected CalendarDate MaxDate { get; }

    protected OrdinalDate MinOrdinal { get; }
    protected OrdinalDate MaxOrdinal { get; }

    protected CalendarMonth MinMonth { get; }
    protected CalendarMonth MaxMonth { get; }

    protected CalendarYear MinYear { get; }
    protected CalendarYear MaxYear { get; }

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

    /// <summary>
    /// We only use this sample year when its value matters (mathops); otherwise
    /// just use the year 1.
    /// </summary>
    private CalendarYear GetSampleYear() => Calendar.GetCalendarYear(1234);
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
        var date = Calendar.GetCalendarDate(1, 1, 1);
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
        int ys = Calendar.SupportedYears.Count() - 1;
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
        int ys = Calendar.SupportedYears.Count() - 1;
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
    #region CountYearsBetween()

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

    [Theory, MemberData(nameof(CountYearsBetweenData))]
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
        var date = Calendar.GetCalendarDate(1, 1, 1);
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
    #region CountMonthsBetween()

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

    [Theory, MemberData(nameof(CountMonthsBetweenData))]
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
        var date = Calendar.GetOrdinalDate(1, 1);
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
        int ys = Calendar.SupportedYears.Count() - 1;
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
        int ys = Calendar.SupportedYears.Count() - 1;
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
    #region CountYearsBetween()

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

    [Theory, MemberData(nameof(CountYearsBetweenOrdinalData))]
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
        int ys = Calendar.SupportedYears.Count() - 1;
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
        int ys = Calendar.SupportedYears.Count() - 1;
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

    [Theory, MemberData(nameof(CountYearsBetweenMonthData))]
    public void CountYearsBetween﹍CalendarMonth(YemoPairAnd<int> info)
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
    // Month (base) unit
    //

    #region AddMonths() & CountMonthsBetween()

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
        var supportedYears = Calendar.SupportedYears;
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

    #endregion
}

public partial class CalendarMathFacts<TDataSet> // CalendarYear
{
    //
    // Year (base) unit
    //
    // We also test
    // - CalendarYear.NextYear()
    // - CalendarYear.PreviousYear()
    // - CalendarYear.PlusYear()
    // - CalendarYear.CountYearsSince()
    // and the related math operators.

    #region NextYear()

    [Fact]
    public void NextYear﹍CalendarYear_Overflows_AtMaxValue()
    {
        var copy = MaxYear;
        // Act & Assert
        Assert.Overflows(() => copy++);
        Assert.Overflows(() => MaxYear.NextYear());
    }

    [Fact]
    public void NextYear﹍CalendarYear()
    {
        var year = GetSampleYear();
        var copy = year;
        var yearAfter = Calendar.GetCalendarYear(year.Year + 1);
        // Act & Assert
        Assert.Equal(yearAfter, ++copy);
        Assert.Equal(yearAfter, year.NextYear());
    }

    #endregion
    #region PreviousYear()

    [Fact]
    public void PreviousYear﹍CalendarYear_Overflows_AtMinValue()
    {
        var copy = MinYear;
        // Act & Assert
        Assert.Overflows(() => copy--);
        Assert.Overflows(() => MinYear.PreviousYear());
    }

    [Fact]
    public void PreviousYear﹍CalendarYear()
    {
        var year = GetSampleYear();
        var copy = year;
        var yearBefore = Calendar.GetCalendarYear(year.Year - 1);
        // Act & Assert
        Assert.Equal(yearBefore, --copy);
        Assert.Equal(yearBefore, year.PreviousYear());
    }

    #endregion

    #region PlusYears() & CountYearsSince()

    [Fact]
    public void PlusYears﹍CalendarYear_Overflows_WithMaxYears()
    {
        var year = Calendar.GetCalendarYear(1);
        // Act & Assert
        Assert.Overflows(() => year + Int32.MinValue);
        Assert.Overflows(() => year + Int32.MaxValue);

        Assert.Overflows(() => year.PlusYears(Int32.MinValue));
        Assert.Overflows(() => year.PlusYears(Int32.MaxValue));
    }

    [Fact]
    public void PlusYears﹍CalendarYear_WithLimitYears()
    {
        var year = GetSampleYear();
        int minYs = MinYear - year;
        int maxYs = MaxYear - year;
        // Act & Assert
        Assert.Overflows(() => year + (minYs - 1));
        Assert.Equal(MinYear, year + minYs);
        Assert.Equal(MaxYear, year + maxYs);
        Assert.Overflows(() => year + (maxYs + 1));

        Assert.Overflows(() => year.PlusYears(minYs - 1));
        Assert.Equal(MinYear, year.PlusYears(minYs));
        Assert.Equal(MaxYear, year.PlusYears(maxYs));
        Assert.Overflows(() => year.PlusYears(maxYs + 1));
    }

    [Fact]
    public void CountYearsSince﹍CalendarYear_DoesNotOverflow()
    {
        int ys = MaxYear.Year - MinYear.Year;
        // Act & Assert
        Assert.Equal(ys, MaxYear - MinYear);
        Assert.Equal(-ys, MinYear - MaxYear);

        Assert.Equal(ys, MaxYear.CountYearsSince(MinYear));
        Assert.Equal(-ys, MinYear.CountYearsSince(MaxYear));
    }

    [Fact]
    public void PlusYears﹍CalendarYear_AtMinYear()
    {
        // We could have written:
        // > int ys = MaxYear - MinYear;
        // but this is CountYearsSince() in disguise and I prefer to stick to
        // basic maths.
        int ys = Calendar.SupportedYears.Count() - 1;
        // Act & Assert
        Assert.Overflows(() => MinYear - 1);
        Assert.Equal(MinYear, MinYear - 0);
        Assert.Equal(MinYear, MinYear + 0);
        Assert.Equal(MaxYear, MinYear + ys);
        Assert.Overflows(() => MinYear + (ys + 1));

        Assert.Overflows(() => MinYear.PlusYears(-1));
        Assert.Equal(MinYear, MinYear.PlusYears(0));
        Assert.Equal(MaxYear, MinYear.PlusYears(ys));
        Assert.Overflows(() => MinYear.PlusYears(ys + 1));
    }

    [Fact]
    public void PlusYears﹍CalendarYear_AtMaxYear()
    {
        // We could have written:
        // > int ys = MaxYear - MinYear;
        // but this is CountYearsSince() in disguise and I prefer to stick to
        // basic maths.
        int ys = Calendar.SupportedYears.Count() - 1;
        // Act & Assert
        Assert.Overflows(() => MaxYear - (ys + 1));
        Assert.Equal(MinYear, MaxYear - ys);
        Assert.Equal(MaxYear, MaxYear - 0);
        Assert.Equal(MaxYear, MaxYear + 0);
        Assert.Overflows(() => MaxYear + 1);

        Assert.Overflows(() => MaxYear.PlusYears(-ys - 1));
        Assert.Equal(MinYear, MaxYear.PlusYears(-ys));
        Assert.Equal(MaxYear, MaxYear.PlusYears(0));
        Assert.Overflows(() => MaxYear.PlusYears(1));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void PlusYears﹍CalendarYear_Zero_IsNeutral(YearInfo info)
    {
        var year = Calendar.GetCalendarYear(info.Year);
        // Act & Assert
        Assert.Equal(year, year + 0);
        Assert.Equal(year, year - 0);
        Assert.Equal(year, year.PlusYears(0));

        Assert.Equal(0, year - year);
        Assert.Equal(0, year.CountYearsSince(year));
    }

    [Fact]
    public void PlusYears﹍CalendarYear()
    {
        // NB: ys is such that "other" is a valid year for both standard and
        // proleptic calendars.
        int ys = 876;
        var year = GetSampleYear();
        var other = Calendar.GetCalendarYear(year.Year + ys);
        // Act & Assert
        Assert.Equal(other, year + ys);
        Assert.Equal(other, year - (-ys));
        Assert.Equal(year, other - ys);
        Assert.Equal(year, other + (-ys));

        Assert.Equal(other, year.PlusYears(ys));
        Assert.Equal(year, other.PlusYears(-ys));

        Assert.Equal(ys, other - year);
        Assert.Equal(-ys, year - other);

        Assert.Equal(ys, other.CountYearsSince(year));
        Assert.Equal(-ys, year.CountYearsSince(other));
    }

    #endregion
}
