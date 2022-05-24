// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using System.Collections.Generic;

    using Zorglub.Time.Core.Intervals;

    // TODO(code): optimize enumeration (see DateRange).
    // Range.Create(date, length).
    // Add tests to certify that it's not possible to create a range with
    // endpoints in different calendars.

    /// <summary>
    /// Provides static helpers and extension methods for <see cref="Range{T}"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static partial class SimpleRange
    {
    }

    public partial class SimpleRange // Range<CalendarDate>
    {
        /// <summary>
        /// Obtains the calendar to which belongs the specified range.
        /// </summary>
        [Pure]
        public static Calendar GetCalendar(this Range<CalendarDate> @this) => @this.Min.Calendar;

        /// <summary>
        /// Obtains the number of days in the specified range.
        /// </summary>
        [Pure]
        public static int Count(this Range<CalendarDate> @this) => @this.Max - @this.Min + 1;

        /// <summary>
        /// Obtains an <see cref="IEnumerable{T}"/> view of the specified range.
        /// </summary>
        [Pure]
        public static IEnumerable<CalendarDate> ToEnumerable(this Range<CalendarDate> @this)
        {
            var min = @this.Min;
            var max = @this.Max;

            for (var date = min; date <= max; date++)
            {
                yield return date;
            }
        }

        /// <summary>
        /// Interconverts the specified range to a range within a different calendar.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
        /// </exception>
        [Pure]
        public static Range<CalendarDate> WithCalendar(this Range<CalendarDate> @this, Calendar newCalendar)
        {
            Requires.NotNull(newCalendar);

            // TODO(code): check that both dates use the same calendar; idem with
            // the other WithCalendar().
            var (min, max) = @this.Endpoints;

            var start = min.WithCalendar(newCalendar);
            var end = max.WithCalendar(newCalendar);

            return Range.Create(start, end);
        }
    }

    public partial class SimpleRange // Range<CalendarDay>
    {
        /// <summary>
        /// Obtains the calendar to which belongs the specified range.
        /// </summary>
        [Pure]
        public static Calendar GetCalendar(this Range<CalendarDay> @this) => @this.Min.Calendar;

        /// <summary>
        /// Obtains the number of days in the specified range.
        /// </summary>
        [Pure]
        public static int Count(this Range<CalendarDay> @this) => @this.Max - @this.Min + 1;

        /// <summary>
        /// Obtains an <see cref="IEnumerable{T}"/> view of the specified range.
        /// </summary>
        [Pure]
        public static IEnumerable<CalendarDay> ToEnumerable(this Range<CalendarDay> @this)
        {
            var min = @this.Min;
            var max = @this.Max;

            for (var date = min; date <= max; date++)
            {
                yield return date;
            }
        }

        /// <summary>
        /// Interconverts the specified range to a range within a different calendar.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
        /// </exception>
        [Pure]
        public static Range<CalendarDay> WithCalendar(this Range<CalendarDay> @this, Calendar newCalendar)
        {
            Requires.NotNull(newCalendar);

            var (min, max) = @this.Endpoints;

            var start = min.WithCalendar(newCalendar);
            var end = max.WithCalendar(newCalendar);

            return Range.Create(start, end);
        }
    }

    public partial class SimpleRange // Range<OrdinalDate>
    {
        /// <summary>
        /// Obtains the calendar to which belongs the specified range.
        /// </summary>
        [Pure]
        public static Calendar GetCalendar(this Range<OrdinalDate> @this) => @this.Min.Calendar;

        /// <summary>
        /// Obtains the number of days in the specified range.
        /// </summary>
        [Pure]
        public static int Count(this Range<OrdinalDate> @this) => @this.Max - @this.Min + 1;

        /// <summary>
        /// Obtains an <see cref="IEnumerable{T}"/> view of the specified range.
        /// </summary>
        [Pure]
        public static IEnumerable<OrdinalDate> ToEnumerable(this Range<OrdinalDate> @this)
        {
            var min = @this.Min;
            var max = @this.Max;

            for (var date = min; date <= max; date++)
            {
                yield return date;
            }
        }

        /// <summary>
        /// Interconverts the specified range to a range within a different calendar.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
        /// </exception>
        [Pure]
        public static Range<OrdinalDate> WithCalendar(this Range<OrdinalDate> @this, Calendar newCalendar)
        {
            Requires.NotNull(newCalendar);

            var (min, max) = @this.Endpoints;

            var start = min.WithCalendar(newCalendar);
            var end = max.WithCalendar(newCalendar);

            return Range.Create(start, end);
        }
    }

    public partial class SimpleRange // Range<CalendarMonth>
    {
        /// <summary>
        /// Obtains the calendar to which belongs the specified range.
        /// </summary>
        [Pure]
        public static Calendar GetCalendar(this Range<CalendarMonth> @this) => @this.Min.Calendar;

        /// <summary>
        /// Obtains the number of months in the specified range.
        /// </summary>
        [Pure]
        public static int Count(this Range<CalendarMonth> @this) => @this.Max - @this.Min + 1;

        /// <summary>
        /// Obtains an <see cref="IEnumerable{T}"/> view of the specified range.
        /// </summary>
        [Pure]
        public static IEnumerable<CalendarMonth> ToEnumerable(this Range<CalendarMonth> @this)
        {
            var min = @this.Min;
            var max = @this.Max;

            for (var month = min; month <= max; month++)
            {
                yield return month;
            }
        }
    }

    public partial class SimpleRange // Range<CalendarYear>
    {
        /// <summary>
        /// Obtains the calendar to which belongs the specified range.
        /// </summary>
        [Pure]
        public static Calendar GetCalendar(this Range<CalendarYear> @this) => @this.Min.Calendar;

        /// <summary>
        /// Obtains the number of years in the specified range.
        /// </summary>
        [Pure]
        public static int Count(this Range<CalendarYear> @this) => @this.Max - @this.Min + 1;

        /// <summary>
        /// Obtains an <see cref="IEnumerable{T}"/> view of the specified range.
        /// </summary>
        [Pure]
        public static IEnumerable<CalendarYear> ToEnumerable(this Range<CalendarYear> @this)
        {
            var min = @this.Min;
            var max = @this.Max;

            for (var month = min; month <= max; month++)
            {
                yield return month;
            }
        }
    }
}
