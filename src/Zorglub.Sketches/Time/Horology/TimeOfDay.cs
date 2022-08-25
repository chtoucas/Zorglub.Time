// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology
{
    using static Zorglub.Time.Core.TemporalConstants;

    //  millisecondOfMinute (0-59_999) 16 bits
    // ou
    //  second              (0-59)      6 bits
    //  millisecondOfSecond (0-999)    10 bits
    // 27 bits used, 5 remaining.
    //
    // - Now()
    // - adjustments, other ctors
    // - math ops, checked or not?
    // - leap seconds
    //   https://github.com/golang/go/issues/12914
    //
    // https://en.wikipedia.org/wiki/12-hour_clock
    // https://en.wikipedia.org/wiki/24-hour_clock
    //
    // See https://docs.oracle.com/javase/8/docs/api/java/time/LocalTime.html.
    // https://www.gnu.org/software/pspp/manual/html_node/Time-and-Date-Formats.html
    // https://docs.oracle.com/javase/tutorial/i18n/format/simpleDateFormat.html
    // https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings#how-standard-format-strings-work

    /// <summary>
    /// Represents a time of the day (hour:minute:second) with millisecond precision.
    /// <para><see cref="TimeOfDay"/> is an immutable struct.</para>
    /// </summary>
    public readonly partial struct TimeOfDay :
        ITimeOfDay,
        IComparisonOperators<TimeOfDay, TimeOfDay>,
        IMinMaxValue<TimeOfDay>
    {
        #region Bit settings

        /// <summary>
        /// <see cref="Hour"/> is a 5-bit unsigned integer.
        /// <para>This field is a constant equal to 5.</para>
        /// </summary>
        private const int HourBits = 5;

        /// <summary>
        /// <see cref="Minute"/> is a 6-bit unsigned integer.
        /// <para>This field is a constant equal to 6.</para>
        /// </summary>
        private const int MinuteBits = 6;

        /// <summary>
        /// <see cref="Second"/> is a 6-bit unsigned integer.
        /// <para>This field is a constant equal to 6.</para>
        /// </summary>
        private const int SecondBits = 6;

        /// <summary>
        /// <see cref="Millisecond"/> is a 6-bit unsigned integer.
        /// <para>This field is a constant equal to 10.</para>
        /// </summary>
        private const int MillisecondBits = 10;

        /// <summary>This field is a constant equal to 10.</summary>
        private const int SecondShift = MillisecondBits;

        /// <summary>This field is a constant equal to 16.</summary>
        private const int MinuteShift = SecondShift + SecondBits;

        /// <summary>This field is a constant equal to 22.</summary>
        private const int HourShift = MinuteShift + MinuteBits;

        /// <summary>This field is a constant equal to 27.</summary>
        internal const int HighestBit = HourShift + HourBits;

        private const int MillisecondMask = (1 << MillisecondBits) - 1;
        private const int SecondMask = (1 << SecondBits) - 1;
        private const int MinuteMask = (1 << MinuteBits) - 1;

        #endregion

        /// <summary>
        /// Represents the binary data stored in this instance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The data is organised as follows:
        /// <code><![CDATA[
        ///   Hour         0000 0bbb bb
        ///   Minute                   bb bbbb
        ///   Second                           bbbb bb
        ///   Millisecond                             bb bbbb bbbb
        /// ]]></code>
        /// </para>
        /// </remarks>
        private readonly int _bin;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeOfDay"/> struct directly from the
        /// specified binary data.
        /// <para>This constructor does NOT validate its parameter.</para>
        /// </summary>
        private TimeOfDay(int bin)
        {
            DebugCheckBinaryData(bin);

            _bin = bin;
        }

        /// <summary>
        /// Represents the smallest possible value of a <see cref="TimeOfDay"/>; equivalent to
        /// <see cref="Midnight"/>.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static TimeOfDay MinValue { get; }

        /// <summary>
        /// Represents the largest possible value of a <see cref="TimeOfDay"/>; one millisecond
        /// before midnight.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static TimeOfDay MaxValue { get; } = FromHourMinuteSecondMillisecond(23, 59, 59, 999);

        /// <summary>
        /// Gets the value of a <see cref="TimeOfDay"/> at 00:00.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static TimeOfDay Midnight => MinValue;

        /// <summary>
        /// Gets the value of a <see cref="TimeOfDay"/> at 12:00.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static TimeOfDay Noon { get; } = FromHourMinute(12, 0);

        /// <inheritdoc />
        public int Hour => unchecked(_bin >> HourShift);

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
        public int Minute => unchecked((_bin >> MinuteShift) & MinuteMask);

        /// <inheritdoc />
        public int Second => unchecked((_bin >> SecondShift) & SecondMask);

        /// <inheritdoc />
        public int Millisecond => unchecked(_bin & MillisecondMask);

        /// <inheritdoc />
        public int SecondOfDay
        {
            get
            {
                Unpack(out int h, out int m, out int s);
                return SecondsPerHour * h
                    + SecondsPerMinute * m
                    + s;
            }
        }

        /// <inheritdoc />
        public int MillisecondOfDay
        {
            get
            {
                Unpack(out int h, out int m, out int s);
                return MillisecondsPerHour * h
                    + MillisecondsPerMinute * m
                    + MillisecondsPerSecond * s
                    + Millisecond;
            }
        }

        /// <summary>
        /// Returns a culture-independent string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            Unpack(out int h, out int m, out int s);
            return FormattableString.Invariant($"{h:D2}:{m:D2}:{s:D2}.{Millisecond:D3}");
        }

        /// <inheritdoc />
        public void Deconstruct(out int hour, out int minute, out int second) =>
            Unpack(out hour, out minute, out second);

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        public void Deconstruct(out int hour, out int minute, out int second, out int millisecond)
        {
            Unpack(out hour, out minute, out second);
            millisecond = Millisecond;
        }
    }

    public partial struct TimeOfDay // Binary data helpers
    {
        /// <summary>
        /// Deserializes a 32-bit binary value and recreates an original serialized
        /// <see cref="TimeOfDay"/> object.
        /// </summary>
        /// <exception cref="ArgumentException">The specified binary data is not well-formed.
        /// </exception>
        [Pure]
        public static TimeOfDay FromBinary(int data)
        {
            ValidateBinaryData(data);
            return new TimeOfDay(data);
        }

        /// <summary>
        /// Serializes the current <see cref="TimeOfDay"/> object to a 32-bit binary value that
        /// subsequently can be used to recreate the <see cref="TimeOfDay"/> object.
        /// </summary>
        [Pure]
        public int ToBinary() => _bin;

        /// <summary>
        /// Packs the specified time parts into a single 32-bit word.
        /// </summary>
        [Pure]
        // CIL code size = 17 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Pack(int h, int m, int s, int ms)
        {
            unchecked
            {
                return (h << HourShift) | (m << MinuteShift) | (s << SecondShift) | ms;
            }
        }

        /// <summary>
        /// Packs the specified time parts into a single 32-bit word.
        /// </summary>
        [Pure]
        // CIL code size = 15 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Pack(int h, int m, int s)
        {
            unchecked
            {
                return (h << HourShift) | (m << MinuteShift) | (s << SecondShift);
            }
        }

        /// <summary>
        /// Unpacks the binary data.
        /// </summary>
        // CIL code size = 32 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Unpack(out int h, out int m, out int s)
        {
            // Perf: local copy of the field _bin.
            int bin = _bin;

            unchecked
            {
                h = bin >> HourShift;
                m = (bin >> MinuteShift) & MinuteMask;
                s = (bin >> SecondShift) & SecondMask;
            }
        }

        /// <summary>
        /// Validates the specified binary data.
        /// </summary>
        /// <exception cref="ArgumentException">The specified binary data is not well-formed.</exception>
        private static void ValidateBinaryData(int data)
        {
            // The 5 high bits are always equal to zero.
            if (data >> HighestBit != 0) Throw.BadBinaryInput();

            int h = data >> HourShift;
            if (h < 0 || h >= HoursPerDay) Throw.BadBinaryInput();

            int m = (data >> MinuteShift) & MinuteMask;
            if (m < 0 || m >= MinutesPerHour) Throw.BadBinaryInput();

            int s = (data >> SecondShift) & SecondMask;
            if (s < 0 || s >= SecondsPerMinute) Throw.BadBinaryInput();

            int ms = data & MillisecondMask;
            if (ms < 0 || ms >= MillisecondsPerSecond) Throw.BadBinaryInput();
        }

        [Conditional("DEBUG")]
        [ExcludeFromCodeCoverage]
        private static void DebugCheckBinaryData(int bin) => ValidateBinaryData(bin);
    }

    public partial struct TimeOfDay // Factories, conversions...
    {
        #region Factories using (hh, mm, ss, subunit-of-second)

        /// <summary>
        /// Creates a new instance of <see cref="TimeOfDay"/> from the specified hour-of-day and
        /// minute-of-hour.
        /// </summary>
        /// <exception cref="AoorException">One of the parameters is out of range.</exception>
        [Pure]
        public static TimeOfDay FromHourMinute(int hour, int minute)
        {
            if (hour < 0 || hour >= HoursPerDay) Throw.ArgumentOutOfRange(nameof(hour));
            if (minute < 0 || minute >= MinutesPerHour) Throw.ArgumentOutOfRange(nameof(minute));

            int bin = Pack(hour, minute, 0);
            return new TimeOfDay(bin);
        }

        /// <summary>
        /// Creates a new instance of <see cref="TimeOfDay"/> from the specified hour-of-day,
        /// minute-of-hour and second-of-minute.
        /// </summary>
        /// <exception cref="AoorException">One of the parameters is out of range.</exception>
        [Pure]
        public static TimeOfDay FromHourMinuteSecond(int hour, int minute, int second)
        {
            if (hour < 0 || hour >= HoursPerDay) Throw.ArgumentOutOfRange(nameof(hour));
            if (minute < 0 || minute >= MinutesPerHour) Throw.ArgumentOutOfRange(nameof(minute));
            if (second < 0 || second >= SecondsPerMinute) Throw.ArgumentOutOfRange(nameof(second));

            int bin = Pack(hour, minute, second);
            return new TimeOfDay(bin);
        }

        /// <summary>
        /// Creates a new instance of <see cref="TimeOfDay"/> from the specified hour-of-day,
        /// minute-of-hour, second-of-minute and millisecond-of-second.
        /// </summary>
        /// <exception cref="AoorException">One of the parameters is out of range.</exception>
        [Pure]
        public static TimeOfDay FromHourMinuteSecondMillisecond(
            int hour, int minute, int second, int millisecond)
        {
            if (hour < 0 || hour >= HoursPerDay) Throw.ArgumentOutOfRange(nameof(hour));
            if (minute < 0 || minute >= MinutesPerHour) Throw.ArgumentOutOfRange(nameof(minute));
            if (second < 0 || second >= SecondsPerMinute) Throw.ArgumentOutOfRange(nameof(second));
            if (millisecond < 0 || millisecond >= MillisecondsPerSecond)
                Throw.ArgumentOutOfRange(nameof(second));

            int bin = Pack(hour, minute, second, millisecond);
            return new TimeOfDay(bin);
        }

        #endregion
        #region Factories using a subunit-of-day

        /// <summary>
        /// Creates a new instance of <see cref="TimeOfDay64"/> from the specified elapsed hours
        /// since midnight.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="hourOfDay"/> is out of range.</exception>
        [Pure]
        public static TimeOfDay FromHoursSinceMidnight(int hourOfDay)
        {
            if (hourOfDay < 0 || hourOfDay >= HoursPerDay)
                Throw.ArgumentOutOfRange(nameof(hourOfDay));

            return new TimeOfDay(Pack(hourOfDay, 0, 0));
        }

        /// <summary>
        /// Creates a new instance of <see cref="TimeOfDay64"/> from the specified elapsed minutes
        /// since midnight.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="minuteOfDay"/> is out of range.
        /// </exception>
        [Pure]
        public static TimeOfDay FromMinutesSinceMidnight(int minuteOfDay)
        {
            if (minuteOfDay < 0 || minuteOfDay >= MinutesPerDay)
                Throw.ArgumentOutOfRange(nameof(minuteOfDay));

            int h = minuteOfDay / MinutesPerHour;
            int m = minuteOfDay % MinutesPerHour;

            return new TimeOfDay(Pack(h, m, 0));
        }

        /// <summary>
        /// Creates a new instance of <see cref="TimeOfDay"/> from the specified elapsed seconds
        /// since midnight.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="secondOfDay"/> is out of range.
        /// </exception>
        [Pure]
        public static TimeOfDay FromSecondsSinceMidnight(int secondOfDay)
        {
            if (secondOfDay < 0 || secondOfDay >= SecondsPerDay)
                Throw.ArgumentOutOfRange(nameof(secondOfDay));

            int h = secondOfDay / SecondsPerHour;
            int m = secondOfDay / SecondsPerMinute % MinutesPerHour;
            int s = secondOfDay % SecondsPerMinute;

            return new TimeOfDay(Pack(h, m, s));
        }

        /// <summary>
        /// Creates a new instance of <see cref="TimeOfDay"/> from the specified elapsed milliseconds
        /// since midnight.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="millisecondOfDay"/> is out of range.
        /// </exception>
        [Pure]
        public static TimeOfDay FromMillisecondsSinceMidnight(int millisecondOfDay)
        {
            if (millisecondOfDay < 0 || millisecondOfDay >= MillisecondsPerDay)
                Throw.ArgumentOutOfRange(nameof(millisecondOfDay));

            return FromMillisecondsSinceMidnightCore(millisecondOfDay);
        }

        /// <summary>
        /// Creates a new instance of <see cref="TimeOfDay"/> from the specified elapsed milliseconds
        /// since midnight.
        /// <para>This method does NOT validate its parameter.</para>
        /// </summary>
        [Pure]
        internal static TimeOfDay FromMillisecondsSinceMidnightCore(int millisecondOfDay)
        {
            int h = millisecondOfDay / MillisecondsPerHour;
            int m = millisecondOfDay / MillisecondsPerMinute % MinutesPerHour;
            int s = millisecondOfDay / MillisecondsPerSecond % SecondsPerMinute;
            int ms = millisecondOfDay % MillisecondsPerSecond;

            return new TimeOfDay(Pack(h, m, s, ms));
        }

        /// <summary>
        /// Creates a new instance of <see cref="TimeOfDay"/> from the specified fraction of the day.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="fractionOfDay"/> is out of range.
        /// </exception>
        [Pure]
        public static TimeOfDay FromFractionOfDay(double fractionOfDay)
        {
            if (fractionOfDay < 0d || fractionOfDay >= 1d)
                Throw.ArgumentOutOfRange(nameof(fractionOfDay));

            int millisecondOfDay = (int)(fractionOfDay * MillisecondsPerDay);

            return FromMillisecondsSinceMidnightCore(millisecondOfDay);
        }

        /// <summary>
        /// Creates a new instance of <see cref="TimeOfDay64"/> from the specified fraction of the
        /// day.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="fractionOfDay"/> is out of range.
        /// </exception>
        [Pure]
        internal static TimeOfDay FromFractionOfDay(decimal fractionOfDay)
        {
            if (fractionOfDay < 0m || fractionOfDay >= 1m)
                Throw.ArgumentOutOfRange(nameof(fractionOfDay));

            int millisecondOfDay = (int)(fractionOfDay * MillisecondsPerDay);

            return FromMillisecondsSinceMidnightCore(millisecondOfDay);
        }

        #endregion
        #region Conversions

        /// <summary>
        /// Converts the current instance to a <see cref="TimeOfDay64"/>.
        /// </summary>
        [Pure]
        public TimeOfDay64 ToTimeOfDay64() => new((long)NanosecondsPerMillisecond * MillisecondOfDay);

        /// <summary>
        /// Converts this instance to a fraction of the day.
        /// </summary>
        [Pure]
        public double ToFractionOfDay() => (double)MillisecondOfDay / MillisecondsPerDay;

        /// <summary>
        /// Converts this instance to a fraction of the day.
        /// </summary>
        [Pure]
        internal decimal ToDecimal() => (decimal)MillisecondOfDay / MillisecondsPerDay;

        #endregion
    }

    public partial struct TimeOfDay // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="TimeOfDay"/> are equal.
        /// </summary>
        public static bool operator ==(TimeOfDay left, TimeOfDay right) => left._bin == right._bin;

        /// <summary>
        /// Determines whether two specified instances of <see cref="TimeOfDay"/> are not equal.
        /// </summary>
        public static bool operator !=(TimeOfDay left, TimeOfDay right) => left._bin != right._bin;

        /// <inheritdoc />
        [Pure]
        public bool Equals(TimeOfDay other) => _bin == other._bin;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is TimeOfDay timeOfDay && Equals(timeOfDay);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => _bin;
    }

    public partial struct TimeOfDay // IComparable
    {
        /// <summary>
        /// Compares the two specified times of the day to see if the left one is strictly earlier
        /// than the right one.
        /// </summary>
        public static bool operator <(TimeOfDay left, TimeOfDay right) => left._bin < right._bin;

        /// <summary>
        /// Compares the two specified times of the day to see if the left one is earlier than or
        /// equal to the right one.
        /// </summary>
        public static bool operator <=(TimeOfDay left, TimeOfDay right) => left._bin <= right._bin;

        /// <summary>
        /// Compares the two specified times of the day to see if the left one is strictly later
        /// than the right one.
        /// </summary>
        public static bool operator >(TimeOfDay left, TimeOfDay right) => left._bin > right._bin;

        /// <summary>
        /// Compares the two specified times of the day to see if the left one is later than or equal
        /// to the right one.
        /// </summary>
        public static bool operator >=(TimeOfDay left, TimeOfDay right) => left._bin >= right._bin;

        /// <summary>
        /// Obtains the earlier time of two specified times.
        /// </summary>
        [Pure]
        public static TimeOfDay Min(TimeOfDay left, TimeOfDay right) => left < right ? left : right;

        /// <summary>
        /// Obtains the later time of two specified times.
        /// </summary>
        [Pure]
        public static TimeOfDay Max(TimeOfDay left, TimeOfDay right) => left > right ? left : right;

        /// <summary>
        /// Indicates whether this instance is earlier, later or the same as the specified one.
        /// </summary>
        [Pure]
        public int CompareTo(TimeOfDay other) => _bin.CompareTo(other._bin);

        /// <inheritdoc/>
        [Pure]
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is TimeOfDay hmss ? CompareTo(hmss)
            : Throw.NonComparable(typeof(TimeOfDay), obj);
    }
}
