// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Micro;

using Zorglub.Time.Core;

using static Zorglub.Time.Core.TemporalConstants;

/*
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1415 (20H2/October2020Update)
Intel Core2 Duo CPU E8500 3.16GHz, 1 CPU, 2 logical and 2 physical cores
.NET SDK=6.0.101
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT

|                             Method |     Mean |     Error |    StdDev | Rank |
|----------------------------------- |---------:|----------:|----------:|-----:|
|         DivideByNanosecondsPerHour | 1.460 ns | 0.0259 ns | 0.0242 ns |    I |
|       DivideByNanosecondsPerMinute | 1.884 ns | 0.0052 ns | 0.0044 ns |   II |
|   DivideByNanosecondsPerHour_Plain | 2.213 ns | 0.0185 ns | 0.0174 ns |  III |
| DivideByNanosecondsPerMinute_Plain | 2.214 ns | 0.0027 ns | 0.0023 ns |  III |
|          DivideByTicksPerDay_Plain | 2.216 ns | 0.0036 ns | 0.0034 ns |  III |
|                DivideByTicksPerDay | 2.220 ns | 0.0054 ns | 0.0051 ns |  III |
 */

public class TemporalArithmeticDivBenchmark : BenchmarkBase
{
    private const long NanosecondOfDay = (NanosecondsPerDay - 1) / 2;

    private static readonly long s_Ticks = DateTime.Now.Ticks;

    [Benchmark]
    public long DivideByTicksPerDay()
    {
        long value = TemporalArithmetic.DivideByTicksPerDay(s_Ticks);
        Consume(in value);
        return value;
    }

    [Benchmark]
    public long DivideByTicksPerDay_Plain()
    {
        long value = s_Ticks / TicksPerDay;
        Consume(in value);
        return value;
    }

    [Benchmark]
    public int DivideByNanosecondsPerHour()
    {
        int value = TemporalArithmetic.DivideByNanosecondsPerHour(NanosecondOfDay);
        Consume(in value);
        return value;
    }

    [Benchmark]
    public long DivideByNanosecondsPerHour_Plain()
    {
        long value = NanosecondOfDay / NanosecondsPerHour;
        Consume(in value);
        return value;
    }

    [Benchmark]
    public int DivideByNanosecondsPerMinute()
    {
        int value = TemporalArithmetic.DivideByNanosecondsPerMinute(NanosecondOfDay);
        Consume(in value);
        return value;
    }

    [Benchmark]
    public long DivideByNanosecondsPerMinute_Plain()
    {
        long value = NanosecondOfDay / NanosecondsPerMinute;
        Consume(in value);
        return value;
    }
}
