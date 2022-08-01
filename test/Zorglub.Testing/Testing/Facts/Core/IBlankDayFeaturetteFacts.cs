// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Core;

using Zorglub.Testing.Data;

public abstract class IBlankDayFeaturetteFacts<TSchema, TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TSchema : IBlankDayFeaturette
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    private readonly TSchema _schema;

    protected IBlankDayFeaturetteFacts(TSchema schema)
    {
        Requires.NotNull(schema);
        _schema = schema;
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void IsBlankDay(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        bool actual = _schema.IsBlankDay(y, m, d);
        // Assert
        Assert.Equal(info.IsSupplementary, actual);
    }
}
