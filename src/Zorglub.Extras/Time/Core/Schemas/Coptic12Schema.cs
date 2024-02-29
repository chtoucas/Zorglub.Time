// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Schemas;

/// <summary>
/// Represents the Coptic schema.
/// </summary>
/// <remarks>
/// <para>This class cannot be inherited.</para>
/// <para>This class can ONLY be initialized from within friend assemblies.</para>
/// </remarks>
public sealed partial class Coptic12Schema :
    CopticSchema,
    IEpagomenalDayFeaturette,
    IDaysInMonthDistribution,
    IBoxable<Coptic12Schema>
{
    /// <summary>Represents the number of months in a year.</summary>
    /// <remarks>This field is a constant equal to 12.</remarks>
    internal const int MonthsPerYear = 12;

    /// <summary>Initializes a new instance of the <see cref="Coptic12Schema"/> class.</summary>
    internal Coptic12Schema() : base(30) { }

    /// <inheritdoc />
    public sealed override int MonthsInYear => MonthsPerYear;

    /// <summary>Creates a new (boxed) instance of the <see cref="Coptic12Schema"/> class.</summary>
    [Pure]
    public static Box<Coptic12Schema> GetInstance() => Box.Create(new Coptic12Schema());

    /// <inheritdoc />
    [Pure]
    static ReadOnlySpan<byte> IDaysInMonthDistribution.GetDaysInMonthDistribution(bool leap) =>
        leap
        ? [30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 36]
        : [30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 35];
}

public partial class Coptic12Schema // Year, month or day infos
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenalDay(int y, int m, int d, out int epagomenalNumber) =>
        Twelve.IsEpagomenalDay(d, out epagomenalNumber);

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsIntercalaryDay(int y, int m, int d) => Twelve.IsIntercalaryDay(d);

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsSupplementaryDay(int y, int m, int d) => Twelve.IsSupplementaryDay(d);
}

public partial class Coptic12Schema // Counting months and days within a year or a month
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountMonthsInYear(int y) => MonthsPerYear;

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInMonth(int y, int m) =>
        Twelve.CountDaysInMonth(IsLeapYear(y), m);
}

public partial class Coptic12Schema // Conversions
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountMonthsSinceEpoch(int y, int m) =>
        MonthsCalculator.Regular12.CountMonthsSinceEpoch(y, m);

    /// <inheritdoc />
    public sealed override void GetMonthParts(int monthsSinceEpoch, out int y, out int m) =>
        MonthsCalculator.Regular12.GetMonthParts(monthsSinceEpoch, out y, out m);

    /// <inheritdoc />
    public sealed override void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
    {
        y = GetYear(daysSinceEpoch, out int doy);
        m = Twelve.GetMonth(doy - 1, out d);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int GetMonth(int y, int doy, out int d) =>
        Twelve.GetMonth(doy - 1, out d);
}

public partial class Coptic12Schema // Dates in a given year or month
{
    /// <inheritdoc />
    public sealed override void GetDatePartsAtEndOfYear(int y, out int m, out int d) =>
        Twelve.GetEndOfYearParts(IsLeapYear(y), out m, out d);
}
