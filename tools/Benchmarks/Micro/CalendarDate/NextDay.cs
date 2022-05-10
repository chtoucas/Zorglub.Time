// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Micro;

using Zorglub.Time.Simple;

public class CalendarDate_NextDay
{
    private static readonly CalendarDate s_MiddleOfMonth = new(1777, 11, 19);
    private static readonly CalendarDate s_EndOfMonth = new(1777, 1, 31);
    private static readonly CalendarDate s_EndOfYear = new(1777, 12, 31);

    [Benchmark(Description = "Middle     NextDay()")]
    public CalendarDate NextDay_Middle() => s_MiddleOfMonth.NextDay();

    [Benchmark(Description = "EndOfMonth NextDay()")]
    public CalendarDate NextDay_EndOfMonth() => s_EndOfMonth.NextDay();

    [Benchmark(Description = "EndOfYear  NextDay()")]
    public CalendarDate NextDay_EndOfYear() => s_EndOfYear.NextDay();

    //
    // PlusDays(1)
    //

    [Benchmark(Description = "Middle     PlusDays(1)")]
    public CalendarDate PlusDays_Middle() => s_MiddleOfMonth.PlusDays(1);

    [Benchmark(Description = "EndOfMonth PlusDays(1)")]
    public CalendarDate PlusDays_EndOfMonth() => s_EndOfMonth.PlusDays(1);

    [Benchmark(Description = "EndOfYear  PlusDays(1)")]
    public CalendarDate PlusDays_EndOfYear() => s_EndOfYear.PlusDays(1);
}
