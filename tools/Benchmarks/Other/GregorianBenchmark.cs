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

|                    Method |      Mean |    Error |   StdDev | Ratio | RatioSD | Rank |
|-------------------------- |----------:|---------:|---------:|------:|--------:|-----:|
|          'CivilDay   (g)' |  39.40 ns | 0.061 ns | 0.048 ns |  1.00 |    0.00 |    I |
|       'DayNumber64      ' |  42.46 ns | 0.255 ns | 0.226 ns |  1.08 |    0.00 |   II |
|         'DayNumber      ' |  45.75 ns | 0.142 ns | 0.126 ns |  1.16 |    0.00 |  III |
|      'GregorianDay   (g)' |  45.95 ns | 0.356 ns | 0.333 ns |  1.17 |    0.01 |  III |
|          'DateTime *    ' |  52.09 ns | 0.189 ns | 0.167 ns |  1.32 |    0.00 |   IV |
|          'DateOnly *    ' |  53.46 ns | 0.148 ns | 0.131 ns |  1.36 |    0.00 |    V |
|       'DayTemplate      ' |  53.74 ns | 0.443 ns | 0.414 ns |  1.36 |    0.01 |    V |
|       'CalendarDay      ' |  59.79 ns | 0.365 ns | 0.341 ns |  1.52 |    0.01 |   VI |
| 'DayNumber (Naked)      ' |  62.87 ns | 0.440 ns | 0.411 ns |  1.59 |    0.01 |  VII |
|         'CivilDate  (Yg)' |  64.20 ns | 0.122 ns | 0.095 ns |  1.63 |    0.00 | VIII |
|      'DateTemplate  (Y) ' |  78.14 ns | 0.294 ns | 0.261 ns |  1.98 |    0.01 |   IX |
|   'GregorianTriple  (Y) ' |  80.79 ns | 0.380 ns | 0.355 ns |  2.05 |    0.01 |    X |
|       'OrdinalDate  (O) ' |  81.99 ns | 0.471 ns | 0.440 ns |  2.08 |    0.01 |   XI |
|      'CalendarDate  (Y) ' |  91.88 ns | 0.386 ns | 0.342 ns |  2.33 |    0.01 |  XII |
|   'GregorianRecord  (Y) ' |  98.36 ns | 0.259 ns | 0.242 ns |  2.50 |    0.01 | XIII |
|          'WideDate  (Y) ' | 111.34 ns | 0.610 ns | 0.509 ns |  2.83 |    0.01 |  XIV |
|         'LocalDate *(Y) ' | 117.50 ns | 0.606 ns | 0.567 ns |  2.98 |    0.02 |   XV |

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
