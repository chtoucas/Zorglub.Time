// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1822 // Mark members as static (Performance)

//#define TROESCH_JD

namespace Zorglub.Bulgroz.Formulae;

using static Zorglub.Bulgroz.GJConstants;
using static Zorglub.Bulgroz.GregorianConstants;

/// <summary>
/// Formulae for the Gregorian calendar (Troesch).
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class GregorianTroeschFormulae : IInterconversionFormulae
{
#if TROESCH_JD
    /// <summary>Constant = 1_721_426; JDN = 1_721_425.5</summary>
    private const int EpochJD = 1_721_426;
    /// <summary>Constant = -6_884_480</summary>
    private const int __1224 = 4 * DaysInYearAfterFebruary - 4 * EpochJD;
#else
    private const int __1224 = 4 * DaysInYearAfterFebruary;
#endif

    // Version utilisée dans GregorianSchema.
    [Pure]
    public int CountDaysSinceEpochNonVirtual(int y, int m, int d)
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

        return -DaysInYearAfterFebruary
            + (DaysPer400YearCycle * C >> 2) + (DaysPer4YearSubcycle * Y >> 2)
            + (int)((uint)(153 * m + 2) / 5) + d - 1
            ;
    }

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

        int C = MathZ.Divide(y, 100, out int Y);

        return
            ((DaysPer400YearCycle * C - __1224) >> 2) + (DaysPer4YearSubcycle * Y >> 2)
            + (int)((uint)(153 * m + 2) / 5) + d - 1
#if TROESCH_JD
            - EpochJD;
#else
            ;
#endif
    }

    // Inspiré de Hinnant (cycle de 400 ans) ainsi que de Troesch (formes quasi-affines).
    [Pure]
    public int CountDaysSinceEpoch400(int y, int m, int d)
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

        // La seule réelle différence avec BasicGregorianSchemaTroesch est
        // qu'on y divise par 100 puis 4, alors qu'ici on divise par 400
        // puis 100.
        // Pro-GregorianTroesch400Formulae:
        // - en divisant par 400, on réduit les chances de débordements
        //   arithmétiques quand on calcule (DaysPer400YearCycle * C).
        // - on n'a qu'une seule division dans Z ; dans la version "Troesch"
        //   il y en a deux.
        // Pro-GregorianTroeschFormulae:
        // - la division par 4 est "optimale",
        //   (DaysPer400YearCycle * C - 1224) / 4, alors qu'ici on
        //   doit calculer (Y / 100).
        int C = MathZ.Divide(y, 400, out int Y);

        // Ici, on n'a pas intérêt à "incorporer" DaysInYearAfterFebruary
        // dans la forme des années car le numérateur pourrait alors être
        // négatif.
        int D = (DaysPer4YearSubcycle * Y >> 2) - Y / 100
            + (int)((uint)(153 * m + 2) / 5) + d - 1;

        return -DaysInYearAfterFebruary + DaysPer400YearCycle * C + D;
    }

    public void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
    {
#if TROESCH_JD
        daysSinceEpoch += EpochJD;
#endif

        int C = MathZ.Divide((daysSinceEpoch << 2) + 3 + __1224, DaysPer400YearCycle);
        int D = daysSinceEpoch - ((DaysPer400YearCycle * C - __1224) >> 2);

        int Y = ((D << 2) + 3) / DaysPer4YearSubcycle;
        int d0y = D - (DaysPer4YearSubcycle * Y >> 2);

        m = (5 * d0y + 2) / 153;
        d = 1 + d0y - (153 * m + 2) / 5;

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
}
