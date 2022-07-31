// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using System;

    /// <summary>
    /// Defines the common adjusters for <typeparamref name="TDate"/>.
    /// </summary>
    /// <typeparam name="TDate">The type of date object.</typeparam>
    public interface IDateAdjusters<TDate>
    {
        /// <summary>
        /// Obtains the first day of the month to which belongs the specified day.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="date"/> is null.</exception>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", MessageId = "end", Justification = "F# & VB.NET End statement.")]
        [Pure] TDate GetStartOfMonth(TDate date);

        /// <summary>
        /// Obtains the last day of the month to which belongs the specified day.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="date"/> is null.</exception>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", MessageId = "end", Justification = "F# & VB.NET End statement.")]
        [Pure] TDate GetEndOfMonth(TDate date);

        /// <summary>
        /// Obtains the first day of the year to which belongs the specified day.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="date"/> is null.</exception>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", MessageId = "end", Justification = "F# & VB.NET End statement.")]
        [Pure] TDate GetStartOfYear(TDate date);

        /// <summary>
        /// Obtains the last day of the year to which belongs the specified day.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="date"/> is null.</exception>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", MessageId = "end", Justification = "F# & VB.NET End statement.")]
        [Pure] TDate GetEndOfYear(TDate date);
    }
}
