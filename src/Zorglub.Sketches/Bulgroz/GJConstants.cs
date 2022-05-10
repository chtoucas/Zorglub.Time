// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz;

using Zorglub.Time.Core.Schemas;

/// <summary>
/// Provides constants common to both Gregorian and Julian calendars.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static class GJConstants
{
    /// <summary>
    /// Represents the number of days in a common year.
    /// <para>This field is a constant equal to 365.</para>
    /// </summary>
    public const int DaysInCommonYear = GJSchema.DaysInCommonYear;

    /// <summary>
    /// Represents the number of days in a leap year.
    /// <para>This field is a constant equal to 366.</para>
    /// </summary>
    public const int DaysInLeapYear = GJSchema.DaysInLeapYear;

    /// <summary>
    /// Represents the number of days from march to december, both
    /// included.
    /// <para>This field is constant equal to 306.</para>
    /// </summary>
    public const int DaysInYearAfterFebruary = GJSchema.DaysInYearAfterFebruary;

    /// <summary>
    /// <para>This field is constant equal to 306.</para>
    /// </summary>
    public const int DaysFromEndOfFebruaryYear0ToEpoch = DaysInYearAfterFebruary;

    /// <summary>
    /// Represents the number of days in year 0.
    /// <para>This field is a constant equal to 366.</para>
    /// </summary>
    public const int DaysInYear0 = DaysInLeapYear;

    /// <summary>
    /// Represents the number of months in a year.
    /// <para>This field is a constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = 12;

    public const int ExceptionalMonth = 2;
}
