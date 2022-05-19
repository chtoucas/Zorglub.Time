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
    private UnboundedPaxDataSet() : base(PaxDataSet.Instance, CalendarEpoch.SundayBeforeGregorian) { }

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
        MapToTheoryData(s_MoreDayNumberInfoData);

    [Pure]
    private static TheoryData<DayNumber, int, int, DayOfWeek> MapToTheoryData(
        IEnumerable<(int Ord, int Year, int WeekOfYear, DayOfWeek DayOfWeek)> source)
    {
        var data = new TheoryData<DayNumber, int, int, DayOfWeek>();
        foreach (var (ord, y, woy, dayOfWeek) in source)
        {
            data.Add(DayZero.NewStyle + (ord - 1), y, woy, dayOfWeek);
        }
        return data;
    }

    private static readonly List<(int Ord, int Year, int WeekOfYear, DayOfWeek DayOfWeek)> s_MoreDayNumberInfoData = new()
    {
        // First week.
        (0, 1, 1, DayOfWeek.Sunday),
        (1, 1, 1, DayOfWeek.Monday),
        (2, 1, 1, DayOfWeek.Tuesday),
        (3, 1, 1, DayOfWeek.Wednesday),
        (4, 1, 1, DayOfWeek.Thursday),
        (5, 1, 1, DayOfWeek.Friday),
        (6, 1, 1, DayOfWeek.Saturday),
        // Second week.
        (7, 1, 2, DayOfWeek.Sunday),
        (13, 1, 2, DayOfWeek.Saturday),
        // Last week of first year.
        (357, 1, 52, DayOfWeek.Sunday),
        (363, 1, 52, DayOfWeek.Saturday),
        // First week of second year.
        (364, 2, 1, DayOfWeek.Sunday),
        (370, 2, 1, DayOfWeek.Saturday),
    };
}
