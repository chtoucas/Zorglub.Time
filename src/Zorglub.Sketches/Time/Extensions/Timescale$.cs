// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Extensions;

using Zorglub.Time.Horology.Astronomy;

/// <summary>
/// Provides extension methods for <see cref="Timescale"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static class TimescaleExtensions
{
    /// <summary>
    /// Returns the standard abbreviated name of the specified time scale.
    /// </summary>
    public static string GetAbbrName(this Timescale scale) =>
        scale switch
        {
            Timescale.Atomic => "TAI",
            Timescale.Terrestrial => "TT",
            Timescale.Universal => "UT1",
            Timescale.Unspecified => "???",
            Timescale.Utc => "UTC",
            _ => Throw.ArgumentOutOfRange<string>(nameof(scale)),
        };

    /// <summary>
    /// Returns the english name of the specified time scale.
    /// </summary>
    public static string GetEnglishName(this Timescale scale) =>
        scale switch
        {
            Timescale.Atomic => "International Atomic Time",
            Timescale.Terrestrial => "Terrestrial Time",
            Timescale.Universal => "Universal Time",
            Timescale.Unspecified => "unspecified",
            Timescale.Utc => "Coordinated Universal Time",
            _ => Throw.ArgumentOutOfRange<string>(nameof(scale)),
        };

    /// <summary>
    /// Retourne le nom en français de l'échelle de temps.
    /// </summary>
    public static string GetFrenchName(this Timescale scale)
        => scale switch
        {
            Timescale.Atomic => "temps atomique international",
            Timescale.Terrestrial => "temps terrestre",
            Timescale.Universal => "temps universel",
            Timescale.Unspecified => "non précisée",
            Timescale.Utc => "temps universel coordonné",
            _ => Throw.ArgumentOutOfRange<string>(nameof(scale)),
        };
}
