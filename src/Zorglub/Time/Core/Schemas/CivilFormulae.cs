// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Schemas;

// REVIEW(perf): seems to be a bit slower than GregorianSchema when using a
// y/m/d repr.

// In fact, the formulae should work with year >= 0, nevertheless since
// daysSinceEpoch < 0 when year = 0, it's better to ignore that.

/// <summary>Provides static formulae for the Gregorian schema (year > 0).</summary>
/// <remarks>
/// <para>See also <seealso cref="GregorianFormulae"/>.</para>
/// <para>This class cannot be inherited.</para>
/// </remarks>
internal static class CivilFormulae
{
    /// <summary>Counts the number of consecutive days from the epoch to the specified date.
    /// </summary>
    [Pure]
    public static int CountDaysSinceEpoch(int y, int m, int d)
    {
        Debug.Assert(y > 0);

        if (m < 3)
        {
            y--;
            m += 9;
        }
        else
        {
            m -= 3;
        }

        int C = MathN.Divide(y, 100, out int Y);

        return -GJSchema.DaysInYearAfterFebruary
            + (GregorianSchema.DaysPer400YearCycle * C >> 2)
            + (GregorianSchema.DaysPer4YearSubcycle * Y >> 2)
            + (int)((uint)(153 * m + 2) / 5) + d - 1;
    }

    /// <summary>Obtains the date parts for the specified day count (the number of consecutive days
    /// from the epoch to a date); the results are given in output parameters.</summary>
    public static void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
    {
        Debug.Assert(daysSinceEpoch >= 0);

#if false
        uint daysSinceEpoch1 = (uint)daysSinceEpoch + GJSchema.DaysInYearAfterFebruary;

        uint C = ((daysSinceEpoch1 << 2) + 3) / GregorianSchema.DaysPer400YearCycle;
        uint D = daysSinceEpoch1 - (GregorianSchema.DaysPer400YearCycle * C >> 2);

        uint Y = ((D << 2) + 3) / GregorianSchema.DaysPer4YearSubcycle;
        uint d0y = D - (GregorianSchema.DaysPer4YearSubcycle * Y >> 2);

        m = (int)((5 * d0y + 2) / 153);
        d = (int)(1 + d0y - (uint)(153 * m + 2) / 5);

        if (m > 9)
        {
            Y++;
            m -= 9;
        }
        else
        {
            m += 3;
        }

        y = (int)(100 * C + Y);
#else
        daysSinceEpoch += GJSchema.DaysInYearAfterFebruary;

        int C = (int)((uint)((daysSinceEpoch << 2) + 3) / GregorianSchema.DaysPer400YearCycle);
        int D = daysSinceEpoch - (GregorianSchema.DaysPer400YearCycle * C >> 2);

        int Y = (int)((uint)((D << 2) + 3) / GregorianSchema.DaysPer4YearSubcycle);
        int d0y = D - (GregorianSchema.DaysPer4YearSubcycle * Y >> 2);

        m = (int)((uint)(5 * d0y + 2) / 153);
        d = 1 + d0y - (int)((uint)(153 * m + 2) / 5);

        if (m > 9)
        {
            Y++;
            m -= 9;
        }
        else
        {
            m += 3;
        }

        y = 100 * C + Y;
#endif
    }

    /// <summary>Obtains the ordinal date parts for the specified day count (the number of
    /// consecutive days from the epoch to a date); the results are given in output parameters.
    /// </summary>
    [Pure]
    public static int GetYear(int daysSinceEpoch, out int doy)
    {
        int y = GetYear(daysSinceEpoch);
        doy = 1 + daysSinceEpoch - GetStartOfYear(y);
        return y;
    }

    /// <summary>Obtains the year from the specified day count (the number of consecutive days from
    /// the epoch to a date).</summary>
    [Pure]
    public static int GetYear(int daysSinceEpoch)
    {
        Debug.Assert(daysSinceEpoch >= 0);

        // Int64 to prevent overflows.
        int y = (int)(400L * (daysSinceEpoch + 2) / GregorianSchema.DaysPer400YearCycle);
        int c = y / 100;
        int startOfYearAfter = GJSchema.DaysInCommonYear * y + (y >> 2) - c + (c >> 2);

        return daysSinceEpoch < startOfYearAfter ? y : y + 1;
    }

    /// <summary>Counts the number of consecutive days from the epoch to the first day of the
    /// specified year.</summary>
    [Pure]
    public static int GetStartOfYear(int y)
    {
        Debug.Assert(y > 0);

        y--;
        int c = y / 100;
        return GJSchema.DaysInCommonYear * y + (y >> 2) - c + (c >> 2);
    }
}
