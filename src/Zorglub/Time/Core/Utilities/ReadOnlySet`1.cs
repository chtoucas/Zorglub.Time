// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Utilities;

using System.Collections;

/// <summary>Represents a read-only set of values.</summary>
/// <remarks>This class cannot be inherited.</remarks>
/// <typeparam name="T">The type of the set's elements.</typeparam>
[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(ReadOnlySet<>.DebugView))]
public sealed partial class ReadOnlySet<T> : IReadOnlyCollection<T>, IReadOnlySet<T>
{
    private readonly HashSet<T> _set;

    /// <summary>Initializes a new instance of the <see cref="ReadOnlySet{T}"/> class.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="set"/> is null.</exception>
    public ReadOnlySet(HashSet<T> set)
    {
        _set = set ?? throw new ArgumentNullException(nameof(set));
    }

    /// <summary>Initializes a new instance of the <see cref="ReadOnlySet{T}"/> class.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is null.</exception>
    public ReadOnlySet(IEnumerable<T> collection)
    {
        // NB: we do not offer the ability to choose a comparer.
        // Looking at HashSet's ctor, it knows better.
        _set = new HashSet<T>(collection);
    }

    /// <summary>Represents a debugger type proxy for <see cref="ReadOnlySet{T}"/>.</summary>
    [ExcludeFromCodeCoverage]
    private sealed class DebugView
    {
        private readonly ReadOnlySet<T> _obj;

        public DebugView(ReadOnlySet<T> obj)
        {
            _obj = obj ?? throw new ArgumentNullException(nameof(obj));
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                var items = new T[_obj.Count];
                _obj._set.CopyTo(items, 0);
                return items;
            }
        }
    }
}

public partial class ReadOnlySet<T> // IReadOnlyCollection
{
    /// <inheritdoc />
    public int Count => _set.Count;

    /// <inheritdoc />
    [Pure] public IEnumerator<T> GetEnumerator() => _set.GetEnumerator();

    /// <inheritdoc />
    [Pure] IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_set).GetEnumerator();
}

public partial class ReadOnlySet<T> // IReadOnlySet
{
    /// <inheritdoc />
    [Pure]
    public bool Contains(T item) => _set.Contains(item);

    /// <inheritdoc />
    [Pure]
    public bool IsProperSubsetOf(IEnumerable<T> other) => _set.IsProperSubsetOf(other);

    /// <inheritdoc />
    [Pure]
    public bool IsProperSupersetOf(IEnumerable<T> other) => _set.IsProperSupersetOf(other);

    /// <inheritdoc />
    [Pure]
    public bool IsSubsetOf(IEnumerable<T> other) => _set.IsSubsetOf(other);

    /// <inheritdoc />
    [Pure]
    public bool IsSupersetOf(IEnumerable<T> other) => _set.IsSupersetOf(other);

    /// <inheritdoc />
    [Pure]
    public bool Overlaps(IEnumerable<T> other) => _set.Overlaps(other);

    /// <inheritdoc />
    [Pure]
    public bool SetEquals(IEnumerable<T> other) => _set.SetEquals(other);
}
