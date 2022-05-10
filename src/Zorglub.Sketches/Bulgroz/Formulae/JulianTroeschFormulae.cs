// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Formulae;

using static Zorglub.Bulgroz.GJConstants;
using static Zorglub.Bulgroz.JulianConstants;

/// <summary>
/// Formulae for the Julian calendar (Troesch).
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class JulianTroeschFormulae : IInterconversionFormulae
{
    /// <summary>1224</summary>
    private const int L1224 = 4 * DaysInYearAfterFebruary;

    [Pure]
    public int CountDaysSinceEpoch(int y, int m, int d)
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

        return ((DaysPer4YearCycle * y - L1224) >> 2) + (153 * m + 2) / 5 + d - 1;
    }

    public void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
    {
        y = MathZ.Divide(4 * daysSinceEpoch + L1224 + 3, DaysPer4YearCycle);

        int d0y = daysSinceEpoch - MathZ.Divide(DaysPer4YearCycle * y - L1224, 4);

        m = (5 * d0y + 2) / 153;
        d = 1 + d0y - (153 * m + 2) / 5;

        if (m > 9)
        {
            y++;
            m -= 9;
        }
        else
        {
            m += 3;
        }
    }

    [Pure]
    public static int GetStartOfYear(int y)
    {
        y--;
        return DaysPer4YearCycle * y >> 2;
    }
}
