// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Core.Intervals;

/// <summary>Defines a half-bounded interval, also known as a ray or a half-line.</summary>
/// <typeparam name="T">The type of the ray's elements.</typeparam>
public interface IRay<T> : IInterval<T>
    where T : struct, IEquatable<T>, IComparable<T>
{
    /// <summary>Gets the endpoint.</summary>
    T Endpoint { get; }
}
