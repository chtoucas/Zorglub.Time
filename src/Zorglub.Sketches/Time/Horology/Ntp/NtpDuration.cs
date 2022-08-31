// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    using System.Globalization;
    using System.Text;

    public sealed partial class NtpDuration
    {
        [Pure]
        internal static NtpDuration FromBytes(ReadOnlySpan<byte> buf)
        {
            Debug.Assert(buf.Length >= 8);

            throw new NotImplementedException();
        }

        //private readonly byte[] _bin;

        //public NtpDuration(byte[] bytes)
        //{
        //    Debug.Assert(bytes != null);
        //    _bin = bytes;
        //}

        //public double RootDelay
        //{
        //    get
        //    {
        //        int x = 256 * (256 * (256 * _bin[4] + _bin[5]) + _bin[6]) + _bin[7];
        //        return 1000 * ((double)x / 0x10000);
        //    }
        //}

        //public double RootDispersion
        //{
        //    get
        //    {
        //        int x = 256 * (256 * (256 * _bin[8] + _bin[9]) + _bin[10]) + _bin[11];
        //        return 1000 * ((double)x / 0x10000);
        //    }
        //}

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
