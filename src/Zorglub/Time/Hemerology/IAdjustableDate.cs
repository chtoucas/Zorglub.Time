// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    // Namespace Simple. We have three date types: ordinal, normal, day count.
    // Only CalendarDate implements IAdjustableDate. The two others have their
    // own adjustment methods. For instance, OrdinalDate has WithYear() and
    // WithDayOfYear(), and to change the month, first we convert the ordinal
    // date to a CalendarDate, then call WithMonth() on the result.
    //
    // For a date type available in a single form, we should implement this
    // interface plus the other adjuster(s) if there is any.

    /// <summary>
    /// Defines an adjustable date type.
    /// </summary>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    public interface IAdjustableDate<TSelf> : IDateable
        where TSelf : IAdjustableDate<TSelf>
    {
        /// <summary>
        /// Adjusts the date fields to the specified values, yielding a new date.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="adjuster"/> is null.</exception>
        /// <exception cref="AoorException">The resulting date would be invalid.</exception>
        [Pure] TSelf Adjust(Func<DateParts, DateParts> adjuster);

        /// <summary>
        /// Adjusts the year field to the specified value, yielding a new date.
        /// </summary>
        /// <exception cref="AoorException">The resulting date would be invalid.</exception>
        [Pure] TSelf WithYear(int newYear);

        /// <summary>
        /// Adjusts the month field to the specified value, yielding a new date.
        /// </summary>
        /// <exception cref="AoorException">The resulting date would be invalid.</exception>
        [Pure] TSelf WithMonth(int newMonth);

        /// <summary>
        /// Adjusts the day of the month field to the specified value, yielding a new date.
        /// </summary>
        /// <exception cref="AoorException">The resulting date would be invalid.</exception>
        [Pure] TSelf WithDay(int newDay);
    }
}
