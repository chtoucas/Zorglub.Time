// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Bounded;

using Zorglub.Testing.Data.Unbounded;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;
using Zorglub.Time.Simple;

/// <summary>
/// 1 &lt;= year &lt;= 9999
/// </summary>
public class StandardCalendarDataSet<TDataSet> : MinMaxYearCalendarDataSet<TDataSet>
    where TDataSet : ICalendarDataSet
{
    public StandardCalendarDataSet(TDataSet inner)
        : base(inner, StandardShortScope.MinYear, StandardShortScope.MaxYear) { }
}

/// <summary>
/// Provides test data for <see cref="ArmenianCalendar"/> and related date types.
/// </summary>
public sealed class StandardArmenian12DataSet :
    StandardCalendarDataSet<Armenian12CalendarDataSet>, IEpagomenalDataSet, ISingleton<StandardArmenian12DataSet>
{
    private StandardArmenian12DataSet() : base(Armenian12CalendarDataSet.Instance) { }

    public static StandardArmenian12DataSet Instance { get; } = new();

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => Inner.EpagomenalDayInfoData.WhereT(DataFilter.Filter);
}

/// <summary>
/// Provides test data for <see cref="CopticCalendar"/> and related date types.
/// </summary>
public sealed class StandardCoptic12DataSet :
    StandardCalendarDataSet<Coptic12CalendarDataSet>, IEpagomenalDataSet, ISingleton<StandardCoptic12DataSet>
{
    private StandardCoptic12DataSet() : base(Coptic12CalendarDataSet.Instance) { }

    public static StandardCoptic12DataSet Instance { get; } = new();

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => Inner.EpagomenalDayInfoData.WhereT(DataFilter.Filter);
}

/// <summary>
/// Provides test data for <see cref="EthiopicCalendar"/> and related date types.
/// </summary>
public sealed class StandardEthiopic12DataSet :
    StandardCalendarDataSet<Ethiopic12CalendarDataSet>, IEpagomenalDataSet, ISingleton<StandardEthiopic12DataSet>
{
    private StandardEthiopic12DataSet() : base(Ethiopic12CalendarDataSet.Instance) { }

    public static StandardEthiopic12DataSet Instance { get; } = new();

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => Inner.EpagomenalDayInfoData.WhereT(DataFilter.Filter);
}

/// <summary>
/// Provides test data for <see cref="GregorianCalendar"/> and <see cref="CivilDate"/>.
/// </summary>
public sealed class StandardGregorianDataSet :
    StandardCalendarDataSet<GregorianCalendarDataSet>,
    IYearAdjustmentDataSet,
    IAdvancedMathDataSet,
    IDayOfWeekDataSet,
    ISingleton<StandardGregorianDataSet>
{
    private StandardGregorianDataSet() : base(GregorianCalendarDataSet.Instance) { }

    public static StandardGregorianDataSet Instance { get; } = new();

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
/// Provides test data for <see cref="TabularIslamicCalendar"/> and related date types.
/// </summary>
public sealed class StandardTabularIslamicDataSet :
    StandardCalendarDataSet<TabularIslamicCalendarDataSet>, ISingleton<StandardTabularIslamicDataSet>
{
    private StandardTabularIslamicDataSet() : base(TabularIslamicCalendarDataSet.Instance) { }

    public static StandardTabularIslamicDataSet Instance { get; } = new();
}

/// <summary>
/// Provides test data for <see cref="ZoroastrianCalendar"/> and related date types.
/// </summary>
public sealed class StandardZoroastrian12DataSet :
    StandardCalendarDataSet<Zoroastrian12CalendarDataSet>, IEpagomenalDataSet, ISingleton<StandardZoroastrian12DataSet>
{
    private StandardZoroastrian12DataSet() : base(Zoroastrian12CalendarDataSet.Instance) { }

    public static StandardZoroastrian12DataSet Instance { get; } = new();

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => Inner.EpagomenalDayInfoData.WhereT(DataFilter.Filter);
}
