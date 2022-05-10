// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry.Forms
{
    /// <summary>
    /// This form encodes the sequence of lengths of the centuries in a
    /// calendrical cycle.
    /// </summary>
    /// <remarks>
    /// <para>To build such a form, we must in general change the origin of
    /// centuries (the epoch) such that the exceptionnal century becomes the
    /// last one.</para>
    /// </remarks>
    public sealed record CenturyForm(int DaysPerCycle, int CenturiesPerCycle, int Remainder)
        : CalendricalForm(DaysPerCycle, CenturiesPerCycle, Remainder)
    {
        [Pure]
        public CenturyForm PatchValue(int days) =>
            this with { Remainder = Remainder + CenturiesPerCycle * days };

        /// <summary>
        /// Counts the number of consecutive days from the epoch to the first
        /// day of the specified century.
        /// </summary>
        [Pure]
        public int GetStartOfCentury(int c) => ValueAt(c);

        /// <summary>
        /// Obtains the century of the specified count of consecutive days since
        /// the epoch.
        /// </summary>
        [Pure]
        public int GetCentury(int daysSinceEpoch, out int daysSinceStartOfCentury) =>
            Divide(daysSinceEpoch, out daysSinceStartOfCentury);

        /// <summary>
        /// Obtains the number of days in the specified century.
        /// </summary>
        [Pure]
        public int CountDaysInCentury(int c) => CodeAt(c);
    }
}
