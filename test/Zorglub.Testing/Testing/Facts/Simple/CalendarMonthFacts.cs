// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using System.Linq;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

using static Zorglub.Time.Extensions.SimpleInterconversions;
using static Zorglub.Time.Extensions.SimpleRangeExtensions;

// NB: we know that all years within the range [1..9999] are valid.

/// <summary>
/// Provides facts about <see cref="CalendarMonth"/>.
/// </summary>
public abstract partial class CalendarMonthFacts<TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarMonthFacts(SimpleCalendar calendar, SimpleCalendar otherCalendar)
    {
        Requires.NotNull(calendar);
        Requires.NotNull(otherCalendar);
        // NB: instances of type Calendar are singletons.
        if (ReferenceEquals(otherCalendar, calendar))
        {
            throw new ArgumentException(
                "\"otherCalendar\" MUST NOT be equal to \"calendar\"", nameof(otherCalendar));
        }
        if (calendar.IsUserDefined)
        {
            throw new ArgumentException(
                "\"calendar\" MUST NOT be a user-defined calendar", nameof(calendar));
        }

        CalendarUT = calendar;
        OtherCalendar = otherCalendar;

        SupportedYearsTester = new SupportedYearsTester(calendar.YearsValidator.Range);

        (MinMonth, MaxMonth) = calendar.MinMaxMonth;
    }

    protected SimpleCalendar CalendarUT { get; }
    protected SimpleCalendar OtherCalendar { get; }

    protected SupportedYearsTester SupportedYearsTester { get; }

    protected CalendarMonth MinMonth { get; }
    protected CalendarMonth MaxMonth { get; }

    protected CalendarMonth GetMonth(Yemo ym)
    {
        var (y, m) = ym;
        return CalendarUT.GetCalendarMonth(y, m);
    }

    /// <summary>
    /// We only use this sample year when its value matters (mathops); otherwise
    /// just use the first month of the year 1. It is initialized to ensure that
    /// the math operations we are going to perform will work.
    /// </summary>
    private CalendarMonth GetSampleMonth() => CalendarUT.GetCalendarMonth(1234, 2);
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
        var startOfMonth = CalendarUT.GetDate(y, m, 1);
        // Act & Assert
        Assert.Equal(startOfMonth, month.FirstDay);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void LastDay_Prop(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var daysInMonth = info.DaysInMonth;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var endOfMonth = CalendarUT.GetDate(y, m, daysInMonth);
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

public partial class CalendarMonthFacts<TDataSet> // Conversions
{
    [Theory, MemberData(nameof(MonthInfoData))]
    public void ToRange(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var min = CalendarUT.GetDate(y, m, 1);
        var max = CalendarUT.GetDate(y, m, info.DaysInMonth);
        // Act
        var range = month.ToRange();
        // Assert
        Assert.Equal(min, range.Min);
        Assert.Equal(max, range.Max);
    }

    [Fact]
    public void WithCalendar_InvalidCalendar()
    {
        var month = CalendarUT.GetCalendarMonth(1, 1);
        // Act & Assert
        Assert.ThrowsAnexn("newCalendar", () => month.WithCalendar(null!));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void WithCalendar_Invariance(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var range = month.ToRange().ToCalendarDayRange();
        // Act & Assert
        Assert.Equal(range, month.WithCalendar(CalendarUT));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void WithCalendar(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var range = month.ToRange().WithCalendar(OtherCalendar);
        // Act & Assert
        Assert.Equal(range, month.WithCalendar(OtherCalendar));
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
        var date = CalendarUT.GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, month.GetDayOfMonth(d));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetAllDays(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var exp = from d in Enumerable.Range(1, info.DaysInMonth)
                  select CalendarUT.GetDate(y, m, d);
        // Act
        var actual = month.GetAllDays();
        // Assert
        Assert.Equal(exp, actual);
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
    public void WithYear_Invariance(MonthInfo info)
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
    public void WithMonth_InvalidMonth(int y, int newMonth)
    {
        var month = CalendarUT.GetCalendarMonth(y, 1);
        // Act & Assert
        Assert.ThrowsAoorexn("newMonth", () => month.WithMonth(newMonth));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void WithMonth_Invariance(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(month, month.WithMonth(m));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void WithMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, 1);
        var exp = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(exp, month.WithMonth(m));
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

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CompareFast_WhenEqual(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var left = CalendarUT.GetCalendarMonth(y, m);
        var right = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(0, left.CompareFast(right));
    }

    [Theory]
    [InlineData(2, 1)]
    [InlineData(1, 2)]
    public void CompareFast_WhenNotEqual(int y, int m)
    {
        var left = CalendarUT.GetCalendarMonth(1, 1);
        var right = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.True(left.CompareFast(right) < 0);
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

public partial class CalendarMonthFacts<TDataSet> // Math
{
    #region NextMonth()

    [Fact]
    public void NextMonth_Overflows_AtMaxMonth()
    {
        var copy = MaxMonth;
        // Act & Assert
        Assert.Overflows(() => copy++);
        Assert.Overflows(() => MaxMonth.NextMonth());
    }

    [Theory, MemberData(nameof(ConsecutiveMonthsData))]
    public void NextMonth(YemoPair pair)
    {
        var month = GetMonth(pair.First);
        var copy = month;
        var monthAfter = GetMonth(pair.Second);
        // Act & Assert
        Assert.Equal(monthAfter, ++copy);
        Assert.Equal(monthAfter, month.NextMonth());
    }

    #endregion
    #region PreviousMonth()

    [Fact]
    public void PreviousMonth_Overflows_AtMinMonth()
    {
        var copy = MinMonth;
        // Act & Assert
        Assert.Overflows(() => copy--);
        Assert.Overflows(() => MinMonth.PreviousMonth());
    }

    [Theory, MemberData(nameof(ConsecutiveMonthsData))]
    public void PreviousMonth(YemoPair pair)
    {
        var month = GetMonth(pair.First);
        var monthAfter = GetMonth(pair.Second);
        var copy = monthAfter;
        // Act & Assert
        Assert.Equal(month, --copy);
        Assert.Equal(month, monthAfter.PreviousMonth());
    }

    #endregion
    #region AddMonths() & CountMonthsBetween()

    [Fact]
    public void AddMonths_Overflows_WithMaxMonths()
    {
        var month = CalendarUT.GetCalendarMonth(1, 1);
        // Act & Assert
        Assert.Overflows(() => month + Int32.MinValue);
        Assert.Overflows(() => month + Int32.MaxValue);
        Assert.Overflows(() => month.PlusMonths(Int32.MinValue));
        Assert.Overflows(() => month.PlusMonths(Int32.MaxValue));
    }

    [Fact]
    public void AddMonths_AtMinMonth()
    {
        int ms = MaxMonth - MinMonth;
        // Act & Assert
        Assert.Overflows(() => MinMonth - 1);
        Assert.Equal(MinMonth, MinMonth - 0);
        Assert.Equal(MinMonth, MinMonth + 0);
        Assert.Equal(MaxMonth, MinMonth + ms);
        Assert.Overflows(() => MinMonth + (ms + 1));

        Assert.Overflows(() => MinMonth.PlusMonths(-1));
        Assert.Equal(MinMonth, MinMonth.PlusMonths(0));
        Assert.Equal(MaxMonth, MinMonth.PlusMonths(ms));
        Assert.Overflows(() => MinMonth.PlusMonths(ms + 1));
    }

    [Fact]
    public void AddMonths_AtMaxMonth()
    {
        int ms = MaxMonth - MinMonth;
        // Act & Assert
        Assert.Overflows(() => MaxMonth - (ms + 1));
        Assert.Equal(MinMonth, MaxMonth - ms);
        Assert.Equal(MaxMonth, MaxMonth - 0);
        Assert.Equal(MaxMonth, MaxMonth + 0);
        Assert.Overflows(() => MaxMonth + 1);

        Assert.Overflows(() => MaxMonth.PlusMonths(-ms - 1));
        Assert.Equal(MinMonth, MaxMonth.PlusMonths(-ms));
        Assert.Equal(MaxMonth, MaxMonth.PlusMonths(0));
        Assert.Overflows(() => MaxMonth.PlusMonths(1));
    }

    [Fact]
    public void AddMonths_WithLimitMonths()
    {
        var supportedYears = CalendarUT.YearsValidator.Range;
        var month = GetSampleMonth();
        int minMs = MinMonth - month;
        int maxMs = MaxMonth - month;
        // Act & Assert
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
    public void CountMonthsBetween_DoesNotOverflow()
    {
        _ = MaxMonth - MinMonth;
        _ = MinMonth - MaxMonth;
        _ = MaxMonth.CountMonthsSince(MinMonth);
        _ = MinMonth.CountMonthsSince(MaxMonth);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void AddMonths_Zero_IsNeutral(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(month, month + 0);
        Assert.Equal(month, month - 0);
        Assert.Equal(month, month.PlusMonths(0));

        Assert.Equal(0, month - month);
        Assert.Equal(0, month.CountMonthsSince(month));
    }

    [Theory, MemberData(nameof(AddMonthsMonthData))]
    public void AddMonths(YemoPairAnd<int> info)
    {
        int ms = info.Value;
        var month = GetMonth(info.First);
        var other = GetMonth(info.Second);
        // Act & Assert
        Assert.Equal(other, month + ms);
        Assert.Equal(other, month - (-ms));
        Assert.Equal(month, other - ms);
        Assert.Equal(month, other + (-ms));

        Assert.Equal(other, month.PlusMonths(ms));
        Assert.Equal(month, other.PlusMonths(-ms));

        Assert.Equal(ms, other - month);
        Assert.Equal(-ms, month - other);

        Assert.Equal(ms, other.CountMonthsSince(month));
        Assert.Equal(-ms, month.CountMonthsSince(other));
    }

    #endregion
}