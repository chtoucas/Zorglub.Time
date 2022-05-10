// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz;

using Zorglub.Time.Core.Schemas;

/// <summary>
/// Provides constants related to the Tabular Islamic calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static class TabularIslamicConstants
{
    /// <summary>
    /// Represents the number of days per 30-year cycle.
    /// <para>This field is a constant equal to 10_631.</para>
    /// </summary>
    public const int DaysPer30YearCycle = TabularIslamicSchema.DaysPer30YearCycle;

    /// <summary>
    /// Represents the number of days in a common year.
    /// <para>This field is a constant equal to 354.</para>
    /// </summary>
    public const int DaysInCommonYear = TabularIslamicSchema.DaysInCommonYear;

    /// <summary>
    /// Represents the number of days in a leap year.
    /// <para>This field is a constant equal to 355.</para>
    /// </summary>
    public const int DaysInLeapYear = TabularIslamicSchema.DaysInLeapYear;

    /// <summary>
    /// Represents the number of days in year 0.
    /// <para>This field is a constant equal to 354.</para>
    /// </summary>
    public const int DaysInYear0 = DaysInCommonYear;

    /// <summary>
    /// Represents the number of months in a year.
    /// <para>This field is a constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = 12;

    public const int ExceptionalMonth = 12;
}
