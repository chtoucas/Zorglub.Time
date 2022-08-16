// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using System.Collections;

    using Zorglub.Time.Core.Intervals;

    public static partial class RangeExtensions2
    {
        ///// <summary>
        ///// Converts the specified range of days to a range of <see cref="CalendarDay"/>.
        ///// </summary>
        //[Pure]
        //public static Range<CalendarDay> ToCalendarDayRange(this Range<CalendarDate> range)
        //{
        //    var (min, max) = range.Endpoints;
        //    return Range.Create(min.ToCalendarDay(), max.ToCalendarDay());
        //}

        ///// <summary>
        ///// Converts the specified range of days to a range of <see cref="OrdinalDate"/>.
        ///// </summary>
        //[Pure]
        //public static Range<OrdinalDate> ToOrdinalDateRange(this Range<CalendarDate> range)
        //{
        //    var (min, max) = range.Endpoints;
        //    return Range.Create(min.ToOrdinalDate(), max.ToOrdinalDate());
        //}

        ///// <summary>
        ///// Converts the specified range of days to a range of <see cref="CalendarDate"/>.
        ///// </summary>
        //[Pure]
        //public static Range<CalendarDate> ToCalendarDateRange(this Range<CalendarDay> range)
        //{
        //    var (min, max) = range.Endpoints;
        //    return Range.Create(min.ToCalendarDate(), max.ToCalendarDate());
        //}

        ///// <summary>
        ///// Converts the specified range of days to a range of <see cref="OrdinalDate"/>.
        ///// </summary>
        //[Pure]
        //public static Range<OrdinalDate> ToOrdinalDateRange(this Range<CalendarDay> range)
        //{
        //    var (min, max) = range.Endpoints;
        //    return Range.Create(min.ToOrdinalDate(), max.ToOrdinalDate());
        //}

        ///// <summary>
        ///// Converts the specified range of days to a range of <see cref="CalendarDate"/>.
        ///// </summary>
        //[Pure]
        //public static Range<CalendarDate> ToCalendarDateRange(this Range<OrdinalDate> range)
        //{
        //    var (min, max) = range.Endpoints;
        //    return Range.Create(min.ToCalendarDate(), max.ToCalendarDate());
        //}

        ///// <summary>
        ///// Converts the specified range of days to a range of <see cref="CalendarDay"/>.
        ///// </summary>
        //[Pure]
        //public static Range<CalendarDay> ToCalendarDayRange(this Range<OrdinalDate> range)
        //{
        //    var (min, max) = range.Endpoints;
        //    return Range.Create(min.ToCalendarDay(), max.ToCalendarDay());
        //}

        ///// <summary>
        ///// Determines whether the specified range contains the specified month or not.
        ///// </summary>
        ///// <exception cref="ArgumentException"><paramref name="month"/> does not belong to the
        ///// calendar of the specified range.</exception>
        //[Pure]
        //public static bool ContainsFast(this Range<CalendarDate> range, CalendarMonth month)
        //{
        //    var cuid = range.GetCalendar().Id;
        //    if (month.Cuid != cuid) Throw.BadCuid(nameof(month), cuid, month.Cuid);

        //    return range.Min.CompareFast(month.FirstDay) <= 0
        //        && month.LastDay.CompareFast(range.Max) <= 0;
        //}

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
