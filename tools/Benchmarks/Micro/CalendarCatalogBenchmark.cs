// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Micro;

using Zorglub.Time;
using Zorglub.Time.Simple;

/*
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1586 (21H2)
Intel Core i7-4500U CPU 1.80GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.201
  [Host]     : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
  DefaultJob : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT

|               Method |       Mean |     Error |    StdDev | Ratio | RatioSD | Rank | Code Size |
|--------------------- |-----------:|----------:|----------:|------:|--------:|-----:|----------:|
|    GetCalendarUnsafe |  0.3455 ns | 0.0235 ns | 0.0208 ns |  0.95 |    0.10 |    I |      25 B |
| GetCalendarUnchecked |  0.3694 ns | 0.0274 ns | 0.0256 ns |  1.00 |    0.00 |   II |      42 B |
|    GetSystemCalendar |  2.3740 ns | 0.0523 ns | 0.0489 ns |  6.46 |    0.49 |  III |     117 B |
|          GetCalendar | 25.0066 ns | 0.1987 ns | 0.1859 ns | 68.02 |    5.04 |   IV |     151 B |

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1586 (20H2/October2020Update)
Intel Core2 Duo CPU E8500 3.16GHz, 1 CPU, 2 logical and 2 physical cores
.NET SDK=6.0.201
  [Host]     : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
  DefaultJob : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT

|               Method |       Mean |     Error |    StdDev | Ratio | RatioSD | Rank | Code Size |
|--------------------- |-----------:|----------:|----------:|------:|--------:|-----:|----------:|
|    GetCalendarUnsafe |  0.3171 ns | 0.0031 ns | 0.0029 ns |  0.50 |    0.01 |    I |      25 B |
| GetCalendarUnchecked |  0.6360 ns | 0.0036 ns | 0.0030 ns |  1.00 |    0.00 |   II |      42 B |
|    GetSystemCalendar |  2.5310 ns | 0.0026 ns | 0.0021 ns |  3.98 |    0.02 |  III |     117 B |
|          GetCalendar | 40.2644 ns | 0.0564 ns | 0.0528 ns | 63.32 |    0.30 |   IV |     151 B |
 */

[DisassemblyDiagnoser]
public class CalendarCatalogBenchmark : BenchmarkBase
{
    private const int Id = (int)CalendarId.Gregorian;

    [Benchmark]
    public string GetCalendar()
    {
        var chr = CalendarCatalog.GetCalendar("Gregorian");
        return chr.Key;
    }

    [Benchmark]
    public string GetSystemCalendar()
    {
        var chr = CalendarCatalog.GetSystemCalendar(CalendarId.Gregorian);
        return chr.Key;
    }

    [Benchmark]
    public string GetCalendarUnsafe()
    {
        ref readonly SimpleCalendar chr = ref CalendarCatalog.GetCalendarUnsafe(Id);
        return chr.Key;
    }

    [Benchmark(Baseline = true)]
    public string GetCalendarUnchecked()
    {
        var chr = CalendarCatalog.GetCalendarUnchecked(Id);
        return chr.Key;
    }
}
