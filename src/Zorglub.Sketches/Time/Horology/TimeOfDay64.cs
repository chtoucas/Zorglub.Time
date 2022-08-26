// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology
{
    using static Zorglub.Time.Core.TemporalArithmetic;
    using static Zorglub.Time.Core.TemporalConstants;

    // TimeOfDay et TimeOfDay64 sont interchangeables mais TimeOfDay64 offre une
    // plus grande précision.

    /// <summary>
    /// Represents a time of the day (hour:minute:second) with nanosecond precision.
    /// <para>Nanosecond precision does not necessarily mean nanosecond resolution (clock
    /// frequency).</para>
    /// <para><see cref="TimeOfDay64"/> is an immutable struct.</para>
    /// </summary>
    public readonly partial struct TimeOfDay64 :
        ITimeOfDay,
        IComparisonOperators<TimeOfDay64, TimeOfDay64>,
        IMinMaxValue<TimeOfDay64>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="TimeOfDay64"/> from the specified number of
        /// elapsed nanoseconds since midnight.
        /// <para>This constructor does NOT validate its parameter.</para>
        /// </summary>
        internal TimeOfDay64(long nanosecondOfDay)
        {
            Debug.Assert(nanosecondOfDay >= 0);

            NanosecondOfDay = nanosecondOfDay;
        }

        /// <summary>
        /// Represents the smallest possible value of a <see cref="TimeOfDay64"/>; equivalent to
        /// <see cref="Midnight"/>.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static TimeOfDay64 MinValue => Midnight;

        /// <summary>
        /// Represents the largest possible value of a <see cref="TimeOfDay"/>; one nanosecond
        /// before midnight.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static TimeOfDay64 MaxValue { get; } = new(NanosecondsPerDay - 1);

        /// <summary>
        /// Gets the value of a <see cref="TimeOfDay64"/> at 00:00.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static TimeOfDay64 Midnight { get; }

        /// <summary>
        /// Gets the value of a <see cref="TimeOfDay64"/> at 12:00.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static TimeOfDay64 Noon { get; } = new(NanosecondsPerDay / 2);

        /// <inheritdoc />
        public int Hour => DivideByNanosecondsPerHour(NanosecondOfDay);

        /// <inheritdoc />
        public int HourOfHalfDay
        {
            get
            {
                int value = Hour % 12;
                return value == 0 ? 12 : value;
            }
        }

        /// <inheritdoc />
        public bool IsAnteMeridiem => Hour < 12;

        /// <inheritdoc />
        public int Minute => DivideByNanosecondsPerMinute(NanosecondOfDay) % MinutesPerHour;

        /// <inheritdoc />
        public int Second => SecondOfDay % SecondsPerMinute;

        /// <inheritdoc />
        public int Millisecond => MillisecondOfDay % MillisecondsPerSecond;

        /// <summary>
        /// Gets the microsecond of second.
        /// <para>The result is in the range from 0 to 999_999.</para>
        /// </summary>
        public int Microsecond => (int)(MicrosecondOfDay % MicrosecondsPerSecond);

        /// <summary>
        /// Gets the tick of second.
        /// <para>The result is in the range from 0 to 9_999_999.</para>
        /// </summary>
        public int Tick => (int)(TickOfDay % TicksPerSecond);

        /// <summary>
        /// Gets the nanosecond of second.
        /// <para>The result is in the range from 0 to 999_999_999.</para>
        /// </summary>
        public int Nanosecond => (int)(NanosecondOfDay % NanosecondsPerSecond);

        /// <inheritdoc />
        public int SecondOfDay => (int)(NanosecondOfDay / NanosecondsPerSecond);

        /// <inheritdoc />
        public int MillisecondOfDay => (int)(NanosecondOfDay / NanosecondsPerMillisecond);

        /// <summary>
        /// Gets the number of elapsed microseconds since midnight.
        /// <para>The result is in the range from 0 to 86_399_999_999.</para>
        /// </summary>
        public long MicrosecondOfDay => NanosecondOfDay / NanosecondsPerMicrosecond;

        /// <summary>
        /// Gets the number of elapsed ticks since midnight.
        /// <para>The result is in the range from 0 to 863_999_999_999.</para>
        /// </summary>
        public long TickOfDay => NanosecondOfDay / NanosecondsPerTick;

        /// <summary>
        /// Gets the number of elapsed nanoseconds since midnight.
        /// <para>The result is in the range from 0 to 86_399_999_999_999.</para>
        /// </summary>
        public long NanosecondOfDay { get; }

        /// <summary>
        /// Returns a culture-independent string representation of this instance.
        /// </summary>
        public override string ToString() =>
            FormattableString.Invariant($"{Hour:D2}:{Minute:D2}:{Second:D2}.{Nanosecond:D9}");

        /// <inheritdoc />
        public void Deconstruct(out int hour, out int minute, out int second) =>
            (hour, minute, second) = (Hour, Minute, Second);

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        public void Deconstruct(out int hour, out int minute, out int second, out int nanosecond) =>
            (hour, minute, second, nanosecond) = (Hour, Minute, Second, Nanosecond);
    }

    public partial struct TimeOfDay64 // Factories, conversions...
    {
        #region Factories using (hh, mm, ss, subunit-of-second)

        /// <summary>
        /// Creates a new instance of <see cref="TimeOfDay64"/> from the specified hour-of-day and
        /// minute-of-hour.
        /// </summary>
        /// <exception cref="AoorException">One of the parameters is out of range.</exception>
        [Pure]
        public static TimeOfDay64 FromHourMinute(int hour, int minute)
        {
            if (hour < 0 || hour >= HoursPerDay) Throw.ArgumentOutOfRange(nameof(hour));
            if (minute < 0 || minute >= MinutesPerHour) Throw.ArgumentOutOfRange(nameof(minute));

            long nanosecondOfDay = MultiplyByNanosecondsPerHour(hour) + NanosecondsPerMinute * minute;

            return new TimeOfDay64(nanosecondOfDay);
        }

        /// <summary>
        /// Creates a new instance of <see cref="TimeOfDay64"/> from the specified hour-of-day,
        /// minute-of-hour and second-of-minute.
        /// </summary>
        /// <exception cref="AoorException">One of the parameters is out of range.</exception>
        [Pure]
        public static TimeOfDay64 FromHourMinuteSecond(int hour, int minute, int second)
        {
            if (hour < 0 || hour >= HoursPerDay) Throw.ArgumentOutOfRange(nameof(hour));
            if (minute < 0 || minute >= MinutesPerHour) Throw.ArgumentOutOfRange(nameof(minute));
            if (second < 0 || second >= SecondsPerMinute) Throw.ArgumentOutOfRange(nameof(second));

            long nanosecondOfDay = MultiplyByNanosecondsPerHour(hour)
                + NanosecondsPerMinute * minute
                + (long)NanosecondsPerSecond * second;

            return new TimeOfDay64(nanosecondOfDay);
        }

        /// <summary>
        /// Creates a new instance of <see cref="TimeOfDay64"/> from the specified hour-of-day,
        /// minute-of-hour, second-of-minute and millisecond-of-second.
        /// </summary>
        /// <exception cref="AoorException">One of the parameters is out of range.</exception>
        [Pure]
        public static TimeOfDay64 FromHourMinuteSecondMillisecond(
            int hour, int minute, int second, int millisecond)
        {
            if (hour < 0 || hour >= HoursPerDay) Throw.ArgumentOutOfRange(nameof(hour));
            if (minute < 0 || minute >= MinutesPerHour) Throw.ArgumentOutOfRange(nameof(minute));
            if (second < 0 || second >= SecondsPerMinute) Throw.ArgumentOutOfRange(nameof(second));
            if (millisecond < 0 || millisecond >= MillisecondsPerSecond)
                Throw.ArgumentOutOfRange(nameof(second));

            long nanosecondOfDay = MultiplyByNanosecondsPerHour(hour)
                + NanosecondsPerMinute * minute
                + (long)NanosecondsPerSecond * second
                + (long)NanosecondsPerMillisecond * millisecond;

            return new TimeOfDay64(nanosecondOfDay);
        }
        /// <summary>
        /// Creates a new instance of <see cref="TimeOfDay64"/> from the specified hour-of-day,
        /// minute-of-hour, second-of-minute and microsecond-of-second.
        /// </summary>
        /// <exception cref="AoorException">One of the parameters is out of range.</exception>
        [Pure]
        public static TimeOfDay64 FromHourMinuteSecondMicrosecond(
            int hour, int minute, int second, int microsecond)
        {
            if (hour < 0 || hour >= HoursPerDay) Throw.ArgumentOutOfRange(nameof(hour));
            if (minute < 0 || minute >= MinutesPerHour) Throw.ArgumentOutOfRange(nameof(minute));
            if (second < 0 || second >= SecondsPerMinute) Throw.ArgumentOutOfRange(nameof(second));
            if (microsecond < 0 || microsecond >= MicrosecondsPerSecond)
                Throw.ArgumentOutOfRange(nameof(second));

            long nanosecondOfDay = MultiplyByNanosecondsPerHour(hour)
                + NanosecondsPerMinute * minute
                + (long)NanosecondsPerSecond * second
                + (long)NanosecondsPerMicrosecond * microsecond;

            return new TimeOfDay64(nanosecondOfDay);
        }

        /// <summary>
        /// Creates a new instance of <see cref="TimeOfDay64"/> from the specified hour-of-day,
        /// minute-of-hour, second-of-minute and tick-of-second.
        /// </summary>
        /// <exception cref="AoorException">One of the parameters is out of range.</exception>
        [Pure]
        public static TimeOfDay64 FromHourMinuteSecondTick(
            int hour, int minute, int second, int tick)
        {
            if (hour < 0 || hour >= HoursPerDay) Throw.ArgumentOutOfRange(nameof(hour));
            if (minute < 0 || minute >= MinutesPerHour) Throw.ArgumentOutOfRange(nameof(minute));
            if (second < 0 || second >= SecondsPerMinute) Throw.ArgumentOutOfRange(nameof(second));
            if (tick < 0 || tick >= TicksPerSecond) Throw.ArgumentOutOfRange(nameof(second));

            long nanosecondOfDay = MultiplyByNanosecondsPerHour(hour)
                + NanosecondsPerMinute * minute
                + (long)NanosecondsPerSecond * second
                + (long)NanosecondsPerTick * tick;

            return new TimeOfDay64(nanosecondOfDay);
        }

        /// <summary>
        /// Creates a new instance of <see cref="TimeOfDay64"/> from the specified hour-of-day,
        /// minute-of-hour, second-of-minute and nanosecond-of-second.
        /// </summary>
        /// <exception cref="AoorException">One of the parameters is out of range.</exception>
        [Pure]
        public static TimeOfDay64 FromHourMinuteSecondNanosecond(
            int hour, int minute, int second, int nanosecond)
        {
            if (hour < 0 || hour >= HoursPerDay) Throw.ArgumentOutOfRange(nameof(hour));
            if (minute < 0 || minute >= MinutesPerHour) Throw.ArgumentOutOfRange(nameof(minute));
            if (second < 0 || second >= SecondsPerMinute) Throw.ArgumentOutOfRange(nameof(second));
            if (nanosecond < 0 || nanosecond >= NanosecondsPerSecond)
                Throw.ArgumentOutOfRange(nameof(second));

            long nanosecondOfDay = MultiplyByNanosecondsPerHour(hour)
                + NanosecondsPerMinute * minute
                + (long)NanosecondsPerSecond * second
                + nanosecond;

            return new TimeOfDay64(nanosecondOfDay);
        }

        #endregion
        #region Factories using a subunit-of-day

        /// <summary>
        /// Creates a new instance of <see cref="TimeOfDay64"/> from the specified elapsed hours
        /// since midnight.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="hourOfDay"/> is out of range.</exception>
        [Pure]
        public static TimeOfDay64 FromHoursSinceMidnight(int hourOfDay)
        {
            if (hourOfDay < 0 || hourOfDay >= HoursPerDay)
                Throw.ArgumentOutOfRange(nameof(hourOfDay));

            return new TimeOfDay64(MultiplyByNanosecondsPerHour(hourOfDay));
        }

        /// <summary>
        /// Creates a new instance of <see cref="TimeOfDay64"/> from the specified elapsed minutes
        /// since midnight.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="minuteOfDay"/> is out of range.
        /// </exception>
        [Pure]
        public static TimeOfDay64 FromMinutesSinceMidnight(int minuteOfDay)
        {
            if (minuteOfDay < 0 || minuteOfDay >= MinutesPerDay)
                Throw.ArgumentOutOfRange(nameof(minuteOfDay));

            return new TimeOfDay64(NanosecondsPerMinute * minuteOfDay);
        }

        /// <summary>
        /// Creates a new instance of <see cref="TimeOfDay64"/> from the specified elapsed seconds
        /// since midnight.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="secondOfDay"/> is out of range.
        /// </exception>
        [Pure]
        public static TimeOfDay64 FromSecondsSinceMidnight(int secondOfDay)
        {
            if (secondOfDay < 0 || secondOfDay >= SecondsPerDay)
                Throw.ArgumentOutOfRange(nameof(secondOfDay));

            return new TimeOfDay64((long)NanosecondsPerSecond * secondOfDay);
        }

        /// <summary>
        /// Creates a new instance of <see cref="TimeOfDay64"/> from the specified elapsed
        /// milliseconds since midnight.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="millisecondOfDay"/> is out of range.
        /// </exception>
        [Pure]
        public static TimeOfDay64 FromMillisecondsSinceMidnight(int millisecondOfDay)
        {
            if (millisecondOfDay < 0 || millisecondOfDay >= MillisecondsPerDay)
                Throw.ArgumentOutOfRange(nameof(millisecondOfDay));

            return new TimeOfDay64((long)NanosecondsPerMillisecond * millisecondOfDay);
        }

        /// <summary>
        /// Creates a new instance of <see cref="TimeOfDay64"/> from the specified elapsed
        /// milliseconds since midnight.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="microsecondOfDay"/> is out of range.
        /// </exception>
        [Pure]
        public static TimeOfDay64 FromMicrosecondsSinceMidnight(long microsecondOfDay)
        {
            if (microsecondOfDay < 0 || microsecondOfDay >= MicrosecondsPerDay)
                Throw.ArgumentOutOfRange(nameof(microsecondOfDay));

            return new TimeOfDay64(NanosecondsPerMicrosecond * microsecondOfDay);
        }

        /// <summary>
        /// Creates a new instance of <see cref="TimeOfDay64"/> from the specified elapsed ticks
        /// since midnight.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="tickOfDay"/> is out of range.</exception>
        [Pure]
        public static TimeOfDay64 FromTicksSinceMidnight(long tickOfDay)
        {
            if (tickOfDay < 0 || tickOfDay >= TicksPerDay)
                Throw.ArgumentOutOfRange(nameof(tickOfDay));

            return new TimeOfDay64(NanosecondsPerTick * tickOfDay);
        }

        /// <summary>
        /// Creates a new instance of <see cref="TimeOfDay64"/> from the specified elapsed ticks
        /// since midnight.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="nanosecondOfDay"/> is out of range.
        /// </exception>
        [Pure]
        public static TimeOfDay64 FromNanosecondsSinceMidnight(long nanosecondOfDay)
        {
            if (nanosecondOfDay < 0 || nanosecondOfDay >= NanosecondsPerDay)
                Throw.ArgumentOutOfRange(nameof(nanosecondOfDay));

            return new TimeOfDay64(nanosecondOfDay);
        }

        #endregion
        #region Other factories

        /// <summary>
        /// Creates a new instance of <see cref="TimeOfDay64"/> from the specified fraction of the
        /// day.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="fractionOfDay"/> is out of range.
        /// </exception>
        [Pure]
        public static TimeOfDay64 FromFractionOfDay(double fractionOfDay)
        {
            if (fractionOfDay < 0d || fractionOfDay >= 1d)
                Throw.ArgumentOutOfRange(nameof(fractionOfDay));

            long nanosecondOfDay = (long)(fractionOfDay * NanosecondsPerDay);

            return new TimeOfDay64(nanosecondOfDay);
        }

        /// <summary>
        /// Creates a new instance of <see cref="TimeOfDay64"/> from the specified fraction of the
        /// day.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="fractionOfDay"/> is out of range.
        /// </exception>
        [Pure]
        internal static TimeOfDay64 FromFractionOfDay(decimal fractionOfDay)
        {
            if (fractionOfDay < 0m || fractionOfDay >= 1m)
                Throw.ArgumentOutOfRange(nameof(fractionOfDay));

            long nanosecondOfDay = (long)(fractionOfDay * NanosecondsPerDay);

            return new TimeOfDay64(nanosecondOfDay);
        }

        #endregion
        #region Conversions

        /// <summary>
        /// Converts the current instance to a <see cref="TimeOfDay"/>.
        /// </summary>
        [Pure]
        public TimeOfDay ToTimeOfDay() =>
            TimeOfDay.FromMillisecondsSinceMidnightCore(MillisecondOfDay);

        /// <summary>
        /// Converts this instance to a fraction of the day.
        /// </summary>
        [Pure]
        public double ToFractionOfDay() => (double)NanosecondOfDay / NanosecondsPerDay;

        /// <summary>
        /// Converts this instance to a fraction of the day.
        /// </summary>
        [Pure]
        internal decimal ToDecimal() => (decimal)NanosecondOfDay / NanosecondsPerDay;

        #endregion
    }

    public partial struct TimeOfDay64 // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="TimeOfDay64"/> are equal.
        /// </summary>
        public static bool operator ==(TimeOfDay64 left, TimeOfDay64 right) =>
            left.NanosecondOfDay == right.NanosecondOfDay;

        /// <summary>
        /// Determines whether two specified instances of <see cref="TimeOfDay64"/> are not equal.
        /// </summary>
        public static bool operator !=(TimeOfDay64 left, TimeOfDay64 right) =>
            left.NanosecondOfDay != right.NanosecondOfDay;

        /// <inheritdoc />
        [Pure]
        public bool Equals(TimeOfDay64 other) => NanosecondOfDay == other.NanosecondOfDay;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is TimeOfDay64 timeOfDay && Equals(timeOfDay);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => NanosecondOfDay.GetHashCode();
    }

    public partial struct TimeOfDay64 // IComparable
    {
        /// <summary>
        /// Compares the two specified fractions of the day to see if the left one is strictly
        /// earlier than the right one.
        /// </summary>
        public static bool operator <(TimeOfDay64 left, TimeOfDay64 right) =>
            left.NanosecondOfDay < right.NanosecondOfDay;

        /// <summary>
        /// Compares the two specified fractions of the day to see if the left one is earlier than
        /// or equal to the right one.
        /// </summary>
        public static bool operator <=(TimeOfDay64 left, TimeOfDay64 right) =>
            left.NanosecondOfDay <= right.NanosecondOfDay;

        /// <summary>
        /// Compares the two specified fractions of the day to see if the left one is strictly later
        /// than the right one.
        /// </summary>
        public static bool operator >(TimeOfDay64 left, TimeOfDay64 right) =>
            left.NanosecondOfDay > right.NanosecondOfDay;

        /// <summary>
        /// Compares the two specified fractions of the day to see if the left one is later than or
        /// equal to the right one.
        /// </summary>
        public static bool operator >=(TimeOfDay64 left, TimeOfDay64 right) =>
            left.NanosecondOfDay >= right.NanosecondOfDay;

        /// <summary>
        /// Obtains the earlier time of two specified times.
        /// </summary>
        [Pure]
        public static TimeOfDay64 Min(TimeOfDay64 left, TimeOfDay64 right) =>
            left < right ? left : right;

        /// <summary>
        /// Obtains the later time of two specified times.
        /// </summary>
        [Pure]
        public static TimeOfDay64 Max(TimeOfDay64 left, TimeOfDay64 right) =>
            left > right ? left : right;

        /// <summary>
        /// Indicates whether this instance is earlier, later or the same as the specified one.
        /// </summary>
        [Pure]
        public int CompareTo(TimeOfDay64 other) => NanosecondOfDay.CompareTo(other.NanosecondOfDay);

        [Pure]
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is TimeOfDay64 fractionOfDay ? CompareTo(fractionOfDay)
            : Throw.NonComparable(typeof(TimeOfDay64), obj);
    }
}
