// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

using Zorglub.Testing.Data.Bounded;

public sealed class GregorianDateRangeTests : DateRangeFacts<ProlepticGregorianDataSet>
{
    public GregorianDateRangeTests()
        : base(GregorianCalendar.Instance, JulianCalendar.Instance) { }

    [Fact]
    public void WithCalendar()
    {
        var date = CalendarUT.GetCalendarDate(1970, 1, 1);
        var range = DateRange.Create(date, 2);
        // Act
        var actual = range.WithCalendar(OtherCalendar);
        // Assert
        Assert.Equal(OtherCalendar.GetCalendarDate(1969, 12, 19), actual.Start);
        Assert.Equal(OtherCalendar.GetCalendarDate(1969, 12, 20), actual.End);
        Assert.Equal(OtherCalendar.Id, actual.Calendar.Id);
    }
}

public sealed class JulianDateRangeTests : DateRangeFacts<ProlepticJulianDataSet>
{
    public JulianDateRangeTests()
        : base(JulianCalendar.Instance, GregorianCalendar.Instance) { }

    [Fact]
    public void WithCalendar()
    {
        var date = CalendarUT.GetCalendarDate(1969, 12, 19);
        var range = DateRange.Create(date, 2);
        // Act
        var actual = range.WithCalendar(OtherCalendar);
        // Assert
        Assert.Equal(OtherCalendar.GetCalendarDate(1970, 1, 1), actual.Start);
        Assert.Equal(OtherCalendar.GetCalendarDate(1970, 1, 2), actual.End);
        Assert.Equal(OtherCalendar.Id, actual.Calendar.Id);
    }
}
