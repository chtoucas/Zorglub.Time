// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Core;

using Zorglub.Testing.Data;
using Zorglub.Time.Core.Arithmetic;

// Sync with SystemArithmeticFacts.

public partial class PartsArithmeticFacts<TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    public PartsArithmeticFacts(PartsArithmetic arithmetic)
    {
        ArithmeticUT = arithmetic ?? throw new ArgumentNullException(nameof(arithmetic));
    }

    protected PartsArithmetic ArithmeticUT { get; }
    protected CalendricalSegment Segment => ArithmeticUT.Segment;
}

public partial class PartsArithmeticFacts<TDataSet> // DateParts
{
    [Theory, MemberData(nameof(AddDaysData))]
    public void AddDays﹍DateParts(YemodaPairAnd<int> p)
    {
        var pair = (DatePartsPairAnd<int>)p;
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
    public void AddDays﹍DateParts_ViaConsecutiveDays(YemodaPair p)
    {
        var pair = (DatePartsPair)p;
        var date = pair.First;
        var dateAfter = pair.Second;
        // Act & Assert
        Assert.Equal(dateAfter, ArithmeticUT.AddDays(date, 1));
        Assert.Equal(date, ArithmeticUT.AddDays(dateAfter, -1));
    }

    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void NexDay﹍DateParts(YemodaPair p)
    {
        var pair = (DatePartsPair)p;
        var date = pair.First;
        var dateAfter = pair.Second;
        // Act & Assert
        Assert.Equal(dateAfter, ArithmeticUT.NextDay(date));
    }

    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void PreviousDay﹍DateParts(YemodaPair p)
    {
        var pair = (DatePartsPair)p;
        var date = pair.First;
        var dateAfter = pair.Second;
        // Act & Assert
        Assert.Equal(date, ArithmeticUT.PreviousDay(dateAfter));
    }

    [Theory, MemberData(nameof(AddDaysData))]
    public void CountDaysBetween﹍DateParts(YemodaPairAnd<int> p)
    {
        var pair = (DatePartsPairAnd<int>)p;
        int days = pair.Value;
        var date = pair.First;
        var other = pair.Second;
        // Act & Assert
        Assert.Equal(days, ArithmeticUT.CountDaysBetween(date, other));
        Assert.Equal(-days, ArithmeticUT.CountDaysBetween(other, date));
    }

    [RedundantTest]
    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void CountDaysBetween﹍DateParts_ViaConsecutiveDays(YemodaPair p)
    {
        var pair = (DatePartsPair)p;
        var date = pair.First;
        var dateAfter = pair.Second;
        // Act & Assert
        Assert.Equal(1, ArithmeticUT.CountDaysBetween(date, dateAfter));
        Assert.Equal(-1, ArithmeticUT.CountDaysBetween(dateAfter, date));
    }
}

public partial class PartsArithmeticFacts<TDataSet> // OrdinalParts
{
    [Theory, MemberData(nameof(AddDaysOrdinalData))]
    public void AddDays﹍OrdinalParts(YedoyPairAnd<int> p)
    {
        var pair = (OrdinalPartsPairAnd<int>)p;
        int days = pair.Value;
        var date = pair.First;
        var other = pair.Second;
        // Act & Assert
        Assert.Equal(other, ArithmeticUT.AddDays(date, days));
        Assert.Equal(date, ArithmeticUT.AddDays(other, -days));
    }

    [RedundantTest]
    [Theory, MemberData(nameof(ConsecutiveDaysOrdinalData))]
    public void AddDays﹍OrdinalParts_ViaConsecutiveDays(YedoyPair p)
    {
        var pair = (OrdinalPartsPair)p;
        var date = pair.First;
        var dateAfter = pair.Second;
        // Act & Assert
        Assert.Equal(dateAfter, ArithmeticUT.AddDays(date, 1));
        Assert.Equal(date, ArithmeticUT.AddDays(dateAfter, -1));
    }

    [Theory, MemberData(nameof(ConsecutiveDaysOrdinalData))]
    public void NexDay﹍OrdinalParts(YedoyPair p)
    {
        var pair = (OrdinalPartsPair)p;
        var date = pair.First;
        var dateAfter = pair.Second;
        // Act & Assert
        Assert.Equal(dateAfter, ArithmeticUT.NextDay(date));
    }

    [Theory, MemberData(nameof(ConsecutiveDaysOrdinalData))]
    public void PreviousDay﹍OrdinalParts(YedoyPair p)
    {
        var pair = (OrdinalPartsPair)p;
        var date = pair.First;
        var dateAfter = pair.Second;
        // Act & Assert
        Assert.Equal(date, ArithmeticUT.PreviousDay(dateAfter));
    }

    [Theory, MemberData(nameof(AddDaysOrdinalData))]
    public void CountDaysBetween﹍OrdinalParts(YedoyPairAnd<int> p)
    {
        var pair = (OrdinalPartsPairAnd<int>)p;
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
    // See also AddDays﹍DateParts_ViaConsecutiveDays().
    //[RedundantTest]
    [Theory, MemberData(nameof(ConsecutiveDaysOrdinalData))]
    public void CountDaysBetween﹍OrdinalParts_ViaConsecutiveDays(YedoyPair p)
    {
        var pair = (OrdinalPartsPair)p;
        var date = pair.First;
        var dateAfter = pair.Second;
        // Act & Assert
        Assert.Equal(1, ArithmeticUT.CountDaysBetween(date, dateAfter));
        Assert.Equal(-1, ArithmeticUT.CountDaysBetween(dateAfter, date));
    }
}

public partial class PartsArithmeticFacts<TDataSet> // MonthParts
{
    [Theory, MemberData(nameof(AddMonthsMonthData))]
    public void AddMonths﹍MonthParts(YemoPairAnd<int> p)
    {
        var pair = (MonthPartsPairAnd<int>)p;
        int ms = pair.Value;
        var month = pair.First;
        var other = pair.Second;
        // Act & Assert
        Assert.Equal(other, ArithmeticUT.AddMonths(month, ms));
        Assert.Equal(month, ArithmeticUT.AddMonths(other, -ms));
    }

    [RedundantTest]
    [Theory, MemberData(nameof(ConsecutiveMonthsData))]
    public void AddMonths﹍MonthParts_ViaConsecutiveMonths(YemoPair p)
    {
        var pair = (MonthPartsPair)p;
        var month = pair.First;
        var monthAfter = pair.Second;
        // Act & Assert
        Assert.Equal(monthAfter, ArithmeticUT.AddMonths(month, 1));
        Assert.Equal(month, ArithmeticUT.AddMonths(monthAfter, -1));
    }

    [Theory, MemberData(nameof(ConsecutiveMonthsData))]
    public void NextMonth﹍MonthParts(YemoPair p)
    {
        var pair = (MonthPartsPair)p;
        var month = pair.First;
        var monthAfter = pair.Second;
        // Act & Assert
        Assert.Equal(monthAfter, ArithmeticUT.NextMonth(month));
    }

    [Theory, MemberData(nameof(ConsecutiveMonthsData))]
    public void PreviousMonth﹍MonthParts(YemoPair p)
    {
        var pair = (MonthPartsPair)p;
        var month = pair.First;
        var monthAfter = pair.Second;
        // Act & Assert
        Assert.Equal(month, ArithmeticUT.PreviousMonth(monthAfter));
    }

    [Theory, MemberData(nameof(AddMonthsMonthData))]
    public void CountMonthsBetween﹍MonthParts(YemoPairAnd<int> p)
    {
        var pair = (MonthPartsPairAnd<int>)p;
        int ms = pair.Value;
        var month = pair.First;
        var other = pair.Second;
        // Act & Assert
        Assert.Equal(ms, ArithmeticUT.CountMonthsBetween(month, other));
        Assert.Equal(-ms, ArithmeticUT.CountMonthsBetween(other, month));
    }

    [RedundantTest]
    [Theory, MemberData(nameof(ConsecutiveMonthsData))]
    public void CountMonthsBetween﹍MonthParts_ViaConsecutiveMonths(YemoPair p)
    {
        var pair = (MonthPartsPair)p;
        var month = pair.First;
        var monthAfter = pair.Second;
        // Act & Assert
        Assert.Equal(1, ArithmeticUT.CountMonthsBetween(month, monthAfter));
        Assert.Equal(-1, ArithmeticUT.CountMonthsBetween(monthAfter, month));
    }
}

public partial class PartsArithmeticFacts<TDataSet> // Overflows
{
    //
    // DateParts
    //

    [Fact]
    public void AddDays﹍DateParts_Overflows()
    {
        var (minDaysSinceEpoch, maxDaysSinceEpoch) = Segment.SupportedDays.Endpoints;
        var epoch = new DateParts(1, 1, 1);
        // Act & Assert
        Assert.Overflows(() => ArithmeticUT.AddDays(epoch, minDaysSinceEpoch - 1));
        _ = ArithmeticUT.AddDays(epoch, minDaysSinceEpoch);
        _ = ArithmeticUT.AddDays(epoch, maxDaysSinceEpoch);
        Assert.Overflows(() => ArithmeticUT.AddDays(epoch, maxDaysSinceEpoch + 1));
    }

    [Fact]
    public void AddDays﹍DateParts_Overflows_AtStartOfMinYear() =>
        Assert.Overflows(() => ArithmeticUT.AddDays(Segment.MinMaxDateParts.LowerValue, -1));

    [Fact]
    public void AddDays﹍DateParts_Overflows_AtEndOfMaxYear() =>
        Assert.Overflows(() => ArithmeticUT.AddDays(Segment.MinMaxDateParts.UpperValue, 1));

    [Fact]
    public void PreviousDay﹍DateParts_Overflows_AtStartOfMinYear() =>
        Assert.Overflows(() => ArithmeticUT.PreviousDay(Segment.MinMaxDateParts.LowerValue));

    [Fact]
    public void NextDay﹍DateParts_Overflows_AtEndOfMaxYear() =>
        Assert.Overflows(() => ArithmeticUT.NextDay(Segment.MinMaxDateParts.UpperValue));

    //
    // OrdinalParts
    //

    [Fact]
    public void AddDays﹍OrdinalParts_Overflows()
    {
        var (minDaysSinceEpoch, maxDaysSinceEpoch) = Segment.SupportedDays.Endpoints;
        var epoch = new OrdinalParts(1, 1);
        // Act & Assert
        Assert.Overflows(() => ArithmeticUT.AddDays(epoch, minDaysSinceEpoch - 1));
        _ = ArithmeticUT.AddDays(epoch, minDaysSinceEpoch);
        _ = ArithmeticUT.AddDays(epoch, maxDaysSinceEpoch);
        Assert.Overflows(() => ArithmeticUT.AddDays(epoch, maxDaysSinceEpoch + 1));
    }

    [Fact]
    public void AddDays﹍OrdinalParts_Overflows_AtStartOfMinYear() =>
        Assert.Overflows(() => ArithmeticUT.AddDays(Segment.MinMaxOrdinalParts.LowerValue, -1));

    [Fact]
    public void AddDays﹍OrdinalParts_Overflows_AtEndOfMaxYear() =>
        Assert.Overflows(() => ArithmeticUT.AddDays(Segment.MinMaxOrdinalParts.UpperValue, 1));

    [Fact]
    public void PreviousDay﹍OrdinalParts_Overflows_AtStartOfMinYear() =>
        Assert.Overflows(() => ArithmeticUT.PreviousDay(Segment.MinMaxOrdinalParts.LowerValue));

    [Fact]
    public void NextDay﹍OrdinalParts_Overflows_AtEndOfMaxYear() =>
        Assert.Overflows(() => ArithmeticUT.NextDay(Segment.MinMaxOrdinalParts.UpperValue));

    //
    // MonthParts
    //

    [Fact]
    public void AddMonths﹍MonthParts_Overflows()
    {
        var (minMonthsSinceEpoch, maxMonthsSinceEpoch) = Segment.SupportedMonths.Endpoints;
        var epoch = new MonthParts(1, 1);
        // Act & Assert
        Assert.Overflows(() => ArithmeticUT.AddMonths(epoch, minMonthsSinceEpoch - 1));
        _ = ArithmeticUT.AddMonths(epoch, minMonthsSinceEpoch);
        _ = ArithmeticUT.AddMonths(epoch, maxMonthsSinceEpoch);
        Assert.Overflows(() => ArithmeticUT.AddMonths(epoch, maxMonthsSinceEpoch + 1));
    }

    [Fact]
    public void AddMonths﹍MonthParts_Overflows_AtStartOfMinYear() =>
        Assert.Overflows(() => ArithmeticUT.AddMonths(Segment.MinMaxMonthParts.LowerValue, -1));

    [Fact]
    public void AddMonths﹍MonthParts_Overflows_AtEndOfMaxYear() =>
        Assert.Overflows(() => ArithmeticUT.AddMonths(Segment.MinMaxMonthParts.UpperValue, 1));

    [Fact]
    public void PreviousMonth﹍MonthParts_Overflows_AtStartOfMinYear() =>
        Assert.Overflows(() => ArithmeticUT.PreviousMonth(Segment.MinMaxMonthParts.LowerValue));

    [Fact]
    public void NextMonth﹍MonthParts_Overflows_AtEndOfMaxYear() =>
        Assert.Overflows(() => ArithmeticUT.NextMonth(Segment.MinMaxMonthParts.UpperValue));
}
