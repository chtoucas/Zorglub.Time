// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extensions
{
    using Zorglub.Time.Simple;

    [Obsolete("Use the methods from CalendarDate.")]
    public static partial class DateResult { }

    public partial class DateResult // CalendarYear
    {
        /// <summary>
        /// Obtains the first day of the specified year.
        /// </summary>
        [Pure]
        public static CalendarDate GetStartOfYear(this CalendarYear @this) =>
            CalendarDate.AtStartOfYear(@this);

        /// <summary>
        /// Obtains the date corresponding to the specified day of the specified
        /// year.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfYear"/> is
        /// outside the range of valid values.</exception>
        [Pure]
        public static CalendarDate GetDayOfYear(this CalendarYear @this, int dayOfYear) =>
            CalendarDate.AtDayOfYear(@this, dayOfYear);

        /// <summary>
        /// Obtains the last day of the specified year.
        /// </summary>
        [Pure]
        public static CalendarDate GetEndOfYear(this CalendarYear @this) =>
            CalendarDate.AtEndOfYear(@this);
    }

    public partial class DateResult // CalendarMonth.
    {
#if false
        // Il me semble plus naturel et plus logique d'écrire :
        // - @this.CalendarYear.GetStartOfYear()
        // - @this.CalendarYear.GetEndOfYear()

        /// <summary>
        /// Obtains the first day of the year to which belongs the specified month.
        /// </summary>
        [Pure]
        public static CalendarDate GetStartOfYear(this CalendarMonth @this) =>
            new(@this.Parts.StartOfYear, @this.Cuid);

        /// <summary>
        /// Obtains the last day of the year to which belongs the specified month.
        /// </summary>
        [Pure]
        public static CalendarDate GetEndOfYear(this CalendarMonth @this)
        {
            var ymd = @this.Calendar.Schema.GetEndOfYearParts(@this.Year);
            return new CalendarDate(ymd, @this.Cuid);
        }

#endif

        //
        // Dates in a given month.
        //
        // Now these are methods/props on CalendarMonth.

        /// <summary>
        /// Obtains the first day of the specified month.
        /// </summary>
        [Pure]
        public static CalendarDate GetStartOfMonth(this CalendarMonth @this) => @this.FirstDay;

        /// <summary>
        /// Obtains the date corresponding to the specified day of the specified
        /// month.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfMonth"/> is
        /// outside the range of valid values.</exception>
        [Pure]
        public static CalendarDate GetDayOfMonth(this CalendarMonth @this, int dayOfMonth) =>
            @this.GetDayOfMonth(dayOfMonth);

        /// <summary>
        /// Obtains the last day of the specified month.
        /// </summary>
        [Pure]
        public static CalendarDate GetEndOfMonth(this CalendarMonth @this) => @this.LastDay;
    }
}
