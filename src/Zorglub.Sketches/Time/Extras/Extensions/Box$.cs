// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extras.Extensions
{
    #region Developer Notes

    // Monad
    // -----
    // Box<T> "is" a Monad
    // - Select() the monadic "fmap"
    // - Flatten() the monadic "join".
    // Box<T> "is" a MonadOr
    // - Empty the monadic "zero".
    // - Otherwise() the monadic "plus".
    //
    // Equivalently, we could have chosen Bind() and Create() instead of Select()
    // and Flatten() as building blocks for this monad.
    //
    // Query Expression Syntax
    // -----------------------
    // In this project, we only need a select clause.
    //
    // We only implement the relevant parts
    // - Where()
    // - Select()
    // - SelectMany()
    // - Join()
    // In addition, we include two simpler forms of SelectMany():
    // - Bind()
    // - ZipWith()
    //
    // See https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/expressions#11174-the-query-expression-pattern

    #endregion

    /// <summary>
    /// Provides extension methods for <see cref="Box{T}"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static partial class BoxExtensions { }

    public partial class BoxExtensions // Querying
    {
        /// <summary>
        /// Filter this box using the specified predicate.
        /// </summary>
        /// <remarks>
        /// <example>Query expression syntax:
        /// <code><![CDATA[
        ///   from x in box
        ///   where predicate(x)
        ///   select x
        /// ]]></code>
        /// </example>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="this"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is null.</exception>
        [Pure]
        public static Box<T> Where<T>(this Box<T> @this, Func<T, bool> predicate)
            where T : class
        {
            Requires.NotNull(@this);
            Requires.NotNull(predicate);

            return !@this.IsEmpty && predicate(@this.Content) ? @this : Box<T>.Empty;
        }

        /// <summary>
        /// Unbox then apply a binder, and eventually zip the result with another box.
        /// </summary>
        /// <remarks>
        /// <example>Query expression syntax:
        /// <code><![CDATA[
        ///   from x in box
        ///   from y in binder(x)
        ///   select zipper(x, y)
        /// ]]></code>
        /// </example>
        /// <para><c>SelectMany</c> generalizes both <c>ZipWith</c> and <c>Bind</c>.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="this"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="binder"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="zipper"/> is null.</exception>
        [Pure]
        public static Box<TResult> SelectMany<TSource, TMiddle, TResult>(
            this Box<TSource> @this,
            Func<TSource, Box<TMiddle>> binder,
            Func<TSource, TMiddle, TResult?> zipper)
            where TSource : class
            where TMiddle : class
            where TResult : class
        {
            Requires.NotNull(@this);
            Requires.NotNull(binder);
            Requires.NotNull(zipper);

            if (@this.IsEmpty) { return Box<TResult>.Empty; }

            Box<TMiddle> middle = binder(@this.Content);
            if (middle.IsEmpty) { return Box<TResult>.Empty; }

            return Box.Create(zipper(@this.Content, middle.Content));
        }

        /// <remarks>
        /// <example>Query expression syntax:
        /// <code><![CDATA[
        ///   from x in outer
        ///   join y in inner
        ///   on outerSelector(x) equals innerSelector(y)
        ///   select resultSelector(x, y)
        /// ]]></code>
        /// </example>
        /// <para>The same can be achieved with <c>SelectMany</c> combined with <c>Where</c>.
        /// <code><![CDATA[
        ///   from x in outer
        ///   from y in inner
        ///   where outerSelector(x) == innerSelector(y)
        ///   select resultSelector(x, y)
        /// ]]></code>
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="outer"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="inner"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="outerSelector"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="innerSelector"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null.</exception>
        [Pure]
        public static Box<TResult> Join<TOuter, TInner, TKey, TResult>(
            this Box<TOuter> outer,
            Box<TInner> inner,
            Func<TOuter, TKey> outerSelector,
            Func<TInner, TKey> innerSelector,
            Func<TOuter, TInner, TResult?> resultSelector)
            where TOuter : class
            where TInner : class
            where TResult : class
        {
            return JoinImpl(
                outer,
                inner,
                outerSelector,
                innerSelector,
                resultSelector,
                EqualityComparer<TKey>.Default);
        }

        // No query expression syntax.
        // If "comparer" is null, the default equality comparer is used instead.
        /// <exception cref="ArgumentNullException"><paramref name="outer"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="inner"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="outerSelector"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="innerSelector"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null.</exception>
        [Pure]
        public static Box<TResult> Join<TOuter, TInner, TKey, TResult>(
            this Box<TOuter> outer,
            Box<TInner> inner,
            Func<TOuter, TKey> outerSelector,
            Func<TInner, TKey> innerSelector,
            Func<TOuter, TInner, TResult?> resultSelector,
            IEqualityComparer<TKey>? comparer)
            where TOuter : class
            where TInner : class
            where TResult : class
        {
            return JoinImpl(
                outer,
                inner,
                outerSelector,
                innerSelector,
                resultSelector,
                comparer ?? EqualityComparer<TKey>.Default);
        }

        /// <exception cref="ArgumentNullException"><paramref name="outer"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="inner"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="outerSelector"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="innerSelector"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null.</exception>
        [Pure]
        private static Box<TResult> JoinImpl<TOuter, TInner, TKey, TResult>(
            Box<TOuter> outer,
            Box<TInner> inner,
            Func<TOuter, TKey> outerSelector,
            Func<TInner, TKey> innerSelector,
            Func<TOuter, TInner, TResult?> resultSelector,
            IEqualityComparer<TKey> comparer)
            where TOuter : class
            where TInner : class
            where TResult : class
        {
            Requires.NotNull(outer);
            Requires.NotNull(inner);
            Requires.NotNull(outerSelector);
            Requires.NotNull(innerSelector);
            Requires.NotNull(resultSelector);
            Debug.Assert(comparer != null);

            if (outer.IsEmpty || inner.IsEmpty) { return Box<TResult>.Empty; }

            TKey outerKey = outerSelector(outer.Content);
            TKey innerKey = innerSelector(inner.Content!);

            return comparer.Equals(outerKey, innerKey)
                ? Box.Create(resultSelector(outer.Content, inner.Content))
                : Box<TResult>.Empty;
        }
    }

    public partial class BoxExtensions // Monad
    {
        /// <summary>
        /// Unbox then apply a binder.
        /// </summary>
        /// <remarks>
        /// <para><c>Bind</c> is <c>SelectMany</c> with a constant zipper <c>(_, y) => y</c>.</para>
        /// <para>Query expression syntax:
        /// <code><![CDATA[
        ///   from x in box
        ///   from y in binder(x)
        ///   select y
        /// ]]></code>
        /// Beware, this query does NOT actually use <c>ZipWith</c>, it's a <c>SelectMany</c> in
        /// disguise.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="this"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="binder"/> is null.</exception>
        [Pure]
        public static Box<TResult> Bind<TSource, TResult>(
            this Box<TSource> @this,
            Func<TSource, Box<TResult>> binder)
            where TSource : class
            where TResult : class
        {
            Requires.NotNull(@this);
            Requires.NotNull(binder);

            return @this.IsEmpty ? Box<TResult>.Empty : binder(@this.Content);
        }

        /// <summary>
        /// Null-coalescing operator (??).
        /// <code><![CDATA[
        ///   x.Otherwise(y) = x is empty ? y : x
        /// ]]></code>
        /// </summary>
        /// <returns>The current box if it isn't empty; otherwise the other box.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="this"/> is null.</exception>
        [Pure]
        [return: NotNullIfNotNull("other")]
        public static Box<T>? Otherwise<T>(this Box<T> @this, Box<T>? other)
            where T : class
        {
            Requires.NotNull(@this);

            return @this.IsEmpty ? other : @this;
        }

        /// <summary>
        /// Unbox this box and another, then apply a zipper, and eventually box the result.
        /// </summary>
        /// <remarks>
        /// <para><c>ZipWith</c> is <c>SelectMany</c> with a constant binder <c>_ => other</c>.
        /// </para>
        /// <para><c>ZipWith</c> generalizes <c>Select</c> to two boxes.</para>
        /// <para><c>ZipWith</c> may be thought as a cross join.</para>
        /// <para><c>ZipWith</c> promotes an ordinary function (the zipper) to a function with boxed
        /// parameters and returning a boxed result.</para>
        /// <para>Query expression syntax:
        /// <code><![CDATA[
        ///   from x in box
        ///   from y in other
        ///   select zipper(x, y)
        /// ]]></code>
        /// Beware, this query does NOT actually use <c>ZipWith</c>, it's a <c>SelectMany</c> in
        /// disguise.</para>
        /// <para>DO NOT use this method when a <c>select</c> clause combined with a <c>let</c>
        /// clause would do the job:
        /// <code><![CDATA[
        ///   // DO NOT
        ///   var other = from x in box select map(x);
        ///   var q = from x in box
        ///           from y in other
        ///           select zipper(x, y)
        ///   // DO
        ///   from x in box
        ///   let y = map(x)
        ///   select zipper(x, y)
        /// ]]></code>
        /// where <c>map</c> is just a transformation from <typeparamref name="TSource"/> to
        /// <typeparamref name="TOther"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="this"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="zipper"/> is null.</exception>
        [Pure]
        public static Box<TResult> ZipWith<TSource, TOther, TResult>(
            this Box<TSource> @this,
            Box<TOther> other,
            Func<TSource, TOther, TResult?> zipper)
            where TSource : class
            where TOther : class
            where TResult : class
        {
            Requires.NotNull(@this);
            Requires.NotNull(other);
            Requires.NotNull(zipper);

            return @this.IsEmpty || other.IsEmpty ? Box<TResult>.Empty
                : Box.Create(zipper(@this.Content, other.Content));
        }
    }
}
