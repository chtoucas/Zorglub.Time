// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

/// <summary>
/// Provides facts about <see cref="CalendarMonth"/> and its mathematical operations.
/// </summary>
public abstract partial class CalendarMonthMathFacts<TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, IAdvancedMathDataSet, ISingleton<TDataSet>
{
    protected CalendarMonthMathFacts(Calendar calendar)
    {
        CalendarUT = calendar ?? throw new ArgumentNullException(nameof(calendar));
    }

    /// <summary>
    /// Gets the calendar under test.
    /// </summary>
    protected Calendar CalendarUT { get; }

    private CalendarMonth GetMonth(Yemoda ymd)
    {
        var (y, m, _) = ymd;
        return CalendarUT.GetCalendarMonth(y, m);
    }

    public static DataGroup<YemodaPairAnd<int>> AddYearsData => DataSet.AddYearsData;
    public static DataGroup<YemodaPairAnd<int>> AddMonthsData => DataSet.AddMonthsData;

    [Theory, MemberData(nameof(AddMonthsData))]
    public void PlusMonths(YemodaPairAnd<int> info)
    {
        int ms = info.Value;
        var month = GetMonth(info.First);
        var other = GetMonth(info.Second);
        // Act & Assert
        // 1) month + ms = other.
        Assert.Equal(other, month + ms);
        Assert.Equal(other, month.PlusMonths(ms));
        // 2) other - ms = month.
        Assert.Equal(month, other - ms);
        Assert.Equal(month, other.PlusMonths(-ms));
        // 3) month - (-ms) = other.
        Assert.Equal(other, month - (-ms));
    }

    [Theory, MemberData(nameof(AddMonthsData))]
    public void CountMonthsSince(YemodaPairAnd<int> info)
    {
        int ms = info.Value;
        var month = GetMonth(info.First);
        var other = GetMonth(info.Second);
        // Act & Assert
        // 1) other - month = ms.
        Assert.Equal(ms, other - month);
        Assert.Equal(ms, other.CountMonthsSince(month));
        // 2) month - other = -ms.
        Assert.Equal(-ms, month - other);
        Assert.Equal(-ms, month.CountMonthsSince(other));
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void PlusYears(YemodaPairAnd<int> info)
    {
        int ys = info.Value;
        var month = GetMonth(info.First);
        var other = GetMonth(info.Second);
        // Act & Assert
        Assert.Equal(other, month.PlusYears(ys));
        Assert.Equal(month, other.PlusYears(-ys));
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void CountYearsSince(YemodaPairAnd<int> info)
    {
        int ys = info.Value;
        var month = GetMonth(info.First);
        var other = GetMonth(info.Second);
        // Act & Assert
        Assert.Equal(ys, other.CountYearsSince(month));
        Assert.Equal(-ys, month.CountYearsSince(other));
    }
}
