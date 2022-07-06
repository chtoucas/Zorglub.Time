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
    // ### Public Hierarchy
    //
    // ICalendarScope
    // └─ CalendarScope [A]
    //    ├─ BoundedBelowScope
    //    └─ MinMaxYearScope
    //
    // Annotation: [A] = abstract
    //
    // ### Full Hierarchy
    //
    // ICalendarScope
    // ├─ ProlepticShortScope [A]
    // ├─ StandardShortScope [A]
    // └─ CalendarScope [A]
    //    ├─ BoundedBelowScope
    //    └─ MinMaxYearScope
    //
    // Construction
    // ------------
    // Public ctors:
    //   BoundedBelowScope.ctor(ICalendricalSchema)
    //   MinMaxYearScope.ctor(ICalendricalSchema)
    //   ProlepticShortScope.ctor(ICalendricalSchema)
    //   StandardShortScope.ctor(ICalendricalSchema)

    // Public factory methods:
    //   MinMaxYearScope.WithMaxYear(ICalendricalSchema)
    //   MinMaxYearScope.WithMinYear(ICalendricalSchema)
    //
    // Internal factory methods:
    //   MinMaxYearScope.WithMaximalRange(ICalendricalSchema)
    //
    // Range of Supported Years
    // ------------------------
    // Only BoundedBelowScope supports ranges that do not map to complete years.
    //
    // - ProlepticShortScope    [-9998..9999]
    // - StandardShortScope     [1..9999]
    // - CalendarScope          Subintervals of [Yemoda.MinYear..Yemoda.MaxYear]
    //   - MinMaxYearScope      [minYear..maxYear] ⋂ [Yemoda.MinYear..Yemoda.MaxYear]
    //   - BoundedBelowScope    [minDate..maxYear] ⋂ [Yemoda.MinYear..Yemoda.MaxYear]

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