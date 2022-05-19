// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;

/// <summary>
/// Provides test data for the (unbounded) Julian calendar.
/// </summary>
public sealed class JulianCalendarDataSet :
    UnboundedCalendarDataSet<JulianDataSet>, ISingleton<JulianCalendarDataSet>
{
    private JulianCalendarDataSet() : base(JulianDataSet.Instance, DayZero.OldStyle) { }

    public static JulianCalendarDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly JulianCalendarDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(JulianDataSet.DaysSinceRataDieInfos);
}
