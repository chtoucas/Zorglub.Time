// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

public partial class TabularIslamicDataSet // IMathDataSet
{
    public override DataGroup<YemodaPair> ConsecutiveDaysData { get; } = new DataGroup<YemodaPair>()
    {
        // End of month.
        new(new(CommonYear, 1, 30), new(CommonYear, 2, 1)),
        new(new(CommonYear, 2, 29), new(CommonYear, 3, 1)),
        new(new(CommonYear, 3, 30), new(CommonYear, 4, 1)),
        new(new(CommonYear, 4, 29), new(CommonYear, 5, 1)),
        new(new(CommonYear, 5, 30), new(CommonYear, 6, 1)),
        new(new(CommonYear, 6, 29), new(CommonYear, 7, 1)),
        new(new(CommonYear, 7, 30), new(CommonYear, 8, 1)),
        new(new(CommonYear, 8, 29), new(CommonYear, 9, 1)),
        new(new(CommonYear, 9, 30), new(CommonYear, 10, 1)),
        new(new(CommonYear, 10, 29), new(CommonYear, 11, 1)),
        new(new(CommonYear, 11, 30), new(CommonYear, 12, 1)),
        new(new(CommonYear, 12, 29), new(CommonYear + 1, 1, 1)),
        new(new(LeapYear, 12, 29), new(LeapYear, 12, 30)),
        new(new(LeapYear, 12, 30), new(LeapYear + 1, 1, 1)),
    }.ConcatT(ConsecutiveDaysSamples);

    public override DataGroup<YedoyPair> ConsecutiveDaysOrdinalData { get; } = new DataGroup<YedoyPair>()
    {
        // End of year.
        new(new(CommonYear, 354), new(CommonYear + 1, 1)),
        new(new(LeapYear, 354), new(LeapYear, 355)),
        new(new(LeapYear, 355), new(LeapYear + 1, 1)),
    }.ConcatT(ConsecutiveDaysOrdinalSamples);
}
