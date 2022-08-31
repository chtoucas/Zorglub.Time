// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    using System.Buffers.Binary;
    using System.Globalization;
    using System.Text;

    internal static partial class Rfc4330
    {
        public const int DataLength = 48;

        // +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
        // |LI | VN  |Mode |    Stratum    |     Poll      |   Precision   |
        // +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
        // |                          Root Delay                           |
        // |                       Root Dispersion                         |
        // |                     Reference Identifier                      |
        // |                   Reference Timestamp (64)                    |
        // |                   Originate Timestamp (64)                    |
        // |                    Receive Timestamp (64)                     |
        // |                    Transmit Timestamp (64)                    |
        // |                 Key Identifier (optional) (32)                |
        // |                 Message Digest (optional) (128)               |

        [Pure]
        public static SntpResponse ReadResponse(ReadOnlySpan<byte> buf, DateTime destinationTime)
        {
            Debug.Assert(buf != null);
            Debug.Assert(buf.Length >= DataLength);

            var rsp = new SntpResponse
            {
                // First byte.
                LeapIndicator = ReadLeapIndicator((buf[0] >> 6) & 3),
                Version = (buf[0] >> 3) & 7,
                Mode = ReadMode(buf[0] & 7),

                Stratum = ReadStratum(buf[1]),
                // (int)Math.Pow(2, bin[2]);
                PollInterval = buf[2],
                // Math.Pow(2, bin[3]);
                Precision = buf[3],

                //RootDelay = NtpDuration.FromBytes(buf[4..]),
                //RootDispersion = NtpDuration.FromBytes(buf[8..]),

                // Bytes 13 to 16; see Reference below.

                ReferenceTimestamp = NtpTimestamp.FromBytes(buf[16..]),
                OriginateTimestamp = NtpTimestamp.FromBytes(buf[24..]),
                ReceiveTimestamp = NtpTimestamp.FromBytes(buf[32..]),
                TransmitTimestamp = NtpTimestamp.FromBytes(buf[40..]),

                //
                DestinationTime = destinationTime
            };

            rsp.Reference = GetReference(rsp, buf[12..16]);

            ValidateResponse(rsp);

            return rsp;
        }

        public static void ValidateResponse(SntpResponse rsp)
        {
            if (rsp.LeapIndicator == LeapIndicator.Invalid
                || rsp.LeapIndicator == LeapIndicator.Alarm)
                Throw.Argument(nameof(rsp));

            if (rsp.Mode != NtpMode.Server) Throw.Argument(nameof(rsp));

            if (rsp.Stratum != NtpStratum.PrimaryReference
                && rsp.Stratum != NtpStratum.SecondaryReference)
                Throw.Argument(nameof(rsp));
        }

        [Pure]
        public static LeapIndicator ReadLeapIndicator(int x) =>
            // 2-bit number.
            x switch
            {
                0 => LeapIndicator.NoWarning,
                1 => LeapIndicator.PositiveLeapSecond,
                2 => LeapIndicator.NegativeLeapSecond,
                3 => LeapIndicator.Alarm,

                _ => LeapIndicator.Invalid
            };

        [Pure]
        public static NtpMode ReadMode(int x) =>
            // 3-bit number.
            x switch
            {
                0 => NtpMode.Reserved,
                1 => NtpMode.SymmetricActive,
                2 => NtpMode.SymmetricPassive,
                3 => NtpMode.Client,
                4 => NtpMode.Server,
                5 => NtpMode.Broadcast,
                6 => NtpMode.ReservedForNtpControlMessage,
                7 => NtpMode.ReservedForPrivateUse,

                _ => NtpMode.Invalid
            };

        [Pure]
        public static NtpStratum ReadStratum(byte x) =>
            // 8-bit unsigned integer, a byte.
            x switch
            {
                0 => NtpStratum.Unavailable,
                1 => NtpStratum.PrimaryReference,
                <= 15 => NtpStratum.SecondaryReference,
                _ => NtpStratum.Reserved
            };

        [Pure]
        // CIL code size = XXX bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadUInt32(ReadOnlySpan<byte> buf)
        {
            Debug.Assert(buf.Length >= 4);

#if true
            return BinaryPrimitives.ReadUInt32BigEndian(buf);
#else
            return
                (uint)buf[0] << 24
                | (uint)buf[1] << 16
                | (uint)buf[2] << 8
                | buf[3];
#endif
        }

        [Pure]
        public static ulong ReadUInt64(ReadOnlySpan<byte> buf)
        {
            Debug.Assert(buf.Length >= 8);

#if true
            return BinaryPrimitives.ReadUInt64BigEndian(buf);
#else
            return
                (ulong)buf[0] << 56
                | (ulong)buf[1] << 48
                | (ulong)buf[2] << 40
                | (ulong)buf[3] << 32
                | (ulong)buf[4] << 24
                | (ulong)buf[5] << 16
                | (ulong)buf[6] << 8
                | buf[7];
#endif
        }

#if false
        // https://stackoverflow.com/questions/1193955/how-to-query-an-ntp-server-using-c

        ulong GetPart(int startIndex) =>
            SwapEndianness(BitConverter.ToUInt32(buf, startIndex));

        // stackoverflow.com/a/3294698/162671
        static uint SwapEndianness(ulong n)
        {
            return (uint)(((n & 0x000000ff) << 24) +
                            ((n & 0x0000ff00) << 8) +
                            ((n & 0x00ff0000) >> 8) +
                            ((n & 0xff000000) >> 24));
        }
#endif
    }

    internal partial class Rfc4330
    {
        [Pure]
        public static string GetReference(SntpResponse rsp, ReadOnlySpan<byte> buf)
        {
            Debug.Assert(rsp != null);
            Debug.Assert(buf.Length == 4);

            return rsp.Stratum switch
            {
                NtpStratum.Unavailable
                or NtpStratum.PrimaryReference =>
                    // Four-character ASCII string, left justified and zero
                    // padded to 32 bits.
                    Encoding.ASCII.GetString(buf),

                NtpStratum.SecondaryReference =>
                    rsp.Version switch
                    {
                        // 32-bit IPv4 address of the reference source.
                        3 => FormattableString.Invariant($"{buf[0]}.{buf[1]}.{buf[2]}.{buf[3]}"),

                        // Low order 32 bits of the latest transmit timestamp of
                        // the reference source.
                        4 => throw new NotImplementedException(),
                        // TODO(code): low order 32 bits only!
                        // NtpTimestamp.FromBytes(buf).ToDateTime().ToString(CultureInfo.CurrentCulture),

                        _ => String.Empty
                    },

                _ => String.Empty,
            };
            ;
        }
    }
}
