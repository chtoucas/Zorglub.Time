// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides more facts about <see cref="IDate{TSelf}"/> and its standard mathematical operations.
/// </summary>
public abstract partial class IDateArithmeticFacts<TDate, TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDate : IDate<TDate>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected IDateArithmeticFacts() { }

    protected abstract TDate GetDate(int y, int m, int d);

    protected TDate GetDate(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return GetDate(y, m, d);
    }
}

public partial class IDateArithmeticFacts<TDate, TDataSet> // Increment or decrement
{
    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void Increment(YemodaPair pair)
    {
        var date = GetDate(pair.First);
        var dateAfter = GetDate(pair.Second);
        // Act & Assert
        Assert.Equal(dateAfter, ++date);
    }

    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void Decrement(YemodaPair pair)
    {
        var date = GetDate(pair.First);
        var dateAfter = GetDate(pair.Second);
        // Act & Assert
        Assert.Equal(date, --dateAfter);
    }

    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void NextDay(YemodaPair pair)
    {
        var date = GetDate(pair.First);
        var dateAfter = GetDate(pair.Second);
        // Act & Assert
        Assert.Equal(dateAfter, date.NextDay());
    }

    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void PreviousDay(YemodaPair pair)
    {
        var date = GetDate(pair.First);
        var dateAfter = GetDate(pair.Second);
        // Act & Assert
        Assert.Equal(date, dateAfter.PreviousDay());
    }
}

public partial class IDateArithmeticFacts<TDate, TDataSet> // Addition
{
    [Theory, MemberData(nameof(AddDaysData))]
    public void PlusDays(YemodaPairAnd<int> pair)
    {
        int days = pair.Value;
        var date = GetDate(pair.First);
        var other = GetDate(pair.Second);
        // Act & Assert
        Assert.Equal(other, date + days);
        Assert.Equal(date, other - days);
        Assert.Equal(other, date - (-days));
        Assert.Equal(other, date.PlusDays(days));
        Assert.Equal(date, other.PlusDays(-days));
    }

    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void PlusDays_ViaConsecutiveDays(YemodaPair pair)
    {
        var date = GetDate(pair.First);
        var dateAfter = GetDate(pair.Second);
        // Act & Assert
        Assert.Equal(dateAfter, date + 1);
        Assert.Equal(date, dateAfter - 1);
        Assert.Equal(dateAfter, date - (-1));
        Assert.Equal(dateAfter, date.PlusDays(1));
        Assert.Equal(date, dateAfter.PlusDays(-1));
    }

    [Theory, MemberData(nameof(AddDaysData))]
    public void CountDaysSince(YemodaPairAnd<int> pair)
    {
        int days = pair.Value;
        var date = GetDate(pair.First);
        var other = GetDate(pair.Second);
        // Act & Assert
        Assert.Equal(days, other - date);
        Assert.Equal(-days, date - other);
        Assert.Equal(days, other.CountDaysSince(date));
        Assert.Equal(-days, date.CountDaysSince(other));
    }

    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void CountDaysSince_ViaConsecutiveDays(YemodaPair pair)
    {
        var date = GetDate(pair.First);
        var dateAfter = GetDate(pair.Second);
        // Act & Assert
        Assert.Equal(1, dateAfter - date);
        Assert.Equal(-1, date - dateAfter);
        Assert.Equal(1, dateAfter.CountDaysSince(date));
        Assert.Equal(-1, date.CountDaysSince(dateAfter));
    }
}
