﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

// TODO(data): use DataGroup. Filter matching data in Bounded.

using static Zorglub.Testing.Data.Extensions.TheoryDataExtensions;

public partial class GregorianDataSet // IMathDataSet (days)
{
    public DataGroup<YemodaPairAnd<int>> AddDaysData { get; } = new()
    {
        // A full month.
        new(new(3, 4, 5), new(3, 4, 30), 25),
        new(new(3, 4, 5), new(3, 4, 29), 24),
        new(new(3, 4, 5), new(3, 4, 28), 23),
        new(new(3, 4, 5), new(3, 4, 27), 22),
        new(new(3, 4, 5), new(3, 4, 26), 21),
        new(new(3, 4, 5), new(3, 4, 25), 20),
        new(new(3, 4, 5), new(3, 4, 24), 19),
        new(new(3, 4, 5), new(3, 4, 23), 18),
        new(new(3, 4, 5), new(3, 4, 22), 17),
        new(new(3, 4, 5), new(3, 4, 21), 16),
        new(new(3, 4, 5), new(3, 4, 20), 15),
        new(new(3, 4, 5), new(3, 4, 19), 14),
        new(new(3, 4, 5), new(3, 4, 18), 13),
        new(new(3, 4, 5), new(3, 4, 17), 12),
        new(new(3, 4, 5), new(3, 4, 16), 11),
        new(new(3, 4, 5), new(3, 4, 15), 10),
        new(new(3, 4, 5), new(3, 4, 14), 9),
        new(new(3, 4, 5), new(3, 4, 13), 8),
        new(new(3, 4, 5), new(3, 4, 12), 7),
        new(new(3, 4, 5), new(3, 4, 11), 6),
        new(new(3, 4, 5), new(3, 4, 10), 5),
        new(new(3, 4, 5), new(3, 4, 9), 4),
        new(new(3, 4, 5), new(3, 4, 8), 3),
        new(new(3, 4, 5), new(3, 4, 7), 2),
        new(new(3, 4, 5), new(3, 4, 6), 1),
        new(new(3, 4, 5), new(3, 4, 5), 0),
        new(new(3, 4, 5), new(3, 4, 4), -1),
        new(new(3, 4, 5), new(3, 4, 3), -2),
        new(new(3, 4, 5), new(3, 4, 2), -3),
        new(new(3, 4, 5), new(3, 4, 1), -4),

        // Change of year.
        new(new(3, 11, 30), new(4, 1, 1), 32),
        new(new(3, 2, 1), new(2, 12, 31), -32),

        // Change of month.
        new(new(3, 4, 5), new(3, 5, 1), 26),
        new(new(3, 4, 5), new(3, 3, 31), -5),
        // February, common year.
        new(new(3, 2, 28), new(3, 3, 1), 1),
        new(new(3, 3, 1), new(3, 2, 28), -1),
        // February, leap year.
        new(new(4, 2, 28), new(4, 2, 29), 1),
        new(new(4, 2, 28), new(4, 3, 1), 2),
        new(new(4, 2, 29), new(4, 3, 1), 1),
        new(new(4, 3, 1), new(4, 2, 29), -1),

        // years < -365, no leap year in the middle.
        new(new(3, 4, 5), new(2, 4, 5), -365),
        new(new(3, 4, 5), new(1, 4, 5), -2 * 365),

        // years > 365, one leap year in the middle.
        new(new(3, 4, 5), new(4, 4, 4), 365),
        new(new(3, 4, 5), new(5, 4, 4), 2 * 365),
    };

    // Also useful for testing Next() and Previous().
    public DataGroup<YemodaPair> ConsecutiveDaysData { get; } = new()
    {
        new(new(CommonYear, 4, 1), new(CommonYear, 4, 2)),
        new(new(CommonYear, 4, 2), new(CommonYear, 4, 3)),
        new(new(CommonYear, 4, 3), new(CommonYear, 4, 4)),
        new(new(CommonYear, 4, 4), new(CommonYear, 4, 5)),
        new(new(CommonYear, 4, 5), new(CommonYear, 4, 6)),
        new(new(CommonYear, 4, 6), new(CommonYear, 4, 7)),
        new(new(CommonYear, 4, 7), new(CommonYear, 4, 8)),
        new(new(CommonYear, 4, 8), new(CommonYear, 4, 9)),
        new(new(CommonYear, 4, 9), new(CommonYear, 4, 10)),
        new(new(CommonYear, 4, 10), new(CommonYear, 4, 11)),
        new(new(CommonYear, 4, 11), new(CommonYear, 4, 12)),
        new(new(CommonYear, 4, 12), new(CommonYear, 4, 13)),
        new(new(CommonYear, 4, 13), new(CommonYear, 4, 14)),
        new(new(CommonYear, 4, 14), new(CommonYear, 4, 15)),
        new(new(CommonYear, 4, 15), new(CommonYear, 4, 16)),
        new(new(CommonYear, 4, 16), new(CommonYear, 4, 17)),
        new(new(CommonYear, 4, 17), new(CommonYear, 4, 18)),
        new(new(CommonYear, 4, 18), new(CommonYear, 4, 19)),
        new(new(CommonYear, 4, 19), new(CommonYear, 4, 20)),
        new(new(CommonYear, 4, 20), new(CommonYear, 4, 21)),
        new(new(CommonYear, 4, 21), new(CommonYear, 4, 22)),
        new(new(CommonYear, 4, 22), new(CommonYear, 4, 23)),
        new(new(CommonYear, 4, 23), new(CommonYear, 4, 24)),
        new(new(CommonYear, 4, 24), new(CommonYear, 4, 25)),
        new(new(CommonYear, 4, 25), new(CommonYear, 4, 26)),
        new(new(CommonYear, 4, 26), new(CommonYear, 4, 27)),
        new(new(CommonYear, 4, 27), new(CommonYear, 4, 28)),
        new(new(CommonYear, 4, 28), new(CommonYear, 4, 29)),
        new(new(CommonYear, 4, 29), new(CommonYear, 4, 30)),
        new(new(CommonYear, 4, 30), new(CommonYear, 5, 1)),
        new(new(CommonYear, 5, 1), new(CommonYear, 5, 2)),

        // End of february.
        new(new(CommonYear, 2, 28), new(CommonYear, 3, 1)),
        new(new(LeapYear, 2, 28), new(LeapYear, 2, 29)),
        new(new(LeapYear, 2, 29), new(LeapYear, 3, 1)),

        // End of month.
        new(new(CommonYear, 1, 31), new(CommonYear, 2, 1)),
        new(new(CommonYear, 3, 31), new(CommonYear, 4, 1)),
        new(new(CommonYear, 5, 31), new(CommonYear, 6, 1)),
        new(new(CommonYear, 6, 30), new(CommonYear, 7, 1)),
        new(new(CommonYear, 7, 31), new(CommonYear, 8, 1)),
        new(new(CommonYear, 8, 31), new(CommonYear, 9, 1)),
        new(new(CommonYear, 9, 30), new(CommonYear, 10, 1)),
        new(new(CommonYear, 10, 31), new(CommonYear, 11, 1)),
        new(new(CommonYear, 11, 30), new(CommonYear, 12, 1)),
        new(new(3, 12, 31), new(4, 1, 1)),
    };
}

public partial class GregorianDataSet // Addition (months)
{
    // Date, expected result, months to be added.
    public TheoryData<Yemoda, Yemoda, int> AddMonthsData =>
        s_AddMonths.MapToTheoryDataOfTwoYemodas();

    // Date, expected result, months to be added.
    public static TheoryData<Yemoda, Yemoda, int> AddMonthsCutOffData =>
        s_AddMonthsCutOff.MapToTheoryDataOfTwoYemodas();

    // Date, expected result, months to be added.
    private static readonly List<(int date, int, int, int exp, int, int, int months)> s_AddMonths = new()
    {
        // Simple cases.
        (date: 3, 4, 5, exp: 5, 1, 5, months: 21),
        (date: 3, 4, 5, exp: 4, 12, 5, months: 20),
        (date: 3, 4, 5, exp: 4, 11, 5, months: 19),
        (date: 3, 4, 5, exp: 4, 10, 5, months: 18),
        (date: 3, 4, 5, exp: 4, 9, 5, months: 17),
        (date: 3, 4, 5, exp: 4, 8, 5, months: 16),
        (date: 3, 4, 5, exp: 4, 7, 5, months: 15),
        (date: 3, 4, 5, exp: 4, 6, 5, months: 14),
        (date: 3, 4, 5, exp: 4, 5, 5, months: 13),
        (date: 3, 4, 5, exp: 4, 4, 5, months: 12),
        (date: 3, 4, 5, exp: 4, 3, 5, months: 11),
        (date: 3, 4, 5, exp: 4, 2, 5, months: 10),
        (date: 3, 4, 5, exp: 4, 1, 5, months: 9),
        (date: 3, 4, 5, exp: 3, 12, 5, months: 8),
        (date: 3, 4, 5, exp: 3, 11, 5, months: 7),
        (date: 3, 4, 5, exp: 3, 10, 5, months: 6),
        (date: 3, 4, 5, exp: 3, 9, 5, months: 5),
        (date: 3, 4, 5, exp: 3, 8, 5, months: 4),
        (date: 3, 4, 5, exp: 3, 7, 5, months: 3),
        (date: 3, 4, 5, exp: 3, 6, 5, months: 2),
        (date: 3, 4, 5, exp: 3, 5, 5, months: 1),
        (date: 3, 4, 5, exp: 3, 4, 5, months: 0),
        (date: 3, 4, 5, exp: 3, 3, 5, months: -1),
        (date: 3, 4, 5, exp: 3, 2, 5, months: -2),
        (date: 3, 4, 5, exp: 3, 1, 5, months: -3),
        (date: 3, 4, 5, exp: 2, 12, 5, months: -4),
        (date: 3, 4, 5, exp: 2, 11, 5, months: -5),
        (date: 3, 4, 5, exp: 2, 10, 5, months: -6),
        (date: 3, 4, 5, exp: 2, 9, 5, months: -7),
        (date: 3, 4, 5, exp: 2, 8, 5, months: -8),
        (date: 3, 4, 5, exp: 2, 7, 5, months: -9),
        (date: 3, 4, 5, exp: 2, 6, 5, months: -10),
        (date: 3, 4, 5, exp: 2, 5, 5, months: -11),
        (date: 3, 4, 5, exp: 2, 4, 5, months: -12),
        (date: 3, 4, 5, exp: 2, 3, 5, months: -13),
        (date: 3, 4, 5, exp: 2, 2, 5, months: -14),
        (date: 3, 4, 5, exp: 2, 1, 5, months: -15),
        (date: 3, 4, 5, exp: 1, 12, 5, months: -16),

        // 30 days long month -> 31 days long month.
        (date: 1, 4, 30, exp: 1, 7, 30, months: 3),
        (date: 1, 4, 30, exp: 1, 1, 30, months: -3),

        // Target February (common year).
        (date: 1, 1, 27, exp: 1, 2, 27, months: 1),
        (date: 1, 1, 28, exp: 1, 2, 28, months: 1),
        // Backward.
        (date: 1, 3, 27, exp: 1, 2, 27, months: -1),
        (date: 1, 3, 28, exp: 1, 2, 28, months: -1),

        // Target February (leap year).
        (date: 4, 1, 27, exp: 4, 2, 27, months: 1),
        (date: 4, 1, 28, exp: 4, 2, 28, months: 1),
        (date: 4, 1, 29, exp: 4, 2, 29, months: 1),
        // Backward.
        (date: 4, 3, 27, exp: 4, 2, 27, months: -1),
        (date: 4, 3, 28, exp: 4, 2, 28, months: -1),
        (date: 4, 3, 29, exp: 4, 2, 29, months: -1),
    };

    // Date, expected result, months to be added.
    private static readonly List<(int date, int, int, int exp, int, int, int months)> s_AddMonthsCutOff = new()
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

public partial class GregorianDataSet // Addition (years)
{
    // Date, expected result, years to be added.
    public TheoryData<Yemoda, Yemoda, int> AddYearsData =>
        s_AddYears.MapToTheoryDataOfTwoYemodas();

    // Intercalary day, expected result in a common year, years to be added.
    public static TheoryData<Yemoda, Yemoda, int> AddYearsCutOffData =>
        ToTheoryData(s_AddYearsCutOff);

    // Intercalary day, expected result in a common year, years to be added.
    public static TheoryData<Yemoda, Yemoda, int> AddYearsLongCutOffData =>
        ToTheoryData(s_AddYearsLongCutOff);

    // Date, expected result, years to be added.
    private static readonly List<(int date, int, int, int exp, int, int, int years)> s_AddYears = new()
    {
        (date: 3, 4, 5, exp: 9, 4, 5, years: 6),
        (date: 3, 4, 5, exp: 8, 4, 5, years: 5),
        (date: 3, 4, 5, exp: 7, 4, 5, years: 4),
        (date: 3, 4, 5, exp: 6, 4, 5, years: 3),
        (date: 3, 4, 5, exp: 5, 4, 5, years: 2),
        (date: 3, 4, 5, exp: 4, 4, 5, years: 1),
        (date: 3, 4, 5, exp: 3, 4, 5, years: 0),
        (date: 3, 4, 5, exp: 2, 4, 5, years: -1),
        (date: 3, 4, 5, exp: 1, 4, 5, years: -2),

        // End of february, common year -> common year.
        (date: 3, 2, 28, exp: 5, 2, 28, years: 2),
        (date: 3, 2, 28, exp: 1, 2, 28, years: -2),

        // End of february, common year -> leap year.
        (date: 3, 2, 28, exp: 4, 2, 28, years: 1),
        (date: 5, 2, 28, exp: 4, 2, 28, years: -1),

        // End of february, leap year -> leap year.
        (date: 4, 2, 29, exp: 8, 2, 29, years: 4),
        (date: 8, 2, 29, exp: 4, 2, 29, years: -4),

        // End of february, leap year -> common year.
        // See samples with cutoff.
    };

    // Intercalary day, expected result in a common year, years to be added.
    private static readonly List<(int year, int exp, int, int, int years)> s_AddYearsCutOff = new()
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

    // TODO(data): s_AddYearsLongCutOff. See GregorianMathTests.
    // Intercalary day, expected result in a common year, years to be added.
    private static readonly List<(int year, int exp, int, int, int years)> s_AddYearsLongCutOff = new()
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

    private static TheoryData<Yemoda, Yemoda, int> ToTheoryData(
        IEnumerable<(int, int, int, int, int)> @this)
    {
        var data = new TheoryData<Yemoda, Yemoda, int>();
        foreach (var item in @this)
        {
            var (y0, y, m, d, years) = item;
            data.Add(
                new Yemoda(y0, 2, 29),
                new Yemoda(y, m, d),
                years);
        }
        return data;
    }
}

public partial class GregorianDataSet // Subtraction
{
    // Start date, end date, exact diff between.
    public TheoryData<Yemoda, Yemoda, int, int, int> DiffData =>
        MapToTheoryDataOfTwoYemodas(s_Diff);

    // Start date, end date, exact diff between.
    public static TheoryData<Yemoda, Yemoda, int, int, int> DiffCutOffData =>
        MapToTheoryDataOfTwoYemodas(s_DiffCutOff);

    [Pure]
    private static TheoryData<Yemoda, Yemoda, T1, T2, T3> MapToTheoryDataOfTwoYemodas<T1, T2, T3>(
        IEnumerable<(int, int, int, int, int, int, T1, T2, T3)> source)
    {
        var data = new TheoryData<Yemoda, Yemoda, T1, T2, T3>();
        foreach (var (y1, m1, d1, y2, m2, d2, t1, t2, t3) in source)
        {
            data.Add(new Yemoda(y1, m1, d1), new Yemoda(y2, m2, d2), t1, t2, t3);
        }
        return data;
    }

    // Start date, end date, exact diff between.
    private static readonly List<(int start, int, int, int end, int, int, int diff, int, int)> s_Diff = new()
    {
        #region After >= 1 year.

        // At least 2 years.
        (start: 3, 4, 5, end: 9, 4, 6, diff: 6, 0, 1),
        (start: 3, 4, 5, end: 9, 4, 5, diff: 6, 0, 0),
        (start: 3, 4, 5, end: 9, 4, 4, diff: 5, 11, 30),

        (start: 3, 4, 5, end: 8, 4, 6, diff: 5, 0, 1),
        (start: 3, 4, 5, end: 8, 4, 5, diff: 5, 0, 0),
        (start: 3, 4, 5, end: 8, 4, 4, diff: 4, 11, 30),

        (start: 3, 4, 5, end: 7, 4, 6, diff: 4, 0, 1),
        (start: 3, 4, 5, end: 7, 4, 5, diff: 4, 0, 0),
        (start: 3, 4, 5, end: 7, 4, 4, diff: 3, 11, 30),

        (start: 3, 4, 5, end: 6, 4, 6, diff: 3, 0, 1),
        (start: 3, 4, 5, end: 6, 4, 5, diff: 3, 0, 0),
        (start: 3, 4, 5, end: 6, 4, 4, diff: 2, 11, 30),

        (start: 3, 4, 5, end: 5, 4, 6, diff: 2, 0, 1),
        (start: 3, 4, 5, end: 5, 4, 5, diff: 2, 0, 0),
        (start: 3, 4, 5, end: 5, 4, 4, diff: 1, 11, 30),

        // 1 year and 2 months.
        (start: 3, 4, 5, end: 4, 6, 29, diff: 1, 2, 24),
        (start: 3, 4, 5, end: 4, 6, 6, diff: 1, 2, 1),
        (start: 3, 4, 5, end: 4, 6, 5, diff: 1, 2, 0),
        // 1 year and 1 month.
        (start: 3, 4, 5, end: 4, 5, 29, diff: 1, 1, 24),
        (start: 3, 4, 5, end: 4, 5, 6, diff: 1, 1, 1),
        (start: 3, 4, 5, end: 4, 5, 5, diff: 1, 1, 0),

        // 1 year.
        (start: 3, 4, 5, end: 4, 4, 30, diff: 1, 0, 25),
        (start: 3, 4, 5, end: 4, 4, 6, diff: 1, 0, 1),
        (start: 3, 4, 5, end: 4, 4, 5, diff: 1, 0, 0),

        #endregion

        #region After < 1 year.

        (start: 3, 4, 5, end: 4, 4, 4, diff: 0, 11, 30),
        (start: 3, 4, 5, end: 4, 4, 1, diff: 0, 11, 27),
        (start: 3, 4, 5, end: 4, 3, 31, diff: 0, 11, 26),
        (start: 3, 4, 5, end: 4, 3, 5, diff: 0, 11, 0),
        (start: 3, 4, 5, end: 4, 2, 5, diff: 0, 10, 0),
        (start: 3, 4, 5, end: 4, 1, 5, diff: 0, 9, 0),

        (start: 3, 4, 5, end: 3, 12, 5, diff: 0, 8, 0),
        (start: 3, 4, 5, end: 3, 11, 5, diff: 0, 7, 0),
        (start: 3, 4, 5, end: 3, 10, 5, diff: 0, 6, 0),
        (start: 3, 4, 5, end: 3, 9, 5, diff: 0, 5, 0),
        (start: 3, 4, 5, end: 3, 8, 5, diff: 0, 4, 0),
        (start: 3, 4, 5, end: 3, 7, 5, diff: 0, 3, 0),

        // 2 months.
        (start: 3, 4, 5, end: 3, 7, 4, diff: 0, 2, 29),
        (start: 3, 4, 5, end: 3, 7, 1, diff: 0, 2, 26),
        (start: 3, 4, 5, end: 3, 6, 30, diff: 0, 2, 25),
        (start: 3, 4, 5, end: 3, 6, 5, diff: 0, 2, 0),
        // 1 month.
        (start: 3, 4, 5, end: 3, 6, 4, diff: 0, 1, 30),
        (start: 3, 4, 5, end: 3, 6, 1, diff: 0, 1, 27),
        (start: 3, 4, 5, end: 3, 5, 31, diff: 0, 1, 26),
        (start: 3, 4, 5, end: 3, 5, 5, diff: 0, 1, 0),
        // < 1 month.
        (start: 3, 4, 5, end: 3, 5, 4, diff: 0, 0, 29),
        (start: 3, 4, 5, end: 3, 5, 1, diff: 0, 0, 26),
        (start: 3, 4, 5, end: 3, 4, 30, diff: 0, 0, 25),
        (start: 3, 4, 5, end: 3, 4, 6, diff: 0, 0, 1),

        #endregion

        // Identical dates.
        (start: 3, 4, 5, end: 3, 4, 5, diff: 0, 0, 0),

        #region Before < 1 year.

        // < 1 month.
        (start: 3, 4, 5, end: 3, 4, 4, diff: 0, 0, -1),
        (start: 3, 4, 5, end: 3, 4, 1, diff: 0, 0, -4),
        (start: 3, 4, 5, end: 3, 3, 31, diff: 0, 0, -5),
        (start: 3, 4, 5, end: 3, 3, 6, diff: 0, 0, -30),
        // 1 month.
        (start: 3, 4, 5, end: 3, 3, 5, diff: 0, -1, 0),
        (start: 3, 4, 5, end: 3, 3, 1, diff: 0, -1, -4),
        (start: 3, 4, 5, end: 3, 2, 28, diff: 0, -1, -5),
        (start: 3, 4, 5, end: 3, 2, 6, diff: 0, -1, -27),
        // 2 months.
        (start: 3, 4, 5, end: 3, 2, 5, diff: 0, -2, 0),
        (start: 3, 4, 5, end: 3, 2, 1, diff: 0, -2, -4),
        (start: 3, 4, 5, end: 3, 1, 31, diff: 0, -2, -5),
        (start: 3, 4, 5, end: 3, 1, 6, diff: 0, -2, -30),
        // 3 months.
        (start: 3, 4, 5, end: 3, 1, 5, diff: 0, -3, 0),
        (start: 3, 4, 5, end: 3, 1, 1, diff: 0, -3, -4),

        (start: 3, 4, 5, end: 2, 4, 6, diff: 0, -11, -29),

        #endregion

        #region Before >= 1 year.

        (start: 3, 4, 5, end: 2, 4, 5, diff: -1, 0, 0),
        (start: 3, 4, 5, end: 2, 4, 1, diff: -1, 0, -4),
        (start: 3, 4, 5, end: 2, 3, 31, diff: -1, 0, -5),
        (start: 3, 4, 5, end: 2, 3, 6, diff: -1, 0, -30),
        // 1 year and 1 month.
        (start: 3, 4, 5, end: 2, 3, 5, diff: -1, -1, 0),
        (start: 3, 4, 5, end: 2, 3, 4, diff: -1, -1, -1),
        (start: 3, 4, 5, end: 2, 3, 1, diff: -1, -1, -4),
        (start: 3, 4, 5, end: 2, 2, 28, diff: -1, -1, -5),
        (start: 3, 4, 5, end: 2, 2, 6, diff: -1, -1, -27),
        // 1 year and 2 months.
        (start: 3, 4, 5, end: 2, 2, 5, diff: -1, -2, 0),
        (start: 3, 4, 5, end: 2, 2, 4, diff: -1, -2, -1),
        (start: 3, 4, 5, end: 2, 2, 1, diff: -1, -2, -4),

        (start: 3, 4, 5, end: 1, 4, 6, diff: -1, -11, -29),

        // At least 2 years.
        (start: 3, 4, 5, end: 1, 4, 5, diff: -2, 0, 0),
        (start: 3, 4, 5, end: 1, 4, 4, diff: -2, 0, -1),
        (start: 3, 4, 5, end: 1, 1, 1, diff: -2, -3, -4),

        #endregion

        #region END/m/y.

        (start: 3, 4, 30, end: 4, 5, 1, diff: 1, 0, 1),
        (start: 3, 4, 30, end: 4, 4, 30, diff: 1, 0, 0),

        (start: 3, 4, 30, end: 4, 4, 29, diff: 0, 11, 30),
        (start: 3, 4, 30, end: 4, 4, 2, diff: 0, 11, 3),
        (start: 3, 4, 30, end: 4, 4, 1, diff: 0, 11, 2),
        (start: 3, 4, 30, end: 4, 3, 31, diff: 0, 11, 1),
        (start: 3, 4, 30, end: 4, 3, 30, diff: 0, 11, 0),

        (start: 3, 4, 30, end: 4, 3, 29, diff: 0, 10, 29),
        (start: 3, 4, 30, end: 4, 3, 1, diff: 0, 10, 1),
        // XXX ( start: 3, 4, 30, end: 4,  2, 29, diff:  0,  10,   0 ),

        (start: 3, 4, 30, end: 4, 2, 28, diff: 0, 9, 29),
        (start: 3, 4, 30, end: 4, 2, 1, diff: 0, 9, 2),
        (start: 3, 4, 30, end: 4, 1, 31, diff: 0, 9, 1),
        (start: 3, 4, 30, end: 4, 1, 30, diff: 0, 9, 0),

        (start: 3, 4, 30, end: 2, 5, 1, diff: 0, -11, -29),
        (start: 3, 4, 30, end: 2, 4, 30, diff: -1, 0, 0),
        (start: 3, 4, 30, end: 2, 4, 29, diff: -1, 0, -1),
        (start: 3, 4, 30, end: 2, 4, 2, diff: -1, 0, -28),
        (start: 3, 4, 30, end: 2, 4, 1, diff: -1, 0, -29),
        // REVIEW(data): test case. Should we modify Subtract()?
        // XXX ( start: 3, 4, 30, end: 2,  3, 31, diff: -1,  -1,   1 ),
        // XXX ( start: 2, 3, 31, end: 3,  4, 30, diff:  1,   1,   0 ),

        (start: 3, 4, 30, end: 2, 3, 30, diff: -1, -1, 0),

        #endregion

        #region Intercalary day.

        // end is in a leap year and after start.
        // 29/02/0008 -> 29/02/0012 -> 01/03/0012.
        (start: 8, 2, 29, end: 12, 3, 1, diff: 4, 0, 1),
        // 29/02/0008 -> 29/02/0012.
        (start: 8, 2, 29, end: 12, 2, 29, diff: 4, 0, 0),
        // 29/02/0008 -> 28/02/0011 -> 28/02/0012.
        // NB: newStart for CountMonthsBetween() is 29/01/0012, not 28/02/0011.
        (start: 8, 2, 29, end: 12, 2, 28, diff: 3, 11, 30),
        // 29/02/0008 -> 28/02/0011 -> 27/02/0012.
        // NB: newStart for CountMonthsBetween() is 29/01/0012, not 28/02/0011.
        (start: 8, 2, 29, end: 12, 2, 27, diff: 3, 11, 29),

        // end is in a common year and after start.
        // 29/02/0008 -> 28/02/0011 -> 01/03/0011.
        (start: 8, 2, 29, end: 11, 3, 1, diff: 3, 0, 1),
        // 29/02/0008 -> 28/02/0011.
        // XXXX ( start: 8, 2, 29, end: 11,  2, 28, diff:  3,   0,   0 ),
        // 29/02/0008 -> 28/02/0010 -> 27/02/0011.
        // NB: newStart for CountMonthsBetween() is 29/01/0011, not 28/02/0010.
        (start: 8, 2, 29, end: 11, 2, 27, diff: 2, 11, 29),

        // 29/02/0008 -> 28/02/0009 -> 01/03/0009.
        (start: 8, 2, 29, end: 9, 3, 1, diff: 1, 0, 1),
        // 29/02/0008 -> 28/02/0009.
        // XXX ( start: 8, 2, 29, end:  9,  2, 28, diff:  1,   0,   0 ),
        // 29/02/0008 -> 29/02/0008 -> 27/02/0009.
        (start: 8, 2, 29, end: 9, 2, 27, diff: 0, 11, 29),

        // end same year.
        (start: 8, 2, 29, end: 8, 3, 1, diff: 0, 0, 1),
        (start: 8, 2, 29, end: 8, 2, 28, diff: 0, 0, -1),
        (start: 8, 2, 29, end: 8, 2, 27, diff: 0, 0, -2),

        // end is in a common year and before start.
        (start: 8, 2, 29, end: 7, 3, 1, diff: 0, -11, -28),
        (start: 8, 2, 29, end: 7, 2, 28, diff: -1, 0, 0),
        (start: 8, 2, 29, end: 7, 2, 27, diff: -1, 0, -1),

        // end is in a leap year and before start.
        (start: 8, 2, 29, end: 4, 3, 1, diff: -3, -11, -28),
        (start: 8, 2, 29, end: 4, 2, 29, diff: -4, 0, 0),
        (start: 8, 2, 29, end: 4, 2, 28, diff: -4, 0, -1),

        #endregion
    };

    // Start date, end date, exact diff between.
    private static readonly List<(int start, int, int, int end, int, int, int diff, int, int)> s_DiffCutOff = new()
    {
        (start: 3, 4, 30, end: 4, 2, 29, diff: 0, 9, 30),
        (start: 3, 4, 30, end: 2, 3, 31, diff: -1, 0, -30),
        (start: 3, 4, 30, end: 3, 3, 31, diff: 0, 0, -30),

        (start: 8, 2, 29, end: 11, 2, 28, diff: 2, 11, 30),
        (start: 8, 2, 29, end: 9, 2, 28, diff: 0, 11, 30),
    };
}
