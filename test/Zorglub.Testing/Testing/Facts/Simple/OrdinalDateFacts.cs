// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

// Tests indirects : OrdinalDate utilise la repr Yedoy mais ici on en
// passe systématiquement par CreateDate(y, m, d).

/// <summary>
/// Provides facts about <see cref="OrdinalDate"/>.
/// </summary>
[TestExcludeFrom(TestExcludeFrom.Smoke)] // Indirect tests
public abstract partial class OrdinalDateFacts<TDataSet> : IDateFacts<OrdinalDate, TDataSet>
    where TDataSet :
        ICalendarDataSet,
        IDaysAfterDataSet,
        IMathDataSet,
        IDayOfWeekDataSet,
        ISingleton<TDataSet>
{
    protected OrdinalDateFacts(Calendar calendar) : this(calendar, CreateCtorArgs(calendar)) { }

    private OrdinalDateFacts(Calendar calendar, CtorArgs args) : base(args)
    {
        CalendarUT = calendar;

        (MinDate, MaxDate) = calendar.MinMaxOrdinal;
    }

    protected Calendar CalendarUT { get; }

    protected sealed override OrdinalDate MinDate { get; }
    protected sealed override OrdinalDate MaxDate { get; }

    protected sealed override OrdinalDate CreateDate(int y, int m, int d) =>
        CalendarUT.GetCalendarDate(y, m, d).ToOrdinalDate();
}

public partial class OrdinalDateFacts<TDataSet>
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void Deconstructor(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Arrange
        var date = CalendarUT.GetOrdinalDate(y, doy);
        // Act
        var (year, month, day) = date;
        // Assert
        Assert.Equal(y, year);
        Assert.Equal(m, month);
        Assert.Equal(d, day);
    }
}

public partial class OrdinalDateFacts<TDataSet> // Properties
{
    // We also test the internal prop Cuid.
    [Theory, MemberData(nameof(DateInfoData))]
    public void Calendar_Prop(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        // Arrange
        var date = CalendarUT.GetOrdinalDate(y, doy);
        // Act & Assert
        Assert.Equal(CalendarUT, date.Calendar);
        Assert.Equal(CalendarUT.Id, date.Cuid);
    }
}

public partial class OrdinalDateFacts<TDataSet> // Conversions
{
    //public void ToCalendarDay(DayNumberInfo info)

    [Theory, MemberData(nameof(DateInfoData))]
    public void ToCalendarDate(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Arrange
        var odate = CalendarUT.GetOrdinalDate(y, doy);
        var date = CalendarUT.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, odate.ToCalendarDate());
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void ToOrdinalDate(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        // Arrange
        var odate = CalendarUT.GetOrdinalDate(y, doy);
        // Act & Assert
        Assert.Equal(odate, ((ISimpleDate)odate).ToOrdinalDate());
    }

    //public void WithCalendar_NullCalendar()
}
