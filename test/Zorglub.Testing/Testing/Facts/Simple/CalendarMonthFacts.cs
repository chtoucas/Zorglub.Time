// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using System.Linq;

using Zorglub.Testing.Data;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Simple;

// NB: we know that years between 1 to 9999 are valid.

public abstract partial class CalendarMonthFacts<TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarMonthFacts(Calendar calendar, Calendar otherCalendar)
    {
        Requires.NotNull(calendar);
        Requires.NotNull(otherCalendar);
        // NB: calendars of type Calendar are singletons.
        if (ReferenceEquals(otherCalendar, calendar))
        {
            throw new ArgumentException(
                "\"otherCalendar\" MUST NOT be equal to \"calendar\"", nameof(otherCalendar));
        }

        CalendarUT = calendar;
        OtherCalendar = otherCalendar;

        SupportedYearsTester = new SupportedYearsTester(calendar.SupportedYears);

        (MinValue, MaxValue) = calendar.MinMaxMonth;
    }

    protected Calendar CalendarUT { get; }
    protected Calendar OtherCalendar { get; }

    protected SupportedYearsTester SupportedYearsTester { get; }

    protected CalendarMonth MinValue { get; }
    protected CalendarMonth MaxValue { get; }

    /// <summary>
    /// We only use this sample year when its value matters (mathops); otherwise
    /// just use the first month of the year 1. It is initialized to ensure that
    /// the math operations we are going to perform will work.
    /// </summary>
    protected CalendarMonth GetSampleValue() => CalendarUT.GetCalendarMonth(1234, 2);
}

public partial class CalendarMonthFacts<TDataSet> // Prelude
{
    [Theory, MemberData(nameof(MonthInfoData))]
    public void Deconstructor(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        // Act
        var (yA, mA) = month;
        // Assert
        Assert.Equal(y, yA);
        Assert.Equal(m, mA);
    }

    //
    // Properties
    //

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void CenturyOfEra_Prop(CenturyInfo info)
    {
        var (y, century, _) = info;
        var month = CalendarUT.GetCalendarMonth(y, 1);
        var centuryOfEra = Ord.Zeroth + century;
        // Act & Assert
        Assert.Equal(centuryOfEra, month.CenturyOfEra);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void Century_Prop(CenturyInfo info)
    {
        var (y, century, _) = info;
        var month = CalendarUT.GetCalendarMonth(y, 1);
        // Act & Assert
        Assert.Equal(century, month.Century);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void YearOfEra_Prop(CenturyInfo info)
    {
        int y = info.Year;
        var month = CalendarUT.GetCalendarMonth(y, 1);
        var yearOfEra = Ord.Zeroth + y;
        // Act & Assert
        Assert.Equal(yearOfEra, month.YearOfEra);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void YearOfCentury_Prop(CenturyInfo info)
    {
        var (y, _, yearOfCentury) = info;
        var month = CalendarUT.GetCalendarMonth(y, 1);
        // Act & Assert
        Assert.Equal(yearOfCentury, month.YearOfCentury);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void IsIntercalary_Prop(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Act
        var month = CalendarUT.GetCalendarMonth(y, m);
        // Assert
        Assert.Equal(info.IsIntercalary, month.IsIntercalary);
    }

    [Fact]
    public void Calendar_Prop()
    {
        var month = CalendarUT.GetCalendarMonth(1, 1);
        // Act & Assert
        Assert.Equal(CalendarUT, month.Calendar);
        // We also test the internal prop Cuid.
        Assert.Equal(CalendarUT.Id, month.Cuid);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CalendarYear_Prop(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var exp = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.Equal(exp, month.CalendarYear);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void FirstDay_Prop(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var startOfMonth = CalendarUT.GetCalendarDate(y, m, 1);
        // Act & Assert
        Assert.Equal(startOfMonth, month.FirstDay);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void LastDay_Prop(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var daysInMonth = info.DaysInMonth;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var endOfMonth = CalendarUT.GetCalendarDate(y, m, daysInMonth);
        // Act & Assert
        Assert.Equal(endOfMonth, month.LastDay);
    }
}

public partial class CalendarMonthFacts<TDataSet> // Calendar mismatch
{
    [Fact]
    public void Equality_OtherCalendar()
    {
        var month = CalendarUT.GetCalendarMonth(1, 1);
        var other = OtherCalendar.GetCalendarMonth(1, 1);
        // Act & Assert
        Assert.False(month == other);
        Assert.True(month != other);

        Assert.False(month.Equals(other));
        Assert.False(month.Equals((object)other));
    }

    [Fact]
    public void Comparison_OtherCalendar()
    {
        var month = CalendarUT.GetCalendarMonth(1, 1);
        var other = OtherCalendar.GetCalendarMonth(1, 1);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => month > other);
        Assert.Throws<ArgumentException>("other", () => month >= other);
        Assert.Throws<ArgumentException>("other", () => month < other);
        Assert.Throws<ArgumentException>("other", () => month <= other);

        Assert.Throws<ArgumentException>("other", () => month.CompareTo(other));
    }

    [Fact]
    public void CountMonthsSince_OtherCalendar()
    {
        var month = CalendarUT.GetCalendarMonth(1, 1);
        var other = OtherCalendar.GetCalendarMonth(1, 1);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => month.CountMonthsSince(other));
        Assert.Throws<ArgumentException>("other", () => month - other);
    }

    [Fact]
    public void CountYearsSince_OtherCalendar()
    {
        var month = CalendarUT.GetCalendarMonth(1, 1);
        var other = OtherCalendar.GetCalendarMonth(1, 1);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => month.CountYearsSince(other));
    }
}

public partial class CalendarMonthFacts<TDataSet> // Serialization
{
    [Theory, MemberData(nameof(MonthInfoData))]
    public void Serialization_Roundtrip(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(month, CalendarMonth.FromBinary(month.ToBinary()));
    }
}

public partial class CalendarMonthFacts<TDataSet> // Counting
{
    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountElapsedDaysInYear(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        // Act
        int actual = month.CountElapsedDaysInYear();
        // Assert
        Assert.Equal(info.DaysInYearBeforeMonth, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountRemainingDaysInYear(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        // Act
        int actual = month.CountRemainingDaysInYear();
        //
        Assert.Equal(info.DaysInYearAfterMonth, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        // Act
        int actual = month.CountDaysInMonth();
        // Assert
        Assert.Equal(info.DaysInMonth, actual);
    }
}

public partial class CalendarMonthFacts<TDataSet> // Days
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void GetDayOfMonth(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var date = CalendarUT.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, month.GetDayOfMonth(d));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetDaysInMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var list = from i in Enumerable.Range(1, info.DaysInMonth)
                   select CalendarUT.GetCalendarDate(y, m, i);
        // Act
        var actual = month.GetDaysInMonth();
        // Assert
        Assert.Equal(list, actual);
    }
}

public partial class CalendarMonthFacts<TDataSet> // Adjustments
{
    #region Year adjustment

    [Theory, MemberData(nameof(MonthInfoData))]
    public void WithYear_InvalidYears(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        SupportedYearsTester.TestInvalidYear(month.WithYear, "newYear");
    }

    [Fact]
    public void WithYear_ValidYears()
    {
        foreach (int y in SupportedYearsTester.ValidYears)
        {
            var month = CalendarUT.GetCalendarMonth(1, 1);
            var exp = CalendarUT.GetCalendarMonth(y, 1);
            // Act & Assert
            Assert.Equal(exp, month.WithYear(y));
        }
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void WithYear_Invariant(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(month, month.WithYear(y));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void WithYear(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(1, m);
        var exp = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(exp, month.WithYear(y));
    }

    #endregion
    #region Month adjustment

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void WithMonthOfYear_InvalidMonth(int y, int newMonth)
    {
        var month = CalendarUT.GetCalendarMonth(y, 1);
        // Act & Assert
        Assert.ThrowsAoorexn("newMonth", () => month.WithMonthOfYear(newMonth));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void WithMonthOfYear_Invariant(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(month, month.WithMonthOfYear(m));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void WithMonthOfYear(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, 1);
        var exp = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(exp, month.WithMonthOfYear(m));
    }

    #endregion
}

public partial class CalendarMonthFacts<TDataSet> // IEquatable
{
    [Theory, MemberData(nameof(MonthInfoData))]
    public void Equals_WhenSame(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var same = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.True(month == same);
        Assert.False(month != same);

        Assert.True(month.Equals(same));
        Assert.True(month.Equals((object)same));
    }

    [Theory]
    [InlineData(2, 1)]
    [InlineData(1, 2)]
    public void Equals_WhenNotSame(int y, int m)
    {
        var month = CalendarUT.GetCalendarMonth(1, 1);
        var notSame = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.False(month == notSame);
        Assert.True(month != notSame);

        Assert.False(month.Equals(notSame));
        Assert.False(month.Equals((object)notSame));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void Equals_NullOrPlainObject(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.False(month.Equals(1));
        Assert.False(month.Equals(null));
        Assert.False(month.Equals(new object()));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetHashCode_Repeated(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var obj = (object)month;
        // Act & Assert
        Assert.Equal(month.GetHashCode(), month.GetHashCode());
        Assert.Equal(month.GetHashCode(), obj.GetHashCode());
    }
}

public partial class CalendarMonthFacts<TDataSet> // IComparable
{
    [Fact]
    public void CompareTo_Null()
    {
        var month = CalendarUT.GetCalendarMonth(1, 1);
        var comparable = (IComparable)month;
        // Act & Assert
        Assert.Equal(1, comparable.CompareTo(null));
    }

    [Fact]
    public void CompareTo_PlainObject()
    {
        var month = CalendarUT.GetCalendarMonth(1, 1);
        var comparable = (IComparable)month;
        object other = new();
        // Act & Assert
        Assert.Throws<ArgumentException>("obj", () => comparable.CompareTo(other));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CompareTo_WhenEqual(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var left = CalendarUT.GetCalendarMonth(y, m);
        var right = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.False(left > right);
        Assert.True(left >= right);
        Assert.True(left <= right);
        Assert.False(left < right);

        Assert.Equal(0, left.CompareTo(right));
        Assert.Equal(0, left.CompareTo((object)right));
    }

    [Theory]
    [InlineData(2, 1)]
    [InlineData(1, 2)]
    public void CompareTo_WhenNotEqual(int y, int m)
    {
        var left = CalendarUT.GetCalendarMonth(1, 1);
        var right = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.False(left > right);
        Assert.False(left >= right);
        Assert.True(left <= right);
        Assert.True(left < right);

        Assert.True(left.CompareTo(right) < 0);
        Assert.True(left.CompareTo((object)right) < 0);
    }

    [Theory]
    [InlineData(2, 1)]
    [InlineData(1, 2)]
    [InlineData(1, 1)]
    public void Min(int y, int m)
    {
        var min = CalendarUT.GetCalendarMonth(1, 1);
        var max = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(min, CalendarMonth.Min(min, max));
        Assert.Equal(min, CalendarMonth.Min(max, min));
    }

    [Theory]
    [InlineData(2, 1)]
    [InlineData(1, 2)]
    [InlineData(1, 1)]
    public void Max(int y, int m)
    {
        var min = CalendarUT.GetCalendarMonth(1, 1);
        var max = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(max, CalendarMonth.Max(min, max));
        Assert.Equal(max, CalendarMonth.Max(max, min));
    }
}

public partial class CalendarMonthFacts<TDataSet> // Increment / Decrement
{
    [Fact]
    public void Increment_Overflows_AtMaxValue()
    {
        var max = CalendarUT.MinMaxMonth.UpperValue;
        // Act & Assert
        Assert.Overflows(() => max++);
    }

    [Fact]
    public void NextMonth_Overflows_AtMaxValue()
    {
        var max = CalendarUT.MinMaxMonth.UpperValue;
        // Act & Assert
        Assert.Overflows(() => max.NextMonth());
    }

    [Fact]
    public void Increment()
    {
        var month = GetSampleValue();
        var monthAfter = CalendarUT.GetCalendarMonth(month.Year, month.MonthOfYear + 1);
        // Act & Assert
        Assert.Equal(monthAfter, ++month);
    }

    [Fact]
    public void NextYear()
    {
        var month = GetSampleValue();
        var monthAfter = CalendarUT.GetCalendarMonth(month.Year, month.MonthOfYear + 1);
        // Act & Assert
        Assert.Equal(monthAfter, month.NextMonth());
    }

    [Fact]
    public void Decrement_Overflows_AtMinValue()
    {
        var min = CalendarUT.MinMaxMonth.LowerValue;
        // Act & Assert
        Assert.Overflows(() => min--);
    }

    [Fact]
    public void PreviousMonth_Overflows_AtMinValue()
    {
        var min = CalendarUT.MinMaxMonth.LowerValue;
        // Act & Assert
        Assert.Overflows(() => min.PreviousMonth());
    }

    [Fact]
    public void Decrement()
    {
        var month = GetSampleValue();
        var monthBefore = CalendarUT.GetCalendarMonth(month.Year, month.MonthOfYear - 1);
        // Act & Assert
        Assert.Equal(monthBefore, --month);
    }

    [Fact]
    public void PreviousYear()
    {
        var month = GetSampleValue();
        var monthBefore = CalendarUT.GetCalendarMonth(month.Year, month.MonthOfYear - 1);
        // Act & Assert
        Assert.Equal(monthBefore, month.PreviousMonth());
    }
}

public partial class CalendarMonthFacts<TDataSet> // Math
{
    //
    // PlusMonths()
    //

    [Fact]
    public void PlusMonths_Overflows()
    {
        var month = CalendarUT.GetCalendarMonth(1, 1);
        // Act & Assert
        Assert.Overflows(() => month + Int32.MinValue);
        Assert.Overflows(() => month.PlusMonths(Int32.MinValue));

        Assert.Overflows(() => month + Int32.MaxValue);
        Assert.Overflows(() => month.PlusMonths(Int32.MaxValue));
    }

    [Fact]
    public void PlusMonths_WithLimitValues()
    {
        var supportedYears = CalendarUT.SupportedYears;
        var month = GetSampleValue();
        int minMs = MinValue - month;
        int maxMs = MaxValue - month;
        // Act & Assert
        Assert.Overflows(() => month + (minMs - 1));
        Assert.Overflows(() => month.PlusMonths(minMs - 1));

        Assert.Equal(MinValue, month + minMs);
        Assert.Equal(MinValue, month.PlusMonths(minMs));

        Assert.Equal(MaxValue, month + maxMs);
        Assert.Equal(MaxValue, month.PlusMonths(maxMs));

        Assert.Overflows(() => month + (maxMs + 1));
        Assert.Overflows(() => month.PlusMonths(maxMs + 1));
    }

    [Fact]
    public void PlusMonths_WithLimitValues_AtMinValue()
    {
        int ms = MaxValue - MinValue;
        // Act & Assert
        Assert.Overflows(() => MinValue - 1);
        Assert.Overflows(() => MinValue.PlusMonths(-1));

        Assert.Equal(MinValue, MinValue - 0);
        Assert.Equal(MinValue, MinValue + 0);
        Assert.Equal(MinValue, MinValue.PlusMonths(0));

        Assert.Equal(MaxValue, MinValue + ms);
        Assert.Equal(MaxValue, MinValue.PlusMonths(ms));

        Assert.Overflows(() => MinValue + (ms + 1));
        Assert.Overflows(() => MinValue.PlusMonths(ms + 1));
    }

    [Fact]
    public void PlusMonths_WithLimitValues_AtMaxValue()
    {
        int ms = MaxValue - MinValue;
        // Act & Assert
        Assert.Overflows(() => MaxValue - (ms + 1));
        Assert.Overflows(() => MaxValue.PlusMonths(-ms - 1));

        Assert.Equal(MinValue, MaxValue - ms);
        Assert.Equal(MinValue, MaxValue.PlusMonths(-ms));

        Assert.Equal(MaxValue, MaxValue - 0);
        Assert.Equal(MaxValue, MaxValue + 0);
        Assert.Equal(MaxValue, MaxValue.PlusMonths(0));

        Assert.Overflows(() => MaxValue + 1);
        Assert.Overflows(() => MaxValue.PlusMonths(1));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void PlusMonths_Zero_IsNeutral(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(month, month + 0);
        Assert.Equal(month, month - 0);
        Assert.Equal(month, month.PlusMonths(0));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountMonthsSince_WhenSame_IsZero(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(0, month - month);
        Assert.Equal(0, month.CountMonthsSince(month));
    }

    //
    // PlusYears()
    //

    [Fact]
    public void PlusYears_Overflows()
    {
        var month = CalendarUT.GetCalendarMonth(1, 1);
        // Act & Assert
        Assert.Overflows(() => month.PlusYears(Int32.MinValue));
        Assert.Overflows(() => month.PlusYears(Int32.MaxValue));
    }

    [Fact]
    public void PlusYears_WithLimitValues()
    {
        var month = GetSampleValue();
        int minYs = MinValue.Year - month.Year;
        int maxYs = MaxValue.Year - month.Year;
        // NB: for calendars with a variable number of months per year, this
        // might not work.
        var minValue = MinValue.WithMonthOfYear(month.MonthOfYear);
        var maxValue = MaxValue.WithMonthOfYear(month.MonthOfYear);
        // Act & Assert
        Assert.Overflows(() => month.PlusYears(minYs - 1));
        Assert.Equal(minValue, month.PlusYears(minYs));
        Assert.Equal(maxValue, month.PlusYears(maxYs));
        Assert.Overflows(() => month.PlusYears(maxYs + 1));
    }

    [Fact]
    public void PlusYears_WithLimitValues_AtMinValue()
    {
        int ys = CalendarUT.SupportedYears.Count() - 1;
        // NB: for calendars with a variable number of months per year, this
        // might not work.
        var maxValue = MaxValue.WithMonthOfYear(MinValue.MonthOfYear);
        // Act & Assert
        Assert.Overflows(() => MinValue.PlusYears(-1));
        Assert.Equal(MinValue, MinValue.PlusYears(0));
        Assert.Equal(maxValue, MinValue.PlusYears(ys));
        Assert.Overflows(() => MinValue.PlusYears(ys + 1));
    }

    [Fact]
    public void PlusYears_WithLimitValues_AtMaxValue()
    {
        int ys = CalendarUT.SupportedYears.Count() - 1;
        // NB: for calendars with a variable number of months per year, this
        // might not work.
        var minValue = MinValue.WithMonthOfYear(MaxValue.MonthOfYear);
        // Act & Assert
        Assert.Overflows(() => MaxValue.PlusYears(-ys - 1));
        Assert.Equal(minValue, MaxValue.PlusYears(-ys));
        Assert.Equal(MaxValue, MaxValue.PlusYears(0));
        Assert.Overflows(() => MaxValue.PlusYears(1));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void PlusYears_Zero_IsNeutral(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(month, month.PlusYears(0));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountYearsSince_WhenSame_IsZero(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(0, month.CountYearsSince(month));
    }
}