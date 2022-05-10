// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extensions
{
    using Zorglub.Time.Horology;

    /// <summary>
    /// Provides extension methods for <see cref="Timescale"/>.
    /// </summary>
    public static class TimescaleExtensions
    {
        /// <summary>
        /// Returns the standard abbreviated name of the specified timescale.
        /// </summary>
        public static string GetAbbrName(this Timescale @this) =>
            @this switch
            {
                Timescale.Atomic => "TAI",
                Timescale.Terrestrial => "TT",
                Timescale.Universal => "UT1",
                Timescale.Unspecified => "???",
                Timescale.Utc => "UTC",
                _ => Throw.ArgumentOutOfRange<string>(nameof(@this)),
            };

        /// <summary>
        /// Returns the english name of the specified timescale.
        /// </summary>
        public static string GetEnglishName(this Timescale @this) =>
            @this switch
            {
                Timescale.Atomic => "International Atomic Time",
                Timescale.Terrestrial => "Terrestrial Time",
                Timescale.Universal => "Universal Time",
                Timescale.Unspecified => "unspecified",
                Timescale.Utc => "Coordinated Universal Time",
                _ => Throw.ArgumentOutOfRange<string>(nameof(@this)),
            };

        /// <summary>
        /// Retourne le nom en français de l'échelle de temps.
        /// </summary>
        public static string GetFrenchName(this Timescale @this)
            => @this switch
            {
                Timescale.Atomic => "temps atomique international",
                Timescale.Terrestrial => "temps terrestre",
                Timescale.Universal => "temps universel",
                Timescale.Unspecified => "non précisée",
                Timescale.Utc => "temps universel coordonné",
                _ => Throw.ArgumentOutOfRange<string>(nameof(@this)),
            };
    }
}
