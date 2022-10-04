// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Schemas;

// The Min/MaxYear limits do not apply here.

// REVIEW(perf): GetYear() overflows pretty quickly. Optimize * 400? Idem w/ 32-bit
// FIXME(code): checked ops?
// formulae should use the new signature: GetOrdinalParts() -> GetYear().

/// <summary>Provides static formulae for the Gregorian schema (32-bit and 64-bit versions).
/// <para>See also <seealso cref="GregorianSchema"/>.</para>
/// <para>This class cannot be inherited.</para></summary>
internal static class GregorianFormulae
{
    /// <summary>Determines whether the specified date is an intercalary day or not.</summary>
    [Pure]
    public static bool IsIntercalaryDay(int m, int d) => m == 2 && d == 29;

    /// <summary>Determines whether the specified year is leap or not.</summary>
    [Pure]
    // CIL code size = 30 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLeapYear(long y) =>
        (y & 3) == 0 && (y % 100 != 0 || y % 400 == 0);

    /// <summary>Determines whether the specified year is leap or not.</summary>
    [Pure]
    // CIL code size = 26 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLeapYear(int y) =>
        (y & 3) == 0 && (y % 100 != 0 || y % 400 == 0);

    /// <summary>Obtains the number of days in the specified year.</summary>
    [Pure]
    public static int CountDaysInYear(long y) =>
        IsLeapYear(y) ? GJSchema.DaysInLeapYear : GJSchema.DaysInCommonYear;

    /// <summary>Obtains the number of days in the specified year.</summary>
    [Pure]
    public static int CountDaysInYear(int y) =>
        IsLeapYear(y) ? GJSchema.DaysInLeapYear : GJSchema.DaysInCommonYear;

    /// <summary>Obtains the number of days before the start of the specified month.</summary>
    [Pure]
    public static int CountDaysInYearBeforeMonth(long y, int m) =>
        m < 3 ? 31 * (m - 1)
        : IsLeapYear(y) ? (int)((uint)(153 * m - 157) / 5)
        : (int)((uint)(153 * m - 162) / 5);

    /// <summary>Obtains the number of days before the start of the specified month.</summary>
    [Pure]
    public static int CountDaysInYearBeforeMonth(int y, int m) =>
        m < 3 ? 31 * (m - 1)
        : IsLeapYear(y) ? (int)((uint)(153 * m - 157) / 5)
        : (int)((uint)(153 * m - 162) / 5);

    /// <summary>Obtains the number of days in the specified month.</summary>
    [Pure]
    public static int CountDaysInMonth(long y, int m) =>
        m != 2 ? 30 + ((m + (m >> 3)) & 1)
        : IsLeapYear(y) ? 29
        : 28;

    /// <summary>Obtains the number of days in the specified month.</summary>
    [Pure]
    public static int CountDaysInMonth(int y, int m) =>
        m != 2 ? 30 + ((m + (m >> 3)) & 1)
        : IsLeapYear(y) ? 29
        : 28;

    /// <summary>Counts the number of consecutive days from the epoch to the specified date.
    /// </summary>
    [Pure]
    public static long CountDaysSinceEpoch(long y, int m, int d)
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

        long C = MathZ.Divide(y, 100L, out long Y);

        return -GJSchema.DaysInYearAfterFebruary
            + (GregorianSchema.DaysPer400YearCycle * C >> 2)
            + (GregorianSchema.DaysPer4YearSubcycle * Y >> 2)
            + (int)((uint)(153 * m + 2) / 5 + d) - 1;
    }

    /// <summary>Counts the number of consecutive days from the epoch to the specified date.
    /// </summary>
    [Pure]
    public static int CountDaysSinceEpoch(int y, int m, int d)
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

        int C = MathZ.Divide(y, 100, out int Y);

        return -GJSchema.DaysInYearAfterFebruary
            + (GregorianSchema.DaysPer400YearCycle * C >> 2)
            + (GregorianSchema.DaysPer4YearSubcycle * Y >> 2)
            + (int)((uint)(153 * m + 2) / 5) + d - 1;
    }

    /// <summary>Obtains the date parts for the specified day count (the number of consecutive days
    /// from the epoch to a date); the results are given in output parameters.</summary>
    public static void GetDateParts(long daysSinceEpoch, out long y, out int m, out int d)
    {
        daysSinceEpoch += GJSchema.DaysInYearAfterFebruary;

        long C = MathZ.Divide((daysSinceEpoch << 2) + 3, GregorianSchema.DaysPer400YearCycle);
        long D = daysSinceEpoch - (GregorianSchema.DaysPer400YearCycle * C >> 2);

        long Y = ((D << 2) + 3) / GregorianSchema.DaysPer4YearSubcycle;
        int d0y = (int)(D - (GregorianSchema.DaysPer4YearSubcycle * Y >> 2));

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
    }

    /// <summary>Obtains the date parts for the specified day count (the number of consecutive days
    /// from the epoch to a date); the results are given in output parameters.</summary>
    public static void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
    {
        daysSinceEpoch += GJSchema.DaysInYearAfterFebruary;

        int C = MathZ.Divide((daysSinceEpoch << 2) + 3, GregorianSchema.DaysPer400YearCycle);
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
    }

    /// <summary>Obtains the date parts for the specified day count (the number of consecutive days
    /// from the epoch to a date).</summary>
    [Pure]
    // CIL code size = 21 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Yemoda GetDateParts(int daysSinceEpoch)
    {
        GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
        return new Yemoda(y, m, d);
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

    /// <summary>Obtains the ordinal date parts for the specified day count (the number of
    /// consecutive days from the epoch to a date).</summary>
    [Pure]
    // CIL code size = 18 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Yedoy GetOrdinalParts(int daysSinceEpoch)
    {
        int y = GetYear(daysSinceEpoch, out int doy);
        return new Yedoy(y, doy);
    }

    /// <summary>Obtains the year from the specified day count (the number of consecutive days from
    /// the epoch to a date).</summary>
    [Pure]
    public static long GetYear(long daysSinceEpoch)
    {
        long y = MathZ.Divide(400 * (daysSinceEpoch + 2), GregorianSchema.DaysPer400YearCycle);
        long c = MathZ.Divide(y, 100);
        long startOfYearAfter = GJSchema.DaysInCommonYear * y + (y >> 2) - c + (c >> 2);

        return daysSinceEpoch < startOfYearAfter ? y : y + 1;
    }

    /// <summary>Obtains the year from the specified day count (the number of consecutive days from
    /// the epoch to a date).</summary>
    [Pure]
    public static int GetYear(int daysSinceEpoch)
    {
        int y = (int)MathZ.Divide(400L * (daysSinceEpoch + 2), GregorianSchema.DaysPer400YearCycle);
        int c = MathZ.Divide(y, 100);
        int startOfYearAfter = GJSchema.DaysInCommonYear * y + (y >> 2) - c + (c >> 2);

        return daysSinceEpoch < startOfYearAfter ? y : y + 1;
    }

    /// <summary>Counts the number of consecutive days from the epoch to the first day of the
    /// specified year.</summary>
    [Pure]
    public static long GetStartOfYear(long y)
    {
        y--;
        long c = MathZ.Divide(y, 100);
        return GJSchema.DaysInCommonYear * y + (y >> 2) - c + (c >> 2);
    }

    /// <summary>Counts the number of consecutive days from the epoch to the first day of the
    /// specified year.</summary>
    [Pure]
    public static int GetStartOfYear(int y)
    {
        y--;
        int c = MathZ.Divide(y, 100);
        return GJSchema.DaysInCommonYear * y + (y >> 2) - c + (c >> 2);
    }

    /// <summary>Counts the number of consecutive days from the epoch to the last day of the
    /// specified year.</summary>
    [Pure]
    public static int GetEndOfYear(int y) => GetStartOfYear(y) + CountDaysInYear(y) - 1;
}
