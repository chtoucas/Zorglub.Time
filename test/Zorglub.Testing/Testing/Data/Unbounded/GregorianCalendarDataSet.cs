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

    private DataGroup<DayNumberInfo>? _dayNumberInfoData;
    public override DataGroup<DayNumberInfo> DayNumberInfoData =>
        _dayNumberInfoData ??= DataGroup.CreateDayNumberInfoData(GregorianDataSet.DaysSinceRataDieInfos);

    public TheoryData<YemoAnd<int>> DaysInYearAfterMonthData => DataSet.DaysInYearAfterMonthData;

    // IYearAdjustmentDataSet
    public TheoryData<YemodaAnd<int>> InvalidYearAdjustementData => DataSet.InvalidYearAdjustementData;
    public TheoryData<YemodaAnd<int>> YearAdjustementData => DataSet.YearAdjustementData;

    // IMathDataSet
    public TheoryData<Yemoda, Yemoda, int> AddDaysData => DataSet.AddDaysData;
    public TheoryData<Yemoda, Yemoda> ConsecutiveDaysData => DataSet.ConsecutiveDaysData;

    // IAdvancedMathDataSet
    public TheoryData<Yemoda, Yemoda, int> AddYearsData => DataSet.AddYearsData;
    public TheoryData<Yemoda, Yemoda, int> AddMonthsData => DataSet.AddMonthsData;
    public TheoryData<Yemoda, Yemoda, int, int, int> DiffData => DataSet.DiffData;
}
