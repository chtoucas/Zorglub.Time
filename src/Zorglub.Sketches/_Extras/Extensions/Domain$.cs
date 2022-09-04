// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extensions
{
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Validation;

    // Pour les tests, voir ceux qui ont été commentés dans
    // Zorglub.Testing.Facts.Hemerology.INakedCalendarFacts.

    /// <summary>
    /// Provides extension methods for <see cref="Range{DayNumber}"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class DomainExtensions
    {
        /// <summary>
        /// Adds a number of days to the specified day number.
        /// </summary>
        /// <remarks>
        /// <para>
        /// You only need this method if you do NOT intend to pass the result to another calendar
        /// method, and you want to be sure that the result is valid for the current calendar.
        /// Otherwise always prefer the simpler and faster plain addition: dayNumber + days.
        /// </para>
        /// </remarks>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// values supported by this calendar.</exception>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported dates.</exception>
        [Pure]
        public static DayNumber AddDays(this Range<DayNumber> domain, DayNumber dayNumber, int days)
        {
            // Ajouter un nombre de jours à un dayNumber est toujours une
            // opération légitime et se fait en dehors de tout calendrier. Ici,
            // il s'agit d'une version stricte de DayNumber.PlusDays() qui
            // garantit que le résultat reste dans les limites du calendrier.
            // Cependant comme la plupart du temps on passe immédiatement le
            // résultat à GetDateParts() qui valide le dayNumber, on peut souvent
            // ignorer cette méthode et se contenter d'écrire
            // > GetDateParts(dayNumber + days);
            //
            // On pourrait ne pas valider dayNumber mais seulement le résultat,
            // cependant pour rester cohérent avec le reste de cette classe,
            // on valide à l'entrée et à la sortie.
            domain.Validate(dayNumber);
            var dayNumberOut = dayNumber + days;
            domain.CheckOverflow(dayNumberOut);
            return dayNumberOut;
        }
    }
}
