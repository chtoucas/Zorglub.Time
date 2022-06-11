// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time;

using Zorglub.Time.Hemerology;

public static partial class DayNumberTests
{
    [Fact]
    public static void FromDayNumber()
    {
        Test<DayNumber>(DayNumber.MinValue);
        Test<DayNumber>(DayNumber.Zero);
        Test<DayNumber>(DayNumber.MaxValue);

        static void Test<T>(DayNumber x) where T : IFixedDay<DayNumber> =>
            Assert.Equal(x, T.FromDayNumber(x));
    }

    [Fact]
    public static void Op_Increment_Overflows_AtMaxValue()
    {
        var copy = DayNumber.MaxValue;
        Assert.Overflows(() => copy++);
        Assert.Overflows(() => ++copy);
    }

    [Fact]
    public static void Op_Increment()
    {
        var dayNumber = DayNumber.Zero + 345;
        var dayNumberAfter = DayNumber.Zero + 346;
        // Act & Assert
        var copy = dayNumber;
        copy++;
        Assert.Equal(dayNumberAfter, copy);
        Assert.Equal(dayNumberAfter, ++dayNumber);
    }

    [Fact]
    public static void Op_Decrement_Overflows_AtMinValue()
    {
        var copy = DayNumber.MinValue;
        Assert.Overflows(() => copy--);
        Assert.Overflows(() => --copy);
    }

    [Fact]
    public static void Op_Decrement()
    {
        var dayNumber = DayNumber.Zero + 345;
        var dayNumberBefore = DayNumber.Zero + 344;
        // Act & Assert
        var copy = dayNumber;
        copy--;
        Assert.Equal(dayNumberBefore, copy);
        Assert.Equal(dayNumberBefore, --dayNumber);
    }
}

