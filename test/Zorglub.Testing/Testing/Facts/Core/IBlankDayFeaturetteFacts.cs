// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Core;

using Zorglub.Testing.Data;

// WARNING: formally the test methods could be static, but then the tests won't
// be run by Xunit.

public abstract class IBlankDayFeaturetteFacts<T, TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where T : IBlankDayFeaturette
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected IBlankDayFeaturetteFacts() { }

    [Theory, MemberData(nameof(DateInfoData))]
    public void IsBlankDay(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        bool actual = T.IsBlankDay(y, m, d);
        // Assert
        Assert.Equal(info.IsSupplementary, actual);
    }
}
