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
|          'CivilDay   (g)' |  40.77 ns | 0.192 ns | 0.179 ns |  1.00 |    0.00 |    I |
|       'DayNumber64      ' |  42.28 ns | 0.181 ns | 0.161 ns |  1.04 |    0.01 |   II |
|         'DayNumber      ' |  46.75 ns | 0.182 ns | 0.171 ns |  1.15 |    0.01 |  III |
|      'GregorianDay   (g)' |  47.23 ns | 0.437 ns | 0.365 ns |  1.16 |    0.01 |  III |
|          'DateTime *    ' |  51.97 ns | 0.129 ns | 0.121 ns |  1.27 |    0.01 |   IV |
|          'DateOnly *    ' |  52.96 ns | 0.197 ns | 0.184 ns |  1.30 |    0.01 |    V |
|       'DayTemplate      ' |  58.84 ns | 0.229 ns | 0.203 ns |  1.44 |    0.01 |   VI |
|       'CalendarDay      ' |  59.55 ns | 0.182 ns | 0.161 ns |  1.46 |    0.01 |   VI |
|          'WideDate      ' |  63.53 ns | 0.284 ns | 0.252 ns |  1.56 |    0.01 |  VII |
|         'CivilDate  (Yg)' |  64.30 ns | 0.283 ns | 0.250 ns |  1.58 |    0.01 |  VII |
| 'DayNumber (Naked)      ' |  70.76 ns | 0.419 ns | 0.391 ns |  1.74 |    0.01 | VIII |
|       'OrdinalDate  (O) ' |  76.91 ns | 0.651 ns | 0.577 ns |  1.89 |    0.01 |   IX |
|   'GregorianTriple  (Y) ' |  77.28 ns | 0.643 ns | 0.570 ns |  1.90 |    0.02 |   IX |
|      'DateTemplate  (Y) ' |  81.84 ns | 0.158 ns | 0.132 ns |  2.01 |    0.01 |    X |
|      'CalendarDate  (Y) ' |  85.24 ns | 0.378 ns | 0.354 ns |  2.09 |    0.01 |   XI |
|   'GregorianRecord  (Y) ' | 115.98 ns | 0.664 ns | 0.621 ns |  2.84 |    0.02 |  XII |
|         'LocalDate *(Y) ' | 117.28 ns | 0.535 ns | 0.501 ns |  2.88 |    0.01 |  XII |

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1766 (21H2)
Intel Core2 Duo CPU E8500 3.16GHz, 1 CPU, 2 logical and 2 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  DefaultJob : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

|                    Method |      Mean |    Error |   StdDev | Ratio | RatioSD | Rank |
|-------------------------- |----------:|---------:|---------:|------:|--------:|-----:|
|          'CivilDay   (g)' |  51.68 ns | 0.059 ns | 0.055 ns |  1.00 |    0.00 |    I |
|      'GregorianDay   (g)' |  59.64 ns | 0.071 ns | 0.059 ns |  1.15 |    0.00 |   II |
|       'DayNumber64      ' |  62.56 ns | 0.079 ns | 0.070 ns |  1.21 |    0.00 |  III |
|       'DayTemplate      ' |  67.13 ns | 0.067 ns | 0.063 ns |  1.30 |    0.00 |   IV |
|         'DayNumber      ' |  67.25 ns | 0.064 ns | 0.057 ns |  1.30 |    0.00 |   IV |
| 'DayNumber (Naked)      ' |  82.72 ns | 0.188 ns | 0.176 ns |  1.60 |    0.00 |    V |
|       'CalendarDay      ' |  85.38 ns | 0.082 ns | 0.073 ns |  1.65 |    0.00 |   VI |
|          'WideDate      ' |  90.21 ns | 0.130 ns | 0.115 ns |  1.75 |    0.00 |  VII |
|         'CivilDate  (Yg)' |  90.92 ns | 0.130 ns | 0.122 ns |  1.76 |    0.00 |  VII |
|   'GregorianTriple  (Y) ' |  95.91 ns | 0.177 ns | 0.165 ns |  1.86 |    0.00 | VIII |
|      'DateTemplate  (Y) ' |  98.32 ns | 0.136 ns | 0.127 ns |  1.90 |    0.00 |   IX |
|       'OrdinalDate  (O) ' | 100.77 ns | 0.099 ns | 0.093 ns |  1.95 |    0.00 |    X |
|      'CalendarDate  (Y) ' | 112.22 ns | 0.079 ns | 0.066 ns |  2.17 |    0.00 |   XI |
|          'DateOnly *    ' | 115.76 ns | 2.610 ns | 7.696 ns |  2.25 |    0.14 |   XI |
|          'DateTime *    ' | 127.20 ns | 2.454 ns | 2.626 ns |  2.46 |    0.05 |  XII |
|         'LocalDate *(Y) ' | 138.56 ns | 0.211 ns | 0.198 ns |  2.68 |    0.00 | XIII |
|   'GregorianRecord  (Y) ' | 142.99 ns | 1.469 ns | 1.302 ns |  2.77 |    0.03 |  XIV |
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
