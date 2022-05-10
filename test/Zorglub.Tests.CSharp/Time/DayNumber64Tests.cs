// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time;

using System.Globalization;

using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology;

public sealed partial class DayNumber64Tests
{
    public static readonly TheoryData<long> DaysSinceZero = new()
    {
        Int64.MinValue + 1,
        Int32.MinValue,
        Int32.MinValue + 1,
        -1_000_000,
        -100_000,
        -10_000,
        -1000,
        -100,
        -400_000,
        -40_000,
        -4000,
        -400,
        -987_654,
        -98_765,
        -9876,
        -543,
        -321,
        -30,
        -29,
        -28,
        -27,
        -26,
        -25,
        -24,
        -23,
        -22,
        -21,
        -20,
        -19,
        -18,
        -17,
        -16,
        -15,
        -14,
        -13,
        -12,
        -11,
        -10,
        -9,
        -8,
        -7,
        -6,
        -5,
        -4,
        -3,
        -2,
        -1,
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8,
        9,
        10,
        11,
        12,
        13,
        14,
        15,
        16,
        17,
        18,
        19,
        20,
        21,
        22,
        23,
        24,
        25,
        26,
        27,
        28,
        29,
        30,
        123,
        345,
        6789,
        56_789,
        456_789,
        400,
        4000,
        40_000,
        400_000,
        100,
        1000,
        10_000,
        100_000,
        1_000_000,
        Int32.MaxValue - 1,
        Int32.MaxValue,
        Int64.MaxValue - 1,
    };

    // left ord, right ord, left > right? left = right?
    public static readonly TheoryData<int, int, bool, bool> MinMaxDayNumbers = new()
    {
        { 3, 4, false, false },
        { 3, 3, false, true },
        { 3, 2, true, false },
    };
}

// "Construction".
public partial class DayNumber64Tests
{
    [Theory, MemberData(nameof(DaysSinceZero))]
    public static void AddToZero(long daysSinceZero)
    {
        // Act
        var dayNumber = DayNumber64.Zero + daysSinceZero;
        // Assert
        Assert.Equal(daysSinceZero, dayNumber.DaysSinceZero);
    }

    [Theory, MemberData(nameof(DaysSinceZero))]
    public static void ToString_CurrentCulture(long daysSinceZero)
    {
        // Arrange
        var dayNumber = DayNumber64.Zero + daysSinceZero;
        string str = daysSinceZero.ToString(CultureInfo.CurrentCulture);
        // Act
        string actual = dayNumber.ToString();
        // Assert
        Assert.Equal(str, actual);
    }
}

// Properties.
public partial class DayNumber64Tests
{
    [Fact]
    public static void Zero()
    {
        // Act
        DayNumber64.Zero.GetGregorianParts(out long y, out int m, out int d);
        // Assert
        Assert.Equal(0, DayNumber64.Zero.DaysSinceZero);
        Assert.Equal(Ord64.First, DayNumber64.Zero.Ordinal);
        Assert.Equal(DayOfWeek.Monday, DayNumber64.Zero.DayOfWeek);
        Assert.Equal(1, y);
        Assert.Equal(1, m);
        Assert.Equal(1, d);
    }

    [Fact]
    public static void DayBeforeZero()
    {
        // Act
        DayNumber64 dayBeforeZero = DayNumber64.Zero - 1;
        dayBeforeZero.GetGregorianParts(out long y, out int m, out int d);
        // Assert
        Assert.Equal(-1, dayBeforeZero.DaysSinceZero);
        Assert.Equal(Ord64.Zeroth, dayBeforeZero.Ordinal);
        Assert.Equal(DayOfWeek.Sunday, dayBeforeZero.DayOfWeek);
        Assert.Equal(0, y);
        Assert.Equal(12, m);
        Assert.Equal(31, d);
    }

    [Fact]
    public static void MinValue()
    {
        Assert.Equal(Int64.MinValue + 1, DayNumber64.MinValue.DaysSinceZero);
        // Along the way, we also check that the foll. props do not overflow.
        Assert.Equal(Ord64.MinValue, DayNumber64.MinValue.Ordinal);
        Assert.Equal(DayOfWeek.Monday, DayNumber64.MinValue.DayOfWeek);
    }

    [Fact]
    public static void MaxValue()
    {
        Assert.Equal(Int64.MaxValue - 1, DayNumber64.MaxValue.DaysSinceZero);
        // Along the way, we also check that the foll. props do not overflow.
        Assert.Equal(Ord64.MaxValue, DayNumber64.MaxValue.Ordinal);
        Assert.Equal(DayOfWeek.Sunday, DayNumber64.MaxValue.DayOfWeek);
    }

    [Fact]
    public static void Today()
    {
        // Arrange
        var today = CivilDate.Today();
        // Assert
        Assert.Equal(today.DaysSinceEpoch, DayNumber64.Today().DaysSinceZero);
    }

    [Theory, MemberData(nameof(CalCalDataSet.DayOfWeekData64), MemberType = typeof(CalCalDataSet))]
    public static void DayOfWeek_Prop(DayNumber64 dayNumber, DayOfWeek dayOfWeek) =>
        Assert.Equal(dayOfWeek, dayNumber.DayOfWeek);

    [Fact]
    public static void MinGregorianValue()
    {
        // Act
        var dayNumber = DayNumber64.FromGregorianParts(DayNumber64.MinSupportedYear, 1, 1);
        var dayNumber1 = DayNumber64.FromGregorianOrdinalParts(DayNumber64.MinSupportedYear, 1);
        dayNumber.GetGregorianParts(out long y, out int m, out int d);
        dayNumber.GetGregorianOrdinalParts(out _, out int doy);

        // Assert
        Assert.Equal(DayNumber64.GregorianDomain.Min, dayNumber);
        Assert.Equal(DayNumber64.GregorianDomain.Min, dayNumber1);

        Assert.Equal(DayNumber64.MinSupportedYear, y);
        Assert.Equal(1, m);
        Assert.Equal(1, d);
        Assert.Equal(1, doy);
    }

    [Fact]
    public static void MaxGregorianValue()
    {
        const int DaysInYear = 366; // Leap year.

        // Act
        var dayNumber = DayNumber64.FromGregorianParts(DayNumber64.MaxSupportedYear, 12, 31);
        var dayNumber1 = DayNumber64.FromGregorianOrdinalParts(DayNumber64.MaxSupportedYear, DaysInYear);
        dayNumber.GetGregorianParts(out long y, out int m, out int d);
        dayNumber.GetGregorianOrdinalParts(out _, out int doy);

        // Assert
        Assert.True(GregorianFormulae.IsLeapYear(DayNumber64.MaxSupportedYear));

        Assert.Equal(DayNumber64.GregorianDomain.Max, dayNumber);
        Assert.Equal(DayNumber64.GregorianDomain.Max, dayNumber1);

        Assert.Equal(DayNumber64.MaxSupportedYear, y);
        Assert.Equal(12, m);
        Assert.Equal(31, d);
        Assert.Equal(DaysInYear, doy);
    }
}

// Math ops.
public partial class DayNumber64Tests
{
    [Fact]
    public static void Plus_IntegerUnderflow()
    {
        // Arrange
        var pos = DayNumber64.Zero;
        var neg = DayNumber64.Zero - 1;

        // Act & Assert
        Assert.Overflows(() => pos + Int64.MaxValue);
        Assert.Overflows(() => pos.PlusDays(Int64.MaxValue));

        Assert.Overflows(() => neg + Int64.MinValue);
        Assert.Overflows(() => neg.PlusDays(Int64.MinValue));
    }

    [Fact]
    public static void Plus_AtMinValue()
    {
        // Arrange
        var min = DayNumber64.MinValue;

        // Act & Assert
        Assert.Overflows(() => min - 1);
        Assert.Overflows(() => min + (-1));
        Assert.Overflows(() => min.PlusDays(-1));

        Assert.Equal(min, min - 0);
        Assert.Equal(min, min + 0);
        Assert.Equal(min, min.PlusDays(0));

        Assert.Equal(DayNumber64.Zero - 1, min + (Int64.MaxValue - 1));
        Assert.Equal(DayNumber64.Zero - 1, min.PlusDays(Int64.MaxValue - 1));
    }

    [Fact]
    public static void Plus_AtMaxValue()
    {
        // Arrange
        var max = DayNumber64.MaxValue;

        // Act & Assert
        Assert.Equal(DayNumber64.Zero - 1, max - Int64.MaxValue);
        Assert.Equal(DayNumber64.Zero - 1, max.PlusDays(-Int64.MaxValue));

        Assert.Equal(max, max - 0);
        Assert.Equal(max, max + 0);
        Assert.Equal(max, max.PlusDays(0));

        Assert.Overflows(() => max + 1);
        Assert.Overflows(() => max - (-1));
        Assert.Overflows(() => max.PlusDays(1));
    }

    [Fact]
    public static void Minus_Overflows() =>
        Assert.Overflows(() => DayNumber64.MaxValue - DayNumber64.MinValue);

    [Fact]
    public static void Next_Overflows_AtMaxValue()
    {
        Assert.Overflows(() => DayNumber64.MaxValue.NextDay());

        var copy = DayNumber64.MaxValue;
        Assert.Overflows(() => copy++);
    }

    [Fact]
    public static void Previous_Overflows_AtMinValue()
    {
        Assert.Overflows(() => DayNumber64.MinValue.PreviousDay());

        var copy = DayNumber64.MinValue;
        Assert.Overflows(() => copy--);
    }

    [Fact]
    public static void Plus()
    {
        // Arrange
        var dayNumber = DayNumber64.Zero + 345;
        var exp = DayNumber64.Zero + 351;
        // Act & Assert
        Assert.Equal(exp, dayNumber + 6);
        Assert.Equal(exp, dayNumber.PlusDays(6));
    }

    [Fact]
    public static void Subtract()
    {
        // Arrange
        var left = DayNumber64.Zero - 3;
        var right = DayNumber64.Zero + 15;
        // Act & Assert
        Assert.Equal(-18, left - right);
        Assert.Equal(-18, left.CountDaysSince(right));
    }

    [Fact]
    public static void Minus_Days()
    {
        // Arrange
        var dayNumber = DayNumber64.Zero + 4;
        var exp = DayNumber64.Zero - 3;
        // Act & Assert
        Assert.Equal(exp, dayNumber - 7);
        Assert.Equal(exp, dayNumber.PlusDays(-7));
    }

    [Fact]
    public static void Next()
    {
        // Arrange
        var dayNumber = DayNumber64.Zero + 345;
        var dayNumberAfter = DayNumber64.Zero + 346;

        // Act & Assert
        Assert.Equal(dayNumberAfter, dayNumber.NextDay());

        var copy = dayNumber;
        copy++;
        Assert.Equal(dayNumberAfter, copy);
    }

    [Fact]
    public static void Previous()
    {
        // Arrange
        var dayNumber = DayNumber64.Zero + 345;
        var dayNumberBefore = DayNumber64.Zero + 344;

        // Act & Assert
        Assert.Equal(dayNumberBefore, dayNumber.PreviousDay());

        var copy = dayNumber;
        copy--;
        Assert.Equal(dayNumberBefore, copy);
    }
}

// IEquatable<>.
public partial class DayNumber64Tests
{
    [Theory, MemberData(nameof(DaysSinceZero))]
    public static void Equality(long daysSinceZero)
    {
        // Arrange
        var dayNumber = DayNumber64.Zero + daysSinceZero;
        var same = DayNumber64.Zero + daysSinceZero;
        var notSame = dayNumber == DayNumber64.MaxValue ? dayNumber - 1 : dayNumber + 1;

        // Act & Assert
        Assert.True(dayNumber == same);
        Assert.True(same == dayNumber);
        Assert.False(dayNumber == notSame);
        Assert.False(notSame == dayNumber);

        Assert.False(dayNumber != same);
        Assert.False(same != dayNumber);
        Assert.True(dayNumber != notSame);
        Assert.True(notSame != dayNumber);

        Assert.True(dayNumber.Equals(dayNumber));
        Assert.True(dayNumber.Equals(same));
        Assert.True(same.Equals(dayNumber));
        Assert.False(dayNumber.Equals(notSame));
        Assert.False(notSame.Equals(dayNumber));

        Assert.True(dayNumber.Equals((object)same));
        Assert.False(dayNumber.Equals((object)notSame));

        Assert.False(dayNumber.Equals(new object()));
    }

    [Theory, MemberData(nameof(DaysSinceZero))]
    public static void GetHashCode_SanityChecks(long daysSinceZero)
    {
        // Arrange
        var dayNumber = DayNumber64.Zero + daysSinceZero;
        var same = DayNumber64.Zero + daysSinceZero;
        var notSame = dayNumber == DayNumber64.MaxValue ? dayNumber - 1 : dayNumber + 2;
        // Act & Assert
        Assert.Equal(dayNumber.GetHashCode(), dayNumber.GetHashCode());
        Assert.Equal(dayNumber.GetHashCode(), same.GetHashCode());
        // C'est un peu un hasard si ça marche. Changer +2 en +1 lors de la
        // construction, et ça plantera pour daysSinceZero = -1L car -1L et
        // 0L ont le même hash.
        Assert.NotEqual(dayNumber.GetHashCode(), notSame.GetHashCode());
    }
}

// IComparable<>.
public partial class DayNumber64Tests
{
    [Fact]
    public static void CompareTo_Null()
    {
        // Arrange
        var dayNumber = DayNumber64.Zero + 123456789;
        var comparable = (IComparable)dayNumber;
        // Act & Assert
        Assert.Equal(1, comparable.CompareTo(null));
    }

    [Fact]
    public static void CompareTo_InvalidObject()
    {
        // Arrange
        var dayNumber = DayNumber64.Zero + 123456789;
        var comparable = (IComparable)dayNumber;
        object other = new();
        // Act & Assert
        Assert.Throws<ArgumentException>("obj", () => comparable.CompareTo(other));
    }

    [Theory, MemberData(nameof(MinMaxDayNumbers))]
    public static void CompareTo(
        int xleft, int xright, bool leftIsMax, bool areEqual)
    {
        // Arrange
        var left = DayNumber64.Zero + xleft;
        var right = DayNumber64.Zero + xright;
        var comparable = (IComparable)left;

        // Act & Assert
        if (areEqual)
        {
            Assert.Equal(0, left.CompareTo(right));
            Assert.Equal(0, right.CompareTo(left));
            Assert.Equal(0, comparable.CompareTo(right));
        }
        else if (leftIsMax)
        {
            Assert.True(left.CompareTo(right) > 0);
            Assert.True(right.CompareTo(left) < 0);
            Assert.True(comparable.CompareTo(right) > 0);
        }
        else
        {
            Assert.True(left.CompareTo(right) < 0);
            Assert.True(right.CompareTo(left) > 0);
            Assert.True(comparable.CompareTo(right) < 0);
        }
    }

    [Theory, MemberData(nameof(MinMaxDayNumbers))]
    public static void ComparisonOperators(
        int xleft, int xright, bool leftIsMax, bool areEqual)
    {
        // Arrange
        var left = DayNumber64.Zero + xleft;
        var right = DayNumber64.Zero + xright;

        // Act & Assert
        if (areEqual)
        {
            Assert.False(left > right);
            Assert.False(left < right);
            Assert.True(left >= right);
            Assert.True(left <= right);

            Assert.False(right > left);
            Assert.False(right < left);
            Assert.True(right >= left);
            Assert.True(right <= left);
        }
        else if (leftIsMax)
        {
            Assert.True(left > right);
            Assert.False(left < right);
            Assert.True(left >= right);
            Assert.False(left <= right);

            Assert.True(right < left);
            Assert.False(right > left);
            Assert.True(right <= left);
            Assert.False(right >= left);
        }
        else
        {
            Assert.False(left > right);
            Assert.True(left < right);
            Assert.False(left >= right);
            Assert.True(left <= right);

            Assert.False(right < left);
            Assert.True(right > left);
            Assert.False(right <= left);
            Assert.True(right >= left);
        }
    }

    [Theory, MemberData(nameof(MinMaxDayNumbers))]
    public static void Min(int xleft, int xright, bool leftIsMax, bool _4)
    {
        // Arrange
        var left = DayNumber64.Zero + xleft;
        var right = DayNumber64.Zero + xright;
        var min = leftIsMax ? right : left;
        // Act
        var actual = DayNumber64.Min(left, right);
        // Assert
        Assert.Equal(min, actual);
    }

    [Theory, MemberData(nameof(MinMaxDayNumbers))]
    public static void Max(int xleft, int xright, bool leftIsMax, bool _4)
    {
        // Arrange
        var left = DayNumber64.Zero + xleft;
        var right = DayNumber64.Zero + xright;
        var max = leftIsMax ? left : right;
        // Act
        var actual = DayNumber64.Max(left, right);
        // Assert
        Assert.Equal(max, actual);
    }
}
