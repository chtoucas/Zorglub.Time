// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Micro;

using Zorglub.Time.Core.Schemas;

public class GregorianSchema_GetDateParts : GregorianSchemaBenchmark
{
    [Benchmark(Baseline = true)]
    public int GetDateParts()
    {
        CurrentSchema.GetDateParts(DaysSinceEpoch, out int y, out int m, out int d);
        Consume(in y);
        Consume(in m);
        return d;
    }

    [Benchmark]
    public int GetDateParts_Formulae()
    {
        var (y, m, d) = GregorianFormulae.GetDateParts(DaysSinceEpoch);
        Consume(in y);
        Consume(in m);
        return d;
    }

    [Benchmark]
    public int GetDateParts_Formulae64()
    {
        GregorianFormulae.GetDateParts(DaysSinceEpoch, out long y, out int m, out int d);
        Consume(in y);
        Consume(in m);
        return d;
    }

    [Benchmark]
    public int Hinnant()
    {
        HinnantFormulae.GetDateParts(DaysSinceEpoch, out int y, out int m, out int d);
        Consume(in y);
        Consume(in m);
        return d;
    }

    [Benchmark]
    public int Troesch()
    {
        TroeschFormulae.GetDateParts(DaysSinceEpoch, out int y, out int m, out int d);
        Consume(in y);
        Consume(in m);
        return d;
    }
}
