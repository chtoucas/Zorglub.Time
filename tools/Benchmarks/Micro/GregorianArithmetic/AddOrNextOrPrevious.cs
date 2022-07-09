// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Micro;

using Zorglub.Time.Core;
using Zorglub.Time.Core.Arithmetic;
using Zorglub.Time.Core.Schemas;

public class GregorianArithmetic_AddOrNextOrPrevious : BenchmarkBase
{
    private static readonly GregorianSchema s_Schema = new();
    private static readonly GregorianArithmetic s_Arithmetic = new(s_Schema, s_Schema.SupportedYears);
    private static readonly Yemoda s_StartOfMonth = new(1777, 1, 1);
    private static readonly Yemoda s_MiddleOfMonth = new(1777, 11, 22); // 22 + 7 > MinDaysInMonth
    private static readonly Yemoda s_EndOfMonth = new(1777, 11, 31);

    [Benchmark(Description = "Start NextDay()")]
    public Yemoda NextDay_Start() => s_Arithmetic.NextDay(s_StartOfMonth);
    [Benchmark(Description = "Start PreviousDay()")]
    public Yemoda PreviousDay_Start() => s_Arithmetic.PreviousDay(s_StartOfMonth);
    [Benchmark(Description = "Start AddDaysViaDayOfMonth(-1)")]
    public Yemoda AddDaysViaDayOfMonth_Start() => s_Arithmetic.AddDaysViaDayOfMonth(s_StartOfMonth, -1);

    [Benchmark(Description = "Middle NextDay()")]
    public Yemoda NextDay_Middle() => s_Arithmetic.NextDay(s_MiddleOfMonth);
    [Benchmark(Description = "Middle PreviousDay()")]
    public Yemoda PreviousDay_Middle() => s_Arithmetic.PreviousDay(s_MiddleOfMonth);
    [Benchmark(Description = "Middle AddDaysViaDayOfMonth(1)")]
    public Yemoda AddDaysViaDayOfMonth_Middle() => s_Arithmetic.AddDaysViaDayOfMonth(s_MiddleOfMonth, 1);

    [Benchmark(Description = "End NextDay()")]
    public Yemoda NextDay_End() => s_Arithmetic.NextDay(s_EndOfMonth);
    [Benchmark(Description = "End PreviousDay()")]
    public Yemoda PreviousDay_End() => s_Arithmetic.PreviousDay(s_EndOfMonth);
    [Benchmark(Description = "End AddDaysViaDayOfMonth(1)")]
    public Yemoda AddDaysViaDayOfMonth_End() => s_Arithmetic.AddDaysViaDayOfMonth(s_EndOfMonth, 1);
}
