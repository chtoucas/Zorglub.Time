// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for the (unbounded) Persian calendar (proposed arithmetical form).
/// </summary>
public sealed class UnboundedPersian2820DataSet :
    UnboundedCalendarDataSet<Persian2820DataSet>, ISingleton<UnboundedPersian2820DataSet>
{
    private UnboundedPersian2820DataSet() : base(Persian2820DataSet.Instance, DayZero.Persian) { }

    public static UnboundedPersian2820DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedPersian2820DataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(Persian2820DataSet.DaysSinceRataDieInfos);
}

