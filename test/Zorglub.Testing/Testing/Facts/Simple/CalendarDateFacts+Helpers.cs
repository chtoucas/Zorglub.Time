// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

public abstract partial class CalendarDateHelpersFacts<TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarDateHelpersFacts(Calendar calendar)
    {
        CalendarUT = calendar ?? throw new ArgumentNullException(nameof(calendar));
    }

    protected Calendar CalendarUT { get; }
}

public partial class CalendarDateHelpersFacts<TDataSet>
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void AtStartOfYear(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var startOfYear = CalendarUT.GetCalendarDate(y, 1, 1);
        // Act & Assert
        Assert.Equal(startOfYear, CalendarDateHelpers.GetStartOfYear(year));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AtDayOfYear(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var year = CalendarUT.GetCalendarYear(y);
        var exp = CalendarUT.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(exp, CalendarDateHelpers.GetDayOfYear(year, doy));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void AtEndOfYear(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var endOfYear = CalendarUT.GetOrdinalDate(y, info.DaysInYear).ToCalendarDate();
        // Act & Assert
        Assert.Equal(endOfYear, CalendarDateHelpers.GetEndOfYear(year));
    }
}
