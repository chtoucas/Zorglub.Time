// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Bulgroz.Formulae;

using static Zorglub.Bulgroz.TabularIslamicConstants;

/// <summary>
/// Formulae for the Tabular Islamic calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class TabularIslamicFormulae : ICalendricalFormulae
{
    [Pure]
    public int CountDaysSinceEpoch(int y, int m, int d) =>
        MathZ.Divide(DaysPer30YearCycle * y - 10_617, 30) + (325 * m - 320) / 11 + d - 1;

    public void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
    {
        y = MathZ.Divide(30 * daysSinceEpoch + 10_646, DaysPer30YearCycle);

        int d0y = daysSinceEpoch - MathZ.Divide(DaysPer30YearCycle * y - 10_617, 30);

        m = (11 * d0y + 330) / 325;
        d = 1 + d0y - (325 * m - 320) / 11;
    }

    [Pure]
    public static int GetStartOfYear(int y) => MathZ.Divide(DaysPer30YearCycle * y - 10_617, 30);

    [Pure]
    public static int CountDaysInYearBeforeMonth(int m) => (325 * m - 320) / 11;

    [Pure]
    public static int GetYear(int daysSinceEpoch) =>
        MathZ.Divide(30 * daysSinceEpoch + 10_646, DaysPer30YearCycle);

    [Pure]
    public static int GetMonth(int doy)
    {
        int d0y = doy - 1;
        return (11 * d0y + 330) / 325;
    }
}
