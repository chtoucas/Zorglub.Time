// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using Zorglub.Time.Core.Intervals;

    #region Developer Notes

    // Types Implementing ICalendricalArithmetic
    // -----------------------------------------
    // **Only ICalendricalArithmetic and CalendricalArithmetic are public.**
    //
    // ICalendricalArithmetic
    // ├─ CalendricalArithmetic
    // └─ SystemArithmetic [A]
    //    ├─ GregorianArithmetic     (Gregorian-only)
    //    ├─ LunarArithmetic         (CalendricalSchema)
    //    ├─ LunisolarArithmetic     (CalendricalSchema)
    //    ├─ PlainArithmetic         (ICalendricalSchema)
    //    ├─ RegularArithmetic       (ICalendricalSchema)
    //    └─ SolarArithmetic [A]     (CalendricalSchema)
    //       ├─ Solar12Arithmetic    (CalendricalSchema)
    //       └─ Solar13Arithmetic    (CalendricalSchema)
    //
    // Annotation: [A] = abstract
    //
    // Construction
    // ------------
    // Public:
    //   ICalendricalSchema.Arithmetic
    //   CalendricalSchema.TryGetCustomArithmetic()
    //
    // Internal:
    //   SystemArithmetic.Create(CalendricalSchema)
    //
    // Comments
    // --------
    // ICalendricalArithmetic is more naturally part of ICalendricalSchema but
    // the code being the same for very different types of schemas, adding the
    // members of this interface to ICalendricalSchema would lead to a lot of
    // duplications. Therefore this is just an implementation detail and one
    // should really use the public property ICalendricalSchema.Arithmetic.
    //
    // An implementation of ICalendricalArithmetic should follow the rules of
    // ICalendricalSchema: no overflow, lenient methods, same range of years,
    // etc.
    //
    // All methods assume that a Yemoda (or a Yedoy) input forms a valid date
    // for the underlying schema.

    #endregion

    /// <summary>
    /// Defines the arithmetical operations on dates related to the day unit.
    /// </summary>
    public interface ICalendricalArithmetic
    {
        /// <summary>
        /// Gets the calendrical segment.
        /// </summary>
        CalendricalSegment Segment { get; }

        //
        // Operations on Yemoda
        //

        /// <summary>
        /// Adds a number of days to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] Yemoda AddDays(Yemoda ymd, int days);

        /// <summary>
        /// Obtains the day after the specified date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] Yemoda NextDay(Yemoda ymd);

        /// <summary>
        /// Obtains the day before the specified date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] Yemoda PreviousDay(Yemoda ymd);

        /// <summary>
        /// Counts the number of days between the two specified dates.
        /// </summary>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", MessageId = "end", Justification = "F# & VB.NET End statement.")]
        [Pure] int CountDaysBetween(Yemoda start, Yemoda end);

        //
        // Operations on Yedoy
        //

        /// <summary>
        /// Adds a number of days to the specified ordinal date, yielding a new ordinal date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] Yedoy AddDays(Yedoy ydoy, int days);

        /// <summary>
        /// Obtains the day after the specified ordinal date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] Yedoy NextDay(Yedoy ydoy);

        /// <summary>
        /// Obtains the day before the specified ordinal date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] Yedoy PreviousDay(Yedoy ydoy);

        /// <summary>
        /// Counts the number of days between the two specified ordinal dates.
        /// </summary>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", MessageId = "end", Justification = "F# & VB.NET End statement.")]
        [Pure] int CountDaysBetween(Yedoy start, Yedoy end);

        //
        // Operations on Yemo
        //

        /// <summary>
        /// Adds a number of months to the specified month, yielding a new month.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] Yemo AddMonths(Yemo ym, int months);

        /// <summary>
        /// Obtains the month after the specified month.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] Yemo NextMonth(Yemo ym);

        /// <summary>
        /// Obtains the month before the specified month.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] Yemo PreviousMonth(Yemo ym);

        /// <summary>
        /// Counts the number of months between the two specified months.
        /// </summary>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", MessageId = "end", Justification = "F# & VB.NET End statement.")]
        [Pure] int CountMonthsBetween(Yemo start, Yemo end);
    }
}