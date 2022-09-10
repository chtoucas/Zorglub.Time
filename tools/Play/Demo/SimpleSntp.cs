// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Play.Demo;

using System;

using Zorglub.Time.Horology.Ntp;

using static System.Console;
using static Zorglub.Time.Core.TemporalConstants;

public static class SimpleSntp
{
    public static void QueryTime()
    {
        var cli = new SntpClient("pool.ntp.org");
        //var cli = new SntpClient("fr.pool.ntp.org");
        //var cli = new SntpClient("time.windows.com") { DisableVersionCheck = true };
        // One official primary server.
        //var cli = new SntpClient("time.nist.gov") { DisableVersionCheck = true, SendTimeout = 1000, ReceiveTimeout = 1000 };
        // One non-official and leap-smearing primary server.
        //var cli = new SntpClient("time.google.com");

        var (si, ti) = cli.QueryTime();

        double precisionInMicroseconds = MicrosecondsPerSecond * Math.Pow(2, si.Precision);

        WriteLine("NTP response (server info)");
        WriteLine($"  Version:            {si.Version}");
        WriteLine($"  Leap second:        {si.LeapIndicator}");
        WriteLine($"  Stratum:            {si.StratumLevel} ({si.StratumFamily})");
        WriteLine($"  Reference ID:       {si.NtpCode} (\"{si.ReferenceId}\")");
        WriteLine("  Reference time:     {0:HH:mm:ss.fff}", si.ReferenceTimestamp.ToDateTime());
        WriteLine($"  RTT:                {si.Rtt.TotalMilliseconds:F3}ms\t({si.Rtt})");
        WriteLine($"  Dispersion:         {si.Dispersion.TotalMilliseconds:F3}ms\t({si.Dispersion})");
        WriteLine($"  Poll interval:      {1 << si.PollInterval}s\t(2^{si.PollInterval})");
        WriteLine($"  Precision:          {precisionInMicroseconds:F3}µs\t(2^{si.Precision})");

        WriteLine("NTP response (time info)");
        WriteLine("  Client transmits:   {0:HH:mm:ss.fff}", ti.RequestTimestamp.ToDateTime());
        WriteLine("  Server receives:    {0:HH:mm:ss.fff}", ti.ReceiveTimestamp.ToDateTime());
        WriteLine("  Server transmits:   {0:HH:mm:ss.fff}", ti.TransmitTimestamp.ToDateTime());
        WriteLine("  Client receives:    {0:HH:mm:ss.fff}", ti.ResponseTimestamp.ToDateTime());
        WriteLine($"  Clock offset:       {ti.ClockOffset.TotalSeconds:+#.###;0.000;-#.###}s\t({ti.ClockOffset})");
        WriteLine($"  RTT:                {ti.Rtt.TotalMilliseconds:F3}ms\t({ti.Rtt})");
    }
}
