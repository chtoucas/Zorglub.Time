// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    /// <summary>
    /// Defines the strategies employed to resolve ambiguities when adding a number of months or
    /// years to a calendrical object.
    /// <para><see cref="AdditionRuleset"/> is an immutable struct.</para>
    /// </summary>
    public readonly record struct AdditionRuleset
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionRuleset"/> struct.
        /// </summary>
        /// <exception cref="AoorException">One of the parameters is outside the range of its valid
        /// values.</exception>
        public AdditionRuleset(
            AdditionRule dateRule,
            AdditionRule ordinalRule,
            AdditionRule monthRule)
        {
            if (dateRule < AdditionRule.Truncate || dateRule > AdditionRule.Throw)
                Throw.ArgumentOutOfRange(nameof(dateRule));
            if (ordinalRule < AdditionRule.Truncate || ordinalRule > AdditionRule.Throw)
                Throw.ArgumentOutOfRange(nameof(ordinalRule));
            if (monthRule < AdditionRule.Truncate || monthRule > AdditionRule.Throw)
                Throw.ArgumentOutOfRange(nameof(monthRule));

            DateRule = dateRule;
            OrdinalRule = ordinalRule;
            MonthRule = monthRule;
        }

        /// <summary>
        /// Gets the strategy employed to resolve ambiguities when adding a number of months or
        /// years to a date.
        /// </summary>
        public AdditionRule DateRule { get; }

        /// <summary>
        /// Gets the strategy employed to resolve ambiguities when adding a number of years to an
        /// ordinal date.
        /// </summary>
        public AdditionRule OrdinalRule { get; }

        /// <summary>
        /// Gets the strategy employed to resolve ambiguities when adding a number of years to a
        /// month.
        /// </summary>
        public AdditionRule MonthRule { get; }
    }

    /// <summary>
    /// Specifies the strategy to resolve ambiguities when adding a number of months or years to a
    /// date, or a number of years to a month.
    /// <para><see cref="Truncate"/> is the <i>default</i> strategy.</para>
    /// </summary>
    public enum AdditionRule
    {
        /// <summary>
        /// When the result is not a valid day (resp. month), return the end of the target month
        /// (resp. year).
        /// <para>This is the <i>default</i> strategy.</para>
        /// </summary>
        // Si le résultat d'une opération arithmétique n'est pas exact (roundoff > 0),
        // on retourne le "dernier jour du mois cible".
        // Par exemple, pour les calendriers grégorien et julien,
        // * 31/5/2016 + 1 mois = 30/6/2016 (le 31/6/2016 n'existe pas)
        // * 31/5/2016 - 1 mois = 30/4/2016 (le 31/4/2016 n'existe pas)
        // * 29/2/2016 + 1 an   = 28/2/2017 (le 29/2/2017 n'existe pas)
        // * 29/2/2016 - 1 an   = 28/2/2015 (le 29/2/2015 n'existe pas)
        // Exemples avec un décalage supérieur à 1 jour :
        // * 31/5/2015 + 9 mois = 28/2/2016 (le 31/2/2016 n'existe pas)
        // * 30/5/2015 + 9 mois = 28/2/2016 (le 30/2/2016 n'existe pas)
        // * 31/5/2015 - 3 mois = 28/2/2015 (le 31/2/2016 n'existe pas)
        // * 30/5/2015 - 3 mois = 28/2/2015 (le 30/2/2016 n'existe pas)
        Truncate = 0,

        /// <summary>
        /// When the result is not a valid day (resp. month), return the start of the next month
        /// (resp. year).
        /// </summary>
        // Premier jour du mois suivant.
        // Par exemple, pour les calendriers grégorien et julien,
        // * 31/5/2016 + 1 mois = 1/7/2016
        // * 31/5/2016 - 1 mois = 1/5/2016
        // * 29/2/2016 + 1 an   = 1/3/2017
        // * 29/2/2016 - 1 an   = 1/3/2015
        // Exemples avec un décalage supérieur à 1 jour :
        // * 31/5/2015 + 9 mois = 1/3/2016
        // * 30/5/2015 + 9 mois = 1/3/2016
        // * 31/5/2015 - 3 mois = 1/3/2015
        // * 30/5/2015 - 3 mois = 1/3/2015
        Overspill,

        /// <summary>
        /// When the result is not a valid day (resp. month), return the day (resp. month) obtained
        /// by adding "roundoff" days (resp. months) to the end of the target month (resp. year).
        /// </summary>
        // On décale d'autant de jours que nécessaire pour supprimer l'arrondi.
        // Par exemple, pour les calendriers grégorien et julien, décalage = +1,
        // identique à TowardPast,
        // * 31/5/2016 + 1 mois = 1/7/2016
        // * 31/5/2016 - 1 mois = 1/5/2016
        // * 29/2/2016 + 1 an   = 1/3/2017
        // * 29/2/2016 - 1 an   = 1/3/2015
        // Exemples avec un décalage supérieur à 1 jour :
        // * 31/5/2015 + 9 mois = 3/3/2016 (décalage = +3)
        // * 30/5/2015 + 9 mois = 2/3/2016 (décalage = +2)
        // * 31/5/2015 - 3 mois = 3/3/2015 (décalage = +3)
        // * 30/5/2015 - 3 mois = 2/3/2015 (décalage = +2)
        Exact,

        /// <summary>
        /// When the result is not a valid day (resp. month), throw an exception of
        /// type <see cref="OverflowException"/>.
        /// </summary>
        Throw,
    }
}
