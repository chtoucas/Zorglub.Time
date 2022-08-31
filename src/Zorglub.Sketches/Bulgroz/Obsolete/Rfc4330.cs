// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Obsolete
{
    internal static class Rfc4330
    {
        [Pure]
        public static uint ReadUInt32(ReadOnlySpan<byte> buf)
        {
            Debug.Assert(buf.Length >= 4);

            return
                (uint)buf[0] << 24
                | (uint)buf[1] << 16
                | (uint)buf[2] << 8
                | buf[3];
        }

        [Pure]
        public static ulong ReadUInt64(ReadOnlySpan<byte> buf)
        {
            Debug.Assert(buf.Length >= 8);

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
}
