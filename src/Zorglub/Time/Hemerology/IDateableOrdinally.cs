// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    public interface IDateableOrdinally
    {
        /// <summary>
        /// Gets the day number.
        /// </summary>
        DayNumber DayNumber { get; }

        /// <summary>
        /// Gets the count of days since the epoch of the calendar to which belongs the current
        /// instance.
        /// </summary>
        int DaysSinceEpoch { get; }
    }
}
