// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Bounded;

using Zorglub.Testing.Data.Schemas;
using Zorglub.Testing.Data.Unbounded;
using Zorglub.Time.Hemerology.Scopes;
using Zorglub.Time.Simple;

/// <summary>
/// Defines test data for a calendar with years within the range [-9998..9999] and provides a base
/// for derived classes.
/// </summary>
/// <typeparam name="TDataSet">The type that represents the original calendar dataset.</typeparam>
public class ProlepticCalendarDataSet<TDataSet> : MinMaxYearCalendarDataSet<TDataSet>
    where TDataSet : UnboundedCalendarDataSet
{
    public ProlepticCalendarDataSet(TDataSet inner)
        : base(inner, ProlepticShortScope.MinYear, ProlepticShortScope.MaxYear) { }
}

/// <summary>
/// Provides test data for Gregorian calendar with years within the range [-9998..9999].
/// </summary>
public sealed class ProlepticGregorianDataSet :
    ProlepticCalendarDataSet<UnboundedGregorianDataSet>,
    IYearAdjustmentDataSet,
    IMathDataSet,
    IAdvancedMathDataSet,
    IDayOfWeekDataSet,
    ISingleton<ProlepticGregorianDataSet>
{
    private ProlepticGregorianDataSet() : base(UnboundedGregorianDataSet.Instance) { }

    public static ProlepticGregorianDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly ProlepticGregorianDataSet Instance = new();
        static Singleton() { }
    }

    // IYearAdjustmentDataSet
    public DataGroup<YemodaAnd<int>> InvalidYearAdjustementData => Inner.InvalidYearAdjustementData.WhereT(DataFilter.Filter);
    public DataGroup<YemodaAnd<int>> YearAdjustementData => Inner.YearAdjustementData.WhereT(DataFilter.Filter);

    // IMathDataSet
    public DataGroup<YemodaPairAnd<int>> AddDaysData => Inner.AddDaysData.WhereT(DataFilter.Filter);
    public DataGroup<YemodaPair> ConsecutiveDaysData => Inner.ConsecutiveDaysData.WhereT(DataFilter.Filter);

    // IAdvancedMathDataSet
    public AddAdjustment AddAdjustment => Inner.AddAdjustment;
    public DataGroup<YemodaPairAnd<int>> AddYearsData => Inner.AddYearsData.WhereT(DataFilter.Filter);
    public DataGroup<YemodaPairAnd<int>> AddMonthsData => Inner.AddMonthsData.WhereT(DataFilter.Filter);
    public DataGroup<DateDiff> DateDiffData => Inner.DateDiffData.WhereT(DataFilter.Filter);

    // IDayOfWeekDataSet
    public DataGroup<YemodaAnd<DayOfWeek>> DayOfWeekData => Inner.DayOfWeekData.WhereT(DataFilter.Filter);
    public DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_Before_Data => Inner.DayOfWeek_Before_Data.WhereT(DataFilter.Filter);
    public DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_OnOrBefore_Data => Inner.DayOfWeek_OnOrBefore_Data.WhereT(DataFilter.Filter);
    public DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_Nearest_Data => Inner.DayOfWeek_Nearest_Data.WhereT(DataFilter.Filter);
    public DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_OnOrAfter_Data => Inner.DayOfWeek_OnOrAfter_Data.WhereT(DataFilter.Filter);
    public DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_After_Data => Inner.DayOfWeek_After_Data.WhereT(DataFilter.Filter);
}

/// <summary>
/// Provides test data for the Julian calendar with years within the range [-9998..9999].
/// </summary>
public sealed class ProlepticJulianDataSet :
    ProlepticCalendarDataSet<UnboundedJulianDataSet>, ISingleton<ProlepticJulianDataSet>
{
    private ProlepticJulianDataSet() : base(UnboundedJulianDataSet.Instance) { }

    public static ProlepticJulianDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly ProlepticJulianDataSet Instance = new();
        static Singleton() { }
    }
}
