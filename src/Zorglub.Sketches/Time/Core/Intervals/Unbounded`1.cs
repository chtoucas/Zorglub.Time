// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Core.Intervals;

/// <summary>
/// Represents an unbounded interval.
/// <para>This class cannot be inherited.</para>
/// </summary>
/// <typeparam name="T">The type of the interval's elements.</typeparam>
public sealed class Unbounded<T> : IInterval<T>
    where T : struct, IEquatable<T>, IComparable<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Unbounded{T}"/> class.
    /// </summary>
    private Unbounded() { }

    /// <summary>
    /// Gets a singleton instance of the <see cref="Unbounded{T}"/> class.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    [SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "<Pending>")]
    public static Unbounded<T> Instance { get; } = new();

    /// <inheritdoc />
    public bool IsLeftOpen => true;

    /// <inheritdoc />
    public bool IsRightOpen => true;

    /// <inheritdoc />
    public bool IsLeftBounded => false;

    /// <inheritdoc />
    public bool IsRightBounded => false;

    /// <summary>
    /// Returns a culture-independent string representation of this interval.
    /// </summary>
    [Pure]
    public override string ToString() => IntervalFormat.Unbounded;

    #region Interval methods
    // Perf: we do NOT implement ISetComparable<IInterval<T>> to avoid
    // boxing when IInterval<T> is a struct type.

    /// <inheritdoc />
    [Pure]
    public bool Contains(T value) => true;

    /// <summary>
    /// Determines whether this set is a subset of <paramref name="other"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
    /// <typeparam name="TInterval">The type of object to test for set inclusion.</typeparam>
    [Pure]
    public bool IsSubsetOf<TInterval>(TInterval other)
        where TInterval : IInterval<T>
    {
        return SetEquals(other);
    }

#if false

    /// <summary>
    /// Determines whether this set is a proper (strict) subset of <paramref name="other"/>.
    /// </summary>
    /// <typeparam name="TInterval">The type of object to test for set inclusion.</typeparam>
    [Pure]
    public bool IsProperSubsetOf<TInterval>(TInterval other)
        where TInterval : IInterval<T>
    {
        return false;
    }

    /// <summary>
    /// Determines whether this set is a superset of <paramref name="other"/>.
    /// </summary>
    /// <typeparam name="TInterval">The type of object to test for set inclusion.</typeparam>
    [Pure]
    public bool IsSupersetOf<TInterval>(TInterval other)
        where TInterval : IInterval<T>
    {
        return true;
    }

#endif

    /// <summary>
    /// Determines whether this set is a proper (strict) superset of <paramref name="other"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
    /// <typeparam name="TInterval">The type of object to test for set inclusion.</typeparam>
    [Pure]
    public bool IsProperSupersetOf<TInterval>(TInterval other)
        where TInterval : IInterval<T>
    {
        return SetEquals(other) == false;
    }

    /// <summary>
    /// Determines whether this set and <paramref name="other"/> contains the same elements.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
    /// <typeparam name="TInterval">The type of object to test for set equality.</typeparam>
    public bool SetEquals<TInterval>(TInterval other)
        where TInterval : IInterval<T>
    {
        Requires.NotNull(other);

        if (ReferenceEquals(this, other)) { return true; }
        return !other.IsLeftBounded && !other.IsRightBounded;
    }

    #endregion
}
