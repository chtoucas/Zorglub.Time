// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1000 // Do not declare static members on generic types (Design) 👈 PreviewFeatures

namespace Zorglub.Testing.Data;

/// <summary>
/// Defines a singleton type.
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface ISingleton<TSelf> where TSelf : ISingleton<TSelf>
{
    /// <summary>
    /// Obtains a singleton instance of the <typeparamref name="TSelf"/> class.
    /// </summary>
    [Pure] static abstract TSelf Instance { get; }
}
