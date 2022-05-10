// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1822 // Mark members as static (Performance)

namespace Zorglub.Bulgroz.Formulae;

using Zorglub.Time.Core.Schemas;

using static Zorglub.Bulgroz.GregorianConstants;

/// <summary>
/// Formulae for the Gregorian calendar (Calendrical Calculations).
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class GregorianCalCalFormulae
{
    [Pure]
    public int CountDaysInYearBeforeMonth(int y, int m)
    {
        int days = (367 * m - 362) / 12;

        return m < 3 ? days
            : days + (GregorianFormulae.IsLeapYear(y) ? -1 : -2);
    }

    [Pure]
    public int GetYear(int daysSinceEpoch)
    {
        int d400 = MathZ.Modulo(daysSinceEpoch, DaysPer400YearCycle, out int c400);

        int d100 = d400 % DaysPer100YearSubcycle;
        int c100 = d400 / DaysPer100YearSubcycle;

        int d4 = d100 % DaysPer4YearSubcycle;
        int c4 = d100 / DaysPer4YearSubcycle;

        int y1 = d4 / 365;

        int y = 100 * ((c400 << 2) + c100) + (c4 << 2) + y1;

        return c100 == 4 || y1 == 4 ? y : y + 1;
    }

    [Pure]
    public int GetMonth(int y, int doy, out int d)
    {
        int δ = GregorianFormulae.IsLeapYear(y) ? (doy < 61 ? -1 : 0) : (doy < 60 ? -1 : 1);
        int m = (12 * (doy + δ) + 373) / 367;
        d = doy - GregorianFormulae.CountDaysInYearBeforeMonth(y, m);
        return m;
    }
}
