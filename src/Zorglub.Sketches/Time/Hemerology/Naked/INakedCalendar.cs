// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Naked
{
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Core;

    /// <summary>
    /// Defines a calendar without a companion date type.
    /// </summary>
    public partial interface INakedCalendar : ICalendar<DateParts> { }

    public partial interface INakedCalendar // Factories, conversions
    {
        /// <summary>
        /// Obtains the current date on this machine.
        /// </summary>
        /// <exception cref="AoorException">Today is not within the calendar boundaries.</exception>
        [Pure] DateParts Today() => GetDateParts(DayNumber.Today());

        /// <summary>
        /// Obtains the date parts for the specified day number.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// values supported by this calendar.</exception>
        [Pure] DateParts GetDateParts(DayNumber dayNumber);

        /// <summary>
        /// Obtains the date parts for the specified ordinal date.
        /// </summary>
        /// <exception cref="AoorException">The ordinal date is either invalid or outside the range
        /// of supported dates.</exception>
        [Pure] DateParts GetDateParts(int year, int dayOfYear);

        /// <summary>
        /// Obtains the ordinal date parts for the specified day number.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// values supported by this calendar.</exception>
        [Pure] OrdinalParts GetOrdinalParts(DayNumber dayNumber);

        /// <summary>
        /// Obtains the ordinal date parts for the specified date.
        /// </summary>
        /// <exception cref="AoorException">The date is either invalid or outside the range of
        /// supported dates.</exception>
        [Pure] OrdinalParts GetOrdinalParts(int year, int month, int day);
    }

    public partial interface INakedCalendar // Arithmetic
    {
        // REVIEW(api): supprimer AddDays().

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
        public DayNumber AddDays(DayNumber dayNumber, int days)
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
            Domain.Validate(dayNumber);
            var dayNumberOut = dayNumber + days;
            Domain.CheckOverflow(dayNumberOut);
            return dayNumberOut;
        }
    }
}
