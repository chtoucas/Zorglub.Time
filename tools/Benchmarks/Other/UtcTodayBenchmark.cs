// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Other;

using NodaTime;

using Zorglub.Time;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Simple;
using Zorglub.Time.Specialized;

using ZorglubSystemClock = Zorglub.Time.Horology.SystemClock;

using static NodaTime.Extensions.ClockExtensions;

/*
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1889 (21H2)
Intel Core i7-4500U CPU 1.80GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.400
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  DefaultJob : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT

|              Method |      Mean |    Error |   StdDev | Ratio | Rank |
|-------------------- |----------:|---------:|---------:|------:|-----:|
|    'CivilDate     ' |  48.39 ns | 0.226 ns | 0.177 ns |  0.85 |    I |
|  'CalendarDay     ' |  54.19 ns | 0.133 ns | 0.104 ns |  0.95 |   II |
|    'DayNumber     ' |  56.96 ns | 0.151 ns | 0.134 ns |  1.00 |  III |
|  'SystemClock     ' |  57.02 ns | 0.136 ns | 0.128 ns |  1.00 |  III |
|        'ZDate     ' |  59.54 ns | 0.241 ns | 0.226 ns |  1.04 |   IV |
|     'DateTime *   ' |  61.25 ns | 0.296 ns | 0.277 ns |  1.07 |    V |
|  'OrdinalDate  (O)' |  66.17 ns | 0.405 ns | 0.379 ns |  1.16 |   VI |
| 'CalendarDate  (Y)' |  66.33 ns | 0.474 ns | 0.443 ns |  1.16 |   VI |
|    'LocalDate *(Y)' | 118.81 ns | 0.579 ns | 0.542 ns |  2.08 |  VII |

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
|    'DayNumber     ' | 1.154 μs | 0.0037 μs | 0.0034 μs |  1.02 |    I |
|     'DateTime *   ' | 1.158 μs | 0.0015 μs | 0.0013 μs |  1.03 |    I |
|  'OrdinalDate  (O)' | 1.160 μs | 0.0011 μs | 0.0009 μs |  1.03 |    I |
| 'CalendarDate  (Y)' | 1.187 μs | 0.0167 μs | 0.0140 μs |  1.05 |   II |
|    'LocalDate *(Y)' | 1.441 μs | 0.0012 μs | 0.0011 μs |  1.28 |  III |
 */
//
// * = external, Y = Y/M/D repr., O = ord. repr.

public class UtcTodayBenchmark
{
    [Benchmark(Description = "SystemClock     ", Baseline = true)]
    public (int, int, int, DayOfWeek) WithSystemClock()
    {
        var clock = ZorglubSystemClock.Utc;
        DayNumber today = clock.Today();
        var (y, m, d) = today.GetGregorianParts();

        return (y, m, d, today.DayOfWeek);
    }

    [Benchmark(Description = "DayNumber     ")]
    public (int, int, int, DayOfWeek) WithDayNumber()
    {
        DayNumber today = DayNumber.UtcToday();
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

    [Benchmark(Description = "CalendarDate  (Y)")]
    public (int, int, int, DayOfWeek) WithCalendarDate()
    {
        var clock = SimpleCalendar.Civil.UtcClock;
        CalendarDate today = clock.GetCurrentDate();
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

    [Benchmark(Description = "OrdinalDate  (O)")]
    public (int, int, int, DayOfWeek) WithOrdinalDate()
    {
        var clock = SimpleCalendar.Civil.UtcClock;
        OrdinalDate today = clock.GetCurrentOrdinal();
        (int y, int m, int d) = today;

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
