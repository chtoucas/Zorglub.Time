// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extras;

using Zorglub.Testing.Data.Unbounded;
using Zorglub.Time.Simple;

public sealed partial class ZCalendarTests :
    ICalendarTFacts<ZDate, ZCalendar, UnboundedGregorianDataSet>
{
    public ZCalendarTests() : this(ZCalendar.Gregorian) { }

    private ZCalendarTests(ZCalendar calendar) : base(calendar) { }

    protected override ZDate GetDate(int y, int m, int d) => CalendarUT.GetDate(y, m, d);
    protected override ZDate GetDate(int y, int doy) => CalendarUT.GetDate(y, doy);
    protected override ZDate GetDate(DayNumber dayNumber) => CalendarUT.GetDate(dayNumber);
}

public partial class ZCalendarTests // Properties
{
    [Fact]
    public void Key() =>
        Assert.Equal(GregorianCalendar.Instance.Key, CalendarUT.Key);

    [Fact]
    public sealed override void Algorithm_Prop() =>
        Assert.Equal(CalendricalAlgorithm.Arithmetical, CalendarUT.Algorithm);

    [Fact]
    public sealed override void Family_Prop() =>
        Assert.Equal(CalendricalFamily.Solar, CalendarUT.Family);

    [Fact]
    public sealed override void PeriodicAdjustments_Prop() =>
        Assert.Equal(CalendricalAdjustments.Days, CalendarUT.PeriodicAdjustments);

    [Fact]
    public void ToString_ReturnsKey() =>
        Assert.Equal(CalendarUT.Key, CalendarUT.ToString());

    [Fact]
    public void Today()
    {
        var exp = DateTime.Now;
        // Act
        var today = CalendarUT.Today();
        // Assert
        Assert.Equal(exp.Year, today.Year);
        Assert.Equal(exp.Month, today.Month);
        Assert.Equal(exp.Day, today.Day);
    }
}

public partial class ZCalendarTests // Validation
{
    [Fact]
    public void ValidateYearMonthDay_InvalidYear()
    {
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.Scope.ValidateYearMonthDay(y, 1, 1));
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.Scope.ValidateYearMonthDay(y, 1, 1, "paramName"), "paramName");
    }

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void ValidateYearMonthDay_InvalidMonthOfYear(int y, int m)
    {
        Assert.ThrowsAoorexn("month",
            () => CalendarUT.Scope.ValidateYearMonthDay(y, m, 1));
        Assert.ThrowsAoorexn("paramName",
            () => CalendarUT.Scope.ValidateYearMonthDay(y, m, 1, "paramName"));
    }

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void ValidateYearMonthDay_InvalidDayOfMonth(int y, int m, int d)
    {
        Assert.ThrowsAoorexn("day",
            () => CalendarUT.Scope.ValidateYearMonthDay(y, m, d));
        Assert.ThrowsAoorexn("paramName",
            () => CalendarUT.Scope.ValidateYearMonthDay(y, m, d, "paramName"));
    }

    //[Theory, MemberData(nameof(InvalidMonthFieldData))]
    //public void ValidateMonthDay_InvalidMonthOfYear(int y, int m)
    //{
    //    Assert.ThrowsAoorexn("paramName",
    //        () => CalendarUT.PreValidator.ValidateMonthDay(y, m, 1, "paramName"));
    //}

    //[Theory, MemberData(nameof(InvalidDayFieldData))]
    //public void ValidateMonthDay_InvalidDayOfMonth(int y, int m, int d)
    //{
    //    Assert.ThrowsAoorexn("paramName",
    //        () => CalendarUT.PreValidator.ValidateMonthDay(y, m, d, "paramName"));
    //}
}

public partial class ZCalendarTests // Conversions
{
    [Fact]
    public void CreationFromOrdinalDate_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.GetDate(y, 1));

    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void CreationFromOrdinalDate_InvalidDayOfYear(int y, int doy) =>
        Assert.ThrowsAoorexn("dayOfYear", () => CalendarUT.GetDate(y, doy));

    [Theory, MemberData(nameof(DateInfoData))]
    public void CreationFromOrdinalDate(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Act
        var date = CalendarUT.GetDate(y, doy);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
        Assert.Equal(doy, date.DayOfYear);
    }
}
