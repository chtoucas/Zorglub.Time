// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Geometry;

using Zorglub.Testing.Data.Unbounded;
using Zorglub.Time;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Geometry.Discrete;
using Zorglub.Time.Geometry.Forms;

using static Zorglub.Bulgroz.GJConstants;
using static Zorglub.Bulgroz.GregorianConstants;

// TODO: More tests for CountDaysFromEpochToStartOfYear().
// Migrate StartOfYearDayNumberData.

// Testing the "rotated" forms using GENUINE data.
// See Zorglub.Time.Geometry.Forms.GJGeometry.YearForm0/1/2/3.
//
// In the Julian case, things are simple since the year forms are "global",
// they are valid for whatever year we feed them with.
// On the contrary, in the Gregorian case, we have to be careful.
//
// When necessary, the forms are rebased to use 01/01/0000 as origin.
// The genuine sequence is: 366, 365, 365, 365.
//
// With YearLengths, we already tested YearForm1.CountDaysInYear() (see
// YearForm_CountDaysInYear() above). This is the only form that will
// pass this test, indeed remember that the other forms use a "rotated"
// version of YearLengths.

// Gregorian/Julian normal forms.
public partial class GJYearFormTests
{
    // These values are valid in both Gregorian and Julian cases.
    private const int DaysInYear2 = DaysInCommonYear;
    private const int DaysInYear2And3 = 2 * DaysInCommonYear;

    private static readonly YearForm YearForm0 = GJGeometry.YearForm0;
    private static readonly YearForm YearForm1 = GJGeometry.YearForm1;
    private static readonly YearForm YearForm2 = GJGeometry.YearForm2;
    private static readonly YearForm YearForm3 = GJGeometry.YearForm3;

    // WARNING: keep this after the initialization of YearForm0/1/2/3.
    private static readonly YearForm NormalYearForm0 = YearForm0.Normalize();
    private static readonly YearForm NormalYearForm1 = YearForm1.Normalize();
    private static readonly YearForm NormalYearForm2 = YearForm2.Normalize();
    private static readonly YearForm NormalYearForm3 = YearForm3.Normalize();

    private static readonly JulianSchema s_JulianSchema = new();
    private static readonly GregorianSchema s_GregorianSchema = new();

    private static UnboundedGregorianDataSet GregorianDataSet => UnboundedGregorianDataSet.Instance;
    private static UnboundedJulianDataSet JulianDataSet => UnboundedJulianDataSet.Instance;

    public static DataGroup<YearInfo> JulianYearInfoData => JulianDataSet.YearInfoData;
    public static DataGroup<YearDayNumber> JulianStartOfYearDayNumberData => JulianDataSet.StartOfYearDayNumberData;
    public static DataGroup<DayNumberInfo> JulianDayNumberInfoData => JulianDataSet.DayNumberInfoData;

    public static DataGroup<YearInfo> GregorianYearInfoData => GregorianDataSet.YearInfoData;
    public static DataGroup<YearDayNumber> GregorianStartOfYearDayNumberData => GregorianDataSet.StartOfYearDayNumberData;
    public static DataGroup<DayNumberInfo> GregorianDayNumberInfoData => GregorianDataSet.DayNumberInfoData;

    [Fact]
    public static void Check_DaysInYear2() =>
        Assert.Equal(DaysInYear2, s_GregorianSchema.CountDaysInYear(2));

    [Fact]
    public static void Check_DaysInYear2And3() =>
        Assert.Equal(DaysInYear2And3, s_GregorianSchema.CountDaysInYear(3) + s_GregorianSchema.CountDaysInYear(2));
}

// YearForm & AltYearForm.
// REVIEW: revoir ça.
// We test YearForm further below. Indeed YearForm is YearForm1 but with a
// different origin which the formulae do NOT use.
// Nevertheless remember that YearForm and AltYearForm are used in a very
// different context, namely within a 3rd order schema which expects a year
// form using a zero-based year of century (y % 100 instead of y).
// Furthermore, because of their origin (March 1st), in the end these tests
// are not really meaningful.
public partial class GJYearFormTests
{
    //
    // Julian case.
    //

    [Theory, MemberData(nameof(JulianYearInfoData))]
    public static void Julian_AltYearForm_CountDaysInYear(YearInfo info) =>
        Assert.Equal(info.DaysInYear, GJGeometry.AltYearForm.CountDaysInYear(info.Year - 1));

    [Theory, MemberData(nameof(JulianStartOfYearDayNumberData))]
    public static void Julian_AltYearForm_GetStartOfYear(YearDayNumber info)
    {
        // Act
        int daysSinceEpoch = GJGeometry.AltYearForm.GetStartOfYear(info.Year - 1)
            + DaysFromEndOfFebruaryYear0ToEpoch;
        // Act & Assert
        Assert.Equal(info.DayNumber, DayZero.OldStyle + daysSinceEpoch);
    }

    [Theory, MemberData(nameof(JulianDayNumberInfoData))]
    public static void Julian_AltYearForm_GetYear(DayNumberInfo info)
    {
        var (dayNumber, y, m, d) = info;

        int daysSinceEpoch = dayNumber - DayZero.OldStyle;
        int d0yE = s_JulianSchema.GetDayOfYear(y, m, d) - 1;
        // Act
        int yA = 1 + GJGeometry.AltYearForm.GetYear(
            daysSinceEpoch - DaysFromEndOfFebruaryYear0ToEpoch, out int d0yA);
        // Act & Assert
        Assert.Equal(y, yA);
        Assert.Equal(d0yE, d0yA);
    }

    //
    // Gregorian case.
    //

    [Theory, MemberData(nameof(GregorianYearInfoData))]
    public static void Gregorian_AltYearForm_CountDaysInYear(YearInfo info)
    {
        int y = info.Year;

        // Gregorian centennial common years.
        // See further below for the reason why we have to exclude these years.
        if (y != 0 && y % 100 == 0 && y % 400 != 0) { return; }

        // Assert
        Assert.Equal(info.DaysInYear, GJGeometry.AltYearForm.CountDaysInYear(y - 1));
    }

    [Theory, MemberData(nameof(GregorianStartOfYearDayNumberData))]
    public static void Gregorian_AltYearForm_GetStartOfYear(YearDayNumber info)
    {
        int y = info.Year;

        // See further below for the reason why we have to exclude these years.
        if (y < -99 || 100 < y) { return; }

        // Act
        int daysSinceEpoch = GJGeometry.AltYearForm.GetStartOfYear(y - 1)
            + DaysFromEndOfFebruaryYear0ToEpoch;
        // Assert
        Assert.Equal(info.DayNumber, DayZero.NewStyle + daysSinceEpoch);
    }

    [Theory, MemberData(nameof(GregorianDayNumberInfoData))]
    public static void Gregorian_AltYearForm_GetYear(DayNumberInfo info)
    {
        var (dayNumber, y, m, d) = info;

        // See further below for the reason why we have to exclude these years.
        if (y < -99 || 100 < y) { return; }

        int daysSinceEpoch = dayNumber - DayZero.NewStyle;
        int d0yE = s_GregorianSchema.GetDayOfYear(y, m, d) - 1;
        // Act
        int yA = 1 + GJGeometry.AltYearForm.GetYear(
            daysSinceEpoch - DaysFromEndOfFebruaryYear0ToEpoch, out int d0yA);
        // Assert
        Assert.Equal(y, yA);
        Assert.Equal(d0yE, d0yA);
    }
}

// Rotated quasi-affine forms.
// Below we refer to RotatedForms from Zorglub.Time.Geometry.Forms.GJFormTests.
// There is nothing to say about YearForm0 and YearForm1, they already have
// the right property: f(y + 4) = f(y) + 1461.
public partial class GJYearFormTests
{
    #region YearForm2, sequence 365, 365, 366, 365.

    // The form found in RotatedForms is (1096, 3, 0), but we can't use it.
    // We have to double the 4-year cycle.

    [Theory]
    [InlineData(0, 365)]
    [InlineData(1, 365)]
    [InlineData(2, 366)]
    [InlineData(3, 365)]
    // Second cycle.
    [InlineData(4, 365)]
    [InlineData(5, 366)] // Should be 365.
    [InlineData(6, 365)] // Should be 366.
    [InlineData(7, 365)]
    public static void QuasiAffineForm2_IsNotYearForm(int y, int value)
    {
        var form = new QuasiAffineForm(1096, 3, 0);
        // Act & Assert
        Assert.Equal(value, form.CodeAt(y));
    }

    [Fact]
    public static void YearForm2_TryConvertCodeToForm()
    {
        var codes = new int[8] { 365, 365, 366, 365, /* Second cycle */ 365, 365, 366, 365 };
        var code = new CodeArray(codes);
        // Act & Assert
        Assert.True(TroeschAnalyzer.TryConvertCodeToForm(code, out var formA));
        Assert.Equal(new(1461, 4, 1), formA);
    }

    #endregion
    #region YearForm3, sequence 365, 366, 365, 365.

    // The form found in RotatedForms is (1096, 3, 1), but we can't use it.
    // We have to double the 4-year cycle.

    [Theory]
    [InlineData(0, 365)]
    [InlineData(1, 366)]
    [InlineData(2, 365)]
    [InlineData(3, 365)]
    // Second cycle.
    [InlineData(4, 366)] // Should be 365.
    [InlineData(5, 365)] // Should be 366.
    [InlineData(6, 365)]
    [InlineData(7, 366)] // Should be 365.
    public static void QuasiAffineForm3_IsNotYearForm(int y, int value)
    {
        var form = new QuasiAffineForm(1096, 3, 1);
        // Act & Assert
        Assert.Equal(value, form.CodeAt(y));
    }

    [Fact]
    public static void YearForm3_TryConvertCodeToForm()
    {
        var codes = new int[8] { 365, 366, 365, 365, /* Second cycle */ 365, 366, 365, 365 };
        var code = new CodeArray(codes);
        // Act & Assert
        Assert.True(TroeschAnalyzer.TryConvertCodeToForm(code, out var formA));
        Assert.Equal(new(1461, 4, 2), formA);
    }

    #endregion

    // Pseudo-periodicity of the forms: f(y + 4) = f(y) + 1461.
    // This is pretty obvious but let's check it anyway.
    [Fact]
    public static void YearForm0123_PseudoPeriodicity()
    {
        for (int y = -2000; y <= 2000; y++)
        {
            // Act
            int diff0 = YearForm0.GetStartOfYear(y + 4) - YearForm0.GetStartOfYear(y);
            int diff1 = YearForm1.GetStartOfYear(y + 4) - YearForm1.GetStartOfYear(y);
            int diff2 = YearForm2.GetStartOfYear(y + 4) - YearForm2.GetStartOfYear(y);
            int diff3 = YearForm3.GetStartOfYear(y + 4) - YearForm3.GetStartOfYear(y);
            // Assert
            Assert.Equal(DaysPer4YearSubcycle, diff0);
            Assert.Equal(DaysPer4YearSubcycle, diff1);
            Assert.Equal(DaysPer4YearSubcycle, diff2);
            Assert.Equal(DaysPer4YearSubcycle, diff3);
        }
    }
}

// CountDaysFromEpochToStartOfYear().
public partial class GJYearFormTests
{
    [Fact]
    public static void YearForm0123_CountDaysFromEpochToStartOfYear()
    {
        Assert.Equal(-DaysInYear0, YearForm0.CountDaysFromEpochToStartOfYear(0));
        Assert.Equal(0, YearForm1.CountDaysFromEpochToStartOfYear(1));
        Assert.Equal(DaysInYear2, YearForm2.CountDaysFromEpochToStartOfYear(2));
        Assert.Equal(DaysInYear2And3, YearForm3.CountDaysFromEpochToStartOfYear(3));
    }
}

// CountDaysInYear().
// In the Gregorian case, we exclude the centennial common years which
// cannot be handled by the year forms.
public partial class GJYearFormTests
{
    [Theory, MemberData(nameof(JulianYearInfoData))]
    public static void Julian_YearForm0123_CountDaysInYear(YearInfo info)
    {
        int y = info.Year;
        int daysInYear = info.DaysInYear;

        // Plain forms.
        Assert.Equal(daysInYear, YearForm0.CountDaysInYear(y));
        Assert.Equal(daysInYear, YearForm1.CountDaysInYear(y - 1));
        Assert.Equal(daysInYear, YearForm2.CountDaysInYear(y - 2));
        Assert.Equal(daysInYear, YearForm3.CountDaysInYear(y - 3));

        // Normal forms.
        Assert.Equal(daysInYear, NormalYearForm0.CountDaysInYear(y));
        Assert.Equal(daysInYear, NormalYearForm1.CountDaysInYear(y));
        Assert.Equal(daysInYear, NormalYearForm2.CountDaysInYear(y));
        Assert.Equal(daysInYear, NormalYearForm3.CountDaysInYear(y));
    }

    [Theory, MemberData(nameof(GregorianYearInfoData))]
    public static void Gregorian_YearForm0123_CountDaysInYear(YearInfo info)
    {
        int y = info.Year;
        int daysInYear = info.DaysInYear;

        // Gregorian centennial common years.
        if (y != 0 && y % 100 == 0 && y % 400 != 0) { return; }

        // Plain forms.
        Assert.Equal(daysInYear, YearForm0.CountDaysInYear(y));
        Assert.Equal(daysInYear, YearForm1.CountDaysInYear(y - 1));
        Assert.Equal(daysInYear, YearForm2.CountDaysInYear(y - 2));
        Assert.Equal(daysInYear, YearForm3.CountDaysInYear(y - 3));

        // Normal forms.
        Assert.Equal(daysInYear, NormalYearForm0.CountDaysInYear(y));
        Assert.Equal(daysInYear, NormalYearForm1.CountDaysInYear(y));
        Assert.Equal(daysInYear, NormalYearForm2.CountDaysInYear(y));
        Assert.Equal(daysInYear, NormalYearForm3.CountDaysInYear(y));
    }
}

// GetStartOfYear().
// In the Gregorian case, we must stay within the range of years around
// zero that follow the Julian rules: -99 <= y <= 100 --- we can keep
// the year 100 since we are computing the start of the year.
public partial class GJYearFormTests
{
    [Theory, MemberData(nameof(JulianStartOfYearDayNumberData))]
    public static void Julian_YearForm0123_GetStartOfYear(YearDayNumber info)
    {
        var (y, startOfYear) = info;

        // Act
        int daysSinceEpoch0 = YearForm0.GetStartOfYear(y) - DaysInYear0;
        int daysSinceEpoch1 = YearForm1.GetStartOfYear(y - 1);
        int daysSinceEpoch2 = YearForm2.GetStartOfYear(y - 2) + DaysInYear2;
        int daysSinceEpoch3 = YearForm3.GetStartOfYear(y - 3) + DaysInYear2And3;

        // Act & Assert

        // Plain forms.
        Assert.Equal(startOfYear, DayZero.OldStyle + daysSinceEpoch0);
        Assert.Equal(startOfYear, DayZero.OldStyle + daysSinceEpoch1);
        Assert.Equal(startOfYear, DayZero.OldStyle + daysSinceEpoch2);
        Assert.Equal(startOfYear, DayZero.OldStyle + daysSinceEpoch3);

        // Normal forms.
        Assert.Equal(startOfYear, DayZero.OldStyle + NormalYearForm0.GetStartOfYear(y));
        Assert.Equal(startOfYear, DayZero.OldStyle + NormalYearForm1.GetStartOfYear(y));
        Assert.Equal(startOfYear, DayZero.OldStyle + NormalYearForm2.GetStartOfYear(y));
        Assert.Equal(startOfYear, DayZero.OldStyle + NormalYearForm3.GetStartOfYear(y));
    }

    [Fact]
    public static void Gregorian_YearForm0123_GetStartOfYear_Fails_AtMinus100()
    {
        // See GregorianData.StartofYear.
        int startOfYear = -GregorianSchema.DaysPer100YearSubcycle - 1 - 365;

        // Act & Assert

        // Auto-check.
        Assert.Equal(startOfYear, s_GregorianSchema.GetStartOfYear(-100));

        // Plain forms.
        Assert.Equal(startOfYear - 1, YearForm0.GetStartOfYear(-100) - DaysInYear0);
        Assert.Equal(startOfYear - 1, YearForm1.GetStartOfYear(-100 - 1));
        Assert.Equal(startOfYear - 1, YearForm2.GetStartOfYear(-100 - 2) + DaysInYear2);
        Assert.Equal(startOfYear - 1, YearForm3.GetStartOfYear(-100 - 3) + DaysInYear2And3);

        // Normal forms.
        Assert.Equal(startOfYear - 1, NormalYearForm0.GetStartOfYear(-100));
        Assert.Equal(startOfYear - 1, NormalYearForm1.GetStartOfYear(-100));
        Assert.Equal(startOfYear - 1, NormalYearForm2.GetStartOfYear(-100));
        Assert.Equal(startOfYear - 1, NormalYearForm3.GetStartOfYear(-100));
    }

    [Fact]
    public static void Gregorian_YearForm0123_GetStartOfYear_Succeeds_FromMinus99To100()
    {
        for (int y = -99; y <= 100; y++)
        {
            // See GregorianData.StartofYear.
            int startOfYear = s_GregorianSchema.GetStartOfYear(y);

            // Act & Assert

            // Plain forms.
            Assert.Equal(startOfYear, YearForm0.GetStartOfYear(y) - DaysInYear0);
            Assert.Equal(startOfYear, YearForm1.GetStartOfYear(y - 1));
            Assert.Equal(startOfYear, YearForm2.GetStartOfYear(y - 2) + DaysInYear2);
            Assert.Equal(startOfYear, YearForm3.GetStartOfYear(y - 3) + DaysInYear2And3);

            // Normal forms.
            Assert.Equal(startOfYear, NormalYearForm0.GetStartOfYear(y));
            Assert.Equal(startOfYear, NormalYearForm1.GetStartOfYear(y));
            Assert.Equal(startOfYear, NormalYearForm2.GetStartOfYear(y));
            Assert.Equal(startOfYear, NormalYearForm3.GetStartOfYear(y));
        }
    }

    [Fact]
    public static void Gregorian_YearForm0123_GetStartOfYear_Fails_At101()
    {
        // See GregorianData.StartofYear.
        int startOfYear = GregorianSchema.DaysPer100YearSubcycle;

        // Act & Assert

        // Auto-check.
        Assert.Equal(startOfYear, s_GregorianSchema.GetStartOfYear(101));

        // Plain forms.
        Assert.Equal(startOfYear + 1, YearForm0.GetStartOfYear(101) - DaysInYear0);
        Assert.Equal(startOfYear + 1, YearForm1.GetStartOfYear(101 - 1));
        Assert.Equal(startOfYear + 1, YearForm2.GetStartOfYear(101 - 2) + DaysInYear2);
        Assert.Equal(startOfYear + 1, YearForm3.GetStartOfYear(101 - 3) + DaysInYear2And3);

        // Normal forms.
        Assert.Equal(startOfYear + 1, NormalYearForm0.GetStartOfYear(101));
        Assert.Equal(startOfYear + 1, NormalYearForm1.GetStartOfYear(101));
        Assert.Equal(startOfYear + 1, NormalYearForm2.GetStartOfYear(101));
        Assert.Equal(startOfYear + 1, NormalYearForm3.GetStartOfYear(101));
    }
}

// GetYear().
public partial class GJYearFormTests
{
    [Theory, MemberData(nameof(JulianDayNumberInfoData))]
    public static void Julian_YearForm0123_GetYear(DayNumberInfo info)
    {
        var (dayNumber, y, m, d) = info;

        int daysSinceEpoch = dayNumber - DayZero.OldStyle;
        int d0yE = s_JulianSchema.GetDayOfYear(y, m, d) - 1;

        // Act & Assert

        // Plain forms.
        Assert.Equal(y, YearForm0.GetYear(daysSinceEpoch + DaysInYear0, out int d0yA));
        Assert.Equal(d0yE, d0yA);
        Assert.Equal(y, 1 + YearForm1.GetYear(daysSinceEpoch, out d0yA));
        Assert.Equal(d0yE, d0yA);
        Assert.Equal(y, 2 + YearForm2.GetYear(daysSinceEpoch - DaysInYear2, out d0yA));
        Assert.Equal(d0yE, d0yA);
        Assert.Equal(y, 3 + YearForm3.GetYear(daysSinceEpoch - DaysInYear2And3, out d0yA));
        Assert.Equal(d0yE, d0yA);

        // Normal forms.
        Assert.Equal(y, NormalYearForm0.GetYear(daysSinceEpoch, out d0yA));
        Assert.Equal(d0yE, d0yA);
        Assert.Equal(y, NormalYearForm1.GetYear(daysSinceEpoch, out d0yA));
        Assert.Equal(d0yE, d0yA);
        Assert.Equal(y, NormalYearForm2.GetYear(daysSinceEpoch, out d0yA));
        Assert.Equal(d0yE, d0yA);
        Assert.Equal(y, NormalYearForm3.GetYear(daysSinceEpoch, out d0yA));
        Assert.Equal(d0yE, d0yA);
    }

    [Fact]
    public static void Gregorian_YearForm0123_GetYear_Fails_AtEndOfMinus100()
    {
        // See GregorianData.StartofYear.
        int endOfYear = -GregorianSchema.DaysPer100YearSubcycle - 1 - 1;

        // Act & Assert

        // Auto-check.
        Assert.Equal(endOfYear, s_GregorianSchema.GetEndOfYear(-100));

        // The first failure happens at the end of year -100.
        // Genuine result: year -100, doy = 365 (-100 is a common year).

        // Plain forms.
        Assert.Equal(-100, YearForm0.GetYear(endOfYear + DaysInYear0, out int d0y));
        Assert.Equal(366, 1 + d0y);
        Assert.Equal(-100, 1 + YearForm1.GetYear(endOfYear, out d0y));
        Assert.Equal(366, 1 + d0y);
        Assert.Equal(-100, 2 + YearForm2.GetYear(endOfYear - DaysInYear2, out d0y));
        Assert.Equal(366, 1 + d0y);
        Assert.Equal(-100, 3 + YearForm3.GetYear(endOfYear - DaysInYear2And3, out d0y));
        Assert.Equal(366, 1 + d0y);

        // Normal forms.
        Assert.Equal(-100, NormalYearForm0.GetYear(endOfYear, out d0y));
        Assert.Equal(366, 1 + d0y);
        Assert.Equal(-100, NormalYearForm1.GetYear(endOfYear, out d0y));
        Assert.Equal(366, 1 + d0y);
        Assert.Equal(-100, NormalYearForm2.GetYear(endOfYear, out d0y));
        Assert.Equal(366, 1 + d0y);
        Assert.Equal(-100, NormalYearForm3.GetYear(endOfYear, out d0y));
        Assert.Equal(366, 1 + d0y);
    }

    [Fact]
    public static void Gregorian_YearForm0123_GetYear_Succeeds_FromStartOfMinus99ToEndOf100()
    {
        // See GregorianData.StartofYear.
        int startOfYearMinus99 = -GregorianSchema.DaysPer100YearSubcycle - 1;
        int endOfYear100 = GregorianSchema.DaysPer100YearSubcycle - 1;

        // Act & Assert

        // Auto-check.
        Assert.Equal(startOfYearMinus99, s_GregorianSchema.GetStartOfYear(-99));
        Assert.Equal(endOfYear100, s_GregorianSchema.GetEndOfYear(100));

        for (int daysSinceEpoch = startOfYearMinus99; daysSinceEpoch <= endOfYear100; daysSinceEpoch++)
        {
            int y = s_GregorianSchema.GetYear(daysSinceEpoch, out int doy);

            // Plain forms.
            Assert.Equal(y, YearForm0.GetYear(daysSinceEpoch + DaysInYear0, out int d0yA));
            Assert.Equal(doy, 1 + d0yA);
            Assert.Equal(y, 1 + YearForm1.GetYear(daysSinceEpoch, out d0yA));
            Assert.Equal(doy, 1 + d0yA);
            Assert.Equal(y, 2 + YearForm2.GetYear(daysSinceEpoch - DaysInYear2, out d0yA));
            Assert.Equal(doy, 1 + d0yA);
            Assert.Equal(y, 3 + YearForm3.GetYear(daysSinceEpoch - DaysInYear2And3, out d0yA));
            Assert.Equal(doy, 1 + d0yA);

            // Normal forms.
            Assert.Equal(y, NormalYearForm0.GetYear(daysSinceEpoch, out d0yA));
            Assert.Equal(doy, 1 + d0yA);
            Assert.Equal(y, NormalYearForm1.GetYear(daysSinceEpoch, out d0yA));
            Assert.Equal(doy, 1 + d0yA);
            Assert.Equal(y, NormalYearForm2.GetYear(daysSinceEpoch, out d0yA));
            Assert.Equal(doy, 1 + d0yA);
            Assert.Equal(y, NormalYearForm3.GetYear(daysSinceEpoch, out d0yA));
            Assert.Equal(doy, 1 + d0yA);
        }
    }

    [Fact]
    public static void Gregorian_YearForm0123_GetYear_Fails_AtStartOf101()
    {
        // See GregorianData.StartofYear.
        int startOfYear = GregorianSchema.DaysPer100YearSubcycle;

        // Act & Assert

        // Auto-check.
        Assert.Equal(startOfYear, s_GregorianSchema.GetStartOfYear(101));

        // The first failure happens at the start of year 100.
        // Genuine result: year 101, doy = 1.

        // Plain forms.
        Assert.Equal(100, YearForm0.GetYear(startOfYear + DaysInYear0, out int d0y));
        Assert.Equal(366, 1 + d0y);
        Assert.Equal(100, 1 + YearForm1.GetYear(startOfYear, out d0y));
        Assert.Equal(366, 1 + d0y);
        Assert.Equal(100, 2 + YearForm2.GetYear(startOfYear - DaysInYear2, out d0y));
        Assert.Equal(366, 1 + d0y);
        Assert.Equal(100, 3 + YearForm3.GetYear(startOfYear - DaysInYear2And3, out d0y));
        Assert.Equal(366, 1 + d0y);

        // Normal forms.
        Assert.Equal(100, NormalYearForm0.GetYear(startOfYear, out d0y));
        Assert.Equal(366, 1 + d0y);
        Assert.Equal(100, NormalYearForm1.GetYear(startOfYear, out d0y));
        Assert.Equal(366, 1 + d0y);
        Assert.Equal(100, NormalYearForm2.GetYear(startOfYear, out d0y));
        Assert.Equal(366, 1 + d0y);
        Assert.Equal(100, NormalYearForm3.GetYear(startOfYear, out d0y));
        Assert.Equal(366, 1 + d0y);
    }
}
