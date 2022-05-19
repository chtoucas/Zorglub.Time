// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Bounded;

using Zorglub.Testing.Data.Unbounded;
using Zorglub.Time.Hemerology.Scopes;
using Zorglub.Time.Simple;

/// <summary>
/// Defines test data for a calendar with years within the range [-9998..9999] and provides a base
/// for derived classes.
/// </summary>
/// <typeparam name="TDataSet">The type that represents the original calendar dataset.</typeparam>
public class ProlepticCalendarDataSet<TDataSet> : MinMaxYearCalendarDataSet<TDataSet>
    where TDataSet : ICalendarDataSet
{
    public ProlepticCalendarDataSet(TDataSet inner)
        : base(inner, ProlepticShortScope.MinYear, ProlepticShortScope.MaxYear) { }
}

/// <summary>
/// Provides test data for Gregorian calendar with years within the range [-9998..9999].
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

    public DataGroup<YemoAnd<int>> DaysInYearAfterMonthData => Inner.DaysInYearAfterMonthData.WhereT(DataFilter.Filter);

    // IYearAdjustmentDataSet
    public DataGroup<YemodaAnd<int>> InvalidYearAdjustementData => Inner.InvalidYearAdjustementData.WhereT(DataFilter.Filter);
    public DataGroup<YemodaAnd<int>> YearAdjustementData => Inner.YearAdjustementData.WhereT(DataFilter.Filter);

    // IMathDataSet
    public DataGroup<YemodaPairAnd<int>> AddDaysData => Inner.AddDaysData.WhereT(DataFilter.Filter);
    public DataGroup<YemodaPair> ConsecutiveDaysData => Inner.ConsecutiveDaysData.WhereT(DataFilter.Filter);

    // IAdvancedMathDataSet
    public TheoryData<Yemoda, Yemoda, int> AddYearsData => Inner.AddYearsData;
    public TheoryData<Yemoda, Yemoda, int> AddMonthsData => Inner.AddMonthsData;
    public TheoryData<Yemoda, Yemoda, int, int, int> DiffData => Inner.DiffData;

    // IDayOfWeekDataSet
    public DataGroup<YemodaAnd<DayOfWeek>> DayOfWeekData => Inner.DayOfWeekData.WhereT(DataFilter.Filter);
    public TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_Before_Data => Inner.DayOfWeek_Before_Data;
    public TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_OnOrBefore_Data => Inner.DayOfWeek_OnOrBefore_Data;
    public TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_Nearest_Data => Inner.DayOfWeek_Nearest_Data;
    public TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_OnOrAfter_Data => Inner.DayOfWeek_OnOrAfter_Data;
    public TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_After_Data => Inner.DayOfWeek_After_Data;
}

/// <summary>
/// Provides test data for the Julian calendar with years within the range [-9998..9999].
/// </summary>
public sealed class ProlepticJulianDataSet :
    ProlepticCalendarDataSet<JulianCalendarDataSet>, ISingleton<ProlepticJulianDataSet>
{
    private ProlepticJulianDataSet() : base(JulianCalendarDataSet.Instance) { }

    public static ProlepticJulianDataSet Instance { get; } = new();
}
