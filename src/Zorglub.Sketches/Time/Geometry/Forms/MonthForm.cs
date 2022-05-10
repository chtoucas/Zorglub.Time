// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry.Forms
{
    /// <summary>
    /// This form encodes the sequence of lengths of the months of a year.
    /// </summary>
    /// <remarks>
    /// <para>To build such a form, we must in general change the origin of months such that the
    /// exceptionnal month holding the intercalary days becomes the last one.</para>
    /// </remarks>
    public record MonthForm(int A, int B, int Remainder, MonthFormNumbering Numbering)
        : CalendricalForm(A, B, Remainder)
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MonthForm"/> record with the default
        /// (algebraic) numbering system.
        /// </summary>
        public MonthForm(int A, int B, int Remainder)
            : this(A, B, Remainder, MonthFormNumbering.Algebraic) { }

        /// <summary>
        /// Obtains the number of whole days elapsed since the start of the year and before the
        /// specified month.
        /// </summary>
        [Pure]
        public int CountDaysInYearBeforeMonth(int m) => ValueAt(m);

        /// <summary>
        /// Obtains the month of the specified (algebraic) day of the year.
        /// </summary>
        // Il s'agit de la formule standard donnant le jour du mois :
        // > int d = doy - CountDaysInYearBeforeMonth(y, m)
        [Pure]
        public int GetMonth(int d0y, out int d0) => Divide(d0y, out d0);

        [Pure]
        public int GetMonth(int d0y) => Divide(d0y);

        /// <summary>
        /// Obtains the number of days in the specified month.
        /// </summary>
        // OK sauf peut-être pour le dernier mois, celui contenant les jours
        // bissextils ; tout dépend du domaine de validité de la forme.
        [Pure]
        public int CountDaysInMonth(int m) => CodeAt(m);
    }
}
