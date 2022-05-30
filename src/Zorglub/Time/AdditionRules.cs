// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    // TODO(api): other value Overflow? Rename Exact -> Spillover?
    // Préciser que cela n'affecte que les méthodes AddYears(date)
    // et AddMonths(date), peut-être aussi Subtract()?

    public readonly record struct AdditionRules(
        DateAdditionRule DateRule,
        OrdinalAdditionRule OrdinalRule,
        MonthAdditionRule MonthRule);

    /// <summary>
    /// Specifies the strategy to resolve ambiguities that can occur after adding a number of months
    /// or years to a date.
    /// </summary>
    public enum DateAdditionRule
    {
        /// <summary>
        /// When the result is not a valid day of the month (roundoff > 0), return the last day of
        /// the month. This is the default strategy.
        /// </summary>
        // Si le résultat d'une opération arithmétique n'est pas exact (roundoff > 0),
        // on renvoie le "dernier jour du mois".
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
        // * 31/5/2016 + 1 mois = 1/7/2016 (le 31/6/2016 n'existe pas)
        // * 31/5/2016 - 1 mois = 1/5/2016 (le 31/4/2016 n'existe pas)
        // * 29/2/2016 + 1 an   = 1/3/2017 (le 29/2/2017 n'existe pas)
        // * 29/2/2016 - 1 an   = 1/3/2015 (le 29/2/2015 n'existe pas)
        // Exemples avec un décalage supérieur à 1 jour :
        // * 31/5/2015 + 9 mois = 1/3/2016 (le 31/2/2016 n'existe pas)
        // * 30/5/2015 + 9 mois = 1/3/2016 (le 30/2/2016 n'existe pas)
        // * 31/5/2015 - 3 mois = 1/3/2015 (le 31/2/2016 n'existe pas)
        // * 30/5/2015 - 3 mois = 1/3/2015 (le 30/2/2016 n'existe pas)
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
        // * 31/5/2016 + 1 mois = 1/7/2016 (le 31/6/2016 n'existe pas)
        // * 31/5/2016 - 1 mois = 1/5/2016 (le 31/4/2016 n'existe pas)
        // * 29/2/2016 + 1 an   = 1/3/2017 (le 29/2/2017 n'existe pas)
        // * 29/2/2016 - 1 an   = 1/3/2015 (le 29/2/2015 n'existe pas)
        // Exemples avec un décalage supérieur à 1 jour :
        // * 31/5/2015 + 9 mois = 3/3/2016 (le 31/2/2016 n'existe pas) décalage = +3
        // * 30/5/2015 + 9 mois = 2/3/2016 (le 30/2/2016 n'existe pas) décalage = +2
        // * 31/5/2015 - 3 mois = 3/3/2015 (le 31/2/2016 n'existe pas) décalage = +3
        // * 30/5/2015 - 3 mois = 2/3/2015 (le 30/2/2016 n'existe pas) décalage = +2
        Exact,
    }

    /// <summary>
    /// Specifies the strategy to resolve ambiguities that can occur after adding a number of months
    /// or years to an ordinal date.
    /// </summary>
    public enum OrdinalAdditionRule
    {
        EndOfYear = 0,
        StartOfNextYear,
        Exact,
    }

    /// <summary>
    /// Specifies the strategy to resolve ambiguities that can occur after adding a number of years
    /// to a calendar month.
    /// </summary>
    public enum MonthAdditionRule
    {
        EndOfYear = 0,
        StartOfNextYear,
        Exact,
    }

    /// <summary>
    /// Provides extension methods for <see cref="AddAdjustment"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal static class AdditionRuleExtensions
    {
        /// <summary>
        /// Returns true if the specified value is invalid; otherwise returns false.
        /// </summary>
        public static bool IsInvalid(this DateAdditionRule @this) =>
            @this < DateAdditionRule.EndOfMonth || @this > DateAdditionRule.Exact;

        /// <summary>
        /// Returns true if the specified value is invalid; otherwise returns false.
        /// </summary>
        public static bool IsInvalid(this OrdinalAdditionRule @this) =>
            @this < OrdinalAdditionRule.EndOfYear || @this > OrdinalAdditionRule.Exact;

        /// <summary>
        /// Returns true if the specified value is invalid; otherwise returns false.
        /// </summary>
        public static bool IsInvalid(this MonthAdditionRule @this) =>
            @this < MonthAdditionRule.EndOfYear || @this > MonthAdditionRule.Exact;
    }
}
