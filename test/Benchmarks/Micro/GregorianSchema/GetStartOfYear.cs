// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Micro;

using Zorglub.Time.Core.Schemas;

// REVIEW: [Perf] why is GetStartOfYear() faster than GetStartOfYear_Formulae()?

// Current results on my computer:
//  1.90 ns (1.00) * GetStartOfYear
//  2.20 ns (1.15)   CountDaysSinceEpoch_Formulae
//          (1.15)   GetStartOfYear_Formulae64
//  2.25 ns (1.20)   GetStartOfYear_Formulae
//  4.75 ns (2.50)   CountDaysSinceEpoch
//  7.80 ns (4.15)   CountDaysSinceEpoch_Formulae64

public class GregorianSchema_GetStartOfYear : GregorianSchemaBenchmark
{
    [Benchmark(Baseline = true)]
    public int GetStartOfYear()
    {
        int n = CurrentSchema.GetStartOfYear(Year);
        Consume(in n);
        return n;
    }

    [Benchmark]
    public int GetStartOfYear_Formulae()
    {
        int n = GregorianFormulae.GetStartOfYear(Year);
        Consume(in n);
        return n;
    }

    [Benchmark]
    public long GetStartOfYear_Formulae64()
    {
        long n = GregorianFormulae.GetStartOfYear((long)Year);
        Consume(in n);
        return n;
    }

    [Benchmark]
    public int CountDaysSinceEpoch()
    {
        int n = CurrentSchema.CountDaysSinceEpoch(Year, 1, 1);
        Consume(in n);
        return n;
    }

    [Benchmark]
    public int CountDaysSinceEpoch_Formulae()
    {
        int n = GregorianFormulae.CountDaysSinceEpoch(Year, 1, 1);
        Consume(in n);
        return n;
    }

    [Benchmark]
    public long CountDaysSinceEpoch_Formulae64()
    {
        long n = GregorianFormulae.CountDaysSinceEpoch((long)Year, 1, 1);
        Consume(in n);
        return n;
    }
}
