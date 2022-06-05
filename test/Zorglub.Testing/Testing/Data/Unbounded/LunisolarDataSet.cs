// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Simple;

/// <summary>
/// Provides test data for the (unbounded) faux Lunisolar calendar.
/// </summary>
public sealed class UnboundedLunisolarDataSet :
    UnboundedCalendarDataSet<LunisolarDataSet>, ISingleton<UnboundedLunisolarDataSet>
{
    private UnboundedLunisolarDataSet() : base(LunisolarDataSet.Instance, UserCalendars.LunisolarEpoch) { }

    public static UnboundedLunisolarDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedLunisolarDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(LunisolarDataSet.DaysSinceRataDieInfos);
}
