// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Other;

using Zorglub.Time.Simple;

/*
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1586 (21H2)
Intel Core i7-4500U CPU 1.80GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.201
  [Host]     : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
  DefaultJob : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT

|             Method |      Mean |    Error |   StdDev | Ratio | Rank |
|------------------- |----------:|---------:|---------:|------:|-----:|
| 'Tropicália 31-30' |  95.54 ns | 0.304 ns | 0.270 ns |  1.00 |    I |
|          Gregorian |  95.85 ns | 0.163 ns | 0.136 ns |  1.00 |    I |
| 'Tropicália 30-31' |  99.92 ns | 0.438 ns | 0.410 ns |  1.04 |   II |
|         Tropicália | 106.74 ns | 0.539 ns | 0.478 ns |  1.11 |  III |

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1586 (20H2/October2020Update)
Intel Core2 Duo CPU E8500 3.16GHz, 1 CPU, 2 logical and 2 physical cores
.NET SDK=6.0.201
  [Host]     : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
  DefaultJob : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT

|             Method |     Mean |   Error |  StdDev | Ratio | Rank |
|------------------- |---------:|--------:|--------:|------:|-----:|
| 'Tropicália 30-31' | 113.7 ns | 0.13 ns | 0.11 ns |  0.97 |    I |
| 'Tropicália 31-30' | 114.1 ns | 0.12 ns | 0.11 ns |  0.98 |    I |
|          Gregorian | 116.9 ns | 0.14 ns | 0.13 ns |  1.00 |   II |
|         Tropicália | 128.9 ns | 0.12 ns | 0.10 ns |  1.10 |  III |
 */

public class TropicaliaBenchmark : BenchmarkBase
{
    private const int D7 = 7;        // No change of month.
    private const int D30 = 30;      // Change of month.
    private const int D401 = 401;    // "Slow-track".

    public TropicaliaBenchmark() { Option = BenchmarkOption.Fixed; }

    [Benchmark(Description = "Gregorian", Baseline = true)]
    public void Gregorian()
    {
        var start = new CalendarDate(Year, Month, Day);
        var end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "Tropicália")]
    public void Tropicalia()
    {
        var start = My.Tropicalia.GetCalendarDate(Year, Month, Day);
        var end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "Tropicália 30-31")]
    public void Tropicalia3031()
    {
        var start = My.Tropicalia3031.GetCalendarDate(Year, Month, Day);
        var end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "Tropicália 31-30")]
    public void Tropicalia3130()
    {
        var start = My.Tropicalia3130.GetCalendarDate(Year, Month, Day);
        var end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }
}
