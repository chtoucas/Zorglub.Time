// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

/// <summary>
/// Represents a singleton type.
/// <para>It's the duty of the implementer to ensure that the type is indeed a singleton.</para>
/// <para>While not strictly necessary, a singleton implementation should be lazy.</para>
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface ISingleton<TSelf> where TSelf : ISingleton<TSelf>
{
    /// <summary>
    /// Gets a singleton instance of the <typeparamref name="TSelf"/> class.
    /// </summary>
    [Pure] static abstract TSelf Instance { get; }
}
