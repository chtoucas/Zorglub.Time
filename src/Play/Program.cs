// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1812 // Avoid uninstantiated internal classes (Performance)
#pragma warning disable CA1852

using System;

using Play.Demo;

using Zorglub.Time.Horology;

using static System.Console;
using static Zorglub.Time.Core.TemporalConstants;

//Showcase.Run();

WriteLine($"Now = {SystemClocks.Local.Now()}");
WriteLine($"UTC now = {SystemClocks.Utc.Now()}");

const int DaysTo1601 = 584_388;
const long FileTimeOffset = DaysTo1601 * TicksPerDay;

var now = DateTime.Now;
var utcNow = DateTime.UtcNow;
// Ticks = number of ticks since 01/01/0001 (Gregorian) at 00:00.
// ToFileTime() = number of ticks since 01/01/1601 (Gregorian) at 00:00 (UTC).

WriteLine("Local");
WriteLine($"  Ticks           = {now.Ticks}");
WriteLine($"  ToFileTime()    = {now.ToFileTime() + FileTimeOffset}");
WriteLine($"  ToFileTimeUtc() = {now.ToFileTimeUtc() + FileTimeOffset}");
WriteLine("UTC");
WriteLine($"  Ticks           = {utcNow.Ticks}");
WriteLine($"  ToFileTime()    = {utcNow.ToFileTime() + FileTimeOffset}");
WriteLine($"  ToFileTimeUtc() = {utcNow.ToFileTimeUtc() + FileTimeOffset}");

new SimpleSntp().QueryTime();
