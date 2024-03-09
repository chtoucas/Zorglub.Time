// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

#pragma warning disable CA1822 // Mark members as static (Performance)

namespace Zorglub.Bulgroz.Formulae;

using Zorglub.Time.Core.Schemas;

using static Zorglub.Bulgroz.GJConstants;
using static Zorglub.Bulgroz.GregorianConstants;

// Very close to what we do in GregorianFormulae, but
// 1. We use the 400-year cycle, not the centuries.
// 2. We also modify the months after February.
// 3. It uses the "natural" formula to compute GetStartOfYear().

/// <summary>
/// Formulae for the Gregorian calendar (Hinnant).
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class GregorianHinnantFormulae : ICalendricalFormulae
{
    [Pure]
    public int CountDaysSinceEpoch(int y, int m, int d)
    {
        // On prétend que le premier mois de l'année est le mois de mars,
        // numérotation commençant à zéro.
        // > m = (month + 9) % 12;
        if (m < 3)
        {
            // Janvier (resp. février) devient l'avant-dernier (resp.
            // dernier) mois de l'année précédente.
            y--;
            m += 9;
        }
        else
        {
            m -= 3; // mars = 0, avril = 1, etc.
        }

        // 400-years cycle, the year number in the cycle.
        int C = MathZ.Divide(y, 400, out int Y);

        // Décompte du nombre de jour depuis le début du cycle de 400 ans.
        int D =
            // Nombre de jours compris entre le début du cycle et le début
            // de notre année "Y". Comme Y < 400, Y/400 est toujours égal
            // à zéro, on peut donc ignorer le décompte des années séculaires
            // multiples de 400.
            DaysInCommonYear * Y + (Y >> 2) - Y / 100
            // Forme quasi-affine (153, 5, 2), inverse = (5, 153, 2).
            // Voir CountDaysInYearBeforeMonth().
            + (int)((uint)(153 * m + 2) / 5)
            // Nombre de jours depuis le début du mois.
            + d - 1;

        return -DaysInYearAfterFebruary + DaysPer400YearCycle * C + D;
    }

    public void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
    {
        daysSinceEpoch += DaysInYearAfterFebruary;

        // C = 400-years cycle, D = day number in the cycle.
        int C = MathZ.Divide(daysSinceEpoch, DaysPer400YearCycle, out int D);
        // The year number in the cycle.
        int Y = (D - D / 1460 + D / DaysPer100YearSubcycle - D / 146_096) / 365;

        int doy = D - DaysInCommonYear * Y - (Y >> 2) + Y / 100;

        m = (5 * doy + 2) / 153;
        d = 1 + doy - (153 * m + 2) / 5;

        if (m > 9)
        {
            Y++;
            m -= 9;
        }
        else
        {
            m += 3;
        }

        y = 400 * C + Y;
    }

    // Détermination empirique de la forme quasi-affine (153, 5, 2).
    //
    // Nombre de jours compris entre le début de l'année "Y" et le
    // début du mois "m" qui, encore une fois, n'est pas le mois
    // au sens ordinaire mais qui permet d'ignorer le problème posé
    // normalement par le mois de février (géré automatiquement
    // quand on ajoute day - 1 à la fin).
    //   N = (153 * m + 2) / 5
    // Attention, / = division euclidienne, ie il y a une partie
    // entière cachée. Cette formule peut paraître un peu mystérieuse,
    // mais elle permet juste d'obtenir la suite d'entiers
    //   0, 31, 61, 92, 122, 153, 184, 214, 245, 275, 306, 337
    // par un calcul purement algébrique. Comparez avec les tableaux
    // s_DaysAtStartOfMonthCommonYear et s_DaysAtStartOfMonthLeapYear
    // de AltGregorian.
    //
    // Si on prétend que tous les mois font exactement 30 jours,
    // on approche de près la réalité :
    //   mois  = 0 1 2 3 4 5 6 7 8 9 10 11
    //   delta = 0 1 1 2 2 3 4 4 5 5 6  7
    // on voit facilement que
    //   N = 30 * m + (m + 1 + m/5)/2
    // Comme il s'agit d'une division euclidienne on ne peut pas
    // simplifier aveuglément l'expression, mais... dans notre cas
    // particulier ça marche :
    //   N = 30 * m + (6 * m + 5) / 10
    // On vérifie aisément que remplacer 5 par 4 ne change pas le
    // résultat, d'où la formule proposée par Hinnant.
    [Pure]
    public int CountDaysInYearBeforeMonth(int y, int m)
    {
        // Formula if the year started in march (algebraic months):
        //   (153 * m + 2) / 5
        // NB: 457 = 3 * 153 - 2, of course this is not accidental.
        if (m < 3)
        {
            // Of course, this can be done more simply.
            m += 9;
            return (153 * m + 2) / 5 - DaysInYearAfterFebruary;
        }
        else
        {
            m -= 3;
            return (153 * m + 2) / 5 + (GregorianFormulae.IsLeapYear(y) ? 60 : 59);
        }
    }
}
