// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Micro;

using Zorglub.Time.Core.Schemas;

public class GregorianSchema_CountDaysInMonth : GregorianSchemaBenchmark
{
    [Benchmark(Baseline = true)]
    public int CountDaysInMonth()
    {
        int n = CurrentSchema.CountDaysInMonth(Year, Month);
        Consume(in n);
        return n;
    }

    [Benchmark]
    public int CountDaysInMonth_Formulae()
    {
        int n = GregorianFormulae.CountDaysInMonth(Year, Month);
        Consume(in n);
        return n;
    }

    [Benchmark]
    public long CountDaysInMonth_Formulae64()
    {
        long n = GregorianFormulae.CountDaysInMonth((long)Year, Month);
        Consume(in n);
        return n;
    }
}
