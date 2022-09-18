// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology;

/// <summary>Represents a clock.</summary>
public interface IClock
{
    /// <summary>Obtains a <see cref="Moment"/> value representing the current time.</summary>
    [Pure] Moment Now();

    /// <summary>Obtains a <see cref="DayNumber"/> value representing the current date.</summary>
    [Pure] DayNumber Today();
}
