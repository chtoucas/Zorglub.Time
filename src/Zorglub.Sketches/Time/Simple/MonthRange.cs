// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using System.Collections.Generic;

    using Zorglub.Time.Core.Intervals;

    /// <summary>
    /// Provides static helpers and extension methods for <see cref="Range{TMonth}"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class MonthRange
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Range{T}"/> class from the specified year.
        /// </summary>
        [Pure]
        public static Range<CalendarMonth> Create(CalendarYear year) =>
            Range.Create(year.FirstMonth, year.LastMonth);

        /// <summary>
        /// Creates a new instance of the <see cref="Range{T}"/> class from the specified start and
        /// length.
        /// </summary>
        [Pure]
        public static Range<CalendarMonth> Create(CalendarMonth start, int length) =>
            Range.Create(start, start + (length - 1));

        /// <summary>
        /// Obtains the calendar to which belongs the specified range.
        /// </summary>
        [Pure]
        public static SimpleCalendar GetCalendar(this Range<CalendarMonth> range) => range.Min.Calendar;

        /// <summary>
        /// Obtains the number of months in the specified range.
        /// </summary>
        [Pure]
        public static int Count(this Range<CalendarMonth> range) => range.Max - range.Min + 1;

        /// <summary>
        /// Obtains an <see cref="IEnumerable{T}"/> view of the specified range.
        /// </summary>
        [Pure]
        public static IEnumerable<CalendarMonth> ToEnumerable(this Range<CalendarMonth> range)
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
