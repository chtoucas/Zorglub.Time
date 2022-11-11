// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Utilities;

/// <summary>Defines a boxable type.</summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface IBoxable<TSelf>
    where TSelf : class, IBoxable<TSelf>
{
    /// <summary>Creates a new (boxed) instance of the <typeparamref name="TSelf"/> class.</summary>
    [Pure]
    static abstract Box<TSelf> GetInstance();
}
