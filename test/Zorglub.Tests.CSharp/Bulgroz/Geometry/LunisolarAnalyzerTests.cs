// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Geometry;

using Zorglub.Time.Core.Utilities;
using Zorglub.Time.Geometry.Discrete;
using Zorglub.Time.Geometry.Forms;

public sealed record YearMonthForm(int MonthsPerCycle, int YearsPerCycle, int Remainder)
    : CalendricalForm(MonthsPerCycle, YearsPerCycle, Remainder)
{
    /// <summary>
    /// Counts the number of consecutive months from the epoch to the first month of the specified
    /// year.
    /// </summary>
    public int GetStartOfYear(int c) => ValueAt(c);

    /// <summary>
    /// Obtains the year of the specified count of consecutive months since the epoch.
    /// </summary>
    public int GetYear(int monthsSinceEpoch, out int monthsSinceStartOfYear) =>
        Divide(monthsSinceEpoch, out monthsSinceStartOfYear);

    /// <summary>
    /// Obtains the number of months in the specified year.
    /// </summary>
    public int CountMonthsInYear(int c) => CodeAt(c);
}

public class LunisolarAnalyzerTests
{
    public static readonly YearMonthForm Form = new(49, 4, 0);

    public static readonly TheoryData<int, int> YearLengthData;
    public static readonly TheoryData<int, int> StartOfYearData;

#pragma warning disable CA1810 // Initialize reference type static fields inline (Performance) 👈 Tests

    static LunisolarAnalyzerTests()
    {
        int[] lens = { 12, 12, 12, 13, 12, 12, 12, 13, 12, 12, 12, 13 };

        YearLengthData = TheoryDataHelpers.ConvertToArrayData(lens);
        StartOfYearData = TheoryDataHelpers.ConvertToArrayData(ArrayHelpers.ConvertToCumulativeArray(lens));
    }

#pragma warning restore CA1810

    // Forme encondant le nombre de mois par année.
    [Fact]
    public void TryConvertCodeToForm()
    {
        var codes = new int[8] { 12, 12, 12, 13, /* Second cycle */ 12, 12, 12, 13 };
        var code = new CodeArray(codes);
        // Act & Assert
        Assert.True(TroeschAnalyzer.TryConvertCodeToForm(code, out var form));
        Assert.Equal(new(49, 4, 0), form);
    }

    [Fact]
    public void Form_Reverse() =>
        Assert.Equal(new CalendricalForm(4, 49, 3), Form.Reverse());

    [Theory, MemberData(nameof(YearLengthData))]
    public static void Form_CountMonthsInYear(int y, int monthsInYear) =>
        Assert.Equal(monthsInYear, Form.CountMonthsInYear(y));

    [Theory, MemberData(nameof(StartOfYearData))]
    public static void Form_GetStartOfYear(int y, int startOfYear) =>
        Assert.Equal(startOfYear, Form.GetStartOfYear(y));

    [Theory, MemberData(nameof(StartOfYearData))]
    public static void Form_GetYear(int y, int startOfYear)
    {
        Assert.Equal(y, Form.GetYear(startOfYear, out int monthsSinceStartOfYear));
        Assert.Equal(0, monthsSinceStartOfYear);
    }
}
