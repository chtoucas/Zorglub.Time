// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Core;

using Zorglub.Testing.Data;
using Zorglub.Time.Core.Intervals;

/// <summary>
/// Provides facts about <see cref="ArchetypalSchema"/>.
/// </summary>
// NB: the trait do not really work right now. We have to copy them to the
// derived classes otherwise they are ignored. It's a problem with the discoverers.
[TestPerformance(TestPerformance.Slow)]
// REVIEW(fact): temporary trait, too slow and ArchetypalSchema is not part of the main assembly.
[TestExcludeFrom(TestExcludeFrom.CodeCoverage)]
public abstract class ArchetypalSchemaFacts<TDataSet> :
    ICalendricalSchemaPlusFacts<ArchetypalSchema, TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected ArchetypalSchemaFacts(ArchetypalSchema schema) : base(schema) { }

    // REVIEW(fact): at some point we should not bypass these tests.
    [Fact] public sealed override void Algorithm_Prop() { }
    [Fact] public sealed override void Family_Prop() { }
    [Fact] public sealed override void PeriodicAdjustments_Prop() { }
    [Fact] public sealed override void PreValidator_Prop() { }
    [Fact] public sealed override void Arithmetic_Prop() { }
    [Fact] public sealed override void IsRegular() { }

    [Fact]
    public override void SupportedYears_Prop() =>
        Assert.Equal(Range.Create(-9998, 9999), SchemaUT.SupportedYears);

    [Fact]
    public override void KernelDoesNotOverflow()
    {
        // Min/MaxYear? We can't take Int32.Min/MaxValue, that's why we override
        // the base method... Yemoda.Min/MaxValue is something not too small that
        // should work most of the time.
        // See also SystemSchemaFacts.KernelDoesNotOverflow()

        _ = SchemaUT.IsLeapYear(Yemoda.MinYear);
        _ = SchemaUT.IsLeapYear(Yemoda.MaxYear);

        _ = SchemaUT.CountMonthsInYear(Int32.MinValue);
        _ = SchemaUT.CountMonthsInYear(Int32.MaxValue);

        _ = SchemaUT.CountDaysInYear(Yemoda.MinYear);
        _ = SchemaUT.CountDaysInYear(Yemoda.MaxYear);

        for (int m = 1; m <= MaxMonth; m++)
        {
            _ = SchemaUT.IsIntercalaryMonth(Int32.MinValue, m);
            _ = SchemaUT.IsIntercalaryMonth(Int32.MaxValue, m);

            _ = SchemaUT.CountDaysInMonth(Yemoda.MinYear, m);
            _ = SchemaUT.CountDaysInMonth(Yemoda.MaxYear, m);

            for (int d = 1; d <= MaxDay; d++)
            {
                _ = SchemaUT.IsIntercalaryDay(Int32.MinValue, m, d);
                _ = SchemaUT.IsIntercalaryDay(Int32.MaxValue, m, d);

                _ = SchemaUT.IsSupplementaryDay(Int32.MinValue, m, d);
                _ = SchemaUT.IsSupplementaryDay(Int32.MaxValue, m, d);
            }
        }
    }
}
