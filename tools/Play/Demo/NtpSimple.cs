// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Play.Demo;

using System;
using System.Net;
using System.Net.Sockets;

using Zorglub.Time.Horology.Ntp;

using static System.Console;

public static class NtpSimple
{
    public static void Query()
    {
        // Other option: "time.windows.com".
        var cli = new SntpClient("fr.pool.ntp.org") { Version = 3 };
        var rsp = cli.Query();

        string reference = GetReference(rsp);

        WriteLine($"NTP response");
        WriteLine($"  Leap second:        {rsp.LeapIndicator}");
        WriteLine($"  Stratum:            {rsp.Stratum}");
        WriteLine($"  Reference source:   {reference}");
        WriteLine("  Reference time:     {0:HH:mm:ss.fff}", rsp.ReferenceTimestamp.ToDateTime());
        WriteLine("  Client transmit:    {0:HH:mm:ss.fff}", rsp.OriginateTimestamp.ToDateTime());
        WriteLine("  Server receive:     {0:HH:mm:ss.fff}", rsp.ReceiveTimestamp.ToDateTime());
        WriteLine("  Server transmit:    {0:HH:mm:ss.fff}", rsp.TransmitTimestamp.ToDateTime());
        WriteLine("  Client receive:     {0:HH:mm:ss.fff}", rsp.DestinationTimestamp.ToDateTime());
        WriteLine($"  Clock offset:       {rsp.ClockOffset} ({rsp.ClockOffset.TotalSeconds:F3}s)");
        WriteLine($"  Round-trip delay:   {rsp.RoundTripDelay} ({rsp.RoundTripDelay.Milliseconds}ms)");
        WriteLine($"  Root delay:         {rsp.RootDelay} ({rsp.RootDelay.Nanoseconds}ns)");
        WriteLine($"  Root dispersion:    {rsp.RootDispersion} ({rsp.RootDispersion.Nanoseconds}ns)");
        WriteLine($"  Poll interval:      2^{rsp.PollInterval}");
        WriteLine($"  Precision:          2^{rsp.Precision}");

        static string GetReference(SntpResponse rsp)
        {
            var refid = rsp.ReferenceId;
            if (refid is null)
            {
                return "NULL";
            }
            else if (rsp.Stratum == NtpStratum.SecondaryReference && rsp.Version == 3)
            {
                try
                {
                    var host = Dns.GetHostEntry(refid);
                    return FormattableString.Invariant($"{host.HostName} ({refid})");
                }
                catch (SocketException)
                {
                    return refid;
                }
            }
            else
            {
                return refid.Length == 0 ? "(not processed)" : refid;
            }
        }
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
