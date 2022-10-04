// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized;

using Zorglub.Time.Core;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology.Scopes;

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999] of years.</remarks>
public partial class CivilCalendar : IRegularFeaturette
{
    private static partial MinMaxYearScope GetScope(CivilSchema schema) =>
        StandardScope.Create(schema, DayZero.NewStyle);

    /// <inheritdoc />
    public int MonthsInYear => GJSchema.MonthsPerYear;
}

/// <remarks>This calendar is <i>proleptic</i>. It supports <i>all</i> dates within the range
/// [-999_998..999_999] of years.</remarks>
public partial class GregorianCalendar : IRegularFeaturette
{
    private static partial MinMaxYearScope GetScope(GregorianSchema schema) =>
        MinMaxYearScope.CreateMaximal(schema, DayZero.NewStyle);

    /// <inheritdoc />
    public int MonthsInYear => GJSchema.MonthsPerYear;
}

/// <remarks>This calendar is <i>proleptic</i>. It supports <i>all</i> dates within the range
/// [-999_998..999_999] of years.</remarks>
public partial class JulianCalendar : IRegularFeaturette
{
    private static partial MinMaxYearScope GetScope(JulianSchema schema) =>
        MinMaxYearScope.CreateMaximal(schema, DayZero.OldStyle);

    /// <inheritdoc />
    public int MonthsInYear => GJSchema.MonthsPerYear;
}
