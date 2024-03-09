// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Micro;

using Zorglub.Time.Core.Schemas;

public class GregorianSchema_GetYear : GregorianSchemaBenchmark
{
    [Benchmark(Baseline = true)]
    public int GetYear_Instance()
    {
        int n = CurrentSchema.GetYear(DaysSinceEpoch);
        Consume(in n);
        return n;
    }

    [Benchmark]
    public int GetYear_Formulae()
    {
        int n = GregorianFormulae.GetYear(DaysSinceEpoch);
        Consume(in n);
        return n;
    }

    [Benchmark]
    public long GetYear_Formulae64()
    {
        long n = GregorianFormulae.GetYear((long)DaysSinceEpoch);
        Consume(in n);
        return n;
    }

    [Benchmark]
    public int CalCal()
    {
        int n = CalCalFormulae.GetYear(DaysSinceEpoch);
        Consume(in n);
        return n;
    }
}
