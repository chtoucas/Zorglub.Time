﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Testing.Data.Bounded;

using Zorglub.Testing.Data.Unbounded;
using Zorglub.Time.Hemerology.Scopes;

/// <summary>
/// Defines test data for a calendar with years within the range [-9998..9999] and provides a base
/// for derived classes.
/// </summary>
/// <typeparam name="TDataSet">The type that represents the original calendar dataset.</typeparam>
public class ProlepticCalendarDataSet<TDataSet> : MinMaxYearCalendarDataSet<TDataSet>
    where TDataSet : UnboundedCalendarDataSet
{
    public ProlepticCalendarDataSet(TDataSet inner)
        : base(inner, ProlepticScope.MinYear, ProlepticScope.MaxYear) { }
}

/// <summary>
/// Provides test data for Gregorian calendar with years within the range [-9998..9999].
/// </summary>
public sealed class ProlepticGregorianDataSet :
    ProlepticCalendarDataSet<UnboundedGregorianDataSet>,
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

    // IDayOfWeekDataSet
    public DataGroup<YemodaAnd<DayOfWeek>> DayOfWeekData => Unbounded.DayOfWeekData.WhereT(DataFilter.Filter);
    public DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_Before_Data => Unbounded.DayOfWeek_Before_Data.WhereT(DataFilter.Filter);
    public DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_OnOrBefore_Data => Unbounded.DayOfWeek_OnOrBefore_Data.WhereT(DataFilter.Filter);
    public DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_Nearest_Data => Unbounded.DayOfWeek_Nearest_Data.WhereT(DataFilter.Filter);
    public DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_OnOrAfter_Data => Unbounded.DayOfWeek_OnOrAfter_Data.WhereT(DataFilter.Filter);
    public DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_After_Data => Unbounded.DayOfWeek_After_Data.WhereT(DataFilter.Filter);
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
