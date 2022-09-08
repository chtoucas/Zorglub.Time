// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp;

public enum ReferenceCodeType
{
    Unknown = 0,

    Primary,

    UnknownPrimary,

    KissCode,

    UnknownKissCode,

    LeapSecondSmearing,

    // IPv4 or MD5 digest of an IPv6 address.
    MaybeIPv4Address,
}

public sealed record ReferenceCode(ReferenceCodeType Type, string Value)
{
    public sealed override string ToString() =>
        FormattableString.Invariant($"{Type}={Value}");
}
