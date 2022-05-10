// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Micro;

public class Yemoda_Dtor : BenchmarkBase
{
    [Benchmark(Baseline = true)]
    public void Unpack()
    {
        Ymd.Unpack(out int y, out int m, out int d);
        Consume(in y);
        Consume(in m);
        Consume(in d);
    }

    [Benchmark]
    public void Dtor()
    {
        var (y, m, d) = Ymd;
        Consume(in y);
        Consume(in m);
        Consume(in d);
    }
}
