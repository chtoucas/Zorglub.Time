// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Horology;

public static partial class InstantOfDayTests
{
    /// <summary>Hour, Minute, Second, SecondOfDay, FractionOfDay</summary>
    public static readonly TheoryData<int, int, int, int, decimal> RationalData = new()
    {
        // Cas simples.
        { 0, 0, 0, 0, 0m },

        { 12, 0, 0, 43_200, .5m },
        { 6, 0, 0, 21_600, .25m },
        { 3, 0, 0, 10_800, .125m },
        { 1, 30, 0, 5_400, .0625m },
        { 0, 45, 0, 2_700, .03125m },
        { 0, 22, 30, 1_350, .015625m },
        { 0, 11, 15, 675, .0078125m },
        { 0, 2, 15, 135, .0015625m },
        { 0, 0, 27, 27, .0003125m },    // plus petite valeur rationnelle

        { 0, 0, 54, 54, .000625m },

        { 22, 30, 0, 81_000, .9375m },
        { 7, 30, 27, 27_027, .3128125m },
        { 2, 24, 0, 8_640, .1m },
    };

    public static readonly TheoryData<int, int, int, int, decimal> ComplexData = new()
    {
        { 0, 0, 9, 9, .0001041666666666666666666667m },
        { 0, 0, 3, 3, .0000347222222222222222222222m },
        { 0, 0, 2, 2, .0000231481481481481481481481m },
        { 0, 0, 1, 1, .0000115740740740740740740740m },
    };
}

public partial class InstantOfDayTests
{
    [Theory, MemberData(nameof(RationalData))]
    public static void FromHourMinuteSecond(int h, int m, int s, int _4, decimal _5)
    {
        // Act
        var time = InstantOfDay.FromHourMinuteSecond(h, m, s);
        // Assert
        Assert.Equal(h, time.Hour);
        Assert.Equal(m, time.Minute);
        Assert.Equal(s, time.Second);
    }

    [Theory, MemberData(nameof(RationalData))]
    public static void FromHourMinuteSecondMillisecond(int h, int m, int s, int _4, decimal _5)
    {
        // Act
        var time = InstantOfDay.FromHourMinuteSecondMillisecond(h, m, s, 345);
        // Assert
        Assert.Equal(h, time.Hour);
        Assert.Equal(m, time.Minute);
        Assert.Equal(s, time.Second);
        Assert.Equal(345, time.Millisecond);
    }

    [Theory, MemberData(nameof(RationalData))]
    public static void Deconstructor(int h, int m, int s, int _4, decimal _5)
    {
        // Act
        var (hA, mA, sA) = InstantOfDay.FromHourMinuteSecond(h, m, s);
        // Assert
        Assert.Equal(h, hA);
        Assert.Equal(m, mA);
        Assert.Equal(s, sA);
    }

    [Theory, MemberData(nameof(RationalData))]
    public static void Deconstructor﹍Nanosecond(int h, int m, int s, int _4, decimal _5)
    {
        // Act
        var (hA, mA, sA, nsA) = InstantOfDay.FromHourMinuteSecondNanosecond(h, m, s, 345);
        // Assert
        Assert.Equal(h, hA);
        Assert.Equal(m, mA);
        Assert.Equal(s, sA);
        Assert.Equal(345, nsA);
    }

    [Theory, MemberData(nameof(RationalData))]
    public static void FromSecondOfDay(int h, int m, int s, int secondOfDay, decimal _5)
    {
        // Act
        var time = InstantOfDay.FromSecondOfDay(secondOfDay);
        // Assert
        Assert.Equal(h, time.Hour);
        Assert.Equal(m, time.Minute);
        Assert.Equal(s, time.Second);
        Assert.Equal(secondOfDay, time.SecondOfDay);
    }

    [Theory, MemberData(nameof(RationalData))]
    public static void FromMillisecondOfDay(int h, int m, int s, int secondOfDay, decimal _5)
    {
        // Act
        var time = InstantOfDay.FromMillisecondOfDay(1000 * secondOfDay);
        // Assert
        Assert.Equal(h, time.Hour);
        Assert.Equal(m, time.Minute);
        Assert.Equal(s, time.Second);
        Assert.Equal(secondOfDay, time.SecondOfDay);
    }

    [Theory, MemberData(nameof(RationalData))]
    public static void SecondOfDay(int h, int m, int s, int secondOfDay, decimal _5)
    {
        var time = InstantOfDay.FromHourMinuteSecond(h, m, s);
        // Assert
        Assert.Equal(secondOfDay, time.SecondOfDay);
    }

    [Theory, MemberData(nameof(RationalData))]
    public static void ToDecimal(int h, int m, int s, int _4, decimal fractionOfDay)
    {
        var time = InstantOfDay.FromHourMinuteSecond(h, m, s);
        // Assert
        Assert.Equal(fractionOfDay, time.ToDecimal());
    }
}
