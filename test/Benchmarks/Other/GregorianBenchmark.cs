// SPDX-License-Identifier: BSD-3-Clause
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
BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.2006/21H2/November2021Update)
Intel Core i7-4500U CPU 1.80GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.401
  [Host]     : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2

|                 Method |      Mean |    Error |   StdDev | Ratio | RatioSD | Rank |
|----------------------- |----------:|---------:|---------:|------:|--------:|-----:|
|     'CivilDate   (g)+' |  38.29 ns | 0.219 ns | 0.183 ns |  1.00 |    0.00 |    I |
|   'DayNumber64   (g) ' |  43.54 ns | 0.144 ns | 0.134 ns |  1.14 |    0.01 |   II |
|     'DayNumber   (g) ' |  44.63 ns | 0.127 ns | 0.119 ns |  1.17 |    0.01 |  III |
| 'GregorianDate   (g) ' |  49.77 ns | 0.185 ns | 0.173 ns |  1.30 |    0.01 |   IV |
|   'MyCivilDate      +' |  50.73 ns | 0.091 ns | 0.081 ns |  1.32 |    0.01 |    V |
|      'DateTime *    +' |  52.57 ns | 0.184 ns | 0.163 ns |  1.37 |    0.01 |   VI |
|      'DateOnly *    +' |  53.44 ns | 0.150 ns | 0.133 ns |  1.40 |    0.01 |  VII |
|    'XCivilDate  (Yg)+' |  63.52 ns | 0.173 ns | 0.154 ns |  1.66 |    0.01 | VIII |
|   'Naked Civil      +' |  64.33 ns | 0.352 ns | 0.330 ns |  1.68 |    0.01 | VIII |
|         'ZDate       ' |  65.34 ns | 0.118 ns | 0.092 ns |  1.71 |    0.01 |   IX |
|   'CalendarDay       ' |  65.78 ns | 0.608 ns | 0.539 ns |  1.72 |    0.02 |   IX |
|   'OrdinalDate  (O)  ' |  75.95 ns | 0.769 ns | 0.682 ns |  1.98 |    0.02 |    X |
|   'CivilTriple  (Y) +' |  85.76 ns | 0.481 ns | 0.426 ns |  2.24 |    0.02 |   XI |
|        'MyDate  (Y) +' |  88.27 ns | 0.493 ns | 0.461 ns |  2.31 |    0.01 |  XII |
|  'CalendarDate  (Y)  ' |  92.48 ns | 0.664 ns | 0.621 ns |  2.42 |    0.02 | XIII |
|    'CivilParts  (Y) +' | 100.70 ns | 0.893 ns | 0.835 ns |  2.63 |    0.03 |  XIV |
|     'LocalDate *(Y)  ' | 117.53 ns | 0.480 ns | 0.449 ns |  3.07 |    0.02 |   XV |

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
