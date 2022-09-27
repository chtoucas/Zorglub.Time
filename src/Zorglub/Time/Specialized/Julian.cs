// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized;

using Zorglub.Time.Core;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology.Scopes;

// TODO(api): non-standard math.
// XML doc, explain the scope for all calendars.
// CountDaysSince(other) checked context or not? do we test it?

/// <remarks>This calendar is proleptic.</remarks>
public sealed partial class JulianCalendar : IRegularFeaturette
{
    internal JulianCalendar(JulianSchema schema)
        : base("Julian", MinMaxYearScope.CreateMaximal(schema, DayZero.OldStyle))
    {
        MonthsInYear = schema.MonthsInYear;
    }

    /// <inheritdoc />
    public int MonthsInYear { get; }
}
