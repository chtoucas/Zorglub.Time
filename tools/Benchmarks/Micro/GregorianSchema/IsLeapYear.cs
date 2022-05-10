// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks.Micro;

using Zorglub.Time.Core.Schemas;

public class GregorianSchema_IsLeapYear : GregorianSchemaBenchmark
{
    [Benchmark]
    public bool IsLeapYear()
    {
        bool b = CurrentSchema.IsLeapYear(Year);
        Consume(in b);
        return b;
    }

    [Benchmark]
    public bool IsLeapYear_Formulae()
    {
        bool b = GregorianFormulae.IsLeapYear(Year);
        Consume(in b);
        return b;
    }

    [Benchmark]
    public bool IsLeapYear_Formulae64()
    {
        bool b = GregorianFormulae.IsLeapYear((long)Year);
        Consume(in b);
        return b;
    }

    [Benchmark]
    public int CountDaysInYear()
    {
        int n = CurrentSchema.CountDaysInYear(Year);
        Consume(in n);
        return n;
    }

    [Benchmark]
    public int CountDaysInYear_Formulae()
    {
        int n = GregorianFormulae.CountDaysInYear(Year);
        Consume(in n);
        return n;
    }

    [Benchmark]
    public int CountDaysInYear_Formulae64()
    {
        int n = GregorianFormulae.CountDaysInYear((long)Year);
        Consume(in n);
        return n;
    }
}
