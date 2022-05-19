// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

public partial class Coptic12DataSet // IEpagomenalDataSet
{
    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData { get; } = new()
    {
        // Common year.
        new(CommonYear, 12, 31, 1),
        new(CommonYear, 12, 32, 2),
        new(CommonYear, 12, 33, 3),
        new(CommonYear, 12, 34, 4),
        new(CommonYear, 12, 35, 5),
        // Leap year.
        new(LeapYear, 12, 31, 1),
        new(LeapYear, 12, 32, 2),
        new(LeapYear, 12, 33, 3),
        new(LeapYear, 12, 34, 4),
        new(LeapYear, 12, 35, 5),
        new(LeapYear, 12, 36, 6),
    };
}

public partial class Coptic13DataSet // IEpagomenalDataSet
{
    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData { get; } = new()
    {
        // Common year.
        new(CommonYear, 13, 1, 1),
        new(CommonYear, 13, 2, 2),
        new(CommonYear, 13, 3, 3),
        new(CommonYear, 13, 4, 4),
        new(CommonYear, 13, 5, 5),
        // Leap year.
        new(LeapYear, 13, 1, 1),
        new(LeapYear, 13, 2, 2),
        new(LeapYear, 13, 3, 3),
        new(LeapYear, 13, 4, 4),
        new(LeapYear, 13, 5, 5),
        new(LeapYear, 13, 6, 6),
    };
}
