// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Play.Demo;

using System;
using System.Net;
using System.Net.Sockets;

using Zorglub.Time.Horology.Ntp;

using static System.Console;
using static Zorglub.Time.Core.TemporalConstants;

public static class NtpSimple
{
    public static void Query()
    {
        // Other options: "time.windows.com", "fr.pool.ntp.org"
        var cli = new SntpClient("pool.ntp.org") { Version = 4 };
        var rsp = cli.Query();

        var si = rsp.ServerInfo;
        var ti = rsp.TimeInfo;

        var precisionInMicroseconds = MicrosecondsPerSecond * Math.Pow(2, si.Precision);

        WriteLine($"NTP response (server info)");
        WriteLine($"  Version:            {si.Version}");
        WriteLine($"  Leap second:        {si.LeapIndicator}");
        WriteLine($"  Stratum:            {si.Stratum}");
        WriteLine("  Reference time:     {0:HH:mm:ss.fff}", si.ReferenceTimestamp.ToDateTime());
        WriteLine($"  RTT:                {si.Rtt} ({si.Rtt.TotalMilliseconds:F3}ms)");
        WriteLine($"  Dispersion:         {si.Dispersion} ({si.Dispersion.TotalMilliseconds:F3}ms)");
        WriteLine($"  Poll interval:      {1 << si.PollInterval}s");
        WriteLine($"  Precision:          2^{si.Precision} ({precisionInMicroseconds:F3}µs)");

        WriteLine($"NTP response (time info)");
        WriteLine("  Client transmits:   {0:HH:mm:ss.fff}", ti.RequestTimestamp.ToDateTime());
        WriteLine("  Server receives:    {0:HH:mm:ss.fff}", ti.ReceiveTimestamp.ToDateTime());
        WriteLine("  Server transmits:   {0:HH:mm:ss.fff}", ti.TransmitTimestamp.ToDateTime());
        WriteLine("  Client receives:    {0:HH:mm:ss.fff}", ti.ResponseTimestamp.ToDateTime());
        WriteLine($"  Clock offset:       {ti.ClockOffset} ({ti.ClockOffset.TotalSeconds:+#.###;0.000;-#.###}s)");
        WriteLine($"  RTT:                {ti.Rtt} ({ti.Rtt.TotalMilliseconds:F3}ms)");
    }

    //[SuppressMessage("Design", "CA1034:Nested types should not be visible")]
    //public static class Guerrilla
    //{
    //    public static void Query() => Query(NtpClient.Default);

    //    public static void Query(string host)
    //    {
    //        WriteLine("Querying {0}...", host);

    //        Query(new NtpClient(host));
    //    }

    //    // Adapted from https://github.com/robertvazan/guerrillantp/blob/master/GuerrillaNtp.Cli/Program.cs
    //    [CLSCompliant(false)]
    //    public static void Query(NtpClient cli)
    //    {
    //        if (cli is null) throw new ArgumentNullException(nameof(cli));

    //        var clock = cli.Query();
    //        var rsp = clock.Response;

    //        WriteLine("NTP response");
    //        WriteLine("  Synchronized:       {0}", clock.Synchronized ? "yes" : "no");
    //        WriteLine("  Network time (UTC): {0:HH:mm:ss.fff}", clock.UtcNow);
    //        WriteLine("  Network time:       {0:HH:mm:ss.fff}", clock.Now);
    //        WriteLine("  Correction offset:  {0:s'.'FFFFFFF}", clock.CorrectionOffset);
    //        WriteLine("  Round-trip time:    {0:s'.'FFFFFFF}", clock.RoundTripTime);
    //        WriteLine("  Origin time:        {0:HH:mm:ss.fff}", rsp.OriginTimestamp);
    //        WriteLine("  Receive time:       {0:HH:mm:ss.fff}", rsp.ReceiveTimestamp);
    //        WriteLine("  Transmit time:      {0:HH:mm:ss.fff}", rsp.TransmitTimestamp);
    //        WriteLine("  Destination time:   {0:HH:mm:ss.fff}", rsp.DestinationTimestamp);
    //        WriteLine("  Leap second:        {0}", rsp.LeapIndicator);
    //        WriteLine("  Stratum:            {0}", rsp.Stratum);
    //        WriteLine("  Reference ID:       0x{0:x}", rsp.ReferenceId);
    //        WriteLine("  Reference time:     {0:HH:mm:ss.fff}", rsp.ReferenceTimestamp);
    //        WriteLine("  Root delay:         {0}ms", rsp.RootDelay.TotalMilliseconds);
    //        WriteLine("  Root dispersion:    {0}ms", rsp.RootDispersion.TotalMilliseconds);
    //        WriteLine("  Poll interval:      2^{0}s", rsp.PollInterval);
    //        WriteLine("  Precision:          2^{0}s", rsp.Precision);
    //    }
    //}
}
