// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry.Forms;

/// <summary>
/// <para>Beware, the default numbering system is the <i>algebraic</i> one, not the ordinal one
/// starting at 1.</para>
/// <para><i>Intercalary days positioned at the end of a leap year.</i></para>
/// </summary>
public enum MonthFormNumbering
{
    /// <summary>
    /// The algebraic numbering of months: 0, 1, and so forth.
    /// <para>Par ex., pour le calendrier grégorien cela donne : mars 0, avril 1, ..., décembre 9,
    /// janvier 10, février 11.</para>
    /// </summary>
    Algebraic = 0,

    /// <summary>
    /// The ordinal numbering of months: 1, 2, and so forth.
    /// <para>Par ex., pour le calendrier grégorien cela donne : mars 1, avril 2, ..., décembre 10,
    /// janvier 11, février 12.</para>
    /// </summary>
    Ordinal = 1,

    /// <summary>
    /// Any other numbering system.
    /// </summary>
    Other = 2,
}
