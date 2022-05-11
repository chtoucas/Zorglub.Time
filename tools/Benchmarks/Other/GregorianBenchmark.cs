// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Other;

using NodaTime;

using Samples;

using Zorglub.Time;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Simple;

/*
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1586 (21H2)
Intel Core i7-4500U CPU 1.80GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.201
  [Host]     : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
  DefaultJob : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT

|               Method |      Mean |    Error |   StdDev |    Median | Ratio | RatioSD | Rank |
|--------------------- |----------:|---------:|---------:|----------:|------:|--------:|-----:|
|    'DayNumber      ' |  45.45 ns | 0.227 ns | 0.213 ns |  45.36 ns |  1.00 |    0.00 |    I |
|     'DateTime *    ' |  51.96 ns | 0.241 ns | 0.225 ns |  51.91 ns |  1.14 |    0.01 |   II |
|     'DateOnly *    ' |  53.32 ns | 0.133 ns | 0.111 ns |  53.31 ns |  1.17 |    0.01 |  III |
|  'CalendarDay      ' |  60.43 ns | 0.948 ns | 2.361 ns |  59.60 ns |  1.38 |    0.09 |   IV |
|  'OrdinalDate  (O) ' |  86.54 ns | 0.451 ns | 0.422 ns |  86.59 ns |  1.90 |    0.01 |    V |
| 'CalendarDate  (Y) ' |  96.15 ns | 0.374 ns | 0.332 ns |  96.09 ns |  2.12 |    0.01 |   VI |
|    'LocalDate *(Y) ' | 117.73 ns | 0.428 ns | 0.357 ns | 117.62 ns |  2.59 |    0.02 |  VII |

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1586 (20H2/October2020Update)
Intel Core2 Duo CPU E8500 3.16GHz, 1 CPU, 2 logical and 2 physical cores
.NET SDK=6.0.201
  [Host]     : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
  DefaultJob : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT

|                    Method |      Mean |    Error |   StdDev |    Median | Ratio | RatioSD | Rank |
|-------------------------- |----------:|---------:|---------:|----------:|------:|--------:|-----:|
|          'CivilDay   (g)' |  48.65 ns | 0.066 ns | 0.062 ns |  48.66 ns |  1.00 |    0.00 |    I |
|      'GregorianDay   (g)' |  55.57 ns | 0.045 ns | 0.040 ns |  55.56 ns |  1.14 |    0.00 |   II |
|       'DayNumber64      ' |  61.67 ns | 0.101 ns | 0.095 ns |  61.61 ns |  1.27 |    0.00 |  III |
|         'DayNumber      ' |  70.98 ns | 0.094 ns | 0.088 ns |  70.96 ns |  1.46 |    0.00 |   IV |
|       'DayTemplate      ' |  71.95 ns | 0.107 ns | 0.095 ns |  71.93 ns |  1.48 |    0.00 |    V |
|       'CalendarDay      ' |  80.88 ns | 0.095 ns | 0.089 ns |  80.85 ns |  1.66 |    0.00 |   VI |
| 'DayNumber (Naked)      ' |  90.93 ns | 0.726 ns | 0.679 ns |  91.11 ns |  1.87 |    0.01 |  VII |
|         'CivilDate  (Yg)' |  91.27 ns | 0.114 ns | 0.107 ns |  91.28 ns |  1.88 |    0.00 |  VII |
|       'OrdinalDate  (O) ' | 106.81 ns | 0.101 ns | 0.094 ns | 106.76 ns |  2.20 |    0.00 | VIII |
|   'GregorianTriple  (Y) ' | 107.95 ns | 0.122 ns | 0.108 ns | 107.93 ns |  2.22 |    0.00 |   IX |
|          'DateOnly *    ' | 116.23 ns | 2.688 ns | 7.926 ns | 113.90 ns |  2.38 |    0.15 |    X |
|      'DateTemplate  (Y) ' | 120.62 ns | 2.282 ns | 2.135 ns | 120.12 ns |  2.48 |    0.04 |   XI |
|      'CalendarDate  (Y) ' | 120.92 ns | 0.115 ns | 0.102 ns | 120.92 ns |  2.48 |    0.00 |   XI |
|   'GregorianRecord  (Y) ' | 125.43 ns | 0.246 ns | 0.218 ns | 125.47 ns |  2.58 |    0.00 |  XII |
|          'DateTime *    ' | 126.57 ns | 2.575 ns | 3.774 ns | 126.88 ns |  2.59 |    0.07 |  XII |
|         'LocalDate *(Y) ' | 136.62 ns | 0.227 ns | 0.202 ns | 136.56 ns |  2.81 |    0.01 | XIII |
|          'WideDate  (Y) ' | 137.06 ns | 0.085 ns | 0.071 ns | 137.03 ns |  2.82 |    0.00 | XIII |
 */
//
// * = external, Y = Y/M/D repr., O = ord. repr., g = Gregorian-only
//
// Comments:
// - Here, we do not really test the overall design, we only test the
//   various implementations of a Gregorian calendar which are usually over
//   optimized. See JulianBenchmark for more realistic benchmarks.
// - This is the worst possible scenario for a date object: arithmetic and
//   DayOfWeek rely on the count of days since the epoch not on the Y/M/D
//   representation, therefore DayNumber, DateTime and other types are
//   favoured. By the way, these types have predictable performances,
//   their results do not vary depending on the input.
// - CivilDate and DateTime only deal with positive years (faster divisions).
// - LocalDate caches the start of the year.
// - DateTime is a time object, not a date object.
// - With DayNumber and DayNumber64, we only validate the result twice (at
//   the start and end) to mimic the behaviour of other date types.

public class GregorianBenchmark : BenchmarkBase
{
    private const int D7 = 7;        // No change of month.
    private const int D30 = 30;      // Change of month.
    private const int D401 = 401;    // "Slow-track".

    public GregorianBenchmark() { Option = BenchmarkOption.Fixed; }

    [Benchmark(Description = "CalendarDate  (Y) ")]
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

    [Benchmark(Description = "CalendarDay      ")]
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

    [Benchmark(Description = "OrdinalDate  (O) ")]
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

    [Benchmark(Description = "WideDate  (Y) ")]
    public void WithWideDate()
    {
        WideDate start = new(Year, Month, Day);
        WideDate end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);

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

    [Benchmark(Description = "DayNumber      ")]
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

    [Benchmark(Description = "DayNumber64      ")]
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

    [Benchmark(Description = "DayNumber (Naked)      ")]
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

    [Benchmark(Description = "GregorianRecord  (Y) ")]
    public void WithGregorianRecord()
    {
        GregorianRecord start = new(Year, Month, Day);
        GregorianRecord end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);
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

    [Benchmark(Description = "GregorianTriple  (Y) ")]
    public void WithGregorianTriple()
    {
        GregorianTriple start = new(Year, Month, Day);
        GregorianTriple end = start.NextDay().PlusDays(D7).PlusDays(D30).PlusDays(D401);
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

    [Benchmark(Description = "CivilDate  (Yg)")]
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

    [Benchmark(Description = "CivilDay   (g)", Baseline = true)]
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

    [Benchmark(Description = "GregorianDay   (g)")]
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

    [Benchmark(Description = "DateTemplate  (Y) ")]
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

    [Benchmark(Description = "DayTemplate      ")]
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

    [Benchmark(Description = "LocalDate *(Y) ")]
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

    [Benchmark(Description = "DateOnly *    ")]
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

    [Benchmark(Description = "DateTime *    ")]
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
