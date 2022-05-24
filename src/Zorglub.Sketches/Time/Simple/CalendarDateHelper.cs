// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;

    /// <summary>
    /// Provides helper methods for <see cref="CalendarDate"/>.
    /// </summary>
    public sealed partial class CalendarDateHelper : IDateHelper<CalendarDate>
    {
        public CalendarDateHelper(CalendarDate date)
        {
            Date = date;
        }

        public CalendarDate Date { get; }
    }

    public partial class CalendarDateHelper //
    {
        /// <inheritdoc/>
        [Pure]
        public CalendarDate GetStartOfYear() =>
            new(Date.Parts.StartOfYear, Date.Cuid);

        /// <inheritdoc/>
        [Pure]
        public CalendarDate GetEndOfYear()
        {
            ref readonly var chr = ref Date.CalendarRef;
            var ymd = chr.Schema.GetEndOfYearParts(Date.Year);
            return new CalendarDate(ymd, Date.Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        public CalendarDate GetStartOfMonth() =>
            new(Date.Parts.StartOfMonth, Date.Cuid);

        /// <inheritdoc/>
        [Pure]
        public CalendarDate GetEndOfMonth()
        {
            Date.Parts.Unpack(out int y, out int m);
            ref readonly var chr = ref Date.CalendarRef;
            var ymd = chr.Schema.GetEndOfMonthParts(y, m);
            return new CalendarDate(ymd, Date.Cuid);
        }
    }

    public partial class CalendarDateHelper // CalendarYear
    {
        /// <inheritdoc/>
        [Pure]
        public static IEnumerable<CalendarDate> GetDaysInYear(CalendarYear year)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        [Pure]
        public static CalendarDate GetStartOfYear(CalendarYear year) =>
            new(Yemoda.AtStartOfYear(year.Year), year.Cuid);

        /// <inheritdoc/>
        [Pure]
        public static CalendarDate GetDayOfYear(CalendarYear year, int dayOfYear)
        {
            ref readonly var chr = ref year.CalendarRef;
            chr.PreValidator.ValidateDayOfYear(year.Year, dayOfYear);
            var ymd = chr.Schema.GetDateParts(year.Year, dayOfYear);
            return new CalendarDate(ymd, year.Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        public static CalendarDate GetEndOfYear(CalendarYear year)
        {
            ref readonly var chr = ref year.CalendarRef;
            var ymd = chr.Schema.GetEndOfYearParts(year.Year);
            return new CalendarDate(ymd, year.Cuid);
        }
    }

    public partial class CalendarDateHelper // CalendarMonth
    {
#if false
        // Il me semble plus naturel et plus logique d'écrire :
        // - month.CalendarYear.GetStartOfYear()
        // - month.CalendarYear.GetEndOfYear()

        /// <summary>
        /// Obtains the first day of the year to which belongs the specified month.
        /// </summary>
        [Pure]
        public static CalendarDate GetStartOfYear(CalendarMonth month) =>
            new(month.Parts.StartOfYear, month.Cuid);

        /// <summary>
        /// Obtains the last day of the year to which belongs the specified month.
        /// </summary>
        [Pure]
        public static CalendarDate GetEndOfYear(CalendarMonth month)
        {
            var ymd = month.Calendar.Schema.GetEndOfYearParts(month.Year);
            return new CalendarDate(ymd, month.Cuid);
        }

#endif

        /// <inheritdoc/>
        [Pure]
        public static IEnumerable<CalendarDate> GetDaysInMonth(CalendarMonth month)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        [Pure]
        public static CalendarDate GetStartOfMonth(CalendarMonth month) => month.FirstDay;

        /// <inheritdoc/>
        [Pure]
        public static CalendarDate GetDayOfMonth(CalendarMonth month, int dayOfMonth) =>
            month.GetDayOfMonth(dayOfMonth);

        /// <inheritdoc/>
        [Pure]
        public static CalendarDate GetEndOfMonth(CalendarMonth month) => month.LastDay;
    }
}
