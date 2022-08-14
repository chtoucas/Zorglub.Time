// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using System.Collections.Generic;

    using Zorglub.Time.Core.Intervals;

    /// <summary>
    /// Provides static helpers and extension methods for <see cref="Range{TYear}"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class YearRange
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Range{T}"/> class from the specified start and
        /// length.
        /// </summary>
        [Pure]
        public static Range<CalendarYear> Create(CalendarYear start, int length) =>
            Range.Create(start, start + (length - 1));

        /// <summary>
        /// Obtains the calendar to which belongs the specified range.
        /// </summary>
        [Pure]
        public static SimpleCalendar GetCalendar(this Range<CalendarYear> range) => range.Min.Calendar;

        /// <summary>
        /// Obtains the number of years in the specified range.
        /// </summary>
        [Pure]
        public static int Count(this Range<CalendarYear> range) => range.Max - range.Min + 1;

        /// <summary>
        /// Obtains an <see cref="IEnumerable{T}"/> view of the specified range.
        /// </summary>
        [Pure]
        public static IEnumerable<CalendarYear> ToEnumerable(this Range<CalendarYear> range)
        {
            var min = range.Min;
            var max = range.Max;

            for (var month = min; month <= max; month++)
            {
                yield return month;
            }
        }
    }
}
