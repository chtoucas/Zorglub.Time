// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using System.Linq;

using Zorglub.Testing.Data;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Simple;

// TODO(fact): tests with ranges that are not a complete year or month...

public abstract partial class SimpleRangeFacts<TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected SimpleRangeFacts(Calendar calendar, Calendar otherCalendar)
    {
        Requires.NotNull(calendar);
        Requires.NotNull(otherCalendar);
        // NB: instances of type Calendar are singletons.
        if (ReferenceEquals(otherCalendar, calendar))
        {
            throw new ArgumentException(
                "\"otherCalendar\" MUST NOT be equal to \"calendar\"", nameof(otherCalendar));
        }

        CalendarUT = calendar;
        OtherCalendar = otherCalendar;
    }

    protected Calendar CalendarUT { get; }
    protected Calendar OtherCalendar { get; }
}

public abstract partial class SimpleRangeFacts<TDataSet> // CalendarDate
{
    [Theory, MemberData(nameof(MonthInfoData))]
    public void CalendarDate_ToEnumerable(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var exp = from d in Enumerable.Range(1, info.DaysInMonth)
                  select CalendarUT.GetCalendarDate(y, m, d);
        // Act
        var actual = month.GetAllDays();
        // Assert
        Assert.Equal(exp, actual);
    }
}

public abstract partial class SimpleRangeFacts<TDataSet> // OrdinalDate
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void OrdinalDate_ToEnumerable(YearInfo info)
    {
        int y = info.Year;
        var range = CalendarUT.GetCalendarYear(y).ToRange();
        var exp = from doy in Enumerable.Range(1, info.DaysInYear)
                  select CalendarUT.GetOrdinalDate(y, doy);
        // Act
        var actual = range.ToEnumerable();
        // Assert
        Assert.Equal(exp, actual);
    }
}

public abstract partial class SimpleRangeFacts<TDataSet> // Month
{
    [Theory, MemberData(nameof(MonthInfoData))]
    public void Month_Count(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        // Act
        var range = month.ToRange();
        // Assert
        Assert.Equal(info.DaysInMonth, range.Count());
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void Month_ToEnumerable(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var range = MonthRange.Create(year);
        var exp = from m in Enumerable.Range(1, info.MonthsInYear)
                  select CalendarUT.GetCalendarMonth(y, m);
        // Act
        var actual = range.ToEnumerable();
        // Assert
        Assert.Equal(exp, actual);
    }

    [Fact]
    public void Month_WithCalendar_InvalidCalendar()
    {
        var range = CalendarUT.GetCalendarMonth(1, 1).ToRange();
        // Act & Assert
        Assert.ThrowsAnexn("newCalendar", () => range.WithCalendar(null!));
    }
}

public abstract partial class SimpleRangeFacts<TDataSet> // Year
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void Year_Count(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        // Act
        var range = year.ToRange();
        // Assert
        Assert.Equal(info.DaysInYear, range.Count());
    }

    [Fact]
    public void Year_WithCalendar_InvalidCalendar()
    {
        var range = CalendarUT.GetCalendarYear(1).ToRange();
        // Act & Assert
        Assert.ThrowsAnexn("newCalendar", () => range.WithCalendar(null!));
    }
}
