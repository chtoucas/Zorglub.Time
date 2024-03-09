// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Micro;

using Zorglub.Time.Core;

public class YemoBenchmark : GJBenchmarkBase
{
    [Benchmark]
    public Yemo Ctor()
    {
        var ym = new Yemo(Year, Month);
        Consume(in ym);
        return ym;
    }

    [Benchmark]
    public Yemo Create()
    {
        var ym = Yemo.Create(Year, Month);
        Consume(in ym);
        return ym;
    }

    [Benchmark]
    public Yemo Yemoda_Yemo()
    {
        var ym = Ymd.Yemo;
        Consume(in ym);
        return ym;
    }

    [Benchmark]
    public int Unpack()
    {
        Ym.Unpack(out int y, out int m);
        Consume(in y);
        return m;
    }

    [Benchmark]
    public int Dtor()
    {
        var (y, m) = Ym;
        Consume(in y);
        return m;
    }
}
