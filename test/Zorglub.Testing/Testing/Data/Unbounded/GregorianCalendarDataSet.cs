// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Core;

/// <summary>
/// Provides test data for the (unbounded) Gregorian calendar.
/// </summary>
public sealed partial class GregorianCalendarDataSet :
    UnboundedCalendarDataSet<GregorianDataSet>,
    IYearAdjustmentDataSet,
    IAdvancedMathDataSet,
    IDayOfWeekDataSet,
    ISingleton<GregorianCalendarDataSet>
{
    private GregorianCalendarDataSet() : base(GregorianDataSet.Instance, DayZero.NewStyle) { }

    public static GregorianCalendarDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly GregorianCalendarDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(GregorianDataSet.DaysSinceRataDieInfos);

    // IYearAdjustmentDataSet
    public DataGroup<YemodaAnd<int>> InvalidYearAdjustementData => CalendricalDataSet.InvalidYearAdjustementData;
    public DataGroup<YemodaAnd<int>> YearAdjustementData => CalendricalDataSet.YearAdjustementData;

    // IMathDataSet
    public DataGroup<YemodaPairAnd<int>> AddDaysData => CalendricalDataSet.AddDaysData;
    public DataGroup<YemodaPair> ConsecutiveDaysData => CalendricalDataSet.ConsecutiveDaysData;

    // IAdvancedMathDataSet
    public DataGroup<YemodaPairAnd<int>> AddYearsData => CalendricalDataSet.AddYearsData;
    public DataGroup<YemodaPairAnd<int>> AddMonthsData => CalendricalDataSet.AddMonthsData;
    public TheoryData<Yemoda, Yemoda, int, int, int> DiffData => CalendricalDataSet.DiffData;
}
