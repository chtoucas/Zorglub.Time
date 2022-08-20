// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Other;

using Zorglub.Time.Simple;

// REVIEW(perf): why is Tropicália slower? Civil vs Gregorian?

/*
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1889 (21H2)
Intel Core i7-4500U CPU 1.80GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.400
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  DefaultJob : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT

|                   Method |     Mean |    Error |   StdDev | Ratio | Rank |
|------------------------- |---------:|---------:|---------:|------:|-----:|
|    'Armenian (Egyptian)' | 44.20 ns | 0.205 ns | 0.192 ns |  0.79 |    I |
| 'Zoroastrian (Egyptian)' | 46.26 ns | 0.144 ns | 0.135 ns |  0.82 |   II |
|                   Julian | 50.54 ns | 0.234 ns | 0.207 ns |  0.90 |  III |
|                   Coptic | 52.97 ns | 0.465 ns | 0.412 ns |  0.94 |   IV |
|      'Ethiopic (Coptic)' | 53.64 ns | 0.304 ns | 0.285 ns |  0.96 |   IV |
|       'Tropicália 31-30' | 53.89 ns | 0.159 ns | 0.141 ns |  0.96 |   IV |
|       'Tropicália 30-31' | 53.96 ns | 0.268 ns | 0.238 ns |  0.96 |   IV |
|                    Civil | 55.81 ns | 0.156 ns | 0.131 ns |  1.00 |    V |
|                Gregorian | 56.13 ns | 0.275 ns | 0.257 ns |  1.00 |    V |
|           TabularIslamic | 60.68 ns | 0.388 ns | 0.362 ns |  1.08 |   VI |
|               Tropicália | 62.29 ns | 0.314 ns | 0.294 ns |  1.11 |  VII |

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1889 (21H2)
Intel Core2 Duo CPU E8500 3.16GHz, 1 CPU, 2 logical and 2 physical cores
.NET SDK=6.0.400
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  DefaultJob : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT

|                   Method |     Mean |    Error |   StdDev | Ratio | Rank |
|------------------------- |---------:|---------:|---------:|------:|-----:|
| 'Zoroastrian (Egyptian)' | 58.44 ns | 0.036 ns | 0.034 ns |  0.74 |    I |
|    'Armenian (Egyptian)' | 60.68 ns | 0.153 ns | 0.144 ns |  0.77 |   II |
|                   Coptic | 67.60 ns | 0.242 ns | 0.226 ns |  0.86 |  III |
|      'Ethiopic (Coptic)' | 68.70 ns | 0.239 ns | 0.223 ns |  0.87 |   IV |
|       'Tropicália 30-31' | 69.82 ns | 0.118 ns | 0.110 ns |  0.89 |    V |
|                   Julian | 70.35 ns | 0.076 ns | 0.071 ns |  0.89 |    V |
|       'Tropicália 31-30' | 71.00 ns | 0.065 ns | 0.060 ns |  0.90 |    V |
|                Gregorian | 78.83 ns | 0.119 ns | 0.111 ns |  1.00 |   VI |
|                    Civil | 79.19 ns | 0.050 ns | 0.042 ns |  1.00 |   VI |
|           TabularIslamic | 79.54 ns | 0.114 ns | 0.107 ns |  1.01 |   VI |
|               Tropicália | 82.67 ns | 0.119 ns | 0.112 ns |  1.05 |  VII |
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
        CalendarDay start = SimpleCalendar.Gregorian.GetDate(Year, Month, Day).ToCalendarDay();
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

    [Benchmark(Description = "Armenian (Egyptian)")]
    public void Armenian()
    {
        CalendarDay start = SimpleCalendar.Armenian.GetDate(Year, Month, Day).ToCalendarDay();
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
        CalendarDay start = SimpleCalendar.Civil.GetDate(Year, Month, Day).ToCalendarDay();
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
        CalendarDay start = SimpleCalendar.Coptic.GetDate(Year, Month, Day).ToCalendarDay();
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

    [Benchmark(Description = "Ethiopic (Coptic)")]
    public void Ethiopic()
    {
        CalendarDay start = SimpleCalendar.Ethiopic.GetDate(Year, Month, Day).ToCalendarDay();
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
        CalendarDay start = SimpleCalendar.Julian.GetDate(Year, Month, Day).ToCalendarDay();
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
        CalendarDay start = SimpleCalendar.TabularIslamic.GetDate(Year, Month, Day).ToCalendarDay();
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

    [Benchmark(Description = "Zoroastrian (Egyptian)")]
    public void Zoroastrian()
    {
        CalendarDay start = SimpleCalendar.Zoroastrian.GetDate(Year, Month, Day).ToCalendarDay();
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
        CalendarDay start = My.Tropicalia.GetDate(Year, Month, Day).ToCalendarDay();
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
        CalendarDay start = My.Tropicalia3031.GetDate(Year, Month, Day).ToCalendarDay();
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
        CalendarDay start = My.Tropicalia3130.GetDate(Year, Month, Day).ToCalendarDay();
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
