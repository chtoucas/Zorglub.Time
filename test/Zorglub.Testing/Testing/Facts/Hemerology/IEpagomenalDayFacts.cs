// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Hemerology;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;

public abstract class IEpagomenalDayFacts<TDate, TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDate : IEpagomenalDay<TDate>
    where TDataSet : ICalendarDataSet, IEpagomenalDataSet, ISingleton<TDataSet>
{
    protected IEpagomenalDayFacts() { }

    protected abstract TDate GetDate(int y, int m, int d);

    public static DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => DataSet.EpagomenalDayInfoData;

    [Theory, MemberData(nameof(DateInfoData))]
    public void IsEpagomenal(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act
        bool isEpagomenal = date.IsEpagomenal(out int epanum);
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
    public void IsEpagomenal_EpagomenalNumber(YemodaAnd<int> info)
    {
        var (y, m, d, epanum) = info;
        var date = GetDate(y, m, d);
        // Act
        bool isEpagomenal = date.IsEpagomenal(out int epanumA);
        // Assert
        Assert.True(isEpagomenal);
        Assert.Equal(epanum, epanumA);
    }
}
