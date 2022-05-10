// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    // Jours d'un décan égyptien, un cycle de 10 jours.
    //
    // Contrairement à une semaine, un décan est une **subdivision d'un mois**
    // qui en comporte trois --- l'année en compte 36. Les jours épagomènes
    // n'appartiennent à **aucun** décan.
    // Dans le calendrier républican français, on parle plutôt de décade.
    // Malheureusement, les anglais comprennent une "période de 10 ans" quand on
    // évoque une décade. En français, on évite l'ambiguïté en parlant plutôt
    // de décennie (adj. décennal).
    //
    // Attention, cette énumération ne suit pas le même schéma que DayOfWeek.
    // Ainsi le dernier jour d'un décan est numéroté 10 pas 0. Une autre
    // différence majeure est que la suite des décans est interrompue par les
    // jours épagomènes (Blank).

    /// <summary>
    /// Specifies the day of the decan.
    /// </summary>
    public enum DayOfDecan
    {
        // La valeur de Blank doit rester inférieur aux autres.
        // À confirmer quand on créera (si on crée) OnOrBefore() & co.

        /// <summary>
        /// Indicates a day outside the decan cycle.
        /// </summary>
        Blank = 0,

        /// <summary>
        /// Indicates the first day of the decan.
        /// </summary>
        First = 1,

        /// <summary>
        /// Indicates the second day of the decan.
        /// </summary>
        Second = 2,

        /// <summary>
        /// Indicates the third day of the decan.
        /// </summary>
        Third = 3,

        /// <summary>
        /// Indicates the fourth day of the decan.
        /// </summary>
        Fourth = 4,

        /// <summary>
        /// Indicates the fifth day of the decan.
        /// </summary>
        Fifth = 5,

        /// <summary>
        /// Indicates the sixth day of the decan.
        /// </summary>
        Sixth = 6,

        /// <summary>
        /// Indicates the seventh day of the decan.
        /// </summary>
        Seventh = 7,

        /// <summary>
        /// Indicates the eighth day of the decan.
        /// </summary>
        Eighth = 8,

        /// <summary>
        /// Indicates the ninth day of the decan.
        /// </summary>
        Ninth = 9,

        /// <summary>
        /// Indicates the tenth day of the decan.
        /// </summary>
        Tenth = 10
    }
}
