// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Micro;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology.Scopes;

// TODO(perf): Oddly, it seems that the first benchmark to run is slower
// than it should (compare Lenient and AAA).

/* BenchmarkOption.Fixed
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1586 (21H2)
Intel Core i7-4500U CPU 1.80GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.201
  [Host]     : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
  DefaultJob : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT

|                      Method |     Mean |    Error |   StdDev | Ratio | RatioSD | Rank | Code Size |
|---------------------------- |---------:|---------:|---------:|------:|--------:|-----:|----------:|
|                         AAA | 11.24 ns | 0.100 ns | 0.084 ns |  0.99 |    0.01 |    I |     500 B |
|                     Lenient | 11.38 ns | 0.100 ns | 0.093 ns |  1.00 |    0.00 |    I |     500 B |
|     GregorianProlepticShort | 13.39 ns | 0.127 ns | 0.119 ns |  1.18 |    0.01 |   II |     751 B |
|         SolarProlepticShort | 13.44 ns | 0.144 ns | 0.134 ns |  1.18 |    0.01 |   II |     643 B |
|                  SolarShort | 13.46 ns | 0.131 ns | 0.123 ns |  1.18 |    0.01 |   II |     639 B |
|            GregorianMaximal | 13.54 ns | 0.058 ns | 0.051 ns |  1.19 |    0.01 |   II |     758 B |
|                  MinMaxYear | 13.80 ns | 0.057 ns | 0.051 ns |  1.21 |    0.01 |  III |     569 B |
| GregorianProlepticShortImpl | 13.98 ns | 0.132 ns | 0.123 ns |  1.23 |    0.01 |  III |     716 B |
|       GregorianMaximal_Wide | 14.09 ns | 0.173 ns | 0.162 ns |  1.24 |    0.02 |  III |     716 B |
|       DefaultProlepticShort | 14.13 ns | 0.085 ns | 0.080 ns |  1.24 |    0.01 |  III |     575 B |
|                DefaultShort | 18.50 ns | 0.060 ns | 0.050 ns |  1.63 |    0.01 |   IV |     672 B |

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1766 (21H2)
Intel Core2 Duo CPU E8500 3.16GHz, 1 CPU, 2 logical and 2 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  DefaultJob : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

|                  Method |     Mean |    Error |   StdDev | Ratio | Rank | Code Size |
|------------------------ |---------:|---------:|---------:|------:|-----:|----------:|
|                 Lenient | 13.00 ns | 0.041 ns | 0.038 ns |  1.00 |    I |     500 B |
|                     AAA | 15.86 ns | 0.026 ns | 0.024 ns |  1.22 |   II |     500 B |
|   GregorianMaximal_Wide | 16.46 ns | 0.023 ns | 0.021 ns |  1.27 |  III |     721 B |
| GregorianProlepticShort | 16.47 ns | 0.027 ns | 0.025 ns |  1.27 |  III |     721 B |
|          ProlepticShort | 16.47 ns | 0.022 ns | 0.020 ns |  1.27 |  III |     577 B |
|              MinMaxYear | 16.47 ns | 0.029 ns | 0.027 ns |  1.27 |  III |     571 B |
|           StandardShort | 16.47 ns | 0.012 ns | 0.011 ns |  1.27 |  III |     573 B |
|        GregorianMaximal | 18.37 ns | 0.022 ns | 0.021 ns |  1.41 |   IV |     767 B |
 */

[DisassemblyDiagnoser]
public class CalendarScopeBenchmark : BenchmarkBase
{
    private static readonly ProlepticShortScope s_ProlepticShort =
        new(new GregorianSchema(), DayZero.NewStyle);

    private static readonly StandardShortScope s_StandardShort =
        new(new GregorianSchema(), DayZero.NewStyle);

    private static readonly GregorianMaximalScope s_GregorianMaximal =
        new(new GregorianSchema(), DayZero.NewStyle, onOrAfterEpoch: false);

    private static readonly MinMaxYearScope s_MinMaxYear =
        MinMaxYearScope.WithMaximalRange(new GregorianSchema(), DayZero.NewStyle, onOrAfterEpoch: false);

    public CalendarScopeBenchmark()
    {
        // Fixed => we benchmark the fast track of ValidateYearMonthDay().
        Option = BenchmarkOption.Fixed;
        //Option = BenchmarkOption.Slow;
    }

    [Benchmark]
    public void AAA()
    {
        var ymd = Core(Year, Month, Day);
        Consume(in ymd);

        static Yemoda Core(int y, int m, int d) => new(y, m, d);
    }

    [Benchmark(Baseline = true)]
    public void Lenient()
    {
        var ymd = Core(Year, Month, Day);
        Consume(in ymd);

        static Yemoda Core(int y, int m, int d) => new(y, m, d);
    }

    [Benchmark]
    public void GregorianProlepticShort()
    {
        var ymd = Core(Year, Month, Day);
        Consume(in ymd);

        static Yemoda Core(int y, int m, int d)
        {
            GregorianProlepticShortScope.ValidateYearMonthDayImpl(y, m, d);
            return new Yemoda(y, m, d);
        }
    }

    [Benchmark]
    public void ProlepticShort()
    {
        var ymd = Core(Year, Month, Day);
        Consume(in ymd);

        static Yemoda Core(int y, int m, int d)
        {
            s_ProlepticShort.ValidateYearMonthDay(y, m, d);
            return new Yemoda(y, m, d);
        }
    }

    [Benchmark]
    public void StandardShort()
    {
        var ymd = Core(Year, Month, Day);
        Consume(in ymd);

        static Yemoda Core(int y, int m, int d)
        {
            s_StandardShort.ValidateYearMonthDay(y, m, d);
            return new Yemoda(y, m, d);
        }
    }

    [Benchmark]
    public void GregorianMaximal()
    {
        var ymd = Core(Year, Month, Day);
        Consume(in ymd);

        static Yemoda Core(int y, int m, int d)
        {
            s_GregorianMaximal.ValidateYearMonthDay(y, m, d);
            return new Yemoda(y, m, d);
        }
    }

    [Benchmark]
    public void GregorianMaximal_Wide()
    {
        var ymd = Core(Year, Month, Day);
        Consume(in ymd);

        static Yemoda Core(int y, int m, int d)
        {
            GregorianMaximalScope.ValidateWideYearMonthDay(y, m, d);
            return new Yemoda(y, m, d);
        }
    }

    [Benchmark]
    public void MinMaxYear()
    {
        var ymd = Core(Year, Month, Day);
        Consume(in ymd);

        static Yemoda Core(int y, int m, int d)
        {
            s_MinMaxYear.ValidateYearMonthDay(y, m, d);
            return new Yemoda(y, m, d);
        }
    }
}
