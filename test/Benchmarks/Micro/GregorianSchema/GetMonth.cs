// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Micro;

public class GregorianSchema_GetMonth : GregorianSchemaBenchmark
{
    [Benchmark(Baseline = true)]
    public int GetMonth_()
    {
        int m = CurrentSchema.GetMonth(Year, DayOfYear, out int d);
        Consume(in m);
        return d;
    }

    [Benchmark]
    public int CalCal()
    {
        int m = CalCalFormulae.GetMonth(Year, DayOfYear, out int d);
        Consume(in m);
        return d;
    }
}
