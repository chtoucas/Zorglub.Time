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
#if false
        // Il me semble plus naturel et plus logique d'écrire :
        // - month.CalendarYear.GetStartOfYear()
        // - month.CalendarYear.GetEndOfYear()

        /// <summary>
        /// Obtains the first day of the year to which belongs the specified month.
        /// </summary>
        [Pure]
        public CalendarDate GetStartOfYear(CalendarMonth month) =>
            new(month.Parts.StartOfYear, month.Cuid);

        /// <summary>
        /// Obtains the last day of the year to which belongs the specified month.
        /// </summary>
        [Pure]
        public CalendarDate GetEndOfYear(CalendarMonth month)
        {
            var ymd = month.Calendar.Schema.GetEndOfYearParts(month.Year);
            return new CalendarDate(ymd, month.Cuid);
        }

#endif

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<CalendarDate> GetDaysInMonth(CalendarMonth month) =>
            month.GetAllDays();

        /// <inheritdoc/>
        [Pure]
        public CalendarDate GetStartOfMonth(CalendarMonth month) => month.FirstDay;

        /// <inheritdoc/>
        [Pure]
        public CalendarDate GetDayOfMonth(CalendarMonth month, int dayOfMonth) =>
            month.GetDayOfMonth(dayOfMonth);

        /// <inheritdoc/>
        [Pure]
        public CalendarDate GetEndOfMonth(CalendarMonth month) => month.LastDay;
    }
}
