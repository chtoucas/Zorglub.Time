// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Samples;

using System;
using System.Diagnostics.Contracts;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Hemerology.Scopes;

using static Zorglub.Time.Extensions.Unboxing;

internal static class SchemaActivator
{
    /// <summary>
    /// Creates a new instance of the <typeparamref name="TSchema"/> class.
    /// </summary>
    [Pure]
    public static TSchema CreateInstance<TSchema>()
        where TSchema : class, ICalendricalSchema, IBoxable<TSchema>
    {
        return TSchema.GetInstance().Unbox();
    }
}

internal static class ScopeActivator
{
    /// <summary>Dates between years 1 and 9999.</summary>
    [Pure]
    public static CalendarScope<TSchema> CreateStandard<TSchema>(DayNumber epoch)
        where TSchema : class, ICalendricalSchema, IBoxable<TSchema>
    {
        var sch = SchemaActivator.CreateInstance<TSchema>();
        var scope = new MinMaxYearScope(sch, epoch, 1, 9999);

        return new CalendarScope<TSchema>(scope);
    }
}

internal sealed class CalendarScope<TSchema> where TSchema : ICalendricalSchema
{
    public CalendarScope(CalendarScope scope)
    {
        Scope = scope ?? throw new ArgumentNullException(nameof(scope));
    }

    public CalendarScope Scope { get; }
    public TSchema Schema => ((ISchemaBound<TSchema>)Scope).Schema;
}
