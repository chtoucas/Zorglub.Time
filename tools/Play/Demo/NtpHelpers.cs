﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Play.Demo;

using System;

using GuerrillaNtp;

using static System.Console;

public static class NtpHelpers
{
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