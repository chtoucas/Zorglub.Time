// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp;

/// <summary>Represents an NTP code identifying a particular server or reference clock, or a "kiss
/// code".</summary>
/// <remarks>This class cannot be inherited.</remarks>
public sealed record NtpCode(NtpCodeType Type, string Value)
{
    /// <summary>Returns a culture-independent string representation of the current instance.
    /// </summary>
    public sealed override string ToString() => FormattableString.Invariant($"{Type}={Value}");
}

/// <summary>Specifies the NTP code type.</summary>
public enum NtpCodeType
{
    /// <summary>The NTP code type is unknown.</summary>
    /// <remarks>This value is considered to be <i>invalid</i>. We never use it, and neither should
    /// you.</remarks>
    Unknown = 0,

    /// <summary>The NTP code is an identifier assigned to a reference clock by the IANA.</summary>
    Identifier,

    /// <summary>The NTP code is a reserved identifier assigned to a reference clock for
    /// experimentation and development.</summary>
    PrivateIdentifier,

    /// <summary>The NTP code is an unrecognised identifier for a reference clock.</summary>
    UnknownIdentifier,

    /// <summary>The NTP code is a Kiss-of-Death code assigned by the IANA.</summary>
    KissCode,

    /// <summary>The NTP code is a reserved Kiss-of-Death code for experimentation and development.
    /// </summary>
    PrivateKissCode,

    /// <summary>The NTP code is an unrecognised Kiss-of-Death code.</summary>
    UnknownKissCode,

    /// <summary>The NTP code is a leap smear value.</summary>
    /// <remarks>"Leap Second Smearing" as done by Google and others implies that the NTP server
    /// <i>does not return time in the UTC timescale</i>.</remarks>
    LeapSecondSmearing,

    /// <summary>The NTP code is an IPv4 address -or- the first four bytes of the MD5 digest of an
    /// IPv6 address.</summary>
    IPAddressMaybe,
}
