// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Geometry;

using Zorglub.Time.Core;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Core.Utilities;
using Zorglub.Time.Geometry.Discrete;
using Zorglub.Time.Geometry.Forms;

using static Zorglub.Bulgroz.GJConstants;

// CenturyForm's.
public sealed partial class GregorianCenturyFormTests : AnalyzerFacts
{
    private static readonly int[] s_CenturyLengths;

    /// <summary>36_524, 36_524, 36_524, 36_525.</summary>
    public static readonly TheoryData<int, int> CenturyLengths;
    public static readonly TheoryData<int, int> StartOfCenturyList;

    static GregorianCenturyFormTests()
    {
        int[] lens = ReadOnlySpanHelpers.Rotate(GregorianSchema.DaysIn4CenturyCycle, 1);
        s_CenturyLengths = lens;

        CenturyLengths = TheoryDataHelpers.ConvertToArrayData(lens);
        StartOfCenturyList = TheoryDataHelpers.ConvertToArrayData(ArrayHelpers.ConvertToCumulativeArray(lens));
    }

    public GregorianCenturyFormTests() : base(s_CenturyLengths) { }

    public override CodeArray CodeArray0 { get; } = new(4);

    /// <summary>(146_097, 4, 0)</summary>
    public override QuasiAffineForm Form { get; } = new(146_097, 4, 0);

    public override IEnumerable<QuasiAffineForm> FormList
    {
        get
        {
            yield return Form0;
            yield return new(1, 4, 0);
            yield return Form;
        }
    }

    public override IEnumerable<CodeArray> CodeArrayList
    {
        get
        {
            yield return CodeArray;
            yield return CodeArray0;
        }
    }

    public override IEnumerable<TroeschMap> TroeschMapList
    {
        get
        {
            yield return new(36_524, false, 0);
        }
    }

    private QuasiAffineForm[] RotatedForms => new[] {
        Form,
        new(109_573, 3, 0), // Not a century form.
        new(109_573, 3, 1), // Not a century form.
        new(146_097, 4, 3),
    };

    [Fact]
    public override void TryConvertCodeToForm_RotatedCode() =>
        TryConvertCodeToForm_AllVersions(RotatedForms);
}

public partial class GregorianCenturyFormTests
{
    [Fact]
    public void CenturyForm_Values()
    {
        var form = GregorianGeometry.CenturyForm;
        var altForm = GregorianGeometry.AltCenturyForm;

        // Act & Assert
        Form_Equals(form);

        Assert.Equal(new CenturyForm(146_097, 4, 0) { Origin = new Yemoda(0, 3, 1) }, form);
        Assert.Equal(new CalendricalForm(4, 146_097, 3), form.Reverse());

        Assert.Equal(new CenturyForm(146_097, 4, -1224) { Origin = new Yemoda(0, 3, 1) }, altForm);
        Assert.Equal(new CalendricalForm(4, 146_097, 1227), altForm.Reverse());
    }

    #region AltCenturyForm

    [Theory, MemberData(nameof(CenturyLengths))]
    public static void AltCenturyForm_CountDaysInCentury(int c, int daysInCentury) =>
        Assert.Equal(daysInCentury, GregorianGeometry.AltCenturyForm.CountDaysInCentury(c));

    [Theory, MemberData(nameof(StartOfCenturyList))]
    public static void AltCenturyForm_GetStartOfCentury(int c, int startOfCentury) =>
        Assert.Equal(
            startOfCentury - DaysFromEndOfFebruaryYear0ToEpoch,
            GregorianGeometry.AltCenturyForm.GetStartOfCentury(c));

    [Theory, MemberData(nameof(StartOfCenturyList))]
    public static void AltCenturyForm_GetCentury(int c, int startOfCentury)
    {
        Assert.Equal(c, GregorianGeometry.AltCenturyForm.GetCentury(startOfCentury, out int d0c));
        Assert.Equal(DaysFromEndOfFebruaryYear0ToEpoch, d0c);
    }

    #endregion
    #region Normal forms

    // The form found in RotatedForm is (109_573, 3, 0), but we can't use it.
    // We have to double the 400-year cycle.
    [Fact]
    public void CenturyForm2()
    {
        var codes = new int[8] { 36_524, 36_524, 36_525, 36_524, /* Second cycle */ 36_524, 36_524, 36_525, 36_524 };
        var code = new CodeArray(codes);
        // Act & Assert
        Assert.True(TroeschAnalyzer.TryConvertCodeToForm(code, out var formA));
        Assert.Equal(new(146_097, 4, 1), formA);
    }

    // The form found in RotatedForm is (109_573, 3, 1), but we can't use it.
    // We have to double the 400-year cycle.
    [Fact]
    public void CenturyForm3()
    {
        var codes = new int[8] { 36_524, 36_525, 36_524, 36_524, /* Second cycle */ 36_524, 36_525, 36_524, 36_524 };
        var code = new CodeArray(codes);
        // Act & Assert
        Assert.True(TroeschAnalyzer.TryConvertCodeToForm(code, out var formA));
        Assert.Equal(new(146_097, 4, 2), formA);
    }

    #endregion
}
