// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    // FIXME(api): name, purpose.
    // CalendarDay should implement this interface.

    public interface IFixedDateable : IDateable
    {
        /// <summary>
        /// Gets the day number.
        /// </summary>
        DayNumber DayNumber { get; }

        /// <summary>
        /// Gets the count of days since the epoch.
        /// </summary>
        int DaysSinceEpoch { get; }
    }
}
