// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core;

// Un jour épagomène est un des 5 ou 6 jours ajoutés en fin d'année
// d'un calendrier composé de 12 mois de 30 jours pour synchroniser les
// années avec le cycle solaire.
// Un jour épagomène ne fait partie d'aucun mois, cependant pour des
// questions d'ordre technique on le rattache au douzième mois.
// Ex. : le jour de la révolution du calendrier républicain.

/// <summary>Defines methods specific to calendrical schemas featuring epagomenal days.
/// <para>The epagomenal days are usually found in descendants of the Egyptian calendar.</para>
/// </summary>
public interface IEpagomenalDayFeaturette : ICalendricalKernel
{
    /// <summary>Determines whether the specified date is an epagomenal day or not, and also returns
    /// the epagomenal number of the day in an output parameter, zero if the date is not an
    /// epagomenal day.</summary>
    [Pure] bool IsEpagomenalDay(int y, int m, int d, out int epagomenalNumber);
}
