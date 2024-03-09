// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Bulgroz.Geometry;

using System.Diagnostics.CodeAnalysis;

using Zorglub.Time.Core;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Core.Utilities;
using Zorglub.Time.Geometry.Discrete;
using Zorglub.Time.Geometry.Forms;

using static Zorglub.Bulgroz.GJConstants;

// See also Zorglub.Time.Geometry.Forms.GJYearFormTests.

// YearForm's.
public sealed partial class GJYearFormTests : AnalyzerFacts
{
    private static readonly int[] s_YearLengths;

    /// <summary>365, 365, 365, 366.</summary>
    public static readonly TheoryData<int, int> YearLengths;
    public static readonly TheoryData<int, int> StartOfYearList;

    [SuppressMessage("Performance", "CA1810:Initialize reference type static fields inline", Justification = "<Pending>")]
    static GJYearFormTests()
    {
        int[] lens = ReadOnlySpanHelpers.Rotate(GJSchema.DaysIn4YearCycle, 1);
        s_YearLengths = lens;

        YearLengths = TheoryDataHelpers.ConvertToArrayData(lens);
        StartOfYearList = TheoryDataHelpers.ConvertToArrayData(ArrayHelpers.ConvertToCumulativeArray(lens));
    }

    public GJYearFormTests() : base(s_YearLengths) { }

    public override CodeArray CodeArray0 { get; } = new(4);

    /// <summary>(1461, 4, 0)</summary>
    public override QuasiAffineForm Form { get; } = new(1461, 4, 0);

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
            yield return new(365, false, 0);
        }
    }

    private QuasiAffineForm[] RotatedForms => [
        Form,
        // Remark: 1096 = 3 * 365 + 1 (two common years and a leap one).
        // The two following forms are not year forms, they don't have the
        // right property f(y + 4) = f(y) + 1461. See below how one can
        // correct that.
        new(1096, 3, 0),
        new(1096, 3, 1),
        new(1461, 4, 3),
    ];

    [Fact]
    public override void TryConvertCodeToForm_RotatedCode() =>
        TryConvertCodeToForm_AllVersions(RotatedForms);
}

public partial class GJYearFormTests
{
    [Fact]
    public void YearForm_Values()
    {
        var form = GJGeometry.YearForm;
        var altForm = GJGeometry.AltYearForm;

        // Act & Assert
        Form_Equals(form);

        Assert.Equal(new YearForm(1461, 4, 0) { Origin = new Yemoda(0, 3, 1) }, form);
        Assert.Equal(new CalendricalForm(4, 1461, 3), form.Reverse());

        Assert.Equal(new YearForm(1461, 4, -1224) { Origin = new Yemoda(0, 3, 1) }, altForm);
        Assert.Equal(new CalendricalForm(4, 1461, 1227), altForm.Reverse());
    }

    #region YearForm

    [Theory, MemberData(nameof(YearLengths))]
    public static void YearForm_CountDaysInYear(int y, int daysInYear) =>
        Assert.Equal(daysInYear, GJGeometry.YearForm.CountDaysInYear(y));

    [Theory, MemberData(nameof(StartOfYearList))]
    public static void YearForm_GetStartOfYear(int y, int startOfYear) =>
        Assert.Equal(startOfYear, GJGeometry.YearForm.GetStartOfYear(y));

    [Theory, MemberData(nameof(StartOfYearList))]
    public static void YearForm_GetYear(int y, int startOfYear)
    {
        Assert.Equal(y, GJGeometry.YearForm.GetYear(startOfYear, out int d0y));
        Assert.Equal(0, d0y);
    }

    #endregion
    #region AltYearForm

    [Theory, MemberData(nameof(YearLengths))]
    public static void AltYearForm_CountDaysInYear(int y, int daysInYear) =>
        Assert.Equal(daysInYear, GJGeometry.AltYearForm.CountDaysInYear(y));

    [Theory, MemberData(nameof(StartOfYearList))]
    public static void AltYearForm_GetStartOfYear(int y, int startOfYear) =>
        Assert.Equal(
            startOfYear - DaysFromEndOfFebruaryYear0ToEpoch,
            GJGeometry.AltYearForm.GetStartOfYear(y));

    [Theory, MemberData(nameof(StartOfYearList))]
    public static void AltYearForm_GetYear(int y, int startOfYear)
    {
        Assert.Equal(y, GJGeometry.AltYearForm.GetYear(startOfYear, out int d0y));
        Assert.Equal(DaysFromEndOfFebruaryYear0ToEpoch, d0y);
    }

    #endregion
}

// MonthForm's.
public sealed partial class GJMonthFormTests : AnalyzerFacts
{
    /// <summary>31, 30, 31, 30, 31, 31, 30, 31, 30, 31, 31, 28.</summary>
    private static readonly int[] s_MonthLengths_CommonYear;
    /// <summary>31, 30, 31, 30, 31, 31, 30, 31, 30, 31, 31, 29.</summary>
    private static readonly int[] s_MonthLengths_LeapYear;
    /// <summary>31, 30, 31, 30, 31, 31, 30, 31, 30, 31, 31.</summary>
    private static readonly int[] s_MonthLengths;

    public static readonly TheoryData<int, int> MonthLengths;

    [SuppressMessage("Performance", "CA1810:Initialize reference type static fields inline", Justification = "<Pending>")]
    static GJMonthFormTests()
    {
        s_MonthLengths_CommonYear = ArrayHelpers.Rotate(SchemaHelpers.GetDaysInMonthArray<GJSchema>(false), 2);
        s_MonthLengths_LeapYear = ArrayHelpers.Rotate(SchemaHelpers.GetDaysInMonthArray<GJSchema>(true), 2);

        int[] lens = s_MonthLengths_LeapYear[0..^1];
        s_MonthLengths = lens;

        MonthLengths = TheoryDataHelpers.ConvertToArrayData(lens);
    }

    public GJMonthFormTests() : base(s_MonthLengths) { }

    public override CodeArray CodeArray0 { get; } = new(2);

    /// <summary>(153, 5, 2)</summary>
    public override QuasiAffineForm Form { get; } = new(153, 5, 2);

    public override IEnumerable<QuasiAffineForm> FormList
    {
        get
        {
            yield return Form0;
            yield return new(1, 2, 0);
            yield return new(5, 2, 0);
            yield return new(2, 5, 2);
            yield return new(3, 5, 2);
            yield return Form;
        }
    }

    public override IEnumerable<CodeArray> CodeArrayList
    {
        get
        {
            yield return CodeArray;
            yield return new([2, 3, 2, 3]);
            yield return CodeArray0;
        }
    }

    public override IEnumerable<TroeschMap> TroeschMapList
    {
        get
        {
            yield return new(30, true, 2);
            yield return new(2, false, 2);
        }
    }

    private (bool, QuasiAffineForm?)[] RotatedForms => [
        (true, Form),
        (false, null),
        (false, null),
        (false, null),
        (false, null),
        (false, null),
        (false, null),
        (false, null),
        (false, null),
        (false, null),
        (true, new(153, 5, 4)),
    ];

    // On examine aussi les autres combinaisons même si, ici, cela ne sert
    // pas à grand chose puisqu'on travaille sur un tableau tronqué et donc
    // nécessairement fixe.
    [Fact]
    public override void TryConvertCodeToForm_RotatedCode() =>
        TryConvertCodeToForm_SomeVersions(RotatedForms);

    [Fact]
    public void MonthLengths_CommonYear_IsNotReducible()
    {
        var code = new CodeArray(s_MonthLengths_CommonYear);
        // Act & Assert
        Assert.False(code.Reducible);
    }

    [Fact]
    public void MonthLengths_CommonYear_IsAlmostReducible()
    {
        var arr = SchemaHelpers.GetDaysInMonthArray<GJSchema>(false);
        var code = new CodeArray(arr);
        var exp = new CodeArray(s_MonthLengths);
        // Act
        var isAlmostReducible = code.IsAlmostReducible(out var newCode, out int newIndex);
        // Assert
        Assert.True(isAlmostReducible);
        Assert.Equal(exp, newCode);
        Assert.Equal(2, newIndex);
    }

    [Fact]
    public void MonthLengths_LeapYear_IsNotReducible()
    {
        var code = new CodeArray(s_MonthLengths_LeapYear);
        // Act & Assert
        Assert.False(code.Reducible);
    }

    [Fact]
    public void MonthLengths_LeapYear_IsAlmostReducible()
    {
        var arr = SchemaHelpers.GetDaysInMonthArray<GJSchema>(true);
        var code = new CodeArray(arr);
        var exp = new CodeArray(s_MonthLengths);
        // Act
        var isAlmostReducible = code.IsAlmostReducible(out var newCode, out int newIndex);
        // Assert
        Assert.True(isAlmostReducible);
        Assert.Equal(exp, newCode);
        Assert.Equal(2, newIndex);
    }

    [Fact]
    public void Form_FailsForLastMonth()
    {
        int last = s_MonthLengths.Length - 1;
        // Act & Assert
        Assert.NotEqual(s_MonthLengths_CommonYear[^1], Form.CodeAt(last));
        Assert.NotEqual(s_MonthLengths_LeapYear[^1], Form.CodeAt(last));
    }
}

public partial class GJMonthFormTests
{
    [Fact]
    public void MonthForm_Values()
    {
        var form = GJGeometry.MonthForm;

        // Act & Assert
        Form_Equals(form);

        Assert.Equal(new MonthForm(153, 5, 2) { Origin = new Yemoda(0, 3, 1) }, form);
        Assert.Equal(new CalendricalForm(5, 153, 2), form.Reverse());

        var ordForm = form.WithOrdinalNumbering();
        Assert.Equal(new MonthForm(153, 5, -151, MonthFormNumbering.Ordinal) { Origin = new Yemoda(0, 3, 1) }, ordForm);
        Assert.Equal(new CalendricalForm(5, 153, 155), ordForm.Reverse());

        var troeschForm = form.WithTroeschNumbering(ExceptionalMonth);
        Assert.Equal(new TroeschMonthForm(153, 5, -457, ExceptionalMonth) { Origin = new Yemoda(0, 3, 1) }, troeschForm);
        Assert.Equal(new CalendricalForm(5, 153, 461), troeschForm.Reverse());
    }

    // Last month special case.
    [Fact]
    public void MonthForm_CountDaysInMonth_AtMonth11() =>
        Assert.Equal(30, GJGeometry.MonthForm.CountDaysInMonth(11));

    #region Alternatives

    // Ordinal version.
    [Theory, MemberData(nameof(MonthLengths))]
    public void OrdinalMonthForm_CountDaysInMonth(int m, int daysInMonth)
    {
        var form = GJGeometry.MonthForm.WithOrdinalNumbering();
        // Act
        int actual = form.CountDaysInMonth(m + 1); // m = 1, 2, 3, etc.
        // Assert
        Assert.Equal(daysInMonth, actual);
    }

    // Troesch version.
    [Theory, MemberData(nameof(MonthLengths))]
    public void TroeschMonthForm_CountDaysInMonth(int m, int daysInMonth)
    {
        var form = GJGeometry.MonthForm.WithTroeschNumbering(ExceptionalMonth);
        // Act
        int actual = form.CountDaysInMonth(m + 3); // m = 3, 4, 5, etc.
        // Assert
        Assert.Equal(daysInMonth, actual);
    }

    #endregion
}
