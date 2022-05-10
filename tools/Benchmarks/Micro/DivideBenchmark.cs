// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Micro;

using System.Runtime.CompilerServices;
using System.Security.Cryptography;

using Zorglub.Time.Core.Utilities;

// Tout n'est pas comparable.
// - "Div Mul" et "Div Mod"
// - "/ 31" et "(uint) / 31"

/*
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1415 (20H2/October2020Update)
Intel Core2 Duo CPU E8500 3.16GHz, 1 CPU, 2 logical and 2 physical cores
.NET SDK=6.0.101
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT

|        Method |     Mean |     Error |    StdDev | Ratio | Rank | Code Size |
|-------------- |---------:|----------:|----------:|------:|-----:|----------:|
|  MathN.Divide | 1.898 ns | 0.0023 ns | 0.0020 ns |  0.86 |    I |      33 B |
|  MathZ.Divide | 1.898 ns | 0.0021 ns | 0.0018 ns |  0.86 |    I |      33 B |
|  MathU.Divide | 1.899 ns | 0.0018 ns | 0.0014 ns |  0.86 |    I |      33 B |
| '(uint) / 31' | 1.900 ns | 0.0023 ns | 0.0020 ns |  0.86 |    I |      32 B |
|        '/ 31' | 2.216 ns | 0.0051 ns | 0.0048 ns |  1.00 |   II |      32 B |
|   Math.DivRem | 2.217 ns | 0.0060 ns | 0.0056 ns |  1.00 |   II |      33 B |
|     'Div Mul' | 4.434 ns | 0.0068 ns | 0.0064 ns |  2.00 |  III |      75 B |
|     'Div Mod' | 6.108 ns | 0.0084 ns | 0.0074 ns |  2.76 |   IV |      74 B |

With PATCH_DIVREM disabled.

|        Method |     Mean |     Error |    StdDev | Ratio | Rank | Code Size |
|-------------- |---------:|----------:|----------:|------:|-----:|----------:|
|  MathU.Divide | 1.897 ns | 0.0033 ns | 0.0028 ns |  0.86 |    I |      33 B |
|  MathN.Divide | 1.899 ns | 0.0060 ns | 0.0056 ns |  0.86 |    I |      33 B |
|  MathZ.Divide | 1.899 ns | 0.0062 ns | 0.0058 ns |  0.86 |    I |      30 B |
| '(uint) / 31' | 1.900 ns | 0.0028 ns | 0.0025 ns |  0.86 |    I |      32 B |
|   Math.DivRem | 2.214 ns | 0.0032 ns | 0.0028 ns |  1.00 |   II |      33 B |
|        '/ 31' | 2.216 ns | 0.0061 ns | 0.0057 ns |  1.00 |   II |      32 B |
|     'Div Mul' | 4.432 ns | 0.0074 ns | 0.0069 ns |  2.00 |  III |      75 B |
|     'Div Mod' | 6.117 ns | 0.0125 ns | 0.0117 ns |  2.76 |   IV |      74 B |
 */

[DisassemblyDiagnoser]
public class DivideBenchmark : BenchmarkBase
{
    private const int Dividor = 31;

    private static readonly int s_Dividend = RandomNumberGenerator.GetInt32(1000, 1_000_000_000);

    #region Division by a constant

    [Benchmark(Description = "/ 31")]
    public int Div()
    {
        int q = s_Dividend / Dividor;
        Consume(in q);
        return q;
    }

    [Benchmark(Description = "(uint) / 31")]
    public int Div_Unsigned()
    {
        int q = (int)(uint)s_Dividend / Dividor;
        Consume(in q);
        return q;
    }

    #endregion
    #region Muliply or Modulo

    [Benchmark(Description = "Div Mul")]
    public int Div_Mul()
    {
        int q = DivMulImpl(s_Dividend, Dividor, out int r);
        Consume(in q);
        return r;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int DivMulImpl(int m, int n, out int r)
    {
        int q = m / n;
        r = m - q * n;
        return q;
    }

    [Benchmark(Description = "Div Mod")]
    public int Div_Mod()
    {
        int q = DivModImpl(s_Dividend, Dividor, out int r);
        Consume(in q);
        return r;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int DivModImpl(int m, int n, out int r)
    {
        r = m % n;
        return m / n;
    }

    #endregion

    [Benchmark(Description = "Math.DivRem", Baseline = true)]
    public int Math_DivRem()
    {
        int q = Math.DivRem(s_Dividend, Dividor, out int r);
        Consume(in q);
        return r;
    }

    [Benchmark(Description = "MathN.Divide")]
    public int MathN_Divide()
    {
        int q = MathN.Divide(s_Dividend, Dividor, out int r);
        Consume(in q);
        return r;
    }

    [CLSCompliant(false)]
    [Benchmark(Description = "MathU.Divide")]
    public uint MathU_Divide()
    {
        uint q = MathU.Divide((uint)s_Dividend, Dividor, out uint r);
        Consume(in q);
        return r;
    }

    [Benchmark(Description = "MathZ.Divide")]
    public int MathZ_Divide()
    {
        int q = MathZ.Divide(s_Dividend, Dividor, out int r);
        Consume(in q);
        return r;
    }
}
