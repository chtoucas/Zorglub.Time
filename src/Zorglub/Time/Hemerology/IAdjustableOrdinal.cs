// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    // See comments in IAdjustableDate.

    /// <summary>
    /// Defines an adjustable date type.
    /// </summary>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    public interface IAdjustableOrdinal<TSelf> : IDateable
        where TSelf : IAdjustableOrdinal<TSelf>
    {
        /// <summary>
        /// Adjusts the year field to the specified value, yielding a new date.
        /// </summary>
        /// <exception cref="AoorException">The resulting date would be invalid.</exception>
        [Pure] TSelf WithYear(int newYear);

        /// <summary>
        /// Adjusts the day of the year field to the specified value, yielding a new date.
        /// </summary>
        /// <exception cref="AoorException">The resulting date would be invalid.</exception>
        [Pure] TSelf WithDayOfYear(int newDayOfYear);
    }
}
