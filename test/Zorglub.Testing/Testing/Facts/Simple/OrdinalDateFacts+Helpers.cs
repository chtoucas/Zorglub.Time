// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

public abstract partial class OrdinalDateHelpersFacts<TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected OrdinalDateHelpersFacts(Calendar calendar)
    {
        CalendarUT = calendar ?? throw new ArgumentNullException(nameof(calendar));
    }

    protected Calendar CalendarUT { get; }
}

public partial class OrdinalDateHelpersFacts<TDataSet>
{
    [Theory, MemberData(nameof(MonthInfoData))]
    public void AtStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var startOfMonth = CalendarUT.GetCalendarDate(y, m, 1).ToOrdinalDate();
        // Act & Assert
        Assert.Equal(startOfMonth, OrdinalDateHelpers.GetStartOfMonth(month));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AtDayOfMonth(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var date = CalendarUT.GetOrdinalDate(y, doy);
        // Act & Assert
        Assert.Equal(date, OrdinalDateHelpers.GetDayOfMonth(month, d));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void AtEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var daysInMonth = info.DaysInMonth;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var endOfMonth = CalendarUT.GetCalendarDate(y, m, daysInMonth).ToOrdinalDate();
        // Act & Assert
        Assert.Equal(endOfMonth, OrdinalDateHelpers.GetEndOfMonth(month));
    }
}