// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extensions
{
    using System.Collections;

    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Simple;

    /// <summary>
    /// Provides extension methods for <see cref="CalendarYear"/>, <see cref="CalendarMonth"/> and
    /// range of days.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static partial class SimpleRangeExtensions { }

    public partial class SimpleRangeExtensions // Interconversion
    {
        // In general, it's not possible to interconvert a range of years or
        // months. For that, one must first convert the range to a range of days.

        /// <summary>
        /// Interconverts the specified year to a range of days within a different calendar.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
        /// </exception>
        [Pure]
        public static Range<CalendarDay> WithCalendar(this CalendarYear year, SimpleCalendar newCalendar)
        {
            Requires.NotNull(newCalendar);

            var min = newCalendar.GetDate(year.FirstDay.DayNumber);
            var max = newCalendar.GetDate(year.LastDay.DayNumber);

            return Range.Create(min, max);
        }

        /// <summary>
        /// Interconverts the specified month to a range of days within a different calendar.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
        /// </exception>
        [Pure]
        public static Range<CalendarDay> WithCalendar(this CalendarMonth month, SimpleCalendar newCalendar)
        {
            Requires.NotNull(newCalendar);

            var min = newCalendar.GetDate(month.FirstDay.DayNumber);
            var max = newCalendar.GetDate(month.LastDay.DayNumber);

            return Range.Create(min, max);
        }

        /// <summary>
        /// Interconverts the specified range of days to a range within a different calendar.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
        /// </exception>
        [Pure]
        public static Range<CalendarDay> WithCalendar(this Range<CalendarDate> range, SimpleCalendar newCalendar)
        {
            Requires.NotNull(newCalendar);

            var (min, max) = range.Endpoints;

            var start = newCalendar.GetDate(min.DayNumber);
            var end = newCalendar.GetDate(max.DayNumber);

            return Range.Create(start, end);
        }

        /// <summary>
        /// Interconverts the specified range of days to a range within a different calendar.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
        /// </exception>
        [Pure]
        public static Range<CalendarDay> WithCalendar(this Range<CalendarDay> range, SimpleCalendar newCalendar)
        {
            Requires.NotNull(newCalendar);

            var (min, max) = range.Endpoints;

            var start = newCalendar.GetDate(min.DayNumber);
            var end = newCalendar.GetDate(max.DayNumber);

            return Range.Create(start, end);
        }

        /// <summary>
        /// Interconverts the specified range of days to a range within a different calendar.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
        /// </exception>
        [Pure]
        public static Range<CalendarDay> WithCalendar(this Range<OrdinalDate> range, SimpleCalendar newCalendar)
        {
            Requires.NotNull(newCalendar);

            var (min, max) = range.Endpoints;

            var start = newCalendar.GetDate(min.DayNumber);
            var end = newCalendar.GetDate(max.DayNumber);

            return Range.Create(start, end);
        }
    }

    public partial class SimpleRangeExtensions
    {
        /// <summary>
        /// Converts the specified range of days to a range of <see cref="CalendarDay"/>.
        /// </summary>
        [Pure]
        public static Range<CalendarDay> ToCalendarDayRange(this Range<CalendarDate> range)
        {
            var (min, max) = range.Endpoints;
            return Range.Create(min.ToCalendarDay(), max.ToCalendarDay());
        }

        /// <summary>
        /// Converts the specified range of days to a range of <see cref="OrdinalDate"/>.
        /// </summary>
        [Pure]
        public static Range<OrdinalDate> ToOrdinalDateRange(this Range<CalendarDate> range)
        {
            var (min, max) = range.Endpoints;
            return Range.Create(min.ToOrdinalDate(), max.ToOrdinalDate());
        }

        /// <summary>
        /// Converts the specified range of days to a range of <see cref="CalendarDate"/>.
        /// </summary>
        [Pure]
        public static Range<CalendarDate> ToCalendarDateRange(this Range<CalendarDay> range)
        {
            var (min, max) = range.Endpoints;
            return Range.Create(min.ToCalendarDate(), max.ToCalendarDate());
        }

        /// <summary>
        /// Converts the specified range of days to a range of <see cref="OrdinalDate"/>.
        /// </summary>
        [Pure]
        public static Range<OrdinalDate> ToOrdinalDateRange(this Range<CalendarDay> range)
        {
            var (min, max) = range.Endpoints;
            return Range.Create(min.ToOrdinalDate(), max.ToOrdinalDate());
        }

        /// <summary>
        /// Converts the specified range of days to a range of <see cref="CalendarDate"/>.
        /// </summary>
        [Pure]
        public static Range<CalendarDate> ToCalendarDateRange(this Range<OrdinalDate> range)
        {
            var (min, max) = range.Endpoints;
            return Range.Create(min.ToCalendarDate(), max.ToCalendarDate());
        }

        /// <summary>
        /// Converts the specified range of days to a range of <see cref="CalendarDay"/>.
        /// </summary>
        [Pure]
        public static Range<CalendarDay> ToCalendarDayRange(this Range<OrdinalDate> range)
        {
            var (min, max) = range.Endpoints;
            return Range.Create(min.ToCalendarDay(), max.ToCalendarDay());
        }
    }

    public partial class SimpleRangeExtensions
    {
        /// <summary>
        /// Determines whether the specified range contains the specified month or not.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="month"/> does not belong to the
        /// calendar of the specified range.</exception>
        [Pure]
        public static bool Contains(this Range<CalendarDate> range, CalendarMonth month)
        {
            var cuid = range.GetCalendar().Id;
            if (month.Cuid != cuid) Throw.BadCuid(nameof(month), cuid, month.Cuid);

            return range.Min.CompareFast(month.FirstDay) <= 0
                && month.LastDay.CompareFast(range.Max) <= 0;
        }
    }

    public partial class SimpleRangeExtensions
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
