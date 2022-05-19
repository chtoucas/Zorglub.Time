// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

public static partial class GregorianCutOffMathDataSet { }

public partial class GregorianCutOffMathDataSet // Years
{
    // Intercalary day, expected result in a common year, years to be added.
    public static DataGroup<YemodaPairAnd<int>> AddYearsData { get; } = new()
    {
        new(new(4, 2, 29), new(11, 2, 28), 7),
        new(new(4, 2, 29), new(10, 2, 28), 6),
        new(new(4, 2, 29), new(9, 2, 28), 5),
        new(new(4, 2, 29), new(7, 2, 28), 3),
        new(new(4, 2, 29), new(6, 2, 28), 2),
        new(new(4, 2, 29), new(5, 2, 28), 1),

        new(new(8, 2, 29), new(7, 2, 28), -1),
        new(new(8, 2, 29), new(6, 2, 28), -2),
        new(new(8, 2, 29), new(5, 2, 28), -3),
        new(new(8, 2, 29), new(3, 2, 28), -5),
        new(new(8, 2, 29), new(2, 2, 28), -6),
        new(new(8, 2, 29), new(1, 2, 28), -7),
    };

    // Intercalary day, expected result in a common year, years to be added.
    public static DataGroup<YemodaPairAnd<int>> AddYearsLongData { get; } = new()
    {
        new(new(4, 2, 29), new(11, 3, 1), 7),
        new(new(4, 2, 29), new(10, 3, 1), 6),
        new(new(4, 2, 29), new(9, 3, 1), 5),
        new(new(4, 2, 29), new(7, 3, 1), 3),
        new(new(4, 2, 29), new(6, 3, 1), 2),
        new(new(4, 2, 29), new(5, 3, 1), 1),

        new(new(8, 2, 29), new(7, 3, 1), -1),
        new(new(8, 2, 29), new(6, 3, 1), -2),
        new(new(8, 2, 29), new(5, 3, 1), -3),
        new(new(8, 2, 29), new(3, 3, 1), -5),
        new(new(8, 2, 29), new(2, 3, 1), -6),
        new(new(8, 2, 29), new(1, 3, 1), -7),
    };
}

public partial class GregorianCutOffMathDataSet // Months
{
    // Date, expected result, months to be added.
    public static DataGroup<YemodaPairAnd<int>> AddMonthsData { get; } = new()
    {
        // 31 days long month -> 30 days long month.
        new(new(1, 3, 31), new(1, 6, 30), 3),
        new(new(1, 5, 31), new(1, 4, 30), -1),

        // Target February (common year).
        new(new(1, 1, 29), new(1, 2, 28), 1),
        new(new(1, 1, 30), new(1, 2, 28), 1),
        new(new(1, 1, 31), new(1, 2, 28), 1),
        // Backward.
        new(new(1, 3, 29), new(1, 2, 28), -1),
        new(new(1, 3, 30), new(1, 2, 28), -1),
        new(new(1, 3, 31), new(1, 2, 28), -1),

        // Target February (leap year).
        new(new(4, 1, 30), new(4, 2, 29), 1),
        new(new(4, 1, 31), new(4, 2, 29), 1),
        // Backward.
        new(new(4, 3, 30), new(4, 2, 29), -1),
        new(new(4, 3, 31), new(4, 2, 29), -1),

        // Change of year.
        // Target shorter month.
        new(new(3, 1, 31), new(4, 4, 30), 15),
        new(new(4, 1, 31), new(3, 11, 30), -2),
        // Target end of February.
        new(new(3, 1, 31), new(4, 2, 29), 13),
        new(new(3, 4, 30), new(4, 2, 29), 10),
        new(new(4, 1, 31), new(3, 2, 28), -11),
        new(new(4, 4, 30), new(3, 2, 28), -14),
    };
}

public partial class GregorianCutOffMathDataSet // Diff
{
    // Start date, end date, exact diff between.
    public static TheoryData<Yemoda, Yemoda, int, int, int> DiffData { get; } = new()
    {
        { new(3, 4, 30), new(4, 2, 29), 0, 9, 30 },
        { new(3, 4, 30), new(2, 3, 31), -1, 0, -30 },
        { new(3, 4, 30), new(3, 3, 31), 0, 0, -30 },

        { new(8, 2, 29), new(11, 2, 28), 2, 11, 30 },
        { new(8, 2, 29), new(9, 2, 28), 0, 11, 30 },
    };
}