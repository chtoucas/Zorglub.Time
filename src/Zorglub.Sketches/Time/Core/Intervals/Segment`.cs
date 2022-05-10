// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Intervals
{
    /// <summary>
    /// Provides static helpers for <see cref="Segment{T}"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class Segment
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Segment{T}"/> class representing the segment
        /// |<paramref name="start"/>, <paramref name="end"/>|.
        /// </summary>
        [Pure]
        public static Segment<T> Create<T>(T start, EndpointType startType, T end, EndpointType endType)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            if (end.CompareTo(start) < 0) Throw.ArgumentOutOfRange(nameof(end));

            if (end.Equals(start) && (startType == EndpointType.Open || endType == EndpointType.Open))
            {
                Throw.ArgumentOutOfRange(nameof(end));
            }

            var upperClosure = new UpperClosure<T>(start, startType);
            var lowerClosure = new LowerClosure<T>(end, endType);

            return new Segment<T>(upperClosure, lowerClosure);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Segment{T}"/> class representing the segment
        /// [<paramref name="start"/>, <paramref name="end"/>].
        /// </summary>
        [Pure]
        public static Segment<T> Closed<T>(T start, T end)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            if (end.CompareTo(start) < 0) Throw.ArgumentOutOfRange(nameof(end));

            return new Segment<T>(UpperClosure.Closed(start), LowerClosure.Closed(end));
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Segment{T}"/> class representing the segment
        /// ]<paramref name="start"/>, <paramref name="end"/>[.
        /// </summary>
        [Pure]
        public static Segment<T> Open<T>(T start, T end)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            if (end.CompareTo(start) <= 0) Throw.ArgumentOutOfRange(nameof(end));

            return new Segment<T>(UpperClosure.Open(start), LowerClosure.Open(end));
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Segment{T}"/> class representing the segment
        /// ]<paramref name="start"/>, <paramref name="end"/>].
        /// </summary>
        [Pure]
        public static Segment<T> LeftOpen<T>(T start, T end)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            if (end.CompareTo(start) <= 0) Throw.ArgumentOutOfRange(nameof(end));

            return new Segment<T>(UpperClosure.Open(start), LowerClosure.Closed(end));
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Segment{T}"/> class representing the segment
        /// [<paramref name="start"/>, <paramref name="end"/>[.
        /// </summary>
        [Pure]
        public static Segment<T> RightOpen<T>(T start, T end)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            if (end.CompareTo(start) <= 0) Throw.ArgumentOutOfRange(nameof(end));

            return new Segment<T>(UpperClosure.Closed(start), LowerClosure.Open(end));
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Segment{T}"/> class representing the segment
        /// ]<paramref name="value"/>, <paramref name="value"/>[.
        /// </summary>
        [Pure]
        public static Segment<T> Singleton<T>(T value)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            return new(UpperClosure.Closed(value), LowerClosure.Closed(value));
        }
    }

    /// <summary>
    /// Represents a segment.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    /// <typeparam name="T">The type of the segment's elements.</typeparam>
    public sealed class Segment<T> : ISegment<T>
        where T : struct, IEquatable<T>, IComparable<T>
    {
        /// <summary>
        /// Represents the upper closure.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly UpperClosure<T> _upperClosure;

        /// <summary>
        /// Represents the lower closure.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly LowerClosure<T> _lowerClosure;

        /// <summary>
        /// Initializes a new instance of the <see cref="Segment{T}"/> class representing the
        /// intersection of <paramref name="lowerClosure"/> = |LowerEnd..[ and
        /// <paramref name="upperClosure"/> = ]...UpperEnd|.
        /// </summary>
        internal Segment(UpperClosure<T> upperClosure, LowerClosure<T> lowerClosure)
        {
            Debug.Assert(upperClosure != null);
            Debug.Assert(lowerClosure != null);

            _upperClosure = upperClosure;
            _lowerClosure = lowerClosure;
        }

        /// <inheritdoc/>
        public OrderedPair<T> Endpoints => new(_upperClosure.LowerEnd, _lowerClosure.UpperEnd);

        /// <inheritdoc/>
        public T LowerEnd => _upperClosure.LowerEnd;

        /// <inheritdoc/>
        public T UpperEnd => _lowerClosure.UpperEnd;

        /// <summary>
        /// Returns true if the left endpoint does not belong to this segment; otherwise returns
        /// false.
        /// </summary>
        public bool IsLeftOpen => _upperClosure.IsLeftOpen;

        /// <summary>
        /// Returns true if the right endpoint does not belong to this segment; otherwise returns
        /// false.
        /// </summary>
        public bool IsRightOpen => _lowerClosure.IsRightOpen;

        /// <inheritdoc/>
        public bool IsLeftBounded => true;

        /// <inheritdoc/>
        public bool IsRightBounded => true;

        // No method IsSingleton. Verifying End.Equals(Start) is not enough,
        // indeed there are singleton sets for which this equality is not
        // satisfied, e.g. the interval ]2, 4[ of integers.

        /// <summary>
        /// Returns a culture-independent string representation of this segment.
        /// </summary>
        [Pure]
        public override string ToString()
        {
            var l = IsLeftOpen ? IntervalFormat.LeftOpen : IntervalFormat.LeftClosed;
            var r = IsRightOpen ? IntervalFormat.RightOpen : IntervalFormat.RightClosed;
            return FormattableString.Invariant($"{l}{LowerEnd}{IntervalFormat.Sep}{UpperEnd}{r}");
        }

        /// <inheritdoc />
        [Pure]
        public bool Contains(T value) => _upperClosure.Contains(value) && _lowerClosure.Contains(value);
    }
}
