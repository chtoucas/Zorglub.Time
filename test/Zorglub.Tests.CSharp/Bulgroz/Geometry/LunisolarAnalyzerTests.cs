// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Geometry;

using System.Diagnostics.Contracts;

using Zorglub.Time.Core;
using Zorglub.Time.Core.Utilities;
using Zorglub.Time.Geometry.Discrete;
using Zorglub.Time.Geometry.Forms;

// YearMonthForm ne devrait pas dériver de CalendricalForm qui utilise l'unité
// jour (YearMonthForm utilise l'unité mois).
public sealed record YearMonthForm(int MonthsPerCycle, int YearsPerCycle, int Remainder)
    : CalendricalForm(MonthsPerCycle, YearsPerCycle, Remainder)
{
    /// <summary>
    /// Counts the number of consecutive months from the epoch to the first month of the specified
    /// year.
    /// </summary>
    [Pure]
    public int GetStartOfYear(int y) => ValueAt(y);

    /// <summary>
    /// Obtains the year of the specified count of consecutive months since the epoch.
    /// </summary>
    [Pure]
    public int GetYear(int monthsSinceEpoch, out int m0) => Divide(monthsSinceEpoch, out m0);

    /// <summary>
    /// Obtains the number of months in the specified year.
    /// </summary>
    [Pure]
    public int CountMonthsInYear(int c) => CodeAt(c);

    public Yemo GetMonthParts(int monthsSinceEpoch)
    {
        int y = GetYear(monthsSinceEpoch, out int m0);
        return new Yemo(y, 1 + m0);
    }

    [Pure]
    public YearMonthForm Normalize()
    {
        var (y0, m0, d0) = Origin;
        if ((m0, d0) != (1, 1)) ThrowHelpers.InvalidOperation();

        int monthsFromEpochToOrigin = CountMonthsFromEpochToStartOfYear(y0);

        return this with
        {
            Remainder = Remainder - MonthsPerCycle * y0 + YearsPerCycle * monthsFromEpochToOrigin,
            Origin = Epoch
        };
    }

    [Pure]
    public int CountMonthsFromEpochToStartOfYear(int y)
    {
        int y0 = Origin.Year;

        return GetStartOfYear(y - y0) - GetStartOfYear(1 - y0);
    }

}

public class LunisolarYearMonthFormTests
{
    /// <summary>Represents the year form (49, 4, 0).</summary>
    /// <remarks>
    /// <para>This form encodes the sequence: 12, 12, 12, 13.</para>
    /// <para>This form origin is 01/01/0000.</para>
    /// </remarks>
    public static readonly YearMonthForm Form = new(49, 4, 0) { Origin = new Yemoda(0, 1, 1) };

    /// <summary>Represents the year form (49, 4, -48).</summary>
    /// <remarks>
    /// <para>This form encodes the sequence: 12, 12, 12, 13.</para>
    /// <para>This form origin is 01/01/0001.</para>
    /// </remarks>
    public static readonly YearMonthForm NormalForm = new(49, 4, -48);

    /// <summary>Represents the year form (49, 4, -49).</summary>
    public static readonly YearMonthForm OrdinalForm = new(49, 4, -49);

    // Year length in months.
    public static readonly TheoryData<int, int> YearLengthData;
    public static readonly TheoryData<int, int> StartOfYearData;

#pragma warning disable CA1810 // Initialize reference type static fields inline (Performance) 👈 Tests

    static LunisolarYearMonthFormTests()
    {
        int[] lens = {
            12, 12, 12, 13,
            12, 12, 12, 13,
            12, 12, 12, 13 };

        YearLengthData = TheoryDataHelpers.ConvertToArrayData(lens);
        StartOfYearData = TheoryDataHelpers.ConvertToArrayData(ArrayHelpers.ConvertToCumulativeArray(lens));
    }

#pragma warning restore CA1810

    [Fact]
    public static void OrdinalForm_CountMonthsInYear()
    {
        Assert.Equal(13, OrdinalForm.CountMonthsInYear(0));
        Assert.Equal(12, OrdinalForm.CountMonthsInYear(1));
        Assert.Equal(12, OrdinalForm.CountMonthsInYear(2));
        Assert.Equal(12, OrdinalForm.CountMonthsInYear(3));
        Assert.Equal(13, OrdinalForm.CountMonthsInYear(4));
        Assert.Equal(12, OrdinalForm.CountMonthsInYear(5));
        Assert.Equal(12, OrdinalForm.CountMonthsInYear(6));
        Assert.Equal(12, OrdinalForm.CountMonthsInYear(7));
        Assert.Equal(13, OrdinalForm.CountMonthsInYear(8));
    }

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
    public void NormalForm_Values()
    {
        Assert.Equal(NormalForm, Form.Normalize());
        Assert.Equal(new CalendricalForm(4, 49, 51), NormalForm.Reverse());
    }

    [Fact]
    public void OrdinalForm_Values()
    {
        Assert.Equal(new CalendricalForm(4, 49, 52), OrdinalForm.Reverse());
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
        Assert.Equal(y, Form.GetYear(startOfYear, out int m0));
        Assert.Equal(0, m0);
    }

    [Theory]
    // Year 0.
    [InlineData(0, 1, 0)]
    [InlineData(0, 2, 1)]
    [InlineData(0, 3, 2)]
    [InlineData(0, 4, 3)]
    [InlineData(0, 5, 4)]
    [InlineData(0, 6, 5)]
    [InlineData(0, 7, 6)]
    [InlineData(0, 8, 7)]
    [InlineData(0, 9, 8)]
    [InlineData(0, 10, 9)]
    [InlineData(0, 11, 10)]
    [InlineData(0, 12, 11)]
    // Year 1.
    [InlineData(1, 1, 12)]
    [InlineData(1, 2, 13)]
    [InlineData(1, 3, 14)]
    [InlineData(1, 4, 15)]
    [InlineData(1, 5, 16)]
    [InlineData(1, 6, 17)]
    [InlineData(1, 7, 18)]
    [InlineData(1, 8, 19)]
    [InlineData(1, 9, 20)]
    [InlineData(1, 10, 21)]
    [InlineData(1, 11, 22)]
    [InlineData(1, 12, 23)]
    // Year 2.
    [InlineData(2, 1, 24)]
    [InlineData(2, 2, 25)]
    [InlineData(2, 3, 26)]
    [InlineData(2, 4, 27)]
    [InlineData(2, 5, 28)]
    [InlineData(2, 6, 29)]
    [InlineData(2, 7, 30)]
    [InlineData(2, 8, 31)]
    [InlineData(2, 9, 32)]
    [InlineData(2, 10, 33)]
    [InlineData(2, 11, 34)]
    [InlineData(2, 12, 35)]
    // Year 3 (leap).
    [InlineData(3, 1, 36)]
    [InlineData(3, 2, 37)]
    [InlineData(3, 3, 38)]
    [InlineData(3, 4, 39)]
    [InlineData(3, 5, 40)]
    [InlineData(3, 6, 41)]
    [InlineData(3, 7, 42)]
    [InlineData(3, 8, 43)]
    [InlineData(3, 9, 44)]
    [InlineData(3, 10, 45)]
    [InlineData(3, 11, 46)]
    [InlineData(3, 12, 47)]
    [InlineData(3, 13, 48)]
    // Year 4.
    [InlineData(4, 1, 49)]
    [InlineData(4, 12, 60)]
    // Year 5.
    [InlineData(5, 1, 61)]
    [InlineData(5, 12, 72)]
    // Year 6.
    [InlineData(6, 1, 73)]
    [InlineData(6, 12, 84)]
    // Year 7 (leap).
    [InlineData(7, 1, 85)]
    [InlineData(7, 13, 97)]
    // Year 8.
    [InlineData(8, 1, 98)]
    [InlineData(8, 12, 109)]
    public static void Form_GetMonthParts(int y, int m, int monthsSinceEpoch)
    {
        var ym = new Yemo(y, m);
        // Act & Assert
        Assert.Equal(ym, Form.GetMonthParts(monthsSinceEpoch));
        Assert.Equal(new Yemo(y + 1, m), OrdinalForm.GetMonthParts(monthsSinceEpoch));
    }
}
