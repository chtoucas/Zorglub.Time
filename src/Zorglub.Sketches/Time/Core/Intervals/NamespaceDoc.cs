// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Core.Intervals;

#region Developer Notes

// There are exactly 10 types of intervals.
// The unbounded ones.
//  [x, +∞[
//  ]x, +∞[
//  ]-∞, x]
//  ]-∞, x[
//  ]-∞, +∞[, the whole interval
// The bounded ones.
//  [x, y]
//  [x, y[
//  ]x, y]
//  ]x, y[
//  {}, the empty interval
//
// Terminology:
// - Ray or half-line = an unbounded interval.
// - Segment = a bounded interval.
// - Range = a closed bounded interval.
//   Range usually means a discrete interval but here we use it also for
//   continuous intervals, even if in practice this is not the case: this is
//   the reason why we have both Range<T> and Segment<T>.
//
// NB: We only consider intervals in a totally ordered set.
//
// Value types:
//  Range<T>         [x, y]
//  RangeSet<T>      [x, y], {}
//  LowerRay<T>      ]-∞, x]
//  UpperRay<T>      [x, +∞[
//
// Reference types found in Zorglub.Sketches:
//  Segment<T>       |x, y|
//  LowerClosure<T>  ]-∞, x|
//  UpperClosure<T>  |x, +∞[
//  Unbounded<T>     ]-∞, +∞[
//  IntervalDouble   All interval types?
//
// Ordered Set
// -----------
//
// The infimum is also called the greatest lower bound.
// If the interval is left-closed, it's an element of the interval also
// called the least element, otherwise the infimum is not an element of the
// set.
//
// The supremum is also called the least upper bound.
// If the interval is right-closed, it's an element of the interval also
// called the greatest element, otherwise the supremum is not an element of
// the set.
//
// Core Properties
// ---------------
//
// An interval is said to be left-open if it has no minimum.
// A discrete interval is left-open iff it's not left-bounded.
// The empty interval is left-open.
//
// An interval is said to be right-open if it has no maximum.
// A discrete interval is right-open iff it's not right-bounded.
// The empty interval is right-open.
//
// The empty range is left-bounded and right-bounded (every value is a lower
// bound and an upper bound) while it has no infimum and supremum.
//
// Topological Properties
// ----------------------
//
// ### Discrete Interval
// An interval is said to be discrete if all of its members are isolated.
// Discrete intervals are always represented using their (closed) normal
// form.
//
// ### Open and Closed Intervals
// Beware, open and closed are NOT mutually exclusive, e.g. the empty set is
// both open and closed (clopen set).
// It is also possible for a set to be neither open nor closed, e.g. the
// half-open intervals.
// Other examples:
// - ranges: a closed bounded interval is closed and non-open.
// - rays: a closed half-bounded interval is closed and non-open.
//
// Rays.
//          open closed
// [x, +∞[  N    Y
// ]x, +∞[  Y    N
// ]-∞, x]  N    Y
// ]-∞, x[  Y    N
// ]-∞, +∞[ Y    Y
//
// Segments.
//          open closed
// [x, y]   N    Y
// [x, y[   N    N
// ]x, y]   N    N
// ]x, y[   Y    N
// empty    Y    Y
//
// open = left-open and right-open
// closed = empty or (not lef-open and not right-open);
//
// Computer Representations
// ------------------------
//
// ### Generic Interval
//
// ### Class vs Struct
// Closed interval: struct
// Open or half-open interval: class
//
// ### How to model a bounded interval?
// 1st option:
// (inf...[ ⋂ ]...sup), the intersection of an upper ray and a lower ray.
// 2nd option:
// (inf, sup), a pair of an infimum and a supremum.
//
// ### Equality
// If a type implements ISetEquatable and IEquatable, both equalities should
// be strictly equivalent.
// For closed intervals, should be unambiguous.
// For open or half-open intervals, it really depends on the type of the
// interval's elements.
//
// Integral interval.
// An instance is always given in its normal (closed) form.
// There is a normal (closed) form: e.g. ]1, 3] == [2, 3].
//
// Interval of doubles.
//
// Non-uniqueness of the representation of the ray => should be a class.

#endregion
