// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using System.Collections.Generic;

    // Do NOT replace CalendarYear or CalendarMonth by int's, it's a bad idea.
    // With an int, we can only produce dates in a single calendar, the
    // default one. With CalendarYear, this is no longer a problem.
    // See also ICalendar<TDate> and IDateAdjuster<TDate>.

    /// <summary>
    /// Provides methods to obtain dates in a year or a month.
    /// </summary>
    /// <typeparam name="TDate">The type of date object to return.</typeparam>
    public interface IDateProvider<out TDate>
    {
        //
        // CalendarYear
        //

        /// <summary>
        /// Obtains the sequence of days in the specified year.
        /// </summary>
        [Pure] IEnumerable<TDate> GetDaysInYear(CalendarYear year);

        /// <summary>
        /// Obtains the first day of the specified year.
        /// </summary>
        [Pure] TDate GetStartOfYear(CalendarYear year);

        /// <summary>
        /// Obtains the date corresponding to the specified day of the specified year.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfYear"/> is outside the range of
        /// valid values.</exception>
        [Pure] TDate GetDayOfYear(CalendarYear year, int dayOfYear);

        /// <summary>
        /// Obtains the last day of the specified year.
        /// </summary>
        [Pure] TDate GetEndOfYear(CalendarYear year);

        //
        // CalendarMonth
        //

        /// <summary>
        /// Obtains the first day of the year to which belongs the specified month.
        /// </summary>
        [Pure] TDate GetStartOfYear(CalendarMonth month);

        /// <summary>
        /// Obtains the last day of the year to which belongs the specified month.
        /// </summary>
        [Pure] TDate GetEndOfYear(CalendarMonth month);

        /// <summary>
        /// Obtains the sequence of days in the specified month.
        /// </summary>
        [Pure] IEnumerable<TDate> GetDaysInMonth(CalendarMonth month);

        /// <summary>
        /// Obtains the first day of the specified month.
        /// </summary>
        [Pure] TDate GetStartOfMonth(CalendarMonth month);

        /// <summary>
        /// Obtains the date corresponding to the specified day of the specified month.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfMonth"/> is outside the range of
        /// valid values.</exception>
        [Pure] TDate GetDayOfMonth(CalendarMonth month, int dayOfMonth);

        /// <summary>
        /// Obtains the last day of the specified month.
        /// </summary>
        [Pure] TDate GetEndOfMonth(CalendarMonth month);

    }
}
