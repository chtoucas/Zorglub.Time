// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Bounded;

using Zorglub.Testing.Data.Unbounded;
using Zorglub.Time.Hemerology.Scopes;
using Zorglub.Time.Simple;

/// <summary>
/// -9998 &lt;= year &lt;= 9999
/// </summary>
public class ProlepticCalendarDataSet<TDataSet> : MinMaxYearCalendarDataSet<TDataSet>
    where TDataSet : ICalendarDataSet
{
    public ProlepticCalendarDataSet(TDataSet inner)
        : base(inner, ProlepticShortScope.MinYear, ProlepticShortScope.MaxYear) { }
}

/// <summary>
/// Provides test data for <see cref="GregorianCalendar"/> and related date types.
/// </summary>
public sealed class ProlepticGregorianDataSet :
    ProlepticCalendarDataSet<GregorianCalendarDataSet>,
    IYearAdjustmentDataSet,
    IAdvancedMathDataSet,
    IDayOfWeekDataSet,
    ISingleton<ProlepticGregorianDataSet>
{
    private ProlepticGregorianDataSet() : base(GregorianCalendarDataSet.Instance) { }

    public static ProlepticGregorianDataSet Instance { get; } = new();

    private DataGroup<YemoAnd<int>>? _daysInYearAfterMonthData;
    public DataGroup<YemoAnd<int>> DaysInYearAfterMonthData =>
        _daysInYearAfterMonthData ??= FilterData(Inner.DaysInYearAfterMonthData, DataFilter.Filter);

    // IYearAdjustmentDataSet
    private DataGroup<YemodaAnd<int>>? _invalidYearAdjustementData;
    public TheoryData<YemodaAnd<int>> InvalidYearAdjustementData =>
        _invalidYearAdjustementData ??= FilterData(Inner.InvalidYearAdjustementData, DataFilter.Filter);

    private DataGroup<YemodaAnd<int>>? _yearAdjustementData;
    public TheoryData<YemodaAnd<int>> YearAdjustementData =>
        _yearAdjustementData ??= FilterData(Inner.YearAdjustementData, DataFilter.Filter);

    // IMathDataSet
    private DataGroup<YemodaPairAnd<int>>? _addDaysData;
    public DataGroup<YemodaPairAnd<int>> AddDaysData =>
        _addDaysData ??= FilterData(Inner.AddDaysData, DataFilter.Filter);

    private DataGroup<YemodaPair>? _consecutiveDaysData;
    public DataGroup<YemodaPair> ConsecutiveDaysData =>
        _consecutiveDaysData ??= FilterData(Inner.ConsecutiveDaysData, DataFilter.Filter);

    // IAdvancedMathDataSet
    public TheoryData<Yemoda, Yemoda, int> AddYearsData => Inner.AddYearsData;
    public TheoryData<Yemoda, Yemoda, int> AddMonthsData => Inner.AddMonthsData;
    public TheoryData<Yemoda, Yemoda, int, int, int> DiffData => Inner.DiffData;

    // IDayOfWeekDataSet
    private DataGroup<YemodaAnd<DayOfWeek>>? _dayOfWeekData;
    public TheoryData<YemodaAnd<DayOfWeek>> DayOfWeekData =>
        _dayOfWeekData ??= FilterData(Inner.DayOfWeekData, DataFilter.Filter);

    public TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_Before_Data => Inner.DayOfWeek_Before_Data;
    public TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_OnOrBefore_Data => Inner.DayOfWeek_OnOrBefore_Data;
    public TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_Nearest_Data => Inner.DayOfWeek_Nearest_Data;
    public TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_OnOrAfter_Data => Inner.DayOfWeek_OnOrAfter_Data;
    public TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_After_Data => Inner.DayOfWeek_After_Data;
}

/// <summary>
/// Provides test data for <see cref="JulianCalendar"/> and related date types.
/// </summary>
public sealed class ProlepticJulianDataSet :
    ProlepticCalendarDataSet<JulianCalendarDataSet>, ISingleton<ProlepticJulianDataSet>
{
    private ProlepticJulianDataSet() : base(JulianCalendarDataSet.Instance) { }

    public static ProlepticJulianDataSet Instance { get; } = new();
}
