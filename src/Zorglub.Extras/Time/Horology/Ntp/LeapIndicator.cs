// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp;

// LI is a 2-bit unsigned integer. The values are in the range from 0 to 3,
// they are fixed in the sense that there is no room left for new values.
// Ignoring O, all values in LeapIndicator are fixed manually to ensure that
// (int)LeapIndicator - 1 matches the binary value.

/// <summary>
/// Specifies the warning of an impending leap second to be inserted/deleted in the last minute
/// of the current day.
/// </summary>
public enum LeapIndicator
{
    /// <summary>Default value is invalid.</summary>
    Invalid = 0,

    /// <summary>No warning.</summary>
    NoWarning = 1,

    /// <summary>Last minute has 61 seconds.</summary>
    PositiveLeapSecond = 2,

    /// <summary>Last minute has 59 seconds.</summary>
    NegativeLeapSecond = 3,

    /// <summary>Alarm condition (clock not synchronized).</summary>
    Unsynchronized = 4
}
