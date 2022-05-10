// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

public interface IDayOfWeekDataSet
{
    TheoryData<YemodaAnd<DayOfWeek>> DayOfWeekData { get; }

    TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_Before_Data { get; }
    TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_OnOrBefore_Data { get; }
    TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_Nearest_Data { get; }
    TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_OnOrAfter_Data { get; }
    TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_After_Data { get; }
}
