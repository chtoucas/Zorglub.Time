// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Formulae;

using static Zorglub.Bulgroz.TropicalistaConstants;

// Formes quasi-affines:
//   (1461, 4, 0)
//   (61, 2, -60)
//   (1, 1, -1)
// Formes affines inverses:
//   (4, 1461, 3)
//   (2, 61, 61)
//   (1, 1, 1)
//
// Bien entendu, le code est très similaire à celui écrit pour Tropicalia3031.
// On en profite pour en donner des versions optimisées.

/// <summary>
/// Formulae for the "Tropicália" (31-30) calendar (Troesch).
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class Tropicalia3130TroeschFormulae : IInterconversionFormulae
{
    [Pure]
    public int CountDaysSinceEpoch(int y, int m, int d)
    {
        y--;
        // Cycle de 128 années et rang dans le cycle.
        int C = MathZ.Divide(y, 128, out int Y);

        return DaysPer128YearCycle * C
            + (DaysPer4YearSubcycle * Y / 4)
            + (61 * m - 60) / 2
            + d - 1;
    }

    public void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
    {
        int C = MathZ.Divide(daysSinceEpoch, DaysPer128YearCycle, out int D);

        // Inverse of YearForm.
        int Y = (4 * D + 3) / DaysPer4YearSubcycle;
        int d0y = D - (DaysPer4YearSubcycle * Y / 4);

        // Inverse of MonthForm & DayForm.
        m = (2 * d0y + 61) / 61;
        d = 1 + d0y - (61 * m - 60) / 2;

        y = 128 * C + Y + 1;
    }

    //
    // Formules tirées des formes quasi-affines.
    //

    [Pure]
    // We remove the unused param "y".
    public static int CountDaysInYearBeforeMonth(int m) => (61 * m - 60) / 2;

    // We remove the unused param "y".
    [Pure]
    public static int GetMonth(int doy, out int d)
    {
        int d0y = doy - 1;

        // Voir les commentaires au niveau de GetMonth().
        int m = (2 * d0y + 61) / 61;
        d = 1 + d0y - (61 * m - 60) / 2;
        return m;
    }

    [Pure]
    public static int GetYear(int daysSinceEpoch)
    {
        int C = MathZ.Divide(daysSinceEpoch, DaysPer128YearCycle, out int D);
        return 1 + 128 * C + (4 * D + 3) / DaysPer4YearSubcycle;
    }

    [Pure]
    public static int GetStartOfYear(int y)
    {
        y--;
        int C = MathZ.Divide(y, 128, out int Y);
        return DaysPer128YearCycle * C + (DaysPer4YearSubcycle * Y / 4);
    }
}
