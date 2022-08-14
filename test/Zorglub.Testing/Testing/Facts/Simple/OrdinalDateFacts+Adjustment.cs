// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Testing.Facts.Hemerology;
using Zorglub.Time.Simple;

#if true

public abstract class OrdinalDateAdjustmentFacts<TDataSet> :
    IDateAdjusterFacts<OrdinalDate, TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected OrdinalDateAdjustmentFacts(SimpleCalendar calendar)
        : base(new OrdinalDateAdjuster(calendar))
    {
        Debug.Assert(calendar != null);

        Calendar = calendar;
    }

    protected SimpleCalendar Calendar { get; }

    protected sealed override OrdinalDate GetDate(int y, int m, int d) => Calendar.GetCalendarDate(y, m, d).ToOrdinalDate();
    protected sealed override OrdinalDate GetDate(int y, int doy) => Calendar.GetOrdinalDate(y, doy);
}

#else

public abstract partial class OrdinalDateAdjustmentFacts<TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected OrdinalDateAdjustmentFacts(SimpleCalendar calendar)
    {
        Debug.Assert(calendar != null);

        CalendarUT = calendar;
        SupportedYearsTester = new SupportedYearsTester(calendar.YearsValidator.Range);
    }

    protected SimpleCalendar CalendarUT { get; }

    protected SupportedYearsTester SupportedYearsTester { get; }

    protected OrdinalDate GetDate(int y, int doy) => CalendarUT.GetOrdinalDate(y, doy);
}

public partial class OrdinalDateAdjustmentFacts<TDataSet> // WithYear()
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void WithYear_InvalidYears(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = GetDate(y, doy);
        // Act & Assert
        SupportedYearsTester.TestInvalidYear(date.WithYear, "newYear");
    }

    [Fact]
    public void WithYear_ValidYears()
    {
        foreach (int y in SupportedYearsTester.ValidYears)
        {
            var date = GetDate(1, 1);
            var exp = GetDate(y, 1);
            // Act & Assert
            Assert.Equal(exp, date.WithYear(y));
        }
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void WithYear_Invariance(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = GetDate(y, doy);
        // Act & Assert
        Assert.Equal(date, date.WithYear(y));
    }

    // NB: disabled because this cannot work in case the matching day in year 1
    // is not valid (leap vs common year). Nevertheless I keep it around just to
    // remind me that I should not try to create it again.
    //[Theory, MemberData(nameof(DateInfoData))]
    //public void WithYear(DateInfo info)
    //{
    //    var (y, doy) = info.Yedoy;
    //    var date = GetDate(1, doy);
    //    var exp = GetDate(y, doy);
    //    // Act & Assert
    //    Assert.Equal(exp, date.WithYear(y));
    //}
}

public partial class OrdinalDateAdjustmentFacts<TDataSet> // WithDayOfYear()
{
    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void WithDayOfYear_InvalidDayOfYear(int y, int newDayOfYear)
    {
        var date = GetDate(y, 1);
        // Act & Assert
        Assert.ThrowsAoorexn("newDayOfYear", () => date.WithDayOfYear(newDayOfYear));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void WithDayOfYear_Invariance(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = GetDate(y, doy);
        // Act & Assert
        Assert.Equal(date, date.WithDayOfYear(doy));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void WithDayOfYear(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = GetDate(y, 1);
        var exp = GetDate(y, doy);
        // Act & Assert
        Assert.Equal(exp, date.WithDayOfYear(doy));
    }
}

#endif
