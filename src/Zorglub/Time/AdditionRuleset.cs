// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time;

#region Developer Notes

// AdditionRule.Truncate
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
//
// AdditionRule.Overspill
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
//
// AdditionRule.Exact
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

#endregion

/// <summary>Defines the strategies employed to resolve ambiguities when adding a number of months
/// or years to a calendrical object.
/// <para><see cref="AdditionRuleset"/> is an immutable struct.</para></summary>
public readonly record struct AdditionRuleset
{
    private readonly AdditionRule _dateRule;
    /// <summary>Gets or initializes the strategy employed to resolve ambiguities when adding a
    /// number of months or years to a date.</summary>
    /// <exception cref="AoorException">value is outside the range of its valid values.</exception>
    public AdditionRule DateRule
    {
        get => _dateRule;
        init
        {
            Requires.Defined(value);
            _dateRule = value;
        }
    }

    private readonly AdditionRule _ordinalRule;
    /// <summary>Gets or initializes the strategy employed to resolve ambiguities when adding a
    /// number of years to an ordinal date.</summary>
    /// <exception cref="AoorException">value is outside the range of its valid values.</exception>
    public AdditionRule OrdinalRule
    {
        get => _ordinalRule;
        init
        {
            Requires.Defined(value);
            _ordinalRule = value;
        }
    }

    private readonly AdditionRule _monthRule;
    /// <summary>Gets or initializes the strategy employed to resolve ambiguities when adding a
    /// number of years to a month.</summary>
    /// <exception cref="AoorException">value is outside the range of its valid values.</exception>
    public AdditionRule MonthRule
    {
        get => _monthRule;
        init
        {
            Requires.Defined(value);
            _monthRule = value;
        }
    }
}

/// <summary>Specifies the strategy to resolve ambiguities when adding a number of months or years
/// to a date, or a number of years to a month.</summary>
public enum AdditionRule
{
    /// <summary>When the result is not a valid day (resp. month), return the end of the target
    /// month (resp. year).
    /// <para>This is the <i>default</i> strategy.</para></summary>
    Truncate = 0,

    /// <summary>When the result is not a valid day (resp. month), return the start of the next
    /// month (resp. year).</summary>
    Overspill,

    /// <summary>When the result is not a valid day (resp. month), return the day (resp. month)
    /// obtained by adding "roundoff" days (resp. months) to the end of the target month (resp.
    /// year).</summary>
    Exact,

    /// <summary>When the result is not a valid day (resp. month), throw an exception of type
    /// <see cref="OverflowException"/>.</summary>
    Throw,
}
