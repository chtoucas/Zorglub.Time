// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Intervals;

/// <summary>
/// Provides static helpers for <see cref="LowerClosure{T}"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static class LowerClosure
{
    /// <summary>
    /// Creates a new instance of the <see cref="LowerClosure{T}"/> class representing the ray
    /// ]-∞, <paramref name="value"/>|.
    /// </summary>
    [Pure]
    public static LowerClosure<T> EndingAt<T>(T value, EndpointType type)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return new(value, type);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="LowerClosure{T}"/> class representing the ray
    /// ]-∞, <paramref name="value"/>], the set of values less than or equal to
    /// <paramref name="value"/>.
    /// </summary>
    /// <returns>The weak lower closure of <paramref name="value"/>.</returns>
    [Pure]
    public static LowerClosure<T> Closed<T>(T value)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return new(value, closed: true);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="LowerClosure{T}"/> class representing the ray
    /// ]-∞, <paramref name="value"/>[, the set of values less than <paramref name="value"/>.
    /// </summary>
    /// <returns>The strict lower closure of <paramref name="value"/>.</returns>
    [Pure]
    public static LowerClosure<T> Open<T>(T value)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return new(value, closed: false);
    }
}

/// <summary>
/// Represents a right-bounded ray; it has at least one upper bound but no lower bound.
/// <para>This class cannot be inherited.</para>
/// </summary>
/// <typeparam name="T">The type of the ray's elements.</typeparam>
public sealed class LowerClosure<T> : IRay<T>
    where T : struct, IEquatable<T>, IComparable<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LowerClosure{T}"/> class representing the
    /// ray ]-∞, <paramref name="value"/>|.
    /// </summary>
    public LowerClosure(T value, EndpointType type)
    {
        UpperEnd = value;
        IsRightOpen = type == EndpointType.Open;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LowerClosure{T}"/> class representing the
    /// ray ]-∞, <paramref name="value"/>|.
    /// </summary>
    internal LowerClosure(T value, bool closed)
    {
        UpperEnd = value;
        IsRightOpen = !closed;
    }

    /// <summary>
    /// Gets the right endpoint.
    /// </summary>
    public T UpperEnd { get; }

    T IRay<T>.Endpoint => UpperEnd;

    /// <inheritdoc />
    public bool IsLeftOpen => true;

    /// <summary>
    /// Returns true if the right endpoint does not belong to this ray; otherwise returns false.
    /// </summary>
    public bool IsRightOpen { get; }

    /// <inheritdoc />
    public bool IsLeftBounded => false;

    /// <inheritdoc />
    public bool IsRightBounded => true;

    /// <summary>
    /// Returns a culture-independent string representation of this ray.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        string r = IsRightOpen ? IntervalFormat.RightOpen : IntervalFormat.RightClosed;

        return FormattableString.Invariant(
            $"{IntervalFormat.LeftUnbounded}{IntervalFormat.Sep}{UpperEnd}{r}");
    }

    /// <inheritdoc />
    [Pure]
    public bool Contains(T value)
    {
        int comp = value.CompareTo(UpperEnd);
        return IsRightOpen ? comp < 0 : comp <= 0;
    }
}
