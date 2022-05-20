// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts;

// Hypothesis: first year is valid and complete.

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides facts about <see cref="IDateable{TSelf}"/>.
/// </summary>
public abstract partial class IDateableFacts<TDate, TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TDate : IDateable<TDate>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected IDateableFacts() { }

    protected abstract TDate GetDate(int y, int m, int d);
}

public partial class IDateableFacts<TDate, TDataSet> // Prelude
{
    [Theory, MemberData(nameof(CenturyInfoData))]
    public void CenturyOfEra_Prop(CenturyInfo info)
    {
        var (y, century, _) = info;
        var date = GetDate(y, 1, 1);
        var centuryOfEra = Ord.Zeroth + century;
        // Act & Assert
        Assert.Equal(centuryOfEra, date.CenturyOfEra);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void Century_Prop(CenturyInfo info)
    {
        var (y, century, _) = info;
        var date = GetDate(y, 1, 1);
        // Act & Assert
        Assert.Equal(century, date.Century);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void YearOfEra_Prop(CenturyInfo info)
    {
        int y = info.Year;
        var date = GetDate(y, 1, 1);
        var yearOfEra = Ord.Zeroth + y;
        // Act & Assert
        Assert.Equal(yearOfEra, date.YearOfEra);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void YearOfCentury_Prop(CenturyInfo info)
    {
        var (y, _, yearOfCentury) = info;
        var date = GetDate(y, 1, 1);
        // Act & Assert
        Assert.Equal(yearOfCentury, date.YearOfCentury);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void DayOfYear_Prop(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(doy, date.DayOfYear);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void IsIntercalary_Prop(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(info.IsIntercalary, date.IsIntercalary);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void IsSupplementary_Prop(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(info.IsSupplementary, date.IsSupplementary);
    }
}

public partial class IDateableFacts<TDate, TDataSet> // Counting
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void CountElapsedDaysInYear(DateInfo info)
    {
        var (y, m, d, doy) = info;
        int daysInYearBefore = doy - 1;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(daysInYearBefore, date.CountElapsedDaysInYear());
    }

    [Theory, MemberData(nameof(DaysInYearAfterDateData))]
    public void CountRemainingDaysInYear(YemodaAnd<int> info)
    {
        var (y, m, d, daysInYearAfter) = info;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(daysInYearAfter, date.CountRemainingDaysInYear());
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountElapsedDaysInMonth(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        int daysInMonthBefore = d - 1;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(daysInMonthBefore, date.CountElapsedDaysInMonth());
    }

    [Theory, MemberData(nameof(DaysInMonthAfterDateData))]
    public void CountRemainingDaysInMonth(YemodaAnd<int> info)
    {
        var (y, m, d, daysInMonthAfter) = info;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(daysInMonthAfter, date.CountRemainingDaysInMonth());
    }
}

public partial class IDateableFacts<TDate, TDataSet> // Year and month boundaries
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void GetStartOfYear(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        var startOfYear = GetDate(y, 1, 1);
        // Act & Assert
        Assert.Equal(startOfYear, TDate.GetStartOfYear(date));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetEndOfYear(YearInfo info)
    {
        int y = info.Year;
        var date = GetDate(y, 1, 1);
        // Act
        var endOfYear = TDate.GetEndOfYear(date);
        // Assert
        Assert.Equal(y, endOfYear.Year);
        Assert.Equal(info.DaysInYear, endOfYear.DayOfYear);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetStartOfMonth(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        var startOfMonth = GetDate(y, m, 1);
        // Act & Assert
        Assert.Equal(startOfMonth, TDate.GetStartOfMonth(date));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var date = GetDate(y, m, 1);
        var endOfMonth = GetDate(y, m, info.DaysInMonth);
        // Act & Assert
        Assert.Equal(endOfMonth, TDate.GetEndOfMonth(date));
    }
}
