// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core;

// Terminology for the set membership relation:
// - "x is a member of A"
// - "x is an element of A"
// - "x belongs to A"
// - "x is in A"
// - "A contains x" (converse relation)
// Terminology for the inclusion relation:
// - "A is a subset of B"
// - "B is a superset of A" (converse relation)
// - "B includes A" (converse relation)

/// <summary>Defines a <i>set membership relation</i> with an object of type
/// <typeparamref name="T"/>.</summary>
/// <typeparam name="T">The type of object to test for set membership.</typeparam>
public interface ISetMembership<in T>
{
    /// <summary>Returns true if this set contains the specified value; otherwise returns false.
    /// </summary>
    [Pure] bool Contains(T value);
}

/// <summary>Defines a <i>set equality relation</i> with an object of type
/// <typeparamref name="TOther"/>.</summary>
/// <typeparam name="TOther">The type of object to test for set equality.</typeparam>
public interface ISetEquatable<in TOther>
{
    /// <summary>Determines whether this set and <paramref name="other"/> contains the same elements.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
    [Pure] bool SetEquals(TOther other);
}

/// <summary>Defines a <i>set inclusion relation</i> with an object of type
/// <typeparamref name="TOther"/>.</summary>
/// <typeparam name="TOther">The type of object to test for set inclusion.</typeparam>
public interface ISetIncludible<in TOther>
{
    /// <summary>Determines whether this set is a subset of <paramref name="other"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
    [Pure] bool IsSubsetOf(TOther other);

    /// <summary>Determines whether this set is a superset of <paramref name="other"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
    [Pure] bool IsSupersetOf(TOther other);
}

/// <summary>Defines a <i>set equality relation</i> and a <i>set inclusion relation</i> (strict and
/// large) with an object of type <typeparamref name="TOther"/>.</summary>
/// <typeparam name="TOther">The type of object to compare.</typeparam>
public interface ISetComparable<in TOther> : ISetEquatable<TOther>, ISetIncludible<TOther>
{
    /// <summary>Determines whether this set is a proper (strict) subset of <paramref name="other"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
    [Pure] bool IsProperSubsetOf(TOther other);

    /// <summary>Determines whether this set is a proper (strict) superset of
    /// <paramref name="other"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
    [Pure] bool IsProperSupersetOf(TOther other);
}
