// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    using Zorglub.Time.Horology;

    using static Zorglub.Time.Core.TemporalConstants;

    // REVIEW(code): size of Moment64. Change name (PreciseMoment, NanoMoment?).
    // In fact, Instant should be used instead of Moment64.

    /// <summary>
    /// Represents a moment with nanosecond precision.
    /// <para><see cref="Moment64"/> is an immutable struct.</para>
    /// </summary>
    public readonly partial struct Moment64 :
        // Comparison
        IComparisonOperators<Moment64, Moment64>,
        IMinMaxValue<Moment64>
    {
        private readonly DayNumber _dayNumber;
        private readonly InstantOfDay _timeOfDay;

        /// <summary>
        /// Initializes a new instance of the <see cref="Moment64"/> struct from the specified day
        /// number and time of the day.
        /// </summary>
        public Moment64(DayNumber dayNumber, InstantOfDay timeOfDay)
        {
            _dayNumber = dayNumber;
            _timeOfDay = timeOfDay;
        }

        /// <summary>
        /// Gets the origin.
        /// <para>The Monday 1st of January, 1 CE within the Gregorian calendar at 0h.</para>
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static Moment64 Zero { get; }

        public static Moment64 MinValue { get; } = new(DayNumber.MinValue, InstantOfDay.MinValue);
        public static Moment64 MaxValue { get; } = new(DayNumber.MaxValue, InstantOfDay.MaxValue);

        public DayNumber DayNumber => _dayNumber;

        public InstantOfDay TimeOfDay => _timeOfDay;

        public long SecondsSinceZero =>
            _dayNumber.DaysSinceZero * (long)SecondsPerDay
            + _timeOfDay.MillisecondOfDay / MillisecondsPerSecond;

        public long MillisecondsSinceZero =>
            _dayNumber.DaysSinceZero * (long)MillisecondsPerDay
            + _timeOfDay.MillisecondOfDay;

        // FIXME(code): may overflow => not suitable for comparison (see below).
        // Reduce the range of DayNumber's?
        public long NanosecondsSinceZero =>
            checked(
                _dayNumber.DaysSinceZero * NanosecondsPerDay
                + _timeOfDay.NanosecondOfDay);

        /// <summary>
        /// Returns a culture-independent string representation of this instance.
        /// </summary>
        public override string ToString() =>
            FormattableString.Invariant($"{DayNumber}+{TimeOfDay}ns");

        /// <summary>
        /// Deconstructs this instance into its components.
        /// </summary>
        public void Deconstruct(out DayNumber dayNumber, out InstantOfDay timeOfDay) =>
            (dayNumber, timeOfDay) = (DayNumber, TimeOfDay);
    }

    public partial struct Moment64 // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="Moment64"/> are equal.
        /// </summary>
        public static bool operator ==(Moment64 left, Moment64 right) =>
            left._dayNumber == right._dayNumber && left._timeOfDay == right._timeOfDay;

        /// <summary>
        /// Determines whether two specified instances of <see cref="Moment64"/> are not equal.
        /// </summary>
        public static bool operator !=(Moment64 left, Moment64 right) =>
            left._dayNumber != right._dayNumber || left._timeOfDay != right._timeOfDay;

        /// <inheritdoc />
        public bool Equals(Moment64 other) =>
            _dayNumber == other._dayNumber && _timeOfDay == other._timeOfDay;

        /// <inheritdoc />
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Moment64 moment && Equals(moment);

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(_dayNumber, _timeOfDay);
    }

    public partial struct Moment64 // IComparable
    {
        /// <summary>
        /// Compares the two specified moments to see if the left one is strictly earlier than the
        /// right one.
        /// </summary>
        public static bool operator <(Moment64 left, Moment64 right) => left.CompareTo(right) < 0;

        /// <summary>
        /// Compares the two specified moments to see if the left one is earlier than or equal to
        /// the right one.
        /// </summary>
        public static bool operator <=(Moment64 left, Moment64 right) => left.CompareTo(right) <= 0;

        /// <summary>
        /// Compares the two specified moments to see if the left one is strictly later than the
        /// right one.
        /// </summary>
        public static bool operator >(Moment64 left, Moment64 right) => left.CompareTo(right) > 0;

        /// <summary>
        /// Compares the two specified moments to see if the left one is later than or equal to the
        /// right one.
        /// </summary>
        public static bool operator >=(Moment64 left, Moment64 right) => left.CompareTo(right) >= 0;

        /// <summary>
        /// Indicates whether this moment instance is earlier, later or the same as the specified one.
        /// </summary>
        public int CompareTo(Moment64 other) =>
            NanosecondsSinceZero.CompareTo(other.NanosecondsSinceZero);

        /// <inheritdoc/>
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is Moment64 moment ? CompareTo(moment)
            : Throw.NonComparable(typeof(Moment64), obj);
    }
}
