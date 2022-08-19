// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) 👈 PreviewFeatures

namespace Zorglub.Time.Core
{
    // Arithmetic operators
    // 1) x - n = y and x - y = n
    // 2) x + n = y and x - y = -n
    // Error CS0695: we cannot have both
    //   ISubtractionOperators<TSelf, TSelf, TResult>
    //   ISubtractionOperators<TSelf, TResult, TSelf>
    //
    // A "numeric" type should implement
    // - IAdditionOperators<TSelf, TOther, TSelf>
    // - ISubtractionOperators<TSelf, TOther, TSelf>
    // - IDifferenceOperators<TSelf, TResult>
    // where TOther = TResult.
    //
    // IDifferenceOperators<TSelf, TResult> is just
    // ISubtractionOperators<TSelf, TSelf, TResult>.

    /// <summary>
    /// Defines a mechanism for computing the difference of two values of the same type.
    /// </summary>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    /// <typeparam name="TResult">The type that contains the difference of two values.</typeparam>
    public interface IDifferenceOperators<TSelf, out TResult>
        where TSelf : IDifferenceOperators<TSelf, TResult>
    {
        /// <summary>
        /// Subtracts two values to compute their difference.
        /// </summary>
        public static abstract TResult operator -(TSelf left, TSelf right);
    }
}
