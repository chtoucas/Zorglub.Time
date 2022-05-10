// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Micro;

using Zorglub.Time.Core;

public class Yemoda_StartOfYear : BenchmarkBase
{
    [Benchmark(Baseline = true)]
    public void AtStartOfYear()
    {
        var startOfYear = Yemoda.AtStartOfYear(Year);
        Consume(in startOfYear);
    }

    [Benchmark]
    public void Constructor()
    {
        var startOfYear = new Yemoda(Year, 1, 1);
        Consume(in startOfYear);
    }

    [Benchmark]
    public void Create()
    {
        var startOfYear = Yemoda.Create(Year, 1, 1);
        Consume(in startOfYear);
    }

    [Benchmark]
    public void StartOfYear()
    {
        var startOfYear = Ymd.StartOfYear;
        Consume(in startOfYear);
    }

    [Benchmark]
    public void Yemo_StartOfYear()
    {
        var startOfYear = Ym.StartOfYear;
        Consume(in startOfYear);
    }
}
