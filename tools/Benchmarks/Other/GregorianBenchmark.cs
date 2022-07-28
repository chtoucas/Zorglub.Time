// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Other;

using NodaTime;

using Samples;

using Zorglub.Time;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Simple;
using Zorglub.Time.Specialized;

/*
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1826 (21H2)
Intel Core i7-4500U CPU 1.80GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.302
  [Host]     : .NET 6.0.7 (6.0.722.32202), X64 RyuJIT
  DefaultJob : .NET 6.0.7 (6.0.722.32202), X64 RyuJIT

|                   Method |      Mean |    Error |   StdDev | Ratio | RatioSD | Rank |
|------------------------- |----------:|---------:|---------:|------:|--------:|-----:|
|       'CivilDate   (g)+' |  37.36 ns | 0.343 ns | 0.321 ns |  1.00 |    0.00 |    I |
|   'GregorianDate   (g) ' |  42.34 ns | 0.207 ns | 0.184 ns |  1.13 |    0.01 |   II |
|     'DayNumber64   (g) ' |  42.82 ns | 0.228 ns | 0.214 ns |  1.15 |    0.01 |   II |
|       'DayNumber   (g) ' |  47.11 ns | 0.182 ns | 0.171 ns |  1.26 |    0.01 |   IV |
|        'DateTime *    +' |  51.15 ns | 0.186 ns | 0.165 ns |  1.37 |    0.01 |    V |
|        'DateOnly *    +' |  52.93 ns | 0.183 ns | 0.171 ns |  1.42 |    0.01 |   VI |
|    'DateTemplate      +' |  53.12 ns | 0.235 ns | 0.220 ns |  1.42 |    0.02 |   VI |
|     'CalendarDay       ' |  61.93 ns | 0.266 ns | 0.236 ns |  1.66 |    0.01 |  VII |
|           'ZDate       ' |  62.39 ns | 0.432 ns | 0.404 ns |  1.67 |    0.02 |  VII |
|  'CivilPrototype  (Yg)+' |  63.98 ns | 0.261 ns | 0.245 ns |  1.71 |    0.01 | VIII |
| 'Naked DayNumber      +' |  67.32 ns | 0.711 ns | 0.594 ns |  1.80 |    0.01 |   IX |
|     'OrdinalDate  (O)  ' |  76.93 ns | 0.336 ns | 0.314 ns |  2.06 |    0.02 |    X |
|     'CivilTriple  (Y) +' |  87.64 ns | 0.341 ns | 0.319 ns |  2.35 |    0.02 |   XI |
|    'CalendarDate  (Y)  ' |  88.94 ns | 0.440 ns | 0.390 ns |  2.38 |    0.02 |  XII |
|      'DateTriple  (Y) +' |  90.27 ns | 0.566 ns | 0.530 ns |  2.42 |    0.03 | XIII |
|      'CivilParts  (Y) +' | 108.27 ns | 0.502 ns | 0.470 ns |  2.90 |    0.02 |  XIV |
|       'LocalDate *(Y)  ' | 117.49 ns | 0.537 ns | 0.502 ns |  3.14 |    0.03 |   XV |

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1826 (21H2)
Intel Core2 Duo CPU E8500 3.16GHz, 1 CPU, 2 logical and 2 physical cores
.NET SDK=6.0.302
  [Host]     : .NET 6.0.7 (6.0.722.32202), X64 RyuJIT
  DefaultJob : .NET 6.0.7 (6.0.722.32202), X64 RyuJIT

|                   Method |      Mean |    Error |   StdDev | Ratio | RatioSD | Rank |
|------------------------- |----------:|---------:|---------:|------:|--------:|-----:|
|       'CivilDate   (g)+' |  56.07 ns | 0.072 ns | 0.067 ns |  1.00 |    0.00 |    I |
|   'GregorianDate   (g) ' |  59.18 ns | 0.049 ns | 0.044 ns |  1.06 |    0.00 |   II |
|     'DayNumber64   (g) ' |  63.40 ns | 0.425 ns | 0.397 ns |  1.13 |    0.01 |  III |
|    'DateTemplate      +' |  66.16 ns | 0.415 ns | 0.388 ns |  1.18 |    0.01 |    V |
|       'DayNumber   (g) ' |  66.63 ns | 0.026 ns | 0.020 ns |  1.19 |    0.00 |    V |
|     'CalendarDay       ' |  85.37 ns | 0.151 ns | 0.142 ns |  1.52 |    0.00 |   VI |
|  'CivilPrototype  (Yg)+' |  86.92 ns | 0.172 ns | 0.161 ns |  1.55 |    0.00 |  VII |
| 'Naked DayNumber      +' |  87.12 ns | 0.075 ns | 0.070 ns |  1.55 |    0.00 |  VII |
|           'ZDate       ' |  91.32 ns | 0.631 ns | 0.590 ns |  1.63 |    0.01 | VIII |
|     'OrdinalDate  (O)  ' | 104.29 ns | 0.079 ns | 0.074 ns |  1.86 |    0.00 |   IX |
|     'CivilTriple  (Y) +' | 105.15 ns | 0.130 ns | 0.122 ns |  1.88 |    0.00 |   IX |
|      'DateTriple  (Y) +' | 107.84 ns | 0.136 ns | 0.127 ns |  1.92 |    0.00 |    X |
|    'CalendarDate  (Y)  ' | 114.11 ns | 0.099 ns | 0.093 ns |  2.04 |    0.00 |   XI |
|        'DateOnly *    +' | 118.96 ns | 2.403 ns | 6.457 ns |  2.12 |    0.13 |  XII |
|        'DateTime *    +' | 126.58 ns | 2.533 ns | 4.503 ns |  2.24 |    0.08 | XIII |
|      'CivilParts  (Y) +' | 132.92 ns | 0.200 ns | 0.187 ns |  2.37 |    0.00 |  XIV |
|       'LocalDate *(Y)  ' | 136.54 ns | 0.117 ns | 0.109 ns |  2.44 |    0.00 |   XV |
 */
//
// * = external, Y = Y/M/D repr., O = ord. repr., g = Gregorian-optimized, + = year > 0
//
// Comments:
// - Here, we do not really test the overall design, we only test the
//   various implementations of a Gregorian calendar which are usually over-
//   optimized. See JulianBenchmark for more realistic benchmarks.
// - This is the worst possible scenario for a date object: arithmetic and
//   DayOfWeek rely on the count of consecutive days since the epoch not on the
//   Y/M/D representation, therefore DayNumber, DateTime and other types are
//   favoured. By the way, these types have predictable performances,
//   their results do not vary depending on the input.
// - CivilDate, CivilPrototype, DateTemplate, DateTriple, DateOnly and DateTime
//   only deal with positive years (faster divisions).
// - LocalDate caches the start of the year.
// - DateTime is a time object, not a date object.
// - With DayNumber and DayNumber64, we only validate the result twice (at
//   the start and at the end) to mimic the behaviour of other date types.

public class GregorianBenchmark : BenchmarkBase
{
    private const int D7 = 7;        // No change of month.
    private const int D30 = 30;      // Change of month.
    private const int D401 = 401;    // "Slow-track".

    public GregorianBenchmark() { Option = BenchmarkOption.Fixed; }

    [Benchmark(Description = "CalendarDate  (Y)  ")]
    public void WithCalendarDate()
    {
        CalendarDate start = new(Year, Month, Day);
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

    [Benchmark(Description = "CalendarDay       ")]
    public void WithCalendarDay()
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

    [Benchmark(Description = "OrdinalDate  (O)  ")]
    public void WithOrdinalDate()
    {
        OrdinalDate start = new CalendarDate(Year, Month, Day).ToOrdinalDate();
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

    [Benchmark(Description = "ZDate       ")]
    public void WithZDate()
    {
        ZDate start = new(Year, Month, Day);
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

    #region DayNumber

    [Benchmark(Description = "DayNumber   (g) ")]
    public void WithDayNumber()
    {
        DayNumber start = DayNumber.FromGregorianParts(Year, Month, Day);
        DayNumber end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);
        var parts = end.GetGregorianParts();
        var oparts = end.GetGregorianOrdinalParts();

        var (y, m, d) = parts;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = oparts.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "DayNumber64   (g) ")]
    public void WithDayNumber64()
    {
        DayNumber64 start = DayNumber64.FromGregorianParts(Year, Month, Day);
        DayNumber64 end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);
        end.GetGregorianParts(out long y, out int m, out int d);
        end.GetGregorianOrdinalParts(out _, out int dayOfYear);

        DayOfWeek dayOfWeek = end.DayOfWeek;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "Naked DayNumber      +")]
    public void WithDayNumberNaked()
    {
        var chr = My.NakedGregorian;
        DayNumber start = chr.GetDayNumberOn(Year, Month, Day);
        DayNumber end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);
        var parts = chr.GetDateParts(end);
        var oparts = chr.GetOrdinalParts(end);

        var (y, m, d) = parts;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = oparts.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    #endregion
    #region Affine date types

    [Benchmark(Description = "CivilParts  (Y) +")]
    public void WithCivilParts()
    {
        CivilParts start = new(Year, Month, Day);
        CivilParts end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);
        var dayNumber = DayZero.NewStyle + end.CountDaysSinceEpoch();

        var (y, m, d) = end;
        DayOfWeek dayOfWeek = dayNumber.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "CivilTriple  (Y) +")]
    public void WithCivilTriple()
    {
        CivilTriple start = new(Year, Month, Day);
        CivilTriple end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);
        var dayNumber = DayZero.NewStyle + end.CountDaysSinceEpoch();

        var (y, m, d) = end;
        DayOfWeek dayOfWeek = dayNumber.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    #endregion
    #region Specialized date types

    [Benchmark(Description = "CivilPrototype  (Yg)+")]
    public void WithCivilPrototype()
    {
        CivilPrototype start = new(Year, Month, Day);
        CivilPrototype end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "CivilDate   (g)+", Baseline = true)]
    public void WithCivilDate()
    {
        CivilDate start = new(Year, Month, Day);
        CivilDate end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "GregorianDate   (g) ")]
    public void WithGregorianDate()
    {
        GregorianDate start = new(Year, Month, Day);
        GregorianDate end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    #endregion
    #region Templates

    [Benchmark(Description = "DateTriple  (Y) +")]
    public void WithDateTriple()
    {
        DateTriple start = new(Year, Month, Day);
        DateTriple end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "DateTemplate      +")]
    public void WithDateTemplate()
    {
        DateTemplate start = new(Year, Month, Day);
        DateTemplate end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    #endregion
    #region External date types

    [Benchmark(Description = "LocalDate *(Y)  ")]
    public void WithLocalDate()
    {
        LocalDate start = new(Year, Month, Day);
        LocalDate end = start.PlusDays(1).PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        NodaTime.IsoDayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "DateOnly *    +")]
    public void WithDateOnly()
    {
        DateOnly start = new(Year, Month, Day);
        DateOnly end = start.AddDays(1).AddDays(D7).AddDays(D30).AddDays(D401);

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

    [Benchmark(Description = "DateTime *    +")]
    public void WithDateTime()
    {
        DateTime start = new(Year, Month, Day);
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

    #endregion
}
