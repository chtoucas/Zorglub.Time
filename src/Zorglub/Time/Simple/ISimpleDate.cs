// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Hemerology;

    #region Developer Notes

    // API differences between the three date types:
    // - Constructor/deconstructor.
    // - ToString().
    // - Adjusters for core parts.
    // - Some specific math ops.
    // - CalendarDay.DayNumber instead of CalendarDay.ToDayNumber().

    #endregion

    /// <summary>
    /// Defines a "simple" date.
    /// </summary>
    public interface ISimpleDate :
        IDate,
        ISerializable<int>
    {
        /// <summary>
        /// Gets the calendar to which belongs the current instance.
        /// </summary>
        /// <remarks>
        /// <para>Performance tip: cache this property locally if used repeatedly within a code
        /// block.</para>
        /// </remarks>
        SimpleCalendar Calendar { get; }

        /// <summary>
        /// Gets the calendar year to which belongs the current instance.
        /// </summary>
        CalendarYear CalendarYear { get; }

        /// <summary>
        /// Gets the calendar month to which belongs the current instance.
        /// </summary>
        CalendarMonth CalendarMonth { get; }

        //
        // Conversions
        //
        // To hide a conversion method when it is meaningless use an explicit
        // interface implementation. No need to add static factories. For
        // instance, CalendarDay.FromCalendarDate(CalendarDate) is just:
        // > CalendarDate date = ...
        // > CalendarDay day = date.ToCalendarDay();

        /// <summary>
        /// Converts the current instance to a calendar day.
        /// </summary>
        [Pure] CalendarDay ToCalendarDay();

        /// <summary>
        /// Converts the current instance to a calendar date.
        /// </summary>
        [Pure] CalendarDate ToCalendarDate();

        /// <summary>
        /// Converts the current instance to an ordinal date.
        /// </summary>
        [Pure] OrdinalDate ToOrdinalDate();
    }

    /// <summary>
    /// Defines a "simple" date type.
    /// </summary>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    public interface ISimpleDate<TSelf> :
        ISimpleDate,
        IDate<TSelf>,
        ISerializable<TSelf, int>
        where TSelf : ISimpleDate<TSelf>
    { }
}
