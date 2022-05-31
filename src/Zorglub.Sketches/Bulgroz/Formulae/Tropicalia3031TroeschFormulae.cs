// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

//#define COMMON_YEAR_FORM

namespace Zorglub.Bulgroz.Formulae;

using Zorglub.Time.Core.Schemas;

using static Zorglub.Bulgroz.TropicalistaConstants;

// Formes quasi-affines:
//   (1461, 4, 0)
//   (335, 11, -330) ou (61, 2, -61)
//   (1, 1, -1)
// Formes affines inverses:
//   (4, 1461, 3)
//   (11, 335, 340) ou (2, 61, 62)
//   (1, 1, 1)
//
// Formes des mois :
// - (335, 11, -330) les années communes
// - (61, 2, -61) les années bissextiles
// On peut utiliser l'une ou l'autre sauf pour GetMonth().

/// <summary>
/// Formulae for the "Tropicália" (30-31) calendar (Troesch).
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class Tropicalia3031TroeschFormulae : ICalendricalFormulae
{
    [Pure]
    public int CountDaysSinceEpoch(int y, int m, int d)
    {
        // TODO(doc): expliquer y--

        y--;
        // Cycle de 128 années et rang dans le cycle.
        // Ce calcul peut être optimisé :
        //   int C = y >> 7;
        //   int Y = y & 127;
        // comme bien d'autres ! Voir p.ex. Tropicalia3130TroeschFormulae.
        int C = MathZ.Divide(y, 128, out int Y);

        return DaysPer128YearCycle * C
            // YearForm
            + (DaysPer4YearSubcycle * Y / 4)
            // MonthForm
#if COMMON_YEAR_FORM
            + (335 * m - 330) / 11
#else
            + 61 * (m - 1) / 2
#endif
            // DayForm
            + d - 1;
    }

    public void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
    {
        int C = MathZ.Divide(daysSinceEpoch, DaysPer128YearCycle, out int D);

        // Inverse of YearForm.
        int Y = (4 * D + 3) / DaysPer4YearSubcycle;
        int d0y = D - (DaysPer4YearSubcycle * Y / 4);

        // Inverse of MonthForm & DayForm.
#if COMMON_YEAR_FORM
        m = (11 * d0y + 340) / 335;
        d = 1 + d0y - (335 * m - 330) / 11;
#else
        m = (2 * d0y + 62) / 61;
        d = 1 + d0y - 61 * (m - 1) / 2;
#endif

        y = 128 * C + Y + 1;
    }

    //
    // Formules tirées des formes quasi-affines.
    //

    [Pure]
    public static int CountDaysInYear(int y) => GetStartOfYear(y + 1) - GetStartOfYear(y);

    [Pure]
    // We remove the unused param "y".
    public static int CountDaysInYearBeforeMonth(int m) =>
#if COMMON_YEAR_FORM
        (335 * m - 330) / 11;
#else
        61 * (m - 1) / 2;
#endif

    [Pure]
    public static int CountDaysInMonth(int y, int m)
    {
        return TropicalistaSchema.IsLeapYearImpl(y)
            ? CountDaysInLeapYearBeforeMonth(m + 1) - CountDaysInLeapYearBeforeMonth(m)
            : CountDaysInCommonYearBeforeMonth(m + 1) - CountDaysInCommonYearBeforeMonth(m);

        static int CountDaysInCommonYearBeforeMonth(int m) => (335 * m - 330) / 11;
        static int CountDaysInLeapYearBeforeMonth(int m) => 61 * (m - 1) / 2;
    }

    // We remove the unused param "y".
    [Pure]
    public static int GetMonth(int doy, out int d)
    {
        int d0y = doy - 1;

        // On utilise la forme des mois des années bissextiles.
        // Celle des années communes échouera quand doy = 366.
        int m = (2 * d0y + 62) / 61;

#if COMMON_YEAR_FORM
        d = 1 + d0y - (335 * m - 330) / 11;
#else
        d = 1 + d0y - 61 * (m - 1) / 2;
#endif
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
