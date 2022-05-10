// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Simple;

public abstract class IEpagomenalCalendarFacts<TCalendar, TDataSet> : CalendarDataConsumer<TDataSet>
    where TCalendar : Calendar, IEpagomenalCalendar<CalendarDate>
    where TDataSet : ICalendarDataSet, IEpagomenalDataSet, ISingleton<TDataSet>
{
    private readonly TCalendar _calendar;

    protected IEpagomenalCalendarFacts(TCalendar calendar!!)
    {
        _calendar = calendar;
    }

    public static TheoryData<EpagomenalDayInfo> EpagomenalDayInfoData => DataSet.EpagomenalDayInfoData;

    [Theory, MemberData(nameof(DateInfoData))]
    public void IsEpagomenalDay(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = _calendar.GetCalendarDate(y, m, d);
        // Act
        bool isEpagomenal = _calendar.IsEpagomenalDay(date, out int epanum);
        // Assert
        Assert.Equal(info.IsSupplementary, isEpagomenal);
        if (isEpagomenal)
        {
            Assert.True(epanum > 0);
        }
        else
        {
            Assert.Equal(0, epanum);
        }
    }

    [Theory, MemberData(nameof(EpagomenalDayInfoData))]
    public void IsEpagomenalDay_EpagomenalNumber(EpagomenalDayInfo info)
    {
        var (y, m, d, epanum) = info;
        var date = _calendar.GetCalendarDate(y, m, d);
        // Act
        bool isEpagomenal = _calendar.IsEpagomenalDay(date, out int epanumA);
        // Assert
        Assert.True(isEpagomenal);
        Assert.Equal(epanum, epanumA);
    }
}
