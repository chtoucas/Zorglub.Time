// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;

/// <summary>
/// Provides test data for the (unbounded) International Fixed calendar.
/// </summary>
public sealed class UnboundedInternationalFixedDataSet :
    UnboundedCalendarDataSet<InternationalFixedDataSet>, ISingleton<UnboundedInternationalFixedDataSet>
{
    private static readonly DayNumber s_Epoch = DayZero.NewStyle;

    private UnboundedInternationalFixedDataSet() : base(InternationalFixedDataSet.Instance, s_Epoch) { }

    public static UnboundedInternationalFixedDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedInternationalFixedDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(InternationalFixedDataSet.DaysSinceEpochInfos, s_Epoch);
}
