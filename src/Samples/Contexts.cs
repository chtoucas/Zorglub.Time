// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Samples;

using System;
using System.Diagnostics.Contracts;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Hemerology;

using static Zorglub.Time.Extensions.Unboxing;

// TODO(code): remplacer SystemSchema par CalendricalSchema.

// Helpers to initialize all the fields at once, otherwise proper initialization
// of static fields would depend on the order in which they are written.

internal sealed class CalendarContext
{
    public CalendarContext(SystemSchema schema, DayNumber epoch, CalendarScope scope)
    {
        Schema = schema ?? throw new ArgumentNullException(nameof(schema));
        Epoch = epoch;
        Scope = scope ?? throw new ArgumentNullException(nameof(scope));

        PartsFactory = ICalendricalPartsFactory.Create(schema);
        PartsCreator = new PartsCreator(scope);
        Arithmetic = SystemArithmetic.CreateDefault(schema).WithSupportedYears(scope.SupportedYears);
    }

    public CalendricalSchema Schema { get; }
    public DayNumber Epoch { get; }
    public CalendarScope Scope { get; }

    public ICalendricalPartsFactory PartsFactory { get; }
    public PartsCreator PartsCreator { get; }
    public ICalendricalArithmetic Arithmetic { get; }

    public Range<DayNumber> Domain => Scope.Domain;

    /// <summary>Dates on or after year 1.</summary>
    [Pure]
    public static CalendarContext WithYearsAfterZero<TSchema>(DayNumber epoch)
        where TSchema : SystemSchema, IBoxable<TSchema>
    {
        var sch = TSchema.GetInstance().Unbox();
        var scope = MinMaxYearScope.WithMinYear(sch, epoch, 1);

        return new CalendarContext(sch, epoch, scope);
    }

    /// <summary>Dates between years 1 and 9999.</summary>
    [Pure]
    public static CalendarContext WithYearsBetween1And9999<TSchema>(DayNumber epoch)
        where TSchema : SystemSchema, IBoxable<TSchema>
    {
        var sch = TSchema.GetInstance().Unbox();
        var scope = new MinMaxYearScope(sch, epoch, 1, 9999);

        return new CalendarContext(sch, epoch, scope);
    }
}

internal sealed class SchemaContext
{
    private SchemaContext(SystemSchema schema)
    {
        Schema = schema ?? throw new ArgumentNullException(nameof(schema));

        PartsFactory = ICalendricalPartsFactory.Create(schema);
        PartsCreator = PartsCreator.Create(schema);
        Arithmetic = SystemArithmetic.CreateDefault(schema);
    }

    public CalendricalSchema Schema { get; }

    public ICalendricalPartsFactory PartsFactory { get; }
    public PartsCreator PartsCreator { get; }
    public ICalendricalArithmetic Arithmetic { get; }

    public ICalendricalPreValidator PreValidator => Schema.PreValidator;
    public Range<int> Domain => Schema.Domain;

    [Pure]
    public static SchemaContext Create<TSchema>()
        where TSchema : SystemSchema, IBoxable<TSchema>
    {
        var sch = TSchema.GetInstance().Unbox();

        return new SchemaContext(sch);
    }
}
