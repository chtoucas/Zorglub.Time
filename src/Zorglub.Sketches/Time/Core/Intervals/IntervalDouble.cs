// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Intervals;

// This is a draft, most certainly wrong in many places!
// Too big to be a struct: 32 bytes.

/// <summary>
/// Represents an interval of double-precision floating-point numbers.
/// <para>The interval may be empty or reduced to a single double.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class IntervalDouble :
    IInterval<double>,
    ISetEquatable<IntervalDouble>
{
    /// <summary>
    /// Represents the left endpoint.
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly Endpoint _inf;

    /// <summary>
    /// Represents the right endpoint.
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly Endpoint _sup;

    /// <summary>
    /// Initializes a new instance of the <see cref="IntervalDouble"/> struct from the specified
    /// left and right endpoints.
    /// </summary>
    private IntervalDouble(Endpoint inf, Endpoint sup)
    {
        _inf = inf;
        _sup = sup;
    }

    /// <summary>
    /// Gets the empty set.
    /// <para>The empty set is both an intersection absorber and a hull identity.</para>
    /// </summary>
    // Single instance: Empty = ]0d, 0d[.
    // Do NOT use CreateLeniently(), it relies on Empty.
    public static IntervalDouble Empty { get; } = new(default, default);

    /// <summary>
    /// Gets the interval [<see cref="Double.MinValue"/>, <see cref="Double.MaxValue"/>].
    /// <para>This is the largest interval of double-precision floating-point numbers
    /// representable by the system.</para>
    /// </summary>
    public static IntervalDouble Maximal { get; } =
        new(Endpoint.Closed(Double.MinValue), Endpoint.Closed(Double.MaxValue));

    /// <summary>
    /// Gets the unbounded interval
    /// ]<see cref="Double.NegativeInfinity"/>, <see cref="Double.PositiveInfinity"/>[.
    /// </summary>
    public static IntervalDouble Unbounded { get; } =
        new(Endpoint.Open(Double.NegativeInfinity), Endpoint.Open(Double.PositiveInfinity));

    /// <summary>
    /// Gets the boundary of the interval, its set of endpoints.
    /// </summary>
    /// <returns>The empty set if the interval is empty or unbounded.</returns>
    public IntervalBoundary<double> Boundary =>
        IsEmpty || IsUnbounded ? IntervalBoundary<double>.Empty : new(Inf, Sup);

    /// <inheritdoc/>
    public bool IsLeftOpen => !_inf.IsClosed;

    /// <inheritdoc/>
    public bool IsRightOpen => !_sup.IsClosed;

    /// <inheritdoc />
    public bool IsLeftBounded => Double.IsFinite(_inf.Value);

    /// <inheritdoc />
    public bool IsRightBounded => Double.IsFinite(_sup.Value);

    /// <summary>
    /// Returns true if this interval is bounded; otherwise returns false.
    /// <para>An interval is said to be bounded if it is both left-bounded and right-bounded.
    /// </para>
    /// <para>The empty interval is bounded.</para>
    /// </summary>
    public bool IsBounded => IsLeftBounded && IsRightBounded;

    /// <summary>
    /// Returns true if this interval is unbounded; otherwise returns false.
    /// </summary>
    private bool IsUnbounded => SetEquals(Unbounded);

    /// <summary>
    /// Returns true if this interval is empty; otherwise returns false.
    /// </summary>
    public bool IsEmpty => SetEquals(Empty);

    /// <summary>
    /// Returns true if this interval consists of a single value; otherwise returns false.
    /// <para>A singleton interval is also said to be <i>degenerate</i>.</para>
    /// </summary>
    public bool IsSingleton => _inf.IsClosed && _sup.IsClosed && Inf == Sup;

    /// <summary>
    /// Returns true if this interval is proper; otherwise returns false.
    /// <para>An interval is said to be <i>proper</i> if it is neither empty nor degenerate.
    /// </para>
    /// </summary>
    public bool IsProper => !IsEmpty && !IsSingleton;

    /// <summary>
    /// Gets the width.
    /// <para>The width of an interval is the distance between its endpoints, that is (the
    /// absolute value of) the difference between them.</para>
    /// </summary>
    [Pure]
    public double Width =>
        IsUnbounded ? Double.PositiveInfinity
        : (Boundary.IsEmpty ? 0d : Sup - Inf);

    /// <summary>
    /// Gets the infimum.
    /// <para>Before calling this property, you MUST ensure that this interval is not empty.
    /// </para>
    /// </summary>
    private double Inf { get { Debug.Assert(!IsEmpty); return _inf.Value; } }

    /// <summary>
    /// Gets the supremum.
    /// <para>Before calling this property, you MUST ensure that this interval is not empty.
    /// </para>
    /// </summary>
    private double Sup { get { Debug.Assert(!IsEmpty); return _sup.Value; } }

    /// <summary>
    /// Returns a culture-independent string representation of this interval.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        if (IsEmpty) { return IntervalFormat.Empty; }

        var l = IsLeftOpen ? IntervalFormat.LeftOpen : IntervalFormat.LeftClosed;
        var r = IsRightOpen ? IntervalFormat.RightOpen : IntervalFormat.RightClosed;

        return FormattableString.Invariant($"{l}{_inf.Value}{IntervalFormat.Sep}{_sup.Value}{r}");
    }

    // default(Endpoint) is open which ensures that IntervalDouble.Empty is indeed empty...
    private readonly struct Endpoint : IEquatable<Endpoint>
    {
        public Endpoint(double value, bool closed)
        {
            Value = value;
            IsClosed = closed;
        }

        public Endpoint(double value, EndpointType type)
        {
            Value = value;
            IsClosed = type == EndpointType.Closed;
        }

        public double Value { get; }
        public bool IsClosed { get; }

        [Pure]
        public static Endpoint Closed(double value) => new(value, closed: true);

        [Pure]
        public static Endpoint Open(double value) => new(value, closed: false);

        [Pure]
        public bool Equals(Endpoint other) => Value == other.Value && IsClosed == other.IsClosed;

        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) => obj is Endpoint pt && Equals(pt);

        [Pure]
        public override int GetHashCode() => HashCode.Combine(Value, IsClosed);
    }
}

public partial class IntervalDouble // Factories, Set, Interval
{
    #region Factories

    /// <summary>
    /// Creates a new instance of the <see cref="IntervalDouble"/> struct representing the interval
    /// |<paramref name="inf"/>, <paramref name="sup"/>|.
    /// </summary>
    /// <exception cref="AoorException"><paramref name="sup"/> is less than
    /// <paramref name="inf"/>.</exception>
    [Pure]
    public static IntervalDouble Create(double inf, EndpointType infType, double sup, EndpointType supType)
    {
        if (sup < inf) Throw.ArgumentOutOfRange(nameof(sup));

        // Return the empty interval in the following cases:
        // - ]value, value[
        // - [value, value[
        // - ]value, value]
        if (sup == inf
            && (infType == EndpointType.Open || supType == EndpointType.Open))
        {
            return Empty;
        }

        var lower = new Endpoint(inf, infType);
        var upper = new Endpoint(sup, supType);

        return new IntervalDouble(lower, upper);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="IntervalDouble"/> struct representing the interval
    /// [<paramref name="inf"/>, <paramref name="sup"/>].
    /// </summary>
    /// <exception cref="AoorException"><paramref name="sup"/> is less than
    /// <paramref name="inf"/>.</exception>
    [Pure]
    public static IntervalDouble Closed(double inf, double sup)
    {
        if (sup < inf) Throw.ArgumentOutOfRange(nameof(sup));

        return new IntervalDouble(Endpoint.Closed(inf), Endpoint.Closed(sup));
    }

    /// <summary>
    /// Creates a new instance of the <see cref="IntervalDouble"/> struct representing the interval
    /// ]<paramref name="inf"/>, <paramref name="sup"/>[.
    /// </summary>
    /// <exception cref="AoorException"><paramref name="sup"/> is less than
    /// <paramref name="inf"/>.</exception>
    [Pure]
    public static IntervalDouble Open(double inf, double sup)
    {
        if (sup < inf) Throw.ArgumentOutOfRange(nameof(sup));

        // Return the empty interval instead of ]value, value[.
        if (sup == inf) { return Empty; }

        return new IntervalDouble(Endpoint.Open(inf), Endpoint.Open(sup));
    }

    /// <summary>
    /// Creates a new instance of the <see cref="IntervalDouble"/> struct representing the half-open
    /// (left-open and right-closed) interval ]<paramref name="inf"/>, <paramref name="sup"/>].
    /// </summary>
    /// <exception cref="AoorException"><paramref name="sup"/> is less than
    /// <paramref name="inf"/>.</exception>
    [Pure]
    public static IntervalDouble LeftOpen(double inf, double sup)
    {
        if (sup < inf) Throw.ArgumentOutOfRange(nameof(sup));

        // Return the empty interval instead of ]value, value].
        if (sup == inf) { return Empty; }

        return new IntervalDouble(Endpoint.Open(inf), Endpoint.Closed(sup));
    }

    /// <summary>
    /// Creates a new instance of the <see cref="IntervalDouble"/> struct representing the half-open
    /// (left-closed and right-open) interval [<paramref name="inf"/>, <paramref name="sup"/>[.
    /// </summary>
    /// <exception cref="AoorException"><paramref name="sup"/> is less than
    /// <paramref name="inf"/>.</exception>
    [Pure]
    public static IntervalDouble RightOpen(double inf, double sup)
    {
        if (sup < inf) Throw.ArgumentOutOfRange(nameof(sup));

        // Return the empty interval instead of [value, value[.
        if (sup == inf) { return Empty; }

        return new IntervalDouble(Endpoint.Closed(inf), Endpoint.Open(sup));
    }

    /// <summary>
    /// Creates a new instance of the <see cref="IntervalDouble"/> struct representing the interval
    /// [<paramref name="value"/>, <paramref name="value"/>].
    /// </summary>
    [Pure]
    public static IntervalDouble Singleton(double value) =>
        new(Endpoint.Closed(value), Endpoint.Closed(value));

    /// <summary>
    /// Creates a new instance of the <see cref="IntervalDouble"/> struct representing the interval
    /// (<paramref name="inf"/>, <paramref name="sup"/>).
    /// <para>This factory method does NOT validate its parameters.</para>
    /// </summary>
    [Pure]
    internal static IntervalDouble CreateLeniently(double inf, bool leftOpen, double sup, bool rightOpen)
    {
        Debug.Assert(inf <= sup);

        if (sup == inf && (leftOpen || rightOpen)) { return Empty; }

        var lower = new Endpoint(inf, leftOpen ? EndpointType.Open : EndpointType.Closed);
        var upper = new Endpoint(sup, rightOpen ? EndpointType.Open : EndpointType.Closed);

        return new IntervalDouble(lower, upper);
    }

    #endregion
    #region Interval methods

    /// <summary>
    /// Obtains the distance between the two specified intervals.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="x"/> is null.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="y"/> is null.</exception>
    [Pure]
    public static double Distance(IntervalDouble x, IntervalDouble y)
    {
        Requires.NotNull(x);
        Requires.NotNull(y);

        if (x.IsEmpty || y.IsEmpty) { return 0d; }

        // We don't write
        // > return Math.Max(0, checked(max - min));
        // because (max - min) might overflow and when max <= min we simply
        // don't have to compute the difference to obtain the result.
        var min = Math.Min(x.Sup, y.Sup);
        var max = Math.Max(x.Inf, y.Inf);

        return max <= min ? 0d : max - min;
    }

    /// <summary>
    /// Obtains the intersection of this interval with <paramref name="other"/>.
    /// </summary>
    /// <returns>The empty interval if the two intervals are disjoint.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
    [Pure]
    public IntervalDouble Intersect(IntervalDouble other)
    {
        Requires.NotNull(other);

        if (IsEmpty || other.IsEmpty) { return Empty; }

        double inf = Math.Max(Inf, other.Inf);
        double sup = Math.Min(Sup, other.Sup);

        if (inf > sup) { return Empty; }

        bool leftClosed = Contains(inf) && other.Contains(inf);
        bool rightClosed = Contains(sup) && other.Contains(sup);

        return CreateLeniently(inf, !leftClosed, sup, !rightClosed);
    }

    /// <summary>
    /// Obtains the smallest interval containing this interval and <paramref name="other"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
    [Pure]
    public IntervalDouble Span(IntervalDouble other)
    {
        Requires.NotNull(other);

        if (IsEmpty) { return other; }
        if (other.IsEmpty) { return this; }

        double inf;
        double sup;
        bool leftOpen;
        bool rightOpen;

        // inf = Math.Min(Inf, other.Inf);
        int comp1 = Inf.CompareTo(other.Inf);
        if (comp1 < 0)
        {
            inf = Inf;
            leftOpen = IsLeftOpen;
        }
        else if (comp1 == 0)
        {
            inf = Inf;
            leftOpen = IsLeftOpen && other.IsLeftOpen;
        }
        else
        {
            inf = other.Inf;
            leftOpen = other.IsLeftOpen;
        }

        // sup = Math.Max(Sup, other.Sup);
        int comp2 = Sup.CompareTo(other.Sup);
        if (comp2 > 0)
        {
            sup = Sup;
            rightOpen = IsRightOpen;
        }
        else if (comp2 == 0)
        {
            sup = Sup;
            rightOpen = IsRightOpen && other.IsRightOpen;
        }
        else
        {
            sup = other.Sup;
            rightOpen = other.IsRightOpen;
        }

        return CreateLeniently(inf, leftOpen, sup, rightOpen);
    }

    /// <summary>
    /// Determines whether this interval overlaps with <paramref name="other"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
    [Pure]
    public bool Overlaps(IntervalDouble other)
    {
        Requires.NotNull(other);

        return !IsEmpty
            && !other.IsEmpty
            && Sup >= other.Inf
            && other.Sup >= Inf;
    }

    /// <summary>
    /// Determines whether this interval and <paramref name="other"/> are connected.
    /// <para>Two intervals are said to be <i>connected</i> if their set union is an interval
    /// too.</para>
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
    [Pure]
    public bool IsConnected(IntervalDouble other) =>
        // TODO(code): does work w/ unbounded intervals, use Distance(x, y) == 0?
        Span(other).Width <= Width + other.Width;

    #endregion

#if false
    // Name: Abuts()?
    [Pure]
    public bool IsAdjacent(IntervalDouble other)
    {
        Requires.NotNull(other);

        return IsEmpty
            || other.IsEmpty
            // FIXME(code): (a, b[ is not adjacent with ]b, c).
            || (Sup == other.Inf ^ other.Sup == Inf);
    }

    /// <summary>
    /// Obtains the width of the specified interval.
    /// <para>The width of an interval is the distance between the bounds, that is (the absolute
    /// value of) the difference between its supremum and its infimum.</para>
    /// </summary>
    /// <returns>0 if the interval is empty or degenerate.</returns>
    [Pure]
    public double GetWidth()
    {
        var boundary = Boundary;

        switch (boundary.Count)
        {
            case 0: return 0d;
            case 1: return IsBounded ? 0d : Double.PositiveInfinity;
            default:
                var (inf, sup) = boundary.Endpoints.Value;
                return sup - inf;
        }
    }
#endif
}

public partial class IntervalDouble // ISet...
{
    #region Membership

    /// <inheritdoc/>
    [Pure]
    public bool Contains(double value) =>
        (IsLeftOpen ? _inf.Value < value : _inf.Value <= value)
        && (IsRightOpen ? value < _sup.Value : value <= _sup.Value);

    #endregion
    #region Equality

    /// <inheritdoc />
    [Pure]
    public bool SetEquals(IntervalDouble other)
    {
        Requires.NotNull(other);

        return _inf.Equals(other._inf) && _sup.Equals(other._sup);
    }

    #endregion
}
