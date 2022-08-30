// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    public enum NtpMode
    {
        Unspecified = 0,

        SymmetricActive,
        SymmetricPassive,
        Client,
        Server,
        Broadcast,

        // Reserved values

        NtpControlMessage,
        Special7
    }
}
