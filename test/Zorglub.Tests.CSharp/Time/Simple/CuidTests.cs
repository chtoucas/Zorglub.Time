// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

public static class CuidTests
{
    [Theory]
    [InlineData(Cuid.Zoroastrian + 1)]
    [InlineData(Cuid.MinUser)]
    [InlineData(Cuid.Max)]
    [InlineData(Cuid.Invalid)]
    internal static void IsFixed_False(Cuid id) =>
        Assert.False(id.IsFixed());

    [Theory, MemberData(nameof(EnumDataSet.CalendarIdData), MemberType = typeof(EnumDataSet))]
    public static void IsFixed_True(CalendarId id) =>
        Assert.True(((Cuid)id).IsFixed());
}
