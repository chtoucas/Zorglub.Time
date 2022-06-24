// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Geometry;

using Zorglub.Time.Geometry;
using Zorglub.Time.Geometry.Discrete;
using Zorglub.Time.Geometry.Forms;

public sealed record YearMonthForm(int MonthsPerCycle, int YearsPerCycle, int Remainder)
    : CalendricalForm(MonthsPerCycle, YearsPerCycle, Remainder)
{
    /// <summary>
    /// Counts the number of consecutive months from the epoch to the first
    /// month of the specified year.
    /// </summary>
    public int GetStartOfYear(int c) => ValueAt(c);

    /// <summary>
    /// Obtains the year of the specified count of consecutive months since
    /// the epoch.
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
    /// <summary>12, 12, 12, 13.</summary>
    public static readonly TheoryData<int, int> YearLengths = new()
    {
        { 0, 12 },
        { 1, 12 },
        { 2, 12 },
        { 3, 13 },
    };

    // Forme encondant le nombre de mois par année.
    [Fact]
    public void YearForm()
    {
        var codes = new int[8] { 12, 12, 12, 13, /* Second cycle */ 12, 12, 12, 13 };
        var code = new CodeArray(codes);
        // Act & Assert
        Assert.True(TroeschAnalyzer.TryConvertCodeToForm(code, out var formA));
        Assert.Equal(new(49, 4, 0), formA);
    }

}
