// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

/// <summary>
/// Provides math facts about <see cref="CalendarYear"/> and its mathematical operations.
/// </summary>
public abstract partial class CalendarYearMathFacts<TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, IAdvancedMathDataSet, ISingleton<TDataSet>
{
    protected CalendarYearMathFacts(Calendar calendar)
    {
        CalendarUT = calendar ?? throw new ArgumentNullException(nameof(calendar));
    }

    /// <summary>
    /// Gets the calendar under test.
    /// </summary>
    protected Calendar CalendarUT { get; }

    public static DataGroup<YemodaPairAnd<int>> AddYearsData => DataSet.AddYearsData;
}

public partial class CalendarYearMathFacts<TDataSet> // NextYear()
{
    [Fact]
    public void NextYear_Overflows_AtMaxValue()
    {
        var max = CalendarUT.GetCalendarYear(CalendarUT.Scope.SupportedYears.Max);
        Assert.Overflows(() => max.NextYear());
        Assert.Overflows(() => max++);
    }
}

public partial class CalendarYearMathFacts<TDataSet> // PreviousYear()
{
    [Fact]
    public void PreviousYear_Overflows_AtMinValue()
    {
        var min = CalendarUT.GetCalendarYear(CalendarUT.Scope.SupportedYears.Min);
        Assert.Overflows(() => min.PreviousYear());
        Assert.Overflows(() => min--);
    }
}

public partial class CalendarYearMathFacts<TDataSet> // PlusYears()
{
    [Fact]
    public void PlusYears_OverflowOrUnderflow()
    {
        var year = CalendarUT.GetCalendarYear(3);
        // Act & Assert
        Assert.Overflows(() => year.PlusYears(Int32.MinValue));
        Assert.Overflows(() => year.PlusYears(Int32.MaxValue));
    }

    [Fact]
    public void PlusYears_WithLimitValues_AtMinValue()
    {
        var scope = CalendarUT.Scope;
        var min = CalendarUT.GetCalendarYear(scope.SupportedYears.Min);
        int years = scope.SupportedYears.Max - scope.SupportedYears.Min;
        var exp = CalendarUT.GetCalendarYear(scope.SupportedYears.Max);
        // Act & Assert
        Assert.Overflows(() => min.PlusYears(-1));
        Assert.Equal(min, min.PlusYears(0));
        Assert.Equal(exp, min.PlusYears(years));
        Assert.Overflows(() => min.PlusYears(years + 1));
    }

    [Fact]
    public void PlusYears_WithLimitValues_AtMaxValue()
    {
        var scope = CalendarUT.Scope;
        var max = CalendarUT.GetCalendarYear(scope.SupportedYears.Max);
        int years = scope.SupportedYears.Max - scope.SupportedYears.Min;
        var exp = CalendarUT.GetCalendarYear(scope.SupportedYears.Min);
        // Act & Assert
        Assert.Overflows(() => max.PlusYears(-years - 1));
        Assert.Equal(exp, max.PlusYears(-years));
        Assert.Equal(max, max.PlusYears(0));
        Assert.Overflows(() => max.PlusYears(1));
    }

    [Fact]
    public void PlusYears_WithLimitValues()
    {
        var scope = CalendarUT.Scope;
        var year = CalendarUT.GetCalendarYear(3);
        int minYears = scope.SupportedYears.Min - year.Year;
        int maxYears = scope.SupportedYears.Max - year.Year;
        var earliest = CalendarUT.GetCalendarYear(scope.SupportedYears.Min);
        var latest = CalendarUT.GetCalendarYear(scope.SupportedYears.Max);
        // Act & Assert
        Assert.Overflows(() => year.PlusYears(minYears - 1));
        Assert.Equal(earliest, year.PlusYears(minYears));
        Assert.Equal(latest, year.PlusYears(maxYears));
        Assert.Overflows(() => year.PlusYears(maxYears + 1));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void PlusYears_Zero_IsNeutral(YearInfo info)
    {
        var year = CalendarUT.GetCalendarYear(info.Year);
        // Act & Assert
        Assert.Equal(year, year.PlusYears(0));
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void PlusYears(YemodaPairAnd<int> info)
    {
        int ys = info.Value;
        var year = CalendarUT.GetCalendarYear(info.First.Year);
        var other = CalendarUT.GetCalendarYear(info.Second.Year);
        // Act & Assert
        // 1) year + ys = other.
        Assert.Equal(other, year + ys);
        Assert.Equal(other, year.PlusYears(ys));
        // 2) other - ys = year.
        Assert.Equal(year, other - ys);
        Assert.Equal(year, other.PlusYears(-ys));
        // 3) year - (-ys) = other.
        Assert.Equal(other, year - (-ys));
    }
}

public partial class CalendarYearMathFacts<TDataSet> // CountYearsSince()
{
    [Fact]
    public void CountYearsSince_DoesNotOverflow()
    {
        var scope = CalendarUT.Scope;
        var min = CalendarUT.GetCalendarYear(scope.SupportedYears.Min);
        var max = CalendarUT.GetCalendarYear(scope.SupportedYears.Max);
        int years = scope.SupportedYears.Max - scope.SupportedYears.Min;
        // Act & Assert
        Assert.Equal(years, max.CountYearsSince(min));
        Assert.Equal(-years, min.CountYearsSince(max));
    }

    // TODO: (MATH) CountYearsSince(), itou avec CalendarMonthTests().
    // Utiliser aussi les données DiffCutoff (idem avec PlusYears).
    // Revoir tous les tests des "math ops" pour les objets calendaires.
    //[Theory, MemberData(nameof(GregorianData.Diff), MemberType = typeof(GregorianData))]
    //public void CountYearsSince(Yemoda xstart, Yemoda xend, int years, int months, int days)
    //{
    //    var start = new CalendarYear(xstart.ToCalendarDate(CalendarUT));
    //    var end = new CalendarYear(xend.ToCalendarDate(CalendarUT));
    //    // Act & Assert
    //    Assert.Equal(years, end.CountYearsSince(start));
    //    Assert.Equal(years, end - start);
    //}

    [Theory, MemberData(nameof(YearInfoData))]
    public void CountYearsSince_WhenSame_IsZero(YearInfo info)
    {
        var year = CalendarUT.GetCalendarYear(info.Year);
        // Act & Assert
        Assert.Equal(0, year.CountYearsSince(year));
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void CountYearsSince(YemodaPairAnd<int> info)
    {
        int ys = info.Value;
        var year = CalendarUT.GetCalendarYear(info.First.Year);
        var other = CalendarUT.GetCalendarYear(info.Second.Year);
        // Act & Assert
        // 1) other - year = ys.
        Assert.Equal(ys, other - year);
        Assert.Equal(ys, other.CountYearsSince(year));
        // 2) year - other = -ys.
        Assert.Equal(-ys, year - other);
        Assert.Equal(-ys, year.CountYearsSince(other));
    }
}
