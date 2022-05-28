// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

public partial class PositivistDataSet // IMathDataSet
{
    public DataGroup<YemodaPairAnd<int>> AddDaysData { get; } = new()
    {
        // April.
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
    };

    public DataGroup<YemodaPair> ConsecutiveDaysData { get; } = new()
    {
        // April.
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
        new(new(CommonYear, 4, 28), new(CommonYear, 5, 1)),
        new(new(CommonYear, 5, 1), new(CommonYear, 5, 2)),

        // End of month.
        new(new(CommonYear, 1, 28), new(CommonYear, 2, 1)),
        new(new(CommonYear, 2, 28), new(CommonYear, 3, 1)),
        new(new(CommonYear, 3, 28), new(CommonYear, 4, 1)),
        new(new(CommonYear, 4, 28), new(CommonYear, 5, 1)),
        new(new(CommonYear, 5, 28), new(CommonYear, 6, 1)),
        new(new(CommonYear, 6, 28), new(CommonYear, 7, 1)),
        new(new(CommonYear, 7, 28), new(CommonYear, 8, 1)),
        new(new(CommonYear, 8, 28), new(CommonYear, 9, 1)),
        new(new(CommonYear, 9, 28), new(CommonYear, 10, 1)),
        new(new(CommonYear, 10, 28), new(CommonYear, 11, 1)),
        new(new(CommonYear, 11, 28), new(CommonYear, 12, 1)),
        new(new(CommonYear, 12, 28), new(CommonYear, 13, 1)),
        new(new(CommonYear, 13, 29), new(CommonYear + 1, 1, 1)),
        new(new(LeapYear, 13, 30), new(LeapYear + 1, 1, 1)),
    };

    public override DataGroup<YedoyPair> ConsecutiveDaysOrdinalData { get; } = new DataGroup<YedoyPair>()
    {
        // End of year.
        new(new(CommonYear, 365), new(CommonYear + 1, 1)),
        new(new(LeapYear, 365), new(LeapYear, 366)),
        new(new(LeapYear, 366), new(LeapYear + 1, 1)),
    }.ConcatT(ConsecutiveDaysOrdinalSamples);
}
