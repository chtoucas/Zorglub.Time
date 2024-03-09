// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Testing.Facts.Core;

using Zorglub.Testing.Data;

// REVIEW(fact): once IDaysInMonthDistribution becomes public, this should be
// part of SystemSchemaFacts.

internal static class DaysInMonthDistributionFacts
{
    public static void Test<TSchema>(TSchema schema, int commonYear, int leapYear)
        where TSchema : IDaysInMonthDistribution, ICalendricalKernel
    {
        if (leapYear != commonYear)
        {
            TestCore(schema, leapYear, leap: true);
            TestCore(schema, commonYear, leap: false);
        }
        else
        {
            TestCore(schema, commonYear, leap: false);
        }
    }

    public static void TestCore<TSchema>(TSchema schema, int y, bool leap)
        where TSchema : IDaysInMonthDistribution, ICalendricalKernel
    {
        Requires.NotNull(schema);

        // Sanity check.
        Assert.Equal(leap, schema.IsLeapYear(y));

        var daysInMonth = TSchema.GetDaysInMonthDistribution(leap);

        for (int m = 1; m <= daysInMonth.Length; m++)
        {
            Assert.Equal(
                (m, schema.CountDaysInMonth(y, m)),
                (m, daysInMonth[m - 1]));
        }
    }
}

internal abstract class DaysInMonthDistributionFacts<TSchema, TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TSchema : IDaysInMonthDistribution, ICalendricalKernel
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected DaysInMonthDistributionFacts(TSchema schema)
    {
        SchemaUT = schema ?? throw new ArgumentNullException(nameof(schema));
    }

    protected TSchema SchemaUT { get; }

    [Fact]
    public void GetDaysInMonthDistribution()
    {
        if (SampleLeapYear != SampleCommonYear)
        {
            DaysInMonthDistributionFacts.TestCore(SchemaUT, SampleCommonYear, leap: false);
            DaysInMonthDistributionFacts.TestCore(SchemaUT, SampleLeapYear, leap: true);
        }
        else
        {
            DaysInMonthDistributionFacts.TestCore(SchemaUT, SampleCommonYear, leap: false);
        }
    }
}
