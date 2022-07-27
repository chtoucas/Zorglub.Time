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

|                    Method |      Mean |    Error |   StdDev | Ratio | RatioSD | Rank |
|-------------------------- |----------:|---------:|---------:|------:|--------:|-----:|
|          'CivilDay   (g)' |  39.59 ns | 0.173 ns | 0.162 ns |  1.00 |    0.00 |    I |
|       'DayNumber64   (g)' |  42.06 ns | 0.196 ns | 0.183 ns |  1.06 |    0.01 |   II |
|      'GregorianDay   (g)' |  44.82 ns | 0.285 ns | 0.223 ns |  1.13 |    0.01 |  III |
|         'DayNumber   (g)' |  47.30 ns | 0.185 ns | 0.154 ns |  1.20 |    0.01 |   IV |
|          'DateTime *    ' |  51.04 ns | 0.249 ns | 0.233 ns |  1.29 |    0.01 |    V |
|          'DateOnly *    ' |  53.43 ns | 0.144 ns | 0.128 ns |  1.35 |    0.01 |   VI |
|       'DayTemplate      ' |  53.51 ns | 0.321 ns | 0.300 ns |  1.35 |    0.01 |   VI |
|       'CalendarDay      ' |  59.45 ns | 0.270 ns | 0.240 ns |  1.50 |    0.01 |  VII |
|         'CivilDate  (Yg)' |  64.04 ns | 0.384 ns | 0.340 ns |  1.62 |    0.01 | VIII |
|             'ZDate      ' |  64.66 ns | 0.254 ns | 0.238 ns |  1.63 |    0.01 | VIII |
| 'DayNumber (Naked)      ' |  70.59 ns | 0.601 ns | 0.562 ns |  1.78 |    0.02 |   IX |
|       'OrdinalDate  (O) ' |  74.84 ns | 0.416 ns | 0.368 ns |  1.89 |    0.01 |    X |
|   'GregorianTriple  (Y) ' |  79.10 ns | 0.306 ns | 0.272 ns |  2.00 |    0.01 |   XI |
|      'DateTemplate  (Y) ' |  88.73 ns | 0.489 ns | 0.458 ns |  2.24 |    0.02 |  XII |
|      'CalendarDate  (Y) ' |  88.98 ns | 0.455 ns | 0.403 ns |  2.25 |    0.02 |  XII |
|   'GregorianRecord  (Y) ' | 108.26 ns | 0.338 ns | 0.300 ns |  2.74 |    0.01 | XIII |
|         'LocalDate *(Y) ' | 117.87 ns | 0.837 ns | 0.783 ns |  2.98 |    0.02 |  XIV |

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1826 (21H2)
Intel Core2 Duo CPU E8500 3.16GHz, 1 CPU, 2 logical and 2 physical cores
.NET SDK=6.0.302
  [Host]     : .NET 6.0.7 (6.0.722.32202), X64 RyuJIT
  DefaultJob : .NET 6.0.7 (6.0.722.32202), X64 RyuJIT

|                   Method |      Mean |    Error |   StdDev | Ratio | RatioSD | Rank |
|------------------------- |----------:|---------:|---------:|------:|--------:|-----:|
|        'CivilDay   (g)+' |  53.55 ns | 0.068 ns | 0.063 ns |  1.00 |    0.00 |    I |
|    'GregorianDay   (g) ' |  57.17 ns | 0.038 ns | 0.032 ns |  1.07 |    0.00 |   II |
|     'DayNumber64   (g) ' |  62.26 ns | 0.072 ns | 0.068 ns |  1.16 |    0.00 |  III |
|     'DayTemplate      +' |  67.08 ns | 0.079 ns | 0.074 ns |  1.25 |    0.00 |   IV |
|       'DayNumber   (g) ' |  67.23 ns | 0.050 ns | 0.042 ns |  1.26 |    0.00 |   IV |
|     'CalendarDay       ' |  85.75 ns | 0.095 ns | 0.084 ns |  1.60 |    0.00 |    V |
| 'Naked DayNumber      +' |  87.07 ns | 0.059 ns | 0.049 ns |  1.63 |    0.00 |   VI |
|           'ZDate       ' |  90.26 ns | 0.398 ns | 0.372 ns |  1.69 |    0.01 |  VII |
|       'CivilDate  (Yg)+' |  90.42 ns | 0.112 ns | 0.105 ns |  1.69 |    0.00 |  VII |
|     'OrdinalDate  (O)  ' |  99.55 ns | 0.123 ns | 0.109 ns |  1.86 |    0.00 | VIII |
|     'CivilTriple  (Y) +' | 106.09 ns | 0.117 ns | 0.109 ns |  1.98 |    0.00 |   IX |
|    'DateTemplate  (Y) +' | 107.92 ns | 0.064 ns | 0.060 ns |  2.02 |    0.00 |    X |
|    'CalendarDate  (Y)  ' | 114.08 ns | 0.136 ns | 0.127 ns |  2.13 |    0.00 |   XI |
|        'DateOnly *    +' | 128.91 ns | 2.772 ns | 8.172 ns |  2.44 |    0.10 |  XII |
|        'DateTime *    +' | 130.70 ns | 2.497 ns | 2.336 ns |  2.44 |    0.04 |  XII |
|      'CivilParts  (Y) +' | 131.75 ns | 0.296 ns | 0.277 ns |  2.46 |    0.01 |  XII |
|       'LocalDate *(Y)  ' | 136.11 ns | 0.109 ns | 0.091 ns |  2.54 |    0.00 | XIII |
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
// - CivilDate, CivilDay, DayTemplate, DateTemplate, DateOnly and DateTime only
//   deal with positive years (faster divisions).
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

    [Benchmark(Description = "CivilDate  (Yg)+")]
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

    [Benchmark(Description = "CivilDay   (g)+", Baseline = true)]
    public void WithCivilDay()
    {
        CivilDay start = new(Year, Month, Day);
        CivilDay end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

        var (y, m, d) = end;
        DayOfWeek dayOfWeek = end.DayOfWeek;
        int dayOfYear = end.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "GregorianDay   (g) ")]
    public void WithGregorianDay()
    {
        GregorianDay start = new(Year, Month, Day);
        GregorianDay end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

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

    [Benchmark(Description = "DateTemplate  (Y) +")]
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

    [Benchmark(Description = "DayTemplate      +")]
    public void WithDayTemplate()
    {
        DayTemplate start = new(Year, Month, Day);
        DayTemplate end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

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
