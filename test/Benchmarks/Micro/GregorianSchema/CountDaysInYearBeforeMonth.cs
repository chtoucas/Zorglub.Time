// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Micro;

using Zorglub.Time.Core.Schemas;

public class GregorianSchema_CountDaysInYearBeforeMonth : GregorianSchemaBenchmark
{
    [Benchmark(Baseline = true)]
    public int CountDaysInYearBeforeMonth()
    {
        int n = CurrentSchema.CountDaysInYearBeforeMonth(Year, Month);
        Consume(in n);
        return n;
    }

    [Benchmark]
    public int CountDaysInYearBeforeMonth_Formulae()
    {
        int n = GregorianFormulae.CountDaysInYearBeforeMonth(Year, Month);
        Consume(in n);
        return n;
    }

    [Benchmark]
    public long CountDaysInYearBeforeMonth_Formulae64()
    {
        long n = GregorianFormulae.CountDaysInYearBeforeMonth((long)Year, Month);
        Consume(in n);
        return n;
    }

    [Benchmark]
    public int CalCal()
    {
        int n = CalCalFormulae.CountDaysInYearBeforeMonth(Year, Month);
        Consume(in n);
        return n;
    }

    [Benchmark]
    public int Hinnant()
    {
        int n = HinnantFormulae.CountDaysInYearBeforeMonth(Year, Month);
        Consume(in n);
        return n;
    }
}
