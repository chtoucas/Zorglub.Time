// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;

    /// <summary>
    /// Provides methods to obtain new <see cref="CalendarDate"/> instances for a given year or month.
    /// </summary>
    public sealed partial class CalendarDateProvider : IDateProvider<CalendarDate> { }

    public partial class CalendarDateProvider // CalendarYear
    {
        /// <inheritdoc/>
        [Pure]
        public IEnumerable<CalendarDate> GetDaysInYear(CalendarYear year)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        [Pure]
        public CalendarDate GetStartOfYear(CalendarYear year) =>
            new(Yemoda.AtStartOfYear(year.Year), year.Cuid);

        /// <inheritdoc/>
        [Pure]
        public CalendarDate GetDayOfYear(CalendarYear year, int dayOfYear)
        {
            ref readonly var chr = ref year.CalendarRef;
            chr.PreValidator.ValidateDayOfYear(year.Year, dayOfYear);
            var ymd = chr.Schema.GetDateParts(year.Year, dayOfYear);
            return new CalendarDate(ymd, year.Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        public CalendarDate GetEndOfYear(CalendarYear year)
        {
            ref readonly var chr = ref year.CalendarRef;
            var ymd = chr.Schema.GetEndOfYearParts(year.Year);
            return new CalendarDate(ymd, year.Cuid);
        }
    }

    public partial class CalendarDateProvider // CalendarMonth
    {
        /// <inheritdoc/>
        [Pure]
        public CalendarDate GetStartOfYear(CalendarMonth month) =>
            new(month.Parts.StartOfYear, month.Cuid);

        /// <inheritdoc/>
        [Pure]
        public CalendarDate GetEndOfYear(CalendarMonth month)
        {
            var ymd = month.Calendar.Schema.GetEndOfYearParts(month.Year);
            return new CalendarDate(ymd, month.Cuid);
        }

        [Pure]
        IEnumerable<CalendarDate> IDateProvider<CalendarDate>.GetDaysInMonth(CalendarMonth month) =>
            month.GetAllDays();

        [Pure]
        CalendarDate IDateProvider<CalendarDate>.GetStartOfMonth(CalendarMonth month) =>
            month.FirstDay;

        [Pure]
        CalendarDate IDateProvider<CalendarDate>.GetDayOfMonth(CalendarMonth month, int dayOfMonth) =>
            month.GetDayOfMonth(dayOfMonth);

        [Pure]
        CalendarDate IDateProvider<CalendarDate>.GetEndOfMonth(CalendarMonth month) =>
            month.LastDay;
    }
}
