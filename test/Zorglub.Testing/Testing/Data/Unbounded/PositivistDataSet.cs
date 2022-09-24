// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;

/// <summary>
/// Provides test data for the (unbounded) Positivist calendar.
/// </summary>
public sealed class UnboundedPositivistDataSet :
    UnboundedCalendarDataSet<PositivistDataSet>, ISingleton<UnboundedPositivistDataSet>
{
    private UnboundedPositivistDataSet() : base(PositivistDataSet.Instance, DayZero.Positivist) { }

    public static UnboundedPositivistDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedPositivistDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(PositivistDataSet.DaysSinceZeroInfos);
}
