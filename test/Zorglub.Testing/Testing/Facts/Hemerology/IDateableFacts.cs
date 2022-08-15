// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Hemerology;

// Hypothesis: first year is valid and complete.

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides facts about <see cref="IDateable"/>.
/// </summary>
public abstract partial class IDateableFacts<TDate, TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TDate : IDateable
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected IDateableFacts() { }

    protected abstract TDate GetDate(int y, int m, int d);
}

public partial class IDateableFacts<TDate, TDataSet> // Prelude
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void Deconstructor(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act
        var (year, month, day) = date;
        // Assert
        Assert.Equal(y, year);
        Assert.Equal(m, month);
        Assert.Equal(d, day);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Deconstructor﹍Ordinal(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var date = GetDate(y, m, d);
        // Act
        var (year, dayOfYear) = date;
        // Assert
        Assert.Equal(y, year);
        Assert.Equal(doy, dayOfYear);
    }

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
    public void Year_Prop(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(y, date.Year);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Month_Prop(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(m, date.Month);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Day_Prop(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(d, date.Day);
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
