// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Micro;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology.Scopes;

/* BenchmarkOption.Fixed
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1766 (21H2)
Intel Core i7-4500U CPU 1.80GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  DefaultJob : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

|             Method |     Mean |    Error |   StdDev | Ratio | RatioSD | Rank | Code Size |
|------------------- |---------:|---------:|---------:|------:|--------:|-----:|----------:|
|            Lenient | 11.45 ns | 0.115 ns | 0.102 ns |  1.00 |    0.00 |    I |     500 B |
|           Standard | 13.85 ns | 0.077 ns | 0.072 ns |  1.21 |    0.01 |   II |     573 B |
|         MinMaxYear | 13.91 ns | 0.072 ns | 0.067 ns |  1.22 |    0.01 |   II |     571 B |
|          Proleptic | 13.94 ns | 0.140 ns | 0.131 ns |  1.22 |    0.02 |   II |     577 B |
| GregorianProleptic | 14.40 ns | 0.109 ns | 0.097 ns |  1.26 |    0.02 |  III |     721 B |

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1766 (21H2)
Intel Core2 Duo CPU E8500 3.16GHz, 1 CPU, 2 logical and 2 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  DefaultJob : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

|             Method |     Mean |    Error |   StdDev | Ratio | Rank | Code Size |
|------------------- |---------:|---------:|---------:|------:|-----:|----------:|
|            Lenient | 13.00 ns | 0.041 ns | 0.038 ns |  1.00 |    I |     500 B |
| GregorianProleptic | 16.47 ns | 0.027 ns | 0.025 ns |  1.27 |  III |     721 B |
|          Proleptic | 16.47 ns | 0.022 ns | 0.020 ns |  1.27 |  III |     577 B |
|         MinMaxYear | 16.47 ns | 0.029 ns | 0.027 ns |  1.27 |  III |     571 B |
|           Standard | 16.47 ns | 0.012 ns | 0.011 ns |  1.27 |  III |     573 B |
 */

[DisassemblyDiagnoser]
public class CalendarScopeBenchmark : BenchmarkBase
{
    private static readonly ProlepticScope s_ProlepticScope =
        new(new GregorianSchema(), DayZero.NewStyle);

    private static readonly StandardScope s_StandardScope =
        new(new GregorianSchema(), DayZero.NewStyle);

    private static readonly MinMaxYearScope s_MinMaxYearScope =
        MinMaxYearScope.CreateMaximal(new GregorianSchema(), DayZero.NewStyle);

    public CalendarScopeBenchmark()
    {
        // Fixed => we benchmark the fast track of ValidateYearMonthDay().
        Option = BenchmarkOption.Fixed;
        //Option = BenchmarkOption.Slow;
    }

    [Benchmark(Baseline = true)]
    public void Lenient()
    {
        var ymd = Core(Year, Month, Day);
        Consume(in ymd);

        static Yemoda Core(int y, int m, int d) => new(y, m, d);
    }

    [Benchmark]
    public void GregorianProleptic()
    {
        var ymd = Core(Year, Month, Day);
        Consume(in ymd);

        static Yemoda Core(int y, int m, int d)
        {
            GregorianProlepticScope.ValidateYearMonthDay(y, m, d);
            return new Yemoda(y, m, d);
        }
    }

    [Benchmark]
    public void Proleptic()
    {
        var ymd = Core(Year, Month, Day);
        Consume(in ymd);

        static Yemoda Core(int y, int m, int d)
        {
            s_ProlepticScope.ValidateYearMonthDay(y, m, d);
            return new Yemoda(y, m, d);
        }
    }

    [Benchmark]
    public void Standard()
    {
        var ymd = Core(Year, Month, Day);
        Consume(in ymd);

        static Yemoda Core(int y, int m, int d)
        {
            s_StandardScope.ValidateYearMonthDay(y, m, d);
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
            s_MinMaxYearScope.ValidateYearMonthDay(y, m, d);
            return new Yemoda(y, m, d);
        }
    }
}
