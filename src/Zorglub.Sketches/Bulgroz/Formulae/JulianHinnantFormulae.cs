// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Formulae;

using static Zorglub.Bulgroz.GJConstants;
using static Zorglub.Bulgroz.JulianConstants;

/// <summary>
/// Formulae for the Julian calendar (Hinnant).
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class JulianHinnantFormulae : ICalendricalFormulae
{
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

        int c = y >> 2;
        y &= 3;

        return -DaysInYearAfterFebruary + DaysPer4YearCycle * c
            + 365 * y + (153 * m + 2) / 5 + d - 1;
    }

    public void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
    {
        daysSinceEpoch += DaysInYearAfterFebruary;

        // 4-years cycle and the day number in the cycle.
        int C = MathZ.Divide(daysSinceEpoch, DaysPer4YearCycle, out int D);
        // The year number in the cycle.
        int Y = (D - D / 1460) / 365;

        int doy = D - 365 * Y;

        m = (5 * doy + 2) / 153;
        d = doy - (153 * m + 2) / 5 + 1;

        if (m > 9)
        {
            Y++;
            m -= 9;
        }
        else
        {
            m += 3;
        }

        y = Y + (C << 2);
    }
}
