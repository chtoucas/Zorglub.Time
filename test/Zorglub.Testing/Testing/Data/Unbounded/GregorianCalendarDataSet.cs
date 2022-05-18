// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Core;

/// <summary>
/// Provides test data for the (unbounded) Gregorian calendar and related date types.
/// </summary>
public sealed partial class GregorianCalendarDataSet :
    CalendarDataSet<GregorianDataSet>,
    IYearAdjustmentDataSet,
    IAdvancedMathDataSet,
    IDayOfWeekDataSet,
    ISingleton<GregorianCalendarDataSet>
{
    private GregorianCalendarDataSet() : base(GregorianDataSet.Instance, DayZero.NewStyle) { }

    public static GregorianCalendarDataSet Instance { get; } = new();

    public override DataGroup<DayNumberInfo> DayNumberInfoData =>
        DataGroup.CreateDayNumberInfoData(GregorianDataSet.DaysSinceRataDieInfos);

    public DataGroup<YemoAnd<int>> DaysInYearAfterMonthData => DataSet.DaysInYearAfterMonthData;

    // IYearAdjustmentDataSet
    public DataGroup<YemodaAnd<int>> InvalidYearAdjustementData => DataSet.InvalidYearAdjustementData;
    public DataGroup<YemodaAnd<int>> YearAdjustementData => DataSet.YearAdjustementData;

    // IMathDataSet
    public DataGroup<YemodaPairAnd<int>> AddDaysData => DataSet.AddDaysData;
    public DataGroup<YemodaPair> ConsecutiveDaysData => DataSet.ConsecutiveDaysData;

    // IAdvancedMathDataSet
    public TheoryData<Yemoda, Yemoda, int> AddYearsData => DataSet.AddYearsData;
    public TheoryData<Yemoda, Yemoda, int> AddMonthsData => DataSet.AddMonthsData;
    public TheoryData<Yemoda, Yemoda, int, int, int> DiffData => DataSet.DiffData;
}
