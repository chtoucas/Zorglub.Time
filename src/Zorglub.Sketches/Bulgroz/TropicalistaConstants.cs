// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Bulgroz;

using Zorglub.Time.Core.Schemas;

/// <summary>
/// Provides constants related to the Tropicalista calendars.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static class TropicalistaConstants
{
    /// <summary>
    /// Represents the number of days per 128-year cycle.
    /// <para>This field is a constant equal to 46_751.</para>
    /// </summary>
    public const int DaysPer128YearCycle = TropicalistaSchema.DaysPer128YearCycle;

    /// <summary>
    /// Represents the <i>average</i> number of days per 4-year cycle.
    /// <para>This field is a constant equal to 1461.</para>
    /// </summary>
    public const int DaysPer4YearSubcycle = TropicalistaSchema.DaysPer4YearSubcycle;
}
