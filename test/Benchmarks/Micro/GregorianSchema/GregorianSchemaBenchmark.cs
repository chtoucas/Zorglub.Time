﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Micro;

using Zorglub.Bulgroz.Formulae;
using Zorglub.Time.Core.Schemas;

public abstract class GregorianSchemaBenchmark : GJBenchmarkBase
{
    protected static GregorianSchema CurrentSchema { get; } = new();

    // Alternatives.
    protected static readonly GregorianCalCalFormulae CalCalFormulae = new();
    protected static readonly GregorianHinnantFormulae HinnantFormulae = new();
    protected static readonly GregorianTroeschFormulae TroeschFormulae = new();
}
