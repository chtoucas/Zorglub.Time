// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

public partial class Coptic13DataSet // IMathDataSet
{
    public override DataGroup<YemodaPair> ConsecutiveDaysData { get; } = new DataGroup<YemodaPair>()
    {
        // End of month.
        new(new(CommonYear, 1, 30), new(CommonYear, 2, 1)),
        new(new(CommonYear, 2, 30), new(CommonYear, 3, 1)),
        new(new(CommonYear, 3, 30), new(CommonYear, 4, 1)),
        new(new(CommonYear, 4, 30), new(CommonYear, 5, 1)),
        new(new(CommonYear, 5, 30), new(CommonYear, 6, 1)),
        new(new(CommonYear, 6, 30), new(CommonYear, 7, 1)),
        new(new(CommonYear, 7, 30), new(CommonYear, 8, 1)),
        new(new(CommonYear, 8, 30), new(CommonYear, 9, 1)),
        new(new(CommonYear, 9, 30), new(CommonYear, 10, 1)),
        new(new(CommonYear, 10, 30), new(CommonYear, 11, 1)),
        new(new(CommonYear, 11, 30), new(CommonYear, 12, 1)),
        new(new(CommonYear, 12, 30), new(CommonYear, 13, 1)),
        new(new(CommonYear, 13, 5), new(CommonYear + 1, 1, 1)),
        new(new(LeapYear, 13, 5), new(LeapYear, 13, 6)),
        new(new(LeapYear, 13, 6), new(LeapYear + 1, 1, 1)),
    }.ConcatT(ConsecutiveDaysSamples);

    public override DataGroup<YedoyPair> ConsecutiveDaysOrdinalData => new DataGroup<YedoyPair>()
    {
        // End of year.
        new(new(CommonYear, 365), new(CommonYear + 1, 1)),
        new(new(LeapYear, 365), new(LeapYear, 366)),
        new(new(LeapYear, 366), new(LeapYear + 1, 1)),
    }.ConcatT(ConsecutiveDaysOrdinalSamples);
}
