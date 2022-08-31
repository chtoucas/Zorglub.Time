// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    /// <summary>
    /// Specifies the NTP stratum.
    /// </summary>
    [SuppressMessage("Naming", "CA1700:Do not name enum values 'Reserved'", Justification = "RFC wording")]
    public enum NtpStratum
    {
        /// <summary>Invalid.</summary>
        Invalid = 0,

        /// <summary>Unspecified or unavailable (kiss-o'-death message).</summary>
        Unavailable,

        /// <summary>Primary reference (e.g., synchronized by radio clock).</summary>
        PrimaryReference,

        /// <summary>Secondary reference (synchronized by NTP or SNTP).</summary>
        SecondaryReference,

        /// <summary>Reserved.</summary>
        Reserved
    }
}
