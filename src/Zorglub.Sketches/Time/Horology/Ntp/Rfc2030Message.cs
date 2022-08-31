// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    using System.Globalization;
    using System.Text;

    public sealed partial class Rfc2030Message
    {
        private readonly byte[] _bin;

        // This constructor does NOT validate its parameter.
        private Rfc2030Message(byte[] bytes)
        {
            Debug.Assert(bytes != null);
            //Debug.Assert(bytes.Length < Rfc4330.DataLength);

            _bin = bytes;
        }

        public static Rfc2030Message Request(DateTime transmitTime)
        {
            var bin = new byte[Rfc4330.DataLength];
            // Initialize the first byte to: LI = 0, VN = 3, Mode = 3.
            bin[0] = 0x1B;
            // TODO(code): Initialize TransmitTimestamp.

            return new(bin);
        }

        public static Rfc2030Message Response(byte[] data)
        {
            var msg = new Rfc2030Message(data);
            msg.CheckResponse();
            return msg;
        }

        public static Rfc2030Message FromBytes(byte[] data)
        {
            Requires.NotNull(data);
            // The binary message may be >= Rfc4330.DataLength (upward compatibility).
            if (data.Length < Rfc4330.DataLength) Throw.Argument(nameof(data));

            return new(data);
        }

        // Converts the current instance to a byte array.
        public byte[] ToArray() => _bin;

        private void CheckResponse()
        {
            if (LeapIndicator == LeapIndicator.Invalid
                || LeapIndicator == LeapIndicator.Alarm)
                throw new NtpException();

            if (Mode != NtpMode.Server) throw new NtpException();

            if (Stratum != NtpStratum.PrimaryReference
                && Stratum != NtpStratum.SecondaryReference)
                throw new NtpException();
        }
    }

    public partial class Rfc2030Message // First line
    {
        public LeapIndicator LeapIndicator => Rfc4330.ReadLeapIndicator((_bin[0] >> 6) & 3);

        public int Version => (_bin[0] >> 3) & 7;

        public NtpMode Mode => Rfc4330.ReadMode(_bin[0] & 7);

        public NtpStratum Stratum => Rfc4330.ReadStratum(_bin[1]);

        // (int)Math.Pow(2, bin[2]);
        public byte PollInterval => _bin[2];

        // Math.Pow(2, bin[3]);
        public byte Precision => _bin[3];

        public double RootDelay
        {
            get
            {
                int x = 256 * (256 * (256 * _bin[4] + _bin[5]) + _bin[6]) + _bin[7];
                return 1000 * ((double)x / 0x10000);
            }
        }

        public double RootDispersion
        {
            get
            {
                int x = 256 * (256 * (256 * _bin[8] + _bin[9]) + _bin[10]) + _bin[11];
                return 1000 * ((double)x / 0x10000);
            }
        }

        public string Reference
        {
            get
            {
                const string Invalid = "????";

                // Indexes 12..15.
                var bin = _bin[12..16];

                switch (Stratum)
                {
                    case NtpStratum.Unavailable:
                    case NtpStratum.PrimaryReference:
                        return Encoding.ASCII.GetString(bin);

                    case NtpStratum.SecondaryReference:
                        return Version switch
                        {
                            3 => FormattableString.Invariant($"{bin[0]}.{bin[1]}.{bin[2]}.{bin[3]}"),
                            4 => ReadUtcTime(12).ToString(CultureInfo.CurrentCulture),

                            _ => Invalid
                        };

                    case NtpStratum.Reserved:
                    default:
                        return Invalid;
                };
            }
        }

        public DateTime ReferenceTime => ReadUtcTime(Rfc4330.ReferenceTimeOffset);

        public DateTime OriginateTime => ReadUtcTime(Rfc4330.OriginateTimeOffset);

        public DateTime ReceiveTime => ReadUtcTime(Rfc4330.ReceiveTimeOffset);

        public DateTime TransmitTime => ReadUtcTime(Rfc4330.TransmitTimeOffset);
    }

    public partial class Rfc2030Message // NTP Timestamp Format
    {
        private static readonly DateTime s_Epoch = new(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private DateTime ReadUtcTime(byte offset)
        {
            ulong milliseconds = ReadTimestamp(offset);

            return s_Epoch + TimeSpan.FromMilliseconds(milliseconds);
        }

        private ulong ReadTimestamp(byte startIndex)
        {
            ulong high = GetPart(startIndex);
            ulong low = GetPart(startIndex + 4);

            return 1000 * high + 1000 * low / 0x100000000L;

#if false
            // https://stackoverflow.com/questions/1193955/how-to-query-an-ntp-server-using-c

            ulong GetPart(int startIndex) =>
                SwapEndianness(BitConverter.ToUInt32(_bin, startIndex));

            // stackoverflow.com/a/3294698/162671
            static uint SwapEndianness(ulong n)
            {
                return (uint)(((n & 0x000000ff) << 24) +
                               ((n & 0x0000ff00) << 8) +
                               ((n & 0x00ff0000) >> 8) +
                               ((n & 0xff000000) >> 24));
            }
#else
            ulong GetPart(int startIndex)
            {
                return
                    (ulong)_bin[startIndex] << 24
                    | (ulong)_bin[startIndex + 1] << 16
                    | (ulong)_bin[startIndex + 2] << 8
                    | _bin[startIndex + 3];
            }
#endif
        }

        //internal static void WriteTimestamp(byte[] rawdata, byte offset, DateTime date)
        //{
        //    var epoch = new DateTime(1900, 1, 1, 0, 0, 0);

        //    ulong milliseconds = (ulong)(date - epoch).TotalMilliseconds;
        //    ulong intpart = milliseconds / 1000;
        //    ulong fractpart = ((milliseconds % 1000) * 0x100000000L) / 1000;

        //    ulong temp = intpart;
        //    for (int i = 3; i >= 0; i--)
        //    {
        //        rawdata[offset + i] = (byte)(temp % 256);
        //        temp /= 256;
        //    }

        //    temp = fractpart;
        //    for (int i = 7; i >= 4; i--)
        //    {
        //        rawdata[offset + i] = (byte)(temp % 256);
        //        temp /= 256;
        //    }
        //}
    }
}
