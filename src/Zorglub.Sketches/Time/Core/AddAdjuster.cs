// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    // Résultat rectifié en fonction d'un "adjustement".
    //
    // Utilisation : on filtre en amont les cas roundoff = 0 (résultat exact)
    // et rule = DateAdditionRule.EndOfMonth (fonctionnement par défaut de
    // AddYears() et AddMonths()). Ces conditions ne sont pas renforcées en
    // mode RELEASE, important car les méthodes ne fonctionnent plus correctement
    // si roundoff = 0.
    [Obsolete("TO BE REMOVED")]
    internal static class AddAdjuster
    {
        // Version générique.
        [Pure]
        public static int AdjustResult(int daysSinceEpoch, int roundoff, DateAdditionRule rule)
        {
            Debug.Assert(roundoff > 0);

            return rule switch
            {
                // Si on ne filtrait pas roundoff > 0, il faudrait prendre en compte
                // le cas roundoff = 0 et retourner daysSinceEpoch (résultat exact).
                DateAdditionRule.StartOfNextMonth => checked(daysSinceEpoch + 1),
                // Si on ne filtrait pas roundoff > 0, il faudrait prendre en compte
                // le cas roundoff = 0 et retourner daysSinceEpoch (résultat exact).
                DateAdditionRule.Exact => checked(daysSinceEpoch + roundoff),
                // En faisant les choses correctement, on n'arrive jamais ici...
                // Si roundoff > 0, daysSinceEpoch est déjà positionné à "endOfMonth".
                // Si roundoff = 0, le résultat est exact, on retourne aussi daysSinceEpoch.
                DateAdditionRule.EndOfMonth => daysSinceEpoch,

                _ => Throw.ArgumentOutOfRange<int>(nameof(rule)),
            };
        }
    }
}
