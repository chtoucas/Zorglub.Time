// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

using static Zorglub.Testing.Data.Extensions.TheoryDataExtensions;

public static partial class GregorianCutOffMathDataSet { }

public partial class GregorianCutOffMathDataSet // Years
{
    // Intercalary day, expected result in a common year, years to be added.
    public static TheoryData<Yemoda, Yemoda, int> AddYearsData => s_AddYears.ToTheoryData();

    // Intercalary day, expected result in a common year, years to be added.
    public static TheoryData<Yemoda, Yemoda, int> AddYearsLongData => s_AddYearsLong.ToTheoryData();

    // Intercalary day, expected result in a common year, years to be added.
    private static readonly List<(int year, int exp, int, int, int years)> s_AddYears = new()
    {
        (year: 4, exp: 11, 2, 28, years: 7),
        (year: 4, exp: 10, 2, 28, years: 6),
        (year: 4, exp: 9, 2, 28, years: 5),
        (year: 4, exp: 7, 2, 28, years: 3),
        (year: 4, exp: 6, 2, 28, years: 2),
        (year: 4, exp: 5, 2, 28, years: 1),

        (year: 8, exp: 7, 2, 28, years: -1),
        (year: 8, exp: 6, 2, 28, years: -2),
        (year: 8, exp: 5, 2, 28, years: -3),
        (year: 8, exp: 3, 2, 28, years: -5),
        (year: 8, exp: 2, 2, 28, years: -6),
        (year: 8, exp: 1, 2, 28, years: -7),
    };

    // Intercalary day, expected result in a common year, years to be added.
    private static readonly List<(int year, int exp, int, int, int years)> s_AddYearsLong = new()
    {
        (year: 4, exp: 11, 3, 1, years: 7),
        (year: 4, exp: 10, 3, 1, years: 6),
        (year: 4, exp: 9, 3, 1, years: 5),
        (year: 4, exp: 7, 3, 1, years: 3),
        (year: 4, exp: 6, 3, 1, years: 2),
        (year: 4, exp: 5, 3, 1, years: 1),

        (year: 8, exp: 7, 3, 1, years: -1),
        (year: 8, exp: 6, 3, 1, years: -2),
        (year: 8, exp: 5, 3, 1, years: -3),
        (year: 8, exp: 3, 3, 1, years: -5),
        (year: 8, exp: 2, 3, 1, years: -6),
        (year: 8, exp: 1, 3, 1, years: -7),
    };
}

public partial class GregorianCutOffMathDataSet // Months
{
    // Date, expected result, months to be added.
    public static TheoryData<Yemoda, Yemoda, int> AddMonthsData => s_AddMonths.ToTheoryData();

    // Date, expected result, months to be added.
    private static readonly List<(int date, int, int, int exp, int, int, int months)> s_AddMonths = new()
    {
        // 31 days long month -> 30 days long month.
        (date: 1, 3, 31, exp: 1, 6, 30, months: 3),
        (date: 1, 5, 31, exp: 1, 4, 30, months: -1),

        // Target February (common year).
        (date: 1, 1, 29, exp: 1, 2, 28, months: 1),
        (date: 1, 1, 30, exp: 1, 2, 28, months: 1),
        (date: 1, 1, 31, exp: 1, 2, 28, months: 1),
        // Backward.
        (date: 1, 3, 29, exp: 1, 2, 28, months: -1),
        (date: 1, 3, 30, exp: 1, 2, 28, months: -1),
        (date: 1, 3, 31, exp: 1, 2, 28, months: -1),

        // Target February (leap year).
        (date: 4, 1, 30, exp: 4, 2, 29, months: 1),
        (date: 4, 1, 31, exp: 4, 2, 29, months: 1),
        // Backward.
        (date: 4, 3, 30, exp: 4, 2, 29, months: -1),
        (date: 4, 3, 31, exp: 4, 2, 29, months: -1),

        // Change of year.
        // Target shorter month.
        (date: 3, 1, 31, exp: 4, 4, 30, months: 15),
        (date: 4, 1, 31, exp: 3, 11, 30, months: -2),
        // Target end of February.
        (date: 3, 1, 31, exp: 4, 2, 29, months: 13),
        (date: 3, 4, 30, exp: 4, 2, 29, months: 10),
        (date: 4, 1, 31, exp: 3, 2, 28, months: -11),
        (date: 4, 4, 30, exp: 3, 2, 28, months: -14),
    };
}

public partial class GregorianCutOffMathDataSet // Diff
{
    // Start date, end date, exact diff between.
    public static TheoryData<Yemoda, Yemoda, int, int, int> DiffData => s_Diff.ToTheoryData();

    // Start date, end date, exact diff between.
    private static readonly List<(int start, int, int, int end, int, int, int diff, int, int)> s_Diff = new()
    {
        (start: 3, 4, 30, end: 4, 2, 29, diff: 0, 9, 30),
        (start: 3, 4, 30, end: 2, 3, 31, diff: -1, 0, -30),
        (start: 3, 4, 30, end: 3, 3, 31, diff: 0, 0, -30),

        (start: 8, 2, 29, end: 11, 2, 28, diff: 2, 11, 30),
        (start: 8, 2, 29, end: 9, 2, 28, diff: 0, 11, 30),
    };
}