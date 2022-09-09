// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp;

// The binary value for a stratum is a 3-bit unsigned integer.
// Contrary to LeapIndicator and NtpMode, there is no one-to-one mapping
// between NtpStratum and the binary values.

/// <summary>
/// Specifies the NTP stratum.
/// </summary>
[SuppressMessage("Naming", "CA1700:Do not name enum values 'Reserved'", Justification = "RFC wording")]
public enum NtpStratum
{
    /// <summary>
    /// The NTP stratum is invalid.
    /// </summary>
    Invalid = 0,

    /// <summary>
    /// Unspecified or unavailable (Kiss-o'-Death message).
    /// </summary>
    Unspecified,

    /// <summary>
    /// Primary reference (e.g., synchronized by radio clock).
    /// </summary>
    PrimaryReference,

    /// <summary>
    /// Secondary reference (synchronized by NTP or SNTP).
    /// </summary>
    SecondaryReference,

    /// <summary>
    /// Unsynchronized.
    /// </summary>
    Unsynchronized,

    /// <summary>
    /// Reserved.
    /// </summary>
    Reserved
}
