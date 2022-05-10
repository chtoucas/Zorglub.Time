// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Core;

using Zorglub.Testing.Data;

// WARNING: formally the test methods could be static, but then the tests won't
// be run by Xunit.

public abstract class IEpagomenalFeaturetteFacts<T, TDataSet> : CalendricalDataConsumer<TDataSet>
    where T : IEpagomenalFeaturette
    where TDataSet : ICalendricalDataSet, IEpagomenalDataSet, ISingleton<TDataSet>
{
    protected IEpagomenalFeaturetteFacts() { }

    public static TheoryData<EpagomenalDayInfo> EpagomenalDayInfoData => DataSet.EpagomenalDayInfoData;

    [Theory, MemberData(nameof(DateInfoData))]
    public void IsEpagomenalDay(DateInfo info)
    {
        // Arrange
        var (y, m, d) = info.Yemoda;
        // Act
        bool isEpagomenal = T.IsEpagomenalDay(y, m, d, out int epanum);
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
        // Arrange
        var (y, m, d, epanum) = info;
        // Act
        bool isEpagomenal = T.IsEpagomenalDay(y, m, d, out int epanumA);
        // Assert
        Assert.True(isEpagomenal);
        Assert.Equal(epanum, epanumA);
    }
}
