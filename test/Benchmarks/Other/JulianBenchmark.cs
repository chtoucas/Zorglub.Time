// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Other;

using NodaTime;

using Zorglub.Time;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Simple;
using Zorglub.Time.Specialized;

/*
BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.2006/21H2/November2021Update)
Intel Core i7-4500U CPU 1.80GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.401
  [Host]     : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2

|              Method |      Mean |    Error |   StdDev | Ratio | RatioSD | Rank |
|-------------------- |----------:|---------:|---------:|------:|--------:|-----:|
|    'DayNumber     ' |  39.52 ns | 0.282 ns | 0.264 ns |  1.00 |    0.00 |    I |
|   'JulianDate     ' |  45.80 ns | 0.343 ns | 0.321 ns |  1.16 |    0.01 |   II |
|  'CalendarDay     ' |  65.04 ns | 0.254 ns | 0.212 ns |  1.65 |    0.01 |  III |
|        'ZDate  (Y)' |  76.95 ns | 0.351 ns | 0.328 ns |  1.95 |    0.02 |   IV |
|     'DateTime *   ' |  81.13 ns | 0.093 ns | 0.078 ns |  2.05 |    0.01 |    V |
|  'OrdinalDate  (O)' |  83.96 ns | 0.397 ns | 0.371 ns |  2.12 |    0.02 |   VI |
| 'CalendarDate  (Y)' | 103.60 ns | 0.376 ns | 0.314 ns |  2.62 |    0.02 |  VII |
|    'LocalDate *(Y)' | 137.29 ns | 0.510 ns | 0.426 ns |  3.48 |    0.02 | VIII |

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1586 (20H2/October2020Update)
Intel Core2 Duo CPU E8500 3.16GHz, 1 CPU, 2 logical and 2 physical cores
.NET SDK=6.0.201
  [Host]     : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
  DefaultJob : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT

|              Method |      Mean |    Error |   StdDev | Ratio | RatioSD | Rank |
|-------------------- |----------:|---------:|---------:|------:|--------:|-----:|
|    'DayNumber     ' |  51.58 ns | 0.026 ns | 0.023 ns |  1.00 |    0.00 |    I |
|  'CalendarDay     ' |  74.53 ns | 0.067 ns | 0.056 ns |  1.45 |    0.00 |   II |
|  'OrdinalDate  (O)' | 104.87 ns | 0.065 ns | 0.057 ns |  2.03 |    0.00 |  III |
| 'CalendarDate  (Y)' | 128.73 ns | 1.702 ns | 1.592 ns |  2.50 |    0.03 |   IV |
|        'ZDate  (Y)' | 144.60 ns | 0.207 ns | 0.183 ns |  2.80 |    0.00 |    V |
|     'DateTime *   ' | 146.93 ns | 0.171 ns | 0.151 ns |  2.85 |    0.00 |   VI |
|    'LocalDate *(Y)' | 161.88 ns | 0.096 ns | 0.090 ns |  3.14 |    0.00 |  VII |
 */
//
// * = external, Y = Y/M/D repr., O = ord. repr.

public class JulianBenchmark : GJBenchmarkBase
{
    private const int D7 = 7;        // No change of month.
    private const int D30 = 30;      // Change of month.
    private const int D401 = 401;    // "Slow-track".

    // Do NOT change Option. The values for Year/Month/Day are Gregorian but
    // they are compatible with the Julian calendar as long as we don't use
    // DaysSinceEpoch.
    public JulianBenchmark() { Option = BenchmarkOption.Fixed; }

    [Benchmark(Description = "CalendarDate  (Y)")]
    public void WithCalendarDate()
    {
        CalendarDate start = SimpleCalendar.Julian.GetDate(Year, Month, Day);
        CalendarDate end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "CalendarDay     ")]
    public void WithCalendarDay()
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

    [Benchmark(Description = "OrdinalDate  (O)")]
    public void WithOrdinalDate()
    {
        OrdinalDate start = SimpleCalendar.Julian.GetDate(Year, Month, Day).ToOrdinalDate();
        OrdinalDate end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "DayNumber     ", Baseline = true)]
    public void WithDayNumber()
    {
        DayNumber start = DayNumber.FromJulianParts(Year, Month, Day);
        DayNumber end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);
        var parts = end.GetJulianParts();
        var oparts = end.GetJulianOrdinalParts();

        var (y, m, d) = parts;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = oparts.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "JulianDate     ")]
    public void WithJulianDate()
    {
        JulianDate start = new(Year, Month, Day);
        JulianDate end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "ZDate  (Y)")]
    public void WithZDate()
    {
        ZDate start = ZCalendar.Julian.GetDate(Year, Month, Day);
        ZDate end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "LocalDate *(Y)")]
    public void WithLocalDate()
    {
        LocalDate start = new(Year, Month, Day, CalendarSystem.Julian);
        LocalDate end = start.PlusDays(1).PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        IsoDayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "DateTime *   ")]
    public void WithDateTime()
    {
        DateTime start = new(Year, Month, Day, new System.Globalization.JulianCalendar());
        DateTime end = start.AddDays(1).AddDays(D7).AddDays(D30).AddDays(D401);

        int y = end.Year;
        int m = end.Month;
        int d = end.Day;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }
}
