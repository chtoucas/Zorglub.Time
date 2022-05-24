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
        [Pure]
        IEnumerable<OrdinalDate> IDateProvider<OrdinalDate>.GetDaysInYear(CalendarYear year) =>
            year.GetAllDays();

        [Pure]
        OrdinalDate IDateProvider<OrdinalDate>.GetStartOfYear(CalendarYear year) =>
            year.FirstDay;

        [Pure]
        OrdinalDate IDateProvider<OrdinalDate>.GetDayOfYear(CalendarYear year, int dayOfYear) =>
            year.GetDayOfYear(dayOfYear);

        [Pure]
        OrdinalDate IDateProvider<OrdinalDate>.GetEndOfYear(CalendarYear year) =>
            year.LastDay;
    }

    public partial class OrdinalDateProvider // CalendarMonth
    {
        [Pure]
        OrdinalDate IDateProvider<OrdinalDate>.GetStartOfYear(CalendarMonth month) =>
            month.CalendarYear.FirstDay;

        [Pure]
        OrdinalDate IDateProvider<OrdinalDate>.GetEndOfYear(CalendarMonth month) =>
            month.CalendarYear.LastDay;

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
