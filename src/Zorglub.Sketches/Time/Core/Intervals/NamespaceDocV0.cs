// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Intervals
{
    //
    // In order to avoid boxing w/ value types, we don't write
    // > GetWidth(IInterval<XXX> x)
    // but
    // > GetWidth<TInterval>(TInterval x) where TInterval : IInterval<XXX>
    // https://stackoverflow.com/questions/3032750/structs-interfaces-and-boxing?noredirect=1&lq=1
    // https://stackoverflow.com/questions/63671/is-it-safe-for-structs-to-implement-interfaces
    // https://stackoverflow.com/questions/13558579/are-there-other-ways-of-calling-an-interface-method-of-a-struct-without-boxing-e

    // Hausdorff distance overflow: max - min, Width of the gap
    //   remove from set (empty case).
    // Revisit ordering (Range32 order I <= J, (x, y) in IxJ, x <= y
    // Improve perf of GetEnumerator()
    // Interfaces: closed vs open, discrete vs "continuous" (TNumber being
    // integral or not), finite or not.
    // Discrete intervals are always in their normal form.
    // Range32Set could be based on Range32.
    // https://proofwiki.org/wiki/Definition:Interval/Ordered_Set
    // T MUST be immutable but we cannot enforce this, the best we can do is to
    // require a struct but that's not enough.
    // Do not implement IEnumerable?
    // https://stackoverflow.com/questions/3168311/why-do-bcl-collections-use-struct-enumerators-not-classes
    // https://www.red-gate.com/simple-talk/blogs/why-enumerator-structs-are-a-really-bad-idea/

    // REVIEW(api): not part of the interface because I can't figure
    // out generic param and nullable
    // https://github.com/dotnet/csharplang/discussions/3060

    // Two sorts of intervals:
    // - segments, bounded intervals
    // - rays, half-bounded intervals
    // Object equality = Set equality

    // The Empty Interval
    // ------------------
    //
    // The empty interval is special, it is
    // - both open and closed
    // - bounded, but has no infimum and supremum
    // - disjoint from any interval
    // - adjacent with any interval
    // - connected with any interval
    //
    // Connected Intervals
    // -------------------
    //
    // Two intervals are said to be connected if their union is a connected
    // set (an interval). It is equivalent to "overlap or are adjacent".
    //
    // The empty set is NOT a connected set.
    //
    // Adjacent Intervals
    // ------------------
    //
    // Adjacent: near or next to something else.
    // (Oxford dictionary)
    //
    // Two intervals are said to be adjacent if they are disjoint and there
    // is an empty gap between them (see below).
    // A more formal definition is: I and J are adjacent if I or J are empty,
    // or {I, J} is a partition of their convex hull, ie I ⋃ J = Hull(I, J).
    //
    // The adjacency relation is
    // - irreflexive, I is NOT adjacent with itself unless it is empty.
    // - symmetric, I is adjacent with J => J is adjacent with I.
    //
    // The adjacency relation is useful because it is the exact condition
    // that two disjoint intervals must meet for their union to be an
    // interval too.
    //
    // ### Remarks
    // - obviously, overlapping intervals are not adjacent.
    // - the empty interval is adjacent with any interval.
    // - (Oxford dictionary) near or next to something else ("lie near to").
    //
    // ### Gap
    // Let I and J be two intervals. The gap lying between them is
    // constructed as follows.
    // If I and J overlap, K is the empty interval.
    // If I or J is empty, K is the empty interval.
    //
    // If I and J are non-empty disjoint intervals, I = (a, b) and
    // J = (m, n), we can always suppose that b <= m. We define K to be the
    // interval (b, m) with
    // - K left-closed if I is right-open, otherwise open.
    // - K right-closed if J is left-open, otherwise open.
    //
    // Discrete interval.
    //   I and J are always in their normalized form, they are closed,
    //   therefore K = ]b, m[.
    //   K is empty iff m = successor of b (e.g. b + 1 in the "integral" case).
    // Non-discrete interval.
    //   K = (b) if b = m, otherwise K is empty (b < m).
    //
    // Contiguous Intervals
    // --------------------
    //
    // Contiguous: sharing a border.
    // Abut: be next to or touching.
    // (Oxford dictionary)
    //
    // Two intervals are said to be contiguous if they are distinct and
    // the intersection of their boundaries is a singleton.
    //
    // The contiguousness relation is
    // - irreflexive, I is NOT contiguous to itself unless it is empty.
    // - symmetric, I is contiguous to J => J is contiguous to I.
    //
    // ### Remarks
    // - the condition to be distinct ensures that [x] is not contiguous
    //   with itself.
    // - the empty interval is not contiguous to any interval.
    // - for closed intervals, contiguous implies non-disjoint.
    //
    // Examples
    // --------
    //
    // ### Discrete Case
    // [1, 3] [3, 5] are NOT adjacent, contiguous and overlapping
    // [1, 3] [3]    are NOT adjacent, contiguous and overlapping
    //
    // [1, 3] [4, 5] are adjacent, NOT contiguous and disjoint
    // [1, 3] [4]    are adjacent, NOT contiguous and disjoint
    //
    // [1, 3] [2, 5] are NOT adjacent, NOT contiguous and overlapping
    // [1, 3] [2]    are NOT adjacent, NOT contiguous and overlapping
    //
    // [1, 3] [1, 3] are NOT adjacent, NOT contiguous and equal
    //
    // [1]    [1]    are NOT adjacent, NOT contiguous and equal
    // [1]    [2]    are adjacent, NOT contiguous and disjoint
    // [1]    [3]    are NOT adjacent, NOT contiguous and disjoint
    //
    // ### Non-discrete Case
    // (1, 3] [3, 5) are NOT adjacent, contiguous and overlapping
    // (1, 3[ [3, 5) are adjacent, contiguous and disjoint
    // (1, 3] ]3, 5) are adjacent, contiguous and disjoint
    // (1, 3[ ]3, 5) are NOT adjacent, contiguous and disjoint
    // (1, 3] [3]    are NOT adjacent, contiguous and overlapping
    // (1, 3[ [3]    are adjacent, contiguous and disjoint
    //
    // (1, 3) (4, 5) are NOT adjacent, NOT contiguous and disjoint
    // (1, 3) [4]    are NOT adjacent, NOT contiguous and disjoint
    //
    // (1, 3) (2, 5) are NOT adjacent, NOT contiguous and overlapping
    // (1, 3) [2]    are NOT adjacent, NOT contiguous and overlapping
    //
    // [1]    [1]    are NOT adjacent, NOT contiguous and overlapping
    // [1]    [2]    are NOT adjacent, NOT contiguous and disjoint
    // [1]    [3]    are NOT adjacent, NOT contiguous and disjoint

    #region Developer Notes

    // An interval is bounded (finite).
    // A bounded discrete interval should always be in its normal (closed) form:
    // [min, max].
    // An interval set may be empty. It's simpler to disallow the empty interval
    // but without it set-theoretical operations seem a bit odd especially when
    // an interval is a reference type.
    //
    // T is constrained to be a value type, this is to ensure that the order in
    // which the endpoints are at the time of creation of the interval does not
    // change afterward. It also simplify the behaviour of Contains() with
    // respect to null's.
    //
    // Terminology, notations
    // ----------------------
    //
    // Let a, b in (E, <=) a totally ordered set (or a poset)
    // Intervals of endpoints a and b, a <= b (we use the french notation)
    // - [a, b] closed interval
    // - ]a, b[ open interval
    // - ]a, b] left half-open interval (left-open, right-closed)
    // - [a, b[ right half-open interval (left-closed, right-open)
    // - [a, a] degenerate interval, also noted {a}
    // - (a, b) unspecified interval
    // - empty interval
    //
    // Rays or Half-lines
    // - [a..[ closed half-line, upper closure of a
    // - ]..a] closed half-line, lower closure of a
    // - ]a..[ open half-line, strict upper closure of a
    // - ]..a[ open half-line, strict lower closure of a
    // For instance, [a, +∞[ in ℝ.
    // En français: sections commençante ou finissante.
    //
    // We also call interval an half-line.
    // A finite or bounded interval is an interval that is not a half-line.
    // Length of a finite interval (a, b) = b - a
    //
    // Discrete interval: in 1-1 correspondance with an interval of ℕ.
    //
    // We use Width instead of Length to name the "length of an interval"
    // (Length usually means a different thing in .NET).
    // Width = Max - Min except for the empty interval.
    //
    // Since Width may overflow, if possible, don't use it to implement the
    // other methods and provide a LongWidth property.
    // The same remarks apply to Count defined below.
    //
    // For abelian ops, I tend to privilege static methods.
    // In fact, we use static methods in Range32Set to simplify usage.
    // We expect an Range32 to be implicitly castable to an Range32Set so
    // that Range32Set.Intersection(x, y) is usable directly when x and y are
    // Range32 instances.
    //
    // Min/Max rather that Start/End, End being a reserved word in some languages.
    //
    // Interval vs Interval Set
    // ------------------------
    //
    // Interval:             Interval Set:
    //   Span(other)          Hull(x, y)
    //   Overlaps(other)      not Disjoint(x, y)
    //   Abuts(other)         Contiguous(x, y)
    //   IsConnected(other)   Connected(x, y)
    //   IsAdjacent(other)    Adjacent(x, y)
    //
    // Optional Features
    // -----------------
    //
    // - closed:
    //   - Min/Max and hide Inf/Sup
    // - Distance()
    // - LongDistance()
    // - enumerable:
    //   - IEnumerable<T> or ToEnumerable()
    // - countable:
    //   - Count
    //   - LongCount
    // - not countable set:
    //   - Width
    //   - LongWidth
    //   Countrary to Count, Width is always available.
    // - discrete: methods for closed intervals +
    //   - Gap()
    //   - Adjacent()
    // - factories
    //   - Singleton()
    //   - Maximal          when T : IMinMaxValue<T>
    //   - StartingAt()     when T : IMinMaxValue<T>
    //   - EndingAt()       when T : IMinMaxValue<T>
    // - adjustments
    //   - WithMin()
    //   - WithMax()
    // - implicit cast from Range32 to Range32Set
    //
    // ### Countable Interval
    //
    // We use Count not Length. In .NET they usually means more or less the
    // same thing (they can be semantically different but they return the
    // same value), but Length would be confusing as it is not the mathematical
    // length of the interval. An even better name would have been Cardinality,
    // but it's too unusual in the context of C# and its the name used by
    // ISet<T>.
    //
    // Beware, in general we do NOT have: Width = Count - 1. It is only the case
    // when T is discrete, like Int32, and the interval is not empty.
    //
    // Set-theoretical Operations
    // --------------------------
    //
    // Relations.
    // - membership relation
    //   * Contains(Element)
    // - inclusion relation + inverse
    //   * IsSubsetOf(Set)
    //   * IsSupersetOf(Set)
    // - strict inclusion relation + inverse
    //   * IsProperSubsetOf(Set)
    //   * IsProperSupersetOf(Set)
    //
    // WARNING: The result of a set operation may not be a valid interval.
    // - union
    //   * Union(Set)                   not included
    // - intersection
    //   * Intersect(Set)
    //   * Overlaps(Set)
    // - difference
    //   * Except(Set)                  not included
    // - symmetrical difference
    //   * SymmetricExcept(Set)         not included
    //
    // Other stuff.
    // - empty set
    //   * Empty
    //   * IsEmpty
    // - finite set (cardinality)
    //   * Count
    // - equality relation
    //   * IEquatable<TSelf>
    // - enumerable
    //   * IEnumerable<T>
    //     An interval is ALWAYS able to determine whether it contains an
    //     element or not, but it MAY NOT be able list them.
    //
    // Strict Partial Order
    // --------------------
    //
    // NB: the inclusion relation defines a (partial) order too.
    //
    // We define
    //   [a, b] < [m, n] iff b < m
    // It's a strict order
    //   Irreflexivity: not (I < I)
    //   Transitivity:  I < J and J < K => I < K
    // In particular, it's also asymmetric
    //   Asymmetry:     I < J => not (J < I)
    // NB: a transitive relation is asymmetric iff it is irreflexive.
    // It's a partial order; for instance, [1, 5] and [5, 10] are not
    // comparable.
    //
    // Comparison with null always return false...
    // - anything < null
    // - anything > null
    // - anything <= null
    // - anything >= null
    // where "anything" may be "null".
    //
    // MSDN (IComparable):
    // > By definition, any object compares greater than (or follows) null,
    // > and two null references compare equal to each other.
    // https://ericlippert.com/2015/08/31/nullable-comparisons-are-weird/
    //
    // References
    // ----------
    //
    // - BCL has ISet<T> and IReadOnlySet<T> but these interfaces define mutable
    //   sets. See also IImmutableSet<> & friends.
    // - Guava Range and RangeSet.
    // - Eclipse Interval & org.eclipse.collections.api.set
    // - Boost Interval
    // - https://martinfowler.com/eaaDev/Range.html

    #endregion
}
