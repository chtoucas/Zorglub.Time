// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core;

using System.Security.Cryptography;

using static TemporalConstants;

public static partial class TemporalArithmeticTests
{
    // MultiplierData stop at 10.
    private const int RandomFrom = 11;

    public static readonly TheoryData<int> MultiplierData = new()
    { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

    public static readonly TheoryData<int> ZeroToTwentyThreeData = new()
    { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
}

public partial class TemporalArithmeticTests // TicksPerDay
{
    [Theory, MemberData(nameof(MultiplierData))]
    public static void DivideByTicksPerDay(int mul) =>
        Assert.Equal(mul, TemporalArithmetic.DivideByTicksPerDay(mul * TicksPerDay));

    [Fact]
    public static void DivideByTicksPerDay_Fuzzy()
    {
        // Arrange
        int mul = GetFuzzyMulForTicksPerDay();
        // Act & Assert
        Assert.Equal(mul, TemporalArithmetic.DivideByTicksPerDay(mul * TicksPerDay));
    }

    [Theory, MemberData(nameof(MultiplierData))]
    public static void MultiplyByTicksPerDay(int mul) =>
        Assert.Equal(mul * TicksPerDay, TemporalArithmetic.MultiplyByTicksPerDay(mul));

    [Fact]
    public static void MultiplyByTicksPerDay_Fuzzy()
    {
        // Arrange
        int mul = GetFuzzyMulForTicksPerDay();
        // Act & Assert
        Assert.Equal(mul * TicksPerDay, TemporalArithmetic.MultiplyByTicksPerDay(mul));
    }

    private static int GetFuzzyMulForTicksPerDay()
    {
        const long MaxMul = Int64.MaxValue / TicksPerDay;

        int max = (int)System.Math.Min(Int32.MaxValue, MaxMul);
        return RandomNumberGenerator.GetInt32(RandomFrom, max);
    }
}

public partial class TemporalArithmeticTests // NanosecondsPerHour
{
    [Theory, MemberData(nameof(ZeroToTwentyThreeData))]
    public static void DivideByNanosecondsPerHour(int mul) =>
        Assert.Equal(mul, TemporalArithmetic.DivideByNanosecondsPerHour(mul * NanosecondsPerHour));

    [Theory, MemberData(nameof(ZeroToTwentyThreeData))]
    public static void MultiplyByNanosecondsPerHour(int mul) =>
        Assert.Equal(mul * NanosecondsPerHour, TemporalArithmetic.MultiplyByNanosecondsPerHour(mul));
}

public partial class TemporalArithmeticTests // NanosecondsPerMinute
{
    [Theory, MemberData(nameof(MultiplierData))]
    public static void DivideByNanosecondsPerMinute(int mul) =>
        Assert.Equal(mul, TemporalArithmetic.DivideByNanosecondsPerMinute(mul * NanosecondsPerMinute));

    [Fact]
    public static void DivideByNanosecondsPerMinute_Fuzzy()
    {
        // Arrange
        // In Debug mode, we check that the input is < NanosecondsPerDay,
        // which means that "mul" must be < MinutesPerDay.
        int mul = RandomNumberGenerator.GetInt32(RandomFrom, MinutesPerDay);
        // Act & Assert
        Assert.Equal(mul, TemporalArithmetic.DivideByNanosecondsPerMinute(mul * NanosecondsPerMinute));
    }

    [Theory, MemberData(nameof(MultiplierData))]
    public static void MultiplyByNanosecondsPerMinute(int mul) =>
        Assert.Equal(mul * NanosecondsPerMinute, TemporalArithmetic.MultiplyByNanosecondsPerMinute(mul));

    [Fact]
    public static void MultiplyByNanosecondsPerMinute_Fuzzy()
    {
        // Arrange
        int mul = RandomNumberGenerator.GetInt32(RandomFrom, MinutesPerDay);
        // Act & Assert
        Assert.Equal(mul * NanosecondsPerMinute, TemporalArithmetic.MultiplyByNanosecondsPerMinute(mul));
    }
}
