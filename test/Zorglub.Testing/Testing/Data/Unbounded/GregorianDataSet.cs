// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;

/// <summary>
/// Provides test data for the (unbounded) Gregorian calendar.
/// </summary>
public sealed partial class UnboundedGregorianDataSet :
    UnboundedCalendarDataSet<GregorianDataSet>,
    IDayOfWeekDataSet,
    ISingleton<UnboundedGregorianDataSet>
{
    private UnboundedGregorianDataSet() : base(GregorianDataSet.Instance, DayZero.NewStyle) { }

    public static UnboundedGregorianDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedGregorianDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(GregorianDataSet.DaysSinceRataDieInfos);
}

public partial class UnboundedGregorianDataSet // IDayOfWeekDataSet
{
    public DataGroup<YemodaAnd<DayOfWeek>> DayOfWeekData { get; } = new()
    {
        // Epoch.
        new(1, 1, 1, DayOfWeek.Monday),
        // The 9 following days.
        // Used to test Previous(dayOfWeek) and related methods.
        new(1, 1, 2, DayOfWeek.Tuesday),
        new(1, 1, 3, DayOfWeek.Wednesday),
        new(1, 1, 4, DayOfWeek.Thursday),
        new(1, 1, 5, DayOfWeek.Friday),
        new(1, 1, 6, DayOfWeek.Saturday),
        new(1, 1, 7, DayOfWeek.Sunday),
        new(1, 1, 8, DayOfWeek.Monday),
        new(1, 1, 9, DayOfWeek.Tuesday),
        new(1, 1, 10, DayOfWeek.Wednesday),

        // Common year.
        // A week.
        new(51, 11, 6, DayOfWeek.Monday),
        new(51, 11, 7, DayOfWeek.Tuesday),
        new(51, 11, 8, DayOfWeek.Wednesday),
        new(51, 11, 9, DayOfWeek.Thursday),
        new(51, 11, 10, DayOfWeek.Friday),
        new(51, 11, 11, DayOfWeek.Saturday),
        new(51, 11, 12, DayOfWeek.Sunday),
        // January.
        new(51, 1, 1, DayOfWeek.Sunday),
        new(51, 1, 31, DayOfWeek.Tuesday),
        // February.
        new(51, 2, 1, DayOfWeek.Wednesday),
        new(51, 2, 28, DayOfWeek.Tuesday),

        // Leap year.
        // A week.
        new(40, 11, 5, DayOfWeek.Monday),
        new(40, 11, 6, DayOfWeek.Tuesday),
        new(40, 11, 7, DayOfWeek.Wednesday),
        new(40, 11, 8, DayOfWeek.Thursday),
        new(40, 11, 9, DayOfWeek.Friday),
        new(40, 11, 10, DayOfWeek.Saturday),
        new(40, 11, 11, DayOfWeek.Sunday),
        // January.
        new(40, 1, 1, DayOfWeek.Sunday),
        new(40, 1, 31, DayOfWeek.Tuesday),
        // February.
        new(40, 2, 1, DayOfWeek.Wednesday),
        new(40, 2, 29, DayOfWeek.Wednesday),

        // The month of the (official) Gregorian reform.
        new(1582, 12, 1, DayOfWeek.Wednesday),
        new(1582, 12, 2, DayOfWeek.Thursday),
        new(1582, 12, 3, DayOfWeek.Friday),
        new(1582, 12, 4, DayOfWeek.Saturday),
        new(1582, 12, 5, DayOfWeek.Sunday),
        new(1582, 12, 6, DayOfWeek.Monday),
        new(1582, 12, 7, DayOfWeek.Tuesday),
        new(1582, 12, 8, DayOfWeek.Wednesday),
        new(1582, 12, 9, DayOfWeek.Thursday),
        new(1582, 12, 10, DayOfWeek.Friday),
        new(1582, 12, 11, DayOfWeek.Saturday),
        new(1582, 12, 12, DayOfWeek.Sunday),
        new(1582, 12, 13, DayOfWeek.Monday),
        new(1582, 12, 14, DayOfWeek.Tuesday),
        new(1582, 12, 15, DayOfWeek.Wednesday),
        new(1582, 12, 16, DayOfWeek.Thursday),
        new(1582, 12, 17, DayOfWeek.Friday),
        new(1582, 12, 18, DayOfWeek.Saturday),
        new(1582, 12, 19, DayOfWeek.Sunday),
        new(1582, 12, 20, DayOfWeek.Monday),
        new(1582, 12, 21, DayOfWeek.Tuesday),
        new(1582, 12, 22, DayOfWeek.Wednesday),
        new(1582, 12, 23, DayOfWeek.Thursday),
        new(1582, 12, 24, DayOfWeek.Friday),
        new(1582, 12, 25, DayOfWeek.Saturday),
        new(1582, 12, 26, DayOfWeek.Sunday),
        new(1582, 12, 27, DayOfWeek.Monday),
        new(1582, 12, 28, DayOfWeek.Tuesday),
        new(1582, 12, 29, DayOfWeek.Wednesday),
        new(1582, 12, 30, DayOfWeek.Thursday),
        new(1582, 12, 31, DayOfWeek.Friday),

        // Common year.
        // A week.
        new(2018, 11, 5, DayOfWeek.Monday),
        new(2018, 11, 6, DayOfWeek.Tuesday),
        new(2018, 11, 7, DayOfWeek.Wednesday),
        new(2018, 11, 8, DayOfWeek.Thursday),
        new(2018, 11, 9, DayOfWeek.Friday),
        new(2018, 11, 10, DayOfWeek.Saturday),
        new(2018, 11, 11, DayOfWeek.Sunday),
        // January.
        new(2018, 1, 1, DayOfWeek.Monday),
        new(2018, 1, 31, DayOfWeek.Wednesday),
        // Feburary.
        new(2018, 2, 1, DayOfWeek.Thursday),
        new(2018, 2, 28, DayOfWeek.Wednesday),

        // Leap year.
        // A week.
        new(2020, 11, 2, DayOfWeek.Monday),
        new(2020, 11, 3, DayOfWeek.Tuesday),
        new(2020, 11, 4, DayOfWeek.Wednesday),
        new(2020, 11, 8, DayOfWeek.Sunday),
        new(2020, 11, 5, DayOfWeek.Thursday),
        new(2020, 11, 6, DayOfWeek.Friday),
        new(2020, 11, 7, DayOfWeek.Saturday),
        // January.
        new(2020, 1, 1, DayOfWeek.Wednesday),
        new(2020, 1, 31, DayOfWeek.Friday),
        // Feburary.
        new(2020, 2, 1, DayOfWeek.Saturday),
        new(2020, 2, 29, DayOfWeek.Saturday),

        // The 8 days before the last supported date.
        // Used to test Next(dayOfWeek) and related methods.
        new(9999, 12, 22, DayOfWeek.Wednesday),
        new(9999, 12, 23, DayOfWeek.Thursday),
        new(9999, 12, 24, DayOfWeek.Friday),
        new(9999, 12, 25, DayOfWeek.Saturday),
        new(9999, 12, 26, DayOfWeek.Sunday),
        new(9999, 12, 27, DayOfWeek.Monday),
        new(9999, 12, 28, DayOfWeek.Tuesday),
        new(9999, 12, 29, DayOfWeek.Wednesday),
        new(9999, 12, 30, DayOfWeek.Thursday),
        // Last supported date.
        new(9999, 12, 31, DayOfWeek.Friday),
    };

    public DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_Before_Data { get; } = new()
    {
        // Same day a week before.
        new(new(2018, 12, 13), new(2018, 12,  6),  DayOfWeek.Thursday ),
        // Preceding days.
        new(new(2018, 12, 13), new(2018, 12,  7),  DayOfWeek.Friday ),
        new(new(2018, 12, 13), new(2018, 12,  8),  DayOfWeek.Saturday ),
        new(new(2018, 12, 13), new(2018, 12,  9),  DayOfWeek.Sunday ),
        new(new(2018, 12, 13), new(2018, 12, 10),  DayOfWeek.Monday ),
        new(new(2018, 12, 13), new(2018, 12, 11),  DayOfWeek.Tuesday ),
        new(new(2018, 12, 13), new(2018, 12, 12),  DayOfWeek.Wednesday ),

        // Change of year.
        new(new(2018,  1,  1), new(2017, 12, 31),  DayOfWeek.Sunday ),

        // Change of month.
        new(new(2018, 12,  1), new(2018, 11, 28),  DayOfWeek.Wednesday ),
        // February, common year.
        new(new(2018,  3,  1), new(2018,  2, 28),  DayOfWeek.Wednesday ),
        // February, leap year.
        new(new(2016,  3,  1), new(2016,  2, 29),  DayOfWeek.Monday ),
    };

    public DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_OnOrBefore_Data { get; } = new()
    {
        // Preceding days.
        new(new(2018, 12, 13), new(2018, 12,  7),  DayOfWeek.Friday ),
        new(new(2018, 12, 13), new(2018, 12,  8),  DayOfWeek.Saturday ),
        new(new(2018, 12, 13), new(2018, 12,  9),  DayOfWeek.Sunday ),
        new(new(2018, 12, 13), new(2018, 12, 10),  DayOfWeek.Monday ),
        new(new(2018, 12, 13), new(2018, 12, 11),  DayOfWeek.Tuesday ),
        new(new(2018, 12, 13), new(2018, 12, 12),  DayOfWeek.Wednesday ),
        // Same day.
        new(new(2018, 12, 13), new(2018, 12, 13),  DayOfWeek.Thursday ),

        // Change of year.
        new(new(2018,  1,  1), new(2017, 12, 31),  DayOfWeek.Sunday ),

        // Change of month.
        new(new(2018, 12,  1), new(2018, 11, 28),  DayOfWeek.Wednesday ),
        // February, common year.
        new(new(2018,  3,  1), new(2018,  2, 28),  DayOfWeek.Wednesday ),
        // February, leap year.
        new(new(2016,  3,  1), new(2016,  2, 29),  DayOfWeek.Monday ),
    };

    public DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_Nearest_Data { get; } = new()
    {
        // Preceding days.
        new(new(2018, 12,  6), new(2018, 12,  3),  DayOfWeek.Monday ),
        new(new(2018, 12,  6), new(2018, 12,  4),  DayOfWeek.Tuesday ),
        new(new(2018, 12,  6), new(2018, 12,  5),  DayOfWeek.Wednesday ),
        // Same day.
        new(new(2018, 12,  6), new(2018, 12,  6),  DayOfWeek.Thursday ),
        // Following days.
        new(new(2018, 12,  6), new(2018, 12,  7),  DayOfWeek.Friday ),
        new(new(2018, 12,  6), new(2018, 12,  8),  DayOfWeek.Saturday ),
        new(new(2018, 12,  6), new(2018, 12,  9),  DayOfWeek.Sunday ),

        // Change of year.
        new(new(2018, 12, 31), new(2019,  1,  1),  DayOfWeek.Tuesday ),

        // Change of month.
        new(new(2018, 11, 30), new(2018, 12,  1),  DayOfWeek.Saturday ),
        // February, common year.
        new(new(2018,  2, 28), new(2018,  3,  1),  DayOfWeek.Thursday ),
        // February, leap year.
        new(new(2016,  2, 29), new(2016,  3,  1),  DayOfWeek.Tuesday ),
    };

    public DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_OnOrAfter_Data { get; } = new()
    {
        // Same day.
        new(new(2018, 12,  6), new(2018, 12,  6),  DayOfWeek.Thursday ),
        // Following days.
        new(new(2018, 12,  6), new(2018, 12,  7),  DayOfWeek.Friday ),
        new(new(2018, 12,  6), new(2018, 12,  8),  DayOfWeek.Saturday ),
        new(new(2018, 12,  6), new(2018, 12,  9),  DayOfWeek.Sunday ),
        new(new(2018, 12,  6), new(2018, 12, 10),  DayOfWeek.Monday ),
        new(new(2018, 12,  6), new(2018, 12, 11),  DayOfWeek.Tuesday ),
        new(new(2018, 12,  6), new(2018, 12, 12),  DayOfWeek.Wednesday ),

        // Change of year.
        new(new(2018, 12, 31), new(2019,  1,  1),  DayOfWeek.Tuesday ),

        // Change of month.
        new(new(2018, 11, 30), new(2018, 12,  1),  DayOfWeek.Saturday ),
        // February, common year.
        new(new(2018,  2, 28), new(2018,  3,  1),  DayOfWeek.Thursday ),
        // February, leap year.
        new(new(2016,  2, 29), new(2016,  3,  1),  DayOfWeek.Tuesday ),
    };

    public DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_After_Data { get; } = new()
    {
        // Following days.
        new(new(2018, 12,  6), new(2018, 12,  7),  DayOfWeek.Friday ),
        new(new(2018, 12,  6), new(2018, 12,  8),  DayOfWeek.Saturday ),
        new(new(2018, 12,  6), new(2018, 12,  9),  DayOfWeek.Sunday ),
        new(new(2018, 12,  6), new(2018, 12, 10),  DayOfWeek.Monday ),
        new(new(2018, 12,  6), new(2018, 12, 11),  DayOfWeek.Tuesday ),
        new(new(2018, 12,  6), new(2018, 12, 12),  DayOfWeek.Wednesday ),
        // Same day a week after.
        new(new(2018, 12,  6), new(2018, 12, 13),  DayOfWeek.Thursday ),

        // Change of year.
        new(new(2018, 12, 31), new(2019,  1,  1),  DayOfWeek.Tuesday ),

        // Change of month.
        new(new(2018, 11, 30), new(2018, 12,  1),  DayOfWeek.Saturday ),
        // February, common year.
        new(new(2018,  2, 28), new(2018,  3,  1),  DayOfWeek.Thursday ),
        // February, leap year.
        new(new(2016,  2, 29), new(2016,  3,  1),  DayOfWeek.Tuesday ),
    };
}
