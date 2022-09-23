// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

// Adapted from SOFA source code.

namespace Zorglub.Time.Horology.Astronomy;

using Zorglub.Time.Core;

// FIXME: domaine de validité des formules To/FromYemoda() ?
// Pour le moment on ne prend pas de risque, JDN >= 0.
// Plus tard, utiliser les méthodes de GregorianCalculator ?
// FromJD() -> domaine de validité ?

/// <summary>
/// Provides static methods for common operations with Julian day number.
/// </summary>
internal static class Jdn
{
    /// <summary>
    /// Represents the smallest possible value of a Julian day number.
    /// <para>This field is constant.</para>
    /// </summary>
    public const int MinValue = 0;

    /// <summary>
    /// Represents the largest possible value of a Julian day number.
    /// <para>This field is constant.</para>
    /// </summary>
    public const int MaxValue = 384_705_945;

    /// <summary>
    /// Obtains the Julian day number from the specified two-part JD and
    /// also returns the fraction of the day in an output parameter.
    /// <para>This method does NOT validate its parameters.</para>
    /// </summary>
    /// <remarks>Adapted from SOFA::iauJd2cal.</remarks>
    [Pure]
    public static int FromJD(double hi, double lo, out double fractionOfDay)
    {
        // On s'assure que |hi| >= |lo|.
        if (Math.Abs(hi) < Math.Abs(lo))
        {
            (hi, lo) = (lo, hi);
        }
        // On s'aligne avec minuit (0h) car fod = fraction du jour et
        // l'échelle julienne démarre à midi.
        lo -= .5;

        // Calcul de la fraction du jour (0 <= fractionOfDay < 1).
        // dp = decimal part
        double dpHi = hi % 1;
        double dpLo = lo % 1;
        fractionOfDay = (dpHi + dpLo) % 1;
        if (fractionOfDay < 0) { fractionOfDay += 1; }

        // Calcul du jour julien.
        double d = MathOperations.RoundAwayFromZero(hi - dpHi)
            + MathOperations.RoundAwayFromZero(lo - dpLo)
            + MathOperations.RoundAwayFromZero(dpHi + dpLo - fractionOfDay);

        return checked(1 + (int)MathOperations.RoundAwayFromZero(d));
    }

    /// <summary>
    /// Converts a Julian day number to a gregorian date.
    /// <para>This method does NOT validate its parameters.</para>
    /// </summary>
    /// <remarks>Adapted from SOFA::iauJd2cal.</remarks>
    [Pure]
    public static Yemoda ToYemoda(int jdn)
    {
        int l = jdn + 68_569;

        int n = 4 * l / 146_097;
        l -= (146_097 * n + 3) / 4;

        int i = 4000 * (l + 1) / 1_461_001;
        l -= 1461 * i / 4 - 31;

        int k = 80 * l / 2447;

        // Jour.
        int d = l - 2447 * k / 80;
        // Mois.
        l = k / 11;
        int m = k + 2 - 12 * l;
        // Année.
        int y = 100 * (n - 49) + i + l;

        return new Yemoda(y, m, d);
    }

    /// <summary>
    /// Obtains the Julian day number from the specified gregorian date.
    /// <para>This method does NOT validate its parameters.</para>
    /// </summary>
    [Pure]
    public static int FromYemoda(Yemoda ymd)
    {
        var (y, m, d) = ymd;

        int y0 = (m - 14) / 12;
        int y1 = y + y0;

        return 1461 * (y1 + 4800) / 4
            + 367 * (m - 2 - 12 * y0) / 12
            - 3 * ((y1 + 4900) / 100) / 4
            + d - 32_075;
    }
}
