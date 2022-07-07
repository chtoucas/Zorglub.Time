// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;

    #region Developer Notes

    // Types Implementing ICalendarScope
    // ---------------------------------
    //
    // ICalendarScope
    // ├─ MaximalScope
    // ├─ ProlepticScope
    // ├─ StandardScope
    // └─ CalendarScope [A]
    //    ├─ BoundedBelowScope
    //    └─ MinMaxYearScope
    //
    // Annotation: [A] = abstract
    //
    // Range of Supported Years
    // ------------------------
    // Only BoundedBelowScope supports ranges that do not map to an interval of
    // years.
    //
    // - ProlepticScope         [-9998..9999]
    // - StandardScope          [1..9999]
    // - CalendarScope
    //   - MinMaxYearScope      [minYear..maxYear]
    //   - BoundedBelowScope    [minDate..maxYear]

    #endregion

    /// <summary>
    /// Defines the scope of application of a calendar, an interval of days.
    /// </summary>
    public partial interface ICalendarScope : ICalendricalValidator
    {
        /// <summary>
        /// Gets the epoch.
        /// </summary>
        DayNumber Epoch { get; }

        /// <summary>
        /// Gets the range of supported <see cref="DayNumber"/> values.
        /// </summary>
        Range<DayNumber> Domain { get; }
    }
}