// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Other;

using NodaTime;
using NodaTime.Extensions;

using Zorglub.Time;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Simple;
using Zorglub.Time.Specialized;

// REVIEW(perf): what makes LocalDate faster? why is DateTime slower?

/*
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1826 (21H2)
Intel Core i7-4500U CPU 1.80GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.302
  [Host]     : .NET 6.0.7 (6.0.722.32202), X64 RyuJIT
  DefaultJob : .NET 6.0.7 (6.0.722.32202), X64 RyuJIT

|              Method |     Mean |   Error |  StdDev | Ratio | Rank |
|-------------------- |---------:|--------:|--------:|------:|-----:|
|    'CivilDate     ' | 163.1 ns | 2.36 ns | 2.32 ns |  0.95 |    I |
|  'CalendarDay     ' | 170.9 ns | 0.86 ns | 0.81 ns |  1.00 |   II |
|    'DayNumber     ' | 172.6 ns | 0.81 ns | 0.76 ns |  1.01 |   II |
|    'LocalDate *(Y)' | 175.1 ns | 0.77 ns | 0.68 ns |  1.02 |  III |
| 'CalendarDate  (Y)' | 176.3 ns | 0.80 ns | 0.75 ns |  1.03 |  III |
|     'DateTime *   ' | 179.8 ns | 0.62 ns | 0.55 ns |  1.05 |   IV |
|        'ZDate     ' | 181.0 ns | 0.53 ns | 0.50 ns |  1.06 |   IV |
|  'OrdinalDate  (O)' | 185.0 ns | 0.52 ns | 0.46 ns |  1.08 |    V |

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1826 (21H2)
Intel Core2 Duo CPU E8500 3.16GHz, 1 CPU, 2 logical and 2 physical cores
.NET SDK=6.0.302
  [Host]     : .NET 6.0.7 (6.0.722.32202), X64 RyuJIT
  DefaultJob : .NET 6.0.7 (6.0.722.32202), X64 RyuJIT

|              Method |     Mean |     Error |    StdDev | Ratio | Rank |
|-------------------- |---------:|----------:|----------:|------:|-----:|
|    'LocalDate *(Y)' | 1.500 μs | 0.0019 μs | 0.0017 μs |  0.97 |    I |
|    'CivilDate     ' | 1.548 μs | 0.0017 μs | 0.0015 μs |  1.00 |   II |
|  'CalendarDay     ' | 1.548 μs | 0.0014 μs | 0.0013 μs |  1.00 |   II |
|    'DayNumber     ' | 1.549 μs | 0.0022 μs | 0.0021 μs |  1.00 |   II |
|        'ZDate     ' | 1.551 μs | 0.0013 μs | 0.0012 μs |  1.00 |   II |
|  'OrdinalDate  (O)' | 1.557 μs | 0.0013 μs | 0.0012 μs |  1.01 |   II |
| 'CalendarDate  (Y)' | 1.559 μs | 0.0081 μs | 0.0076 μs |  1.01 |   II |
|     'DateTime *   ' | 1.626 μs | 0.0049 μs | 0.0046 μs |  1.05 |  III |
 */
//
// * = external, Y = Y/M/D repr., O = ord. repr.

public class TodayBenchmark
{
    [Benchmark(Description = "CivilDate     ")]
    public (int, int, int, DayOfWeek) WithCivilDate()
    {
        CivilDate today = CivilDate.Today();
        var (y, m, d) = today;

        return (y, m, d, today.DayOfWeek);
    }

    [Benchmark(Description = "CalendarDate  (Y)")]
    public (int, int, int, DayOfWeek) WithCalendarDate()
    {
        CalendarDate today = CalendarDate.Today();
        var (y, m, d) = today;

        return (y, m, d, today.DayOfWeek);
    }

    [Benchmark(Description = "CalendarDay     ", Baseline = true)]
    public (int, int, int, DayOfWeek) WithCalendarDay()
    {
        CalendarDay today = CalendarDay.Today();
        var (y, m, d) = today;

        return (y, m, d, today.DayOfWeek);
    }

    [Benchmark(Description = "OrdinalDate  (O)")]
    public (int, int, int, DayOfWeek) WithOrdinalDate()
    {
        OrdinalDate today = OrdinalDate.Today();
        (int y, int m, int d) = today;

        return (y, m, d, today.DayOfWeek);
    }

    [Benchmark(Description = "DayNumber     ")]
    public (int, int, int, DayOfWeek) WithDayNumber()
    {
        DayNumber today = DayNumber.Today();
        var (y, m, d) = today.GetGregorianParts();

        return (y, m, d, today.DayOfWeek);
    }

    [Benchmark(Description = "ZDate     ")]
    public (int, int, int, DayOfWeek) WithZDate()
    {
        ZDate today = ZDate.Today();
        var (y, m, d) = today;

        return (y, m, d, today.DayOfWeek);
    }

    [Benchmark(Description = "LocalDate *(Y)")]
    public (int, int, int, IsoDayOfWeek) WithLocalDate()
    {
        var zonedClock = SystemClock.Instance.InBclSystemDefaultZone();

        LocalDate now = zonedClock.GetCurrentDate();
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
