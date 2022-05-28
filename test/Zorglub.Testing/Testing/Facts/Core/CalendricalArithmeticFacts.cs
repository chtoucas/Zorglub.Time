// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Core;

using Zorglub.Testing.Data;

public abstract partial class CalendricalArithmeticFacts<TDataSet> : ICalendricalArithmeticFacts<TDataSet>
    where TDataSet : ICalendricalDataSet, IMathDataSet, ISingleton<TDataSet>
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

        var minMaxYear = schema.SupportedYears.Endpoints;
        var partsFactory = ICalendricalPartsFactoryEx.Create(schema, @checked: true);

        (MinYear, MaxYear) = minMaxYear;

        (MinDaysSinceEpoch, MaxDaysSinceEpoch) =
            minMaxYear.Select(schema.GetStartOfYear, schema.GetEndOfYear);
        (MinYemoda, MaxYemoda) =
            minMaxYear.Select(partsFactory.GetStartOfYearParts, partsFactory.GetEndOfYearParts);
        (MinYedoy, MaxYedoy) =
            minMaxYear.Select(partsFactory.GetStartOfYearOrdinalParts, partsFactory.GetEndOfYearOrdinalParts);
    }

    protected ICalendricalSchema Schema { get; }

    protected int MinYear { get; }
    protected int MaxYear { get; }

    protected int MinDaysSinceEpoch { get; }
    protected int MaxDaysSinceEpoch { get; }

    protected Yemoda MinYemoda { get; }
    protected Yemoda MaxYemoda { get; }

    protected Yedoy MinYedoy { get; }
    protected Yedoy MaxYedoy { get; }
}

public partial class CalendricalArithmeticFacts<TDataSet> // Overflows
{
    [Fact]
    public void NextDay﹍Yemoda_Overflows_AtEndOfMaxYear() =>
        Assert.Overflows(() => ArithmeticUT.NextDay(MaxYemoda));

    [Fact]
    public void PreviousDay﹍Yemoda_Overflows_AtStartOfMinYear() =>
        Assert.Overflows(() => ArithmeticUT.PreviousDay(MinYemoda));

    [Fact]
    public void NextDay﹍Yedoy_Overflows_AtEndOfMaxYear() =>
        Assert.Overflows(() => ArithmeticUT.NextDay(MaxYedoy));

    [Fact]
    public void PreviousDay﹍Yedoy_Overflows_AtStartOfMinYear() =>
        Assert.Overflows(() => ArithmeticUT.PreviousDay(MinYedoy));
}