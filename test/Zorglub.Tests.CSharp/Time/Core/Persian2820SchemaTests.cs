// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core;

using Zorglub.Time.Core.Schemas;

public static class Persian2820SchemaTests
{
    private static readonly Persian2820Schema s_Schema = new();

    [Fact]
    public static void IsLeapYear_ZerothCycle() =>
        TestIsLeapYear_WholeCycle(Persian2820Schema.Year0 - 2 * 2820);

    [Fact]
    public static void IsLeapYear_FirstCycle() =>
        // Normalement on n'accepte pas les années négatives, mais on fait
        // une exception pour pouvoir tester les premières années.
        TestIsLeapYear_WholeCycle(Persian2820Schema.Year0 - 2820);

    [Fact]
    public static void IsLeapYear_FirstWholeCycle() =>
        TestIsLeapYear_WholeCycle(Persian2820Schema.Year0);

    [Fact]
    public static void IsLeapYear_SecondWholeCycle() =>
        TestIsLeapYear_WholeCycle(Persian2820Schema.Year0 + 2820);

    // NB: y = last year before the start of the cycle.
    private static void TestIsLeapYear_WholeCycle(int y)
    {
        // Twenty-one 128-year cycles.
        for (int i = 0; i < 21; i++)
        {
            testCycle(ref y, 29);
            testCycle(ref y, 33);
            testCycle(ref y, 33);
            testCycle(ref y, 33);
        }
        // One 132-year cycle.
        testCycle(ref y, 29);
        testCycle(ref y, 33);
        testCycle(ref y, 33);
        testCycle(ref y, 37);

        static void testCycle(ref int y, int length)
        {
            for (int Y = 1; Y <= length; Y++)
            {
                y++;
                // NB: en toute rigueur on ne devrait pas tester les années
                // y < 1, mais ça marche quand même ici car on ne remonte pas
                // trop dans le temps.
                bool isLeap = Y != 1 && Y % 4 == 1;
                Assert.Equal(isLeap, s_Schema.IsLeapYear(y));
            }
        }
    }
}
