// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Core;

using Zorglub.Testing.Data;
using Zorglub.Time.Core.Arithmetic;

/// <summary>
/// Provides facts about <see cref="CalendricalMath"/>.
/// </summary>
public abstract partial class CalendricalMathFacts<TSchema, TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TSchema : SystemSchema
    where TDataSet : ICalendricalDataSet, IAdvancedMathDataSet, ISingleton<TDataSet>
{
    protected CalendricalMathFacts(CalendricalMath math)
    {
        MathUT = math ?? throw new ArgumentNullException(nameof(math));
    }

    /// <summary>
    /// Gets the calculator under test.
    /// </summary>
    private protected CalendricalMath MathUT { get; }

    public static DataGroup<YemodaPairAnd<int>> AddYearsData => DataSet.AddYearsData;
    public static DataGroup<YemodaPairAnd<int>> AddMonthsData => DataSet.AddMonthsData;
}

public partial class CalendricalMathFacts<TSchema, TDataSet>
{
    [Theory, MemberData(nameof(AddMonthsData))]
    public void AddMonths(YemodaPairAnd<int> info)
    {
        var (start, end, months) = info;
        // Act
        var actual = MathUT.AddMonths(start, months, out _);
        // Assert
        Assert.Equal(end, actual);
    }

    [Theory, MemberData(nameof(AddMonthsData))]
    public void CountMonthsBetween(YemodaPairAnd<int> info)
    {
        var (start, end, months) = info;
        // Act
        int actual = MathUT.CountMonthsBetween(start, end, out _);
        // Assert
        Assert.Equal(months, actual);
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void AddYears(YemodaPairAnd<int> info)
    {
        var (start, end, years) = info;
        // Act
        var actual = MathUT.AddMonths(start, years, out _);
        // Act & Assert
        Assert.Equal(end, actual);
        Assert.Equal(years, MathUT.CountYearsBetween(start, end, out _));
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void CountYearsBetween(YemodaPairAnd<int> info)
    {
        var (start, end, years) = info;
        // Act
        int actual = MathUT.CountYearsBetween(start, end, out _);
        // Act & Assert
        Assert.Equal(years, actual);
    }
}
