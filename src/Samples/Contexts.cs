// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Samples;

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Hemerology.Scopes;

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

        PartsCreator = new PartsCreator(scope);
        Arithmetic = schema.Arithmetic.WithSupportedYears(scope.SupportedYears);
    }

    public SystemSchema Schema { get; }
    public DayNumber Epoch { get; }
    public CalendarScope Scope { get; }

    public PartsCreator PartsCreator { get; }
    public SystemArithmetic Arithmetic { get; }

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
    private SchemaContext(CalendricalSchema schema, CalendricalSegment segment)
    {
        Debug.Assert(schema != null);
        Debug.Assert(segment != null);

        Schema = schema;
        Segment = segment;
    }

    public CalendricalSchema Schema { get; }
    public CalendricalSegment Segment { get; }

    [Pure]
    public static SchemaContext Create<TSchema>()
        where TSchema : SystemSchema, IBoxable<TSchema>
    {
        var sch = TSchema.GetInstance().Unbox();
        var seg = CalendricalSegment.CreateMaximal(sch);

        return new SchemaContext(sch, seg);
    }
}

internal sealed class SystemContext
{
    private SystemContext(SystemSchema schema, Range<int> supportedYears)
    {
        Schema = schema ?? throw new ArgumentNullException(nameof(schema));

        // Temporary code.
        Arithmetic = schema.Arithmetic.WithSupportedYears(supportedYears);
        Segment = Arithmetic.Segment;
    }

    public SystemSchema Schema { get; }
    public SystemSegment Segment { get; }
    public SystemArithmetic Arithmetic { get; }

    [Pure]
    public static SystemContext Create<TSchema>()
        where TSchema : SystemSchema, IBoxable<TSchema>
    {
        var sch = TSchema.GetInstance().Unbox();

        return new SystemContext(sch, sch.SupportedYears);
    }
}