﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Other;

using NodaTime;

using Samples;

using Zorglub.Bulgroz;
using Zorglub.Time;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Simple;
using Zorglub.Time.Specialized;

/*
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1889 (21H2)
Intel Core i7-4500U CPU 1.80GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.400
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  DefaultJob : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT

|                 Method |      Mean |    Error |   StdDev | Ratio | RatioSD | Rank |
|----------------------- |----------:|---------:|---------:|------:|--------:|-----:|
|     'CivilDate   (g)+' |  36.75 ns | 0.714 ns | 0.633 ns |  1.00 |    0.00 |    I |
|   'DayNumber64   (g) ' |  42.45 ns | 0.165 ns | 0.147 ns |  1.16 |    0.02 |   II |
|     'DayNumber   (g) ' |  46.99 ns | 0.153 ns | 0.143 ns |  1.28 |    0.02 |  III |
| 'GregorianDate   (g) ' |  47.75 ns | 0.214 ns | 0.190 ns |  1.30 |    0.02 |   IV |
|   'MyCivilDate      +' |  51.09 ns | 0.222 ns | 0.196 ns |  1.39 |    0.03 |    V |
|      'DateTime *    +' |  51.21 ns | 0.195 ns | 0.182 ns |  1.39 |    0.02 |    V |
|      'DateOnly *    +' |  53.06 ns | 0.140 ns | 0.124 ns |  1.44 |    0.02 |   VI |
|   'CalendarDay       ' |  58.89 ns | 0.260 ns | 0.244 ns |  1.60 |    0.03 |  VII |
|   'Naked Civil      +' |  61.85 ns | 0.461 ns | 0.431 ns |  1.68 |    0.03 | VIII |
|         'ZDate       ' |  62.77 ns | 0.209 ns | 0.196 ns |  1.71 |    0.03 |   IX |
|    'XCivilDate  (Yg)+' |  64.09 ns | 0.256 ns | 0.213 ns |  1.74 |    0.03 |    X |
|   'OrdinalDate  (O)  ' |  77.90 ns | 0.328 ns | 0.307 ns |  2.12 |    0.04 |   XI |
|   'CivilTriple  (Y) +' |  85.18 ns | 1.532 ns | 1.433 ns |  2.32 |    0.05 |  XII |
|  'CalendarDate  (Y)  ' |  88.63 ns | 0.405 ns | 0.379 ns |  2.41 |    0.04 | XIII |
|        'MyDate  (Y) +' |  88.96 ns | 0.501 ns | 0.469 ns |  2.42 |    0.04 | XIII |
|    'CivilParts  (Y) +' |  99.10 ns | 0.640 ns | 0.567 ns |  2.70 |    0.05 |  XIV |
|     'LocalDate *(Y)  ' | 115.67 ns | 0.561 ns | 0.438 ns |  3.14 |    0.05 |   XV |

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
|     'MyCivilDate      +' |  66.16 ns | 0.415 ns | 0.388 ns |  1.18 |    0.01 |    V |
|       'DayNumber   (g) ' |  66.63 ns | 0.026 ns | 0.020 ns |  1.19 |    0.00 |    V |
|     'CalendarDay       ' |  85.37 ns | 0.151 ns | 0.142 ns |  1.52 |    0.00 |   VI |
|      'XCivilDate  (Yg)+' |  86.92 ns | 0.172 ns | 0.161 ns |  1.55 |    0.00 |  VII |
|     'Naked Civil      +' |  87.12 ns | 0.075 ns | 0.070 ns |  1.55 |    0.00 |  VII |
|           'ZDate       ' |  91.32 ns | 0.631 ns | 0.590 ns |  1.63 |    0.01 | VIII |
|     'OrdinalDate  (O)  ' | 104.29 ns | 0.079 ns | 0.074 ns |  1.86 |    0.00 |   IX |
|     'CivilTriple  (Y) +' | 105.15 ns | 0.130 ns | 0.122 ns |  1.88 |    0.00 |   IX |
|          'MyDate  (Y) +' | 107.84 ns | 0.136 ns | 0.127 ns |  1.92 |    0.00 |    X |
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
// - CivilDate, MyCivilDate, MyDate, DateOnly and DateTime only deal with
//   positive years (faster divisions).
// - LocalDate caches the start of the year.
// - DateTime is a time object, not a date object.
// - With DayNumber and DayNumber64, we only validate the result twice (at
//   the start and at the end) to mimic the behaviour of other date types.

public class GregorianBenchmark : GJBenchmarkBase
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

    [Benchmark(Description = "Naked Civil      +")]
    public void WithNakedCivil()
    {
        var chr = My.NakedCivil;
        DayNumber start = chr.GetDayNumber(Year, Month, Day);
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
        var dayNumber = DayZero.NewStyle + end.DaysSinceEpoch;

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
        var dayNumber = DayZero.NewStyle + end.DaysSinceEpoch;

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

    [Benchmark(Description = "XCivilDate  (Yg)+")]
    public void WithXCivilDate()
    {
        XCivilDate start = new(Year, Month, Day);
        XCivilDate end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

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

    [Benchmark(Description = "MyDate  (Y) +")]
    public void WithMyDate()
    {
        MyDate start = new(Year, Month, Day);
        MyDate end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

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

    [Benchmark(Description = "MyCivilDate      +")]
    public void WithMyCivilDate()
    {
        MyCivilDate start = new(Year, Month, Day);
        MyCivilDate end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

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
        IsoDayOfWeek dayOfWeek = end.DayOfWeek;
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