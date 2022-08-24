// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extensions
{
    using Zorglub.Time.Horology.Astronomy;

    /// <summary>
    /// Provides extension methods for <see cref="JulianDateVersion"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class JulianDateVersionExtensions
    {
        /// <summary>
        /// Returns the short name of the specified Julian Date version.
        /// </summary>
        public static string GetShortName(this JulianDateVersion version) =>
            version switch
            {
                JulianDateVersion.Astronomical => "JD",
                JulianDateVersion.Modified => "MJD",
                JulianDateVersion.Ccsds => "CCSDS JD",
                JulianDateVersion.Cnes => "CNES JD",
                JulianDateVersion.Dublin => "DJD",
                JulianDateVersion.Reduced => "RJD",
                JulianDateVersion.Truncated => "TJD",
                _ => Throw.ArgumentOutOfRange<string>(nameof(version)),
            };

        /// <summary>
        /// Returns the english name of the specified Julian Date version.
        /// </summary>
        public static string GetEnglishName(this JulianDateVersion version) =>
            version switch
            {
                JulianDateVersion.Astronomical => "Julian Date",
                JulianDateVersion.Modified => "Modified Julian Date",
                JulianDateVersion.Ccsds => "CCSDS Julian Date",
                JulianDateVersion.Cnes => "CNES Julian Date",
                JulianDateVersion.Dublin => "Dublin Julian Date",
                JulianDateVersion.Reduced => "Reduced Julian Date",
                JulianDateVersion.Truncated => "Truncated Julian Date",
                _ => Throw.ArgumentOutOfRange<string>(nameof(version)),
            };
    }
}
