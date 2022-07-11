// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents a range of consecutive days, that is a finite interval of days.
    /// <para>A type implementing this interface should follow the rules of structural equality.
    /// </para>
    /// </summary>
    [Obsolete("See Zorglub.Time.Core.Intervals.")]
    public interface IDateRange<T, TDate> : IEquatable<T>, IEnumerable<TDate>
        where TDate : struct, IComparable<TDate>
    {
        /// <summary>
        /// Gets the start date.
        /// </summary>
        TDate Start { get; }

        /// <summary>
        /// Gets the end date.
        /// </summary>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "F# & VB.NET End statement.")]
        TDate End { get; }

        /// <summary>
        /// Gets the length of the current instance, the number of days in it.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Determines whether this range instance contains the specified date or not.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="date"/> does not belong to the
        /// calendar of the current instance.</exception>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", MessageId = "date", Justification = "VB.NET Date.")]
        [Pure] bool Contains(TDate date);

        /// <summary>
        /// Determines whether this range instance contains the specified range (boundaries included)
        /// or not.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="range"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="range"/> does not belong to the
        /// calendar of the current instance.</exception>
        [Pure] bool IsSupersetOf(T range);

        /// <summary>
        /// Obtains the intersection of this range instance and the other specified range.
        /// </summary>
        /// <returns><see langword="null"/> if the two ranges are disjoint.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="range"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="range"/> does not belong to the
        /// calendar of the current instance.</exception>
        [Pure] T? Intersect(T range);

        /// <summary>
        /// Obtains the union of this range instance with the other specified range.
        /// </summary>
        /// <returns><see langword="null"/> if the two ranges are disjoint.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="range"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="range"/> does not belong to the
        /// calendar of the current instance.</exception>
        [Pure] T? Union(T range);
    }
}
