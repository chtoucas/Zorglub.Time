// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Other;

using Zorglub.Time.Simple;

/*
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1889 (21H2)
Intel Core i7-4500U CPU 1.80GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.400
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  DefaultJob : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT

|             Method |     Mean |    Error |   StdDev | Ratio | Rank |
|------------------- |---------:|---------:|---------:|------:|-----:|
|          Gregorian | 58.96 ns | 0.354 ns | 0.314 ns |  1.00 |    I |
| 'Tropicália 31-30' | 66.29 ns | 0.336 ns | 0.280 ns |  1.13 |   II |
| 'Tropicália 30-31' | 66.53 ns | 0.195 ns | 0.173 ns |  1.13 |   II |
|         Tropicália | 71.03 ns | 0.300 ns | 0.281 ns |  1.20 |  III |

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

public class CalendarDayBenchmark : BenchmarkBase
{
    // The values should be compatible with all calendars.
    private const int Year = 5;
    private const int Month = 6;
    private const int Day = 7;

    private const int D7 = 7;        // No change of month.
    private const int D30 = 30;      // Change of month.
    private const int D401 = 401;    // "Slow-track".

    [Benchmark(Baseline = true)]
    public void Gregorian()
    {
        CalendarDay start = new CalendarDate(Year, Month, Day).ToCalendarDay();
        CalendarDay end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark]
    public void Armenian()
    {
        CalendarDay start = SimpleCalendar.Armenian.GetCalendarDate(Year, Month, Day).ToCalendarDay();
        CalendarDay end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark]
    public void Civil()
    {
        CalendarDay start = SimpleCalendar.Civil.GetCalendarDate(Year, Month, Day).ToCalendarDay();
        CalendarDay end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark]
    public void Coptic()
    {
        CalendarDay start = SimpleCalendar.Coptic.GetCalendarDate(Year, Month, Day).ToCalendarDay();
        CalendarDay end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark]
    public void Ethiopic()
    {
        CalendarDay start = SimpleCalendar.Ethiopic.GetCalendarDate(Year, Month, Day).ToCalendarDay();
        CalendarDay end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark]
    public void Julian()
    {
        CalendarDay start = SimpleCalendar.Julian.GetCalendarDate(Year, Month, Day).ToCalendarDay();
        CalendarDay end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark]
    public void TabularIslamic()
    {
        CalendarDay start = SimpleCalendar.TabularIslamic.GetCalendarDate(Year, Month, Day).ToCalendarDay();
        CalendarDay end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    //
    // User-defined calendars
    //

    [Benchmark(Description = "Tropicália")]
    public void Tropicalia()
    {
        CalendarDay start = My.Tropicalia.GetCalendarDate(Year, Month, Day).ToCalendarDay();
        CalendarDay end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

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
        CalendarDay start = My.Tropicalia3031.GetCalendarDate(Year, Month, Day).ToCalendarDay();
        CalendarDay end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

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
        CalendarDay start = My.Tropicalia3130.GetCalendarDate(Year, Month, Day).ToCalendarDay();
        CalendarDay end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

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
