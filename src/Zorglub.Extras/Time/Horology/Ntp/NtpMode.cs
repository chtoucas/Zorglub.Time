// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp;

// Mode is a 3-bit unsigned integer. The values are in the range from 0 to 7,
// they are fixed in the sense that there is no room left for new values.
// Ignoring O, all values in NtpMode are fixed manually to ensure that
// (int)NtpMode - 1 matches the binary value.

/// <summary>
/// Specifies the NTP mode.
/// </summary>
/// <remarks>
/// <para>Only <see cref="Client"/>, <see cref="Server"/> and <see cref="Broadcast"/> are used
/// by SNTP clients and servers.</para>
/// </remarks>
[SuppressMessage("Naming", "CA1700:Do not name enum values 'Reserved'", Justification = "RFC wording")]
public enum NtpMode
{
    /// <summary>
    /// The NTP mode is invalid.
    /// </summary>
    Invalid = 0,

    /// <summary>
    /// Reserved.
    /// </summary>
    Reserved = 1,

    /// <summary>
    /// Symmetric active.
    /// </summary>
    SymmetricActive = 2,

    /// <summary>
    /// Symmetric passive.
    /// </summary>
    SymmetricPassive = 3,

    /// <summary>
    /// Client.
    /// </summary>
    Client = 4,

    /// <summary>
    /// Server.
    /// </summary>
    Server = 5,

    /// <summary>
    /// Broadcast.
    /// </summary>
    Broadcast = 6,

    /// <summary>
    /// Reserved for NTP control message.
    /// </summary>
    NtpControlMessage = 7,

    /// <summary>
    /// Reserved for private use.
    /// </summary>
    ReservedForPrivateUse = 8
}
