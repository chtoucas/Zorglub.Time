// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time;

using Zorglub.Time.Horology;

public static partial class TimeOfDayTests
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

public partial class TimeOfDayTests
{
    [Theory, MemberData(nameof(RationalData))]
    public static void FromHourMinuteSecond(int h, int m, int s, int _4, decimal _5)
    {
        // Act
        var time = TimeOfDay.FromHourMinuteSecond(h, m, s);
        // Assert
        Assert.Equal(h, time.Hour);
        Assert.Equal(m, time.Minute);
        Assert.Equal(s, time.Second);
    }

    [Theory, MemberData(nameof(RationalData))]
    public static void FromHourMinuteSecond64(int h, int m, int s, int _4, decimal _5)
    {
        // Act
        var time = TimeOfDay64.FromHourMinuteSecond(h, m, s);
        // Assert
        Assert.Equal(h, time.Hour);
        Assert.Equal(m, time.Minute);
        Assert.Equal(s, time.Second);
    }

    [Theory, MemberData(nameof(RationalData))]
    public static void FromHourMinuteSecondMillisecond(int h, int m, int s, int _4, decimal _5)
    {
        // Act
        var time = TimeOfDay.FromHourMinuteSecondMillisecond(h, m, s, 345);
        // Assert
        Assert.Equal(h, time.Hour);
        Assert.Equal(m, time.Minute);
        Assert.Equal(s, time.Second);
        Assert.Equal(345, time.Millisecond);
    }

    [Theory, MemberData(nameof(RationalData))]
    public static void FromHourMinuteSecondMillisecond64(int h, int m, int s, int _4, decimal _5)
    {
        // Act
        var time = TimeOfDay64.FromHourMinuteSecondMillisecond(h, m, s, 345);
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
        var (hA, mA, sA) = TimeOfDay.FromHourMinuteSecond(h, m, s);
        // Assert
        Assert.Equal(h, hA);
        Assert.Equal(m, mA);
        Assert.Equal(s, sA);
    }

    [Theory, MemberData(nameof(RationalData))]
    public static void Deconstructor64(int h, int m, int s, int _4, decimal _5)
    {
        // Act
        var (hA, mA, sA) = TimeOfDay64.FromHourMinuteSecond(h, m, s);
        // Assert
        Assert.Equal(h, hA);
        Assert.Equal(m, mA);
        Assert.Equal(s, sA);
    }

    [Theory, MemberData(nameof(RationalData))]
    public static void Deconstructor﹍Millisecond(int h, int m, int s, int _4, decimal _5)
    {
        // Act
        var (hA, mA, sA, msA) = TimeOfDay.FromHourMinuteSecondMillisecond(h, m, s, 345);
        // Assert
        Assert.Equal(h, hA);
        Assert.Equal(m, mA);
        Assert.Equal(s, sA);
        Assert.Equal(345, msA);
    }

    [Theory, MemberData(nameof(RationalData))]
    public static void Deconstructor64﹍Nanosecond(int h, int m, int s, int _4, decimal _5)
    {
        // Act
        var (hA, mA, sA, nsA) = TimeOfDay64.FromHourMinuteSecondNanosecond(h, m, s, 345);
        // Assert
        Assert.Equal(h, hA);
        Assert.Equal(m, mA);
        Assert.Equal(s, sA);
        Assert.Equal(345, nsA);
    }

    [Theory, MemberData(nameof(RationalData))]
    public static void FromSecondsSinceMidnight(int h, int m, int s, int secondOfDay, decimal _5)
    {
        // Act
        var time = TimeOfDay.FromSecondsSinceMidnight(secondOfDay);
        // Assert
        Assert.Equal(h, time.Hour);
        Assert.Equal(m, time.Minute);
        Assert.Equal(s, time.Second);
        Assert.Equal(secondOfDay, time.SecondOfDay);
    }

    [Theory, MemberData(nameof(RationalData))]
    public static void FromSecondsSinceMidnight64(int h, int m, int s, int secondOfDay, decimal _5)
    {
        // Act
        var time = TimeOfDay64.FromSecondsSinceMidnight(secondOfDay);
        // Assert
        Assert.Equal(h, time.Hour);
        Assert.Equal(m, time.Minute);
        Assert.Equal(s, time.Second);
        Assert.Equal(secondOfDay, time.SecondOfDay);
    }

    [Theory, MemberData(nameof(RationalData))]
    public static void FromMillisecondsSinceMidnight(int h, int m, int s, int secondOfDay, decimal _5)
    {
        // Act
        var time = TimeOfDay.FromMillisecondsSinceMidnight(1000 * secondOfDay);
        // Assert
        Assert.Equal(h, time.Hour);
        Assert.Equal(m, time.Minute);
        Assert.Equal(s, time.Second);
        Assert.Equal(secondOfDay, time.SecondOfDay);
    }

    [Theory, MemberData(nameof(RationalData))]
    public static void FromMillisecondsSinceMidnight64(int h, int m, int s, int secondOfDay, decimal _5)
    {
        // Act
        var time = TimeOfDay64.FromMillisecondsSinceMidnight(1000 * secondOfDay);
        // Assert
        Assert.Equal(h, time.Hour);
        Assert.Equal(m, time.Minute);
        Assert.Equal(s, time.Second);
        Assert.Equal(secondOfDay, time.SecondOfDay);
    }

    [Theory, MemberData(nameof(RationalData))]
    public static void SecondOfDay(int h, int m, int s, int secondOfDay, decimal _5)
    {
        var time = TimeOfDay.FromHourMinuteSecond(h, m, s);
        // Assert
        Assert.Equal(secondOfDay, time.SecondOfDay);
    }

    [Theory, MemberData(nameof(RationalData))]
    public static void SecondOfDay64(int h, int m, int s, int secondOfDay, decimal _5)
    {
        var time = TimeOfDay64.FromHourMinuteSecond(h, m, s);
        // Assert
        Assert.Equal(secondOfDay, time.SecondOfDay);
    }

    [Theory, MemberData(nameof(RationalData))]
    public static void ToDecimal(int h, int m, int s, int _4, decimal fractionOfDay)
    {
        var time = TimeOfDay.FromHourMinuteSecond(h, m, s);
        // Assert
        Assert.Equal(fractionOfDay, time.ToDecimal());
    }

    [Theory, MemberData(nameof(RationalData))]
    public static void ToDecimal64(int h, int m, int s, int _4, decimal fractionOfDay)
    {
        var time = TimeOfDay64.FromHourMinuteSecond(h, m, s);
        // Assert
        Assert.Equal(fractionOfDay, time.ToDecimal());
    }
}

// IEquatable.
public partial class TimeOfDayTests
{
    [Theory, MemberData(nameof(RationalData))]
    public static void Equality(int h, int m, int s, int _4, decimal _5)
    {
        var time = TimeOfDay.FromHourMinuteSecond(h, m, s);
        var same = TimeOfDay.FromHourMinuteSecond(h, m, s);
        var notSame = TimeOfDay.FromHourMinuteSecond(h, m, s == 1 ? 2 : 1);

        // Assert.
        Assert.True(time == same);
        Assert.False(time != same);
        Assert.True(time.Equals(same));
        Assert.True(time.Equals((object)same));

        Assert.False(time == notSame);
        Assert.True(time != notSame);
        Assert.False(time.Equals(notSame));
        Assert.False(time.Equals((object)notSame));

        Assert.False(time.Equals(new object()));
    }

    [Theory, MemberData(nameof(RationalData))]
    public static void GetHashCode_Repeated(int h, int m, int s, int _4, decimal _5)
    {
        var hms = TimeOfDay.FromHourMinuteSecond(h, m, s);
        // Act & Assert
        Assert.Equal(hms.GetHashCode(), hms.GetHashCode());
    }
}
