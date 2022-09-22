// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Utilities;

using System.Collections;

internal static class SingletonSet
{
    public static SingletonSet<T> Create<T>([DisallowNull] T value) => new(value);
}

/// <summary>
/// Represents a single value iterator, a read-only singleton set.
/// <para>This iterator is resettable.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
/// <remarks>
/// We could use:
/// <code>
///   return Enumerable.Repeat(element, 1);
/// </code>
/// but then many LINQ operators are optimized for lists, and
/// Enumerable.Repeat() does not seem to produce one.
/// </remarks>
/// <typeparam name="T">The type of the set's elements.</typeparam>
internal sealed class SingletonSet<T> : IList<T>, IList, IReadOnlyList<T>
{
    [NotNull] private readonly T _element;

    /// <summary>
    /// Initializes a new instance of the <see cref="SingletonSet{T}"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="element"/> is null.</exception>
    public SingletonSet([DisallowNull] T element)
    {
        _element = element ?? throw new ArgumentNullException(nameof(element));
    }

    #region IList<T>

    /// <inheritdoc />
    public T this[int index]
    {
        get => index == 0 ? _element : Throw.ArgumentOutOfRange<T>(nameof(index));
        set => Throw.ReadOnlyCollection();
    }

    /// <inheritdoc />
    [Pure]
    public int IndexOf(T item) => Contains(item) ? 0 : -1;

    /// <inheritdoc />
    public void Insert(int index, T item) => Throw.ReadOnlyCollection();

    /// <inheritdoc />
    public void RemoveAt(int index) => Throw.ReadOnlyCollection();

    #endregion
    #region ICollection<T>

    /// <inheritdoc />
    public int Count => 1;

    /// <inheritdoc />
    public bool IsReadOnly => true;

    /// <inheritdoc />
    public void Add(T item) => Throw.ReadOnlyCollection();

    /// <inheritdoc />
    public void Clear() => Throw.ReadOnlyCollection();

    /// <inheritdoc />
    [Pure]
    public bool Contains(T item) => EqualityComparer<T>.Default.Equals(item, _element);

    /// <inheritdoc />
    public void CopyTo(T[] array, int arrayIndex)
    {
        Requires.NotNull(array);

        array[arrayIndex] = _element;
    }

    /// <inheritdoc />
    public bool Remove(T item) => Throw.ReadOnlyCollection<bool>();

    #endregion
    #region IEnumerable<T>

    /// <inheritdoc />
    [Pure] public IEnumerator<T> GetEnumerator() => new Iterator(_element);

    /// <inheritdoc />
    [Pure] IEnumerator IEnumerable.GetEnumerator() => new Iterator(_element);

    #endregion
    #region IList

    /// <inheritdoc />
    public bool IsFixedSize => true;

    /// <inheritdoc />
    public bool IsSynchronized => false;

    /// <inheritdoc />
    public object SyncRoot => this;

    /// <inheritdoc />
    object? IList.this[int index]
    {
        get => index == 0 ? _element : Throw.ArgumentOutOfRange<object>(nameof(index));
        set => Throw.ReadOnlyCollection();
    }

    /// <inheritdoc />
    public int Add(object? value) => Throw.ReadOnlyCollection<int>();

    /// <inheritdoc />
    public bool Contains(object? value) => _element.Equals(value);

    /// <inheritdoc />
    public int IndexOf(object? value) => Contains(value) ? 0 : -1;

    /// <inheritdoc />
    public void Insert(int index, object? value) => Throw.ReadOnlyCollection();

    /// <inheritdoc />
    public void Remove(object? value) => Throw.ReadOnlyCollection();

    /// <inheritdoc />
    public void CopyTo(Array array, int index)
    {
        Requires.NotNull(array);

        array.SetValue(_element, index);
    }

    #endregion

    public sealed class Iterator : IEnumerator<T>
    {
        [NotNull] private readonly T _element;
        private bool _done;

        public Iterator([DisallowNull] T element)
        {
            _element = element;
        }

        // Common behaviour:
        // - before any call to MoveNext(), returns default(T)
        // - when done iterating, returns the last value
        // Here, we always return _element.
        [Pure] public T Current => _element;
        [Pure] object IEnumerator.Current => _element;

        [Pure]
        public bool MoveNext()
        {
            if (_done) { return false; }
            return _done = true;
        }

        // It seems that it is now a requirement to throw an exception
        // (e.g. not supported), anyway it doesn't really matter.
        public void Reset() => _done = false;

        public void Dispose() { }
    }
}
