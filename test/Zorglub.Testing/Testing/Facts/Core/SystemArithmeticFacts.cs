﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Testing.Facts.Core;

using Zorglub.Testing.Data;

// Sync with PartsArithmeticFacts.

public abstract partial class SystemArithmeticFacts<TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected SystemArithmeticFacts(SystemArithmetic arithmetic)
    {
        ArithmeticUT = arithmetic ?? throw new ArgumentNullException(nameof(arithmetic));
    }

    protected SystemArithmetic ArithmeticUT { get; }
    protected SystemSegment Segment => ArithmeticUT.Segment;
}

public partial class SystemArithmeticFacts<TDataSet> // Yemoda
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

public partial class SystemArithmeticFacts<TDataSet> // Yedoy
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
    // - LunarSystemArithmetic
    // - LunisolarSystemArithmetic
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

public partial class SystemArithmeticFacts<TDataSet> // Yemo
{
    [Theory, MemberData(nameof(AddMonthsMonthData))]
    public void AddMonths﹍Yemo(YemoPairAnd<int> pair)
    {
        int ms = pair.Value;
        var month = pair.First;
        var other = pair.Second;
        // Act & Assert
        Assert.Equal(other, ArithmeticUT.AddMonths(month, ms));
        Assert.Equal(month, ArithmeticUT.AddMonths(other, -ms));
    }

    [RedundantTest]
    [Theory, MemberData(nameof(ConsecutiveMonthsData))]
    public void AddMonths﹍Yemo_ViaConsecutiveMonths(YemoPair pair)
    {
        var month = pair.First;
        var monthAfter = pair.Second;
        // Act & Assert
        Assert.Equal(monthAfter, ArithmeticUT.AddMonths(month, 1));
        Assert.Equal(month, ArithmeticUT.AddMonths(monthAfter, -1));
    }

    [Theory, MemberData(nameof(ConsecutiveMonthsData))]
    public void NextMonth﹍Yemo(YemoPair pair)
    {
        var month = pair.First;
        var monthAfter = pair.Second;
        // Act & Assert
        Assert.Equal(monthAfter, ArithmeticUT.NextMonth(month));
    }

    [Theory, MemberData(nameof(ConsecutiveMonthsData))]
    public void PreviousMonth﹍Yemo(YemoPair pair)
    {
        var month = pair.First;
        var monthAfter = pair.Second;
        // Act & Assert
        Assert.Equal(month, ArithmeticUT.PreviousMonth(monthAfter));
    }

    [Theory, MemberData(nameof(AddMonthsMonthData))]
    public void CountMonthsBetween﹍Yemo(YemoPairAnd<int> pair)
    {
        int ms = pair.Value;
        var month = pair.First;
        var other = pair.Second;
        // Act & Assert
        Assert.Equal(ms, ArithmeticUT.CountMonthsBetween(month, other));
        Assert.Equal(-ms, ArithmeticUT.CountMonthsBetween(other, month));
    }

    [RedundantTest]
    [Theory, MemberData(nameof(ConsecutiveMonthsData))]
    public void CountMonthsBetween﹍Yemo_ViaConsecutiveMonths(YemoPair pair)
    {
        var month = pair.First;
        var monthAfter = pair.Second;
        // Act & Assert
        Assert.Equal(1, ArithmeticUT.CountMonthsBetween(month, monthAfter));
        Assert.Equal(-1, ArithmeticUT.CountMonthsBetween(monthAfter, month));
    }
}

public partial class SystemArithmeticFacts<TDataSet> // Overflows
{
    //
    // Yemoda
    //

    [Fact]
    public void AddDays﹍Yemoda_Overflows()
    {
        var (minDaysSinceEpoch, maxDaysSinceEpoch) = Segment.SupportedDays.Endpoints;
        var epoch = new Yemoda(1, 1, 1);
        // Act & Assert
        Assert.Overflows(() => ArithmeticUT.AddDays(epoch, minDaysSinceEpoch - 1));
        _ = ArithmeticUT.AddDays(epoch, minDaysSinceEpoch);
        _ = ArithmeticUT.AddDays(epoch, maxDaysSinceEpoch);
        Assert.Overflows(() => ArithmeticUT.AddDays(epoch, maxDaysSinceEpoch + 1));
    }

    [Fact]
    public void AddDays﹍Yemoda_Overflows_AtStartOfMinYear() =>
        Assert.Overflows(() => ArithmeticUT.AddDays(Segment.MinMaxDateParts.LowerValue, -1));

    [Fact]
    public void AddDays﹍Yemoda_Overflows_AtEndOfMaxYear() =>
        Assert.Overflows(() => ArithmeticUT.AddDays(Segment.MinMaxDateParts.UpperValue, 1));

    [Fact]
    public void PreviousDay﹍Yemoda_Overflows_AtStartOfMinYear() =>
        Assert.Overflows(() => ArithmeticUT.PreviousDay(Segment.MinMaxDateParts.LowerValue));

    [Fact]
    public void NextDay﹍Yemoda_Overflows_AtEndOfMaxYear() =>
        Assert.Overflows(() => ArithmeticUT.NextDay(Segment.MinMaxDateParts.UpperValue));

    //
    // Yedoy
    //

    [Fact]
    public void AddDays﹍Yedoy_Overflows()
    {
        var (minDaysSinceEpoch, maxDaysSinceEpoch) = Segment.SupportedDays.Endpoints;
        var epoch = new Yedoy(1, 1);
        // Act & Assert
        Assert.Overflows(() => ArithmeticUT.AddDays(epoch, minDaysSinceEpoch - 1));
        _ = ArithmeticUT.AddDays(epoch, minDaysSinceEpoch);
        _ = ArithmeticUT.AddDays(epoch, maxDaysSinceEpoch);
        Assert.Overflows(() => ArithmeticUT.AddDays(epoch, maxDaysSinceEpoch + 1));
    }

    [Fact]
    public void AddDays﹍Yedoy_Overflows_AtStartOfMinYear() =>
        Assert.Overflows(() => ArithmeticUT.AddDays(Segment.MinMaxOrdinalParts.LowerValue, -1));

    [Fact]
    public void AddDays﹍Yedoy_Overflows_AtEndOfMaxYear() =>
        Assert.Overflows(() => ArithmeticUT.AddDays(Segment.MinMaxOrdinalParts.UpperValue, 1));

    [Fact]
    public void PreviousDay﹍Yedoy_Overflows_AtStartOfMinYear() =>
        Assert.Overflows(() => ArithmeticUT.PreviousDay(Segment.MinMaxOrdinalParts.LowerValue));

    [Fact]
    public void NextDay﹍Yedoy_Overflows_AtEndOfMaxYear() =>
        Assert.Overflows(() => ArithmeticUT.NextDay(Segment.MinMaxOrdinalParts.UpperValue));

    //
    // Yemo
    //

    [Fact]
    public void AddMonths﹍Yemo_Overflows()
    {
        var (minMonthsSinceEpoch, maxMonthsSinceEpoch) = Segment.SupportedMonths.Endpoints;
        var epoch = new Yemo(1, 1);
        // Act & Assert
        Assert.Overflows(() => ArithmeticUT.AddMonths(epoch, minMonthsSinceEpoch - 1));
        _ = ArithmeticUT.AddMonths(epoch, minMonthsSinceEpoch);
        _ = ArithmeticUT.AddMonths(epoch, maxMonthsSinceEpoch);
        Assert.Overflows(() => ArithmeticUT.AddMonths(epoch, maxMonthsSinceEpoch + 1));
    }

    [Fact]
    public void AddMonths﹍Yemo_Overflows_AtStartOfMinYear() =>
        Assert.Overflows(() => ArithmeticUT.AddMonths(Segment.MinMaxMonthParts.LowerValue, -1));

    [Fact]
    public void AddMonths﹍Yemo_Overflows_AtEndOfMaxYear() =>
        Assert.Overflows(() => ArithmeticUT.AddMonths(Segment.MinMaxMonthParts.UpperValue, 1));

    [Fact]
    public void PreviousMonth﹍Yemo_Overflows_AtStartOfMinYear() =>
        Assert.Overflows(() => ArithmeticUT.PreviousMonth(Segment.MinMaxMonthParts.LowerValue));

    [Fact]
    public void NextMonth﹍Yemo_Overflows_AtEndOfMaxYear() =>
        Assert.Overflows(() => ArithmeticUT.NextMonth(Segment.MinMaxMonthParts.UpperValue));
}