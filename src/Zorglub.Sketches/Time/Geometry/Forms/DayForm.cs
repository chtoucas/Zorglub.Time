// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry.Forms
{
    // On étend CalendricalForm uniqt pour mettre en évidence le fait qu'il
    // s'agit d'une forme quasi-affine. Mis à part ça, ça ne sert à rien...

    /// <summary>
    /// This form encodes the (trivial) sequence of the days of a month, ie 0, 1, 2, and so forth.
    /// <para>Elle a pour rôle de déplacer l'origine de la numérotation des jours du mois de 0 à 1.
    /// </para>
    /// </summary>
    public sealed record DayForm : CalendricalForm
    {
        internal DayForm() : base(1, 1, -1) { }

        /// <summary>
        /// Obtains the number of whole days elapsed since the start of the month and before the
        /// specified day of the month.
        /// <para>Equivalently, it returns the algebraic day of the month of the specified (ordinary)
        /// day of the month.</para>
        /// </summary>
        [Pure]
        public static int CountDaysInMonthBeforeDay(int d) => d - 1;

        /// <summary>
        /// Obtains the (ordinary) day of the month of the specified algebraic day of the month.
        /// </summary>
        [Pure]
        public static int GetDay(int d0) => 1 + d0;

        [Pure]
        internal int CountDaysInMonthBeforeDayCore(int d) => ValueAt(d);

        // We can ignore the remainder which is always equal to 0.
        [Pure]
        internal int GetDayCore(int d0) => Divide(d0);
    }
}
