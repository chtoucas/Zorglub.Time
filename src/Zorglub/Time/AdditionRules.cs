// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    // TODO(api): Overflow value -> rename, which type of exception should we throw?

    /// <summary>
    /// Defines the strategies employed to resolve overflows when adding a number of months or
    /// years to a calendrical object.
    /// <para><see cref="AdditionRules"/> is an immutable struct.</para>
    /// </summary>
    public readonly record struct AdditionRules
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionRules"/> struct.
        /// </summary>
        /// <exception cref="AoorException">One of the parameters is outside the range of its valid
        /// values.</exception>
        public AdditionRules(
            DateAdditionRule dateRule,
            OrdinalAdditionRule ordinalRule,
            MonthAdditionRule monthRule)
        {
            if (dateRule < DateAdditionRule.EndOfMonth || dateRule > DateAdditionRule.Overflow)
                Throw.ArgumentOutOfRange(nameof(dateRule));
            if (ordinalRule < OrdinalAdditionRule.EndOfYear || ordinalRule > OrdinalAdditionRule.Overflow)
                Throw.ArgumentOutOfRange(nameof(ordinalRule));
            if (monthRule < MonthAdditionRule.EndOfYear || monthRule > MonthAdditionRule.Overflow)
                Throw.ArgumentOutOfRange(nameof(monthRule));

            DateRule = dateRule;
            OrdinalRule = ordinalRule;
            MonthRule = monthRule;
        }

        /// <summary>
        /// Gets the strategy employed to resolve overflows when adding a number of months or
        /// years to a date.
        /// </summary>
        public DateAdditionRule DateRule { get; }

        /// <summary>
        /// Gets the strategy employed to resolve overflows when adding a number of years to an
        /// ordinal date.
        /// </summary>
        public OrdinalAdditionRule OrdinalRule { get; }

        /// <summary>
        /// Gets the strategy employed to resolve overflows when adding a number of years to a
        /// month.
        /// </summary>
        public MonthAdditionRule MonthRule { get; }
    }

    /// <summary>
    /// Specifies the strategy to resolve overflows when adding a number of months or years to a
    /// date.
    /// </summary>
    public enum DateAdditionRule
    {
        /// <summary>
        /// When the result is not a valid day of the month (roundoff > 0), return the last day of
        /// the month. This is the default strategy.
        /// </summary>
        // Si le résultat d'une opération arithmétique n'est pas exact (roundoff > 0),
        // on retourne le "dernier jour du mois".
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
        EndOfMonth = 0,

        /// <summary>
        /// When the result is not a valid day of the month (roundoff > 0), return the first day of
        /// the next month.
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
        StartOfNextMonth,

        /// <summary>
        /// When the result is not a valid day of the month (roundoff > 0), return the date
        /// obtained by adding "roundoff" days to the end of the month (no rounding). For most
        /// calendars it is simply the day of the next month whose day of the month is equal to
        /// "roundoff".
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
        /// When the result is not a valid day of the month (roundoff > 0), throw an exception of
        /// type <see cref="OverflowException"/>.
        /// </summary>
        Overflow,
    }

    /// <summary>
    /// Specifies the strategy to resolve overflows when adding a number of years to an ordinal
    /// date.
    /// </summary>
    public enum OrdinalAdditionRule
    {
        /// <summary>
        /// When the result is not a valid day of the year (roundoff > 0), return the last day of
        /// the year. This is the default strategy.
        /// </summary>
        EndOfYear = 0,

        /// <summary>
        /// When the result is not a valid day of the year (roundoff > 0), return the first day of
        /// the next year.
        /// </summary>
        StartOfNextYear,

        /// <summary>
        /// When the result is not a valid day of the month (roundoff > 0), return the date
        /// obtained by adding "roundoff" days to the end of the year (no rounding). For most
        /// calendars it is simply the day of the next year whose day of the year is equal to
        /// "roundoff".
        /// </summary>
        Exact,

        /// <summary>
        /// When the result is not a valid day of the year (roundoff > 0), throw an exception of
        /// type <see cref="OverflowException"/>.
        /// </summary>
        Overflow,
    }

    /// <summary>
    /// Specifies the strategy to resolve overflows when adding a number of years to a calendar
    /// month.
    /// </summary>
    public enum MonthAdditionRule
    {
        /// <summary>
        /// When the result is not a valid month of the year (roundoff > 0), return the last month
        /// of the year. This is the default strategy.
        /// </summary>
        EndOfYear = 0,

        /// <summary>
        /// When the result is not a valid day of the year (roundoff > 0), return the first month of
        /// the next year.
        /// </summary>
        StartOfNextYear,

        /// <summary>
        /// When the result is not a valid month (roundoff > 0), return the month obtained by adding
        /// "roundoff" months to the end of the year (no rounding). For most calendars it is simply
        /// the month of the next year whose month of the year is equal to "roundoff".
        /// </summary>
        Exact,

        /// <summary>
        /// When the result is not a valid month of the year (roundoff > 0), throw an exception of
        /// type <see cref="OverflowException"/>.
        /// </summary>
        Overflow,
    }
}
