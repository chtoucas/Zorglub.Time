﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Core;

using Zorglub.Testing.Data;

// REVIEW(fact): generic param, use TArithmetic rather than an ICalendricalArithmetic.
// Then, add a specialized derived class for FastArithmetic.
public abstract partial class CalendricalArithmeticFacts<TDataSet> :
    ICalendricalArithmeticFacts<ICalendricalArithmetic, TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected CalendricalArithmeticFacts(ICalendricalSchema schema)
        : this(schema, schema?.Arithmetic ?? throw new ArgumentNullException(nameof(schema))) { }

    protected CalendricalArithmeticFacts(
        ICalendricalSchema schema,
        Func<ICalendricalSchema, ICalendricalArithmetic> factory)
        : this(schema, factory?.Invoke(schema) ?? throw new ArgumentNullException(nameof(factory))) { }

    protected CalendricalArithmeticFacts(
        SystemSchema schema,
        Func<SystemSchema, ICalendricalArithmetic> factory)
        : this(schema, factory?.Invoke(schema) ?? throw new ArgumentNullException(nameof(factory))) { }

    private CalendricalArithmeticFacts(ICalendricalSchema schema, ICalendricalArithmetic arithmetic)
        : base(arithmetic)
    {
        Schema = schema ?? throw new ArgumentNullException(nameof(schema));

        // FIXME(fact): this is wrong, see CalendricalArithmetic's ctor (SupportedYears = set.Range).

        var minMaxYear = schema.SupportedYears.Endpoints;
        var helper = MonthsSinceEpochHelper.Create(schema);
        var partsFactory = new CalendricalPartsFactoryChecked(schema);

        var minMaxMonthsSinceEpoch = minMaxYear.Select(helper.GetStartOfYear, helper.GetEndOfYear);

        (MinYear, MaxYear) = minMaxYear;

        (MinMonthsSinceEpoch, MaxMonthsSinceEpoch) = minMaxMonthsSinceEpoch;
        (MinDaysSinceEpoch, MaxDaysSinceEpoch) =
            minMaxYear.Select(schema.GetStartOfYear, schema.GetEndOfYear);
        (MinYemoda, MaxYemoda) =
            minMaxYear.Select(partsFactory.GetStartOfYearParts, partsFactory.GetEndOfYearParts);
        (MinYedoy, MaxYedoy) =
            minMaxYear.Select(partsFactory.GetStartOfYearOrdinalParts, partsFactory.GetEndOfYearOrdinalParts);
        (MinYemo, MaxYemo) =
            minMaxMonthsSinceEpoch.Select(partsFactory.GetMonthParts, partsFactory.GetMonthParts);
    }

    protected ICalendricalSchema Schema { get; }

    protected int MinYear { get; }
    protected int MaxYear { get; }

    protected int MinMonthsSinceEpoch { get; }
    protected int MaxMonthsSinceEpoch { get; }

    protected int MinDaysSinceEpoch { get; }
    protected int MaxDaysSinceEpoch { get; }

    protected Yemoda MinYemoda { get; }
    protected Yemoda MaxYemoda { get; }

    protected Yedoy MinYedoy { get; }
    protected Yedoy MaxYedoy { get; }

    protected Yemo MinYemo { get; }
    protected Yemo MaxYemo { get; }
}

public partial class CalendricalArithmeticFacts<TDataSet> // Overflows
{
    //
    // Yemoda
    //

    [Fact]
    public void AddDays﹍Yemoda_Overflows()
    {
        var epoch = new Yemoda(1, 1, 1);
        // Act & Assert
        Assert.Overflows(() => ArithmeticUT.AddDays(epoch, MinDaysSinceEpoch - 1));
        _ = ArithmeticUT.AddDays(epoch, MinDaysSinceEpoch);
        _ = ArithmeticUT.AddDays(epoch, MaxDaysSinceEpoch);
        Assert.Overflows(() => ArithmeticUT.AddDays(epoch, MaxDaysSinceEpoch + 1));
    }

    [Fact]
    public void AddDays﹍Yemoda_Overflows_AtStartOfMinYear() =>
        Assert.Overflows(() => ArithmeticUT.AddDays(MinYemoda, -1));

    [Fact]
    public void AddDays﹍Yemoda_Overflows_AtEndOfMaxYear() =>
        Assert.Overflows(() => ArithmeticUT.AddDays(MaxYemoda, 1));

    [Fact]
    public void PreviousDay﹍Yemoda_Overflows_AtStartOfMinYear() =>
        Assert.Overflows(() => ArithmeticUT.PreviousDay(MinYemoda));

    [Fact]
    public void NextDay﹍Yemoda_Overflows_AtEndOfMaxYear() =>
        Assert.Overflows(() => ArithmeticUT.NextDay(MaxYemoda));

    //
    // Yedoy
    //

    [Fact]
    public void AddDays﹍Yedoy_Overflows()
    {
        var epoch = new Yedoy(1, 1);
        // Act & Assert
        Assert.Overflows(() => ArithmeticUT.AddDays(epoch, MinDaysSinceEpoch - 1));
        _ = ArithmeticUT.AddDays(epoch, MinDaysSinceEpoch);
        _ = ArithmeticUT.AddDays(epoch, MaxDaysSinceEpoch);
        Assert.Overflows(() => ArithmeticUT.AddDays(epoch, MaxDaysSinceEpoch + 1));
    }

    [Fact]
    public void AddDays﹍Yedoy_Overflows_AtStartOfMinYear() =>
        Assert.Overflows(() => ArithmeticUT.AddDays(MinYedoy, -1));

    [Fact]
    public void AddDays﹍Yedoy_Overflows_AtEndOfMaxYear() =>
        Assert.Overflows(() => ArithmeticUT.AddDays(MaxYedoy, 1));

    [Fact]
    public void PreviousDay﹍Yedoy_Overflows_AtStartOfMinYear() =>
        Assert.Overflows(() => ArithmeticUT.PreviousDay(MinYedoy));

    [Fact]
    public void NextDay﹍Yedoy_Overflows_AtEndOfMaxYear() =>
        Assert.Overflows(() => ArithmeticUT.NextDay(MaxYedoy));

    //
    // Yemo
    //

    [Fact]
    public void AddMonths﹍Yemo_Overflows()
    {
        var epoch = new Yemo(1, 1);
        // Act & Assert
        Assert.Overflows(() => ArithmeticUT.AddMonths(epoch, MinMonthsSinceEpoch - 1));
        _ = ArithmeticUT.AddMonths(epoch, MinMonthsSinceEpoch);
        _ = ArithmeticUT.AddMonths(epoch, MaxMonthsSinceEpoch);
        Assert.Overflows(() => ArithmeticUT.AddMonths(epoch, MaxMonthsSinceEpoch + 1));
    }

    [Fact]
    public void AddMonths﹍Yemo_Overflows_AtStartOfMinYear() =>
        Assert.Overflows(() => ArithmeticUT.AddMonths(MinYemo, -1));

    [Fact]
    public void AddMonths﹍Yemo_Overflows_AtEndOfMaxYear() =>
        Assert.Overflows(() => ArithmeticUT.AddMonths(MaxYemo, 1));

    [Fact]
    public void PreviousMonth﹍Yemo_Overflows_AtStartOfMinYear() =>
        Assert.Overflows(() => ArithmeticUT.PreviousMonth(MinYemo));

    [Fact]
    public void NextMonth﹍Yemo_Overflows_AtEndOfMaxYear() =>
        Assert.Overflows(() => ArithmeticUT.NextMonth(MaxYemo));
}