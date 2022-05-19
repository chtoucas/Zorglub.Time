// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for the (unbounded) Positivist calendar.
/// </summary>
public sealed class PositivistCalendarDataSet :
    UnboundedCalendarDataSet<PositivistDataSet>, ISingleton<PositivistCalendarDataSet>
{
    private PositivistCalendarDataSet() : base(PositivistDataSet.Instance, CalendarEpoch.Positivist) { }

    public static PositivistCalendarDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly PositivistCalendarDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(PositivistDataSet.DaysSinceZeroInfos);
}
