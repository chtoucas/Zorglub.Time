// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
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
        /// <summary>Invalid.</summary>
        Invalid = 0,

        /// <summary>Reserved.</summary>
        Reserved,

        /// <summary>Symmetric active.</summary>
        SymmetricActive,

        /// <summary>Symmetric passive.</summary>
        SymmetricPassive,

        /// <summary>Client.</summary>
        Client,

        /// <summary>Server.</summary>
        Server,

        /// <summary>Broadcast.</summary>
        Broadcast,

        /// <summary>Reserved for NTP control message.</summary>
        ReservedForNtpControlMessage,

        /// <summary>Reserved for private use.</summary>
        ReservedForPrivateUse
    }
}
