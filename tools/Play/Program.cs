// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1812 // Avoid uninstantiated internal classes (Performance)

//using Play.Demo;

//Showcase.Run();

using System;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Horology;

using static Zorglub.Time.Core.TemporalConstants;

//var now = SystemClocks.Local.Now();
//var utcNow = SystemClocks.Utc.Now();

//Console.WriteLine($"Now = {now}");
//Console.WriteLine($"UTC now = {utcNow}");

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
