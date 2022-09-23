// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Utilities;

using System.Numerics;

/// <summary>Defines a mechanism for comparing two values to determine relative order.</summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
/// <typeparam name="TOther">The type that will be compared with <typeparamref name="TSelf" />.
/// </typeparam>
public interface IComparisonOperators<TSelf, TOther> :
    IComparisonOperators<TSelf, TOther, bool>,
    IEqualityOperators<TSelf, TOther>,
    IComparable<TOther>,
    IComparable
    where TSelf : IComparisonOperators<TSelf, TOther>?
{ }
