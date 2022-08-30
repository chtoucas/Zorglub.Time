// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1812 // Avoid uninstantiated internal classes (Performance)

using System;

using GuerrillaNtp;

using Play.Demo;

using Zorglub.Time.Horology;

using static Zorglub.Time.Core.TemporalConstants;

//Showcase.Run();

Console.WriteLine($"Now = {SystemClocks.Local.Now()}");
Console.WriteLine($"UTC now = {SystemClocks.Utc.Now()}");

const int DaysTo1601 = 584388;
const long FileTimeOffset = DaysTo1601 * TicksPerDay;

var now = DateTime.Now;
var utcNow = DateTime.UtcNow;
// Ticks = number of ticks since 01/01/0001 (Gregorian) at 00:00.
// ToFileTime() = number of ticks since 01/01/1601 (Gregorian) at 00:00 (UTC).

Console.WriteLine($"Local");
Console.WriteLine($"  Ticks           = {now.Ticks}");
Console.WriteLine($"  ToFileTime()    = {now.ToFileTime() + FileTimeOffset}");
Console.WriteLine($"  ToFileTimeUtc() = {now.ToFileTimeUtc() + FileTimeOffset}");
Console.WriteLine($"UTC");
Console.WriteLine($"  Ticks           = {utcNow.Ticks}");
Console.WriteLine($"  ToFileTime()    = {utcNow.ToFileTime() + FileTimeOffset}");
Console.WriteLine($"  ToFileTimeUtc() = {utcNow.ToFileTimeUtc() + FileTimeOffset}");

//NtpSimple.Guerrilla.Query();
NtpSimple.Bocan.Query();
