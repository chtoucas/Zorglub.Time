// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Micro;

using Zorglub.Time.Core;

public class Yemoda_StartOfMonth : BenchmarkBase
{
    [Benchmark(Baseline = true)]
    public void AtStartOfMonth()
    {
        var startOfMonth = Yemoda.AtStartOfMonth(Year, Month);
        Consume(in startOfMonth);
    }

    [Benchmark]
    public void Constructor()
    {
        var startOfMonth = new Yemoda(Year, Month, 1);
        Consume(in startOfMonth);
    }

    [Benchmark]
    public void Create()
    {
        var startOfMonth = Yemoda.Create(Year, Month, 1);
        Consume(in startOfMonth);
    }

    [Benchmark]
    public void StartOfMonth()
    {
        var startOfMonth = Ymd.StartOfMonth;
        Consume(in startOfMonth);
    }

    [Benchmark]
    public void Yemo_StartOfMonth()
    {
        var startOfMonth = Ym.StartOfMonth;
        Consume(in startOfMonth);
    }
}
