// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

// TODO(fact): math.

/// <summary>
/// Provides math facts about <see cref="CalendarYear"/> and its mathematical operations.
/// </summary>
public abstract partial class CalendarYearMathFacts<TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarYearMathFacts(Calendar calendar)
    {
        CalendarUT = calendar ?? throw new ArgumentNullException(nameof(calendar));
    }

    /// <summary>
    /// Gets the calendar under test.
    /// </summary>
    protected Calendar CalendarUT { get; }

    [Theory, MemberData(nameof(AddYearsData))]
    public void PlusYears(YemodaPairAnd<int> info)
    {
        int ys = info.Value;
        var year = CalendarUT.GetCalendarYear(info.First.Year);
        var other = CalendarUT.GetCalendarYear(info.Second.Year);
        // Act & Assert
        // 1) year + ys = other.
        Assert.Equal(other, year + ys);
        Assert.Equal(other, year.PlusYears(ys));
        // 2) other - ys = year.
        Assert.Equal(year, other - ys);
        Assert.Equal(year, other.PlusYears(-ys));
        // 3) year - (-ys) = other.
        Assert.Equal(other, year - (-ys));
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void CountYearsSince(YemodaPairAnd<int> info)
    {
        int ys = info.Value;
        var year = CalendarUT.GetCalendarYear(info.First.Year);
        var other = CalendarUT.GetCalendarYear(info.Second.Year);
        // Act & Assert
        // 1) other - year = ys.
        Assert.Equal(ys, other - year);
        Assert.Equal(ys, other.CountYearsSince(year));
        // 2) year - other = -ys.
        Assert.Equal(-ys, year - other);
        Assert.Equal(-ys, year.CountYearsSince(other));
    }
}
