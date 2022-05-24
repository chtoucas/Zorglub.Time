// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;

    /// <summary>
    /// Provides methods to obtain new <see cref="OrdinalDate"/> instances for a given year or month.
    /// </summary>
    public sealed partial class OrdinalDateProvider : IDateProvider<OrdinalDate> { }

    public partial class OrdinalDateProvider // CalendarYear
    {
        /// <inheritdoc/>
        [Pure]
        public IEnumerable<OrdinalDate> GetDaysInYear(CalendarYear year) => year.GetAllDays();

        /// <inheritdoc/>
        [Pure]
        public OrdinalDate GetStartOfYear(CalendarYear year) => year.FirstDay;

        /// <inheritdoc/>
        [Pure]
        public OrdinalDate GetDayOfYear(CalendarYear year, int dayOfYear) =>
            year.GetDayOfYear(dayOfYear);

        /// <inheritdoc/>
        [Pure]
        public OrdinalDate GetEndOfYear(CalendarYear year) => year.LastDay;
    }

    public partial class OrdinalDateProvider // CalendarMonth
    {
#if false

        /// <summary>
        /// Obtains the first day of the year to which belongs the specified month.
        /// </summary>
        [Pure]
        public OrdinalDate GetStartOfYear(CalendarMonth month)
        {
            var ydoy = month.Calendar.Schema.GetStartOfYearOrdinalParts(month.Year);
            return new OrdinalDate(ydoy, month.Cuid);
        }

        /// <summary>
        /// Obtains the last day of the year to which belongs the specified month.
        /// </summary>
        [Pure]
        public OrdinalDate GetEndOfYear(CalendarMonth month)
        {
            var ydoy = month.Calendar.Schema.GetEndOfYearOrdinalParts(month.Year);
            return new OrdinalDate(ydoy, month.Cuid);
        }

#endif

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<OrdinalDate> GetDaysInMonth(CalendarMonth month)
        {
            var sch = month.Calendar.Schema;
            month.Parts.Unpack(out int y, out int m);
            int startOfMonth = sch.CountDaysInYearBeforeMonth(y, m);
            int daysInMonth = sch.CountDaysInMonth(y, m);

            for (int d = 1; d <= daysInMonth; d++)
            {
                yield return new OrdinalDate(y, startOfMonth + d, month.Cuid);
            }
        }

        /// <inheritdoc/>
        [Pure]
        public OrdinalDate GetStartOfMonth(CalendarMonth month)
        {
            month.Parts.Unpack(out int y, out int m);
            ref readonly var chr = ref month.CalendarRef;
            var ydoy = chr.Schema.GetStartOfMonthOrdinalParts(y, m);
            return new OrdinalDate(ydoy, month.Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        public OrdinalDate GetDayOfMonth(CalendarMonth month, int dayOfMonth)
        {
            month.Parts.Unpack(out int y, out int m);
            ref readonly var chr = ref month.CalendarRef;
            chr.ValidateDayOfMonth(y, m, dayOfMonth);
            int doy = chr.Schema.GetDayOfYear(y, m, dayOfMonth);
            return new OrdinalDate(new Yedoy(y, doy), month.Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        public OrdinalDate GetEndOfMonth(CalendarMonth month)
        {
            month.Parts.Unpack(out int y, out int m);
            ref readonly var chr = ref month.CalendarRef;
            var ydoy = chr.Schema.GetEndOfMonthOrdinalParts(y, m);
            return new OrdinalDate(ydoy, month.Cuid);
        }
    }
}
