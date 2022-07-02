// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Other;

using NodaTime;
using NodaTime.Extensions;

using Zorglub.Time;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Simple;

// REVIEW: what makes LocalDate faster? why is DateTime slower?

/*
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
|     'WideDate  (Y)' | 1.543 μs | 0.0008 μs | 0.0007 μs |  1.01 |    I |
|    'DayNumber     ' | 1.545 μs | 0.0010 μs | 0.0009 μs |  1.01 |    I |
|     'DateTime *   ' | 1.600 μs | 0.0131 μs | 0.0123 μs |  1.05 |   II |
 */
//
// * = external, Y = Y/M/D repr., O = ord. repr.

public class TodayBenchmark
{
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

    [Benchmark(Description = "WideDate0 (Y)")]
    public (int, int, int, DayOfWeek) WithWideDate0()
    {
        WideDate0 today = WideCalendar.Gregorian.Today();
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
