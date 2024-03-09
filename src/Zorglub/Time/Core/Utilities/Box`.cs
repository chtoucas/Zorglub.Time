// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Core.Utilities;

#region Developer Notes

// This class has been added in order to keep the schemas and other types
// (e.g. arithmetic object) public while disallowing -direct- access to
// their methods. For instance, even if GregorianSchema is public, one can
// only create an instance of Box<GregorianSchema>, which effectively hides
// its methods.
//
// This class isn't related to security: it's always possible to "unbox the
// box" explicitely (via the unboxing methods provided by Unboxing in
// Core.Extensions) or by abusing Select().
//
// Considered unsafe:
// - schemas
// - arithmetic/math helpers
// - validators
// Considered safe:
// - scopes
// - calendars
//
// Iterable?
// ---------
//
// Supporting "foreach" might seem odd but maybe not so much if we pretend
// that a box is just a collection (a singleton or an empty collection).
// Furthermore, I find it useful for pattern matching and side effects
// action.
//
// public IEnumerator<T> GetEnumerator() =>
//     IsEmpty ? EmptyIterator<T>.Instance : new SingletonSet<T>.Iterator(Content);
//
// ### Pattern Matching
// Using an explicit iterator:
// > var iter = box.GetEnumerator();
// > var result = iter.MoveNext() ? caseSome(iter.Current) : caseEmpty();
//
// Of course, the same can be achieved using TryUnbox():
// > var result = box.TryUnbox(out T obj) ? caseSome(obj) : caseEmpty();
//
// ### Side effects
// Using an explicit iterator:
// > var iter = box.GetEnumerator();
// > if (iter.MoveNext()) { action(iter.Current); } else { onEmpty(); }
//
// Using an implicit iterator:
// > foreach (var x in box) { action(x); }
//
// Of course, the same can be achieved using TryUnbox():
// > if (box.TryUnbox(out T obj)) { action(obj); } else { onEmpty(); }

#endregion

/// <summary>Provides static helpers and extension methods for <see cref="Box{T}"/>.</summary>
/// <remarks>This class cannot be inherited.</remarks>
public static class Box
{
    /// <summary>Creates a new instance of the <see cref="Box{T}"/> class from the specified object.
    /// </summary>
    [Pure]
    public static Box<T> Create<T>(T? obj) where T : class
    {
        return obj is null ? Box<T>.Empty : new Box<T>(obj);
    }

    /// <summary>Obtains the unique empty instance of <see cref="Box{T}" />.</summary>
    [Pure]
    public static Box<T> Empty<T>() where T : class
    {
        return Box<T>.Empty;
    }

    /// <summary>Removes one level of boxes, projecting the content into the outer box.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="square"/> is null.</exception>
    public static Box<T> Flatten<T>(this Box<Box<T>> square) where T : class
    {
        Requires.NotNull(square);

        return square.IsEmpty ? Box<T>.Empty : square.Content;
    }
}

/// <summary>Represents a "boxed" object.</summary>
/// <remarks>This class cannot be inherited.</remarks>
/// <typeparam name="T">The type of the "boxed" object.</typeparam>
public sealed class Box<T> where T : class
{
    /// <summary>Represents the empty <see cref="Box{T}" />, it does not enclose anything.</summary>
    /// <remarks>This field is read-only.</remarks>
    internal static readonly Box<T> Empty = new();

    /// <summary>Initializes a new instance of the <see cref="Box{T}"/> class.</summary>
    private Box()
    {
        IsEmpty = true;
        Content = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Box{T}"/> class from the specified object.
    /// </summary>
    // Always use Box.Create() instead of this constructor.
    internal Box(T content)
    {
        Debug.Assert(content != null);

        IsEmpty = false;
        Content = content;
    }

    /// <summary>Determines whether this box is empty or not.</summary>
    [MemberNotNullWhen(returnValue: false, member: nameof(Content))]
    internal bool IsEmpty { get; }

    /// <summary>Gets the enclosed object.</summary>
    internal T? Content { get; }

    /// <summary>Unbox then apply a selector, and eventually box the result.</summary>
    /// <remarks>
    /// <example>Query expression syntax:
    /// <code><![CDATA[
    ///   from x in box select selector(x)
    /// ]]></code>
    /// </example>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="selector"/> is null.</exception>
    [Pure]
    public Box<TResult> Select<TResult>(Func<T, TResult?> selector) where TResult : class
    {
        Requires.NotNull(selector);

        return IsEmpty ? Box<TResult>.Empty : Box.Create(selector(Content));
    }
}
