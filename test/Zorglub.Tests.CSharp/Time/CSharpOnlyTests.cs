// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time;
public static partial class CSharpOnlyTests { }

public partial class CSharpOnlyTests // DayNumber
{
    //[Fact]
    //public static void DayNumber_FromDayNumber()
    //{
    //    Test<DayNumber>(DayNumber.MinValue);
    //    Test<DayNumber>(DayNumber.Zero);
    //    Test<DayNumber>(DayNumber.MaxValue);

    //    static void Test<T>(DayNumber x) where T : IFixedDate<DayNumber> =>
    //        Assert.Equal(x, T.FromDayNumber(x));
    //}

    //
    // Math operators
    //

    [Fact]
    public static void DayNumber_Op_Increment_Overflows_AtMaxValue()
    {
        var copy = DayNumber.MaxValue;
        Assert.Overflows(() => copy++);
        Assert.Overflows(() => ++copy);
    }

    [Fact]
    public static void DayNumber_Op_Increment()
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
    public static void DayNumber_Op_Decrement_Overflows_AtMinValue()
    {
        var copy = DayNumber.MinValue;
        Assert.Overflows(() => copy--);
        Assert.Overflows(() => --copy);
    }

    [Fact]
    public static void DayNumber_Op_Decrement()
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

public partial class CSharpOnlyTests // Ord
{
    //
    // Math operators
    //

    [Fact]
    public static void Ord_Op_Increment_Overflows_AtMaxValue()
    {
        var max = Ord.MaxValue;
        Assert.Overflows(() => max++);
        Assert.Overflows(() => ++max);
    }

    [Fact]
    public static void Ord_Op_Increment()
    {
        var ord = Ord.FromRank(345);
        var ordAfter = Ord.FromRank(346);
        // Act & Assert
        var copy = ord;
        copy++;
        Assert.Equal(ordAfter, copy);
        Assert.Equal(ordAfter, ++ord);
    }

    [Fact]
    public static void Ord_Op_Decrement_Overflows_AtMinValue()
    {
        var min = Ord.MinValue;
        Assert.Overflows(() => min--);
        Assert.Overflows(() => --min);
    }

    [Fact]
    public static void Ord_Op_Decrement()
    {
        var ord = Ord.FromRank(345);
        var ordBefore = Ord.FromRank(344);
        // Act & Assert
        var copy = ord;
        copy--;
        Assert.Equal(ordBefore, copy);
        Assert.Equal(ordBefore, --ord);
    }
}
