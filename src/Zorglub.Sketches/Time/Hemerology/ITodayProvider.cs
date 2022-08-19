// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1000 // Do not declare static members on generic types (Design) 👈 PreviewFeatures

namespace Zorglub.Time.Hemerology
{
    // TODO(api): Today(zone). UtcToday()
    // - if static, add to IFixedDay<TSelf>?
    // - if not static, add to ICalendar<TDate> or IDateProvider<TDate>?

    /// <summary>
    /// Provides methods to obtain the current date.
    /// </summary>
    /// <typeparam name="TDate">The type of date object to return.</typeparam>
    public interface ITodayProvider<out TDate>
    {
        /// <summary>
        /// Obtains the current date in the default calendar on this machine, expressed in local
        /// time, not UTC.
        /// </summary>
        /// <exception cref="AoorException">Today is not within the calendar boundaries.</exception>
        [Pure] static abstract TDate Today();
    }
}
