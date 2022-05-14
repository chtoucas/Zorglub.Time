// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Core;

using Zorglub.Testing.Data;

/// <summary>
/// Provides basic facts about <see cref="ArchetypalSchema"/>.
/// </summary>
[TestPerformance(TestPerformance.SlowGroup)]
public abstract class ArchetypalSchemaBasicFacts<TDataSet> : ICalendricalSchemaBasicFacts<ArchetypalSchema, TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected ArchetypalSchemaBasicFacts(ArchetypalSchema schema) : base(schema)
    {
        TestGetYearAnyway = true;
        TestGetMonthAnyway = true;
    }

    public sealed override void Algorithm_Prop() { }
    public sealed override void Family_Prop() { }
    public sealed override void PeriodicAdjustments_Prop() { }
    public sealed override void PreValidator_Prop() { }
    public sealed override void Arithmetic_Prop() { }
    public sealed override void SupportedYears_Prop() { }

    public sealed override void IsRegular() { }

    public sealed override void KernelDoesNotOverflow() { }
}
