// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Micro;

using Zorglub.Time.Core;

public class Yemoda_Ctor : BenchmarkBase
{
    [Benchmark(Baseline = true)]
    public void Ctor()
    {
        var ymd = new Yemoda(Year, Month, Day);
        Consume(in ymd);
    }

    [Benchmark]
    public void Create()
    {
        var ymd = Yemoda.Create(Year, Month, Day);
        Consume(in ymd);
    }

    [Benchmark]
    public void Yemo_GetDayOfMonthUnchecked()
    {
        var ymd = Ym.GetDayOfMonthUnchecked(Day);
        Consume(in ymd);
    }

    [Benchmark]
    public void Yemo_GetDayOfMonth()
    {
        var ymd = Ym.GetDayOfMonth(Day);
        Consume(in ymd);
    }
}
