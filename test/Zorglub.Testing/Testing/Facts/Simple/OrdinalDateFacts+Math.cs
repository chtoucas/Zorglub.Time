// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

/// <summary>
/// Provides facts about <see cref="CalendarDate"/> and its non-standard mathematical operations.
/// </summary>
public abstract partial class OrdinalDateMathFacts<TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, IAdvancedMathDataSet, ISingleton<TDataSet>
{
    protected OrdinalDateMathFacts(Calendar calendar)
    {
        CalendarUT = calendar ?? throw new ArgumentNullException(nameof(calendar));
    }

    protected Calendar CalendarUT { get; }

    protected OrdinalDate GetDate(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return CalendarUT.GetCalendarDate(y, m, d).ToOrdinalDate();
    }

    public static DataGroup<YemodaPairAnd<int>> AddYearsData => DataSet.AddYearsData;
}

public partial class OrdinalDateMathFacts<TDataSet> // PlusYears()
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void PlusYears_Zero_IsNeutral(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = CalendarUT.GetOrdinalDate(y, doy);
        // Act & Assert
        Assert.Equal(date, date.PlusYears(0));
    }

    [Theory(Skip = "This cannot work with AddYearsData"), MemberData(nameof(AddYearsData))]
    public void PlusYears(YemodaPairAnd<int> info)
    {
        int years = info.Value;
        var date = GetDate(info.First);
        var other = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(other, date.PlusYears(years));
    }
}

public partial class OrdinalDateMathFacts<TDataSet> // CountYearsSince()
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void CountYearsSince_WhenSame_IsZero(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = CalendarUT.GetOrdinalDate(y, doy);
        // Act & Assert
        Assert.Equal(0, date.CountYearsSince(date));
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void CountYearsSince(YemodaPairAnd<int> info)
    {
        int years = info.Value;
        var date = GetDate(info.First);
        var other = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(years, other.CountYearsSince(date));
    }
}
