// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Other;

using NodaTime;

using Samples;

using Zorglub.Time;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Simple;

/*
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1766 (21H2)
Intel Core i7-4500U CPU 1.80GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  DefaultJob : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

|                    Method |      Mean |    Error |   StdDev |    Median | Ratio | RatioSD | Rank |
|-------------------------- |----------:|---------:|---------:|----------:|------:|--------:|-----:|
|          'CivilDay   (g)' |  39.70 ns | 0.246 ns | 0.218 ns |  39.60 ns |  1.00 |    0.00 |    I |
|       'DayNumber64      ' |  41.85 ns | 0.119 ns | 0.093 ns |  41.88 ns |  1.05 |    0.01 |   II |
|      'GregorianDay   (g)' |  45.27 ns | 0.398 ns | 0.353 ns |  45.14 ns |  1.14 |    0.01 |  III |
|         'DayNumber      ' |  45.73 ns | 0.140 ns | 0.124 ns |  45.69 ns |  1.15 |    0.01 |  III |
|          'DateTime *    ' |  50.95 ns | 0.227 ns | 0.213 ns |  50.90 ns |  1.28 |    0.01 |   IV |
|          'DateOnly *    ' |  54.39 ns | 0.148 ns | 0.138 ns |  54.42 ns |  1.37 |    0.01 |    V |
|       'DayTemplate      ' |  58.54 ns | 1.027 ns | 2.075 ns |  57.74 ns |  1.49 |    0.06 |   VI |
|       'CalendarDay      ' |  59.78 ns | 0.240 ns | 0.225 ns |  59.74 ns |  1.51 |    0.01 |  VII |
|          'WideDate      ' |  60.45 ns | 0.151 ns | 0.134 ns |  60.41 ns |  1.52 |    0.01 |  VII |
| 'DayNumber (Naked)      ' |  64.11 ns | 0.421 ns | 0.394 ns |  64.18 ns |  1.61 |    0.01 | VIII |
|         'CivilDate  (Yg)' |  64.54 ns | 0.441 ns | 0.412 ns |  64.36 ns |  1.62 |    0.01 | VIII |
|       'OrdinalDate  (O) ' |  80.91 ns | 0.446 ns | 0.417 ns |  80.93 ns |  2.04 |    0.01 |   IX |
|   'GregorianTriple  (Y) ' |  84.07 ns | 1.222 ns | 1.020 ns |  83.80 ns |  2.12 |    0.03 |    X |
|      'DateTemplate  (Y) ' |  90.06 ns | 0.372 ns | 0.348 ns |  89.87 ns |  2.27 |    0.02 |   XI |
|      'CalendarDate  (Y) ' |  94.38 ns | 0.313 ns | 0.277 ns |  94.27 ns |  2.38 |    0.01 |  XII |
|   'GregorianRecord  (Y) ' | 112.01 ns | 1.838 ns | 1.535 ns | 111.53 ns |  2.82 |    0.04 | XIII |
|         'LocalDate *(Y) ' | 115.90 ns | 2.216 ns | 2.176 ns | 115.36 ns |  2.92 |    0.06 |  XIV |

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1766 (21H2)
Intel Core2 Duo CPU E8500 3.16GHz, 1 CPU, 2 logical and 2 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  DefaultJob : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

|                    Method |      Mean |    Error |   StdDev | Ratio | RatioSD | Rank |
|-------------------------- |----------:|---------:|---------:|------:|--------:|-----:|
|          'CivilDay   (g)' |  52.69 ns | 0.026 ns | 0.024 ns |  1.00 |    0.00 |    I |
|      'GregorianDay   (g)' |  59.96 ns | 0.016 ns | 0.012 ns |  1.14 |    0.00 |   II |
|       'DayNumber64      ' |  62.58 ns | 0.046 ns | 0.041 ns |  1.19 |    0.00 |  III |
|         'DayNumber      ' |  71.57 ns | 0.054 ns | 0.051 ns |  1.36 |    0.00 |   IV |
|       'DayTemplate      ' |  75.06 ns | 0.084 ns | 0.079 ns |  1.42 |    0.00 |    V |
|       'CalendarDay      ' |  85.40 ns | 0.058 ns | 0.055 ns |  1.62 |    0.00 |   VI |
| 'DayNumber (Naked)      ' |  89.28 ns | 0.061 ns | 0.057 ns |  1.69 |    0.00 |  VII |
|          'WideDate      ' |  89.80 ns | 0.368 ns | 0.327 ns |  1.70 |    0.01 |  VII |
|         'CivilDate  (Yg)' |  91.25 ns | 0.061 ns | 0.057 ns |  1.73 |    0.00 | VIII |
|   'GregorianTriple  (Y) ' | 104.92 ns | 0.058 ns | 0.054 ns |  1.99 |    0.00 |   IX |
|       'OrdinalDate  (O) ' | 106.12 ns | 0.425 ns | 0.377 ns |  2.01 |    0.01 |   IX |
|      'DateTemplate  (Y) ' | 107.22 ns | 0.071 ns | 0.066 ns |  2.03 |    0.00 |   IX |
|      'CalendarDate  (Y) ' | 115.07 ns | 0.105 ns | 0.098 ns |  2.18 |    0.00 |    X |
|          'DateOnly *    ' | 122.96 ns | 3.308 ns | 9.753 ns |  2.34 |    0.19 |   XI |
|          'DateTime *    ' | 128.34 ns | 2.509 ns | 3.173 ns |  2.44 |    0.07 |  XII |
|         'LocalDate *(Y) ' | 136.28 ns | 0.108 ns | 0.096 ns |  2.59 |    0.00 | XIII |
|   'GregorianRecord  (Y) ' | 138.88 ns | 1.686 ns | 1.494 ns |  2.64 |    0.03 |  XIV |
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

    [Benchmark(Description = "WideDate      ")]
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
