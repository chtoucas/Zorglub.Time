// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    /// <summary>
    /// Defines methods specific to calendars featuring epagomenal days.
    /// <para>The epagomenal days are usually found in descendants of the Egyptian calendar.</para>
    /// </summary>
    public interface IEpagomenalDay : IDateable
    {
        /// <summary>
        /// Determines whether the current instance is an epagomenal day or not, and also returns the
        /// epagomenal number of the day in an output parameter, zero if the date is not an
        /// epagomenal day.
        /// </summary>
        [Pure] bool IsEpagomenal(out int epagomenalNumber);
    }
}
