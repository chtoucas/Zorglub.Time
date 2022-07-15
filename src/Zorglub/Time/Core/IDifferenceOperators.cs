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

    public interface IDifferenceOperators<TSelf, TResult>
        where TSelf : IDifferenceOperators<TSelf, TResult>
    {
        public static abstract TResult operator -(TSelf left, TSelf right);
    }
}
