// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using System.Collections.Generic;

    // Do NOT replace CalendarYear or CalendarMonth by int, it's a bad idea.
    // With an int, we can only produce dates in a single calendar, the
    // default one. With CalendarYear, this is no longer a problem.

    internal interface IDateHelper<TDate>
        where TDate : ISimpleDate
    {
        [Pure] TDate GetStartOfYear();

        [Pure] TDate GetEndOfYear();

        [Pure] TDate GetStartOfMonth();

        [Pure] TDate GetEndOfMonth();

        //
        // Factories (CalendarYear)
        //

        /// <summary>
        /// Obtains the sequence of days in the specified year.
        /// </summary>
        [Pure] static abstract IEnumerable<TDate> GetDaysInYear(CalendarYear year);

        /// <summary>
        /// Obtains the first day of the specified year.
        /// </summary>
        [Pure] static abstract TDate GetStartOfYear(CalendarYear year);

        /// <summary>
        /// Obtains the date corresponding to the specified day of the specified year.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfYear"/> is outside the range of
        /// valid values.</exception>
        [Pure] static abstract TDate GetDayOfYear(CalendarYear year, int dayOfYear);

        /// <summary>
        /// Obtains the last day of the specified year.
        /// </summary>
        [Pure] static abstract TDate GetEndOfYear(CalendarYear year);

        //
        // Factories (CalendarMonth)
        //

        /// <summary>
        /// Obtains the sequence of days in the specified month.
        /// </summary>
        [Pure] static abstract IEnumerable<TDate> GetDaysInMonth(CalendarMonth month);

        /// <summary>
        /// Obtains the first day of the specified month.
        /// </summary>
        [Pure] static abstract TDate GetStartOfMonth(CalendarMonth month);

        /// <summary>
        /// Obtains the date corresponding to the specified day of the specified month.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfMonth"/> is outside the range of
        /// valid values.</exception>
        [Pure] static abstract TDate GetDayOfMonth(CalendarMonth month, int dayOfMonth);

        /// <summary>
        /// Obtains the last day of the specified month.
        /// </summary>
        [Pure] static abstract TDate GetEndOfMonth(CalendarMonth month);

    }
}
