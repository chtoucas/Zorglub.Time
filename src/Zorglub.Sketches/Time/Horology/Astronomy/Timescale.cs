// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Astronomy
{
    /// <summary>
    /// Specifies the time scale.
    /// </summary>
    [SuppressMessage("Design", "CA1028:Enum Storage should be Int32")]
    public enum Timescale : byte
    {
        /// <summary>
        /// Unspecified scale.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// Universal Time (UT1).
        /// </summary>
        Universal,

        /// <summary>
        /// International Atomic Time (TAI).
        /// </summary>
        Atomic,

        /// <summary>
        /// Terrestrial Time (TT).
        /// </summary>
        Terrestrial,

        /// <summary>
        /// Coordinated Universal Time (UTC).
        /// </summary>
        Utc,
    }
}
