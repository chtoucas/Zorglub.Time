// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    // Several reasons to keep this internal.
    // The names could be misleading. A solar calendar may end up with the
    // profile "Other". A "non-solar" calendar could have the profile "Solar"...
    // Also, it could be confused w/ CalendarFamily.
    // Furthermore, we wish to be able to change it (which profile is attached
    // to a schema) freely each time we feel the need to.

    /// <summary>
    /// Specifies the profile of the schema.
    /// <para>A profile gives us basic informations on the layout of a year that prove to be useful
    /// when trying to optimize <i>validation</i> and <i>arithmetic</i>. For more precise data, one
    /// should refer to the schemas themselves.</para>
    /// </summary>
    internal enum CalendricalProfile
    {
        /// <summary>
        /// Unspecified profile.
        /// </summary>
        /// <remarks>
        /// <para>For instance, in this category, we find the Ptolemaic and Egyptian calendars with
        /// a virtual thirteen month shorter than usual (strictly less than 7 days) in which case
        /// trying to bypass CountDaysInMonth() won't actually bring any performance improvement,
        /// it might even be the opposite.</para>
        /// <para>We must be careful with FastArithmetic.AddDaysViaDayOfMonth().</para>
        /// </remarks>
        Other = 0,

        /// <summary>
        /// The schema is regular.
        /// <para>Years are 12-months long and at least 365-days long, and months are at least
        /// 28-days long.</para>
        /// <para>Despite its name, this profile is NOT restricted to solar calendars, but most of
        /// them follow this pattern.</para>
        /// <para>Most solar calendars -or- Annus Vagus.</para>
        /// </summary>
        Solar12,

        /// <summary>
        /// The schema is regular.
        /// <para>Years are 13-months long and at least 365-days long, and months are at least
        /// 28-days long.</para>
        /// <para>Despite its name, this profile is NOT restricted to solar calendars, but most of
        /// them follow this pattern.</para>
        /// <para>Most reformed solar calendars.</para>
        /// </summary>
        Solar13,

        /// <summary>
        /// The schema is regular.
        /// <para>Years are 12-months long and at least 354-days long, and months are at least
        /// 29-days long.</para>
        /// <para>Despite its name, this profile is NOT restricted to lunar calendars, but most of
        /// them follow this pattern.</para>
        /// </summary>
        Lunar,

        /// <summary>
        /// The schema is NOT regular.
        /// <para>Years are at least 353-days long, and months are at least 29-days long.</para>
        /// <para>Despite its name, this profile is NOT restricted to lunisolar calendars, but most
        /// of them follow this pattern.</para>
        /// </summary>
        Lunisolar
    }
}
