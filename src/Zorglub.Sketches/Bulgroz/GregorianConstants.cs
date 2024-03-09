// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Bulgroz;

using Zorglub.Time.Core.Schemas;

/// <summary>
/// Provides constants related to the Gregorian calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static class GregorianConstants
{
    /// <summary>
    /// Represents the number of days per 400-year cycle.
    /// <para>This field is a constant equal to 146_097.</para>
    /// </summary>
    public const int DaysPer400YearCycle = GregorianSchema.DaysPer400YearCycle;

    /// <summary>
    /// Represents the <i>average</i> number of days per 100-year cycle.
    /// <para>This field is a constant equal to 36_524.</para>
    /// </summary>
    public const int DaysPer100YearSubcycle = GregorianSchema.DaysPer100YearSubcycle;

    /// <summary>
    /// Represents the <i>average</i> number of days per 4-year cycle.
    /// <para>This field is a constant equal to 1461.</para>
    /// </summary>
    public const int DaysPer4YearSubcycle = GregorianSchema.DaysPer4YearSubcycle;
}
