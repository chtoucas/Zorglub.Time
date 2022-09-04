// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    /// <summary>
    /// Specifies the warning of an impending leap second to be inserted/deleted in the last minute
    /// of the current day.
    /// </summary>
    public enum LeapIndicator
    {
        /// <summary>Default value is invalid.</summary>
        Invalid = 0,

        /// <summary>No warning.</summary>
        NoWarning,

        /// <summary>Last minute has 61 seconds.</summary>
        PositiveLeapSecond,

        /// <summary>Last minute has 59 seconds.</summary>
        NegativeLeapSecond,

        /// <summary>Alarm condition (clock not synchronized).</summary>
        Alarm
    }
}
