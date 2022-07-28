// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Other;

using NodaTime;
using NodaTime.Extensions;

using Zorglub.Time;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Simple;
using Zorglub.Time.Specialized;

// REVIEW: what makes LocalDate faster? why is DateTime slower?

/*
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1826 (21H2)
Intel Core i7-4500U CPU 1.80GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.302
  [Host]     : .NET 6.0.7 (6.0.722.32202), X64 RyuJIT
  DefaultJob : .NET 6.0.7 (6.0.722.32202), X64 RyuJIT

|              Method |     Mean |   Error |  StdDev | Ratio | Rank |
|-------------------- |---------:|--------:|--------:|------:|-----:|
|    'CivilDate     ' | 162.8 ns | 0.84 ns | 0.78 ns |  0.85 |    I |
|    'LocalDate *(Y)' | 170.1 ns | 0.85 ns | 0.79 ns |  0.89 |   II |
|    'DayNumber     ' | 174.8 ns | 1.05 ns | 0.93 ns |  0.92 |  III |
|        'ZDate     ' | 179.8 ns | 0.49 ns | 0.43 ns |  0.94 |   IV |
| 'CalendarDate  (Y)' | 179.9 ns | 0.68 ns | 0.64 ns |  0.94 |   IV |
|     'DateTime *   ' | 182.2 ns | 1.32 ns | 1.24 ns |  0.96 |   IV |
|  'OrdinalDate  (O)' | 184.2 ns | 0.74 ns | 0.69 ns |  0.97 |   IV |
|  'CalendarDay     ' | 190.7 ns | 1.60 ns | 1.25 ns |  1.00 |    V |

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1348 (20H2/October2020Update)
Intel Core2 Duo CPU E8500 3.16GHz, 1 CPU, 2 logical and 2 physical cores
.NET SDK=6.0.100
  [Host]     : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
  DefaultJob : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT

|              Method |     Mean |     Error |    StdDev | Ratio | Rank |
|-------------------- |---------:|----------:|----------:|------:|-----:|
|    'LocalDate *(Y)' | 1.504 μs | 0.0012 μs | 0.0010 μs |  0.99 |    I |
|  'CalendarDay     ' | 1.524 μs | 0.0076 μs | 0.0071 μs |  1.00 |    I |
| 'CalendarDate  (Y)' | 1.535 μs | 0.0029 μs | 0.0027 μs |  1.01 |    I |
|  'OrdinalDate  (O)' | 1.540 μs | 0.0015 μs | 0.0014 μs |  1.01 |    I |
|        'ZDate  (Y)' | 1.543 μs | 0.0008 μs | 0.0007 μs |  1.01 |    I |
|    'DayNumber     ' | 1.545 μs | 0.0010 μs | 0.0009 μs |  1.01 |    I |
|     'DateTime *   ' | 1.600 μs | 0.0131 μs | 0.0123 μs |  1.05 |   II |
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
        var (y, m, d) = My.NakedGregorian.GetDateParts(today);

        return (y, m, d, today.DayOfWeek);
    }

    [Benchmark(Description = "ZDate     ")]
    public (int, int, int, DayOfWeek) WithZDate()
    {
        ZDate today = ZCalendar.Gregorian.Today();
        var (y, m, d) = today;

        return (y, m, d, today.DayOfWeek);
    }

    [Benchmark(Description = "LocalDate *(Y)")]
    public (int, int, int, NodaTime.IsoDayOfWeek) WithLocalDate()
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
