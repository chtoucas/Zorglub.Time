﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Other;

using NodaTime;

using Zorglub.Time;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Simple;

/*
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1889 (21H2)
Intel Core i7-4500U CPU 1.80GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.400
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  DefaultJob : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT

|              Method |     Mean |    Error |   StdDev | Ratio | Rank |
|-------------------- |---------:|---------:|---------:|------:|-----:|
|        'Naked     ' | 37.30 ns | 0.168 ns | 0.157 ns |  1.00 |    I |
|     'DateTime *   ' | 38.59 ns | 0.072 ns | 0.056 ns |  1.03 |   II |
|  'CalendarDay     ' | 43.20 ns | 0.227 ns | 0.201 ns |  1.16 |  III |
| 'CalendarDate  (Y)' | 45.25 ns | 0.420 ns | 0.393 ns |  1.21 |   IV |
|    'LocalDate *(Y)' | 57.30 ns | 0.266 ns | 0.249 ns |  1.54 |    V |
|  'OrdinalDate  (O)' | 58.59 ns | 0.293 ns | 0.274 ns |  1.57 |   VI |
|        'ZDate  (Y)' | 66.87 ns | 0.228 ns | 0.190 ns |  1.79 |  VII |

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1348 (20H2/October2020Update)
Intel Core2 Duo CPU E8500 3.16GHz, 1 CPU, 2 logical and 2 physical cores
.NET SDK=6.0.100
  [Host]     : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
  DefaultJob : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT

|              Method |     Mean |    Error |   StdDev | Ratio | Rank |
|-------------------- |---------:|---------:|---------:|------:|-----:|
| 'CalendarDate  (Y)' | 45.51 ns | 0.056 ns | 0.047 ns |  0.96 |    I |
|  'CalendarDay     ' | 47.51 ns | 0.025 ns | 0.024 ns |  1.00 |   II |
|        'Naked     ' | 49.71 ns | 0.024 ns | 0.023 ns |  1.05 |  III |
|     'DateTime *   ' | 58.35 ns | 0.099 ns | 0.082 ns |  1.23 |   IV |
|  'OrdinalDate  (O)' | 59.51 ns | 0.053 ns | 0.050 ns |  1.25 |    V |
|        'ZDate  (Y)' | 62.27 ns | 0.045 ns | 0.042 ns |  1.31 |   VI |
|    'LocalDate *(Y)' | 68.44 ns | 0.062 ns | 0.055 ns |  1.44 |  VII |
 */
//
// * = external, Y = Y/M/D repr., O = ord. repr.

public class InterconversionBenchmark : BenchmarkBase
{
    [Benchmark(Description = "CalendarDate  (Y)")]
    public (int, int, int) WithCalendarDate()
    {
        CalendarDate start = SimpleCalendar.Julian.GetCalendarDate(Year, Month, Day);
        var (y, m, d) = start.WithCalendar(SimpleCalendar.Civil);
        return (y, m, d);
    }

    [Benchmark(Description = "CalendarDay     ")]
    public (int, int, int) WithCalendarDay()
    {
        CalendarDay start = SimpleCalendar.Julian.GetCalendarDate(Year, Month, Day).ToCalendarDay();
        var (y, m, d) = start.WithCalendar(SimpleCalendar.Civil);
        return (y, m, d);
    }

    [Benchmark(Description = "OrdinalDate  (O)")]
    public (int, int, int) WithOrdinalDate()
    {
        OrdinalDate start = SimpleCalendar.Julian.GetCalendarDate(Year, Month, Day).ToOrdinalDate();
        (int y, int m, int d) = start.WithCalendar(SimpleCalendar.Civil);
        return (y, m, d);
    }

    [Benchmark(Description = "Naked     ", Baseline = true)]
    public (int, int, int) WithNakedCivil()
    {
        // This is the closest comparable to DateTime.
        DayNumber start = My.NakedJulian.GetDayNumber(Year, Month, Day);
        var (y, m, d) = My.NakedCivil.GetDateParts(start);
        return (y, m, d);
    }

    [Benchmark(Description = "ZDate  (Y)")]
    public (int, int, int) WithZDate()
    {
        ZDate start = ZCalendar.Julian.GetDate(Year, Month, Day);
        var (y, m, d) = start.WithCalendar(ZCalendar.Civil);
        return (y, m, d);
    }

    [Benchmark(Description = "LocalDate *(Y)")]
    public (int, int, int) WithLocalDate()
    {
        LocalDate start = new(Year, Month, Day, CalendarSystem.Julian);
        var (y, m, d) = start.WithCalendar(CalendarSystem.Gregorian);
        return (y, m, d);
    }

    [Benchmark(Description = "DateTime *   ")]
    public (int, int, int) WithDateTime()
    {
        DateTime start = new(Year, Month, Day);
        var target = new System.Globalization.JulianCalendar();
        int y = target.GetYear(start);
        int m = target.GetMonth(start);
        int d = target.GetDayOfMonth(start);
        return (y, m, d);
    }
}
