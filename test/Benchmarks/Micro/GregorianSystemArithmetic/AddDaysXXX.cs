// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Micro;

using Zorglub.Time.Core;
using Zorglub.Time.Core.Arithmetic;
using Zorglub.Time.Core.Schemas;

// With short values of "days",
//  1. AddDaysViaDayOfMonth()
//  2. AddDays()
// With longer values of "days",
//  3. AddDays(300) via AddDaysViaDayOfYear()
//  4. AddDays(400) via DayNumber

public class GregorianSystemArithmetic_AddDaysXXX : BenchmarkBase
{
    private static readonly GregorianSchema s_Schema = new();
    private static readonly GregorianSystemArithmetic s_Arithmetic = new(s_Schema, s_Schema.SupportedYears);
    private static readonly Yemoda s_StartOfMonth = new(1777, 1, 1);
    private static readonly Yemoda s_MiddleOfMonth = new(1777, 11, 22); // 22 + 7 > MinDaysInMonth
    private static readonly Yemoda s_EndOfMonth = new(1777, 11, 31);

    [Benchmark(Description = "Start AddDays(-7)")]
    public Yemoda AddDays_Start() => s_Arithmetic.AddDays(s_StartOfMonth, -7);
    [Benchmark(Description = "Start AddDaysViaDayOfMonth(-7)")]
    public Yemoda AddDaysViaDayOfMonth_Start() => s_Arithmetic.AddDaysViaDayOfMonth(s_StartOfMonth, -7);

    [Benchmark(Description = "Middle AddDays(7)")]
    public Yemoda AddDays_Middle() => s_Arithmetic.AddDays(s_MiddleOfMonth, 7);
    [Benchmark(Description = "Middle AddDaysViaDayOfMonth(7)")]
    public Yemoda AddDaysViaDayOfMonth_Middle() => s_Arithmetic.AddDaysViaDayOfMonth(s_MiddleOfMonth, 7);

    [Benchmark(Description = "End AddDays(7)")]
    public Yemoda AddDays_End() => s_Arithmetic.AddDays(s_EndOfMonth, 7);
    [Benchmark(Description = "End AddDaysViaDayOfMonth(7)")]
    public Yemoda AddDaysViaDayOfMonth_End() => s_Arithmetic.AddDaysViaDayOfMonth(s_EndOfMonth, 7);

    [Benchmark(Description = "AddDays(300)")]
    public Yemoda AddDays_300() => s_Arithmetic.AddDays(s_MiddleOfMonth, 300);

    [Benchmark(Description = "AddDays(400)")]
    public Yemoda AddDays_400() => s_Arithmetic.AddDays(s_MiddleOfMonth, 400);
}
