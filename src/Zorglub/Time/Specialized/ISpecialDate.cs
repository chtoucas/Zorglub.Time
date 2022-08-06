// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized
{
    using Zorglub.Time.Hemerology;

    public interface ISpecialDate : IFixedDay
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
