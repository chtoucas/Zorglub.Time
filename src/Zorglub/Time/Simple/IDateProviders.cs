// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1000 // Do not declare static members on generic types (Design) 👈 PreviewFeatures

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core.Intervals;

    // REVIEW(api): no longer use explicit interface impl?

    // Do NOT replace CalendarYear or CalendarMonth by int's, it's a bad idea.
    // With an int, we can only produce dates in a single calendar, the
    // default one. With CalendarYear, this is no longer a problem.
    // See also ICalendar<TDate> and IDateAdjuster<TDate>.

    /// <summary>
    /// Provides methods to obtain dates in a year or a month.
    /// </summary>
    /// <typeparam name="TDate">The type of date object to return.</typeparam>
    public interface IDateProviders<TDate>
        where TDate : struct, ISimpleDate<TDate>
    {
        //
        // CalendarYear
        //

        /// <summary>
        /// Converts the specified year to a range of days.
        /// </summary>
        [Pure] static abstract Range<TDate> ConvertToRange(CalendarYear year);

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
        // CalendarMonth
        //

        /// <summary>
        /// Obtains the first day of the year to which belongs the specified month.
        /// </summary>
        [Pure] static abstract TDate GetStartOfYear(CalendarMonth month);

        /// <summary>
        /// Obtains the last day of the year to which belongs the specified month.
        /// </summary>
        [Pure] static abstract TDate GetEndOfYear(CalendarMonth month);

        /// <summary>
        /// Converts the specified month to a range of days.
        /// </summary>
        [Pure] static abstract Range<TDate> ConvertToRange(CalendarMonth month);

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
