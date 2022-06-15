// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts;

using Zorglub.Testing.Data;

public abstract partial class ICalendricalArithmeticFacts<TArithmetic, TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TArithmetic : ICalendricalArithmetic
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected ICalendricalArithmeticFacts(TArithmetic arithmetic)
    {
        ArithmeticUT = arithmetic ?? throw new ArgumentNullException(nameof(arithmetic));
    }

    protected TArithmetic ArithmeticUT { get; }
}

public partial class ICalendricalArithmeticFacts<TArithmetic, TDataSet> // Yemoda
{
    [Theory, MemberData(nameof(AddDaysData))]
    public void AddDays﹍Yemoda(YemodaPairAnd<int> pair)
    {
        int days = pair.Value;
        var date = pair.First;
        var other = pair.Second;
        // Act & Assert
        Assert.Equal(other, ArithmeticUT.AddDays(date, days));
        Assert.Equal(date, ArithmeticUT.AddDays(other, -days));
    }

    // NB: this test is not redundant, it is in fact necessary in order to cover
    // all branches of AddDaysViaDayOfMonth().
    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void AddDays﹍Yemoda_ViaConsecutiveDays(YemodaPair pair)
    {
        var date = pair.First;
        var dateAfter = pair.Second;
        // Act & Assert
        Assert.Equal(dateAfter, ArithmeticUT.AddDays(date, 1));
        Assert.Equal(date, ArithmeticUT.AddDays(dateAfter, -1));
    }

    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void NexDay﹍Yemoda(YemodaPair pair)
    {
        var date = pair.First;
        var dateAfter = pair.Second;
        // Act & Assert
        Assert.Equal(dateAfter, ArithmeticUT.NextDay(date));
    }

    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void PreviousDay﹍Yemoda(YemodaPair pair)
    {
        var date = pair.First;
        var dateAfter = pair.Second;
        // Act & Assert
        Assert.Equal(date, ArithmeticUT.PreviousDay(dateAfter));
    }

    [Theory, MemberData(nameof(AddDaysData))]
    public void CountDaysBetween﹍Yemoda(YemodaPairAnd<int> pair)
    {
        int days = pair.Value;
        var date = pair.First;
        var other = pair.Second;
        // Act & Assert
        Assert.Equal(days, ArithmeticUT.CountDaysBetween(date, other));
        Assert.Equal(-days, ArithmeticUT.CountDaysBetween(other, date));
    }

    [RedundantTest]
    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void CountDaysBetween﹍Yemoda_ViaConsecutiveDays(YemodaPair pair)
    {
        var date = pair.First;
        var dateAfter = pair.Second;
        // Act & Assert
        Assert.Equal(1, ArithmeticUT.CountDaysBetween(date, dateAfter));
        Assert.Equal(-1, ArithmeticUT.CountDaysBetween(dateAfter, date));
    }
}

public partial class ICalendricalArithmeticFacts<TArithmetic, TDataSet> // Yedoy
{
    [Theory, MemberData(nameof(AddDaysOrdinalData))]
    public void AddDays﹍Yedoy(YedoyPairAnd<int> pair)
    {
        int days = pair.Value;
        var date = pair.First;
        var other = pair.Second;
        // Act & Assert
        Assert.Equal(other, ArithmeticUT.AddDays(date, days));
        Assert.Equal(date, ArithmeticUT.AddDays(other, -days));
    }

    [RedundantTest]
    [Theory, MemberData(nameof(ConsecutiveDaysOrdinalData))]
    public void AddDays﹍Yedoy_ViaConsecutiveDays(YedoyPair pair)
    {
        var date = pair.First;
        var dateAfter = pair.Second;
        // Act & Assert
        Assert.Equal(dateAfter, ArithmeticUT.AddDays(date, 1));
        Assert.Equal(date, ArithmeticUT.AddDays(dateAfter, -1));
    }

    [Theory, MemberData(nameof(ConsecutiveDaysOrdinalData))]
    public void NexDay﹍Yedoy(YedoyPair pair)
    {
        var date = pair.First;
        var dateAfter = pair.Second;
        // Act & Assert
        Assert.Equal(dateAfter, ArithmeticUT.NextDay(date));
    }

    [Theory, MemberData(nameof(ConsecutiveDaysOrdinalData))]
    public void PreviousDay﹍Yedoy(YedoyPair pair)
    {
        var date = pair.First;
        var dateAfter = pair.Second;
        // Act & Assert
        Assert.Equal(date, ArithmeticUT.PreviousDay(dateAfter));
    }

    [Theory, MemberData(nameof(AddDaysOrdinalData))]
    public void CountDaysBetween﹍Yedoy(YedoyPairAnd<int> pair)
    {
        int days = pair.Value;
        var date = pair.First;
        var other = pair.Second;
        // Act & Assert
        Assert.Equal(days, ArithmeticUT.CountDaysBetween(date, other));
        Assert.Equal(-days, ArithmeticUT.CountDaysBetween(other, date));
    }

    // TODO(fact): should be marked as redundant but we need custom data
    // (AddDaysOrdinalData) for the following schemas:
    // - LunarArithmetic
    // - LunisolarArithmetic
    // See also AddDays﹍Yemoda_ViaConsecutiveDays().
    //[RedundantTest]
    [Theory, MemberData(nameof(ConsecutiveDaysOrdinalData))]
    public void CountDaysBetween﹍Yedoy_ViaConsecutiveDays(YedoyPair pair)
    {
        var date = pair.First;
        var dateAfter = pair.Second;
        // Act & Assert
        Assert.Equal(1, ArithmeticUT.CountDaysBetween(date, dateAfter));
        Assert.Equal(-1, ArithmeticUT.CountDaysBetween(dateAfter, date));
    }
}

public partial class ICalendricalArithmeticFacts<TArithmetic, TDataSet> // Yemo
{
    [Theory, MemberData(nameof(AddMonthsMonthData))]
    public void AddMonths﹍CalendarMonth(YemoPairAnd<int> info)
    {
        int ms = info.Value;
        var month = info.First;
        var other = info.Second;
        // Act & Assert
        Assert.Equal(other, ArithmeticUT.AddMonths(month, ms));
        Assert.Equal(month, ArithmeticUT.AddMonths(other, -ms));
    }

    [Theory, MemberData(nameof(AddMonthsMonthData))]
    public void CountMonthsBetween﹍CalendarMonth(YemoPairAnd<int> info)
    {
        int ms = info.Value;
        var month = info.First;
        var other = info.Second;
        // Act & Assert
        Assert.Equal(ms, ArithmeticUT.CountMonthsBetween(month, other));
        Assert.Equal(-ms, ArithmeticUT.CountMonthsBetween(other, month));
    }
}