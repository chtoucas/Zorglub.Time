// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    // TODO(code): blank-days should be kept outside the week cycle, review all
    // methods that compute the days of the week.

    // The use of blank-days can be traced back to Rev. Hugh Jones (1745) and
    // was rediscovered later by Abbot Marco Mastrofini (1834).
    // Also it seems that the "same idea had been thought of ~1650 years earlier
    // c. 100 BCE and incorporated into the calendar used by the Qumran
    // community"; see the wikipedia page
    // https://en.wikipedia.org/wiki/Hugh_Jones_(professor)

    /// <summary>
    /// Defines methods specific to calendrical schemas featuring blank days.
    /// </summary>
    /// <remarks>
    /// <para>A blank-day schema is a solar schema that adds one extra blank
    /// day on common years and two on leap years. A blank day does not belong
    /// to any month and is kept outside the weekday cycle.</para>
    /// <para>For technical reasons, we pretend that a blank day is the last day
    /// of the preceding month.</para>
    /// <para>Blank-day calendars belong to the larger family of perennial
    /// calendars.</para>
    /// </remarks>
    public interface IBlankDayFeaturette : ICalendricalKernel
    {
        /// <summary>
        /// Determines whether the specified date is a blank day or not.
        /// </summary>
        [Pure] bool IsBlankDay(int y, int m, int d);
    }
}
