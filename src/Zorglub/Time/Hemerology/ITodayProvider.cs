// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    /// <summary>
    /// Provides a method to obtain the current date.
    /// </summary>
    public interface ITodayProvider
    {
        /// <summary>
        /// Obtains a <see cref="DayNumber"/> value representing the current date.
        /// </summary>
        [Pure] DayNumber Today();
    }
}
