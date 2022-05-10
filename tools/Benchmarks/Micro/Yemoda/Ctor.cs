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
    public void Yemo_GetYemodaAtUnchecked()
    {
        var ymd = Ym.GetYemodaAtUnchecked(Day);
        Consume(in ymd);
    }

    [Benchmark]
    public void Yemo_GetYemodaAt()
    {
        var ymd = Ym.GetYemodaAt(Day);
        Consume(in ymd);
    }
}
