// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Play.Demo;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;

using Zorglub.Time.Horology.Ntp;

using GuerrillaNtp;

using static System.Console;

// Other option: "time.windows.com".

public static class NtpSimple
{
    public static void Query()
    {
        var cli = new SntpClient() { Version = 3 };
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
        WriteLine($"  Clock offset:       {rsp.ClockOffset}ms");
        WriteLine($"  Round-trip delay:   {rsp.RoundtripDelay} ({rsp.RoundtripDelay.CountMilliseconds()}ms)");
        WriteLine($"  Root delay:         {rsp.RootDelay} ({rsp.RootDelay.CountNanoseconds()}ns)");
        WriteLine($"  Root dispersion:    {rsp.RootDispersion} ({rsp.RootDispersion.CountNanoseconds()}ns)");
        WriteLine($"  Poll interval:      2^{rsp.PollInterval}");
        WriteLine($"  Precision:          {rsp.Precision}");

        static string GetReference(SntpResponse rsp)
        {
            var reference = rsp.Reference;
            if (reference is null)
            {
                return "NULL";
            }
            else if (rsp.Stratum == NtpStratum.SecondaryReference && rsp.Version == 3)
            {
                //var host = Dns.GetHostEntry(reference);
                //return FormattableString.Invariant($"{host.HostName} ({reference})");
                return reference;
            }
            else
            {
                return reference.Length == 0 ? "(not processed)" : reference;
            }
        }
    }

    [SuppressMessage("Design", "CA1034:Nested types should not be visible")]
    public static class Guerrilla
    {
        public static void Query() => Query(NtpClient.Default);

        public static void Query(string host)
        {
            WriteLine("Querying {0}...", host);

            Query(new NtpClient(host));
        }

        // Adapted from https://github.com/robertvazan/guerrillantp/blob/master/GuerrillaNtp.Cli/Program.cs
        [CLSCompliant(false)]
        public static void Query(NtpClient cli)
        {
            if (cli is null) throw new ArgumentNullException(nameof(cli));

            var clock = cli.Query();
            var rsp = clock.Response;

            WriteLine("NTP response");
            WriteLine("  Synchronized:       {0}", clock.Synchronized ? "yes" : "no");
            WriteLine("  Network time (UTC): {0:HH:mm:ss.fff}", clock.UtcNow);
            WriteLine("  Network time:       {0:HH:mm:ss.fff}", clock.Now);
            WriteLine("  Correction offset:  {0:s'.'FFFFFFF}", clock.CorrectionOffset);
            WriteLine("  Round-trip time:    {0:s'.'FFFFFFF}", clock.RoundTripTime);
            WriteLine("  Origin time:        {0:HH:mm:ss.fff}", rsp.OriginTimestamp);
            WriteLine("  Receive time:       {0:HH:mm:ss.fff}", rsp.ReceiveTimestamp);
            WriteLine("  Transmit time:      {0:HH:mm:ss.fff}", rsp.TransmitTimestamp);
            WriteLine("  Destination time:   {0:HH:mm:ss.fff}", rsp.DestinationTimestamp);
            WriteLine("  Leap second:        {0}", rsp.LeapIndicator);
            WriteLine("  Stratum:            {0}", rsp.Stratum);
            WriteLine("  Reference ID:       0x{0:x}", rsp.ReferenceId);
            WriteLine("  Reference time:     {0:HH:mm:ss.fff}", rsp.ReferenceTimestamp);
            WriteLine("  Root delay:         {0}ms", rsp.RootDelay.TotalMilliseconds);
            WriteLine("  Root dispersion:    {0}ms", rsp.RootDispersion.TotalMilliseconds);
            WriteLine("  Poll interval:      2^{0}s", rsp.PollInterval);
            WriteLine("  Precision:          2^{0}s", rsp.Precision);
        }
    }
}
