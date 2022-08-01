// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    /// <summary>
    /// Defines methods specific to calendars featuring epagomenal days.
    /// <para>The epagomenal days are usually found in descendants of the Egyptian calendar.</para>
    /// </summary>
    public interface IBlankDay<TSelf> : IDateable where TSelf : IBlankDay<TSelf>
    {
        /// <summary>
        /// Returns true if the current instance is a blank day; otherwise returns false.
        /// </summary>
        bool IsBlank { get; }
    }
}
