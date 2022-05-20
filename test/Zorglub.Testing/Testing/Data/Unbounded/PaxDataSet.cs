// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for the (unbounded) Pax calendar.
/// </summary>
public sealed class UnboundedPaxDataSet : UnboundedCalendarDataSet<PaxDataSet>, ISingleton<UnboundedPaxDataSet>
{
    private static readonly DayNumber s_Epoch = CalendarEpoch.SundayBeforeGregorian;

    private UnboundedPaxDataSet() : base(PaxDataSet.Instance, s_Epoch) { }

    public static UnboundedPaxDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedPaxDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(PaxDataSet.DaysSinceZeroInfos);

    /// <summary>Day number, year, week of the year, day of the week.</summary>
    public static TheoryData<DayNumber, int, int, DayOfWeek> MoreDayNumberInfoData =>
        MapToTheoryData(PaxDataSet.MoreDaySinceEpochInfos);

    [Pure]
    private static TheoryData<DayNumber, int, int, DayOfWeek> MapToTheoryData(
        IEnumerable<(int DaysSinceEpoch, int Year, int WeekOfYear, DayOfWeek DayOfWeek)> source)
    {
        var epoch = s_Epoch;
        var data = new TheoryData<DayNumber, int, int, DayOfWeek>();
        foreach (var (daysSinceEpoch, y, woy, dayOfWeek) in source)
        {
            data.Add(epoch + daysSinceEpoch, y, woy, dayOfWeek);
        }
        return data;
    }
}
