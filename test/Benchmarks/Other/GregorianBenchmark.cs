// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Other;

using NodaTime;

using Zorglub.Bulgroz;
using Zorglub.Samples;
using Zorglub.Time;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Simple;
using Zorglub.Time.Specialized;

/*
BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.4046/22H2/2022Update)
Intel Core i7-4500U CPU 1.80GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET SDK 8.0.200
  [Host]     : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2

| Method                 | Mean     | Error    | StdDev   | Ratio | RatioSD | Rank |
|----------------------- |---------:|---------:|---------:|------:|--------:|-----:|
| 'DateOnly *         +' | 29.82 ns | 0.154 ns | 0.144 ns |  0.86 |    0.01 |    I |
| 'DateTime *         +' | 31.49 ns | 0.141 ns | 0.132 ns |  0.91 |    0.01 |   II |
| 'CivilDate       (g)+' | 34.73 ns | 0.219 ns | 0.205 ns |  1.00 |    0.00 |  III |
| 'DayNumber64     (g) ' | 38.73 ns | 0.119 ns | 0.105 ns |  1.11 |    0.01 |   IV |
| 'MyCivilDate        +' | 38.99 ns | 0.186 ns | 0.174 ns |  1.12 |    0.01 |   IV |
| 'Naked Civil        +' | 40.45 ns | 0.199 ns | 0.186 ns |  1.16 |    0.01 |    V |
| 'GregorianDate   (g) ' | 41.12 ns | 0.275 ns | 0.257 ns |  1.18 |    0.01 |   VI |
| 'DayNumber       (g) ' | 41.58 ns | 0.217 ns | 0.203 ns |  1.20 |    0.01 |   VI |
| 'CalendarDay         ' | 55.79 ns | 0.295 ns | 0.261 ns |  1.61 |    0.01 |  VII |
| 'ZDate               ' | 57.29 ns | 0.295 ns | 0.276 ns |  1.65 |    0.01 | VIII |
| 'OrdinalDate    (O)  ' | 59.51 ns | 0.202 ns | 0.189 ns |  1.71 |    0.01 |   IX |
| 'XCivilDate     (Yg)+' | 62.67 ns | 0.236 ns | 0.221 ns |  1.80 |    0.01 |    X |
| 'CivilTriple    (Y) +' | 71.57 ns | 0.263 ns | 0.246 ns |  2.06 |    0.02 |   XI |
| 'MyDate         (Y) +' | 72.58 ns | 0.328 ns | 0.256 ns |  2.09 |    0.01 |  XII |
| 'CalendarDate   (Y)  ' | 77.77 ns | 0.416 ns | 0.347 ns |  2.24 |    0.01 | XIII |
| 'CivilParts     (Y) +' | 90.48 ns | 0.454 ns | 0.425 ns |  2.61 |    0.02 |  XIV |
| 'LocalDate *    (Y)  ' | 91.42 ns | 0.646 ns | 0.605 ns |  2.63 |    0.03 |  XIV |

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.2251/21H2/November2021Update)
Intel Core2 Duo CPU E8500 3.16GHz, 1 CPU, 2 logical and 2 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT SSE4.1
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT SSE4.1

|                 Method |      Mean |    Error |   StdDev | Ratio | Rank |
|----------------------- |----------:|---------:|---------:|------:|-----:|
|     'CivilDate   (g)+' |  49.97 ns | 0.074 ns | 0.070 ns |  1.00 |    I |
|      'DateTime *    +' |  57.04 ns | 0.041 ns | 0.038 ns |  1.14 |   II |
|      'DateOnly *    +' |  57.20 ns | 0.098 ns | 0.092 ns |  1.14 |   II |
|   'DayNumber64   (g) ' |  57.99 ns | 0.058 ns | 0.048 ns |  1.16 |  III |
|     'DayNumber   (g) ' |  62.03 ns | 0.095 ns | 0.089 ns |  1.24 |   IV |
| 'GregorianDate   (g) ' |  63.00 ns | 0.082 ns | 0.077 ns |  1.26 |    V |
|   'MyCivilDate      +' |  65.47 ns | 0.111 ns | 0.104 ns |  1.31 |   VI |
|   'Naked Civil      +' |  72.93 ns | 0.104 ns | 0.097 ns |  1.46 |  VII |
|   'CalendarDay       ' |  80.61 ns | 0.157 ns | 0.139 ns |  1.61 | VIII |
|    'XCivilDate  (Yg)+' |  83.36 ns | 0.170 ns | 0.159 ns |  1.67 |   IX |
|         'ZDate       ' |  95.62 ns | 0.202 ns | 0.189 ns |  1.91 |    X |
|   'OrdinalDate  (O)  ' |  96.71 ns | 0.072 ns | 0.067 ns |  1.94 |   XI |
|   'CivilTriple  (Y) +' | 109.95 ns | 0.190 ns | 0.158 ns |  2.20 |  XII |
|  'CalendarDate  (Y)  ' | 110.04 ns | 0.213 ns | 0.199 ns |  2.20 |  XII |
|        'MyDate  (Y) +' | 111.06 ns | 0.122 ns | 0.114 ns |  2.22 |  XII |
|     'LocalDate *(Y)  ' | 127.40 ns | 0.214 ns | 0.179 ns |  2.55 | XIII |
|    'CivilParts  (Y) +' | 139.66 ns | 0.635 ns | 0.594 ns |  2.79 |  XIV |
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

    [Benchmark(Description = "CalendarDate   (Y)  ")]
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

    [Benchmark(Description = "CalendarDay         ")]
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

    [Benchmark(Description = "OrdinalDate    (O)  ")]
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

    [Benchmark(Description = "ZDate               ")]
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

    [Benchmark(Description = "DayNumber       (g) ")]
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

    [Benchmark(Description = "DayNumber64     (g) ")]
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

    [Benchmark(Description = "Naked Civil        +")]
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

    [Benchmark(Description = "CivilParts     (Y) +")]
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

    [Benchmark(Description = "CivilTriple    (Y) +")]
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

    [Benchmark(Description = "XCivilDate     (Yg)+")]
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

    [Benchmark(Description = "CivilDate       (g)+", Baseline = true)]
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

    [Benchmark(Description = "MyDate  (Y)        +")]
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

    [Benchmark(Description = "MyCivilDate        +")]
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

    [Benchmark(Description = "LocalDate *     (Y) ")]
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

    [Benchmark(Description = "DateOnly *         +")]
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

    [Benchmark(Description = "DateTime *         +")]
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
