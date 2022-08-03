// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Hemerology;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;

public abstract partial class IDateAdjustersFacts<TDate, TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDate : IDateable
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected IDateAdjustersFacts(IDateAdjusters<TDate> adjuster)
    {
        AdjusterUT = adjuster ?? throw new ArgumentNullException(nameof(adjuster));
    }

    /// <summary>
    /// Gets the calendar under test.
    /// </summary>
    protected IDateAdjusters<TDate> AdjusterUT { get; }

    protected abstract TDate GetDate(int y, int m, int d);
}

public partial class IDateAdjustersFacts<TDate, TDataSet>
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void DateAdjusters_GetStartOfYear(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        var startOfYear = GetDate(y, 1, 1);
        // Act & Assert
        Assert.Equal(startOfYear, AdjusterUT.GetStartOfYear(date));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void DateAdjusters_GetEndOfYear(YearInfo info)
    {
        int y = info.Year;
        var date = GetDate(y, 1, 1);
        // Act
        var endOfYear = AdjusterUT.GetEndOfYear(date);
        // Assert
        Assert.Equal(y, endOfYear.Year);
        Assert.Equal(info.DaysInYear, endOfYear.DayOfYear);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void DateAdjusters_GetStartOfMonth(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        var startOfMonth = GetDate(y, m, 1);
        // Act & Assert
        Assert.Equal(startOfMonth, AdjusterUT.GetStartOfMonth(date));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void DateAdjusters_GetEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var date = GetDate(y, m, 1);
        var endOfMonth = GetDate(y, m, info.DaysInMonth);
        // Act & Assert
        Assert.Equal(endOfMonth, AdjusterUT.GetEndOfMonth(date));
    }
}
