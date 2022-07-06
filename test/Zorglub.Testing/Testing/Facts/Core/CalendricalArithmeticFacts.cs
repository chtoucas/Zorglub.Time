// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Core;

using Zorglub.Testing.Data;

// REVIEW(fact): generic param, use TArithmetic rather than an ICalendricalArithmetic.
// Then, add a specialized derived class for FastArithmetic.
public abstract partial class CalendricalArithmeticFacts<TDataSet> :
    ICalendricalArithmeticFacts<ICalendricalArithmetic, TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected CalendricalArithmeticFacts(
        SystemSchema schema,
        Func<ICalendricalSchema, ICalendricalArithmetic> factory)
        : this(schema, factory?.Invoke(schema) ?? throw new ArgumentNullException(nameof(factory))) { }

    private CalendricalArithmeticFacts(ICalendricalSchema schema, ICalendricalArithmetic arithmetic)
        : base(arithmetic)
    {
        Debug.Assert(arithmetic != null);

        Schema = schema ?? throw new ArgumentNullException(nameof(schema));
    }

    protected ICalendricalSchema Schema { get; }
    protected CalendricalSegment Segment => ArithmeticUT.Segment;
}

public partial class CalendricalArithmeticFacts<TDataSet> // Overflows
{
    //
    // Yemoda
    //

    [Fact]
    public void AddDays﹍Yemoda_Overflows()
    {
        var (minDaysSinceEpoch, maxDaysSinceEpoch) = Segment.Domain.Endpoints;
        var epoch = new Yemoda(1, 1, 1);
        // Act & Assert
        Assert.Overflows(() => ArithmeticUT.AddDays(epoch, minDaysSinceEpoch - 1));
        _ = ArithmeticUT.AddDays(epoch, minDaysSinceEpoch);
        _ = ArithmeticUT.AddDays(epoch, maxDaysSinceEpoch);
        Assert.Overflows(() => ArithmeticUT.AddDays(epoch, maxDaysSinceEpoch + 1));
    }

#if false
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
#endif

    //
    // Yedoy
    //

    [Fact]
    public void AddDays﹍Yedoy_Overflows()
    {
        var (minDaysSinceEpoch, maxDaysSinceEpoch) = Segment.Domain.Endpoints;
        var epoch = new Yedoy(1, 1);
        // Act & Assert
        Assert.Overflows(() => ArithmeticUT.AddDays(epoch, minDaysSinceEpoch - 1));
        _ = ArithmeticUT.AddDays(epoch, minDaysSinceEpoch);
        _ = ArithmeticUT.AddDays(epoch, maxDaysSinceEpoch);
        Assert.Overflows(() => ArithmeticUT.AddDays(epoch, maxDaysSinceEpoch + 1));
    }

#if false
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
#endif

    //
    // Yemo
    //

    [Fact]
    public void AddMonths﹍Yemo_Overflows()
    {
        var (minMonthsSinceEpoch, maxMonthsSinceEpoch) = Segment.MonthDomain.Endpoints;
        var epoch = new Yemo(1, 1);
        // Act & Assert
        Assert.Overflows(() => ArithmeticUT.AddMonths(epoch, minMonthsSinceEpoch - 1));
        _ = ArithmeticUT.AddMonths(epoch, minMonthsSinceEpoch);
        _ = ArithmeticUT.AddMonths(epoch, maxMonthsSinceEpoch);
        Assert.Overflows(() => ArithmeticUT.AddMonths(epoch, maxMonthsSinceEpoch + 1));
    }

#if false
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
#endif
}