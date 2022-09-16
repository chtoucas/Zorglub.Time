// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Micro;

using Zorglub.Time.Core.Schemas;

public class GregorianSchema_GetYear : GregorianSchemaBenchmark
{
#pragma warning disable CA1721 // Property names should not match get methods (Naming)
    [Benchmark(Baseline = true)]
    public int GetYear()
    {
        int n = CurrentSchema.GetYear(DaysSinceEpoch);
        Consume(in n);
        return n;
    }
#pragma warning restore CA1721

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
