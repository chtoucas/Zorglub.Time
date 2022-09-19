// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Schemas;

using Zorglub.Time.Core.Intervals;

/// <summary>Represents a Coptic schema and provides a base for derived classes.</summary>
/// <remarks>This class can ONLY be inherited from within friend assemblies.</remarks>
public abstract partial class CopticSchema : PtolemaicSchema
{
    /// <summary>Represents the number of days per 4-year cycle.</summary>
    /// <remarks>
    /// <para>This field is a constant equal to 1461.</para>
    /// <para>On average, a year is 365.25 days long.</para>
    /// </remarks>
    public const int DaysPer4YearCycle = CalendricalConstants.DaysPer4JulianYearCycle;

    /// <summary>Represents the genuine number of days in a month (excluding the epagomenal days
    /// that are not formally part of the twelfth month).</summary>
    /// <remarks>This field is constant equal to 30.</remarks>
    public const int DaysInCopticMonth = 30;

    /// <summary>Called from constructors in derived classes to initialize the
    /// <see cref="CopticSchema"/> class.</summary>
    private protected CopticSchema(int minDaysInMonth) : base(minDaysInMonth)
    {
        SupportedYearsCore = Range.EndingAt(Int32.MaxValue - 1);
    }
}

public partial class CopticSchema // Year, month or day infos
{
    /// <inheritdoc />
    [Pure]
    public sealed override bool IsLeapYear(int y) =>
        // Rule: y mod. 4 == 3.
        (checked(y + 1) & 3) == 0;
}

public partial class CopticSchema // Conversions
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysSinceEpoch(int y, int m, int d) =>
        GetStartOfYear(y) + 30 * (m - 1) + d - 1;

    /// <inheritdoc />
    [Pure]
    public sealed override int GetYear(int daysSinceEpoch) =>
        MathZ.Divide((daysSinceEpoch << 2) + 1463, DaysPer4YearCycle);
}

public partial class CopticSchema // Dates in a given year or month
{
    /// <inheritdoc />
    [Pure]
    public sealed override int GetStartOfYear(int y) => DaysInCommonYear * (y - 1) + (y >> 2);
}
