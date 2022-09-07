// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Play.Demo;

using System;

using Zorglub.Time.Horology.Ntp;

using static System.Console;
using static Zorglub.Time.Core.TemporalConstants;

public static class NtpSimple
{
    public static void Query()
    {
        var cli = new SntpClient();
        //var cli = new SntpClient("fr.pool.ntp.org");
        //var cli = new SntpClient("time.windows.com") { StrictValidation = false };

        var (si, ti) = cli.Query();

        string reference = GetReference(si);
        double precisionInMicroseconds = MicrosecondsPerSecond * Math.Pow(2, si.Precision);

        WriteLine("NTP response (server info)");
        WriteLine($"  Version:            {si.Version}");
        WriteLine($"  Leap second:        {si.LeapIndicator}");
        WriteLine($"  Stratum:            {si.Stratum}");
        WriteLine($"  Reference ID:       {reference}");
        WriteLine("  Reference time:     {0:HH:mm:ss.fff}", si.ReferenceTimestamp.ToDateTime());
        WriteLine($"  RTT:                {si.Rtt} ({si.Rtt.TotalMilliseconds:F3}ms)");
        WriteLine($"  Dispersion:         {si.Dispersion} ({si.Dispersion.TotalMilliseconds:F3}ms)");
        WriteLine($"  Poll interval:      {1 << si.PollInterval}s");
        WriteLine($"  Precision:          2^{si.Precision} ({precisionInMicroseconds:F3}µs)");

        WriteLine("NTP response (time info)");
        WriteLine("  Client transmits:   {0:HH:mm:ss.fff}", ti.RequestTimestamp.ToDateTime());
        WriteLine("  Server receives:    {0:HH:mm:ss.fff}", ti.ReceiveTimestamp.ToDateTime());
        WriteLine("  Server transmits:   {0:HH:mm:ss.fff}", ti.TransmitTimestamp.ToDateTime());
        WriteLine("  Client receives:    {0:HH:mm:ss.fff}", ti.ResponseTimestamp.ToDateTime());
        WriteLine($"  Clock offset:       {ti.ClockOffset} ({ti.ClockOffset.TotalSeconds:+#.###;0.000;-#.###}s)");
        WriteLine($"  RTT:                {ti.Rtt} ({ti.Rtt.TotalMilliseconds:F3}ms)");
    }

    static string GetReference(SntpServerInfo si)
    {
        return si.Stratum switch
        {
            NtpStratum.Unspecified =>
                FormattableString.Invariant($"{si.ReferenceCode} (Kiss Code)"),
            NtpStratum.PrimaryReference =>
                FormattableString.Invariant($".{si.ReferenceCode}. (Code)"),
            NtpStratum.SecondaryReference => GetSecondaryReference(),
            _ => String.Empty,
        };

        string GetSecondaryReference()
        {
            var r = BitConverter.GetBytes(si.ReferenceIdentifier);

            // See https://support.ntp.org/bin/view/Support/RefidFormat
            // https://github.com/ntp-project/ntp/blob/master-no-authorname/README.leapsmear
            if (r[0] == 254)
            {
                // TODO(code): compute the leap smearing value.
                uint smear = BitConverter.ToUInt32(r, 1);
                return FormattableString.Invariant(
                    $"\"254.{r[1]:x2}.{r[2]:x2}.{r[3]:x2}\" (smearing = {smear}s)");
            }
            else
            {
                return FormattableString.Invariant(
                    $"\"{r[0]:x2}{r[1]:x2}{r[2]:x2}{r[3]:x2}\" (IPv4?={r[0]}.{r[1]}.{r[2]}.{r[3]})");
            }
        }
    }
}
