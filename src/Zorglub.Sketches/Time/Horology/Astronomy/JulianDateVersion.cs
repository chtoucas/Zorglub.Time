// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Astronomy
{
    // https://en.wikipedia.org/wiki/Julian_day

    /// <summary>
    /// Specifies the version of a Julian Date.
    /// </summary>
    [SuppressMessage("Design", "CA1028:Enum Storage should be Int32")]
    public enum JulianDateVersion : byte
    {
        /// <summary>
        /// Astronomical Julian Date or simply Julian Date.
        /// <para>Its epoch begins on january 1st, 4713 BC (julian) at noon
        /// (12h).</para>
        /// </summary>
        Astronomical = 0,

        /// <summary>
        /// Modified Julian Date.
        /// <para>Its epoch begins on november 17th, 1858 CE (gregorian) at
        /// midnight (0h).</para>
        /// </summary>
        /// <remarks>
        /// Introduced by the SAO (Smithsonian Astrophysical Observatory).
        /// </remarks>
        Modified,

        /// <summary>
        /// Reduced Julian Date.
        /// <para>Its epoch begins on november 16th, 1858 CE (gregorian) at noon
        /// (12h).</para>
        /// </summary>
        Reduced,

        /// <summary>
        /// Truncated Julian Date.
        /// <para>Its epoch begins on may 24th, 1968 CE (gregorian) at midnight
        /// (0h).</para>
        /// </summary>
        /// <remarks>
        /// <para>Introduced by the NASA (National Aeronautics and Space
        /// Administration).</para>
        /// <para>Formally, this should be a Truncated Julian Day.</para>
        /// </remarks>
        Truncated,

        /// <summary>
        /// Dublin Julian Date.
        /// <para>Its epoch begins on december 31th, 1899 CE (gregorian) at noon
        /// (12h).</para>
        /// </summary>
        /// <remarks>
        /// Introduced by the IAU (International Astronomical Union).
        /// </remarks>
        Dublin,

        /// <summary>
        /// CNES Julian Date.
        /// <para>Its epoch begins on january 1st, 1950 CE (gregorian) at
        /// midnight (0h).</para>
        /// </summary>
        /// <remarks>
        /// Introduced by the CNES ("Centre national d'études spatiales").
        /// </remarks>
        Cnes,

        /// <summary>
        /// CCSDS Julian Date.
        /// <para>Its epoch begins on january 1st, 1958 CE (gregorian) at
        /// midnight (0h).</para>
        /// </summary>
        /// <remarks>
        /// Introduced by the CCSDS (Consultative Committee for Space Data
        /// Systems).
        /// </remarks>
        Ccsds,
    }
}
