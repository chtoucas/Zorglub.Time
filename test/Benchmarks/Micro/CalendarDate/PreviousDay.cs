// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Micro;

using Zorglub.Time.Simple;

public class CalendarDate_PreviousDay
{
    private static readonly CalendarDate s_MiddleOfMonth = new(1777, 11, 19);
    private static readonly CalendarDate s_StartOfMonth = new(1777, 3, 1);
    private static readonly CalendarDate s_StartOfYear = new(1777, 1, 1);

    [Benchmark(Description = "PreviousDay() Middle")]
    public CalendarDate PreviousDay_Middle() => s_MiddleOfMonth.PreviousDay();

    [Benchmark(Description = "PreviousDay() StartOfMonth")]
    public CalendarDate PreviousDay_EndOfMonth() => s_StartOfMonth.PreviousDay();

    [Benchmark(Description = "PreviousDay() StartOfYear")]
    public CalendarDate PreviousDay_EndOfYear() => s_StartOfYear.PreviousDay();

    //
    // PlusDays(-1)
    //

    [Benchmark(Description = "PlusDays(-1) Middle")]
    public CalendarDate PlusDays_Middle() => s_MiddleOfMonth.PlusDays(-1);

    [Benchmark(Description = "PlusDays(-1) StartOfMonth")]
    public CalendarDate PlusDays_EndOfMonth() => s_StartOfMonth.PlusDays(-1);

    [Benchmark(Description = "PlusDays(-1) StartOfYear")]
    public CalendarDate PlusDays_EndOfYear() => s_StartOfYear.PlusDays(-1);
}
