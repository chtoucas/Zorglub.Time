// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

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

|             Method |     Mean |   Error |  StdDev | Ratio | Rank |
|------------------- |---------:|--------:|--------:|------:|-----:|
|   'CivilDate     ' | 164.1 ns | 0.89 ns | 0.83 ns |  0.94 |    I |
| 'SystemClock     ' | 174.8 ns | 0.74 ns | 0.70 ns |  1.00 |   II |
| 'CalendarDay     ' | 176.0 ns | 0.95 ns | 0.84 ns |  1.01 |   II |
|   'LocalDate *(Y)' | 182.1 ns | 0.67 ns | 0.60 ns |  1.04 |  III |
|    'DateTime *   ' | 183.9 ns | 1.06 ns | 0.99 ns |  1.05 |  III |
|       'ZDate     ' | 184.2 ns | 0.85 ns | 0.79 ns |  1.05 |  III |

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1889 (21H2)
Intel Core2 Duo CPU E8500 3.16GHz, 1 CPU, 2 logical and 2 physical cores
.NET SDK=6.0.400
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  DefaultJob : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT

|              Method |     Mean |     Error |    StdDev | Ratio | Rank |
|-------------------- |---------:|----------:|----------:|------:|-----:|
|    'LocalDate *(Y)' | 1.500 μs | 0.0008 μs | 0.0006 μs |  0.97 |    I |
|    'CivilDate     ' | 1.544 μs | 0.0012 μs | 0.0010 μs |  1.00 |   II |
|  'SystemClock     ' | 1.550 μs | 0.0018 μs | 0.0014 μs |  1.00 |   II |
|  'CalendarDay     ' | 1.552 μs | 0.0017 μs | 0.0016 μs |  1.00 |   II |
|        'ZDate     ' | 1.561 μs | 0.0023 μs | 0.0020 μs |  1.01 |   II |
|     'DateTime *   ' | 1.627 μs | 0.0033 μs | 0.0031 μs |  1.05 |  III |
 */
//
// * = external, Y = Y/M/D repr., O = ord. repr.

public class TodayBenchmark
{
    [Benchmark(Description = "SystemClock     ", Baseline = true)]
    public (int, int, int, DayOfWeek) WithSystemClock()
    {
        var clock = SystemClocks.Local;
        DayNumber today = clock.Today();
        var (y, m, d) = today.GetGregorianParts();

        return (y, m, d, today.DayOfWeek);
    }

    [Benchmark(Description = "CivilDate     ")]
    public (int, int, int, DayOfWeek) WithCivilDate()
    {
        var clock = CivilClock.Local;
        CivilDate today = clock.GetCurrentDate();
        var (y, m, d) = today;

        return (y, m, d, today.DayOfWeek);
    }

    [Benchmark(Description = "CalendarDay     ")]
    public (int, int, int, DayOfWeek) WithCalendarDay()
    {
        var clock = SimpleCalendar.Civil.LocalClock;
        CalendarDay today = clock.GetCurrentDay();
        var (y, m, d) = today;

        return (y, m, d, today.DayOfWeek);
    }

    [Benchmark(Description = "ZDate     ")]
    public (int, int, int, DayOfWeek) WithZDate()
    {
        var clock = ZCalendar.Civil.LocalClock;
        ZDate today = clock.GetCurrentDate();
        var (y, m, d) = today;

        return (y, m, d, today.DayOfWeek);
    }

    [Benchmark(Description = "LocalDate *(Y)")]
    public (int, int, int, IsoDayOfWeek) WithLocalDate()
    {
        // This one uses the ISO calendar.
        var clock = SystemClock.Instance.InBclSystemDefaultZone();
        LocalDate now = clock.GetCurrentDate();
        var (y, m, d) = now;

        return (y, m, d, now.DayOfWeek);
    }

    [Benchmark(Description = "DateTime *   ")]
    public (int, int, int, DayOfWeek) WithDateTime()
    {
        DateTime now = DateTime.Now;

        return (now.Year, now.Month, now.Day, now.DayOfWeek);
    }
}
