// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using System.Linq;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

public abstract partial class CalendarYearFacts<TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarYearFacts(Calendar calendar, Calendar otherCalendar)
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
    }

    /// <summary>
    /// Gets the calendar under test.
    /// </summary>
    protected Calendar CalendarUT { get; }

    protected Calendar OtherCalendar { get; }

    protected SupportedYearsTester SupportedYearsTester { get; }
}

public partial class CalendarYearFacts<TDataSet> // Prelude
{
    //
    // Properties
    //

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void CenturyOfEra_Prop(CenturyInfo info)
    {
        var (y, century, _) = info;
        var year = CalendarUT.GetCalendarYear(y);
        var centuryOfEra = Ord.Zeroth + century;
        // Act & Assert
        Assert.Equal(centuryOfEra, year.CenturyOfEra);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void Century_Prop(CenturyInfo info)
    {
        var (y, century, _) = info;
        var year = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.Equal(century, year.Century);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void YearOfEra_Prop(CenturyInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var yearOfEra = Ord.Zeroth + y;
        // Act & Assert
        Assert.Equal(yearOfEra, year.YearOfEra);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void YearOfCentury_Prop(CenturyInfo info)
    {
        var (y, _, yearOfCentury) = info;
        var year = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.Equal(yearOfCentury, year.YearOfCentury);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void Year_Prop(CenturyInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.Equal(y, year.Year);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void IsLeap_Prop(YearInfo info)
    {
        // Act
        var year = CalendarUT.GetCalendarYear(info.Year);
        // Assert
        Assert.Equal(info.IsLeap, year.IsLeap);
    }

    [Fact]
    public void Calendar_Prop()
    {
        var year = CalendarUT.GetCalendarYear(1);
        // Act & Assert
        Assert.Equal(CalendarUT, year.Calendar);
        // We also test the internal prop Cuid.
        Assert.Equal(CalendarUT.Id, year.Cuid);
    }
}

public partial class CalendarYearFacts<TDataSet> // Calendar mismatch
{
    [Fact]
    public void Equality_OtherCalendar()
    {
        var year = CalendarUT.GetCalendarYear(1);
        var other = OtherCalendar.GetCalendarYear(1);
        // Act & Assert
        Assert.False(year == other);
        Assert.True(year != other);

        Assert.False(year.Equals(other));
        Assert.False(year.Equals((object)other));
    }

    [Fact]
    public void Comparison_OtherCalendar()
    {
        var year = CalendarUT.GetCalendarYear(1);
        var other = OtherCalendar.GetCalendarYear(1);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => year > other);
        Assert.Throws<ArgumentException>("other", () => year >= other);
        Assert.Throws<ArgumentException>("other", () => year < other);
        Assert.Throws<ArgumentException>("other", () => year <= other);

        Assert.Throws<ArgumentException>("other", () => year.CompareTo(other));
    }

    [Fact]
    public void CountYearsSince_OtherCalendar()
    {
        var year = CalendarUT.GetCalendarYear(1);
        var other = OtherCalendar.GetCalendarYear(1);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => year.CountYearsSince(other));
        Assert.Throws<ArgumentException>("other", () => year - other);
    }
}

public partial class CalendarYearFacts<TDataSet> // Serialization
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void Serialization_Roundtrip(YearInfo info)
    {
        var year = CalendarUT.GetCalendarYear(info.Year);
        // Act & Assert
        Assert.Equal(year, CalendarYear.FromBinary(year.ToBinary()));
    }
}

public partial class CalendarYearFacts<TDataSet> // Counting
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void CountMonthsInYear(YearInfo info)
    {
        var year = CalendarUT.GetCalendarYear(info.Year);
        // Act & Assert
        Assert.Equal(info.MonthsInYear, year.CountMonthsInYear());
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYear(YearInfo info)
    {
        var year = CalendarUT.GetCalendarYear(info.Year);
        // Act & Assert
        Assert.Equal(info.DaysInYear, year.CountDaysInYear());
    }
}

public partial class CalendarYearFacts<TDataSet> // Months
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void GetFirstMonth(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var month = CalendarUT.GetCalendarMonth(y, 1);
        // Act & Assert
        Assert.Equal(month, year.GetFirstMonth());
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetMonthOfYear(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var year = CalendarUT.GetCalendarYear(y);
        var month = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(month, year.GetMonthOfYear(m));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetLastMonth(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var month = CalendarUT.GetCalendarMonth(y, info.MonthsInYear);
        // Act & Assert
        Assert.Equal(month, year.GetLastMonth());
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetMonthsInYear(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var list = from i in Enumerable.Range(1, info.MonthsInYear)
                   select CalendarUT.GetCalendarMonth(y, i);
        // Act
        var actual = year.GetMonthsInYear();
        // Assert
        Assert.Equal(list, actual);
    }
}

public partial class CalendarYearFacts<TDataSet> // Adjustments
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void WithYear_InvalidYears(YearInfo info)
    {
        var year = CalendarUT.GetCalendarYear(info.Year);
        // Act & Assert
        SupportedYearsTester.TestInvalidYear(year.WithYear, "newYear");
    }

    // TODO(fact): InvalidYearAdjustementData, YearAdjustementData, idem with CalendarMonth.

    [Theory, MemberData(nameof(YearInfoData))]
    public void WithYear_Invariant(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.Equal(year, year.WithYear(y));
    }

    [Fact]
    public void WithYear_ValidYears()
    {
        foreach (int y in SupportedYearsTester.ValidYears)
        {
            var year = CalendarUT.GetCalendarYear(1);
            var exp = CalendarUT.GetCalendarYear(y);
            // Act & Assert
            Assert.Equal(exp, year.WithYear(y));
        }
    }
}

public partial class CalendarYearFacts<TDataSet> // IEquatable
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void Equals_WhenSame(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var same = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.True(year == same);
        Assert.False(year != same);

        Assert.True(year.Equals(same));
        Assert.True(year.Equals((object)same));
    }

    [Fact]
    public void Equals_WhenNotSame()
    {
        var year = CalendarUT.GetCalendarYear(1);
        var notSame = CalendarUT.GetCalendarYear(2);
        // Act & Assert
        Assert.False(year == notSame);
        Assert.True(year != notSame);

        Assert.False(year.Equals(notSame));
        Assert.False(year.Equals((object)notSame));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void Equals_NullOrPlainObject(YearInfo info)
    {
        var year = CalendarUT.GetCalendarYear(info.Year);
        // Act & Assert
        Assert.False(year.Equals(1));
        Assert.False(year.Equals(null));
        Assert.False(year.Equals(new object()));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetHashCode_Repeated(YearInfo info)
    {
        var year = CalendarUT.GetCalendarYear(info.Year);
        var obj = (object)year;
        // Act & Assert
        Assert.Equal(year.GetHashCode(), year.GetHashCode());
        Assert.Equal(year.GetHashCode(), obj.GetHashCode());
    }
}

public partial class CalendarYearFacts<TDataSet> // IComparable
{
    [Fact]
    public void CompareTo_Null()
    {
        var year = CalendarUT.GetCalendarYear(1);
        var comparable = (IComparable)year;
        // Act & Assert
        Assert.Equal(1, comparable.CompareTo(null));
    }

    [Fact]
    public void CompareTo_PlainObject()
    {
        var year = CalendarUT.GetCalendarYear(1);
        var comparable = (IComparable)year;
        object other = new();
        // Act & Assert
        Assert.Throws<ArgumentException>("obj", () => comparable.CompareTo(other));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void CompareTo_WhenEqual(YearInfo info)
    {
        int y = info.Year;
        var left = CalendarUT.GetCalendarYear(y);
        var right = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.False(left > right);
        Assert.True(left >= right);
        Assert.True(left <= right);
        Assert.False(left < right);

        Assert.Equal(0, left.CompareTo(right));
        Assert.Equal(0, left.CompareTo((object)right));
    }

    [Fact]
    public void CompareTo_WhenNotEqual()
    {
        var left = CalendarUT.GetCalendarYear(1);
        var right = CalendarUT.GetCalendarYear(2);
        // Act & Assert
        Assert.False(left > right);
        Assert.False(left >= right);
        Assert.True(left <= right);
        Assert.True(left < right);

        Assert.True(left.CompareTo(right) < 0);
        Assert.True(left.CompareTo((object)right) < 0);
    }

    [Fact]
    public void Min()
    {
        var min = CalendarUT.GetCalendarYear(1);
        var max = CalendarUT.GetCalendarYear(2);
        // Act & Assert
        Assert.Equal(min, CalendarYear.Min(min, max));
        Assert.Equal(min, CalendarYear.Min(max, min));
    }

    [Fact]
    public void Max()
    {
        var min = CalendarUT.GetCalendarYear(1);
        var max = CalendarUT.GetCalendarYear(2);
        // Act & Assert
        Assert.Equal(max, CalendarYear.Max(min, max));
        Assert.Equal(max, CalendarYear.Max(max, min));
    }
}

public partial class CalendarYearFacts<TDataSet> // Increment / Decrement
{
    [Fact]
    public void Increment_Overflows_AtMaxValue()
    {
        var max = CalendarUT.MinMaxYear.UpperValue;
        Assert.Overflows(() => max++);
    }

    [Fact]
    public void Decrement_Overflows_AtMinValue()
    {
        var min = CalendarUT.MinMaxYear.LowerValue;
        Assert.Overflows(() => min--);
    }

    [Fact]
    public void NextYear_Overflows_AtMaxValue()
    {
        var max = CalendarUT.MinMaxYear.UpperValue;
        Assert.Overflows(() => max.NextYear());
    }

    [Fact]
    public void PreviousYear_Overflows_AtMinValue()
    {
        var min = CalendarUT.MinMaxYear.LowerValue;
        Assert.Overflows(() => min.PreviousYear());
    }
}

public partial class CalendarYearFacts<TDataSet> // Addition
{
    [Fact]
    public void PlusYears_Overflows()
    {
        var year = CalendarUT.GetCalendarYear(1);
        // Act & Assert
        Assert.Overflows(() => year.PlusYears(Int32.MinValue));
        Assert.Overflows(() => year + Int32.MinValue);

        Assert.Overflows(() => year.PlusYears(Int32.MaxValue));
        Assert.Overflows(() => year + Int32.MaxValue);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void PlusYears_Zero_IsNeutral(YearInfo info)
    {
        var year = CalendarUT.GetCalendarYear(info.Year);
        // Act & Assert
        Assert.Equal(year, year.PlusYears(0));
        Assert.Equal(year, year + 0);
        Assert.Equal(year, year - 0);
    }

    [Fact]
    public void CountYearsSince_DoesNotOverflow()
    {
        var (min, max) = CalendarUT.MinMaxYear;
        int ys = max.Year - min.Year;
        // Act & Assert
        Assert.Equal(ys, max - min);
        Assert.Equal(ys, max.CountYearsSince(min));

        Assert.Equal(-ys, min - max);
        Assert.Equal(-ys, min.CountYearsSince(max));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void CountYearsSince_WhenSame_IsZero(YearInfo info)
    {
        var year = CalendarUT.GetCalendarYear(info.Year);
        // Act & Assert
        Assert.Equal(0, year.CountYearsSince(year));
        Assert.Equal(0, year - year);
    }
}