// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    /// <summary>
    /// Defines an adjustable date type.
    /// </summary>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    [Obsolete("To be removed.")]
    public interface IAdjustableDate<TSelf> : IDateable
        where TSelf : IAdjustableDate<TSelf>
    {
        ///// <summary>
        ///// Adjusts the current instance using the specified adjuster.
        ///// <para>If the adjuster throws, this method will propagate the exception.</para>
        ///// </summary>
        ///// <exception cref="ArgumentNullException"><paramref name="adjuster"/> is null.</exception>
        //[Pure] TSelf Adjust(Func<TSelf, TSelf> adjuster);

        /// <summary>
        /// Adjusts the year field to the specified value, yielding a new date.
        /// </summary>
        /// <exception cref="AoorException">The resulting date would be invalid.</exception>
        [Pure] TSelf WithYear(int newYear);

        /// <summary>
        /// Adjusts the month field to the specified value, yielding a new date.
        /// </summary>
        /// <exception cref="AoorException">The resulting date would be invalid.</exception>
        [Pure] TSelf WithMonth(int newMonth);

        /// <summary>
        /// Adjusts the day of the month field to the specified value, yielding a new date.
        /// </summary>
        /// <exception cref="AoorException">The resulting date would be invalid.</exception>
        [Pure] TSelf WithDay(int newDay);
    }
}
