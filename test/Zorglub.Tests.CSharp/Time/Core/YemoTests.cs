// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core;

public static class YemoTests
{
    private const int YearMonthBits = 32 - Yemoda.DayBits;
    private const int MinYearMonth = -(1 << (YearMonthBits - 1));    // -33_554_432
    private const int MaxYearMonth = (1 << (YearMonthBits - 1)) - 1; //  33_554_431
    //private const int MinBin = MinYearMonth << Yemoda.MonthShift;
    //private const int MaxBin = MaxYearMonth << Yemoda.MonthShift;

    [Theory]
    [InlineData(MinYearMonth, 1)]
    [InlineData(-10_000_008, 2)]
    [InlineData(-1000_007, 3)]
    [InlineData(-100_006, 4)]
    [InlineData(-10_005, 5)]
    [InlineData(-1004, 6)]
    [InlineData(-103, 7)]
    [InlineData(-12, 8)]
    [InlineData(-1, 9)]
    [InlineData(0, 10)]
    [InlineData(1, 11)]
    [InlineData(12, 12)]
    [InlineData(103, 13)]
    [InlineData(1004, 14)]
    [InlineData(10_005, 15)]
    [InlineData(100_006, 16)]
    [InlineData(1000_007, 17)]
    [InlineData(10_000_008, 18)]
    [InlineData(MaxYearMonth, 19)]
    public static void FromBinary_InvalidBin_WhenDayBitsAreNotZero(int ym, int d)
    {
        Debug.Assert(MinYearMonth <= ym);
        Debug.Assert(ym <= MaxYearMonth);

        // Arrange
        int bin = (ym << Yemoda.MonthShift) | d;
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Yemo.FromBinary(bin));
    }

    [Theory]
    [InlineData(Int32.MinValue, 1)]
    [InlineData(-1000_000_000, 2)]
    [InlineData(-100_000_009, 3)]
    [InlineData(-10_000_008, 4)]
    [InlineData(-1000_007, 5)]
    [InlineData(-100_006, 6)]
    [InlineData(-10_005, 7)]
    [InlineData(-1004, 8)]
    [InlineData(-103, 9)]
    [InlineData(-12, 10)]
    [InlineData(-1, 11)]
    [InlineData(0, 12)]
    [InlineData(1, 13)]
    [InlineData(12, 14)]
    [InlineData(103, 15)]
    [InlineData(1004, 16)]
    [InlineData(10_005, 17)]
    [InlineData(100_006, 18)]
    [InlineData(1000_007, 19)]
    [InlineData(10_000_008, 20)]
    [InlineData(100_000_009, 21)]
    [InlineData(1000_000_000, 22)]
    [InlineData(Int32.MaxValue, 23)]
    public static void FromBinary_InvalidBin_WhenDayBitsAreNotZero_ViaYemoda(int b, int d)
    {
        // Arrange
        int bin = new Yemoda(b).Yemo.ToBinary() | d;
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Yemo.FromBinary(bin));
    }

    [Theory]
    [InlineData(MinYearMonth)]
    [InlineData(-10_000_008)]
    [InlineData(-1000_007)]
    [InlineData(-100_006)]
    [InlineData(-10_005)]
    [InlineData(-1004)]
    [InlineData(-103)]
    [InlineData(-12)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(12)]
    [InlineData(103)]
    [InlineData(1004)]
    [InlineData(10_005)]
    [InlineData(100_006)]
    [InlineData(1000_007)]
    [InlineData(10_000_008)]
    [InlineData(MaxYearMonth)]
    public static void Serialization_RoundTrip(int ym)
    {
        Debug.Assert(MinYearMonth <= ym);
        Debug.Assert(ym <= MaxYearMonth);

        // Arrange
        int bin = ym << Yemoda.MonthShift;
        // Act
        int binA = Yemo.FromBinary(bin).ToBinary();
        // Assert
        Assert.Equal(bin, binA);
    }
}

public static class YemoxTests
{
    private const int YearMonthBits = Yemodax.YearBits + Yemoda.MonthBits;
    private const int MinYearMonth = -(1 << (YearMonthBits - 1));   // -262_144
    private const int MaxYearMonth = (1 << (YearMonthBits - 1)) - 1; // 262_143
    //private const int MinBin = (MinYearMonth << Yemodax.MonthShift) | Yemodax.MinExtra; // MinExtra = 0...
    //private const int MaxBin = (MaxYearMonth << Yemodax.MonthShift) | Yemodax.MaxExtra;

    [Theory]
    [InlineData(MinYearMonth, 1, 15)]
    [InlineData(-100_006, 2, 14)]
    [InlineData(-10_005, 3, 13)]
    [InlineData(-1004, 4, 12)]
    [InlineData(-103, 5, 11)]
    [InlineData(-12, 6, 10)]
    [InlineData(-1, 7, 9)]
    [InlineData(0, 8, 8)]
    [InlineData(1, 9, 7)]
    [InlineData(12, 10, 6)]
    [InlineData(103, 11, 5)]
    [InlineData(1004, 12, 4)]
    [InlineData(10_005, 13, 3)]
    [InlineData(100_006, 14, 2)]
    [InlineData(MaxYearMonth, 15, 1)]
    public static void FromBinary_InvalidBin_WhenDayBitsAreNotZero(int ym, int i, int j)
    {
        Debug.Assert(MinYearMonth <= ym);
        Debug.Assert(ym <= MaxYearMonth);

        // Arrange
        int bin1 = (ym << Yemodax.MonthShift) | (i << Yemodax.DayShift) | j;
        int bin2 = (ym << Yemodax.MonthShift) | (j << Yemodax.DayShift) | i;
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Yemox.FromBinary(bin1));
        Assert.Throws<ArgumentException>(() => Yemox.FromBinary(bin2));
    }

    [Theory]
    [InlineData(Int32.MinValue, 1, 23)]
    [InlineData(-1000_000_000, 2, 22)]
    [InlineData(-100_000_009, 3, 21)]
    [InlineData(-10_000_008, 4, 20)]
    [InlineData(-1000_007, 5, 19)]
    [InlineData(-100_006, 6, 18)]
    [InlineData(-10_005, 7, 17)]
    [InlineData(-1004, 8, 16)]
    [InlineData(-103, 9, 15)]
    [InlineData(-12, 10, 14)]
    [InlineData(-1, 11, 13)]
    [InlineData(0, 12, 12)]
    [InlineData(1, 13, 11)]
    [InlineData(12, 14, 10)]
    [InlineData(103, 15, 9)]
    [InlineData(1004, 16, 8)]
    [InlineData(10_005, 17, 7)]
    [InlineData(100_006, 18, 6)]
    [InlineData(1000_007, 19, 5)]
    [InlineData(10_000_008, 20, 4)]
    [InlineData(100_000_009, 21, 3)]
    [InlineData(1000_000_000, 22, 2)]
    [InlineData(Int32.MaxValue, 23, 1)]
    public static void FromBinary_InvalidBin_WhenDayBitsAreNotZero_ViaYemoda(int b, int i, int j)
    {
        // Arrange
        int bin1 = new Yemoda(b).Yemo.ToBinary() | (i << Yemodax.DayShift) | j;
        int bin2 = new Yemoda(b).Yemo.ToBinary() | (j << Yemodax.DayShift) | i;
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Yemox.FromBinary(bin1));
        Assert.Throws<ArgumentException>(() => Yemox.FromBinary(bin2));
    }

    [Theory]
    [InlineData(MinYearMonth, 0)]
    [InlineData(-100_006, 1)]
    [InlineData(-10_005, 2)]
    [InlineData(-1004, 3)]
    [InlineData(-103, 4)]
    [InlineData(-12, 5)]
    [InlineData(-1, 6)]
    [InlineData(0, 7)]
    [InlineData(1, 8)]
    [InlineData(12, 9)]
    [InlineData(103, 10)]
    [InlineData(1004, 11)]
    [InlineData(10_005, 12)]
    [InlineData(100_006, 13)]
    [InlineData(MaxYearMonth, 14)]
    public static void Serialization_RoundTrip(int ym, int x)
    {
        Debug.Assert(MinYearMonth <= ym);
        Debug.Assert(ym <= MaxYearMonth);

        // Arrange
        int bin = (ym << Yemodax.MonthShift) | x;
        // Act
        int binA = Yemox.FromBinary(bin).ToBinary();
        // Assert
        Assert.Equal(bin, binA);
    }
}
