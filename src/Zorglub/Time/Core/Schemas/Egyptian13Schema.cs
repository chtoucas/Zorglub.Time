// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Core.Schemas;

using Ptolemaic13 = PtolemaicSchema.Thirteen;

/// <summary>
/// Represents the Egyptian schema; alternative form using a virtual month to hold the
/// epagomenal days, see also <seealso cref="Egyptian12Schema"/>.
/// <para>This class cannot be inherited.</para>
/// <para>This class can ONLY be initialized from within friend assemblies.</para>
/// </summary>
/// <remarks>
/// <para>A year is divided into 12 months of 30 days each, followed by 5 epagomenal days.</para>
/// <para>The epagomenal days are outside any month but, for technical reasons, we attach them
/// to a virtual thirteenth month: 1/13 to 5/13.</para>
/// </remarks>
public sealed partial class Egyptian13Schema :
    EgyptianSchema,
    IEpagomenalDayFeaturette,
    IVirtualMonthFeaturette,
    IDaysInMonthDistribution,
    IBoxable<Egyptian13Schema>
{
    /// <summary>
    /// Represents the number of months in a year.
    /// <para>This field is a constant equal to 13.</para>
    /// </summary>
    internal const int MonthsPerYear = 13;

    /// <summary>
    /// Initializes a new instance of the <see cref="Egyptian13Schema"/> class.
    /// </summary>
    internal Egyptian13Schema() : base(5) { }

    /// <inheritdoc />
    public sealed override int MonthsInYear => MonthsPerYear;

    /// <inheritdoc />
    public int VirtualMonth => 13;

    /// <summary>
    /// Creates a new (boxed) instance of the <see cref="Egyptian13Schema"/> class.
    /// </summary>
    [Pure]
    public static Box<Egyptian13Schema> GetInstance() => Box.Create(new Egyptian13Schema());

    /// <inheritdoc />
    [Pure]
    static ReadOnlySpan<byte> IDaysInMonthDistribution.GetDaysInMonthDistribution(bool leap) =>
        // No leap years.
        [30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 5];
}

public partial class Egyptian13Schema // Year, month or day infos
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenalDay(int y, int m, int d, out int epagomenalNumber) =>
        Ptolemaic13.IsEpagomenalDay(m, d, out epagomenalNumber);

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsSupplementaryDay(int y, int m, int d) =>
        Ptolemaic13.IsSupplementaryDay(m);
}

public partial class Egyptian13Schema // Counting months and days within a year or a month
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountMonthsInYear(int y) => MonthsPerYear;

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInMonth(int y, int m) => m == 13 ? 5 : 30;
}

public partial class Egyptian13Schema // Conversions
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountMonthsSinceEpoch(int y, int m) =>
        MonthsCalculator.Regular13.CountMonthsSinceEpoch(y, m);

    /// <inheritdoc />
    public sealed override void GetMonthParts(int monthsSinceEpoch, out int y, out int m) =>
        MonthsCalculator.Regular13.GetMonthParts(monthsSinceEpoch, out y, out m);

    /// <inheritdoc />
    public sealed override void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
    {
        y = 1 + MathZ.Divide(daysSinceEpoch, DaysInYear, out int d0y);
        m = Ptolemaic13.GetMonth(d0y, out d);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int GetMonth(int y, int doy, out int d) =>
        Ptolemaic13.GetMonth(doy - 1, out d);
}

public partial class Egyptian13Schema // Dates in a given year or month
{
    /// <inheritdoc />
    public sealed override void GetDatePartsAtEndOfYear(int y, out int m, out int d)
    {
        m = MonthsPerYear; d = 5;
    }
}
