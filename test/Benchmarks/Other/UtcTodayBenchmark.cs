// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Other;

using NodaTime;

using Zorglub.Time;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Horology;
using Zorglub.Time.Simple;
using Zorglub.Time.Specialized;

using static NodaTime.Extensions.ClockExtensions;

/*
BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.2006/21H2/November2021Update)
Intel Core i7-4500U CPU 1.80GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.401
  [Host]     : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2

|             Method |      Mean |    Error |   StdDev | Ratio | Rank |
|------------------- |----------:|---------:|---------:|------:|-----:|
|   'CivilDate     ' |  48.17 ns | 0.133 ns | 0.117 ns |  0.85 |    I |
| 'CalendarDay     ' |  54.50 ns | 0.063 ns | 0.059 ns |  0.96 |   II |
| 'SystemClock     ' |  56.93 ns | 0.313 ns | 0.293 ns |  1.00 |  III |
|       'ZDate     ' |  60.38 ns | 0.409 ns | 0.383 ns |  1.06 |   IV |
|    'DateTime *   ' |  62.04 ns | 0.396 ns | 0.371 ns |  1.09 |    V |
|   'LocalDate *(Y)' | 121.39 ns | 0.147 ns | 0.130 ns |  2.13 |   VI |

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1889 (21H2)
Intel Core2 Duo CPU E8500 3.16GHz, 1 CPU, 2 logical and 2 physical cores
.NET SDK=6.0.400
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  DefaultJob : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT

|              Method |     Mean |     Error |    StdDev | Ratio | Rank |
|-------------------- |---------:|----------:|----------:|------:|-----:|
|    'CivilDate     ' | 1.127 μs | 0.0145 μs | 0.0129 μs |  1.00 |    I |
|  'SystemClock     ' | 1.129 μs | 0.0036 μs | 0.0032 μs |  1.00 |    I |
|        'ZDate     ' | 1.138 μs | 0.0084 μs | 0.0079 μs |  1.01 |    I |
|  'CalendarDay     ' | 1.138 μs | 0.0123 μs | 0.0115 μs |  1.01 |    I |
|     'DateTime *   ' | 1.158 μs | 0.0015 μs | 0.0013 μs |  1.03 |    I |
|    'LocalDate *(Y)' | 1.441 μs | 0.0012 μs | 0.0011 μs |  1.28 |  III |
 */
//
// * = external, Y = Y/M/D repr., O = ord. repr.

public class UtcTodayBenchmark
{
    [Benchmark(Description = "SystemClock     ", Baseline = true)]
    public (int, int, int, DayOfWeek) WithSystemClock()
    {
        var clock = SystemClocks.Utc;
        DayNumber today = clock.Today();
        var (y, m, d) = today.GetGregorianParts();

        return (y, m, d, today.DayOfWeek);
    }

    [Benchmark(Description = "CivilDate     ")]
    public (int, int, int, DayOfWeek) WithCivilDate()
    {
        var clock = CivilClock.Utc;
        CivilDate today = clock.GetCurrentDate();
        var (y, m, d) = today;

        return (y, m, d, today.DayOfWeek);
    }

    [Benchmark(Description = "CalendarDay     ")]
    public (int, int, int, DayOfWeek) WithCalendarDay()
    {
        var clock = SimpleCalendar.Civil.UtcClock;
        CalendarDay today = clock.GetCurrentDay();
        var (y, m, d) = today;

        return (y, m, d, today.DayOfWeek);
    }

    [Benchmark(Description = "ZDate     ")]
    public (int, int, int, DayOfWeek) WithZDate()
    {
        var clock = ZCalendar.Civil.UtcClock;
        ZDate today = clock.GetCurrentDate();
        var (y, m, d) = today;

        return (y, m, d, today.DayOfWeek);
    }

    [Benchmark(Description = "LocalDate *(Y)")]
    public (int, int, int, IsoDayOfWeek) WithLocalDate()
    {
        // This one uses the ISO calendar.
        var clock = SystemClock.Instance.InUtc();
        LocalDate now = clock.GetCurrentDate();
        var (y, m, d) = now;

        return (y, m, d, now.DayOfWeek);
    }

    [Benchmark(Description = "DateTime *   ")]
    public (int, int, int, DayOfWeek) WithDateTime()
    {
        DateTime now = DateTime.UtcNow;

        return (now.Year, now.Month, now.Day, now.DayOfWeek);
    }
}
