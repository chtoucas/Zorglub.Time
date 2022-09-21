// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp;

using System.Buffers.Binary;

using static Zorglub.Time.Core.TemporalConstants;

// TODO(api): From/ToDateTime(), From/ToMoment().
// Y2036 (era) -> Zero becomes Epoch of the era.
// > If bit 0 is set, the UTC time is in the range 1968-2036, and UTC time
// > is reckoned from 0h 0m 0s UTC on 1 January 1900.
// > If bit 0 is not set, the time is in the range 2036-2104 and UTC time is
// > reckoned from 6h 28m 16s UTC on 7 February 2036.
// > Note that when calculating the correspondence, 2000 is a leap year, and
// > leap seconds are not included in the reckoning.
/* RFC 5905 p.14
   The only arithmetic operation permitted on dates and timestamps is
   twos-complement subtraction, yielding a 127-bit or 63-bit signed
   result.  It is critical that the first-order differences between two
   dates preserve the full 128-bit precision and the first-order
   differences between two timestamps preserve the full 64-bit
   precision.  However, the differences are ordinarily small compared to
   the seconds span, so they can be converted to floating double format
   for further processing and without compromising the precision.
 */

// Adapted from
// https://android.googlesource.com/platform/frameworks/base/+/master/core/java/android/net/sntp/Timestamp64.java

// See
// - https://www.eecis.udel.edu/~mills/time.html
// - https://www.eecis.udel.edu/~mills/y2k.html
// - https://www.eecis.udel.edu/~mills/leap.html

/// <summary>
/// Represents a 64-bit (unsigned) NTP timestamp; see RFC 5905, section 6.
/// <para><see cref="Timestamp64"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct Timestamp64 :
    // Comparison
    IComparisonOperators<Timestamp64, Timestamp64>,
    IMinMaxValue<Timestamp64>,
    IMinMaxFunctions<Timestamp64>,
    // Arithmetic
    ISubtractionOperators<Timestamp64, Timestamp64, Duration64>
{
    /// <summary>
    /// Represents the number of fractional seconds in one second.
    /// <para>This field is a constant equal to 4_294_967_296.</para>
    /// </summary>
    public const long FractionalSecondsPerSecond = 1L << 32;

    /// <summary>
    /// Represents the second of the NTP era.
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly uint _secondOfEra;

    /// <summary>
    /// Represents the fraction of the second.
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly uint _fractionOfSecond;

    /// <summary>
    /// Initializes a new instance of the <see cref="Timestamp64"/> struct.
    /// </summary>
    [CLSCompliant(false)]
    public Timestamp64(uint secondOfEra, uint fractionOfSecond)
    {
        _secondOfEra = secondOfEra;
        _fractionOfSecond = fractionOfSecond;
    }

    /// <summary>
    /// Gets the prime epoch, that is the epoch of first NTP era (numbered 0).
    /// <para>The Monday 1st of January, 1900 CE within the Gregorian calendar.</para>
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Timestamp64 Zero { get; }

    /// <summary>
    /// Gets the smallest possible value of a <see cref="Timestamp64"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Timestamp64 MinValue => Zero;

    /// <summary>
    /// Gets the largest possible value of a <see cref="Timestamp64"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Timestamp64 MaxValue { get; } = new(UInt32.MaxValue, UInt32.MaxValue);

    /// <summary>
    /// Gets the NTP pseudo-era.
    /// </summary>
    public int PseudoEra => 1 - (int)(_secondOfEra >> 31);

    /// <summary>
    /// Gets the second of the NTP era, i.e. the number of elapsed seconds since <see cref="Zero"/>.
    /// </summary>
    public long SecondOfEra => _secondOfEra;

    /// <summary>
    /// Gets the fraction of the second.
    /// </summary>
    public long FractionOfSecond => _fractionOfSecond;

    /// <summary>
    /// Gets the number of fractional seconds since Zero.
    /// </summary>
    private ulong FractionalSecondsSinceZero =>
        ConvertSecondOfEraToFractionalSeconds(_secondOfEra) | _fractionOfSecond;

    /// <summary>
    /// Returns a culture-independent string representation of the current instance.
    /// </summary>
    [Pure]
    public override string ToString() =>
        FormattableString.Invariant($"{_secondOfEra}s+{_fractionOfSecond}");

    /// <summary>
    /// Counts the number of elapsed (whole) milliseconds since <see cref="Zero"/>.
    /// </summary>
    [Pure]
    public long CountMillisecondsSinceZero() => (long)(
        MillisecondsPerSecond * (ulong)_secondOfEra
        + ConvertFractionOfSecondToMillisecondOfSecond(_fractionOfSecond));

    /// <summary>
    /// Counts the number of elapsed (whole) nanoseconds since <see cref="Zero"/>.
    /// </summary>
    [Pure]
    public long CountNanosecondsSinceZero() => (long)(
        NanosecondsPerSecond * (ulong)_secondOfEra
        + ConvertFractionOfSecondToNanosecondOfSecond(_fractionOfSecond));

    /// <summary>
    /// Randomizes the sub-milliseconds part of the current instance, yielding a new
    /// <see cref="Timestamp64"/>.
    /// </summary>
    [Pure]
    internal Timestamp64 RandomizeSubMilliseconds(IRandomNumberGenerator rng)
    {
        Debug.Assert(rng != null);

        // We randomize the submilliseconds part of fraction-of-second.
        //   1 millisecond = 2^32 / 1000 > 4_294_967 fraction-of-second
        // Therefore 2^22 (= 4_194_304) fraction-of-second < 1 millisecond.
        // See RFC 4303 Section 3, p.6.
        const int
            MillisecondResolution = 10,
            LowerBitsToRandomize = 32 - MillisecondResolution,
            MaxRandomExclusive = 1 << LowerBitsToRandomize;
        const long
            LowerBitMask = (1L << LowerBitsToRandomize) - 1L,
            UpperBitMask = ~LowerBitMask;

        int rnd = rng.GetInt32(0, MaxRandomExclusive);

        uint fractionOfSecond = (uint)(
            (_fractionOfSecond & UpperBitMask) | (rnd & LowerBitMask));

        return new Timestamp64(_secondOfEra, fractionOfSecond);
    }
}

public partial struct Timestamp64 // Time helpers
{
    // Unit of 1 fraction of second = 1 / 2^32 second
    // Relation to a subunit-of-second:
    // > subunit-of-second = (SubunitsPerSecond * fraction-of-second) / 2^32
    // > fraction-of-second = (2^32 * subunit-of-second) / SubunitsPerSecond
    // Precision is about 232 picoseconds.

    //
    // MillisecondOfSecond
    //

    /// <summary>
    /// Converts a millisecond of the second to a fraction of the second.
    /// </summary>
    [Pure]
    // CIL code size = 13 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint ConvertMillisecondOfSecondToFractionOfSecond(int millisecondOfSecond)
    {
        Debug.Assert(millisecondOfSecond >= 0);
        Debug.Assert(millisecondOfSecond < MillisecondsPerSecond);

        ulong fractionOfSecond = ((ulong)millisecondOfSecond << 32) / MillisecondsPerSecond;

        Debug.Assert(fractionOfSecond <= UInt32.MaxValue);

        return (uint)fractionOfSecond;
    }

    /// <summary>
    /// Converts a fraction of the second to a millisecond of the second.
    /// <para>The result is in the range from 0 (included) to 1000 (excluded).</para>
    /// </summary>
    [Pure]
    // CIL code size = 13 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong ConvertFractionOfSecondToMillisecondOfSecond(uint fractionOfSecond)
    {
        ulong millisecondOfSecond = (MillisecondsPerSecond * (ulong)fractionOfSecond) >> 32;

        Debug.Assert(millisecondOfSecond < MillisecondsPerSecond);

        return millisecondOfSecond;
    }

    //
    // NanosecondOfSecond
    //

    /// <summary>
    /// Converts a fraction of the second to a nanosecond of the second.
    /// <para>The result is in the range from 0 (included) to 1_000_000_000 (excluded).</para>
    /// </summary>
    [Pure]
    // CIL code size = 13 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong ConvertFractionOfSecondToNanosecondOfSecond(uint fractionOfSecond)
    {
        ulong nanosecondOfSecond = (NanosecondsPerSecond * (ulong)fractionOfSecond) >> 32;

        Debug.Assert(nanosecondOfSecond < NanosecondsPerSecond);

        return nanosecondOfSecond;
    }

    //
    // SecondOfEra
    //

    /// <summary>
    /// Converts a second of the era to a number of fractional seconds.
    /// </summary>
    [Pure]
    // CIL code size = 6 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong ConvertSecondOfEraToFractionalSeconds(uint secondOfEra) =>
        (ulong)secondOfEra << 32;
}

public partial struct Timestamp64 // Binary helpers
{
    /// <summary>
    /// Reads a <see cref="Timestamp64"/> value from the beginning of a read-only span of bytes.
    /// </summary>
    /// <exception cref="AoorException"><paramref name="buf"/> is too small to contain a
    /// <see cref="Timestamp64"/>.</exception>
    [Pure]
    internal static Timestamp64 ReadFrom(ReadOnlySpan<byte> buf)
    {
        Debug.Assert(buf.Length >= 8);

        uint secondOfEra = BinaryPrimitives.ReadUInt32BigEndian(buf);
        uint fractionOfSecond = BinaryPrimitives.ReadUInt32BigEndian(buf[4..]);

        return new Timestamp64(secondOfEra, fractionOfSecond);
    }

    /// <summary>
    /// Writes the current instance into a span of bytes.
    /// </summary>
    /// <exception cref="AoorException"><paramref name="buf"/> is too small to contain a
    /// <see cref="Timestamp64"/>.</exception>
    internal void WriteTo(Span<byte> buf, int index)
    {
        BinaryPrimitives.WriteUInt32BigEndian(buf[index..], _secondOfEra);
        BinaryPrimitives.WriteUInt32BigEndian(buf[(index + 4)..], _fractionOfSecond);
    }
}

public partial struct Timestamp64 // Conversions
{
    /// <summary>
    /// Represents the epoch of the first NTP era.
    /// <para>This field is read-only.</para>
    /// </summary>
    private static readonly DateTime s_Epoch = new(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Creates a new instance of <see cref="Timestamp64"/> from the specified time.
    /// </summary>
    [Pure]
    public static Timestamp64 FromDateTime(DateTime time)
    {
        if (time.Kind != DateTimeKind.Utc) Throw.Argument(nameof(time));

        double secondOfEra = (time - s_Epoch).TotalSeconds;
        uint fractionOfSecond = ConvertMillisecondOfSecondToFractionOfSecond(time.Millisecond);

        return new Timestamp64((uint)secondOfEra, fractionOfSecond);
    }

    /// <summary>
    /// Converts the current instance to a <see cref="DateTime"/>.
    /// </summary>
    [Pure]
    public DateTime ToDateTime() =>
        s_Epoch + TimeSpan.FromMilliseconds(CountMillisecondsSinceZero());
}

public partial struct Timestamp64 // IEquatable
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="Timestamp64"/> are equal.
    /// </summary>
    public static bool operator ==(Timestamp64 left, Timestamp64 right) =>
        left._secondOfEra == right._secondOfEra
        && left._fractionOfSecond == right._fractionOfSecond;

    /// <summary>
    /// Determines whether two specified instances of <see cref="Timestamp64"/> are not equal.
    /// </summary>
    public static bool operator !=(Timestamp64 left, Timestamp64 right) =>
        left._secondOfEra != right._secondOfEra
        || left._fractionOfSecond != right._fractionOfSecond;

    /// <inheritdoc />
    [Pure]
    public bool Equals(Timestamp64 other) =>
        _secondOfEra == other._secondOfEra
        && _fractionOfSecond == other._fractionOfSecond;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Timestamp64 timestamp && Equals(timestamp);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => HashCode.Combine(_secondOfEra, _fractionOfSecond);
}

public partial struct Timestamp64 // IComparable
{
    /// <inheritdoc />
    public static bool operator <(Timestamp64 left, Timestamp64 right) =>
        left.FractionalSecondsSinceZero < right.FractionalSecondsSinceZero;

    /// <inheritdoc />
    public static bool operator <=(Timestamp64 left, Timestamp64 right) =>
        left.FractionalSecondsSinceZero <= right.FractionalSecondsSinceZero;

    /// <inheritdoc />
    public static bool operator >(Timestamp64 left, Timestamp64 right) =>
        left.FractionalSecondsSinceZero > right.FractionalSecondsSinceZero;

    /// <inheritdoc />
    public static bool operator >=(Timestamp64 left, Timestamp64 right) =>
        left.FractionalSecondsSinceZero >= right.FractionalSecondsSinceZero;

    /// <inheritdoc />
    [Pure]
    public static Timestamp64 Min(Timestamp64 x, Timestamp64 y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static Timestamp64 Max(Timestamp64 x, Timestamp64 y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(Timestamp64 other) =>
        FractionalSecondsSinceZero.CompareTo(other.FractionalSecondsSinceZero);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is Timestamp64 timestamp ? CompareTo(timestamp)
        : Throw.NonComparable(typeof(Timestamp64), obj);
}

public partial struct Timestamp64 // Arithmetic
{
    /// <summary>
    /// Subtracts the two specified timestamps and returns the duration between them.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the range of
    /// <see cref="UInt64"/>.</exception>
    public static Duration64 operator -(Timestamp64 left, Timestamp64 right) => left.Subtract(right);

    /// <summary>
    /// Subtracts a timestamp from the current instance and returns the duration between them.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the range of
    /// <see cref="UInt64"/>.</exception>
    [Pure]
    public Duration64 Subtract(Timestamp64 other)
    {
        ulong start = other.FractionalSecondsSinceZero;
        ulong end = FractionalSecondsSinceZero;

        return end > start ? new Duration64((long)(end - start))
            : new Duration64((long)(start - end));
    }
}
