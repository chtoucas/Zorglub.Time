// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using System.Collections;
    using System.Collections.Generic;

    using Zorglub.Time.Core.Intervals;

    // TODO(code): optimize enumeration and Contains (static and instance
    // methods); see DateRange.
    // Add tests to certify that it's not possible to create a range with
    // endpoints in different calendars.

    /// <summary>
    /// Provides static helpers and extension methods for <see cref="Range{TDate}"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static partial class DateRange { }

    public partial class DateRange // Range<CalendarDate>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Range{T}"/> class from the specified start and
        /// length.
        /// </summary>
        [Pure]
        public static Range<CalendarDate> Create(CalendarDate start, int length) =>
            Range.Create(start, start + (length - 1));

        /// <summary>
        /// Obtains the calendar to which belongs the specified range.
        /// </summary>
        [Pure]
        public static SimpleCalendar GetCalendar(this Range<CalendarDate> @this) => @this.Min.Calendar;

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

        ///// <summary>
        ///// Determines whether the specified range contains the specified month or not.
        ///// </summary>
        ///// <exception cref="ArgumentException"><paramref name="month"/> does not belong to the
        ///// calendar of the specified range.</exception>
        //[Pure]
        //public static bool ContainsFast(this Range<CalendarDate> @this, CalendarMonth month)
        //{
        //    var cuid = @this.GetCalendar().Id;
        //    if (month.Cuid != cuid) Throw.BadCuid(nameof(month), cuid, month.Cuid);

        //    return @this.Min.CompareFast(month.FirstDay) <= 0
        //        && month.LastDay.CompareFast(@this.Max) <= 0;
        //}

        /// <summary>
        /// Interconverts the specified range to a range within a different calendar.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
        /// </exception>
        [Pure]
        public static Range<CalendarDate> WithCalendar(this Range<CalendarDate> @this, SimpleCalendar newCalendar)
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

    public partial class DateRange // Range<CalendarDay>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Range{T}"/> class from the specified start and
        /// length.
        /// </summary>
        [Pure]
        public static Range<CalendarDay> Create(CalendarDay start, int length) =>
            Range.Create(start, start + (length - 1));

        /// <summary>
        /// Obtains the calendar to which belongs the specified range.
        /// </summary>
        [Pure]
        public static SimpleCalendar GetCalendar(this Range<CalendarDay> @this) => @this.Min.Calendar;

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
        public static Range<CalendarDay> WithCalendar(this Range<CalendarDay> @this, SimpleCalendar newCalendar)
        {
            Requires.NotNull(newCalendar);

            var (min, max) = @this.Endpoints;

            var start = min.WithCalendar(newCalendar);
            var end = max.WithCalendar(newCalendar);

            return Range.Create(start, end);
        }
    }

    public partial class DateRange // Range<OrdinalDate>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Range{T}"/> class from the specified start and
        /// length.
        /// </summary>
        [Pure]
        public static Range<OrdinalDate> Create(OrdinalDate start, int length) =>
            Range.Create(start, start + (length - 1));

        /// <summary>
        /// Obtains the calendar to which belongs the specified range.
        /// </summary>
        [Pure]
        public static SimpleCalendar GetCalendar(this Range<OrdinalDate> @this) => @this.Min.Calendar;

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
        public static Range<OrdinalDate> WithCalendar(this Range<OrdinalDate> @this, SimpleCalendar newCalendar)
        {
            Requires.NotNull(newCalendar);

            var (min, max) = @this.Endpoints;

            var start = min.WithCalendar(newCalendar);
            var end = max.WithCalendar(newCalendar);

            return Range.Create(start, end);
        }
    }

    public partial class DateRange // Enumerators
    {
        // Intervalle confiné dans une année.
        private sealed class RangeWithinYear : IEnumerable<CalendarDate>
        {
            private readonly Range<CalendarDate> _range;

            /// <summary>
            /// Initializes a new instance of the <see cref="RangeWithinYear"/> class.
            /// </summary>
            public RangeWithinYear(Range<CalendarDate> range)
            {
                _range = range;
            }

            /// <inheritdoc/>
            [Pure]
            public IEnumerator<CalendarDate> GetEnumerator()
            {
                var chr = _range.GetCalendar();
                var cuid = chr.Id;
                var sch = chr.Schema;

                var min = _range.Min;
                var count = _range.Count();

                int y = min.Year;

                int first = min.DayOfYear;
                int last = first + count - 1;
                for (int doy = first; doy <= last; doy++)
                {
                    var ymd = sch.GetDateParts(y, doy);
                    yield return new CalendarDate(ymd, cuid);
                }
            }

            [Pure]
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        // Intervalle confiné dans un mois.
        private sealed class RangeWithinMonth : IEnumerable<CalendarDate>
        {
            private readonly Range<CalendarDate> _range;

            /// <summary>
            /// Initializes a new instance of the <see cref="RangeWithinMonth"/> class.
            /// </summary>
            public RangeWithinMonth(Range<CalendarDate> range)
            {
                _range = range;
            }

            /// <inheritdoc/>
            [Pure]
            public IEnumerator<CalendarDate> GetEnumerator()
            {
                var chr = _range.GetCalendar();
                var cuid = chr.Id;

                var min = _range.Min;
                var count = _range.Count();

                min.Parts.Unpack(out int y, out int m);

                int first = min.Day;
                int last = first + count - 1;
                for (int d = first; d <= last; d++)
                {
                    yield return new CalendarDate(y, m, d, cuid);
                }
            }

            [Pure]
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
