// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Simple;

[Obsolete("TO BE REMOVED")]
internal abstract class IEpagomenalCalendarFacts<TCalendar, TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TCalendar : SimpleCalendar, IEpagomenalCalendar<CalendarDate>
    where TDataSet : ICalendarDataSet, IEpagomenalDataSet, ISingleton<TDataSet>
{
    private readonly TCalendar _calendar;
    private readonly SimpleCalendar _otherCalendar;

    protected IEpagomenalCalendarFacts(TCalendar calendar, SimpleCalendar otherCalendar)
    {
        _calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
        _otherCalendar = otherCalendar ?? throw new ArgumentNullException(nameof(otherCalendar));
    }

    public static DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => DataSet.EpagomenalDayInfoData;

    [Fact]
    public void IsEpagomenalDay_InvalidDate()
    {
        var date = _otherCalendar.GetCalendarDate(1, 1, 1);
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _calendar.IsEpagomenalDay(date, out _));
    }

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
    public void IsEpagomenalDay_EpagomenalNumber(YemodaAnd<int> info)
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
