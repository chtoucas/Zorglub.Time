// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

//#define NO_INLINING
#define ALL

namespace Benchmarks.Micro;

#if NO_INLINING
using System.Runtime.CompilerServices;
#endif

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

// TODO(perf): Oddly, it seems that the first benchmark to run is slower
// than it should (compare Lenient and AAA).

/* BenchmarkOption.Fixed / INLINING
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

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1586 (20H2/October2020Update)
Intel Core2 Duo CPU E8500 3.16GHz, 1 CPU, 2 logical and 2 physical cores
.NET SDK=6.0.201
  [Host]     : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
  DefaultJob : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT

|                      Method |     Mean |    Error |   StdDev | Ratio | Rank | Code Size |
|---------------------------- |---------:|---------:|---------:|------:|-----:|----------:|
|                     Lenient | 12.97 ns | 0.011 ns | 0.009 ns |  1.00 |    I |     500 B |
|                  SolarShort | 15.52 ns | 0.020 ns | 0.018 ns |  1.20 |   II |     797 B |
|                         AAA | 15.85 ns | 0.020 ns | 0.019 ns |  1.22 |  III |     500 B |
|            GregorianMaximal | 16.14 ns | 0.026 ns | 0.025 ns |  1.24 |   IV |     914 B |
|     GregorianProlepticShort | 16.15 ns | 0.031 ns | 0.029 ns |  1.25 |   IV |     907 B |
|         SolarProlepticShort | 16.16 ns | 0.042 ns | 0.039 ns |  1.25 |   IV |     801 B |
| GregorianProlepticShortImpl | 16.46 ns | 0.024 ns | 0.021 ns |  1.27 |    V |     868 B |
|       GregorianMaximal_Wide | 16.47 ns | 0.026 ns | 0.025 ns |  1.27 |    V |     868 B |
|                  MinMaxYear | 17.41 ns | 0.020 ns | 0.018 ns |  1.34 |   VI |     655 B |
|       DefaultProlepticShort | 17.42 ns | 0.027 ns | 0.026 ns |  1.34 |   VI |     661 B |
|                DefaultShort | 20.27 ns | 0.025 ns | 0.024 ns |  1.56 |  VII |     832 B |

NO INLINING

 */

[DisassemblyDiagnoser]
public class CalendarScopeBenchmark : BenchmarkBase
{
    private static readonly GregorianProlepticShortScope s_GregorianProlepticShort =
        new(new GregorianSchema(), DayZero.NewStyle);

#pragma warning disable CS0618 // Type or member is obsolete

    private static readonly Solar12ProlepticShortScope s_SolarProlepticShort =
        new(new GregorianSchema(), DayZero.NewStyle);

    private static readonly Solar12StandardShortScope s_SolarStandardShort =
        new(new GregorianSchema(), DayZero.NewStyle);

#pragma warning restore CS0618 // Type or member is obsolete

#if ALL
    private static readonly PlainProlepticShortScope s_DefaultProlepticShort =
        new(new GregorianSchema(), DayZero.NewStyle);

    private static readonly PlainStandardShortScope s_DefaultStandardShort =
        new(new GregorianSchema(), DayZero.NewStyle);

    private static readonly GregorianMaximalScope s_GregorianMaximal =
        new(new GregorianSchema(), DayZero.NewStyle, onOrAfterEpoch: false);

    private static readonly MinMaxYearScope s_MinMaxYear =
        MinMaxYearScope.WithMaximalRange(new GregorianSchema(), DayZero.NewStyle, onOrAfterEpoch: false);
#endif

    public CalendarScopeBenchmark()
    {
        // Fixed => we benchmark the fast track of ValidateYearMonthDay().
        Option = BenchmarkOption.Fixed;
        //Option = BenchmarkOption.Slow;
    }

#if ALL
    [Benchmark]
    public void AAA()
    {
        var ymd = Core(Year, Month, Day);
        Consume(in ymd);

#if NO_INLINING
        [MethodImpl(MethodImplOptions.NoInlining)]
#endif
        static Yemoda Core(int y, int m, int d) => new(y, m, d);
    }
#endif

    [Benchmark(Baseline = true)]
    public void Lenient()
    {
        var ymd = Core(Year, Month, Day);
        Consume(in ymd);

#if NO_INLINING
        [MethodImpl(MethodImplOptions.NoInlining)]
#endif
        static Yemoda Core(int y, int m, int d) => new(y, m, d);
    }

    #region Proleptic short scope

    [Benchmark]
    public void GregorianProlepticShort()
    {
        var ymd = Core(Year, Month, Day);
        Consume(in ymd);

#if NO_INLINING
        [MethodImpl(MethodImplOptions.NoInlining)]
#endif
        static Yemoda Core(int y, int m, int d)
        {
            s_GregorianProlepticShort.ValidateYearMonthDay(y, m, d);
            return new Yemoda(y, m, d);
        }
    }

    [Benchmark]
    public void GregorianProlepticShort_Static()
    {
        var ymd = Core(Year, Month, Day);
        Consume(in ymd);

#if NO_INLINING
        [MethodImpl(MethodImplOptions.NoInlining)]
#endif
        static Yemoda Core(int y, int m, int d)
        {
            GregorianProlepticShortScope.ValidateYearMonthDayImpl(y, m, d);
            return new Yemoda(y, m, d);
        }
    }

    [Benchmark]
    public void SolarProlepticShort()
    {
        var ymd = Core(Year, Month, Day);
        Consume(in ymd);

#if NO_INLINING
        [MethodImpl(MethodImplOptions.NoInlining)]
#endif
        static Yemoda Core(int y, int m, int d)
        {
            s_SolarProlepticShort.ValidateYearMonthDay(y, m, d);
            return new Yemoda(y, m, d);
        }
    }

#if ALL
    [Benchmark]
    public void DefaultProlepticShort()
    {
        var ymd = Core(Year, Month, Day);
        Consume(in ymd);

#if NO_INLINING
        [MethodImpl(MethodImplOptions.NoInlining)]
#endif
        static Yemoda Core(int y, int m, int d)
        {
            s_DefaultProlepticShort.ValidateYearMonthDay(y, m, d);
            return new Yemoda(y, m, d);
        }
    }
#endif

    #endregion
    #region Standard short scope

    [Benchmark]
    public void SolarStandardShort()
    {
        var ymd = Core(Year, Month, Day);
        Consume(in ymd);

#if NO_INLINING
        [MethodImpl(MethodImplOptions.NoInlining)]
#endif
        static Yemoda Core(int y, int m, int d)
        {
            s_SolarStandardShort.ValidateYearMonthDay(y, m, d);
            return new Yemoda(y, m, d);
        }
    }

#if ALL
    [Benchmark]
    public void DefaultStandardShort()
    {
        var ymd = Core(Year, Month, Day);
        Consume(in ymd);

#if NO_INLINING
        [MethodImpl(MethodImplOptions.NoInlining)]
#endif
        static Yemoda Core(int y, int m, int d)
        {
            s_DefaultStandardShort.ValidateYearMonthDay(y, m, d);
            return new Yemoda(y, m, d);
        }
    }
#endif

    #endregion
    #region Maximal
#if ALL

    [Benchmark]
    public void GregorianMaximal()
    {
        var ymd = Core(Year, Month, Day);
        Consume(in ymd);

#if NO_INLINING
        [MethodImpl(MethodImplOptions.NoInlining)]
#endif
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

#if NO_INLINING
        [MethodImpl(MethodImplOptions.NoInlining)]
#endif
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

#if NO_INLINING
        [MethodImpl(MethodImplOptions.NoInlining)]
#endif
        static Yemoda Core(int y, int m, int d)
        {
            s_MinMaxYear.ValidateYearMonthDay(y, m, d);
            return new Yemoda(y, m, d);
        }
    }

#endif
    #endregion
}
