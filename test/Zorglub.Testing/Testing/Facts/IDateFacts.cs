// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts;

// Hypothesis:
// - TDate is a value type.
// - First year is valid and complete.

using Zorglub.Testing.Data;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides facts about <see cref="IDate{TSelf}"/>.
/// </summary>
public abstract partial class IDateFacts<TDate, TDataSet> : CalendarTesting<TDataSet>
    where TDate : struct, IDate<TDate>
    where TDataSet :
        ICalendarDataSet,
        IDaysAfterDataSet,
        IMathDataSet,
        IDayOfWeekDataSet,
        ISingleton<TDataSet>
{
    protected IDateFacts(CtorArgs args) : base(args) { }

    protected abstract TDate MinDate { get; }
    protected abstract TDate MaxDate { get; }

    protected abstract TDate CreateDate(int y, int m, int d);

    protected TDate CreateDate(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return CreateDate(y, m, d);
    }

    // REVIEW(fact): do we still need these indirect methods.
    protected TDate Op_Subtraction(TDate date, int days) => date + (-days);
    protected TDate Op_Increment(TDate date) { date++; return date; }
    protected TDate Op_Decrement(TDate date) { date--; return date; }

    #region Static access to IDaysAfterDataSet

    public static TheoryData<YemodaAnd<int>> DaysInYearAfterDateData => DataSet.DaysInYearAfterDateData;
    public static TheoryData<YemodaAnd<int>> DaysInMonthAfterDateData => DataSet.DaysInMonthAfterDateData;

    #endregion
    #region Static access to IMathDataSet

    public static TheoryData<Yemoda, Yemoda, int> AddDaysData => DataSet.AddDaysData;
    public static TheoryData<Yemoda, Yemoda> ConsecutiveDaysData => DataSet.ConsecutiveDaysData;

    #endregion
    #region Static access to IDayOfWeekDataSet

    public static TheoryData<YemodaAnd<DayOfWeek>> DayOfWeekData => DataSet.DayOfWeekData;

    public static TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_Before_Data => DataSet.DayOfWeek_Before_Data;
    public static TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_OnOrBefore_Data => DataSet.DayOfWeek_OnOrBefore_Data;
    public static TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_Nearest_Data => DataSet.DayOfWeek_Nearest_Data;
    public static TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_OnOrAfter_Data => DataSet.DayOfWeek_OnOrAfter_Data;
    public static TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_After_Data => DataSet.DayOfWeek_After_Data;

    #endregion
}

public partial class IDateFacts<TDate, TDataSet> // Prelude
{
    // TODO(fact): for simple date objects the constructor is tested in
    // CalendarFacts (Factories). For the others, copy this code: CivilDate
    // and WideDate.

    //[Fact]
    //public void Constructor_InvalidYear() =>
    //    SupportedYearsTester.TestInvalidYear(y => CreateDate(y, 1, 1));

    //[Theory, MemberData(nameof(InvalidMonthFieldData))]
    //public void Constructor_InvalidMonth(int y, int m) =>
    //    Assert.ThrowsAoorexn("month", () => CreateDate(y, m, 1));

    //[Theory, MemberData(nameof(InvalidDayFieldData))]
    //public void Constructor_InvalidDay(int y, int m, int d) =>
    //    Assert.ThrowsAoorexn("day", () => CreateDate(y, m, d));

    //[Theory, MemberData(nameof(DateInfoData))]
    //public void Constructor(DateInfo info)
    //{
    //    var (y, m, d) = info.Yemoda;
    //    // Act
    //    var date = CreateDate(y, m, d);
    //    // Assert
    //    Assert.Equal(y, date.Year);
    //    Assert.Equal(m, date.Month);
    //    Assert.Equal(d, date.Day);
    //}

    //
    // Properties
    //

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void CenturyOfEra_Prop(CenturyInfo info)
    {
        var (y, century, _) = info;
        var date = CreateDate(y, 1, 1);
        var centuryOfEra = Ord.Zeroth + century;
        // Act & Assert
        Assert.Equal(centuryOfEra, date.CenturyOfEra);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void Century_Prop(CenturyInfo info)
    {
        var (y, century, _) = info;
        var date = CreateDate(y, 1, 1);
        // Act & Assert
        Assert.Equal(century, date.Century);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void YearOfEra_Prop(CenturyInfo info)
    {
        int y = info.Year;
        var date = CreateDate(y, 1, 1);
        var yearOfEra = Ord.Zeroth + y;
        // Act & Assert
        Assert.Equal(yearOfEra, date.YearOfEra);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void YearOfCentury_Prop(CenturyInfo info)
    {
        var (y, _, yearOfCentury) = info;
        var date = CreateDate(y, 1, 1);
        // Act & Assert
        Assert.Equal(yearOfCentury, date.YearOfCentury);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void DayOfYear_Prop(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var date = CreateDate(y, m, d);
        // Act & Assert
        Assert.Equal(doy, date.DayOfYear);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void IsIntercalary_Prop(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CreateDate(y, m, d);
        // Act & Assert
        Assert.Equal(info.IsIntercalary, date.IsIntercalary);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void IsSupplementary_Prop(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CreateDate(y, m, d);
        // Act & Assert
        Assert.Equal(info.IsSupplementary, date.IsSupplementary);
    }

    [Theory, MemberData(nameof(DayOfWeekData))]
    public void DayOfWeek_Prop(YemodaAnd<DayOfWeek> info)
    {
        var (y, m, d, dayOfWeek) = info;
        var date = CreateDate(y, m, d);
        // Act & Assert
        Assert.Equal(dayOfWeek, date.DayOfWeek);
    }

    [Theory, MemberData(nameof(CalCalDataSet.DayOfWeekData), MemberType = typeof(CalCalDataSet))]
    public void DayOfWeek_Prop_ViaDayNumber(DayNumber dayNumber, DayOfWeek dayOfWeek)
    {
        if (Domain.Contains(dayNumber) == false) { return; }
        var date = TDate.FromDayNumber(dayNumber);
        // Act & Assert
        Assert.Equal(dayOfWeek, date.DayOfWeek);
    }
}

public partial class IDateFacts<TDate, TDataSet> // Conversions
{
    [Fact]
    public void FromDayNumber_InvalidDayNumber() =>
        DomainTester.TestInvalidDayNumber(TDate.FromDayNumber);

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void FromDayNumber(DayNumberInfo info)
    {
        var (dayNumber, y, m, d) = info;
        // Act
        var date = TDate.FromDayNumber(dayNumber);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
    }

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void ToDayNumber(DayNumberInfo info)
    {
        var (dayNumber, y, m, d) = info;
        var date = CreateDate(y, m, d);
        // Act & Assert
        Assert.Equal(dayNumber, date.ToDayNumber());
    }
}

public partial class IDateFacts<TDate, TDataSet> // Counting
{
    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountElapsedDaysInYear_ViaDaysInYearBeforeMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int daysInYearBeforeMonth = info.DaysInYearBeforeMonth;
        // Act
        var date1 = CreateDate(y, m, 1);
        var date2 = CreateDate(y, m, 2);
        var date3 = CreateDate(y, m, 3);
        var date4 = CreateDate(y, m, 4);
        var date5 = CreateDate(y, m, 5);
        // We can't go further because of the schemas w/ a virtual thirteen month.
        // Assert
        Assert.Equal(daysInYearBeforeMonth, date1.CountElapsedDaysInYear());
        Assert.Equal(daysInYearBeforeMonth + 1, date2.CountElapsedDaysInYear());
        Assert.Equal(daysInYearBeforeMonth + 2, date3.CountElapsedDaysInYear());
        Assert.Equal(daysInYearBeforeMonth + 3, date4.CountElapsedDaysInYear());
        Assert.Equal(daysInYearBeforeMonth + 4, date5.CountElapsedDaysInYear());
    }

    [Theory, MemberData(nameof(DaysInYearAfterDateData))]
    public void CountRemainingDaysInYear(YemodaAnd<int> info)
    {
        var (y, m, d, days) = info;
        var date = CreateDate(y, m, d);
        // Act & Assert
        Assert.Equal(days, date.CountRemainingDaysInYear());
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountElapsedDaysInMonth(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CreateDate(y, m, d);
        // Act & Assert
        Assert.Equal(d - 1, date.CountElapsedDaysInMonth());
    }

    [Theory, MemberData(nameof(DaysInMonthAfterDateData))]
    public void CountRemainingDaysInMonth(YemodaAnd<int> info)
    {
        var (y, m, d, days) = info;
        var date = CreateDate(y, m, d);
        // Act & Assert
        Assert.Equal(days, date.CountRemainingDaysInMonth());
    }
}

public partial class IDateFacts<TDate, TDataSet> // Year and month boundaries
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void GetStartOfYear(YearInfo info)
    {
        int y = info.Year;
        var date = CreateDate(y, 4, 5);
        var startOfYear = CreateDate(y, 1, 1);
        // Act & Assert
        Assert.Equal(startOfYear, TDate.GetStartOfYear(date));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetEndOfYear(YearInfo info)
    {
        int y = info.Year;
        var date = CreateDate(y, 4, 5);
        // Act
        var actual = TDate.GetEndOfYear(date);
        // Assert
        Assert.Equal(y, actual.Year);
        Assert.Equal(info.DaysInYear, actual.DayOfYear);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var date = CreateDate(y, m, 5);
        var startOfMonth = CreateDate(y, m, 1);
        // Act & Assert
        Assert.Equal(startOfMonth, TDate.GetStartOfMonth(date));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var date = CreateDate(y, m, 5);
        var endOfMonth = CreateDate(y, m, info.DaysInMonth);
        // Act & Assert
        Assert.Equal(endOfMonth, TDate.GetEndOfMonth(date));
    }
}

public partial class IDateFacts<TDate, TDataSet> // Adjust the day of the week
{
    #region Arg check

    [Theory, MemberData(nameof(EnumDataSet.InvalidDayOfWeekData), MemberType = typeof(EnumDataSet))]
    public void Previous_InvalidDayOfWeek(DayOfWeek dayOfWeek)
    {
        var date = CreateDate(1, 1, 1);
        // Act & Assert
        Assert.ThrowsAoorexn("dayOfWeek", () => date.Previous(dayOfWeek));
    }

    [Theory, MemberData(nameof(EnumDataSet.InvalidDayOfWeekData), MemberType = typeof(EnumDataSet))]
    public void PreviousOrSame_InvalidDayOfWeek(DayOfWeek dayOfWeek)
    {
        var date = CreateDate(1, 1, 1);
        // Act & Assert
        Assert.ThrowsAoorexn("dayOfWeek", () => date.PreviousOrSame(dayOfWeek));
    }

    [Theory, MemberData(nameof(EnumDataSet.InvalidDayOfWeekData), MemberType = typeof(EnumDataSet))]
    public void Nearest_InvalidDayOfWeek(DayOfWeek dayOfWeek)
    {
        var date = CreateDate(1, 1, 1);
        // Act & Assert
        Assert.ThrowsAoorexn("dayOfWeek", () => date.Nearest(dayOfWeek));
    }

    [Theory, MemberData(nameof(EnumDataSet.InvalidDayOfWeekData), MemberType = typeof(EnumDataSet))]
    public void NextOrSame_InvalidDayOfWeek(DayOfWeek dayOfWeek)
    {
        var date = CreateDate(1, 1, 1);
        // Act & Assert
        Assert.ThrowsAoorexn("dayOfWeek", () => date.NextOrSame(dayOfWeek));
    }

    [Theory, MemberData(nameof(EnumDataSet.InvalidDayOfWeekData), MemberType = typeof(EnumDataSet))]
    public void Next_InvalidDayOfWeek(DayOfWeek dayOfWeek)
    {
        var date = CreateDate(1, 1, 1);
        // Act & Assert
        Assert.ThrowsAoorexn("dayOfWeek", () => date.Next(dayOfWeek));
    }

    #endregion

    [Fact]
    public void Previous_NearMinValue() =>
        DayOfWeekAdjusterTester.NearMinValue(MinDate).TestPrevious();

    [Fact]
    public void PreviousOrSame_NearMinValue() =>
        DayOfWeekAdjusterTester.NearMinValue(MinDate).TestPreviousOrSame();

    [Fact]
    public void NextOrSame_NearMaxValue() =>
        DayOfWeekAdjusterTester.NearMaxValue(MaxDate).TestNextOrSame();

    [Fact]
    public void Next_NearMaxValue() =>
        DayOfWeekAdjusterTester.NearMaxValue(MaxDate).TestNext();

    [Theory, MemberData(nameof(DayOfWeek_Before_Data))]
    public void Previous(Yemoda xdate, Yemoda xexp, DayOfWeek dayOfWeek)
    {
        var date = CreateDate(xdate);
        var exp = CreateDate(xexp);
        // Act
        var actual = date.Previous(dayOfWeek);
        // Assert
        Assert.Equal(exp, actual);
    }

    [Theory, MemberData(nameof(DayOfWeek_OnOrBefore_Data))]
    public void PreviousOrSame(Yemoda xdate, Yemoda xexp, DayOfWeek dayOfWeek)
    {
        var date = CreateDate(xdate);
        var exp = CreateDate(xexp);
        // Act
        var actual = date.PreviousOrSame(dayOfWeek);
        // Assert
        Assert.Equal(exp, actual);
    }

    [Theory, MemberData(nameof(DayOfWeek_Nearest_Data))]
    public void Nearest(Yemoda xdate, Yemoda xexp, DayOfWeek dayOfWeek)
    {
        var date = CreateDate(xdate);
        var exp = CreateDate(xexp);
        // Act
        var actual = date.Nearest(dayOfWeek);
        // Assert
        Assert.Equal(exp, actual);
    }

    [Theory, MemberData(nameof(DayOfWeek_OnOrAfter_Data))]
    public void NextOrSame(Yemoda xdate, Yemoda xexp, DayOfWeek dayOfWeek)
    {
        var date = CreateDate(xdate);
        var exp = CreateDate(xexp);
        // Act
        var actual = date.NextOrSame(dayOfWeek);
        // Assert
        Assert.Equal(exp, actual);
    }

    [Theory, MemberData(nameof(DayOfWeek_After_Data))]
    public void Next(Yemoda xdate, Yemoda xexp, DayOfWeek dayOfWeek)
    {
        var date = CreateDate(xdate);
        var exp = CreateDate(xexp);
        // Act
        var actual = date.Next(dayOfWeek);
        // Assert
        Assert.Equal(exp, actual);
    }
}

public partial class IDateFacts<TDate, TDataSet> // Addition / Subtraction
{
    [Fact]
    public void PlusDays_WithLimitValues()
    {
        var (minDayNumber, maxDayNumber) = Domain.Endpoints;
        var date = CreateDate(1, 1, 1);
        var dayNumber = date.ToDayNumber();
        int minDays = minDayNumber - dayNumber;
        int maxDays = maxDayNumber - dayNumber;
        // Act & Assert
        Assert.Overflows(() => date + (minDays - 1));
        Assert.Overflows(() => date.PlusDays(minDays - 1));

        Assert.Equal(MinDate, date + minDays);
        Assert.Equal(MinDate, date.PlusDays(minDays));

        Assert.Equal(MaxDate, date + maxDays);
        Assert.Equal(MaxDate, date.PlusDays(maxDays));

        Assert.Overflows(() => date + (maxDays + 1));
        Assert.Overflows(() => date.PlusDays(maxDays + 1));
    }

    [Fact]
    public void CountDaysSince_DoesNotOverflow()
    {
        int days = Domain.Count() - 1;
        // Act & Assert
        Assert.Equal(days, MaxDate - MinDate);
        Assert.Equal(days, MaxDate.CountDaysSince(MinDate));

        Assert.Equal(-days, MinDate - MaxDate);
        Assert.Equal(-days, MinDate.CountDaysSince(MaxDate));
    }

    [Theory, MemberData(nameof(AddDaysData))]
    public void CountDaysSince(Yemoda xdate, Yemoda xother, int days)
    {
        var date = CreateDate(xdate);
        var other = CreateDate(xother);
        // Act & Assert
        // 3) other - date -> days.
        Assert.Equal(days, other - date);
        Assert.Equal(days, other.CountDaysSince(date));

        // 4) date - other -> -days.
        Assert.Equal(-days, date - other);
        Assert.Equal(-days, date.CountDaysSince(other));
    }

    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void CountDaysSince_ViaConsecutiveDays(Yemoda xdate, Yemoda xdateAfter)
    {
        var date = CreateDate(xdate);
        var dateAfter = CreateDate(xdateAfter);
        // Act & Assert
        // 3) dateAfter - date -> 1.
        Assert.Equal(1, dateAfter - date);
        Assert.Equal(1, dateAfter.CountDaysSince(date));

        // 4) date - dateAfter -> -1.
        Assert.Equal(-1, date - dateAfter);
        Assert.Equal(-1, date.CountDaysSince(dateAfter));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountDaysSince_Same_IsZero(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CreateDate(y, m, d);
        // Act & Assert
        Assert.Equal(0, date - date);
        Assert.Equal(0, date.CountDaysSince(date));
    }
}

// NextDay(), PreviousDay(), op_Increment, op_Decrement.
public partial class IDateFacts<TDate, TDataSet> // Increment / decrement
{
    [Fact]
    public void Increment_Overflows_AtMaxValue() =>
        Assert.Overflows(() => Op_Increment(MaxDate));

    [Fact]
    public void Decrement_Underflows_AtMinValue() =>
        Assert.Overflows(() => Op_Decrement(MinDate));

    [Fact]
    public void NextDay_Overflows_AtMaxValue() =>
        Assert.Overflows(() => MaxDate.NextDay());

    [Fact]
    public void PreviousDay_Underflows_AtMinValue() =>
        Assert.Overflows(() => MinDate.PreviousDay());

    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void Increment(Yemoda xdate, Yemoda xdateAfter)
    {
        var date = CreateDate(xdate);
        var dateAfter = CreateDate(xdateAfter);
        // Act & Assert
        Assert.Equal(dateAfter, Op_Increment(date));
    }

    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void Decrement(Yemoda xdate, Yemoda xdateAfter)
    {
        var date = CreateDate(xdate);
        var dateAfter = CreateDate(xdateAfter);
        // Act & Assert
        Assert.Equal(date, Op_Decrement(dateAfter));
    }

    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void NextDay(Yemoda xdate, Yemoda xdateAfter)
    {
        var date = CreateDate(xdate);
        var dateAfter = CreateDate(xdateAfter);
        // Act & Assert
        Assert.Equal(dateAfter, date.NextDay());
    }

    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void PreviousDay(Yemoda xdate, Yemoda xdateAfter)
    {
        var date = CreateDate(xdate);
        var dateAfter = CreateDate(xdateAfter);
        // Act & Assert
        Assert.Equal(date, dateAfter.PreviousDay());
    }
}

// PlusDays(), CountDaysSince(), op_Addition, op_Subtraction.
//
// Expected algebraic properties.
// The following properties should be equivalent:
//   1) d + days = d'
//   2) d' - days = d
//   3) d' - d = days
//   4) d - d' = -days
// Other properties: 0 is neutral.
//   5) d + 0 = d
//   6) d - 0 = d
//   7) d - d = 0
public partial class IDateFacts<TDate, TDataSet> // Addition
{
    [Fact]
    public void PlusDays_OverflowOrUnderflow()
    {
        var date = CreateDate(1, 1, 1);
        // Act & Assert
        Assert.Overflows(() => date.PlusDays(Int32.MinValue));
        Assert.Overflows(() => date + Int32.MinValue);

        Assert.Overflows(() => date.PlusDays(Int32.MaxValue));
        Assert.Overflows(() => date + Int32.MaxValue);
    }

    [Fact]
    public void PlusDays_WithLimitValues_AtMinValue()
    {
        int days = Domain.Count() - 1;
        // Act & Assert
        Assert.Overflows(() => Op_Subtraction(MinDate, 1));
        Assert.Overflows(() => MinDate.PlusDays(-1));

        Assert.Equal(MinDate, Op_Subtraction(MinDate, 0));
        Assert.Equal(MinDate, MinDate + 0);
        Assert.Equal(MinDate, MinDate.PlusDays(0));

        Assert.Equal(MaxDate, MinDate + days);
        Assert.Equal(MaxDate, MinDate.PlusDays(days));

        Assert.Overflows(() => MinDate + (days + 1));
        Assert.Overflows(() => MinDate.PlusDays(days + 1));
    }

    [Fact]
    public void PlusDays_WithLimitValues_AtMaxValue()
    {
        int days = Domain.Count() - 1;
        // Act & Assert
        Assert.Overflows(() => Op_Subtraction(MaxDate, days + 1));
        Assert.Overflows(() => MaxDate.PlusDays(-days - 1));

        Assert.Equal(MinDate, Op_Subtraction(MaxDate, days));
        Assert.Equal(MinDate, MaxDate.PlusDays(-days));

        Assert.Equal(MaxDate, Op_Subtraction(MaxDate, 0));
        Assert.Equal(MaxDate, MaxDate + 0);
        Assert.Equal(MaxDate, MaxDate.PlusDays(0));

        Assert.Overflows(() => MaxDate + 1);
        Assert.Overflows(() => MaxDate.PlusDays(1));
    }

    [Theory, MemberData(nameof(AddDaysData))]
    public void PlusDays(Yemoda xdate, Yemoda xother, int days)
    {
        var date = CreateDate(xdate);
        var other = CreateDate(xother);
        // Act & Assert
        // 1) date + days -> other.
        Assert.Equal(other, date + days);
        Assert.Equal(other, date.PlusDays(days));

        // 2) other - days -> date.
        Assert.Equal(date, Op_Subtraction(other, days));
        Assert.Equal(date, other.PlusDays(-days));
    }

    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void PlusDays_ViaConsecutiveDays(Yemoda xdate, Yemoda xdateAfter)
    {
        var date = CreateDate(xdate);
        var dateAfter = CreateDate(xdateAfter);
        // Act & Assert
        // 1) date + 1 -> dateAfter.
        Assert.Equal(dateAfter, date + 1);
        Assert.Equal(dateAfter, date.PlusDays(1));

        // 2) dateAfter - 1 -> date.
        Assert.Equal(date, Op_Subtraction(dateAfter, 1));
        Assert.Equal(date, dateAfter.PlusDays(-1));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void PlusDays_Zero_IsNeutral(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CreateDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, date + 0);
        Assert.Equal(date, Op_Subtraction(date, 0));
        Assert.Equal(date, date.PlusDays(0));
    }
}

// Expected algebraic properties.
//   1) Reflexivity
//   2) Symmetry
//   3) Transitivity
public partial class IDateFacts<TDate, TDataSet> // IEquatable
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void Equals_WhenSame(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CreateDate(y, m, d);
        var same = CreateDate(y, m, d);
        // Act & Assert
        Assert.True(date == same);
        Assert.False(date != same);

        Assert.True(date.Equals(same));
        Assert.True(date.Equals((object)same));
    }

    [Theory]
    [InlineData(2, 1, 1)]
    [InlineData(1, 2, 1)]
    [InlineData(1, 1, 2)]
    public void Equals_WhenNotSame(int y, int m, int d)
    {
        var date = CreateDate(1, 1, 1);
        var notSame = CreateDate(y, m, d);
        // Act & Assert
        Assert.False(date == notSame);
        Assert.True(date != notSame);

        Assert.False(date.Equals(notSame));
        Assert.False(date.Equals((object)notSame));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Equals_NullOrOtherType(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CreateDate(y, m, d);
        // Act & Assert
        Assert.False(date.Equals(1));
        Assert.False(date.Equals(null));
        Assert.False(date.Equals(new object()));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetHashCode_Repeated(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CreateDate(y, m, d);
        var obj = (object)date;
        // Act & Assert
        Assert.Equal(date.GetHashCode(), date.GetHashCode());
        Assert.Equal(date.GetHashCode(), obj.GetHashCode());
    }
}

// Expected algebraic properties.
//   1) Reflexivity
//   2) Anti-symmetry
//   3) Transitivity
public partial class IDateFacts<TDate, TDataSet> // IComparable
{
    [Fact]
    public void CompareTo_Null()
    {
        var date = CreateDate(1, 1, 1);
        var comparable = (IComparable)date;
        // Act & Assert
        Assert.Equal(1, comparable.CompareTo(null));
    }

    [Fact]
    public void CompareTo_InvalidObject()
    {
        var date = CreateDate(1, 1, 1);
        var comparable = (IComparable)date;
        object other = new();
        // Act & Assert
        Assert.Throws<ArgumentException>("obj", () => comparable.CompareTo(other));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CompareTo_WhenEqual(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var left = CreateDate(y, m, d);
        var right = CreateDate(y, m, d);
        // Act & Assert
        Assert.False(left > right);
        Assert.True(left >= right);
        Assert.True(left <= right);
        Assert.False(left < right);

        Assert.Equal(0, left.CompareTo(right));
        Assert.Equal(0, left.CompareTo((object)right));
    }

    [Theory]
    [InlineData(2, 1, 1)]
    [InlineData(1, 2, 1)]
    [InlineData(1, 1, 2)]
    public void CompareTo_WhenNotEqual(int y, int m, int d)
    {
        var left = CreateDate(1, 1, 1);
        var right = CreateDate(y, m, d);
        // Act & Assert
        Assert.False(left > right);
        Assert.False(left >= right);
        Assert.True(left <= right);
        Assert.True(left < right);

        Assert.True(left.CompareTo(right) < 0);
        Assert.True(left.CompareTo((object)right) < 0);
    }

    [Theory]
    [InlineData(2, 1, 1)]
    [InlineData(1, 2, 1)]
    [InlineData(1, 1, 2)]
    public void Min(int y, int m, int d)
    {
        var min = CreateDate(1, 1, 1);
        var max = CreateDate(y, m, d);
        // Act & Assert
        Assert.Equal(min, TDate.Min(min, max));
        Assert.Equal(min, TDate.Min(max, min));
    }

    [Theory]
    [InlineData(2, 1, 1)]
    [InlineData(1, 2, 1)]
    [InlineData(1, 1, 2)]
    public void Max(int y, int m, int d)
    {
        var min = CreateDate(1, 1, 1);
        var max = CreateDate(y, m, d);
        // Act & Assert
        Assert.Equal(max, TDate.Max(min, max));
        Assert.Equal(max, TDate.Max(max, min));
    }
}
