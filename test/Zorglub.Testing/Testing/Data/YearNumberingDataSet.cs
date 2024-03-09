// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Testing.Data;

public static class YearNumberingDataSet
{
    public static TheoryData<DecadeOfCenturyInfo> DecadeOfCenturyInfoData { get; } = new()
    {
        new(-109, -1, 10, 1),
        new(-100, -1, 10, 10),

        new(-99, 0, 1, 1),
        new(-90, 0, 1, 10),

        new(-19, 0, 9, 1),
        new(-10, 0, 9, 10),

        new(-9, 0, 10, 1),
        new(0, 0, 10, 10),

        // First decade.
        new(1, 1, 1, 1),
        new(10, 1, 1, 10),

        new(11, 1, 2, 1),
        new(20, 1, 2, 10),

        new(91, 1, 10, 1),
        new(100, 1, 10, 10),

        new(101, 2, 1, 1),
        new(110, 2, 1, 10),
    };

    public static TheoryData<DecadeOfCenturyInfo> IsoDecadeOfCenturyInfoData { get; } = new()
    {
        new(-110, -1, 10, 1),
        new(-101, -1, 10, 10),

        new(-100, 0, 1, 1),
        new(-91, 0, 1, 10),

        new(-20, 0, 9, 1),
        new(-11, 0, 9, 10),

        new(-10, 0, 10, 1),
        new(-1, 0, 10, 10),

        // First decade.
        new(0, 1, 1, 1),
        new(1, 1, 1, 2),
        new(9, 1, 1, 10),

        new(10, 1, 2, 1),
        new(19, 1, 2, 10),

        new(90, 1, 10, 1),
        new(99, 1, 10, 10),

        new(100, 2, 1, 1),
        new(109, 2, 1, 10),
    };

    public static TheoryData<DecadeInfo> DecadeInfoData { get; } = new()
    {
        new(-109, -10, 1),
        new(-100, -10, 10),

        new(-19, -1, 1),
        new(-10, -1, 10),

        new(-9, 0, 1),
        new(0, 0, 10),

        // First decade.
        new(1, 1, 1),
        new(10, 1, 10),

        new(11, 2, 1),
        new(20, 2, 10),

        new(101, 11, 1),
        new(110, 11, 10),
    };

    public static TheoryData<DecadeInfo> IsoDecadeInfoData { get; } = new()
    {
        new(-110, -10, 1),
        new(-101, -10, 10),

        new(-20, -1, 1),
        new(-11, -1, 10),

        new(-10, 0, 1),
        new(-1, 0, 10),

        // First decade.
        new(0, 1, 1),
        new(1, 1, 2),
        new(9, 1, 10),

        new(10, 2, 1),
        new(19, 2, 10),

        new(100, 11, 1),
        new(109, 11, 10),
    };

    public static DataGroup<CenturyInfo> CenturyInfoData { get; } = new()
    {
        new(-1099, -10, 1),
        new(-1000, -10, 100),

        new(-199, -1, 1),
        new(-100, -1, 100),

        new(-99, 0, 1),
        new(0, 0, 100),

        // First century.
        new(1, 1, 1),
        new(100, 1, 100),

        new(101, 2, 1),
        new(102, 2, 2),
        new(103, 2, 3),
        new(104, 2, 4),
        new(105, 2, 5),
        new(106, 2, 6),
        new(107, 2, 7),
        new(108, 2, 8),
        new(109, 2, 9),
        new(110, 2, 10),
        new(200, 2, 100),

        new(1901, 20, 1),
        new(2000, 20, 100),

        new(2001, 21, 1),
        new(2100, 21, 100),
    };

    public static TheoryData<CenturyInfo> IsoCenturyInfoData { get; } = new()
    {
        new(-1100, -10, 1),
        new(-1001, -10, 100),

        new(-200, -1, 1),
        new(-101, -1, 100),

        new(-100, 0, 1),
        new(-1, 0, 100),

        // First century.
        new(0, 1, 1),
        new(1, 1, 2),
        new(99, 1, 100),

        new(100, 2, 1),
        new(101, 2, 2),
        new(102, 2, 3),
        new(103, 2, 4),
        new(104, 2, 5),
        new(105, 2, 6),
        new(106, 2, 7),
        new(107, 2, 8),
        new(108, 2, 9),
        new(109, 2, 10),
        new(199, 2, 100),

        new(1900, 20, 1),
        new(1999, 20, 100),

        new(2000, 21, 1),
        new(2099, 21, 100),
    };

    public static TheoryData<MillenniumInfo> MillenniumInfoData { get; } = new()
    {
        new(-2999, -2, 1),
        new(-2000, -2, 1000),

        new(-1999, -1, 1),
        new(-1000, -1, 1000),

        new(-999, 0, 1),
        new(0, 0, 1000),

        // First millennium.
        new(1, 1, 1),
        new(1000, 1, 1000),

        new(1001, 2, 1),
        new(2000, 2, 1000),

        new(2001, 3, 1),
        new(3000, 3, 1000),
    };

    public static TheoryData<MillenniumInfo> IsoMillenniumInfoData { get; } = new()
    {
        new(-3000, -2, 1),
        new(-2001, -2, 1000),

        new(-2000, -1, 1),
        new(-1001, -1, 1000),

        new(-1000, 0, 1),
        new(-1, 0, 1000),

        // First millennium.
        new(0, 1, 1),
        new(1, 1, 2),
        new(999, 1, 1000),

        new(1000, 2, 1),
        new(1999, 2, 1000),

        new(2000, 3, 1),
        new(2999, 3, 1000),
    };
}
