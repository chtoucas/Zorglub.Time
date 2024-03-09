// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Horology.Ntp;

#region Developer Notes

// There are differences in the way data types are defined by either RFC 4330
// (SNTP) or RFC 5905 (NTP). We follow RFC 5905.
//
// RFC 4330 says that root delay (resp. dispersion) is a 32-bit signed (resp.
// unsigned) fixed-point numbers and that they can be negative.
// RFC 5905 says that both are in NTP short format (unsigned); see also
// https://www.rfc-editor.org/rfc/rfc5905#appendix-A.5.1.1
//
// Poll interval and precision are 8-bit integers, in log2 seconds.
// RFC 4330: poll interval is unsigned and precision is signed.
// RFC 5905: both are signed.
// In fact, it does not really matter if the poll interval iss signed or not
// since its value is certainly in the range from 0 to 127 which is shared by
// both byte and sbyte.
//
// Clock resolution =
//   2^-p where p is the number of significant bits in the fraction part, e.g.
//   Timestamp64.RandomizeSubMilliseconds() randomize the 22 lower bits,
//   therefore the resolution is equal to 10 (~ millisecond).
// Clock precision =
//   Running time to read the system clock, in seconds.
// Precision = Max(clock resolution, clock precision).
//
// See also https://support.ntp.org/bin/view/Support/NTPRelatedDefinitions

#endregion

/// <summary>
/// Represents an NTP packet.
/// <para><see cref="NtpPacket"/> is an immutable struct.</para>
/// </summary>
/// <remarks>
/// <para>Beware, <see cref="NtpPacket"/> is a HUGE struct.</para>
/// </remarks>
internal readonly struct NtpPacket
{
    /// <summary>
    /// Represents the size of an NTP packet.
    /// <para>This field is a constant equal to 48.</para>
    /// </summary>
    public const int BinarySize = 48;

    /// <summary>
    /// Represents the offset of the Transmit Timestamp within an NTP packet.
    /// <para>This field is a constant equal to 40.</para>
    /// </summary>
    public const int TransmitTimestampOffset = 40;

    public required LeapIndicator LeapIndicator { get; init; }
    public required byte Version { get; init; }
    public required NtpMode Mode { get; init; }
    public required byte StratumLevel { get; init; }

    public required sbyte PollExponent { get; init; }
    public required sbyte PrecisionExponent { get; init; }
    public required Duration32 RootDelay { get; init; }
    public required Duration32 RootDispersion { get; init; }

    public required ReferenceId ReferenceId { get; init; }
    public required Timestamp64 ReferenceTimestamp { get; init; }

    public required Timestamp64 OriginateTimestamp { get; init; }
    public required Timestamp64 ReceiveTimestamp { get; init; }
    public required Timestamp64 TransmitTimestamp { get; init; }

    /// <summary>
    /// Reads an <see cref="NtpPacket"/> value from the beginning of a read-only span of bytes.
    /// </summary>
    /// <exception cref="AoorException"><paramref name="buf"/> is too small to contain an
    /// <see cref="NtpPacket"/>.</exception>
    [Pure]
    public static NtpPacket ReadFrom(ReadOnlySpan<byte> buf)
    {
        Debug.Assert(buf.Length >= BinarySize);

        // First byte.
        int li = (buf[0] >> 6) & 3; // 0 <= li < 4
        int vn = (buf[0] >> 3) & 7; // 0 <= vn < 8
        int mode = buf[0] & 7;      // 0 <= mode < 8

        return new NtpPacket
        {
            // Be sure to read the comments in LeapIndicator and NtpMode to
            // understand the + 1.
            LeapIndicator = (LeapIndicator)(li + 1),
            Version = (byte)vn,
            Mode = (NtpMode)(mode + 1),

            StratumLevel = buf[1],
            // Do NOT remove "unchecked".
            PollExponent = unchecked((sbyte)buf[2]),
            PrecisionExponent = unchecked((sbyte)buf[3]),

            RootDelay = Duration32.ReadFrom(buf[4..]),
            RootDispersion = Duration32.ReadFrom(buf[8..]),

            ReferenceId = ReferenceId.ReadFrom(buf[12..]),
            ReferenceTimestamp = Timestamp64.ReadFrom(buf[16..]),

            OriginateTimestamp = Timestamp64.ReadFrom(buf[24..]),
            ReceiveTimestamp = Timestamp64.ReadFrom(buf[32..]),
            TransmitTimestamp = Timestamp64.ReadFrom(buf[40..])
        };
    }
}
