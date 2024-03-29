﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Play.Demo;

using System;

using Zorglub.Time.Horology.Ntp;

using static System.Console;
using static Zorglub.Time.Core.TemporalConstants;

public sealed class SimpleSntp
{
    // RFC 4330: RootDelay and RootDispersion >= 0 and < 1s.
    // Notice that positivity is always guaranteed.
    // Remember that 1s = 65536 fractional seconds.
    public Duration32 MaxRootDelay { get; init; } = new(0, 3500); // ~50 ms
    public Duration32 MaxRootDispersion { get; init; } = new(0, 3500); // ~50 ms

    public void QueryTime()
    {
        var cli = new SntpClient("pool.ntp.org");
        //var cli = new SntpClient("fr.pool.ntp.org") { EnableVersionCheck = true };
        //var cli = new SntpClient("time.windows.com");
        // Primary servers. WARNING: bad practice when using SNTP.
        // One official primary server.
        //var cli = new SntpClient("time.nist.gov") { SendTimeout = 1000, ReceiveTimeout = 1000 };
        // One non-official and leap-smearing primary server.
        //var cli = new SntpClient("time.google.com");

        var (si, ti) = cli.QueryTime();

        CheckResponse(si);

        double precisionInMicroseconds = MicrosecondsPerSecond * si.Precision;

        WriteLine("NTP response (server info)");
        WriteLine($"  Version:            {si.Version}");
        WriteLine($"  Leap second:        {si.LeapIndicator}");
        WriteLine($"  Stratum:            {si.StratumLevel} ({si.StratumFamily})");
        WriteLine($"  Reference ID:       {si.NtpCode} (\"{si.ReferenceId}\")");
        WriteLine("  Reference time:     {0:HH:mm:ss.fff}", si.ReferenceTimestamp.ToDateTime());
        WriteLine($"  RTT:                {si.RoundTripTime.TotalMilliseconds:F3}ms\t({si.RoundTripTime})");
        WriteLine($"  Dispersion:         {si.Dispersion.TotalMilliseconds:F3}ms\t({si.Dispersion})");
        WriteLine($"  Poll interval:      {si.PollInterval}s\t(2^{si.PollExponent})");
        WriteLine($"  Precision:          {precisionInMicroseconds:F3}µs\t(2^{si.PrecisionExponent})");

        WriteLine("NTP response (time info)");
        WriteLine("  Client transmits:   {0:HH:mm:ss.fff}", ti.RequestTimestamp.ToDateTime());
        WriteLine("  Server receives:    {0:HH:mm:ss.fff}", ti.ReceiveTimestamp.ToDateTime());
        WriteLine("  Server transmits:   {0:HH:mm:ss.fff}", ti.TransmitTimestamp.ToDateTime());
        WriteLine("  Client receives:    {0:HH:mm:ss.fff}", ti.ResponseTimestamp.ToDateTime());
        WriteLine($"  Clock offset:       {ti.ClockOffset.TotalSeconds:+#.###;0.000;-#.###}s\t({ti.ClockOffset})");
        WriteLine($"  RTT:                {ti.RoundTripTime.TotalMilliseconds:F3}ms\t({ti.RoundTripTime})");
    }

    // Other things we could/should check:
    // - ReferenceCode (KissCode, LeapSmearing)
    // - ReferenceTimestamp is not too far in the past
    // - Peer sync distance: RootDelay / 2 + RootDispersion < 1s = distance threshold
    // RFC 5905 p.63
    // MINDISP .01s
    // MAXDISP 16s
    // MAXDIST 1s
    // MINPOLL 6
    // MAXPOLL 17
    private void CheckResponse(NtpServerInfo si)
    {
        if (si.RoundTripTime > MaxRootDelay)
            throw new NtpException(FormattableString.Invariant(
                $"Root delay >= {MaxRootDelay.TotalMilliseconds}ms: {si.RoundTripTime}."));
        if (si.Dispersion > MaxRootDispersion)
            throw new NtpException(FormattableString.Invariant(
                $"Root dispersion >= {MaxRootDispersion.TotalMilliseconds}ms: {si.Dispersion}."));
    }
}
