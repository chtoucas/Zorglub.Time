﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Testing.Facts.Hemerology;
using Zorglub.Time.Simple;

// In addition, one should test WithYear() with valid and invalid results.

// TODO(fact): remove old tests here and in CalendarDateFacts.
// Do the same with OrdinalDate and CalendarDay.

#if true

public abstract class CalendarDateAdjustmentFacts<TDataSet> :
    IDateAdjusterFacts<CalendarDate, TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarDateAdjustmentFacts(SimpleCalendar calendar)
        : base(new CalendarDateAdjuster(calendar))
    {
        Debug.Assert(calendar != null);

        Calendar = calendar;
    }

    protected SimpleCalendar Calendar { get; }

    protected sealed override CalendarDate GetDate(int y, int m, int d) => Calendar.GetCalendarDate(y, m, d);
    protected sealed override CalendarDate GetDate(int y, int doy) => Calendar.GetOrdinalDate(y, doy).ToCalendarDate();
}

#else

public abstract partial class CalendarDateAdjustmentFacts<TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarDateAdjustmentFacts(SimpleCalendar calendar)
    {
        Debug.Assert(calendar != null);

        CalendarUT = calendar;
        SupportedYearsTester = new SupportedYearsTester(calendar.YearsValidator.Range);
    }

    protected SimpleCalendar CalendarUT { get; }

    protected SupportedYearsTester SupportedYearsTester { get; }

    protected CalendarDate GetDate(int y, int m, int d) => CalendarUT.GetCalendarDate(y, m, d);
}

public partial class CalendarDateAdjustmentFacts<TDataSet> // WithYear()
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void WithYear_InvalidYears(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act & Assert
        SupportedYearsTester.TestInvalidYear(date.WithYear, "newYear");
    }

    [Fact]
    public void WithYear_ValidYears()
    {
        foreach (int y in SupportedYearsTester.ValidYears)
        {
            var date = GetDate(1, 1, 1);
            var exp = GetDate(y, 1, 1);
            // Act & Assert
            Assert.Equal(exp, date.WithYear(y));
        }
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void WithYear_Invariance(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, date.WithYear(y));
    }

    // NB: disabled because this cannot work in case the matching day in year 1
    // is not valid. Nevertheless I keep it around just to remind me that I
    // should not try to create it again.
    //[Theory, MemberData(nameof(DateInfoData))]
    //public void WithYear(DateInfo info)
    //{
    //    var (y, m, d) = info.Yemoda;
    //    var date = GetDate(1, m, d);
    //    var exp = GetDate(y, m, d);
    //    // Act & Assert
    //    Assert.Equal(exp, date.WithYear(y));
    //}
}

public partial class CalendarDateAdjustmentFacts<TDataSet> // WithMonth()
{
    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void WithMonth_InvalidMonth(int y, int newMonth)
    {
        var date = GetDate(y, 1, 1);
        // Act & Assert
        Assert.ThrowsAoorexn("newMonth", () => date.WithMonth(newMonth));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void WithMonth_Invariance(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, date.WithMonth(m));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void WithMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var date = GetDate(y, 1, 1);
        var exp = GetDate(y, m, 1);
        // Act & Assert
        Assert.Equal(exp, date.WithMonth(m));
    }
}

public partial class CalendarDateAdjustmentFacts<TDataSet> // WithDay()
{
    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void WithDay_InvalidDay(int y, int m, int newDay)
    {
        var date = GetDate(y, m, 1);
        // Act & Assert
        Assert.ThrowsAoorexn("newDay", () => date.WithDay(newDay));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void WithDay_Invariance(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, date.WithDay(d));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void WithDay(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, 1);
        var exp = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(exp, date.WithDay(d));
    }
}

#endif
