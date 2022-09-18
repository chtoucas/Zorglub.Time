// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1000 // Do not declare static members on generic types (Design) 👈 PreviewFeatures

namespace Zorglub.Time.Core;

/// <summary>Defines the functions Min and Max.</summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface IMinMaxFunctions<TSelf> : IComparable<TSelf>
    where TSelf : IMinMaxFunctions<TSelf>
{
    /// <summary>Obtains the minimum of two specified values.</summary>
    [Pure] static abstract TSelf Min(TSelf x, TSelf y);

    /// <summary>Obtains the maximum of two specified values.</summary>
    [Pure] static abstract TSelf Max(TSelf x, TSelf y);
}
