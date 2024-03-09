// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Testing.Facts.Geometry;

using Zorglub.Testing.Data;
using Zorglub.Time.Geometry.Schemas;

/// <summary>
/// Provides facts about <see cref="IGeometricSchema"/>.
/// </summary>
public abstract class IGeometricSchemaFacts<TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected IGeometricSchemaFacts(IGeometricSchema schema)
    {
        SchemaUT = schema ?? throw new ArgumentNullException(nameof(schema));
    }

    /// <summary>
    /// Gets the schema under test.
    /// </summary>
    protected IGeometricSchema SchemaUT { get; }

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void CountDaysSinceEpoch(DaysSinceEpochInfo info)
    {
        var (daysSinceEpoch, y, m, d) = info;

        // Act
        int actual = SchemaUT.CountDaysSinceEpoch(y, m, d);
        // Assert
        Assert.Equal(daysSinceEpoch, actual);
    }

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void GetDateParts(DaysSinceEpochInfo info)
    {
        var (daysSinceEpoch, y, m, d) = info;

        // Act
        SchemaUT.GetDateParts(daysSinceEpoch, out int year, out int month, out int day);
        // Assert
        Assert.Equal(y, year);
        Assert.Equal(m, month);
        Assert.Equal(d, day);
    }
}
