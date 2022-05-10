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

|                               Method |     Mean |     Error |    StdDev | Rank |
|------------------------------------- |---------:|----------:|----------:|-----:|
|         MultiplyByNanosecondsPerHour | 1.680 ns | 0.0057 ns | 0.0051 ns |    I |
|       MultiplyByNanosecondsPerMinute | 2.213 ns | 0.0025 ns | 0.0019 ns |   II |
| MultiplyByNanosecondsPerMinute_Plain | 2.215 ns | 0.0015 ns | 0.0013 ns |   II |
|                MultiplyByTicksPerDay | 2.215 ns | 0.0014 ns | 0.0012 ns |   II |
|          MultiplyByTicksPerDay_Plain | 2.216 ns | 0.0040 ns | 0.0037 ns |   II |
|   MultiplyByNanosecondsPerHour_Plain | 2.217 ns | 0.0090 ns | 0.0084 ns |   II |
 */

public class TemporalArithmeticMulBenchmark : BenchmarkBase
{
    private const int DaysSinceZero = 1_234_567;
    private const int HourOfDay = (HoursPerDay - 1) / 2;
    private const int MinuteOfDay = (MinutesPerDay - 1) / 2;

    [Benchmark]
    public long MultiplyByTicksPerDay()
    {
        long value = TemporalArithmetic.MultiplyByTicksPerDay(DaysSinceZero);
        Consume(in value);
        return value;
    }

    [Benchmark]
    public long MultiplyByTicksPerDay_Plain()
    {
        long value = TicksPerDay * DaysSinceZero;
        Consume(in value);
        return value;
    }

    [Benchmark]
    public long MultiplyByNanosecondsPerHour()
    {
        long value = TemporalArithmetic.MultiplyByNanosecondsPerHour(HourOfDay);
        Consume(in value);
        return value;
    }

    [Benchmark]
    public long MultiplyByNanosecondsPerHour_Plain()
    {
        long value = NanosecondsPerHour * HourOfDay;
        Consume(in value);
        return value;
    }

    [Benchmark]
    public long MultiplyByNanosecondsPerMinute()
    {
        long value = TemporalArithmetic.MultiplyByNanosecondsPerMinute(MinuteOfDay);
        Consume(in value);
        return value;
    }

    [Benchmark]
    public long MultiplyByNanosecondsPerMinute_Plain()
    {
        long value = NanosecondsPerMinute * MinuteOfDay;
        Consume(in value);
        return value;
    }
}
