// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    /// <summary>
    /// Defines helpers to check for overflows of a range.
    /// </summary>
    /// <typeparam name="T">The type of value to check for overflows.</typeparam>
    internal interface IOverflowChecker<T>
        where T : struct, IEquatable<T>, IComparable<T>
    {
        /// <summary>
        /// Checks whether the specified value is outside the range of supported values or not.
        /// </summary>
        /// <exception cref="OverflowException">The value is outside the range of supported values.
        /// </exception>
        void Check(T value);

        /// <summary>
        /// Checks whether the specified value is greater than the upper bound of the range of
        /// supported values or not.
        /// </summary>
        /// <exception cref="OverflowException">The value is greater than the upper bound of the
        /// range of supported values.</exception>
        void CheckUpperBound(T value);

        /// <summary>
        /// Checks whether the specified value is less than the lower bound of the range of
        /// supported values or not.
        /// </summary>
        /// <exception cref="OverflowException">The value is less than the lower bound of the range
        /// of supported values.</exception>
        void CheckLowerBound(T value);
    }
}
