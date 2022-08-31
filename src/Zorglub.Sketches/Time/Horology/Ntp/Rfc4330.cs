// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    using System.Globalization;
    using System.Text;

    internal sealed class Rfc4330
    {
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

        public const int DataLength = 48;

        public const int RootDelayOffset = 4;       // Indexes 4..7
        public const int RootDispersionOffset = 8;  // Indexes 8..11
        public const int ReferenceOffset = 12;      // Indexes 12..15
        public const int ReferenceTimeOffset = 16;  // Indexes 16..23
        public const int OriginateTimeOffset = 24;  // Indexes 24..31
        public const int ReceiveTimeOffset = 32;    // Indexes 32..39
        public const int TransmitTimeOffset = 40;   // Indexes 40..47

        public static void ReadResponse(byte[] buffer)
        {
            var buf = buffer.AsSpan();
            // First byte.
            var leapIndicator = ReadLeapIndicator((buf[0] >> 6) & 3);
            int version = (buf[0] >> 3) & 7;
            var mode = ReadMode(buf[0] & 7);
            // Bytes 2 to 4.
            var stratum = ReadStratum(buf[1]);
            byte pollInterval = buf[2];
            byte precision = buf[3];
            // Bytes 5 to 8. RootDelay
            var rootDelay = ReadDuration(buf[4..]);
            // Bytes 9 to 12. RootDispersion
            var rootDispersion = ReadDuration(buf[4..]);
            // Bytes 13 to 16. Reference
            var reference = ReadReference(buf[12..16], version, stratum);
            // Bytes 17 to 24. ReferenceTime
            var referenceTime = ReadTimestamp(buf[16..]);
            // Bytes 25 to 32. OriginateTime
            var originateTime = ReadTimestamp(buf[24..]);
            // Bytes 33 to 40. ReceiveTime
            var receiveTime = ReadTimestamp(buf[32..]);
            // Bytes 41 to 48. TransmitTime
            var transmitTime = ReadTimestamp(buf[40..]);
        }

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

        public static NtpStratum ReadStratum(byte x) =>
            // 8-bit unsigned integer, a byte.
            x switch
            {
                0 => NtpStratum.Unavailable,
                1 => NtpStratum.PrimaryReference,
                <= 15 => NtpStratum.SecondaryReference,
                _ => NtpStratum.Reserved
            };

        private static string ReadReference(ReadOnlySpan<byte> buf, int version, NtpStratum stratum)
        {
            const string Invalid = "????";

            Debug.Assert(buf.Length == 4);

            switch (stratum)
            {
                case NtpStratum.Unavailable:
                case NtpStratum.PrimaryReference:
                    return Encoding.ASCII.GetString(buf);

                case NtpStratum.SecondaryReference:
                    return version switch
                    {
                        // IP address.
                        3 => FormattableString.Invariant($"{buf[0]}.{buf[1]}.{buf[2]}.{buf[3]}"),
                        // Timestamp.
                        4 => throw new NotImplementedException(),

                        _ => Invalid
                    };

                case NtpStratum.Reserved:
                default:
                    return Invalid;
            };
        }

        private static Timestamp ReadTimestamp(ReadOnlySpan<byte> buf)
        {
            throw new NotImplementedException();
        }

        private static ulong ReadDuration(ReadOnlySpan<byte> buf)
        {
            throw new NotImplementedException();
        }

        private static ulong ReadTimestampCore(ReadOnlySpan<byte> buf)
        {
            ulong high = ReadUInt32(buf);
            ulong low = ReadUInt32(buf[4..]);

            return 1000 * high + 1000 * low / 0x100_000_000L;
        }

        private static uint ReadUInt32(ReadOnlySpan<byte> buf)
        {
            Debug.Assert(buf.Length == 4);

            return
                (uint)buf[0] << 24
                | (uint)buf[1] << 16
                | (uint)buf[2] << 8
                | buf[3];
        }

        private static ulong ReadUInt64(ReadOnlySpan<byte> buf)
        {
            Debug.Assert(buf.Length == 8);

            return
                (ulong)buf[0] << 56
                | (ulong)buf[1] << 48
                | (ulong)buf[2] << 40
                | (ulong)buf[3] << 32
                | (ulong)buf[4] << 24
                | (ulong)buf[5] << 16
                | (ulong)buf[6] << 8
                | buf[7];
        }
    }
}
