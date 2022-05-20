// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Bounded;

using Zorglub.Testing.Data.Schemas;
using Zorglub.Testing.Data.Unbounded;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;
using Zorglub.Time.Simple;

/// <summary>
/// Defines test data for a calendar with years within the range [1..9999] and provides a base for
/// derived classes.
/// </summary>
/// <typeparam name="TDataSet">The type that represents the original calendar dataset.</typeparam>
public class StandardCalendarDataSet<TDataSet> : MinMaxYearCalendarDataSet<TDataSet>
    where TDataSet : UnboundedCalendarDataSet
{
    public StandardCalendarDataSet(TDataSet inner)
        : base(inner, StandardShortScope.MinYear, StandardShortScope.MaxYear) { }
}

/// <summary>
/// Provides test data for the Armenian calendar with years within the range [1..9999].
/// </summary>
public sealed class StandardArmenian12DataSet :
    StandardCalendarDataSet<UnboundedArmenian12DataSet>, IEpagomenalDataSet, ISingleton<StandardArmenian12DataSet>
{
    private StandardArmenian12DataSet() : base(UnboundedArmenian12DataSet.Instance) { }

    public static StandardArmenian12DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly StandardArmenian12DataSet Instance = new();
        static Singleton() { }
    }

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => Inner.EpagomenalDayInfoData.WhereT(DataFilter.Filter);
}

/// <summary>
/// Provides test data for the Coptic calendar with years within the range [1..9999].
/// </summary>
public sealed class StandardCoptic12DataSet :
    StandardCalendarDataSet<UnboundedCoptic12DataSet>, IEpagomenalDataSet, ISingleton<StandardCoptic12DataSet>
{
    private StandardCoptic12DataSet() : base(UnboundedCoptic12DataSet.Instance) { }

    public static StandardCoptic12DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly StandardCoptic12DataSet Instance = new();
        static Singleton() { }
    }

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => Inner.EpagomenalDayInfoData.WhereT(DataFilter.Filter);
}

/// <summary>
/// Provides test data for the Ethiopic calendar with years within the range [1..9999].
/// </summary>
public sealed class StandardEthiopic12DataSet :
    StandardCalendarDataSet<UnboundedEthiopic12DataSet>, IEpagomenalDataSet, ISingleton<StandardEthiopic12DataSet>
{
    private StandardEthiopic12DataSet() : base(UnboundedEthiopic12DataSet.Instance) { }

    public static StandardEthiopic12DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly StandardEthiopic12DataSet Instance = new();
        static Singleton() { }
    }

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => Inner.EpagomenalDayInfoData.WhereT(DataFilter.Filter);
}

/// <summary>
/// Provides test data for the Gregorian calendar with years within the range [1..9999].
/// </summary>
public sealed class StandardGregorianDataSet :
    StandardCalendarDataSet<UnboundedGregorianDataSet>,
    IYearAdjustmentDataSet,
    IMathDataSet,
    IAdvancedMathDataSet,
    IDayOfWeekDataSet,
    ISingleton<StandardGregorianDataSet>
{
    private StandardGregorianDataSet() : base(UnboundedGregorianDataSet.Instance) { }

    public static StandardGregorianDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly StandardGregorianDataSet Instance = new();
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
/// Provides test data for the Tabular Islamic calendar with years within the range [1..9999].
/// </summary>
public sealed class StandardTabularIslamicDataSet :
    StandardCalendarDataSet<UnboundedTabularIslamicDataSet>, ISingleton<StandardTabularIslamicDataSet>
{
    private StandardTabularIslamicDataSet() : base(UnboundedTabularIslamicDataSet.Instance) { }

    public static StandardTabularIslamicDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly StandardTabularIslamicDataSet Instance = new();
        static Singleton() { }
    }
}

/// <summary>
/// Provides test data for the Zoroastrian calendar with years within the range [1..9999].
/// </summary>
public sealed class StandardZoroastrian12DataSet :
    StandardCalendarDataSet<UnboundedZoroastrian12DataSet>, IEpagomenalDataSet, ISingleton<StandardZoroastrian12DataSet>
{
    private StandardZoroastrian12DataSet() : base(UnboundedZoroastrian12DataSet.Instance) { }

    public static StandardZoroastrian12DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly StandardZoroastrian12DataSet Instance = new();
        static Singleton() { }
    }

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => Inner.EpagomenalDayInfoData.WhereT(DataFilter.Filter);
}
