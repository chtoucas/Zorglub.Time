// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for the (unbounded) faux Lunisolar calendar.
/// </summary>
public sealed class LunisolarCalendarDataSet :
    UnboundedCalendarDataSet<LunisolarDataSet>, ISingleton<LunisolarCalendarDataSet>
{
    private LunisolarCalendarDataSet() : base(LunisolarDataSet.Instance, CalendarEpoch.Positivist) { }

    public static LunisolarCalendarDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly LunisolarCalendarDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(LunisolarDataSet.DaysSinceRataDieInfos);
}
