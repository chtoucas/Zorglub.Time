// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

public partial class Egyptian12DataSet // IEpagomenalDataSet
{
    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData { get; } = new()
    {
        new(SampleYear, 12, 31, 1),
        new(SampleYear, 12, 32, 2),
        new(SampleYear, 12, 33, 3),
        new(SampleYear, 12, 34, 4),
        new(SampleYear, 12, 35, 5),
    };
}

public partial class Egyptian13DataSet // IEpagomenalDataSet
{
    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData { get; } = new()
    {
        new(SampleYear, 13, 1, 1),
        new(SampleYear, 13, 2, 2),
        new(SampleYear, 13, 3, 3),
        new(SampleYear, 13, 4, 4),
        new(SampleYear, 13, 5, 5),
    };
}
