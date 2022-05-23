// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core;

using Zorglub.Time.Core.Arithmetic;

public static class CalendricalMathTests
{
    public static readonly TheoryData<int> YearData = new()
    {
        Yemoda.MinYear,
        -1000_007,
        -100_006,
        -10_005,
        -1004,
        -103,
        -12,
        -1,
        0,
        1,
        12,
        103,
        1004,
        10_005,
        100_006,
        1000_007,
        Yemoda.MaxYear,
    };

    public static readonly TheoryData<int> InvalidYearData = new()
    {
        Int32.MinValue,
        Yemoda.MinYear - 1,
        Yemoda.MaxYear + 1,
        Int32.MaxValue,
    };

    [Theory, MemberData(nameof(InvalidYearData))]
    public static void CheckYearOverflow_InvalidYear(int y) =>
        Assert.Overflows(() => CalendricalMath.CheckYearOverflow(y));

    [Theory, MemberData(nameof(YearData))]
    public static void CheckYearOverflow(int y) =>
        CalendricalMath.CheckYearOverflow(y);
}
