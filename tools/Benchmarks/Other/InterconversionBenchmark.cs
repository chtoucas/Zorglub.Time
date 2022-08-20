// SPDX-License-Identifier: BSD-3-Clause
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
|        'ZDate     ' | 66.87 ns | 0.228 ns | 0.190 ns |  1.79 |  VII |

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1889 (21H2)
Intel Core2 Duo CPU E8500 3.16GHz, 1 CPU, 2 logical and 2 physical cores
.NET SDK=6.0.400
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  DefaultJob : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT

|              Method |     Mean |    Error |   StdDev | Ratio | Rank |
|-------------------- |---------:|---------:|---------:|------:|-----:|
|        'Naked     ' | 57.22 ns | 0.421 ns | 0.394 ns |  1.00 |    I |
| 'CalendarDate  (Y)' | 60.23 ns | 0.073 ns | 0.068 ns |  1.05 |   II |
|  'CalendarDay     ' | 63.09 ns | 0.066 ns | 0.061 ns |  1.10 |  III |
|    'LocalDate *(Y)' | 64.92 ns | 0.032 ns | 0.027 ns |  1.13 |   IV |
|     'DateTime *   ' | 68.27 ns | 0.200 ns | 0.177 ns |  1.19 |    V |
|  'OrdinalDate  (O)' | 72.11 ns | 0.247 ns | 0.231 ns |  1.26 |   VI |
|        'ZDate     ' | 74.76 ns | 0.062 ns | 0.058 ns |  1.31 |  VII |
 */
//
// * = external, Y = Y/M/D repr., O = ord. repr.

public class InterconversionBenchmark : GJBenchmarkBase
{
    [Benchmark(Description = "CalendarDate  (Y)")]
    public (int, int, int) WithCalendarDate()
    {
        CalendarDate start = SimpleCalendar.Civil.GetDate(Year, Month, Day);
        var (y, m, d) = start.WithCalendar(SimpleCalendar.Julian);
        return (y, m, d);
    }

    [Benchmark(Description = "CalendarDay     ")]
    public (int, int, int) WithCalendarDay()
    {
        CalendarDay start = SimpleCalendar.Civil.GetDate(Year, Month, Day).ToCalendarDay();
        var (y, m, d) = start.WithCalendar(SimpleCalendar.Julian);
        return (y, m, d);
    }

    [Benchmark(Description = "OrdinalDate  (O)")]
    public (int, int, int) WithOrdinalDate()
    {
        OrdinalDate start = SimpleCalendar.Civil.GetDate(Year, Month, Day).ToOrdinalDate();
        (int y, int m, int d) = start.WithCalendar(SimpleCalendar.Julian);
        return (y, m, d);
    }

    [Benchmark(Description = "Naked     ", Baseline = true)]
    public (int, int, int) WithNaked()
    {
        // This is the closest comparable to DateTime.
        DayNumber start = My.NakedCivil.GetDayNumber(Year, Month, Day);
        var (y, m, d) = My.NakedJulian.GetDateParts(start);
        return (y, m, d);
    }

    [Benchmark(Description = "ZDate     ")]
    public (int, int, int) WithZDate()
    {
        ZDate start = ZCalendar.Civil.GetDate(Year, Month, Day);
        var (y, m, d) = start.WithCalendar(ZCalendar.Julian);
        return (y, m, d);
    }

    [Benchmark(Description = "LocalDate *(Y)")]
    public (int, int, int) WithLocalDate()
    {
        LocalDate start = new(Year, Month, Day, CalendarSystem.Gregorian);
        var (y, m, d) = start.WithCalendar(CalendarSystem.Julian);
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
