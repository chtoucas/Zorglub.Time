// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    /// <summary>
    /// Defines the standard calendrical arithmetic.
    /// </summary>
    public interface ICalendricalArithmetic
    {
        /// <summary>
        /// Gets the segment of supported days.
        /// </summary>
        CalendricalSegment Segment { get; }

        //
        // Operations on DateParts
        //

        /// <summary>
        /// Adds a number of days to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] DateParts AddDays(DateParts parts, int days);

        /// <summary>
        /// Obtains the day after the specified date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] DateParts NextDay(DateParts parts);

        /// <summary>
        /// Obtains the day before the specified date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] DateParts PreviousDay(DateParts parts);

        /// <summary>
        /// Counts the number of days between the two specified dates.
        /// </summary>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", MessageId = "end", Justification = "F# & VB.NET End statement.")]
        [Pure] int CountDaysBetween(DateParts start, DateParts end);

        //
        // Operations on OrdinalParts
        //

        /// <summary>
        /// Adds a number of days to the specified ordinal date, yielding a new ordinal date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] OrdinalParts AddDays(OrdinalParts parts, int days);

        /// <summary>
        /// Obtains the day after the specified ordinal date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] OrdinalParts NextDay(OrdinalParts parts);

        /// <summary>
        /// Obtains the day before the specified ordinal date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] OrdinalParts PreviousDay(OrdinalParts parts);

        /// <summary>
        /// Counts the number of days between the two specified ordinal dates.
        /// </summary>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", MessageId = "end", Justification = "F# & VB.NET End statement.")]
        [Pure] int CountDaysBetween(OrdinalParts start, OrdinalParts end);

        //
        // Operations on MonthParts
        //

        /// <summary>
        /// Adds a number of months to the specified month, yielding a new month.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] MonthParts AddMonths(MonthParts parts, int months);

        /// <summary>
        /// Obtains the month after the specified month.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] MonthParts NextMonth(MonthParts parts);

        /// <summary>
        /// Obtains the month before the specified month.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] MonthParts PreviousMonth(MonthParts parts);

        /// <summary>
        /// Counts the number of months between the two specified months.
        /// </summary>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", MessageId = "end", Justification = "F# & VB.NET End statement.")]
        [Pure] int CountMonthsBetween(MonthParts start, MonthParts end);
    }
}