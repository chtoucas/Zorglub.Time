// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides more facts about <see cref="IDate{TSelf}"/> and its standard mathematical operations.
/// <para>See also <seealso cref="IDateFacts{TDate, TDataSet}"/> for some basic facts.</para>
/// </summary>
public abstract partial class IDateOrdinalMathFacts<TDate, TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDate : IDate<TDate>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected IDateOrdinalMathFacts() { }

    protected abstract TDate GetDate(int y, int doy);

    protected TDate GetDate(Yedoy ydoy)
    {
        var (y, doy) = ydoy;
        return GetDate(y, doy);
    }
}

public partial class IDateOrdinalMathFacts<TDate, TDataSet> // Increment or decrement
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

public partial class IDateOrdinalMathFacts<TDate, TDataSet> // Addition
{
    [Theory, MemberData(nameof(AddDaysOrdinalData))]
    public void PlusDays(YedoyPairAnd<int> pair)
    {
        int days = pair.Value;
        var date = GetDate(pair.First);
        var other = GetDate(pair.Second);
        // Act & Assert
        // 1) date + days = other.
        Assert.Equal(other, date + days);
        Assert.Equal(other, date.PlusDays(days));
        // 2) other - days = date.
        Assert.Equal(date, other - days);
        Assert.Equal(date, other.PlusDays(-days));
        // 3) date - (-days) = other.
        Assert.Equal(other, date - (-days));
    }

    [Theory, MemberData(nameof(ConsecutiveDaysOrdinalData))]
    public void PlusDays_ViaConsecutiveDays(YedoyPair pair)
    {
        var date = GetDate(pair.First);
        var dateAfter = GetDate(pair.Second);
        // Act & Assert
        // 1) date + 1 = dateAfter.
        Assert.Equal(dateAfter, date + 1);
        Assert.Equal(dateAfter, date.PlusDays(1));
        // 2) dateAfter - 1 = date.
        Assert.Equal(date, dateAfter - 1);
        Assert.Equal(date, dateAfter.PlusDays(-1));
        // 3) date - (-days) = dateAfter.
        Assert.Equal(dateAfter, date - (-1));
    }

    [Theory, MemberData(nameof(AddDaysOrdinalData))]
    public void CountDaysSince(YedoyPairAnd<int> pair)
    {
        int days = pair.Value;
        var date = GetDate(pair.First);
        var other = GetDate(pair.Second);
        // Act & Assert
        // 1) other - date = days.
        Assert.Equal(days, other - date);
        Assert.Equal(days, other.CountDaysSince(date));
        // 2) date - other = -days.
        Assert.Equal(-days, date - other);
        Assert.Equal(-days, date.CountDaysSince(other));
    }

    [Theory, MemberData(nameof(ConsecutiveDaysOrdinalData))]
    public void CountDaysSince_ViaConsecutiveDays(YedoyPair pair)
    {
        var date = GetDate(pair.First);
        var dateAfter = GetDate(pair.Second);
        // Act & Assert
        // 1) dateAfter - date = 1.
        Assert.Equal(1, dateAfter - date);
        Assert.Equal(1, dateAfter.CountDaysSince(date));
        // 2) date - dateAfter = -1.
        Assert.Equal(-1, date - dateAfter);
        Assert.Equal(-1, date.CountDaysSince(dateAfter));
    }
}
