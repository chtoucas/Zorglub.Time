// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

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
    }

    /// <summary>
    /// Gets the calendar under test.
    /// </summary>
    protected Calendar CalendarUT { get; }

    protected Calendar OtherCalendar { get; }
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

    [Fact]
    public void Calendar_Prop()
    {
        var date = CalendarUT.GetCalendarMonth(1, 1);
        // Act & Assert
        Assert.Equal(CalendarUT, date.Calendar);
        // We also test the internal prop Cuid.
        Assert.Equal(CalendarUT.Id, date.Cuid);
    }

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

public partial class CalendarMonthFacts<TDataSet> // Conversions
{
    //[Fact]
    //public void WithCalendar_NullCalendar()
    //{
    //    var month = CalendarUT.GetCalendarMonth(1, 1);
    //    // Act & Assert
    //    Assert.ThrowsAnexn("newCalendar", () => month.WithCalendar(null!));
    //}
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
