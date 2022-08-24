// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Astronomy
{
    /// <summary>
    /// Provides julian dates for the epoch of all Julian Date variants.
    /// </summary>
    public static class JulianDateEpoch
    {
        /// <summary>
        /// Represents the julian date of the epoch of the Modified Julian Date.
        /// <para>This field is constant equal to 2_400_000.5.</para>
        /// </summary>
        public const double Modified = 2_400_000.5;

        /// <summary>
        /// Represents the julian date of the epoch of the CCSDS Julian Date.
        /// <para>This field is constant equal to 2_436_204.5.</para>
        /// </summary>
        public const double Ccsds = 2_436_204.5;

        /// <summary>
        /// Represents the julian date of the epoch of the CNES Julian Date.
        /// <para>This field is constant equal to 2_433_282.5.</para>
        /// </summary>
        public const double Cnes = 2_433_282.5;

        /// <summary>
        /// Represents the julian date of the epoch of the Dublin Julian Date.
        /// <para>This field is constant equal to 2_415_020.</para>
        /// </summary>
        public const int Dublin = 2_415_020;

        /// <summary>
        /// Represents the julian date of the origin of the Reduced Julian Date.
        /// <para>This field is constant equal to 2_400_000.</para>
        /// </summary>
        public const int Reduced = 2_400_000;

        /// <summary>
        /// Represents the julian date of the epoch of the Truncated Julian Date.
        /// <para>This field is constant equal to 2_400_000.5.</para>
        /// </summary>
        public const double Truncated = 2_440_000.5;
    }
}
