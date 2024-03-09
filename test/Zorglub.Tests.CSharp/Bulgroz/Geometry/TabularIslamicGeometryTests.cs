// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Bulgroz.Geometry;

using Zorglub.Time.Core.Schemas;

public sealed class AlgebraicTabularIslamicGeometryTests
    : IGeometricSchemaFacts<TabularIslamicDataSet>
{
    public AlgebraicTabularIslamicGeometryTests() : base(TabularIslamicGeometry.SchemaAlgebraicSystem) { }
}

public sealed class OrdinalTabularIslamicGeometryTests
    : SecondOrderSchemaFacts<TabularIslamicDataSet>
{
    private static readonly TabularIslamicSchema s_Schema = new();

    public OrdinalTabularIslamicGeometryTests() : base(TabularIslamicGeometry.SchemaOrdinalSystem) { }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // It does NOT work for the last month of a common year: MonthForm
        // is the form for leap years.
        if (m == 12 && !s_Schema.IsLeapYear(y)) { return; }

        // Act
        int actual = SecondOrderSchemaUT.CountDaysInMonth(m);
        // Assert
        Assert.Equal(info.DaysInMonth, actual);
    }
}
