// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Micro;

using Zorglub.Time.Core.Schemas;

/*
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1586 (21H2)
Intel Core i7-4500U CPU 1.80GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.201
  [Host]     : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
  DefaultJob : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT

|                         Method |     Mean |    Error |   StdDev | Ratio | RatioSD | Rank | Code Size |
|------------------------------- |---------:|---------:|---------:|------:|--------:|-----:|----------:|
| CountDaysSinceEpoch_NonVirtual | 12.88 ns | 0.101 ns | 0.095 ns |  0.98 |    0.02 |    I |     476 B |
|                        Troesch | 12.89 ns | 0.118 ns | 0.110 ns |  0.98 |    0.02 |    I |     478 B |
|   CountDaysSinceEpoch_Formulae | 13.00 ns | 0.117 ns | 0.104 ns |  0.99 |    0.02 |    I |     476 B |
|                     Troesch400 | 13.02 ns | 0.075 ns | 0.067 ns |  0.99 |    0.02 |    I |     507 B |
|            CountDaysSinceEpoch | 13.16 ns | 0.222 ns | 0.207 ns |  1.00 |    0.00 |    I |     476 B |
|                        Hinnant | 13.52 ns | 0.086 ns | 0.081 ns |  1.03 |    0.02 |   II |     511 B |
| CountDaysSinceEpoch_Formulae64 | 15.84 ns | 0.120 ns | 0.112 ns |  1.20 |    0.02 |  III |     517 B |

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1586 (20H2/October2020Update)
Intel Core2 Duo CPU E8500 3.16GHz, 1 CPU, 2 logical and 2 physical cores
.NET SDK=6.0.201
  [Host]     : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
  DefaultJob : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT

|                         Method |     Mean |    Error |   StdDev | Ratio | Rank |
|------------------------------- |---------:|---------:|---------:|------:|-----:|
|   CountDaysSinceEpoch_Formulae | 15.51 ns | 0.021 ns | 0.019 ns |  0.82 |    I |
| CountDaysSinceEpoch_NonVirtual | 15.51 ns | 0.006 ns | 0.006 ns |  0.82 |    I |
|                        Troesch | 15.52 ns | 0.023 ns | 0.021 ns |  0.82 |    I |
|                     Troesch400 | 16.15 ns | 0.028 ns | 0.026 ns |  0.85 |   II |
|                        Hinnant | 16.47 ns | 0.014 ns | 0.013 ns |  0.87 |  III |
| CountDaysSinceEpoch_Formulae64 | 17.73 ns | 0.016 ns | 0.015 ns |  0.93 |   IV |
|            CountDaysSinceEpoch | 19.02 ns | 0.017 ns | 0.016 ns |  1.00 |    V |
 */
// Comments:
// - Troesch() is faster than CountDaysSinceEpoch() only because the later
//   is virtual.
// - I don't know why CountDaysSinceEpochImpl64() is so slow.

[CLSCompliant(false)]
[DisassemblyDiagnoser]
public class GregorianSchema_CountDaysSinceEpoch : GregorianSchemaBenchmark
{
    [Benchmark(Baseline = true)]
    public int CountDaysSinceEpoch()
    {
        int n = CurrentSchema.CountDaysSinceEpoch(Year, Month, Day);
        Consume(in n);
        return n;
    }

    [Benchmark]
    public int CountDaysSinceEpoch_Formulae()
    {
        int n = GregorianFormulae.CountDaysSinceEpoch(Year, Month, Day);
        Consume(in n);
        return n;
    }

    [Benchmark]
    public long CountDaysSinceEpoch_Formulae64()
    {
        long n = GregorianFormulae.CountDaysSinceEpoch((long)Year, Month, Day);
        Consume(in n);
        return n;
    }

    [Benchmark]
    public int CountDaysSinceEpoch_NonVirtual()
    {
        int n = TroeschFormulae.CountDaysSinceEpochNonVirtual(Year, Month, Day);
        Consume(in n);
        return n;
    }

    [Benchmark]
    public int Hinnant()
    {
        int n = HinnantFormulae.CountDaysSinceEpoch(Year, Month, Day);
        Consume(in n);
        return n;
    }

    [Benchmark]
    public int Troesch()
    {
        int n = TroeschFormulae.CountDaysSinceEpoch(Year, Month, Day);
        Consume(in n);
        return n;
    }

    [Benchmark]
    public int Troesch400()
    {
        int n = TroeschFormulae.CountDaysSinceEpoch400(Year, Month, Day);
        Consume(in n);
        return n;
    }
}
