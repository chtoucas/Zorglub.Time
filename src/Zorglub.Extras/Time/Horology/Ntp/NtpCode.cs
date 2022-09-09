// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp;

/// <summary>
/// Specifies the NTP code.
/// </summary>
[SuppressMessage("Naming", "CA1700:Do not name enum values 'Reserved'", Justification = "<Pending>")]
public enum NtpCodeType
{
    /// <summary>
    /// The NTP code is a unknown.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// The NTP code is a four-character string assigned to the reference clock.
    /// </summary>
    Identifier,

    ReservedIdentifier,

    UnknownIdentifier,

    // Kiss-o'-Death Packet
    KissCode,

    ReservedKissCode,

    // Unknown Kiss-o'-Death Packet
    UnknownKissCode,

    LeapSecondSmearing,

    // IPv4 or MD5 digest of an IPv6 address.
    IPAddressMaybe,
}

public sealed record NtpCode(NtpCodeType Type, string Value)
{
    public sealed override string ToString() =>
        FormattableString.Invariant($"{Type}={Value}");
}
