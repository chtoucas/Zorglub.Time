// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Schemas;

/// <summary>
/// Represents the "Tropicália" schema.
/// <para>This class cannot be inherited.</para>
/// <para>This class can ONLY be initialized from within friend assemblies.
/// </para>
/// </summary>
public sealed partial class TropicaliaSchema :
    TropicalistaSchema,
    IDaysInMonthDistribution,
    IBoxable<TropicaliaSchema>
{
    /// <summary>
    /// Represents the number of days from march to december, both
    /// included.
    /// <para>This field is constant equal to 306.</para>
    /// </summary>
    public const int DaysInYearAfterFebruary = 306;

    /// <summary>
    /// Initializes a new instance of the <see cref="TropicaliaSchema"/>
    /// class.
    /// </summary>
    internal TropicaliaSchema() : base(28) { }

    /// <summary>
    /// Creates a new (boxed) instance of the <see cref="TropicaliaSchema"/>
    /// class.
    /// </summary>
    [Pure]
    public static Box<TropicaliaSchema> GetInstance() => Box.Create(new TropicaliaSchema());

    /// <inheritdoc />
    [Pure]
    static ReadOnlySpan<byte> IDaysInMonthDistribution.GetDaysInMonthDistribution(bool leap) =>
        leap
        ? new byte[12] { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 }
        : new byte[12] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
}

public partial class TropicaliaSchema // Year, month or day infos
{
    /// <inheritdoc />
    [Pure]
    public sealed override bool IsIntercalaryDay(int y, int m, int d) => m == 2 && d == 29;
}

public partial class TropicaliaSchema // Counting months and days within a year or a month
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInYearBeforeMonth(int y, int m) =>
        m < 3 ? 31 * (m - 1)
        : IsLeapYearImpl(y) ? (153 * m - 157) / 5
        : (153 * m - 162) / 5;

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInMonth(int y, int m) =>
        m != 2 ? 30 + ((m + (m >> 3)) & 1) : IsLeapYearImpl(y) ? 29 : 28;
}

public partial class TropicaliaSchema // Conversions
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysSinceEpoch(int y, int m, int d)
    {
        if (m < 3)
        {
            y--;
            m += 9;
        }
        else
        {
            m -= 3;
        }

        int C = y >> 7;
        int Y = y & 127;

        return -DaysInYearAfterFebruary + DaysPer128YearCycle * C
            + 365 * Y + (Y >> 2) + (153 * m + 2) / 5 + d - 1;
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int GetMonth(int y, int doy, out int d)
    {
        int m;

        // Ordered from the most common case to least common one.
        // NB: if we change the code, update GJSchema.
        if (doy > 60)
        {
            doy -= IsLeapYearImpl(y) ? 61 : 60;
            uint n = (uint)(5 * doy + 2) / 153;

            m = (int)(n + 3);
            d = doy - (int)((153 * n + 2) / 5) + 1;
        }
        else if (doy < 60)
        {
            m = MathN.AugmentedDivide(doy - 1, 31, out d);

        }
        else // doy == 60
        {
            if (IsLeapYearImpl(y))
            {
                m = 2; d = 29;
            }
            else
            {
                m = 3; d = 1;
            }
        }

        return m;
    }
}

public partial class TropicaliaSchema // Dates in a given year or month
{
    /// <inheritdoc />
    public sealed override void GetDatePartsAtEndOfYear(int y, out int m, out int d)
    {
        m = 12; d = 31;
    }
}
