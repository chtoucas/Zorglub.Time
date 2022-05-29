// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

public partial class PositivistDataSet // IMathDataSet
{
    public override DataGroup<YemodaPair> ConsecutiveDaysData { get; } = new DataGroup<YemodaPair>()
    {
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
        new(new(LeapYear, 13, 29), new(LeapYear, 13, 30)),
        new(new(LeapYear, 13, 30), new(LeapYear + 1, 1, 1)),
    }.ConcatT(ConsecutiveDaysSamples);

    public override DataGroup<YedoyPair> ConsecutiveDaysOrdinalData { get; } = new DataGroup<YedoyPair>()
    {
        // End of year.
        new(new(CommonYear, 365), new(CommonYear + 1, 1)),
        new(new(LeapYear, 365), new(LeapYear, 366)),
        new(new(LeapYear, 366), new(LeapYear + 1, 1)),
    }.ConcatT(ConsecutiveDaysOrdinalSamples);
}
