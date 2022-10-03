// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized;

using Zorglub.Time.Core;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology.Scopes;

/// <remarks>This calendar is proleptic.</remarks>
public partial class JulianCalendar : IRegularFeaturette
{
    internal JulianCalendar(JulianSchema schema)
        : base("Julian", MinMaxYearScope.CreateMaximal(schema, DayZero.OldStyle)) { }

    /// <inheritdoc />
    public int MonthsInYear => GJSchema.MonthsPerYear;
}
