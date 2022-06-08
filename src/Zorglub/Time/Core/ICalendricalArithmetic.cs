// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    #region Developer Notes

    // Types Implementing ICalendricalArithmetic
    // -----------------------------------------
    // **Only the interface is public.**
    //
    // ICalendricalArithmetic
    // ├─ CalendricalArithmetic         (ICalendricalSchema)
    // ├─ PlainArithmetic               (ICalendricalSchema)
    // └─ FastArithmetic [A]            (SystemSchema)
    //    ├─ GregorianArithmetic        (Gregorian-only)
    //    ├─ LunarArithmetic            (SystemSchema)
    //    ├─ LunisolarArithmetic        (SystemSchema)
    //    ├─ PlainFastArithmetic        (SystemSchema)
    //    └─ SolarArithmetic [A]        (SystemSchema)
    //       ├─ Solar12Arithmetic       (SystemSchema)
    //       └─ Solar13Arithmetic       (SystemSchema)
    //
    // Annotation: [A] = abstract
    //
    // Construction
    // ------------
    // Public:
    //   ICalendricalSchema.Arithmetic
    //   SystemSchema.TryGetCustomArithmetic()
    //
    // Internal:
    //   FastArithmetic.Create(SystemSchema)
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
        /// Adds a number of days to the specified ordinal date, yielding a new
        /// ordinal date.
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
    }
}