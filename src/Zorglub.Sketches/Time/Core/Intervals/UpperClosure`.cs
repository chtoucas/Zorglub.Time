// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Core.Intervals;

/// <summary>
/// Provides static helpers for <see cref="UpperClosure{T}"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static class UpperClosure
{
    /// <summary>
    /// Creates a new instance of the <see cref="UpperClosure{T}"/> class representing the ray
    /// |<paramref name="value"/>, +∞[.
    /// </summary>
    [Pure]
    public static UpperClosure<T> StartingAt<T>(T value, EndpointType type)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return new(value, type);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="UpperClosure{T}"/> class representing the ray
    /// [<paramref name="value"/>, +∞[, the set of values greater than or equal to
    /// <paramref name="value"/>.
    /// </summary>
    /// <returns>The weak upper closure of <paramref name="value"/>.</returns>
    [Pure]
    public static UpperClosure<T> Closed<T>(T value)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return new(value, closed: true);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="UpperClosure{T}"/> class representing the ray
    /// ]<paramref name="value"/>, +∞[, the set of values greater than <paramref name="value"/>.
    /// </summary>
    /// <returns>The strict upper closure of <paramref name="value"/>.</returns>
    [Pure]
    public static UpperClosure<T> Open<T>(T value)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return new(value, closed: false);
    }
}

/// <summary>
/// Represents a left-bounded ray; it has at least one lower bound but no upper bound.
/// <para>This class cannot be inherited.</para>
/// </summary>
/// <typeparam name="T">The type of the ray's elements.</typeparam>
public sealed class UpperClosure<T> : IRay<T>
    where T : struct, IEquatable<T>, IComparable<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpperClosure{T}"/> class representing the
    /// ray |<paramref name="value"/>, +∞[.
    /// </summary>
    public UpperClosure(T value, EndpointType type)
    {
        LowerEnd = value;
        IsLeftOpen = type == EndpointType.Open;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UpperClosure{T}"/> class representing the
    /// ray |<paramref name="value"/>, +∞[.
    /// </summary>
    internal UpperClosure(T value, bool closed)
    {
        LowerEnd = value;
        IsLeftOpen = !closed;
    }

    /// <summary>
    /// Gets the left endpoint.
    /// </summary>
    public T LowerEnd { get; }

    T IRay<T>.Endpoint => LowerEnd;

    /// <summary>
    /// Returns true if the left endpoint does not belong to this ray; otherwise returns false.
    /// </summary>
    public bool IsLeftOpen { get; }

    /// <inheritdoc />
    public bool IsRightOpen => true;

    /// <inheritdoc />
    public bool IsLeftBounded => true;

    /// <inheritdoc />
    public bool IsRightBounded => false;

    /// <summary>
    /// Returns a culture-independent string representation of this ray.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        string l = IsLeftOpen ? IntervalFormat.LeftOpen : IntervalFormat.LeftClosed;

        return FormattableString.Invariant(
            $"{l}{LowerEnd}{IntervalFormat.Sep}{IntervalFormat.RightUnbounded}");
    }

    /// <inheritdoc />
    [Pure]
    public bool Contains(T value)
    {
        int comp = LowerEnd.CompareTo(value);
        return IsLeftOpen ? comp < 0 : comp <= 0;
    }
}
