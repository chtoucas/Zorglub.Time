// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides more facts about <see cref="IDate{TSelf}"/> and its standard mathematical operations.
/// </summary>
public abstract partial class IOrdinalDateArithmeticFacts<TDate, TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDate : IDate<TDate>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected IOrdinalDateArithmeticFacts() { }

    protected abstract TDate GetDate(int y, int doy);

    protected TDate GetDate(Yedoy ydoy)
    {
        var (y, doy) = ydoy;
        return GetDate(y, doy);
    }
}

public partial class IOrdinalDateArithmeticFacts<TDate, TDataSet> // Increment or decrement
{
    [Theory, MemberData(nameof(ConsecutiveDaysOrdinalData))]
    public void Increment(YedoyPair pair)
    {
        var date = GetDate(pair.First);
        var dateAfter = GetDate(pair.Second);
        // Act & Assert
        Assert.Equal(dateAfter, ++date);
    }

    [Theory, MemberData(nameof(ConsecutiveDaysOrdinalData))]
    public void Decrement(YedoyPair pair)
    {
        var date = GetDate(pair.First);
        var dateAfter = GetDate(pair.Second);
        // Act & Assert
        Assert.Equal(date, --dateAfter);
    }

    [Theory, MemberData(nameof(ConsecutiveDaysOrdinalData))]
    public void NextDay(YedoyPair pair)
    {
        var date = GetDate(pair.First);
        var dateAfter = GetDate(pair.Second);
        // Act & Assert
        Assert.Equal(dateAfter, date.NextDay());
    }

    [Theory, MemberData(nameof(ConsecutiveDaysOrdinalData))]
    public void PreviousDay(YedoyPair pair)
    {
        var date = GetDate(pair.First);
        var dateAfter = GetDate(pair.Second);
        // Act & Assert
        Assert.Equal(date, dateAfter.PreviousDay());
    }
}

public partial class IOrdinalDateArithmeticFacts<TDate, TDataSet> // Addition
{
    [Theory, MemberData(nameof(AddDaysOrdinalData))]
    public void PlusDays(YedoyPairAnd<int> pair)
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

    [Theory, MemberData(nameof(ConsecutiveDaysOrdinalData))]
    public void PlusDays_ViaConsecutiveDays(YedoyPair pair)
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

    [Theory, MemberData(nameof(AddDaysOrdinalData))]
    public void CountDaysSince(YedoyPairAnd<int> pair)
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

    [Theory, MemberData(nameof(ConsecutiveDaysOrdinalData))]
    public void CountDaysSince_ViaConsecutiveDays(YedoyPair pair)
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
