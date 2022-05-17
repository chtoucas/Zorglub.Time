// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides more facts about <see cref="IDate{TSelf}"/>.
/// <para>See also <seealso cref="IDateFacts{TDate, TDataSet}"/>.</para>
/// </summary>
public abstract partial class IDateMathFacts<TDate, TDataSet>
    where TDate : IDate<TDate>
    where TDataSet : IMathDataSet, ISingleton<TDataSet>
{
    protected IDateMathFacts() { }

    protected static TDataSet DataSet { get; } = TDataSet.Instance;

    protected abstract TDate GetDate(int y, int m, int d);

    protected TDate GetDate(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return GetDate(y, m, d);
    }

    public static TheoryData<Yemoda, Yemoda, int> AddDaysData => DataSet.AddDaysData;
    public static TheoryData<YemodaPair> ConsecutiveDaysData => DataSet.ConsecutiveDaysData;
}

public partial class IDateMathFacts<TDate, TDataSet> // Increment or decrement
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

public partial class IDateMathFacts<TDate, TDataSet> // Addition
{
    [Theory, MemberData(nameof(AddDaysData))]
    public void PlusDays(Yemoda ymd, Yemoda ymdOther, int days)
    {
        var date = GetDate(ymd);
        var other = GetDate(ymdOther);
        // Act & Assert
        // 1) date + days = other.
        Assert.Equal(other, date + days);
        Assert.Equal(other, date.PlusDays(days));
        // 2) other - days = date.
        Assert.Equal(date, other - days);
        Assert.Equal(date, other.PlusDays(-days));
    }

    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void PlusDays_ViaConsecutiveDays(YemodaPair pair)
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
    }

    [Theory, MemberData(nameof(AddDaysData))]
    public void CountDaysSince(Yemoda ymd, Yemoda ymdOther, int days)
    {
        var date = GetDate(ymd);
        var other = GetDate(ymdOther);
        // Act & Assert
        // 1) other - date = days.
        Assert.Equal(days, other - date);
        Assert.Equal(days, other.CountDaysSince(date));
        // 2) date - other = -days.
        Assert.Equal(-days, date - other);
        Assert.Equal(-days, date.CountDaysSince(other));
    }

    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void CountDaysSince_ViaConsecutiveDays(YemodaPair pair)
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
